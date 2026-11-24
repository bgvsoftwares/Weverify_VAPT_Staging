using DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.ProvidedInfoViewModel;
using ViewModels.TempViewModel;
using System.Globalization;
using ViewModels.VerificationViewModel;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using ViewModels.ClientViewModel;
using System.Security.Cryptography;
using System.IO;

//using WebAppBGV.CommonMethods;

namespace BAL.ProvidedInfoRepository
{
    public class PQPersonalRepository : IPQPersonalRepository
    {
        DataContext db;
        ClientCustomize cc = new ClientCustomize();
        public PQPersonalRepository()
        {
            db = new DataContext();
        }


        public static string PEncrypt(string strText)
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
                string plainText = Convert.ToBase64String(ms.ToArray());
                var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
                return System.Convert.ToBase64String(plainTextBytes);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private const string SP_PARAM_RETURN_VALUE = "@ReturnValue";

        public int AddCandidate(AddPQPersonalViewModel model)
        {

            int intReturnValue = 0;
            SqlParameter objReturnValueParam = DAS.CreateParameter(SP_PARAM_RETURN_VALUE, SqlDbType.Int, null, ParameterDirection.ReturnValue, 0, false);

            DAS.ExecuteNonQuery(DAS.ConnectionString, CommandType.StoredProcedure, "InsertPQPersonals",
            DAS.CreateParameter("@ClientRowID", SqlDbType.Int, model.ClientRowID, ParameterDirection.Input, 20, false),
            DAS.CreateParameter("@Name", SqlDbType.VarChar, model.Name, ParameterDirection.Input, 200, false),
            DAS.CreateParameter("@MiddleName", SqlDbType.VarChar, model.MiddleName, ParameterDirection.Input, 200, false),
            DAS.CreateParameter("@LastName", SqlDbType.VarChar, model.LastName, ParameterDirection.Input, 200, false),
            DAS.CreateParameter("@CandidateFullName", SqlDbType.VarChar, model.CandidateName, ParameterDirection.Input, 200, false),
            DAS.CreateParameter("@EmailID", SqlDbType.VarChar, model.EmailID, ParameterDirection.Input, 200, false),
            DAS.CreateParameter("@MobileNo", SqlDbType.VarChar, model.MobileNo, ParameterDirection.Input, 200, false),
            DAS.CreateParameter("@ClientRefID", SqlDbType.VarChar, model.ClientRefID, ParameterDirection.Input, 200, false),
            DAS.CreateParameter("@CandTitle", SqlDbType.VarChar, model.CandTitle, ParameterDirection.Input, 20, false),
            DAS.CreateParameter("@CaseType", SqlDbType.VarChar, model.CaseType, ParameterDirection.Input, 30, false),
            DAS.CreateParameter("@FromType", SqlDbType.VarChar, model.FromType, ParameterDirection.Input, 30, false),
            DAS.CreateParameter("@ClientUserRowID", SqlDbType.Int, model.ClientUserRowID, ParameterDirection.Input, 20, false),
            DAS.CreateParameter("@CreatedBy", SqlDbType.Int, model.CreatedBy, ParameterDirection.Input, 20, false),
            DAS.CreateParameter("@PackageOrCheckDetails", SqlDbType.VarChar, model.PackageOrCheckDetails, ParameterDirection.Input, 5000, false),
            DAS.CreateParameter("@OtherDetails", SqlDbType.VarChar, model.OtherDetails, ParameterDirection.Input, 300, false),
            DAS.CreateParameter("@CreatedBySource", SqlDbType.VarChar, model.CreatedBySource, ParameterDirection.Input, 300, false),
            DAS.CreateParameter("@TCSClientType", SqlDbType.VarChar, model.TCSClientType, ParameterDirection.Input, 300, false),
            DAS.CreateParameter("@OfferType", SqlDbType.VarChar, model.OfferType, ParameterDirection.Input, 100, false),
            DAS.CreateParameter("@Entity", SqlDbType.NVarChar, model.Entity, ParameterDirection.Input, 200, false),


            objReturnValueParam
            );
            intReturnValue = (int)objReturnValueParam.Value;
            return intReturnValue;

        }

        public IEnumerable<ClientLatestCasesInitiatedToCandidate> LastFiveClientCasesInitiated(int clientRowID, int clientUserRowID)
        {
            DataTable ds = new DataTable();
            ds = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "getLastFiveClientCasesInitiated",
            DAS.CreateParameter("@ClientRowID", SqlDbType.SmallInt, clientRowID, ParameterDirection.Input, 300, false),
            DAS.CreateParameter("@clientUserRowID", SqlDbType.SmallInt, clientUserRowID, ParameterDirection.Input, 300, false)
          );
            foreach (DataRow row in ds.Rows)
            {
                yield return new ClientLatestCasesInitiatedToCandidate
                {
                    Name = Convert.ToString(row["CandidateName"]),
                    MobileNo = Convert.ToString(row["MobileNo"]),
                    EmailID = Convert.ToString(row["EmailID"]),
                    CreationDate = Convert.ToString(row["CreationDate"]),
                    PersonalRowId = Convert.ToString(row["PersonalRowID"])
                };
            }
        }

        public void AddCandidateAsBulkImport(AddPQPersonalBulkImportViewModel model)
        {
            try
            {
                if (model != null)
                {
                    PQPersonal entity = new PQPersonal();
                    entity.ClientRowID = model.ClientRowID;
                    entity.ClientUserRowID = model.ClientUserRowID;
                    entity.FatherName = model.FatherName;
                    entity.ClientRefID = model.ClientRefID;
                    entity.CompanyRefNo = model.CompanyRefNo;
                    entity.CandidateCode = model.CandidateCode;
                    entity.CandTitle = model.CandTitle;
                    entity.Name = model.Name;
                    entity.MiddleName = model.MiddleName;
                    entity.LastName = model.LastName;
                    entity.Gender = model.Gender;
                    entity.DOB = model.DOB;
                    entity.MaritalStatus = model.MaritalStatus;
                    entity.MobileNo = model.MobileNo;
                    entity.EmailID = model.EmailID;
                    entity.EmployeeID = model.EmployeeID;
                    entity.FromType = model.FromType;
                    entity.DetailsEnteredBy = model.DetailsEnteredBy;
                    entity.CreatedBy = model.CreatedBy;
                    entity.CreatedDate = model.CreatedDate;
                    entity.OtherDetails = model.OtherDetails;
                    entity.CaseStatus = model.CaseStatus;
                    entity.Status = model.Status;

                    entity.CaseType = model.CaseType;
                    entity.TAT = model.TAT;
                    entity.OrderDate = model.OrderDate;
                    entity.DeliveryDate = model.DeliveryDate;
                    entity.PanCard = string.IsNullOrEmpty(model.PanCardNo) ? Convert.ToByte(0) : Convert.ToByte(1);
                    entity.PanCardNo = model.PanCardNo;
                    entity.Passport = string.IsNullOrEmpty(model.PassportNo) ? Convert.ToByte(0) : Convert.ToByte(1); ;
                    entity.PassportNo = model.PassportNo;
                    entity.AadhaarCard = string.IsNullOrEmpty(model.AadhaarCardNo) ? Convert.ToByte(0) : Convert.ToByte(1); ;
                    entity.AadhaarCardNo = model.AadhaarCardNo;
                    entity.DrivingLicense = string.IsNullOrEmpty(model.DrivingLicenseNo) ? Convert.ToByte(0) : Convert.ToByte(1); ;
                    entity.DrivingLicenseNo = model.DrivingLicenseNo;
                    entity.VoterIDCard = string.IsNullOrEmpty(model.VoterIDCardNo) ? Convert.ToByte(0) : Convert.ToByte(1); ;
                    entity.VoterIDCardNo = model.VoterIDCardNo;
                    entity.DateofJoining = model.DateofJoining;
                    entity.PlaceofJoining = model.PlaceOfJoining;
                    entity.AlternateNo = model.AlternateNo;

                    if (entity.ClientRowID == 249 || entity.ClientRowID == 277)
                    {
                        entity.ProcessName = model.ProjectId;
                        entity.BusinessName = model.ProjectName;
                    }
                    if (entity.ClientRowID == 607)
                    {
                        entity.ProcessName = model.ProcessName;
                    }

                    db.PQPersonals.Add(entity);
                }
                else
                {
                    throw new Exception("Candidate could not be blank!");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int AddCandidate(AddPQPersonalBulkImportViewModel model)
        {

            int intReturnValue = 0;
            SqlParameter objReturnValueParam = DAS.CreateParameter(SP_PARAM_RETURN_VALUE, SqlDbType.Int, null, ParameterDirection.ReturnValue, 0, false);

            DAS.ExecuteNonQuery(DAS.ConnectionString, CommandType.StoredProcedure, "InsertPQPersonals_New",
            DAS.CreateParameter("@ClientRowID", SqlDbType.Int, model.ClientRowID, ParameterDirection.Input, 20, false),
            DAS.CreateParameter("@Name", SqlDbType.VarChar, model.Name, ParameterDirection.Input, 200, false),
            DAS.CreateParameter("@MiddleName", SqlDbType.VarChar, model.MiddleName, ParameterDirection.Input, 200, false),
            DAS.CreateParameter("@LastName", SqlDbType.VarChar, model.LastName, ParameterDirection.Input, 200, false),
            DAS.CreateParameter("@CandidateFullName", SqlDbType.VarChar, model.Name + " " + model.MiddleName + " " + model.LastName, ParameterDirection.Input, 200, false),
            DAS.CreateParameter("@CandidateType", SqlDbType.VarChar, model.CandidateType, ParameterDirection.Input, 200, false),
            DAS.CreateParameter("@EmailID", SqlDbType.VarChar, model.EmailID, ParameterDirection.Input, 200, false),
            DAS.CreateParameter("@MobileNo", SqlDbType.VarChar, model.MobileNo, ParameterDirection.Input, 200, false),
            DAS.CreateParameter("@ClientRefID", SqlDbType.VarChar, model.ClientRefID, ParameterDirection.Input, 200, false),
            DAS.CreateParameter("@CandTitle", SqlDbType.VarChar, model.CandTitle, ParameterDirection.Input, 20, false),
            DAS.CreateParameter("@CaseType", SqlDbType.VarChar, model.CaseType, ParameterDirection.Input, 30, false),
            DAS.CreateParameter("@FromType", SqlDbType.VarChar, model.FromType, ParameterDirection.Input, 30, false),
            DAS.CreateParameter("@ClientUserRowID", SqlDbType.Int, model.ClientUserRowID, ParameterDirection.Input, 20, false),
            DAS.CreateParameter("@CreatedBy", SqlDbType.Int, model.CreatedBy, ParameterDirection.Input, 20, false),
            DAS.CreateParameter("@PackageOrCheckDetails", SqlDbType.VarChar, model.CandidateCheckOrPackage, ParameterDirection.Input, 300, false),
            DAS.CreateParameter("@OtherDetails", SqlDbType.VarChar, model.OtherDetails, ParameterDirection.Input, 300, false),
                DAS.CreateParameter("@EmployeeID", SqlDbType.VarChar, model.EmployeeID, ParameterDirection.Input, 300, false),
                        DAS.CreateParameter("@FatherName", SqlDbType.VarChar, model.FatherName, ParameterDirection.Input, 300, false),
                        DAS.CreateParameter("@Gender", SqlDbType.VarChar, model.Gender, ParameterDirection.Input, 300, false),
                           DAS.CreateParameter("@DOB", SqlDbType.DateTime, model.DOB, ParameterDirection.Input, 300, false),
                                     DAS.CreateParameter("@CreatedBySource", SqlDbType.VarChar, model.CreatedBySource, ParameterDirection.Input, 300, false),

                                     DAS.CreateParameter("@VoterIDCardNo", SqlDbType.VarChar, model.VoterIDCardNo, ParameterDirection.Input, 30, false),
            DAS.CreateParameter("@DrivingLicenseNo", SqlDbType.VarChar, model.DrivingLicenseNo, ParameterDirection.Input, 30, false),
            DAS.CreateParameter("@AadhaarCardNo", SqlDbType.VarChar, model.AadhaarCardNo, ParameterDirection.Input, 30, false),
            DAS.CreateParameter("@PassportNo", SqlDbType.VarChar, model.PassportNo, ParameterDirection.Input, 30, false),
            DAS.CreateParameter("@PanCardNo", SqlDbType.VarChar, model.PanCardNo, ParameterDirection.Input, 30, false),
            DAS.CreateParameter("@AlternateEmailID", SqlDbType.VarChar, model.AlternateEmailID, ParameterDirection.Input, 100, false),
            DAS.CreateParameter("@DateofJoining", SqlDbType.DateTime, model.DateofJoining, ParameterDirection.Input, 30, false),
            DAS.CreateParameter("@PlaceOfJoining", SqlDbType.VarChar, model.PlaceOfJoining, ParameterDirection.Input, 30, false),
            DAS.CreateParameter("@AlternateNo", SqlDbType.VarChar, model.AlternateNo, ParameterDirection.Input, 30, false),
            DAS.CreateParameter("@ProcessName", SqlDbType.VarChar, model.ProcessName, ParameterDirection.Input, 100, false),
            DAS.CreateParameter("@ProjectId", SqlDbType.VarChar, model.ProjectId, ParameterDirection.Input, 30, false),
            DAS.CreateParameter("@ProjectName", SqlDbType.VarChar, model.ProjectName, ParameterDirection.Input, 100, false),
            DAS.CreateParameter("@Entity", SqlDbType.VarChar, model.Entity, ParameterDirection.Input, 100, false),
              DAS.CreateParameter("@Location", SqlDbType.VarChar, model.Location, ParameterDirection.Input, 100, false),
                  DAS.CreateParameter("@Grade", SqlDbType.VarChar, model.Grade, ParameterDirection.Input, 100, false),
                  DAS.CreateParameter("@Roles", SqlDbType.VarChar, model.Roles, ParameterDirection.Input, 100, false),
                  DAS.CreateParameter("@PID", SqlDbType.VarChar, model.PID, ParameterDirection.Input, 100, false),
                  DAS.CreateParameter("@SDO_Code", SqlDbType.VarChar, model.SDO_Code, ParameterDirection.Input, 100, false),
                  DAS.CreateParameter("@COE_Code", SqlDbType.VarChar, model.COE_Code, ParameterDirection.Input, 100, false),
                  DAS.CreateParameter("@UANNumber", SqlDbType.VarChar, model.UAN, ParameterDirection.Input, 100, false),
                  DAS.CreateParameter("@UID", SqlDbType.VarChar, model.UID, ParameterDirection.Input, 100, false),
                  DAS.CreateParameter("@OHRID", SqlDbType.VarChar, model.OHR_Taleo_ID, ParameterDirection.Input, 100, false),
                  DAS.CreateParameter("@Band", SqlDbType.VarChar, model.Band, ParameterDirection.Input, 100, false),
                  DAS.CreateParameter("@Designation", SqlDbType.VarChar, model.Designation_Genpact, ParameterDirection.Input, 100, false),
                  DAS.CreateParameter("@Region", SqlDbType.VarChar, model.Region, ParameterDirection.Input, 100, false),
                  DAS.CreateParameter("@State", SqlDbType.VarChar, model.State, ParameterDirection.Input, 100, false),
                  DAS.CreateParameter("@HubLocation", SqlDbType.VarChar, model.HubLocation, ParameterDirection.Input, 100, false),
            objReturnValueParam
            );
            intReturnValue = (int)objReturnValueParam.Value;
            return intReturnValue;

        }

        public int AddCandidateNew(AddPQPersonalBulkImportViewModel model)
        {

            int intReturnValue = 0;
            SqlParameter objReturnValueParam = DAS.CreateParameter(SP_PARAM_RETURN_VALUE, SqlDbType.Int, null, ParameterDirection.ReturnValue, 0, false);

            DAS.ExecuteNonQuery(DAS.ConnectionString, CommandType.StoredProcedure, "InsertPQPersonals_New1",
            DAS.CreateParameter("@ClientRowID", SqlDbType.Int, model.ClientRowID, ParameterDirection.Input, 20, false),
            DAS.CreateParameter("@Name", SqlDbType.VarChar, model.Name, ParameterDirection.Input, 200, false),
            DAS.CreateParameter("@MiddleName", SqlDbType.VarChar, model.MiddleName, ParameterDirection.Input, 200, false),
            DAS.CreateParameter("@LastName", SqlDbType.VarChar, model.LastName, ParameterDirection.Input, 200, false),
            DAS.CreateParameter("@CandidateFullName", SqlDbType.VarChar, model.Name + " " + model.MiddleName + " " + model.LastName, ParameterDirection.Input, 200, false),
            DAS.CreateParameter("@CandidateType", SqlDbType.VarChar, model.CandidateType, ParameterDirection.Input, 200, false),
            DAS.CreateParameter("@EmailID", SqlDbType.VarChar, model.EmailID, ParameterDirection.Input, 200, false),
            DAS.CreateParameter("@MobileNo", SqlDbType.VarChar, model.MobileNo, ParameterDirection.Input, 200, false),
            DAS.CreateParameter("@ClientRefID", SqlDbType.VarChar, model.ClientRefID, ParameterDirection.Input, 200, false),
            DAS.CreateParameter("@CandTitle", SqlDbType.VarChar, model.CandTitle, ParameterDirection.Input, 20, false),
            DAS.CreateParameter("@CaseType", SqlDbType.VarChar, model.CaseType, ParameterDirection.Input, 30, false),
            DAS.CreateParameter("@FromType", SqlDbType.VarChar, model.FromType, ParameterDirection.Input, 30, false),
            DAS.CreateParameter("@ClientUserRowID", SqlDbType.Int, model.ClientUserRowID, ParameterDirection.Input, 20, false),
            DAS.CreateParameter("@CreatedBy", SqlDbType.Int, model.CreatedBy, ParameterDirection.Input, 20, false),
            DAS.CreateParameter("@PackageOrCheckDetails", SqlDbType.VarChar, model.CandidateCheckOrPackage, ParameterDirection.Input, 300, false),
            DAS.CreateParameter("@OtherDetails", SqlDbType.VarChar, model.OtherDetails, ParameterDirection.Input, 300, false),
                DAS.CreateParameter("@EmployeeID", SqlDbType.VarChar, model.EmployeeID, ParameterDirection.Input, 300, false),
                        DAS.CreateParameter("@FatherName", SqlDbType.VarChar, model.FatherName, ParameterDirection.Input, 300, false),
                        DAS.CreateParameter("@Gender", SqlDbType.VarChar, model.Gender, ParameterDirection.Input, 300, false),
                           DAS.CreateParameter("@DOB", SqlDbType.DateTime, model.DOB, ParameterDirection.Input, 300, false),
                                     DAS.CreateParameter("@CreatedBySource", SqlDbType.VarChar, model.CreatedBySource, ParameterDirection.Input, 300, false),

                                     DAS.CreateParameter("@VoterIDCardNo", SqlDbType.VarChar, model.VoterIDCardNo, ParameterDirection.Input, 30, false),
            DAS.CreateParameter("@DrivingLicenseNo", SqlDbType.VarChar, model.DrivingLicenseNo, ParameterDirection.Input, 30, false),
            DAS.CreateParameter("@AadhaarCardNo", SqlDbType.VarChar, model.AadhaarCardNo, ParameterDirection.Input, 30, false),
            DAS.CreateParameter("@PassportNo", SqlDbType.VarChar, model.PassportNo, ParameterDirection.Input, 30, false),
            DAS.CreateParameter("@PanCardNo", SqlDbType.VarChar, model.PanCardNo, ParameterDirection.Input, 30, false),
            DAS.CreateParameter("@AlternateEmailID", SqlDbType.VarChar, model.AlternateEmailID, ParameterDirection.Input, 100, false),
            DAS.CreateParameter("@DateofJoining", SqlDbType.DateTime, model.DateofJoining, ParameterDirection.Input, 30, false),
            DAS.CreateParameter("@PlaceOfJoining", SqlDbType.VarChar, model.PlaceOfJoining, ParameterDirection.Input, 30, false),
            DAS.CreateParameter("@AlternateNo", SqlDbType.VarChar, model.AlternateNo, ParameterDirection.Input, 30, false),
            DAS.CreateParameter("@ProcessName", SqlDbType.VarChar, model.ProcessName, ParameterDirection.Input, 100, false),
            DAS.CreateParameter("@ProjectId", SqlDbType.VarChar, model.ProjectId, ParameterDirection.Input, 30, false),
            DAS.CreateParameter("@ProjectName", SqlDbType.VarChar, model.ProjectName, ParameterDirection.Input, 100, false),
            DAS.CreateParameter("@Entity", SqlDbType.VarChar, model.Entity, ParameterDirection.Input, 100, false),
            DAS.CreateParameter("@AccountName", SqlDbType.VarChar, model.AccountName, ParameterDirection.Input, 1000, false),
            DAS.CreateParameter("@PatternName", SqlDbType.VarChar, model.PatternName, ParameterDirection.Input, 1000, false),
              DAS.CreateParameter("@Location", SqlDbType.VarChar, model.Location, ParameterDirection.Input, 100, false),
                  DAS.CreateParameter("@Grade", SqlDbType.VarChar, model.Grade, ParameterDirection.Input, 100, false),
                  DAS.CreateParameter("@Roles", SqlDbType.VarChar, model.Roles, ParameterDirection.Input, 100, false),
                  DAS.CreateParameter("@PID", SqlDbType.VarChar, model.PID, ParameterDirection.Input, 100, false),
                  DAS.CreateParameter("@SDO_Code", SqlDbType.VarChar, model.SDO_Code, ParameterDirection.Input, 100, false),
                  DAS.CreateParameter("@COE_Code", SqlDbType.VarChar, model.COE_Code, ParameterDirection.Input, 100, false),
                  DAS.CreateParameter("@UANNumber", SqlDbType.VarChar, model.UAN, ParameterDirection.Input, 100, false),
                  DAS.CreateParameter("@UID", SqlDbType.VarChar, model.UID, ParameterDirection.Input, 100, false),
                  DAS.CreateParameter("@OHRID", SqlDbType.VarChar, model.OHR_Taleo_ID, ParameterDirection.Input, 100, false),
                  DAS.CreateParameter("@Band", SqlDbType.VarChar, model.Band, ParameterDirection.Input, 100, false),
                  DAS.CreateParameter("@Designation", SqlDbType.VarChar, model.Designation_Genpact, ParameterDirection.Input, 100, false),
                   DAS.CreateParameter("@Education", SqlDbType.VarChar, model.Education, ParameterDirection.Input, 500, false),
                    DAS.CreateParameter("@Employment", SqlDbType.VarChar, model.Employment, ParameterDirection.Input, 500, false),
                     DAS.CreateParameter("@Address", SqlDbType.VarChar, model.Address, ParameterDirection.Input, 500, false),
                      DAS.CreateParameter("@Criminal", SqlDbType.VarChar, model.Criminal, ParameterDirection.Input, 500, false),
                       DAS.CreateParameter("@Identity", SqlDbType.VarChar, model.Identity, ParameterDirection.Input, 500, false),
                        DAS.CreateParameter("@Database", SqlDbType.VarChar, model.Database, ParameterDirection.Input, 500, false),
                         DAS.CreateParameter("@Additional_Professional_Certifications", SqlDbType.VarChar, model.Additional_Professional_Certifications, ParameterDirection.Input, 500, false),
                          DAS.CreateParameter("@Drug_Test_Verification", SqlDbType.VarChar, model.Drug_Test_Verification, ParameterDirection.Input, 500, false),
                           DAS.CreateParameter("@Credit_History_Verification", SqlDbType.VarChar, model.Credit_History_Verification, ParameterDirection.Input, 500, false),
                            DAS.CreateParameter("@Family_Relationship", SqlDbType.VarChar, model.Family_Relationship, ParameterDirection.Input, 500, false),
                             DAS.CreateParameter("@External_Directorship", SqlDbType.VarChar, model.External_Directorship, ParameterDirection.Input, 500, false),
                              DAS.CreateParameter("@Self_Employment_Directorship", SqlDbType.VarChar, model.Self_Employment_Directorship, ParameterDirection.Input, 500, false),
                               DAS.CreateParameter("@Facis_III", SqlDbType.VarChar, model.Facis_III, ParameterDirection.Input, 500, false),
                                DAS.CreateParameter("@Entitlement_to_Work", SqlDbType.VarChar, model.Entitlement_to_Work, ParameterDirection.Input, 500, false),
                                 DAS.CreateParameter("@Professional_Reference_Verification", SqlDbType.VarChar, model.Professional_Reference_Verification, ParameterDirection.Input, 500, false),
                                  DAS.CreateParameter("@Professional_License_Check", SqlDbType.VarChar, model.Professional_License_Check, ParameterDirection.Input, 500, false),

            objReturnValueParam
            );
            intReturnValue = (int)objReturnValueParam.Value;
            return intReturnValue;

        }

        public IEnumerable<PQPersonalViewModel> GetCandidates()
        {
            try
            {
                return db.PQPersonals.Include("PQClientMaster").Select(item => new PQPersonalViewModel
                {
                    PersonalRowID = item.PersonalRowID,
                    ClientRowID = item.ClientRowID,
                    ClientName = item.PQClientMaster.MasterAbbreviation.ClientName + " " + item.PQClientMaster.MasterClientSubGroup.ClientSubGroupName,
                    CandidateCode = item.CandidateCode,
                    CompanyRefNo = item.CompanyRefNo,
                    ClientRefID = item.ClientRefID,
                    CandTitle = item.CandTitle,
                    Name = item.CandTitle + " " + item.Name + " " + item.MiddleName + " " + item.LastName,
                    MiddleName = item.MiddleName,
                    LastName = item.LastName,
                    FatherTitle = item.FatherTitle,
                    FatherName = item.FatherName,
                    FatherMiddleName = item.FatherMiddleName,
                    FatherLastName = item.FatherLastName,
                    Gender = item.Gender,
                    MaritalStatus = item.MaritalStatus,
                    MobileNo = item.MobileNo,
                    EmailID = item.EmailID,
                    AadhaarCard = item.AadhaarCard,
                    AadhaarCardNo = item.AadhaarCardNo,
                    PanCard = item.PanCard,
                    PanCardNo = item.PanCardNo,
                    Passport = item.Passport,
                    PassportNo = item.PassportNo,
                    DrivingLicense = item.DrivingLicense,
                    DrivingLicenseNo = item.DrivingLicenseNo,
                    VoterIDCard = item.VoterIDCard,
                    VoterIDCardNo = item.VoterIDCardNo,
                    CreatedBy = item.CreatedBy,
                    CreatedDate = item.CreatedDate,
                    Status = item.Status,
                }).ToList();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public PQPersonalInfoListPagedModel GetCandidateListForAllocation(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string RecievingfromDate = "", string RecievingToDate = "")
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }
                IQueryable<PQPersonal> data = db.PQPersonals.Include("PQClientMaster").Where(p => p.AllocatedToDE == 0 && p.AllocatedToDEQC == 0 && p.AllocatedToDEScan == 0);
                if (teamMemerRowId > 0)
                {
                    data = data.Where(p => p.CreatedBy == teamMemerRowId);
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    data = data.Where(b => (b.CandTitle + " " + b.Name + " " + b.MiddleName + " " + b.LastName).ToString().Replace(" ", "").Contains(Search.Replace(" ", "")) || b.CompanyRefNo.ToLower().Trim() == Search.ToLower().Trim());
                }
                if (!string.IsNullOrEmpty(RecievingToDate) && !string.IsNullOrEmpty(RecievingfromDate))
                {
                    DateTime startDate = Convert.ToDateTime(RecievingfromDate);
                    DateTime EndDate = Convert.ToDateTime(RecievingToDate);

                    data = data.Where(b => DbFunctions.TruncateTime(b.OrderDate) >= startDate.Date && DbFunctions.TruncateTime(b.OrderDate) <= EndDate.Date);
                }
                if (ClientRowID > 0)
                {
                    data = data.Where(b => b.ClientRowID == ClientRowID);
                }

                switch (sort)
                {
                    default:
                        data = sortDir == "asc" ? data.OrderByDescending(d => d.PersonalRowID) : data.OrderBy(d => d.PersonalRowID);
                        break;
                }

                PQPersonalInfoListPagedModel model = new PQPersonalInfoListPagedModel();
                model.PageSize = pageSize;
                model.TotalRecords = data.Count();
                model.PQPersonalInfo = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new PQPersonalDetailViewModel
                {
                    PersonalRowID = item.PersonalRowID,
                    ClientRowID = item.ClientRowID,
                    ClientName = item.PQClientMaster.MasterAbbreviation.ClientName + " " + item.PQClientMaster.MasterClientSubGroup.ClientSubGroupName,
                    CandidateCode = item.CandidateCode,
                    CompanyRefNo = item.CompanyRefNo,
                    ClientRefID = item.ClientRefID,
                    CandTitle = item.CandTitle,
                    Name = item.CandTitle + " " + item.Name + " " + item.MiddleName + " " + item.LastName,
                    MiddleName = item.MiddleName,
                    LastName = item.LastName,
                    CreatedBy = item.CreatedBy,
                    CreatedDate = item.CreatedDate,
                    OtherDetails1 = item.OtherDetails1,
                    Status = item.Status,
                }).ToList();
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PQPersonalInfoListPagedModel GetCandidateListForAllocation1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string CaseStatus = "", string rolename = "", short teamMemerRowId = 0, short ClientRowID = 0, string RecievingfromDate = "", string RecievingToDate = "",string ModuleType="")
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }
                DataSet ds = new DataSet();               
                string ClientId = "";
                if (ClientRowID == 0)
                {
                    ClientId = null;
                }
                else
                {
                    ClientId = ClientRowID.ToString();
                }
                if (!string.IsNullOrEmpty(Search) || ClientRowID > 0)
                {                   
                    ds = cc.getCasewiseSummaryDashboardInternalDetail(teamMemerRowId, rolename, CaseStatus, Search, ClientId);
                }
                else if(ModuleType!="")
                {
                   
                    ds = cc.getCasewiseSummaryDashboardInternalDetailWithModuleType(teamMemerRowId, rolename, CaseStatus, Search, ModuleType, ClientId);
                }
                else
                {
                    ds = cc.getCasewiseSummaryDashboardInternalDetail(teamMemerRowId, rolename, CaseStatus);
                }
              
                List<PQPersonalDetailViewModel> lmd = new List<PQPersonalDetailViewModel>();
                foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>
                {
                    lmd.Add(new PQPersonalDetailViewModel
                    {
                        PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]),
                        ClientRowID = Convert.ToInt16(dr["ClientRowID"]),
                        Name = (dr["Candidate Name"]).ToString(),
                        CandidateCode = (dr["CandidateCode"]).ToString(),
                        CompanyRefNo = (dr["Securitas ID"]).ToString(),
                        CaseStatus = (dr["Case Status"]).ToString(),
                        CreatedDate = Convert.ToDateTime(dr["Created On"]),
                        ClientName = (dr["ClientName"]).ToString(),
                        Status = byte.Parse(dr["Status"].ToString()),
                        OtherDetails1 = (dr["OtherDetails1"].ToString()),
                        ModuleType = (dr["ModuleType"].ToString()),
                        IsSeniorCase = dr.Table.Columns.Contains("Seniorprofile") ? dr["Seniorprofile"] != DBNull.Value ? dr["Seniorprofile"].ToString() : "0" : "0"

                    });

                }
                PQPersonalInfoListPagedModel model = new PQPersonalInfoListPagedModel();
                model.PageSize = pageSize;
                model.TotalRecords = ds.Tables[0].Rows.Count;
                model.PQPersonalInfo = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;             
            }
            catch (Exception)
            {
                throw;
            }
        }


        //following method added on 20-Feb-2017 by Arvind
        public PQPersonalInfoListPagedModel GetAllocatedCandidateListForDETL(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, short AllocateDEID = 0, short AllocateQCID = 0, short AllocateDEScan = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string SearchType = "", string CaseStatus = "")
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }
                IQueryable<PQPersonal> data = db.PQPersonals.Include("PQClientMaster").Where(p => p.AllocatedToDE > 0 && p.AllocatedToDEQC > 0 && p.AllocatedToDEScan > 0 && (p.CaseStausID == 3 || p.CaseStausID == 4 || p.CaseStausID == 5 || p.CaseStausID == 6 || p.CaseStausID == 7 || p.CaseStausID == 8 || p.CaseStausID == 9 || p.CaseStausID == 10 || p.CaseStausID == 11 || p.CaseStausID == 12 || p.CaseStausID == 105));
                if (teamMemerRowId > 0)
                {
                    data = data.Where(p => p.AllocatedBy == teamMemerRowId);
                }

                if (ClientRowID > 0)
                {
                    data = data.Where(b => b.ClientRowID == ClientRowID);
                }

                if (!string.IsNullOrEmpty(RecievingToDate) && !string.IsNullOrEmpty(RecievingFromDate))
                {
                    DateTime startDate = Convert.ToDateTime(RecievingFromDate);
                    DateTime EndDate = Convert.ToDateTime(RecievingToDate);
                    data = data.Where(b => DbFunctions.TruncateTime(b.OrderDate) >= startDate.Date && DbFunctions.TruncateTime(b.OrderDate) <= EndDate.Date);
                }

                if (!string.IsNullOrEmpty(CompletedToDate) && !string.IsNullOrEmpty(CompletedFromDate))
                {
                    DateTime startDate = Convert.ToDateTime(CompletedFromDate);
                    DateTime EndDate = Convert.ToDateTime(CompletedToDate);
                    data = data.Where(b => DbFunctions.TruncateTime(b.DeliveryDate) >= startDate.Date && DbFunctions.TruncateTime(b.DeliveryDate) <= EndDate.Date);
                }
                if (AllocateDEID > 0 || AllocateDEScan > 0 || AllocateQCID > 0)
                {
                    data = data.Where(b => b.AllocatedToDE == AllocateDEID || b.AllocatedToDEScan == AllocateDEScan || b.AllocatedToDEQC == AllocateQCID);
                }
                if (!string.IsNullOrEmpty(Search))
                {

                    if (SearchType == "Keyword")
                    {
                        data = data.Where(b => (b.CandTitle + " " + b.Name + " " + b.MiddleName + " " + b.LastName).ToString().Replace(" ", "").Contains(Search.Replace(" ", "")) || b.CompanyRefNo.ToLower().Trim() == Search.ToLower().Trim());
                    }
                    else
                    {
                        var firstObj = db.TeamMembers.FirstOrDefault(b => (b.TMTitle + " " + b.TMFirstName + " " + b.TMLastName).ToString().Replace(" ", "").Contains(Search.Replace(" ", "")));
                        if (firstObj != null)
                        {
                            data = data.Where(b => b.AllocatedToDE == firstObj.TeamMemberRowID || b.AllocatedToDEScan == firstObj.TeamMemberRowID || b.AllocatedToDEQC == firstObj.TeamMemberRowID);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(CaseStatus))
                {
                    data = data.Where(b => b.CaseStatus == CaseStatus);
                }
                switch (sort)
                {
                    default:
                        data = sortDir == "asc" ? data.OrderByDescending(d => d.PersonalRowID) : data.OrderBy(d => d.PersonalRowID);
                        break;
                }

                PQPersonalInfoListPagedModel model = new PQPersonalInfoListPagedModel();
                model.PageSize = pageSize;
                model.TotalRecords = data.Count();
                model.PQPersonalInfo = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new PQPersonalDetailViewModel
                {
                    PersonalRowID = item.PersonalRowID,
                    ClientRowID = item.ClientRowID,
                    ClientName = item.PQClientMaster.MasterAbbreviation.ClientName + " " + item.PQClientMaster.MasterClientSubGroup.ClientSubGroupName,
                    CandidateCode = item.CandidateCode,
                    CompanyRefNo = item.CompanyRefNo,
                    ClientRefID = item.ClientRefID,
                    CandTitle = item.CandTitle,
                    Name = item.CandTitle + " " + item.Name + " " + item.MiddleName + " " + item.LastName,
                    MiddleName = item.MiddleName,
                    LastName = item.LastName,
                    AllocatedBy = item.AllocatedBy,
                    AllocatedToDE = item.AllocatedToDE,
                    AllocatedToDEName = item.AllocatedToDE > 0 ? db.TeamMembers.Where(t => t.TeamMemberRowID == item.AllocatedToDE).Select(t => t.TMTitle + " " + t.TMFirstName + " " + t.TMLastName).FirstOrDefault() : null,
                    AllocatedToDEDate = item.AllocatedToDEDate,
                    AllocatedToDEQC = item.AllocatedToDEQC,
                    AllocatedToDEQCName = item.AllocatedToDEQC > 0 ? db.TeamMembers.Where(t => t.TeamMemberRowID == item.AllocatedToDEQC).Select(t => t.TMTitle + " " + t.TMFirstName + " " + t.TMLastName).FirstOrDefault() : null,
                    AllocatedToDEQCDate = item.AllocatedToDEQCDate,
                    AllocatedToDEScanName = item.AllocatedToDEScan > 0 ? db.TeamMembers.Where(t => t.TeamMemberRowID == item.AllocatedToDEScan).Select(t => t.TMTitle + " " + t.TMFirstName + " " + t.TMLastName).FirstOrDefault() : null,
                    AllocatedToDEScan = item.AllocatedToDEScan,
                    AllocatedToDEScanDate = item.AllocatedToDEScanDate,
                    CreatedBy = item.CreatedBy,
                    CreatedDate = item.CreatedDate,
                    CaseStatus = item.CaseStatus,
                    FineStatus = item.FinalStatus,
                    Status = item.Status,
                    CaseStatusId = item.CaseStausID.ToString(),
                }).ToList();
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PQPersonalInfoListPagedModel GetAllocatedCandidateListForDETL1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string rolename = "", short teamMemerRowId = 0, short ClientRowID = 0, short AllocateDEID = 0, short AllocateQCID = 0, short AllocateDEScan = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string SearchType = "", string CaseStatus = "",string ModuleType="")
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }
                string ClientId = "";
                if (ClientRowID == 0)
                {
                    ClientId = null;
                }
                else
                {
                    ClientId = ClientRowID.ToString();
                }

                DataSet ds = new DataSet();
                if (!string.IsNullOrEmpty(Search) || ClientRowID > 0)
                {
                   
                    ds = cc.getCasewiseSummaryDashboardInternalDetail(teamMemerRowId, rolename, CaseStatus, Search, ClientId);
                }
                else if (ModuleType != "")
                {

                    ds = cc.getCasewiseSummaryDashboardInternalDetailWithModuleType(teamMemerRowId, rolename, CaseStatus, Search, ModuleType, ClientId);
                }
                else
                {

                    ds = cc.getCasewiseSummaryDashboardInternalDetail(teamMemerRowId, rolename, CaseStatus);
                }
                // ds = cc.getCasewiseSummaryDetail(ClientRowID, CaseStatus,ClientUserRowID);
                List<PQPersonalDetailViewModel> lmd = new List<PQPersonalDetailViewModel>();
                PQPersonalInfoListPagedModel model = new PQPersonalInfoListPagedModel();
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        PQPersonalDetailViewModel mm = new PQPersonalDetailViewModel();
                        //lmd.Add(new PQPersonalDetailViewModel
                        //{
                        mm.PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]);
                        mm.ClientRowID = Convert.ToInt16(dr["ClientRowID"]);
                        mm.Name = (dr["Candidate Name"]).ToString();
                        mm.CandidateCode = (dr["CandidateCode"]).ToString();
                        mm.CompanyRefNo = (dr["Securitas ID"]).ToString();
                        mm.CaseStatus = (dr["Case Status"]).ToString();
                        //mm.CreatedDate = Convert.ToDateTime(dr["Created On"]);
                        if (!string.IsNullOrEmpty(dr["Created On"].ToString()))
                        {
                            mm.CreatedDate = Convert.ToDateTime(dr["Created On"].ToString());
                        }
                        mm.ClientName = (dr["ClientName"]).ToString();
                        if (!string.IsNullOrEmpty(dr["AllocatedBy"].ToString()))
                        {
                            mm.AllocatedBy = Convert.ToInt16(dr["AllocatedBy"].ToString());
                        }
                        if (!string.IsNullOrEmpty(dr["AllocatedToDE"].ToString()))
                        {
                            mm.AllocatedToDE = Convert.ToInt16(dr["AllocatedToDE"].ToString());
                        }
                        mm.AllocatedToDEName = (dr["DEName"]).ToString();
                        //mm.AllocatedToDEDate = Convert.ToDateTime(dr["AllocatedToDEDate"].ToString());
                        if (!string.IsNullOrEmpty(dr["AllocatedToDEDate"].ToString()))
                        {
                            mm.AllocatedToDEDate = Convert.ToDateTime(dr["AllocatedToDEDate"].ToString());
                        }
                        if (!string.IsNullOrEmpty(dr["AllocatedToDEQC"].ToString()))
                        {
                            mm.AllocatedToDEQC = Convert.ToInt16(dr["AllocatedToDEQC"].ToString());
                        }
                        mm.AllocatedToDEQCName = (dr["DEQCName"]).ToString();

                        //mm.AllocatedToDEQCDate = Convert.ToDateTime(dr["AllocatedToDEQCDate"].ToString());
                        if (!string.IsNullOrEmpty(dr["AllocatedToDEQCDate"].ToString()))
                        {
                            mm.AllocatedToDEQCDate = Convert.ToDateTime(dr["AllocatedToDEQCDate"].ToString());
                        }
                        mm.AllocatedToDEScanName = (dr["scannerName"]).ToString();
                        if (!string.IsNullOrEmpty(dr["AllocatedToDEScan"].ToString()))
                        {
                            mm.AllocatedToDEScan = Convert.ToInt16(dr["AllocatedToDEScan"].ToString());
                        }
                        if (!string.IsNullOrEmpty(dr["AllocatedToDEScanDate"].ToString()))
                        {
                            mm.AllocatedToDEScanDate = Convert.ToDateTime(dr["AllocatedToDEScanDate"].ToString());
                        }
                        if (!string.IsNullOrEmpty(dr["CreatedBy"].ToString()))
                        {
                            mm.CreatedBy = Convert.ToInt16(dr["CreatedBy"].ToString());
                        }
                        mm.FineStatus = (dr["FinalStatus"]).ToString();
                        mm.Status = byte.Parse(dr["Status"].ToString());
                        mm.ModuleType = Convert.ToString(dr["ModuleType"]);
                        //});
                        lmd.Add(mm);

                    }
                    model.TotalRecords = ds.Tables[0].Rows.Count;
                }

                model.PageSize = pageSize;

                model.PQPersonalInfo = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;

                //PQPersonalInfoListPagedModel model = new PQPersonalInfoListPagedModel();
                //model.PageSize = pageSize;
                //model.TotalRecords = data.Count();
                //model.PQPersonalInfo = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new PQPersonalDetailViewModel
                //{
                //    PersonalRowID = item.PersonalRowID,
                //    ClientRowID = item.ClientRowID,
                //    ClientName = item.PQClientMaster.MasterAbbreviation.ClientName + " " + item.PQClientMaster.MasterClientSubGroup.ClientSubGroupName,
                //    CandidateCode = item.CandidateCode,
                //    CompanyRefNo = item.CompanyRefNo,
                //    ClientRefID = item.ClientRefID,
                //    CandTitle = item.CandTitle,
                //    Name = item.CandTitle + " " + item.Name + " " + item.MiddleName + " " + item.LastName,
                //    MiddleName = item.MiddleName,
                //    LastName = item.LastName,
                //    AllocatedBy = item.AllocatedBy,
                //    AllocatedToDE = item.AllocatedToDE,
                //    AllocatedToDEName = item.AllocatedToDE > 0 ? db.TeamMembers.Where(t => t.TeamMemberRowID == item.AllocatedToDE).Select(t => t.TMTitle + " " + t.TMFirstName + " " + t.TMLastName).FirstOrDefault() : null,
                //    AllocatedToDEDate = item.AllocatedToDEDate,
                //    AllocatedToDEQC = item.AllocatedToDEQC,
                //    AllocatedToDEQCName = item.AllocatedToDEQC > 0 ? db.TeamMembers.Where(t => t.TeamMemberRowID == item.AllocatedToDEQC).Select(t => t.TMTitle + " " + t.TMFirstName + " " + t.TMLastName).FirstOrDefault() : null,
                //    AllocatedToDEQCDate = item.AllocatedToDEQCDate,
                //    AllocatedToDEScanName = item.AllocatedToDEScan > 0 ? db.TeamMembers.Where(t => t.TeamMemberRowID == item.AllocatedToDEScan).Select(t => t.TMTitle + " " + t.TMFirstName + " " + t.TMLastName).FirstOrDefault() : null,
                //    AllocatedToDEScan = item.AllocatedToDEScan,
                //    AllocatedToDEScanDate = item.AllocatedToDEScanDate,
                //    CreatedBy = item.CreatedBy,
                //    CreatedDate = item.CreatedDate,
                //    CaseStatus = item.CaseStatus,
                //    FineStatus = item.FinalStatus,
                //    Status = item.Status,
                //}).ToList();
                //return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public PQPersonalInfoListPagedModel GetAllocatedCandidateListForDEScan(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string CaseStatus = "", byte tatHours = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DateTime todayTATDate = DateTime.Now;

                IQueryable<PQPersonal> data = db.PQPersonals.Include("PQClientMaster").Where(p => p.AllocatedToDEScan == teamMemerRowId);

                if (ClientRowID > 0)
                {
                    data = data.Where(b => b.ClientRowID == ClientRowID);
                }

                if (!string.IsNullOrEmpty(CaseStatus))
                {
                    if (CaseStatus == "Received")
                    {
                        data = data.Where(p => DbFunctions.TruncateTime(p.AllocatedToDEScanDate) == DbFunctions.TruncateTime(todayTATDate));
                    }
                    if (CaseStatus == "Pending")
                    {
                        data = data.Where(p => p.CaseStatus == "Allocated");
                    }
                    if (CaseStatus == "RejectionWIP")
                    {
                        data = data.Where(p => p.CaseStatus == "Rejected");
                    }
                    if (CaseStatus == "ReOpen")
                    {
                        data = data.Where(p => p.CaseStatus == "ReOpen");
                    }
                    if (CaseStatus == "Priority")
                    {
                        data = data.Where(p => p.CaseType == "Priority");
                    }
                    if (CaseStatus == "Closed")
                    {
                        data = data.Where(t => DbFunctions.TruncateTime(t.DEScanCompletedDate) == DbFunctions.TruncateTime(todayTATDate) && t.DEScanCaseStatus == "Completed");
                    }
                }

                if (!string.IsNullOrEmpty(RecievingToDate) && !string.IsNullOrEmpty(RecievingFromDate))
                {
                    DateTime startDate = Convert.ToDateTime(RecievingFromDate);
                    DateTime EndDate = Convert.ToDateTime(RecievingToDate);

                    data = data.Where(b => DbFunctions.TruncateTime(b.AllocatedToDEScanDate) >= startDate.Date && DbFunctions.TruncateTime(b.AllocatedToDEScanDate) <= EndDate.Date);
                }

                if (!string.IsNullOrEmpty(CompletedToDate) && !string.IsNullOrEmpty(CompletedFromDate))
                {
                    DateTime startDate = Convert.ToDateTime(CompletedFromDate);
                    DateTime EndDate = Convert.ToDateTime(CompletedToDate);

                    data = data.Where(b => DbFunctions.TruncateTime(b.DEScanCompletedDate) >= startDate.Date && DbFunctions.TruncateTime(b.DEScanCompletedDate) <= EndDate.Date);
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    data = data.Where(b => (b.CandTitle + " " + b.Name + " " + b.MiddleName + " " + b.LastName).ToString().Replace(" ", "").Contains(Search.Replace(" ", "")) || b.CompanyRefNo.ToLower().Trim() == Search.ToLower().Trim());
                }

                if (tatHours > 0)
                {
                    data = data.Where(a => a.CaseStatus == "Allocated" && a.DEScanCaseStatus != "Completed");
                    data = new DashboardDataEntryRepository().GetPersonalInfoDETMScanByTATHours(data, tatHours);
                }

                switch (sort)
                {
                    default:
                        data = sortDir == "asc" ? data.OrderByDescending(d => d.PersonalRowID) : data.OrderBy(d => d.PersonalRowID);
                        break;
                }

                PQPersonalInfoListPagedModel model = new PQPersonalInfoListPagedModel();
                model.PageSize = pageSize;
                model.TotalRecords = data.Count();
                model.PQPersonalInfo = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new PQPersonalDetailViewModel
                {
                    PersonalRowID = item.PersonalRowID,
                    ClientRowID = item.ClientRowID,
                    ClientName = item.PQClientMaster.MasterAbbreviation.ClientName + " " + item.PQClientMaster.MasterClientSubGroup.ClientSubGroupName,
                    CompanyRefNo = item.CompanyRefNo,
                    CandTitle = item.CandTitle,
                    Name = item.CandTitle + " " + item.Name + " " + item.MiddleName + " " + item.LastName,
                    AllocatedToDEScanDate = item.AllocatedToDEScanDate,
                    CaseStatus = item.CaseStatus,
                    FineStatus = item.FinalStatus,
                    OtherDetails = item.OtherDetails,
                    Status = item.Status,
                    DEScanCaseStatus = item.DEScanCaseStatus
                }).ToList();
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PQPersonalInfoListPagedModel GetAllocatedCandidateListForDEScan1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string CaseStatus = "", string rolename = "", short teamMemerRowId = 0, short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", byte tatHours = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataSet ds = new DataSet();
                PQPersonalInfoListPagedModel model = new PQPersonalInfoListPagedModel();
                if (!string.IsNullOrEmpty(Search) || ClientRowID > 0)
                {
                    string ClientId = "";
                    if (ClientRowID == 0)
                    {
                        ClientId = null;
                    }
                    else
                    {
                        ClientId = ClientRowID.ToString();
                    }
                    ds = cc.getCasewiseSummaryDashboardInternalDetail(teamMemerRowId, rolename, CaseStatus, Search, ClientId);
                }
                else
                {

                    ds = cc.getCasewiseSummaryDashboardInternalDetail(teamMemerRowId, rolename, CaseStatus);
                }
                // ds = cc.getCasewiseSummaryDetail(ClientRowID, CaseStatus,ClientUserRowID);
                List<PQPersonalDetailViewModel> lmd = new List<PQPersonalDetailViewModel>();
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        lmd.Add(new PQPersonalDetailViewModel
                        {
                            PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]),
                            ClientRowID = Convert.ToInt16(dr["ClientRowID"]),
                            Name = (dr["Candidate Name"]).ToString(),
                            CandidateCode = (dr["CandidateCode"]).ToString(),
                            CompanyRefNo = (dr["Securitas ID"]).ToString(),
                            CaseStatus = (dr["Case Status"]).ToString(),
                            CreatedDate = Convert.ToDateTime(dr["Created On"]),
                            ClientName = (dr["ClientName"]).ToString(),
                            Status = byte.Parse(dr["status"].ToString()),
                            DEScanCaseStatus = (dr["DEScanCaseStatus"]).ToString(),
                            AllocatedToDEScanDate = Convert.ToDateTime(dr["AllocatedToDEScanDate"].ToString()),
                            OtherDetails = (dr["OtherDetails"]).ToString(),
                            IsSeniorCase = dr.Table.Columns.Contains("Seniorprofile") ? dr["Seniorprofile"] != DBNull.Value ? dr["Seniorprofile"].ToString() : "0" : "0",
                            FineStatus = (dr["FinalStatus"]).ToString(),

                            CheckStatusId = (dr["CheckStatusId"].ToString()),
                            CaseStatusId = (dr["CaseStausID"].ToString()),
                            //SubCheckRowID = Convert.ToInt32(dr["SubCheckFamilyID"].ToString()),
                            EmployeeID = (dr["EmployeeID"].ToString()),
                        });

                    }
                    model.TotalRecords = ds.Tables[0].Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }


                model.PageSize = pageSize;

                model.PQPersonalInfo = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;


                //PQPersonalInfoListPagedModel model = new PQPersonalInfoListPagedModel();
                //model.PageSize = pageSize;
                //model.TotalRecords = data.Count();
                //model.PQPersonalInfo = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new PQPersonalDetailViewModel
                //{
                //    PersonalRowID = item.PersonalRowID,
                //    ClientRowID = item.ClientRowID,
                //    ClientName = item.PQClientMaster.MasterAbbreviation.ClientName + " " + item.PQClientMaster.MasterClientSubGroup.ClientSubGroupName,
                //    CompanyRefNo = item.CompanyRefNo,
                //    CandTitle = item.CandTitle,
                //    Name = item.CandTitle + " " + item.Name + " " + item.MiddleName + " " + item.LastName,
                //    AllocatedToDEScanDate = item.AllocatedToDEScanDate,
                //    CaseStatus = item.CaseStatus,
                //    FineStatus = item.FinalStatus,
                //    OtherDetails = item.OtherDetails,
                //    Status = item.Status,
                //    DEScanCaseStatus = item.DEScanCaseStatus
                //}).ToList();
                //return model;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public PQPersonalInfoListPagedModel GetAllocatedCandidateListForDataEntry(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string CaseStatus = "", byte tatHours = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DateTime todayTATDate = DateTime.Now;

                IQueryable<PQPersonal> data = db.PQPersonals.Include("PQClientMaster").Where(p => p.AllocatedToDE == teamMemerRowId);

                if (ClientRowID > 0)
                {
                    data = data.Where(b => b.ClientRowID == ClientRowID);
                }

                if (!string.IsNullOrEmpty(RecievingToDate) && !string.IsNullOrEmpty(RecievingFromDate))
                {
                    DateTime startDate = Convert.ToDateTime(RecievingFromDate);
                    DateTime EndDate = Convert.ToDateTime(RecievingToDate);

                    data = data.Where(b => DbFunctions.TruncateTime(b.CreatedDate) >= startDate.Date && DbFunctions.TruncateTime(b.CreatedDate) <= EndDate.Date);
                }

                if (!string.IsNullOrEmpty(CompletedToDate) && !string.IsNullOrEmpty(CompletedFromDate))
                {
                    DateTime startDate = Convert.ToDateTime(CompletedFromDate);
                    DateTime EndDate = Convert.ToDateTime(CompletedToDate);

                    data = data.Where(b => DbFunctions.TruncateTime(b.DeliveryDate) >= startDate.Date && DbFunctions.TruncateTime(b.DeliveryDate) <= EndDate.Date);
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    data = data.Where(b => (b.CandTitle + " " + b.Name + " " + b.MiddleName + " " + b.LastName).ToString().Replace(" ", "").Contains(Search.Replace(" ", "")) || b.CompanyRefNo.ToLower().Trim() == Search.ToLower().Trim());
                }

                if (!string.IsNullOrEmpty(CaseStatus))
                {
                    if (CaseStatus == "Received")
                    {
                        data = data.Where(p => DbFunctions.TruncateTime(p.AllocatedToDEDate) == DbFunctions.TruncateTime(todayTATDate));
                    }
                    if (CaseStatus == "Pending")
                    {
                        data = data.Where(p => p.DECaseStatus == "WIP");
                    }
                    if (CaseStatus == "RejectionWIP")
                    {
                        data = data.Where(p => p.CaseStatus == "Rejected");
                    }
                    if (CaseStatus == "ReOpen")
                    {
                        data = data.Where(p => p.CaseStatus == "ReOpen");
                    }
                    if (CaseStatus == "Priority")
                    {
                        data = data.Where(p => p.CaseType == "Priority");
                    }
                    if (CaseStatus == "Closed")
                    {
                        data = data.Where(t => DbFunctions.TruncateTime(t.DECompletedDate) == DbFunctions.TruncateTime(todayTATDate) && t.DECaseStatus == "Completed-DE");
                    }

                    if (CaseStatus == "InsuffRaised")
                    {
                        data = data.Where(p => p.CaseStatus == "Insufficent");
                    }
                    if (CaseStatus == "Research")
                    {
                        data = data.Where(p => p.CaseStatus == "Research");
                    }
                    if (CaseStatus == "YTR")
                    {
                        data = data.Where(p => p.CaseStatus == "YTR");
                    }
                    if (CaseStatus == "InsuffResolved")
                    {
                        data = data.Where(p => p.CaseStatus == "InsuffResolved");
                    }
                }

                if (tatHours > 0)
                {
                    data = data.Where(a => a.DECaseStatus == "WIP");
                    data = new DashboardDataEntryRepository().GetPersonalInfoDETMByTATHours(data, tatHours);
                }

                switch (sort)
                {
                    default:
                        data = sortDir == "asc" ? data.OrderByDescending(d => d.PersonalRowID) : data.OrderBy(d => d.PersonalRowID);
                        break;
                }

                PQPersonalInfoListPagedModel model = new PQPersonalInfoListPagedModel();
                model.PageSize = pageSize;
                model.TotalRecords = data.Count();
                model.PQPersonalInfo = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new PQPersonalDetailViewModel
                {
                    PersonalRowID = item.PersonalRowID,
                    ClientRowID = item.ClientRowID,
                    ClientName = item.PQClientMaster.MasterAbbreviation.ClientName + " " + item.PQClientMaster.MasterClientSubGroup.ClientSubGroupName,
                    CompanyRefNo = item.CompanyRefNo,
                    CandTitle = item.CandTitle,
                    Name = item.CandTitle + " " + item.Name + " " + item.MiddleName + " " + item.LastName,
                    AllocatedToDEDate = item.AllocatedToDEDate,
                    CaseStatus = item.CaseStatus,
                    FineStatus = item.FinalStatus,
                    DECaseStatus = item.DECaseStatus,
                    Status = item.Status
                }).ToList();
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PQPersonalInfoListPagedModel GetAllocatedCandidateListForDataEntry1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string rolename = "", short teamMemerRowId = 0, short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string CaseStatus = "", byte tatHours = 0, string version = "", bool fetchDataCheckbox = false)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataSet ds = new DataSet();
                PQPersonalInfoListPagedModel model = new PQPersonalInfoListPagedModel();
                //if (!string.IsNullOrEmpty(Search))
                //{
                //    ds = cc.getCasewiseSummaryDashboardInternalDetail(teamMemerRowId, rolename, CaseStatus, Search);
                //}
                if (!string.IsNullOrEmpty(Search) || ClientRowID > 0)
                {
                    string ClientId = "";
                    if (ClientRowID == 0)
                    {
                        ClientId = null;
                    }
                    else
                    {
                        ClientId = ClientRowID.ToString();
                    }
                    if (fetchDataCheckbox == true)
                    {
                        ds = cc.getCasewiseSummaryDashboardInternalDetailnew(teamMemerRowId, rolename, CaseStatus, Search, ClientId, fetchDataCheckbox);
                    }
                    else
                    {
                        ds = cc.getCasewiseSummaryDashboardInternalDetail(teamMemerRowId, rolename, CaseStatus, Search, ClientId);
                    }

                }

                else
                {
                    if (fetchDataCheckbox == true)
                    {
                        ds = cc.getCasewiseSummaryDashboardInternalDetailnew(teamMemerRowId, rolename, CaseStatus, "", fetchDataCheckbox);
                    }
                    else
                    {
                        ds = cc.getCasewiseSummaryDashboardInternalDetail(teamMemerRowId, rolename, CaseStatus);
                    }

                }

                List<PQPersonalDetailViewModel> lmd = new List<PQPersonalDetailViewModel>();
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        //lmd.Add(new PQPersonalDetailViewModel
                        //{
                        PQPersonalDetailViewModel bb = new PQPersonalDetailViewModel();
                        bb.PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]);
                        bb.ClientRowID = Convert.ToInt16(dr["ClientRowID"]);
                        bb.Name = (dr["Candidate Name"]).ToString();
                        bb.CandidateCode = (dr["CandidateCode"]).ToString();
                        bb.CompanyRefNo = (dr["Securitas ID"]).ToString();
                        bb.CaseStatus = (dr["Case Status"]).ToString();
                        bb.CreatedDate = Convert.ToDateTime(dr["Created On"]);
                        bb.ClientName = (dr["ClientName"]).ToString();
                        if (dr["AllocatedToDEDate"].ToString() != "")
                            bb.AllocatedToDEDate = Convert.ToDateTime(dr["AllocatedToDEDate"].ToString());
                        bb.FineStatus = (dr["FinalStatus"]).ToString();
                        bb.DECaseStatus = (dr["DECaseStatus"]).ToString();
                        bb.IsSeniorCase = dr.Table.Columns.Contains("Seniorprofile") ? dr["Seniorprofile"] != DBNull.Value ? dr["Seniorprofile"].ToString() : "0" : "0";

                        bb.Status = byte.Parse(dr["Status"].ToString());
                        bb.SubCheckRowID = Convert.ToInt32((dr["SubCheckFamilyID"]).ToString());
                        bb.CheckStatusId = (dr["CheckStatusID"]).ToString();
                        lmd.Add(bb);


                        //});

                    }
                    model.TotalRecords = ds.Tables[0].Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }


                model.PageSize = pageSize;

                model.PQPersonalInfo = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;

                //PQPersonalInfoListPagedModel model = new PQPersonalInfoListPagedModel();
                //model.PageSize = pageSize;
                //model.TotalRecords = data.Count();
                //model.PQPersonalInfo = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new PQPersonalDetailViewModel
                //{
                //    PersonalRowID = item.PersonalRowID,
                //    ClientRowID = item.ClientRowID,
                //    ClientName = item.PQClientMaster.MasterAbbreviation.ClientName + " " + item.PQClientMaster.MasterClientSubGroup.ClientSubGroupName,
                //    CompanyRefNo = item.CompanyRefNo,
                //    CandTitle = item.CandTitle,
                //    Name = item.CandTitle + " " + item.Name + " " + item.MiddleName + " " + item.LastName,
                //    AllocatedToDEDate = item.AllocatedToDEDate,
                //    CaseStatus = item.CaseStatus,
                //    FineStatus = item.FinalStatus,
                //    DECaseStatus = item.DECaseStatus,
                //    Status = item.Status
                //}).ToList();
                //return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PQPersonalInfoListPagedModel GetAllocatedCandidateListForQC(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string SearchType = "", string CaseStatus = "", byte tatHours = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DateTime todayTATDate = DateTime.Now;

                IQueryable<PQPersonal> data = db.PQPersonals.Include("PQClientMaster").Where(p => p.AllocatedToDEQC == teamMemerRowId);

                if (ClientRowID > 0)
                {
                    data = data.Where(b => b.ClientRowID == ClientRowID);
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    if (SearchType == "Keyword")
                    {
                        data = data.Where(b => (b.CandTitle + " " + b.Name + " " + b.MiddleName + " " + b.LastName).ToString().Replace(" ", "").Contains(Search.Replace(" ", "")) || b.CompanyRefNo.ToLower().Trim() == Search.ToLower().Trim());
                    }
                    else
                    {
                        var firstObj = db.TeamMembers.FirstOrDefault(b => (b.TMTitle + " " + b.TMFirstName + " " + b.TMLastName).ToString().Replace(" ", String.Empty) == Search.Trim().Replace(" ", String.Empty));
                        if (firstObj != null)
                        {
                            data = data.Where(b => b.AllocatedToDE == firstObj.TeamMemberRowID || b.AllocatedToDEScan == firstObj.TeamMemberRowID || b.AllocatedToDEQC == firstObj.TeamMemberRowID);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(RecievingToDate) && !string.IsNullOrEmpty(RecievingFromDate))
                {
                    DateTime startDate = Convert.ToDateTime(RecievingFromDate);
                    DateTime EndDate = Convert.ToDateTime(RecievingToDate);

                    data = data.Where(b => DbFunctions.TruncateTime(b.OrderDate) >= startDate.Date && DbFunctions.TruncateTime(b.OrderDate) <= EndDate.Date);
                }

                if (!string.IsNullOrEmpty(CompletedToDate) && !string.IsNullOrEmpty(CompletedFromDate))
                {
                    DateTime startDate = Convert.ToDateTime(CompletedFromDate);
                    DateTime EndDate = Convert.ToDateTime(CompletedToDate);

                    data = data.Where(b => DbFunctions.TruncateTime(b.DeliveryDate) >= startDate.Date && DbFunctions.TruncateTime(b.DeliveryDate) <= EndDate.Date);
                }

                if (!string.IsNullOrEmpty(CaseStatus))
                {
                    if (CaseStatus == "Received")
                    {
                        data = data.Where(p => DbFunctions.TruncateTime(p.AllocatedToDEQCDate) == DbFunctions.TruncateTime(todayTATDate));
                    }
                    if (CaseStatus == "Pending")
                    {
                        data = data.Where(p => p.DEQCCaseStatus == "WIP");
                    }
                    if (CaseStatus == "RejectionWIP")
                    {
                        data = data.Where(p => p.CaseStatus == "Rejected");
                    }
                    if (CaseStatus == "ReOpen")
                    {
                        data = data.Where(p => p.CaseStatus == "ReOpen");
                    }
                    if (CaseStatus == "Priority")
                    {
                        data = data.Where(p => p.CaseType == "Priority");
                    }
                    if (CaseStatus == "Closed")
                    {
                        data = data.Where(t => DbFunctions.TruncateTime(t.DEQCCompletedDate) == DbFunctions.TruncateTime(todayTATDate) && t.DEQCCaseStatus == "Completed-QC");
                    }
                    if (CaseStatus == "InsuffRaised")
                    {
                        data = data.Where(p => p.CaseStatus == "Insufficent");
                    }
                    if (CaseStatus == "Research")
                    {
                        data = data.Where(p => p.CaseStatus == "Research");
                    }
                    if (CaseStatus == "YTR")
                    {
                        data = data.Where(p => p.CaseStatus == "YTR");
                    }
                }

                if (tatHours > 0)
                {
                    data = data.Where(a => a.DEQCCaseStatus == "WIP");
                    data = new DashboardDataEntryRepository().GetPersonalInfoDEQCTMByTATHours(data, tatHours);
                }

                switch (sort)
                {
                    case "clientname":
                        data = sortDir == "asc" ? data.OrderBy(d => d.PQClientMaster.MasterAbbreviation.ClientName) : data.OrderByDescending(d => d.PQClientMaster.MasterAbbreviation.ClientName);
                        break;
                    case "name":
                        data = sortDir == "asc" ? data.OrderBy(d => (d.Name + " " + d.MiddleName + " " + d.LastName)) : data.OrderByDescending(d => (d.Name + " " + d.MiddleName + " " + d.LastName));
                        break;
                    default:
                        data = sortDir == "asc" ? data.OrderByDescending(d => d.PersonalRowID) : data.OrderBy(d => d.PersonalRowID);
                        break;
                }

                PQPersonalInfoListPagedModel model = new PQPersonalInfoListPagedModel();
                model.PageSize = pageSize;
                model.TotalRecords = data.Count();
                model.PQPersonalInfo = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new PQPersonalDetailViewModel
                {
                    PersonalRowID = item.PersonalRowID,
                    ClientRowID = item.ClientRowID,
                    ClientName = item.PQClientMaster.MasterAbbreviation.ClientName + " " + item.PQClientMaster.MasterClientSubGroup.ClientSubGroupName,
                    CompanyRefNo = item.CompanyRefNo,
                    CandTitle = item.CandTitle,
                    Name = item.CandTitle + " " + item.Name + " " + item.MiddleName + " " + item.LastName,
                    AllocatedToDEScanName = item.AllocatedToDEScan > 0 ? db.TeamMembers.Where(t => t.TeamMemberRowID == item.AllocatedToDEScan).Select(t => t.TMTitle + " " + t.TMFirstName + " " + t.TMLastName).FirstOrDefault() : null,
                    AllocatedToDEName = item.AllocatedToDE > 0 ? db.TeamMembers.Where(t => t.TeamMemberRowID == item.AllocatedToDE).Select(t => t.TMTitle + " " + t.TMFirstName + " " + t.TMLastName).FirstOrDefault() : null,
                    AllocatedToDEQCDate = item.AllocatedToDEQCDate,
                    CaseStatus = item.CaseStatus,
                    Status = item.Status,
                    OtherDetails = item.OtherDetails,
                    DEQCCaseStatus = item.DEQCCaseStatus,
                    OtherDetails1 = item.OtherDetails1,
                }).ToList();
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public PQPersonalInfoListPagedModel GetAllocatedCandidateListForQC1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string rolename = "", short teamMemerRowId = 0, short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string SearchType = "", string CaseStatus = "", byte tatHours = 0, bool fetchDataCheckbox = false)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataSet ds = new DataSet();
                PQPersonalInfoListPagedModel model = new PQPersonalInfoListPagedModel();
                if (!string.IsNullOrEmpty(Search) || ClientRowID > 0)
                {
                    string ClientId = "";
                    if (ClientRowID == 0)
                    {
                        ClientId = null;
                    }
                    else
                    {
                        ClientId = ClientRowID.ToString();
                    }
                    if (fetchDataCheckbox == true)
                    {
                        ds = cc.getCasewiseSummaryDashboardInternalDetailnew(teamMemerRowId, rolename, CaseStatus, Search, ClientId, fetchDataCheckbox);
                    }
                    else
                    {


                        ds = cc.getCasewiseSummaryDashboardInternalDetail(teamMemerRowId, rolename, CaseStatus, Search, ClientId);
                    }
                }
                else
                {
                    if (fetchDataCheckbox == true)
                    {
                        ds = cc.getCasewiseSummaryDashboardInternalDetailnew(teamMemerRowId, rolename, CaseStatus, "", fetchDataCheckbox);
                    }
                    else
                    {


                        ds = cc.getCasewiseSummaryDashboardInternalDetail(teamMemerRowId, rolename, CaseStatus);
                    }
                }
                // ds = cc.getCasewiseSummaryDetail(ClientRowID, CaseStatus,ClientUserRowID);
                List<PQPersonalDetailViewModel> lmd = new List<PQPersonalDetailViewModel>();
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        lmd.Add(new PQPersonalDetailViewModel
                        {
                            PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]),
                            EPersonalRowID = PEncrypt(dr["PersonalRowID"].ToString()),
                            ClientRowID = Convert.ToInt16(dr["ClientRowID"]),
                            EClientRowID = PEncrypt(dr["ClientRowID"].ToString()),
                            Name = (dr["Candidate Name"]).ToString(),
                            CandidateCode = (dr["CandidateCode"]).ToString(),
                            CompanyRefNo = (dr["Securitas ID"]).ToString(),
                            CaseStatus = (dr["Case Status"]).ToString(),
                            CreatedDate = Convert.ToDateTime(dr["Created On"]),
                            ClientName = (dr["ClientName"]).ToString(),
                            OtherDetails = (dr["ClientName"]).ToString(),
                            DEQCCaseStatus = (dr["ClientName"]).ToString(),
                            OtherDetails1 = (dr["ClientName"]).ToString(),
                            AllocatedToDEQCDate = Convert.ToDateTime(dr["AllocatedToDEQCDate"].ToString()),
                            AllocatedToDEScanName = (dr["scannerName"]).ToString(),
                            AllocatedToDEName = (dr["DEName"]).ToString(),
                            SubCheckRowID = Convert.ToInt32((dr["SubCheckFamilyID"]).ToString()),
                            CheckStatusId = (dr["CheckStatusId"]).ToString(),
                            ECheckStatusId = PEncrypt(dr["CheckStatusId"].ToString()),
                            IsSeniorCase = dr.Table.Columns.Contains("Seniorprofile") ? dr["Seniorprofile"] != DBNull.Value ? dr["Seniorprofile"].ToString() : "0" : "0"


                        });

                    }
                    model.TotalRecords = ds.Tables[0].Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }


                model.PageSize = pageSize;

                model.PQPersonalInfo = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;

                //PQPersonalInfoListPagedModel model = new PQPersonalInfoListPagedModel();
                //model.PageSize = pageSize;
                //model.TotalRecords = data.Count();
                //model.PQPersonalInfo = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new PQPersonalDetailViewModel
                //{
                //    PersonalRowID = item.PersonalRowID,
                //    ClientRowID = item.ClientRowID,
                //    ClientName = item.PQClientMaster.MasterAbbreviation.ClientName + " " + item.PQClientMaster.MasterClientSubGroup.ClientSubGroupName,
                //    CompanyRefNo = item.CompanyRefNo,
                //    CandTitle = item.CandTitle,
                //    Name = item.CandTitle + " " + item.Name + " " + item.MiddleName + " " + item.LastName,
                //    AllocatedToDEScanName = item.AllocatedToDEScan > 0 ? db.TeamMembers.Where(t => t.TeamMemberRowID == item.AllocatedToDEScan).Select(t => t.TMTitle + " " + t.TMFirstName + " " + t.TMLastName).FirstOrDefault() : null,
                //    AllocatedToDEName = item.AllocatedToDE > 0 ? db.TeamMembers.Where(t => t.TeamMemberRowID == item.AllocatedToDE).Select(t => t.TMTitle + " " + t.TMFirstName + " " + t.TMLastName).FirstOrDefault() : null,
                //    AllocatedToDEQCDate = item.AllocatedToDEQCDate,
                //    CaseStatus = item.CaseStatus,
                //    Status = item.Status,
                //    OtherDetails = item.OtherDetails,
                //    DEQCCaseStatus = item.DEQCCaseStatus,
                //    OtherDetails1 = item.OtherDetails1,
                //}).ToList();
                //return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int SaveChanges()
        {
            try
            {
                return db.SaveChanges();
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                        .SelectMany(x => x.ValidationErrors)
                        .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);
                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);
                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }

        public bool IsCandidateExist(short clientRowId, string VoterIDCardNo, string DrivingLicenseNo, string AadhaarCardNo, string PassportNo, string PanCardNo)
        {
            try
            {
                var candidate = db.PQPersonals.Where(m => m.ClientRowID == clientRowId && (
                (!string.IsNullOrEmpty(VoterIDCardNo) && m.VoterIDCardNo.Trim().ToLower() == VoterIDCardNo.Trim().ToLower()) ||
                (!string.IsNullOrEmpty(DrivingLicenseNo) && m.DrivingLicenseNo.Trim().ToLower() == DrivingLicenseNo.Trim().ToLower()) ||
                (!string.IsNullOrEmpty(AadhaarCardNo) && m.AadhaarCardNo.Trim().ToLower() == AadhaarCardNo.Trim().ToLower()) ||
                (!string.IsNullOrEmpty(PassportNo) && m.PassportNo.Trim().ToLower() == PassportNo.Trim().ToLower()) ||
                (!string.IsNullOrEmpty(PanCardNo) && m.PanCardNo.Trim().ToLower() == PanCardNo.Trim().ToLower()) && m.Status == 1)).FirstOrDefault();

                if (candidate != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool IsCandidateExistWiproBulk(short clientRowId, string VoterIDCardNo, string DrivingLicenseNo, string AadhaarCardNo, string PassportNo, string PanCardNo, string ClientrefId)
        {
            try
            {
                var candidate = db.PQPersonals.Where(m => m.ClientRowID == clientRowId && (
                (!string.IsNullOrEmpty(VoterIDCardNo) && m.VoterIDCardNo.Trim().ToLower() == VoterIDCardNo.Trim().ToLower()) ||
                (!string.IsNullOrEmpty(DrivingLicenseNo) && m.DrivingLicenseNo.Trim().ToLower() == DrivingLicenseNo.Trim().ToLower()) ||
                (!string.IsNullOrEmpty(AadhaarCardNo) && m.AadhaarCardNo.Trim().ToLower() == AadhaarCardNo.Trim().ToLower()) ||
                (!string.IsNullOrEmpty(PassportNo) && m.PassportNo.Trim().ToLower() == PassportNo.Trim().ToLower()) ||
                (!string.IsNullOrEmpty(ClientrefId) && m.ClientRefID.Trim().ToLower() == ClientrefId.Trim().ToLower()) ||
                (!string.IsNullOrEmpty(PanCardNo) && m.PanCardNo.Trim().ToLower() == PanCardNo.Trim().ToLower()) && m.Status == 1)).FirstOrDefault();

                if (candidate != null)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public int GetAllCandidatesById(short ClientRowID = 0)
        {
            try
            {
                return db.PQPersonals.Where(p => p.ClientRowID == ClientRowID).Count();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetAllCandidateCodeGenerationById(short ClientRowID = 0)
        {
            try
            {
                return db.PQPersonals.Where(p => p.ClientRowID == ClientRowID && p.PQClientMaster.CodeGeneration == 1).Count();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public UpdatePQPersonalViewModel GetPQPersonalForUpdateById(int PersonalRowID = 0)
        {
            try
            {
                UpdatePQPersonalViewModel model = new UpdatePQPersonalViewModel();
                var entity = db.PQPersonals.Find(PersonalRowID);

                if (entity != null)
                {
                    model.PersonalRowID = entity.PersonalRowID;
                    model.ClientRowID = entity.ClientRowID;
                    model.ClientUserRowID = entity.ClientUserRowID;
                    model.CandidateUserID = entity.CandidateUserID;
                    model.ClientRefID = entity.ClientRefID;
                    model.CompanyRefNo = entity.CompanyRefNo;
                    model.CandidateCode = entity.CandidateCode;
                    model.CandTitle = entity.CandTitle;
                    model.Name = entity.Name;
                    model.MiddleName = entity.MiddleName;
                    model.LastName = entity.LastName;
                    model.FatherTitle = entity.FatherTitle;
                    model.FatherName = entity.FatherName;
                    model.FatherMiddleName = entity.FatherMiddleName;
                    model.FatherLastName = entity.FatherLastName;
                    model.Gender = entity.Gender;
                    model.DOB = entity.DOB;
                    model.MaritalStatus = entity.MaritalStatus;
                    model.MobileNo = entity.MobileNo;
                    model.AlternateNo = entity.AlternateNo;
                    model.EmailID = entity.EmailID;
                    model.AlternateEmailID = entity.AlternateEmailID;
                    model.STD = entity.STD;
                    model.ResidencePhoneNo = entity.ResidencePhoneNo;
                    model.PanCard = entity.PanCard;
                    model.PanCardNo = entity.PanCardNo;
                    model.Passport = entity.Passport;
                    model.PassportNo = entity.PassportNo;
                    model.AadhaarCard = entity.AadhaarCard;
                    model.AadhaarCardNo = entity.AadhaarCardNo;
                    model.DrivingLicense = entity.DrivingLicense;
                    model.DrivingLicenseNo = entity.DrivingLicenseNo;
                    model.VoterIDCard = entity.VoterIDCard;
                    model.VoterIDCardNo = entity.VoterIDCardNo;
                    model.CandidateType = entity.CandidateType;
                    model.CheckDateTime = entity.CheckDateTime;//No
                    if (entity.ClientRowID != 599)
                    {
                        model.CandidateType = entity.CandidateType;
                    }

                    model.Catagory = entity.Catagory;
                    model.CaseType = entity.CaseType;
                    model.TAT = entity.TAT;
                    model.Nationality = entity.Nationality;
                    model.EmployeeID = entity.EmployeeID;
                    model.Designation = entity.Designation;
                    model.PlaceofJoining = entity.PlaceofJoining;
                    model.DateofJoining = entity.DateofJoining;
                    model.NationalSkills = entity.NationalSkills;
                    model.SSNNo = entity.SSNNo;
                    model.FromType = entity.FromType;//No
                    model.DetailsEnteredBy = entity.DetailsEnteredBy;//No
                    model.ProcessName = entity.ProcessName;
                    model.BusinessName = entity.BusinessName;
                    model.Type = entity.Type;
                    model.SpouseName = entity.SpouseName;
                    model.SpouseMiddleName = entity.SpouseMiddleName;
                    model.SpouseLastName = entity.SpouseLastName;
                    model.OrderDate = entity.OrderDate;
                    model.DeliveryDate = entity.DeliveryDate;
                    model.CreatedBy = entity.CreatedBy;
                    model.CreatedDate = entity.CreatedDate;
                    model.AllocatedBy = entity.AllocatedBy;
                    model.AllocatedToDE = entity.AllocatedToDE;
                    model.AllocatedToDEDate = entity.AllocatedToDEDate;
                    model.AllocatedToDEQC = entity.AllocatedToDEQC;
                    model.AllocatedToDEQCDate = entity.AllocatedToDEQCDate;
                    model.Remark = entity.Remark;
                    model.FinalColor = entity.FinalColor;
                    model.ExitIntrvwApplicable = entity.ExitIntrvwApplicable;
                    model.CaseStatus = entity.CaseStatus;
                    model.FinalStatus = entity.FinalStatus;
                    model.Status = entity.Status;
                    model.OtherDetails = entity.OtherDetails;
                    model.OtherDetails1 = entity.OtherDetails1;
                    model.OtherDetails2 = entity.OtherDetails2;

                }
                else
                {
                    throw new Exception("Invalid Id!");
                }
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateCandidate(UpdatePQPersonalViewModel model)
        {
            try
            {
                if (model != null && model.PersonalRowID > 0)
                {
                    db.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).CandTitle = model.CandTitle;
                    db.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).Name = model.Name;
                    db.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).MiddleName = model.MiddleName;
                    db.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).LastName = model.LastName;
                    db.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).FatherTitle = model.FatherTitle;
                    db.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).FatherName = model.FatherName;
                    db.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).FatherMiddleName = model.FatherMiddleName;
                    db.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).FatherLastName = model.FatherLastName;
                    db.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).Gender = model.Gender;
                    db.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).DOB = model.DOB;
                    db.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).MaritalStatus = model.MaritalStatus;
                    db.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).MobileNo = model.MobileNo;
                    db.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).AlternateNo = model.AlternateNo;
                    db.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).EmailID = model.EmailID;
                    db.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).AlternateEmailID = model.AlternateEmailID;
                    db.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).STD = model.STD;
                    db.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).ResidencePhoneNo = model.ResidencePhoneNo;
                    db.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).PanCard = model.PanCard;
                    db.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).PanCardNo = model.PanCardNo;
                    db.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).Passport = model.Passport;
                    db.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).PassportNo = model.PassportNo;
                    db.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).AadhaarCard = model.AadhaarCard;
                    db.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).AadhaarCardNo = model.AadhaarCardNo;
                    db.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).DrivingLicense = model.DrivingLicense;
                    db.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).DrivingLicenseNo = model.DrivingLicenseNo;
                    db.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).VoterIDCard = model.VoterIDCard;
                    db.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).VoterIDCardNo = model.VoterIDCardNo;
                    if (model.ClientRowID != 599)
                    {
                        db.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).CandidateType = model.CandidateType;

                    }

                    db.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).Catagory = model.Catagory;
                    //db.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).CaseType = model.CaseType;
                    //db.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).TAT = model.TAT;
                    db.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).Nationality = model.Nationality;
                    db.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).EmployeeID = model.EmployeeID;
                    db.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).Designation = model.Designation;
                    db.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).PlaceofJoining = model.PlaceofJoining;
                    db.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).DateofJoining = model.DateofJoining;
                    db.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).NationalSkills = model.NationalSkills;
                    db.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).SSNNo = model.SSNNo;
                    db.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).SpouseName = model.SpouseName;
                    db.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).SpouseMiddleName = model.SpouseMiddleName;
                    db.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).SpouseLastName = model.SpouseLastName;
                    db.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).BusinessName = model.BusinessName;
                    if (model.ClientRowID != 317 && model.ClientRowID != 327)
                    {
                        db.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).ProcessName = model.ProcessName;
                    }
                    //db.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).OrderDate = model.OrderDate;
                    //db.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).DeliveryDate = model.DeliveryDate;
                }
                else
                {
                    throw new Exception("Candidate could not be blank!");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateCadidateAllocationByID(int PersonalRowID = 0)
        {
            try
            {
                if (PersonalRowID > 0)
                {
                    db.PQPersonals.Single(c => c.PersonalRowID == PersonalRowID).AllocatedToDE = 0;
                    db.PQPersonals.Single(c => c.PersonalRowID == PersonalRowID).AllocatedToDEQC = 0;
                    db.PQPersonals.Single(c => c.PersonalRowID == PersonalRowID).AllocatedToDEScan = 0;
                }
                else
                {
                    throw new Exception("Invalid Id!");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Update Personal for the allocation to data entry and Quality checks
        public void UpdatePQPersonalAllocateDEOrQCByID(UpdatePQPersonalViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null && model.PersonalRowID > 0)
                        {
                            context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).AllocatedBy = model.AllocatedBy;
                            context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).AllocatedToDE = model.AllocatedToDE;
                            context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).AllocatedToDEDate = model.AllocatedToDEDate;
                            context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).AllocatedToDEQC = model.AllocatedToDEQC;
                            context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).AllocatedToDEQCDate = model.AllocatedToDEQCDate;
                            context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).AllocatedToDEScan = model.AllocatedToDEScan;
                            context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).AllocatedToDEScanDate = model.AllocatedToDEScanDate;

                            context.SaveChanges();

                            var PersonalDetails = context.PQPersonals.Find(model.PersonalRowID);
                            if (PersonalDetails.FromType == "Client-Cases" && PersonalDetails.DetailsEnteredBy == "Candidate")
                            {
                                var AddressList = context.PQAddressStatus.Where(a => a.PQAddress.PersonalRowID == model.PersonalRowID).ToList();
                                if (AddressList.Count() > 0)
                                {
                                    foreach (var item in AddressList)
                                    {
                                        context.PQAddressStatus.Single(c => c.AddressStatusRowID == item.AddressStatusRowID).DETLAllocatedTo = model.AllocatedBy;
                                        context.PQAddressStatus.Single(c => c.AddressStatusRowID == item.AddressStatusRowID).DETMAllocatedTo = model.AllocatedToDE;
                                        context.PQAddressStatus.Single(c => c.AddressStatusRowID == item.AddressStatusRowID).DETMAllocatedDate = model.AllocatedToDEDate;
                                        context.PQAddressStatus.Single(c => c.AddressStatusRowID == item.AddressStatusRowID).DETMQCAllocatedTo = model.AllocatedToDEQC;
                                        context.PQAddressStatus.Single(c => c.AddressStatusRowID == item.AddressStatusRowID).DETMQCAllocatedDate = model.AllocatedToDEQCDate;

                                        context.SaveChanges();
                                    }
                                }
                                var EmploymentList = context.PQEmploymentStatus.Where(a => a.PQEmployment.PersonalRowID == model.PersonalRowID).ToList();
                                if (EmploymentList.Count() > 0)
                                {
                                    foreach (var item in EmploymentList)
                                    {
                                        context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == item.EmploymentStatusRowID).DETLAllocatedTo = model.AllocatedBy;
                                        context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == item.EmploymentStatusRowID).DETMAllocatedTo = model.AllocatedToDE;
                                        context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == item.EmploymentStatusRowID).DETMAllocatedDate = model.AllocatedToDEDate;
                                        context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == item.EmploymentStatusRowID).DETMQCAllocatedTo = model.AllocatedToDEQC;
                                        context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == item.EmploymentStatusRowID).DETMQCAllocatedDate = model.AllocatedToDEQCDate;

                                        context.SaveChanges();
                                    }
                                }
                                var EducationList = context.PQEducationStatus.Where(a => a.PQEducation.PersonalRowID == model.PersonalRowID).ToList();
                                if (EducationList.Count() > 0)
                                {
                                    foreach (var item in EducationList)
                                    {
                                        context.PQEducationStatus.Single(c => c.EducationStatusRowID == item.EducationStatusRowID).DETLAllocatedTo = model.AllocatedBy;
                                        context.PQEducationStatus.Single(c => c.EducationStatusRowID == item.EducationStatusRowID).DETMAllocatedTo = model.AllocatedToDE;
                                        context.PQEducationStatus.Single(c => c.EducationStatusRowID == item.EducationStatusRowID).DETMAllocatedDate = model.AllocatedToDEDate;
                                        context.PQEducationStatus.Single(c => c.EducationStatusRowID == item.EducationStatusRowID).DETMQCAllocatedTo = model.AllocatedToDEQC;
                                        context.PQEducationStatus.Single(c => c.EducationStatusRowID == item.EducationStatusRowID).DETMQCAllocatedDate = model.AllocatedToDEQCDate;

                                        context.SaveChanges();
                                    }
                                }
                                var CriminalList = context.PQCriminalStatus.Where(a => a.PQCriminal.PersonalRowID == model.PersonalRowID).ToList();
                                if (CriminalList.Count() > 0)
                                {
                                    foreach (var item in CriminalList)
                                    {
                                        context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == item.CriminalStatusRowID).DETLAllocatedTo = model.AllocatedBy;
                                        context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == item.CriminalStatusRowID).DETMAllocatedTo = model.AllocatedToDE;
                                        context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == item.CriminalStatusRowID).DETMAllocatedDate = model.AllocatedToDEDate;
                                        context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == item.CriminalStatusRowID).DETMQCAllocatedTo = model.AllocatedToDEQC;
                                        context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == item.CriminalStatusRowID).DETMQCAllocatedDate = model.AllocatedToDEQCDate;

                                        context.SaveChanges();
                                    }
                                }
                                var IdentityList = context.PQNationalIdentityStatus.Where(a => a.PQNationalIdentity.PersonalRowID == model.PersonalRowID).ToList();
                                if (IdentityList.Count() > 0)
                                {
                                    foreach (var item in IdentityList)
                                    {
                                        context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == item.NationalIdentityStatusRowID).DETLAllocatedTo = model.AllocatedBy;
                                        context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == item.NationalIdentityStatusRowID).DETMAllocatedTo = model.AllocatedToDE;
                                        context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == item.NationalIdentityStatusRowID).DETMAllocatedDate = model.AllocatedToDEDate;
                                        context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == item.NationalIdentityStatusRowID).DETMQCAllocatedTo = model.AllocatedToDEQC;
                                        context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == item.NationalIdentityStatusRowID).DETMQCAllocatedDate = model.AllocatedToDEQCDate;

                                        context.SaveChanges();
                                    }
                                }
                                var ReferenceList = context.PQReferenceStatus.Where(a => a.PQReference.PersonalRowID == model.PersonalRowID).ToList();
                                if (ReferenceList.Count() > 0)
                                {
                                    foreach (var item in ReferenceList)
                                    {
                                        context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == item.ReferenceStatusRowID).DETLAllocatedTo = model.AllocatedBy;
                                        context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == item.ReferenceStatusRowID).DETMAllocatedTo = model.AllocatedToDE;
                                        context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == item.ReferenceStatusRowID).DETMAllocatedDate = model.AllocatedToDEDate;
                                        context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == item.ReferenceStatusRowID).DETMQCAllocatedTo = model.AllocatedToDEQC;
                                        context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == item.ReferenceStatusRowID).DETMQCAllocatedDate = model.AllocatedToDEQCDate;

                                        context.SaveChanges();
                                    }
                                }
                                var SPLCheckList = context.PQSpecialCheckStatus.Where(a => a.PQSpecialCheck.PersonalRowID == model.PersonalRowID).ToList();
                                if (SPLCheckList.Count() > 0)
                                {
                                    foreach (var item in SPLCheckList)
                                    {
                                        context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == item.SpecialCheckStatusRowID).DETLAllocatedTo = model.AllocatedBy;
                                        context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == item.SpecialCheckStatusRowID).DETMAllocatedTo = model.AllocatedToDE;
                                        context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == item.SpecialCheckStatusRowID).DETMAllocatedDate = model.AllocatedToDEDate;
                                        context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == item.SpecialCheckStatusRowID).DETMQCAllocatedTo = model.AllocatedToDEQC;
                                        context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == item.SpecialCheckStatusRowID).DETMQCAllocatedDate = model.AllocatedToDEQCDate;

                                        context.SaveChanges();
                                    }
                                }
                            }

                            transaction.Commit();
                        }
                        else
                        {
                            transaction.Rollback();
                            throw new Exception("Invalid Details!");
                        }
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        //Update Personal for the allocation to data entry and Quality checks
        public bool UpdatePQPersonalReAllocateDEOrQCByID(UpdatePQPersonalViewModel model)
        {

            int intReturnValue = 0;
            string SP_PARAM_RETURN_VALUE = null;
            try
            {
                if (model != null && model.PersonalRowID > 0)
                {
                    SqlParameter objReturnValueParam = DAS.CreateParameter(SP_PARAM_RETURN_VALUE, SqlDbType.Int, null, ParameterDirection.ReturnValue, 0, false);
                    DAS.ExecuteNonQuery(DAS.ConnectionString, CommandType.StoredProcedure, "UpdatePQPersonalReAllocateDEOrQCByID",
                          DAS.CreateParameter("@PersonalRowID", SqlDbType.Int, model.PersonalRowID, ParameterDirection.Input, 15, false),
                               DAS.CreateParameter("@AllocatedToDE", SqlDbType.Int, model.AllocatedToDE, ParameterDirection.Input, 15, false),
                               DAS.CreateParameter("@AllocatedToDEQC", SqlDbType.Int, model.AllocatedToDEQC, ParameterDirection.Input, 15, false),
                               DAS.CreateParameter("@AllocatedToDEScan", SqlDbType.Int, model.AllocatedToDEScan, ParameterDirection.Input, 15, false),
                               DAS.CreateParameter("@AllocatedBy", SqlDbType.Int, model.AllocatedBy, ParameterDirection.Input, 15, false),
                    objReturnValueParam
                                );
                    intReturnValue = Convert.ToInt32(objReturnValueParam.Value);
                }

            }
            catch (Exception ex)
            {

                throw;
            }

            if (intReturnValue == 0)
            {
                return true;
            }
            else
            { return false; }

        }

        //Get PersonalID and other fields for log transaction and checks package details save
        public UpdatePQPersonalViewModel GetPQPersonalByCandidateCode(string CompanyRefId = "")
        {
            try
            {
                if (!string.IsNullOrEmpty(CompanyRefId))
                {
                    return db.PQPersonals.Where(p => p.CompanyRefNo == CompanyRefId).Select(entity => new UpdatePQPersonalViewModel
                    {
                        PersonalRowID = entity.PersonalRowID,
                        CompanyRefNo = entity.CompanyRefNo,
                        MobileNo = entity.MobileNo,
                        AlternateNo = entity.AlternateNo,
                        EmailID = entity.EmailID,
                        AlternateEmailID = entity.AlternateEmailID,
                        STD = entity.STD,
                        ResidencePhoneNo = entity.ResidencePhoneNo,
                        CandidateType = entity.CandidateType,
                        Catagory = entity.Catagory,
                        CaseType = entity.CaseType,
                        TAT = entity.TAT,
                        Nationality = entity.Nationality,
                        EmployeeID = entity.EmployeeID,
                        Designation = entity.Designation,
                        PlaceofJoining = entity.PlaceofJoining,
                        NationalSkills = entity.NationalSkills,
                        SSNNo = entity.SSNNo,
                        Remark = entity.Remark,
                        FinalColor = entity.FinalColor,
                        FromType = entity.FromType,
                        ExitIntrvwApplicable = entity.ExitIntrvwApplicable,
                        ProcessName = entity.ProcessName,
                        BusinessName = entity.BusinessName,
                        Type = entity.Type,
                        CandidateUserID = entity.CandidateUserID,
                        SpouseName = entity.SpouseName,
                        SpouseMiddleName = entity.SpouseMiddleName,
                        SpouseLastName = entity.SpouseLastName,
                        CaseStatus = entity.CaseStatus,
                        FinalStatus = entity.FinalStatus,
                        Status = entity.Status,
                    }).FirstOrDefault();
                }
                else
                {
                    throw new Exception("Invalid Candidate Code!");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public byte GetClientTATByCandidateId(short cid = 0)
        {
            try
            {
                if (cid > 0)
                {
                    //short cid = db.PQPersonals.Find(id).ClientRowID;
                    var cTatList = db.PQClientChecks.Where(c => c.ClientRowID == cid)
                        .Select(c => c.TAT)
                        .Union(db.PQClientPackages.Where(c => c.ClientRowID == cid)
                        .Select(c => c.TAT)).ToList().Max();

                    return cTatList;
                }
                return 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PQPersonalDetailViewModel GetCandidateNameOrCmpIDById(int PersonalRowID = 0)
        {
            try
            {
                PQPersonalDetailViewModel model = new PQPersonalDetailViewModel();
                //var entity = db.PQPersonals.Find(PersonalRowID);
                DataTable dt = new DataTable();
                dt = cc.GetPersonalInfo(PersonalRowID);

                if (dt.Rows.Count > 0)
                {
                    model.PersonalRowID = PersonalRowID;
                    model.ClientRowID = Convert.ToInt16(dt.Rows[0]["ClientRowID"].ToString());
                    model.ClientRefID = Convert.ToString(dt.Rows[0]["ClientRefID"]);
                    model.CompanyRefNo = Convert.ToString(dt.Rows[0]["CompanyRefNo"]);
                    //model.CandidateCode = entity.CandidateCode;
                    model.CandTitle = Convert.ToString(dt.Rows[0]["CandTitle"]);
                    model.Name = Convert.ToString(dt.Rows[0]["CandidateFullName"]);
                    // model.MiddleName = Convert.ToString(dt.Rows[0]["MiddleName"]);
                    //model.LastName = Convert.ToString(dt.Rows[0]["LastName"]);
                    model.CaseStatusId = Convert.ToString(dt.Rows[0]["CaseStausID"]);

                }
                else
                {
                    throw new Exception("Invalid Id!");
                }
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdatePQPersonalForQC(int PersonalRowID = 0, string CaseStatus = "")
        {
            try
            {
                if (PersonalRowID > 0)
                {
                    db.PQPersonals.Single(c => c.PersonalRowID == PersonalRowID).CaseStatus = CaseStatus;

                    db.PQPersonals.Single(c => c.PersonalRowID == PersonalRowID).DEScanCaseStatus = "Completed";
                    db.PQPersonals.Single(c => c.PersonalRowID == PersonalRowID).DEScanCompletedDate = DateTime.Now;

                    db.PQPersonals.Single(c => c.PersonalRowID == PersonalRowID).DECaseStatus = CaseStatus;
                    db.PQPersonals.Single(c => c.PersonalRowID == PersonalRowID).DEQCCaseStatus = CaseStatus;
                }
            }
            catch
            {
                throw;
            }
        }

        public void UpdatePQPersonalForDE(int PersonalRowID = 0, string CaseStatus = "")
        {
            try
            {
                if (PersonalRowID > 0)
                {
                    db.PQPersonals.Single(c => c.PersonalRowID == PersonalRowID).DECaseStatus = CaseStatus;
                    db.PQPersonals.Single(c => c.PersonalRowID == PersonalRowID).DECompletedDate = DateTime.Now;

                    db.PQPersonals.Single(c => c.PersonalRowID == PersonalRowID).DEQCCaseStatus = CaseStatus;
                }
            }
            catch
            {
                throw;
            }
        }

        public void UpdatePQPersonalForDEQualityCheck(int PersonalRowID = 0, string CaseStatus = "")
        {
            try
            {
                if (PersonalRowID > 0)
                {
                    db.PQPersonals.Single(c => c.PersonalRowID == PersonalRowID).DEQCCaseStatus = CaseStatus;
                    db.PQPersonals.Single(c => c.PersonalRowID == PersonalRowID).DEQCCompletedDate = DateTime.Now;
                }
            }
            catch
            {
                throw;
            }
        }
        public short GetClientDetailsByPersonalIdForB2C(int id = 0)
        {
            try
            {
                if (id > 0)
                {
                    var a = db.PQPersonals.Where(x => x.PersonalRowID == id).Select(x => x.ClientRowID).FirstOrDefault();
                    return a;
                }
                return 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public short GetClientDetailsByPersonalId(int id = 0)
        {
            try
            {
                if (id > 0)
                {
                    return db.PQPersonals.Find(id).ClientRowID;
                }
                return 0;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region ****** Temp Client Candidate *****

        public void AddTempPQPersonalCaseOfBulkUpload(AddTempPQPersonalForBulkUpViewModel model)
        {
            try
            {
                if (model != null)
                {
                    TempPQPersonal entity = new TempPQPersonal();
                    entity.ClientRowID = model.ClientRowID;
                    entity.ClientUserRowID = model.ClientUserRowID;
                    entity.CandidateUserID = model.CandidateUserID;
                    if (model.ClientRowID == 593)
                    {
                        entity.EmployeeID = model.ClientRefID;
                    }
                    else
                    {
                        entity.ClientRefID = model.ClientRefID;
                    }
                    //entity.CompanyRefNo = model.CompanyRefNo;
                    entity.CandidateCode = model.CandidateCode;
                    entity.CandTitle = model.CandTitle;
                    entity.Name = model.Name;
                    entity.MiddleName = model.MiddleName;
                    entity.LastName = model.LastName;
                    entity.MobileNo = model.MobileNo;
                    entity.EmailID = model.EmailID;
                    entity.FromType = model.FromType;
                    entity.DetailsEnteredBy = model.DetailsEnteredBy;
                    entity.CreatedBy = model.CreatedBy;
                    entity.CreatedDate = model.CreatedDate;
                    entity.Status = model.Status;
                    entity.OtherDetails = model.OtherDetails;
                    entity.OtherDetails1 = model.OtherDetails1;
                    entity.CaseStatus = model.CaseStatus;
                    entity.SSNNo = model.SSNNo;
                    entity.NationalSkills = model.NationalSkills;
                    entity.AlternateEmailID = model.AlternateEmailID;
                    entity.DOB = model.DOB;
                    entity.Nationality = model.Nationality;
                    if (entity.ClientRowID == 576)
                    {
                        entity.SSNNo = model.Band;
                        entity.CandidateType = model.CandidateType;
                    }
                    if (entity.ClientRowID == 778)
                    {
                        entity.ProcessName = model.ProcessName;
                        entity.BusinessName = model.BusinessName;
                    }
                    if (entity.ClientRowID == 735)
                    {
                        entity.ProcessName = model.ProcessName;
                        entity.SSNNo = model.Band;
                    }
                    db.TempPQPersonals.Add(entity);
                }
                else
                {
                    throw new Exception("PQPersonal could not be blank!");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        //public TempPQPersonalInfoListPagedModel GetTempCandidateListByClientBulkUpload(int pageNo, int pageSize, string sort, string sortDir, string Search = "",
        //    short ClientRowID = 0, int ClientUserRowID = 0)
        //{
        //    try
        //    {
        //        if (pageNo < 0)
        //        {
        //            pageNo = 1;
        //        }
        //        IQueryable<PQCandidateLogin> data = db.PQCandidateLogins.Include("TempPQPersonal").Include("PQClientMaster").
        //            Where(p => p.ClientRowID == ClientRowID && p.TempPQPersonal.ClientRowID == ClientRowID && p.TempPQPersonal.CaseStatus==1 && p.TempPQPersonal.Status == 1 && p.isDeleted==0);

        //        if (!string.IsNullOrEmpty(Search))
        //        {
        //            data = data.Where(b => b.CandidateName.ToString().Contains(Search) || b.TempPQPersonal.ClientRefID == Search || b.TempPQPersonal.CandidateCode == Search);
        //        }

        //        if (ClientUserRowID > 0)
        //        {
        //            data = data.Where(c => c.TempPQPersonal.ClientUserRowID == ClientUserRowID);
        //        }

        //        switch (sort)
        //        {
        //            case "name":
        //                data = sortDir == "asc" ? data.OrderBy(d => d.CandidateName) : data.OrderByDescending(d => d.CandidateName);
        //                break;
        //            default:
        //                data = sortDir == "asc" ? data.OrderBy(d => d.SentMailStatus) : data.OrderBy(d => d.SentMailStatus);
        //                break;
        //        }

        //        TempPQPersonalInfoListPagedModel model = new TempPQPersonalInfoListPagedModel();
        //        model.PageSize = pageSize;
        //        model.TotalRecords = data.Count();
        //        model.TempPQPersonalInfo = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new TempPQPersonalDetailViewModel
        //        {
        //            PersonalRowID = item.TempPQPersonal.PersonalRowID,
        //            ClientUserRowID = item.TempPQPersonal.ClientUserRowID,
        //            ClientRowID = item.ClientRowID,
        //            ClientName = item.PQClientMaster.MasterAbbreviation.ClientName + " " + item.PQClientMaster.MasterClientSubGroup.ClientSubGroupName,
        //            SendMailStatus = db.PQCandidateLogins.Where(t => t.TempPersonalRowID == item.TempPQPersonal.PersonalRowID).FirstOrDefault().SentMailStatus ,
        //            CandidateCode = item.TempPQPersonal.CandidateCode,
        //            CompanyRefNo = item.TempPQPersonal.CompanyRefNo,
        //            ClientRefID = item.TempPQPersonal.ClientRefID,
        //            Name = item.TempPQPersonal.CandTitle + " " + item.TempPQPersonal.Name + " " + item.TempPQPersonal.MiddleName + " " + item.TempPQPersonal.LastName,
        //            CreatedBy = item.TempPQPersonal.CreatedBy,
        //            CreatedDate = item.TempPQPersonal.CreatedDate,
        //            Status = item.TempPQPersonal.Status,
        //            CaseStatusId= item.TempPQPersonal.CaseStatus,
        //        }).ToList();
        //        return model;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}


        public TempPQPersonalInfoListPagedModel GetTempCandidateListByClientBulkUpload(int pageNo, int pageSize, string sort, string sortDir, string Search = "",
            short ClientRowID = 0, int ClientUserRowID = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                ClientCustomize cc = new ClientCustomize();
                DataTable dt = cc.GetTempCandidateListByClientBulkUpload(ClientRowID, Search, ClientUserRowID);
                TempPQPersonalInfoListPagedModel model = new TempPQPersonalInfoListPagedModel();


                List<TempPQPersonalDetailViewModel> lmd = new List<TempPQPersonalDetailViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        //lmd.Add(new PQPersonalDetailViewModel
                        //{

                        //                       pqp.PersonalRowID,pqp.ClientUserRowID,pqp.ClientRowID,'' as ClientName, pql.SentMailDate,CandidateCode,CompanyRefNo,ClientRefID,pqp.name,pqp.CreatedBy,
                        //pqp.CreatedDate,pqp.Status,pqp.CaseStatus
                        TempPQPersonalDetailViewModel bb = new TempPQPersonalDetailViewModel();
                        bb.PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]);
                        bb.ClientUserRowID = Convert.ToInt32(dr["ClientUserRowID"]);
                        bb.ClientRowID = Convert.ToInt16(dr["ClientRowID"]);
                        bb.ClientRefID = (dr["ClientRefID"].ToString());
                        bb.ClientName = (dr["ClientName"].ToString());
                        bb.Name = (dr["name"]).ToString();
                        bb.CandidateCode = (dr["CandidateCode"]).ToString();
                        bb.CompanyRefNo = (dr["CompanyRefNo"]).ToString();
                        bb.CaseStatusId = Convert.ToByte(dr["CaseStatus"]);
                        bb.CreatedBy = Convert.ToInt16(dr["CreatedBy"]);
                        bb.CreatedDate = Convert.ToDateTime(dr["CreatedDate"]);
                        bb.SendMailStatus = Convert.ToInt32(dr["SentMailStatus"]);
                        bb.PackageName = (dr["CandidateType"]).ToString();
                        bb.Entityname = (dr["SSNNo"]).ToString();


                        lmd.Add(bb);


                        //});

                    }
                    model.TotalRecords = dt.Rows.Count;
                    model.TempPQPersonalInfo = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                }
                else
                {
                    model.TotalRecords = 0;
                }


                model.PageSize = pageSize;






                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public TempPQPersonalForSendMailViewModel GetTempPQPersonalDetailsForSendMailById(int TempPersonalRowID = 0)
        {
            try
            {
                TempPQPersonalForSendMailViewModel model = new TempPQPersonalForSendMailViewModel();
                var entity = db.TempPQPersonals.Find(TempPersonalRowID);

                if (entity != null)
                {
                    model.PersonalRowID = entity.PersonalRowID;
                    model.ClientRowID = entity.ClientRowID;
                    model.ClientUserRowID = entity.ClientUserRowID;
                    model.CandidateUserID = entity.CandidateUserID;
                    model.ClientRefID = entity.ClientRefID;
                    model.CompanyRefNo = entity.CompanyRefNo;
                    model.CandTitle = entity.CandTitle;
                    model.Name = entity.Name;
                    model.MiddleName = entity.MiddleName;
                    model.LastName = entity.LastName;
                    model.Gender = entity.Gender;
                    model.MobileNo = entity.MobileNo;
                    model.EmailID = entity.EmailID;
                    model.AlternateEmail = entity.AlternateEmailID;
                    //model.OtherDetails = entity.OtherDetails;
                    model.OtherDetails2 = entity.OtherDetails2;
                    model.ProcessName = entity.ProcessName;
                    model.BusinessName = entity.BusinessName;
                }
                else
                {
                    throw new Exception("Invalid Id!");
                }
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateTempPQPersonalCandidateUserID(int PersonalRowID = 0, int CandidateUserID = 0)
        {
            try
            {
                db.TempPQPersonals.Single(c => c.PersonalRowID == PersonalRowID).CandidateUserID = CandidateUserID;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        public PQPersonalOrCheckForSendMailViewModel GetCheckOrPersonalDetailsForSendMailById(short ChFamilRowId = 0, short SubCheckRowID = 0, int CheckRowID = 0)
        {
            try
            {
                PQPersonalOrCheckForSendMailViewModel model = new PQPersonalOrCheckForSendMailViewModel();

                if (ChFamilRowId == 2)
                {
                    //pageName = "AddAddressInfo";
                    model = db.PQAddressStatus.Where(c => c.AddressRowID == CheckRowID && c.PQAddress.CheckFamilyRowID == ChFamilRowId && c.PQAddress.SubCheckRowID == SubCheckRowID).Select(entity => new PQPersonalOrCheckForSendMailViewModel
                    {
                        PersonalRowID = entity.PQAddress.PersonalRowID,
                        ClientRowID = entity.PQAddress.ClientRowID,
                        ClientUserRowID = entity.PQAddress.PQPersonal.ClientUserRowID,
                        //ClientName=entity.PQClientMaster.
                        CheckFamilyRowID = entity.PQAddress.CheckFamilyRowID,
                        CheckFamilyName = entity.PQAddress.MasterCheckFamily.CheckFamilyName,
                        SubCheckRowID = entity.PQAddress.SubCheckRowID,
                        SubCheckName = entity.PQAddress.MasterSubCheckFamily.SubCheckName,
                        InternalTAT = entity.PQAddress.InternalOutDate,
                        ClientRefID = entity.PQAddress.PQPersonal.ClientRefID,
                        CompanyRefNo = entity.PQAddress.PQPersonal.CompanyRefNo,
                        CandidateName = entity.PQAddress.PQPersonal.CandTitle + " " + entity.PQAddress.PQPersonal.Name + " " + entity.PQAddress.PQPersonal.MiddleName + " " + entity.PQAddress.PQPersonal.LastName,
                        Gender = entity.PQAddress.PQPersonal.Gender,
                        MobileNo = entity.PQAddress.PQPersonal.MobileNo,
                        EmailID = entity.PQAddress.PQPersonal.EmailID,
                        ClientEmailID = entity.PQAddress.PQClientMaster.CSpocEmailID,
                        SpocEmailID = entity.PQAddress.PQClientMaster.SpocEmailID,
                        Remarks = entity.Remarks,
                        InsuffRemarks = entity.InfSuffRaiseRemarks,
                        Verificationsource = entity.PQAddress.AV_Add,
                    }).FirstOrDefault();
                }
                if (ChFamilRowId == 12)
                {
                    //pageName = "AddEmploymentInfo";
                    model = db.PQEmploymentStatus.Where(c => c.EmploymentRowId == CheckRowID && c.PQEmployment.CheckFamilyRowID == ChFamilRowId && c.PQEmployment.SubCheckRowID == SubCheckRowID).Select(entity => new PQPersonalOrCheckForSendMailViewModel
                    {
                        PersonalRowID = entity.PQEmployment.PersonalRowID,
                        ClientRowID = entity.PQEmployment.ClientRowID,
                        ClientUserRowID = entity.PQEmployment.PQPersonal.ClientUserRowID,
                        //ClientName=entity.PQEmployment.PQClientMaster.
                        CheckFamilyRowID = entity.PQEmployment.CheckFamilyRowID,
                        CheckFamilyName = entity.PQEmployment.MasterCheckFamily.CheckFamilyName,
                        SubCheckRowID = entity.PQEmployment.SubCheckRowID,
                        SubCheckName = entity.PQEmployment.MasterSubCheckFamily.SubCheckName,
                        InternalTAT = entity.PQEmployment.InternalOutDate,
                        ClientRefID = entity.PQEmployment.PQPersonal.ClientRefID,
                        CompanyRefNo = entity.PQEmployment.PQPersonal.CompanyRefNo,
                        CandidateName = entity.PQEmployment.PQPersonal.CandTitle + " " + entity.PQEmployment.PQPersonal.Name + " " + entity.PQEmployment.PQPersonal.MiddleName + " " + entity.PQEmployment.PQPersonal.LastName,
                        Gender = entity.PQEmployment.PQPersonal.Gender,
                        MobileNo = entity.PQEmployment.PQPersonal.MobileNo,
                        EmailID = entity.PQEmployment.PQPersonal.EmailID,
                        ClientEmailID = entity.PQEmployment.PQClientMaster.CSpocEmailID,
                        SpocEmailID = entity.PQEmployment.PQClientMaster.SpocEmailID,
                        Remarks = entity.Remarks,
                        InsuffRemarks = entity.InfSuffRaiseRemarks,
                        Verificationsource = db.MasterEmployers.Where(a => a.EmployerRowID.ToString() == entity.PQEmployment.EV_OrganisationName).FirstOrDefault().CompanyName,


                    }).FirstOrDefault();
                }
                if (ChFamilRowId == 11)
                {
                    //pageName = "AddEducationInfo";
                    model = db.PQEducationStatus.Where(c => c.EducationRowId == CheckRowID && c.PQEducation.CheckFamilyRowID == ChFamilRowId && c.PQEducation.SubCheckRowID == SubCheckRowID).Select(entity => new PQPersonalOrCheckForSendMailViewModel
                    {
                        PersonalRowID = entity.PQEducation.PersonalRowID,
                        ClientRowID = entity.PQEducation.ClientRowID,
                        ClientUserRowID = entity.PQEducation.PQPersonal.ClientUserRowID,
                        //ClientName=entity.PQEducation.PQClientMaster.
                        CheckFamilyRowID = entity.PQEducation.CheckFamilyRowID,
                        CheckFamilyName = entity.PQEducation.MasterCheckFamily.CheckFamilyName,
                        SubCheckRowID = entity.PQEducation.SubCheckRowID,
                        SubCheckName = entity.PQEducation.MasterSubCheckFamily.SubCheckName,
                        InternalTAT = entity.PQEducation.InternalOutDate,
                        ClientRefID = entity.PQEducation.PQPersonal.ClientRefID,
                        CompanyRefNo = entity.PQEducation.PQPersonal.CompanyRefNo,
                        CandidateName = entity.PQEducation.PQPersonal.CandTitle + " " + entity.PQEducation.PQPersonal.Name + " " + entity.PQEducation.PQPersonal.MiddleName + " " + entity.PQEducation.PQPersonal.LastName,
                        Gender = entity.PQEducation.PQPersonal.Gender,
                        MobileNo = entity.PQEducation.PQPersonal.MobileNo,
                        EmailID = entity.PQEducation.PQPersonal.EmailID,
                        ClientEmailID = entity.PQEducation.PQClientMaster.CSpocEmailID,
                        SpocEmailID = entity.PQEducation.PQClientMaster.SpocEmailID,
                        Remarks = entity.Remarks,
                        InsuffRemarks = entity.InfSuffRaiseRemarks,
                        Verificationsource = db.MasterColleges.Where(a => a.CollegeRowID == entity.PQEducation.EV_Colg_Schl_Inst_Name).FirstOrDefault().CollegeName,
                    }).FirstOrDefault();
                }
                if (ChFamilRowId == 26)
                {
                    //pageName = "AddNationalIdentityInfo";
                    model = db.PQNationalIdentityStatus.Where(c => c.NationalIdentityRowID == CheckRowID && c.PQNationalIdentity.CheckFamilyRowID == ChFamilRowId && c.PQNationalIdentity.SubCheckRowID == SubCheckRowID).Select(entity => new PQPersonalOrCheckForSendMailViewModel
                    {
                        PersonalRowID = entity.PQNationalIdentity.PersonalRowID,
                        ClientRowID = entity.PQNationalIdentity.ClientRowID,
                        ClientUserRowID = entity.PQNationalIdentity.PQPersonal.ClientUserRowID,
                        //ClientName=entity.PQNationalIdentity.PQClientMaster.
                        CheckFamilyRowID = entity.PQNationalIdentity.CheckFamilyRowID,
                        CheckFamilyName = entity.PQNationalIdentity.MasterCheckFamily.CheckFamilyName,
                        SubCheckRowID = entity.PQNationalIdentity.SubCheckRowID,
                        SubCheckName = entity.PQNationalIdentity.MasterSubCheckFamily.SubCheckName,
                        InternalTAT = entity.PQNationalIdentity.InternalOutDate,
                        ClientRefID = entity.PQNationalIdentity.PQPersonal.ClientRefID,
                        CompanyRefNo = entity.PQNationalIdentity.PQPersonal.CompanyRefNo,
                        CandidateName = entity.PQNationalIdentity.PQPersonal.CandTitle + " " + entity.PQNationalIdentity.PQPersonal.Name + " " + entity.PQNationalIdentity.PQPersonal.MiddleName + " " + entity.PQNationalIdentity.PQPersonal.LastName,
                        Gender = entity.PQNationalIdentity.PQPersonal.Gender,
                        MobileNo = entity.PQNationalIdentity.PQPersonal.MobileNo,
                        EmailID = entity.PQNationalIdentity.PQPersonal.EmailID,
                        ClientEmailID = entity.PQNationalIdentity.PQClientMaster.CSpocEmailID,
                        SpocEmailID = entity.PQNationalIdentity.PQClientMaster.SpocEmailID,
                        Remarks = entity.Remarks,
                        InsuffRemarks = entity.InfSuffRaiseRemarks,
                        Verificationsource = entity.PQNationalIdentity.NIC_Type_Id_Card,
                    }).FirstOrDefault();
                }
                if (ChFamilRowId == 30)
                {
                    //pageName = "AddReferenceInfo";
                    model = db.PQReferenceStatus.Where(c => c.ReferenceRowID == CheckRowID && c.PQReference.CheckFamilyRowID == ChFamilRowId && c.PQReference.SubCheckRowID == SubCheckRowID).Select(entity => new PQPersonalOrCheckForSendMailViewModel
                    {
                        PersonalRowID = entity.PQReference.PersonalRowID,
                        ClientRowID = entity.PQReference.ClientRowID,
                        ClientUserRowID = entity.PQReference.PQPersonal.ClientUserRowID,
                        //ClientName=entity.PQReference.PQClientMaster.
                        CheckFamilyRowID = entity.PQReference.CheckFamilyRowID,
                        CheckFamilyName = entity.PQReference.MasterCheckFamily.CheckFamilyName,
                        SubCheckRowID = entity.PQReference.SubCheckRowID,
                        SubCheckName = entity.PQReference.MasterSubCheckFamily.SubCheckName,
                        InternalTAT = entity.PQReference.InternalOutDate,
                        ClientRefID = entity.PQReference.PQPersonal.ClientRefID,
                        CompanyRefNo = entity.PQReference.PQPersonal.CompanyRefNo,
                        CandidateName = entity.PQReference.PQPersonal.CandTitle + " " + entity.PQReference.PQPersonal.Name + " " + entity.PQReference.PQPersonal.MiddleName + " " + entity.PQReference.PQPersonal.LastName,
                        Gender = entity.PQReference.PQPersonal.Gender,
                        MobileNo = entity.PQReference.PQPersonal.MobileNo,
                        EmailID = entity.PQReference.PQPersonal.EmailID,
                        ClientEmailID = entity.PQReference.PQClientMaster.CSpocEmailID,
                        SpocEmailID = entity.PQReference.PQClientMaster.SpocEmailID,
                        Remarks = entity.Remarks,
                        InsuffRemarks = entity.InfSuffRaiseRemarks,
                        Verificationsource = entity.PQReference.RV_RefereeName,
                    }).FirstOrDefault();
                }
                if (ChFamilRowId == 5)
                {
                    //pageName = "AddCriminalInfo";
                    model = db.PQCriminalStatus.Where(c => c.CriminalRowId == CheckRowID && c.PQCriminal.CheckFamilyRowID == ChFamilRowId && c.PQCriminal.SubCheckRowID == SubCheckRowID).Select(entity => new PQPersonalOrCheckForSendMailViewModel
                    {
                        PersonalRowID = entity.PQCriminal.PersonalRowID,
                        ClientRowID = entity.PQCriminal.ClientRowID,
                        ClientUserRowID = entity.PQCriminal.PQPersonal.ClientUserRowID,
                        //ClientName=entity.PQCriminal.PQClientMaster.
                        CheckFamilyRowID = entity.PQCriminal.CheckFamilyRowID,
                        CheckFamilyName = entity.PQCriminal.MasterCheckFamily.CheckFamilyName,
                        SubCheckRowID = entity.PQCriminal.SubCheckRowID,
                        SubCheckName = entity.PQCriminal.MasterSubCheckFamily.SubCheckName,
                        InternalTAT = entity.PQCriminal.InternalOutDate,
                        ClientRefID = entity.PQCriminal.PQPersonal.ClientRefID,
                        CompanyRefNo = entity.PQCriminal.PQPersonal.CompanyRefNo,
                        CandidateName = entity.PQCriminal.PQPersonal.CandTitle + " " + entity.PQCriminal.PQPersonal.Name + " " + entity.PQCriminal.PQPersonal.MiddleName + " " + entity.PQCriminal.PQPersonal.LastName,
                        Gender = entity.PQCriminal.PQPersonal.Gender,
                        MobileNo = entity.PQCriminal.PQPersonal.MobileNo,
                        EmailID = entity.PQCriminal.PQPersonal.EmailID,
                        ClientEmailID = entity.PQCriminal.PQClientMaster.CSpocEmailID,
                        SpocEmailID = entity.PQCriminal.PQClientMaster.SpocEmailID,
                        Remarks = entity.Remarks,
                        InsuffRemarks = entity.InfSuffRaiseRemarks,
                        Verificationsource = entity.PQCriminal.CRV_Others6,
                    }).FirstOrDefault();
                }
                if (ChFamilRowId == 40)
                {
                    //pageName = "Add Specil Check Info";
                    model = db.PQSpecialCheckStatus.Where(c => c.SpecialCheckRowId == CheckRowID && c.PQSpecialCheck.CheckFamilyRowID == ChFamilRowId && c.PQSpecialCheck.SubCheckRowID == SubCheckRowID).Select(entity => new PQPersonalOrCheckForSendMailViewModel
                    {
                        PersonalRowID = entity.PQSpecialCheck.PersonalRowID,
                        ClientRowID = entity.PQSpecialCheck.ClientRowID,
                        ClientUserRowID = entity.PQSpecialCheck.PQPersonal.ClientUserRowID,
                        //ClientName=entity.PQCriminal.PQClientMaster.
                        CheckFamilyRowID = entity.PQSpecialCheck.CheckFamilyRowID,
                        CheckFamilyName = entity.PQSpecialCheck.MasterCheckFamily.CheckFamilyName,
                        SubCheckRowID = entity.PQSpecialCheck.SubCheckRowID,
                        SubCheckName = entity.PQSpecialCheck.MasterSubCheckFamily.SubCheckName,
                        InternalTAT = entity.PQSpecialCheck.InternalOutDate,
                        ClientRefID = entity.PQSpecialCheck.PQPersonal.ClientRefID,
                        CompanyRefNo = entity.PQSpecialCheck.PQPersonal.CompanyRefNo,
                        CandidateName = entity.PQSpecialCheck.PQPersonal.CandTitle + " " + entity.PQSpecialCheck.PQPersonal.Name + " " + entity.PQSpecialCheck.PQPersonal.MiddleName + " " + entity.PQSpecialCheck.PQPersonal.LastName,
                        Gender = entity.PQSpecialCheck.PQPersonal.Gender,
                        MobileNo = entity.PQSpecialCheck.PQPersonal.MobileNo,
                        EmailID = entity.PQSpecialCheck.PQPersonal.EmailID,
                        ClientEmailID = entity.PQSpecialCheck.PQClientMaster.CSpocEmailID,
                        SpocEmailID = entity.PQSpecialCheck.PQClientMaster.SpocEmailID,
                        Remarks = entity.Remarks,
                        InsuffRemarks = entity.InfSuffRaiseRemarks,
                        Verificationsource = "NA",
                    }).FirstOrDefault();
                }

                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PQPersonalOrCheckForSendMailViewModel GetCheckOrPersonalDetailsForSendMailByIdNew(short ChFamilRowId = 0, short SubCheckRowID = 0, int personalRowid = 0, int CheckStatusId = 0)
        {
            try
            {
                PQPersonalOrCheckForSendMailViewModel model = new PQPersonalOrCheckForSendMailViewModel();


                ClientCustomize cc = new ClientCustomize();
                DataTable dt = cc.GetInsuffDetails(personalRowid, SubCheckRowID, CheckStatusId);
                if (dt.Rows.Count > 0)
                {
                    model.PersonalRowID = Convert.ToInt32(dt.Rows[0]["PersonalRowID"].ToString());
                    model.ClientRowID = Convert.ToInt16(dt.Rows[0]["ClientRowID"].ToString());
                    model.ClientUserRowID = Convert.ToInt32(dt.Rows[0]["ClientUserRowID"].ToString());
                    //ClientName=entity.PQClientMaster.
                    model.CheckFamilyRowID = Convert.ToInt16(dt.Rows[0]["CheckFamilyRowID"].ToString());
                    model.CheckFamilyName = dt.Rows[0]["CheckFamilyName"].ToString();
                    model.SubCheckRowID = Convert.ToInt16(dt.Rows[0]["SubCheckRowID"].ToString());
                    model.SubCheckName = dt.Rows[0]["SubCheckName"].ToString();
                    //model.InternalTAT = dt.Rows[0]["internalTAT"].ToString();
                    model.ClientRefID = dt.Rows[0]["ClientRefID"].ToString();
                    model.CompanyRefNo = dt.Rows[0]["CompanyRefNo"].ToString();
                    model.CandidateName = dt.Rows[0]["CandidateName"].ToString();
                    model.Gender = dt.Rows[0]["Gender"].ToString();
                    model.MobileNo = dt.Rows[0]["MobileNo"].ToString();
                    model.EmailID = dt.Rows[0]["emailid"].ToString();
                    model.ClientEmailID = dt.Rows[0]["ClientEmailID"].ToString();
                    model.SpocEmailID = dt.Rows[0]["ClientEmailID"].ToString();
                    model.Remarks = dt.Rows[0]["Remarks"].ToString();

                    model.Tenure = dt.Columns.Contains("Tenure") && dt.Rows[0]["Tenure"] != DBNull.Value ? dt.Rows[0]["Tenure"].ToString() : "";
                    model.EmployerName = dt.Columns.Contains("EmployerName") && dt.Rows[0]["EmployerName"] != DBNull.Value ? dt.Rows[0]["EmployerName"].ToString() : "";
                    model.Qualification = dt.Columns.Contains("Qualification") && dt.Rows[0]["Qualification"] != DBNull.Value ? dt.Rows[0]["Qualification"].ToString() : "";
                    model.university = dt.Columns.Contains("university") && dt.Rows[0]["university"] != DBNull.Value ? dt.Rows[0]["university"].ToString() : "";
                    model.DocuemtRequired = dt.Columns.Contains("DocuemtRequired") && dt.Rows[0]["DocuemtRequired"] != DBNull.Value ? dt.Rows[0]["DocuemtRequired"].ToString() : "";
                    model.ExpectedTAT = dt.Columns.Contains("ExpectedTAT") && dt.Rows[0]["ExpectedTAT"] != DBNull.Value ? dt.Rows[0]["ExpectedTAT"].ToString() : "";
                    model.Amount = dt.Columns.Contains("Amount") && dt.Rows[0]["Amount"] != DBNull.Value ? dt.Rows[0]["Amount"].ToString() : "";


                    //model.InsuffRemarks = dt.Rows[0]["InsuffRemarks"].ToString();
                    model.Verificationsource = dt.Rows[0]["Verificationsource"].ToString();
                    model.CandidateIncludeInInsuffMail = Convert.ToInt32(dt.Rows[0]["CandidateIncludeInInsuffMail"].ToString());
                    model.locationName = Convert.ToString(dt.Rows[0]["locationName"].ToString());
                    model.AlternateEmailID = Convert.ToString(dt.Rows[0]["AlternateEmailID"].ToString());
                    model.verificationid= Convert.ToString(dt.Rows[0]["verificationid"].ToString());
                    model.PackageName = dt.Columns.Contains("PackageName") && dt.Rows[0]["PackageName"] != DBNull.Value ? dt.Rows[0]["PackageName"].ToString() : "";
                }
                return model;

            }
            catch (Exception)
            {
                throw;
            }
        }


        public IEnumerable<PQPersonalCheckListForClientViewModel> GetPersonalCheckListByPersonalId(int personalId = 0)
        {
            try
            {
                List<PQPersonalCheckListForClientViewModel> model = new List<PQPersonalCheckListForClientViewModel>();

                var CandidateChecks = db.PQCandidateChecks.Where(c => c.PersonalRowID == personalId).ToList();
                PQPersonalCheckListForClientViewModel entity;
                if (CandidateChecks != null && CandidateChecks.Count() > 0)
                {
                    foreach (var item in CandidateChecks)
                    {
                        if (item.ClientPackageRowID > 0)
                        {
                            entity = new PQPersonalCheckListForClientViewModel();

                            var data = db.PQClientPackages.Where(p => p.ClientPackageRowID == item.ClientPackageRowID).FirstOrDefault();

                            entity.PersonalRowID = item.PersonalRowID;
                            entity.ClientRowID = data.ClientRowID;
                            entity.CheckFamilyRowID = data.CheckFamilyRowID;
                            entity.CheckFamilyName = data.MasterCheckFamily.CheckFamilyName;
                            entity.SubCheckRowID = data.SubCheckRowID;
                            entity.SubCheckName = data.MasterSubCheckFamily.SubCheckName;
                            entity.CompanyRefNo = item.PQPersonal.CompanyRefNo;
                            entity.CandidateName = item.PQPersonal.CandTitle + " " + item.PQPersonal.Name + " " + item.PQPersonal.MiddleName + " " + item.PQPersonal.LastName;

                            model.Add(entity);
                        }
                        else if (item.ClientCheckRowID > 0)
                        {
                            entity = new PQPersonalCheckListForClientViewModel();
                            var data = db.PQClientChecks.Where(p => p.ClientCheckRowID == item.ClientCheckRowID).FirstOrDefault();

                            entity.PersonalRowID = item.PersonalRowID;
                            entity.ClientRowID = data.ClientRowID;
                            entity.CheckFamilyRowID = data.CheckFamilyRowID;
                            entity.CheckFamilyName = data.MasterCheckFamily.CheckFamilyName;
                            entity.SubCheckRowID = data.SubCheckRowID;
                            entity.SubCheckName = data.MasterSubCheckFamily.SubCheckName;
                            entity.CompanyRefNo = item.PQPersonal.CompanyRefNo;
                            entity.CandidateName = item.PQPersonal.CandTitle + " " + item.PQPersonal.Name + " " + item.PQPersonal.MiddleName + " " + item.PQPersonal.LastName;

                            model.Add(entity);
                        }
                    }
                }

                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<PQPersonalCheckListForClientViewModel> GetPersonalCheckListByPersonalId1(int personalId = 0)
        {
            try
            {
                List<PQPersonalCheckListForClientViewModel> model = new List<PQPersonalCheckListForClientViewModel>();
                DataSet ds = new DataSet();
                ds = cc.getCheckDetailClientDashboard(personalId);
                foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>
                {
                    PQPersonalCheckListForClientViewModel cc = new PQPersonalCheckListForClientViewModel();
                    //lmd.Add(new PQPersonalDetailViewModel
                    //{
                    cc.PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]);
                    cc.ClientRowID = Convert.ToInt16(dr["ClientRowID"]);
                    cc.CheckFamilyRowID = Convert.ToInt16(dr["CheckFamilyRowID"]);
                    cc.CheckFamilyName = (dr["CheckFamilyName"]).ToString();
                    cc.SubCheckRowID = Convert.ToInt16(dr["SubCheckRowID"]);
                    cc.SubCheckName = (dr["SubCheckName"]).ToString();
                    cc.CompanyRefNo = (dr["CompanyRefNo"]).ToString();
                    cc.CandidateName = (dr["CandidateName"]).ToString();
                    cc.ColorName = (dr["ColorName"]).ToString();
                    cc.CheckStatus = (dr["CheckStatus"]).ToString();
                    cc.CompleteDate = (dr["completedDate"]).ToString();
                    cc.Result = (dr["Result"]).ToString();
                    model.Add(cc);
                    //});

                }

                //var CandidateChecks = db.PQCandidateChecks.Where(c => c.PersonalRowID == personalId).ToList();
                //PQPersonalCheckListForClientViewModel entity;
                //if (CandidateChecks != null && CandidateChecks.Count() > 0)
                //{
                //    foreach (var item in CandidateChecks)
                //    {
                //        if (item.ClientPackageRowID > 0)
                //        {
                //            entity = new PQPersonalCheckListForClientViewModel();

                //            var data = db.PQClientPackages.Where(p => p.ClientPackageRowID == item.ClientPackageRowID).FirstOrDefault();

                //            entity.PersonalRowID = item.PersonalRowID;
                //            entity.ClientRowID = data.ClientRowID;
                //            entity.CheckFamilyRowID = data.CheckFamilyRowID;
                //            entity.CheckFamilyName = data.MasterCheckFamily.CheckFamilyName;
                //            entity.SubCheckRowID = data.SubCheckRowID;
                //            entity.SubCheckName = data.MasterSubCheckFamily.SubCheckName;
                //            entity.CompanyRefNo = item.PQPersonal.CompanyRefNo;
                //            entity.CandidateName = item.PQPersonal.CandTitle + " " + item.PQPersonal.Name + " " + item.PQPersonal.MiddleName + " " + item.PQPersonal.LastName;

                //            model.Add(entity);
                //        }
                //        else if (item.ClientCheckRowID > 0)
                //        {
                //            entity = new PQPersonalCheckListForClientViewModel();
                //            var data = db.PQClientChecks.Where(p => p.ClientCheckRowID == item.ClientCheckRowID).FirstOrDefault();

                //            entity.PersonalRowID = item.PersonalRowID;
                //            entity.ClientRowID = data.ClientRowID;
                //            entity.CheckFamilyRowID = data.CheckFamilyRowID;
                //            entity.CheckFamilyName = data.MasterCheckFamily.CheckFamilyName;
                //            entity.SubCheckRowID = data.SubCheckRowID;
                //            entity.SubCheckName = data.MasterSubCheckFamily.SubCheckName;
                //            entity.CompanyRefNo = item.PQPersonal.CompanyRefNo;
                //            entity.CandidateName = item.PQPersonal.CandTitle + " " + item.PQPersonal.Name + " " + item.PQPersonal.MiddleName + " " + item.PQPersonal.LastName;

                //            model.Add(entity);
                //        }
                //    }
                //}

                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region ******TempPQPersonal******

        public void AddTempPQPersonal(AddTempPQPersonalViewModel model)
        {
            try
            {
                if (model != null)
                {
                    TempPQPersonal entity = new TempPQPersonal();
                    entity.ClientRowID = model.ClientRowID;
                    entity.ClientUserRowID = model.ClientUserRowID;
                    entity.CandidateUserID = model.CandidateUserID;
                    entity.ClientRefID = model.ClientRefID;
                    entity.CompanyRefNo = model.CompanyRefNo;
                    entity.CandidateCode = model.CandidateCode;
                    entity.CandTitle = model.CandTitle;
                    entity.Name = model.Name;
                    entity.MiddleName = model.MiddleName;
                    entity.LastName = model.LastName;
                    entity.MobileNo = model.MobileNo;
                    entity.EmailID = model.EmailID;
                    entity.FromType = model.FromType;
                    entity.DetailsEnteredBy = model.DetailsEnteredBy;
                    entity.CreatedBy = model.CreatedBy;
                    entity.CreatedDate = model.CreatedDate;
                    entity.Status = model.Status;
                    entity.OtherDetails = model.OtherDetails;
                    entity.OtherDetails1 = model.OtherDetails1;
                    entity.CaseStatus = model.CaseStatus;
                    db.TempPQPersonals.Add(entity);
                }
                else
                {
                    throw new Exception("PQPersonal could not be blank!");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool IsTempPQPersonalExist(string ModileNo, string Email)
        {
            try
            {
                var TempPQPersonal = db.TempPQPersonals.Where(m => m.MobileNo.Trim().ToLower() == ModileNo.Trim().ToLower() && m.EmailID.Trim().ToLower() == Email.Trim().ToLower()).FirstOrDefault();
                if (TempPQPersonal != null && TempPQPersonal.EmailID.Trim().Length > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        //Get PersonalID and other fields for log transaction and checks package details save
        public UpdateTempPQPersonalViewModel GetTempPQPersonalByCandidateCode(string CandidateCode = "")
        {
            try
            {
                if (!string.IsNullOrEmpty(CandidateCode))
                {
                    return db.TempPQPersonals.Where(p => p.CandidateCode == CandidateCode).Select(entity => new UpdateTempPQPersonalViewModel
                    {
                        PersonalRowID = entity.PersonalRowID,
                        //CompanyRefNo = entity.CompanyRefNo,                       
                        //MobileNo = entity.MobileNo,
                        //AlternateNo = entity.AlternateNo,
                        //EmailID = entity.EmailID,
                        //AlternateEmailID = entity.AlternateEmailID,
                        //STD = entity.STD,
                        //ResidencePhoneNo = entity.ResidencePhoneNo,
                        //CandidateType = entity.CandidateType,
                        //Catagory = entity.Catagory,
                        //CaseType = entity.CaseType,
                        //TAT = entity.TAT,
                        //Nationality = entity.Nationality,
                        //EmployeeID = entity.EmployeeID,
                        //Designation = entity.Designation,
                        //PlaceofJoining = entity.PlaceofJoining,
                        //NationalSkills = entity.NationalSkills,
                        //SSNNo = entity.SSNNo,                       
                        //Remark = entity.Remark,                       
                        //FromType = entity.FromType,                       
                        //ProcessName = entity.ProcessName,
                        //BusinessName = entity.BusinessName,
                        //Type = entity.Type,
                        //CandidateUserID = entity.CandidateUserID,
                        //SpouseName = entity.SpouseName,
                        //SpouseMiddleName = entity.SpouseMiddleName,
                        //SpouseLastName = entity.SpouseLastName,
                        //CaseStatus = entity.CaseStatus,                      
                        //Status = entity.Status,
                    }).FirstOrDefault();
                }
                else
                {
                    throw new Exception("Invalid Candidate Code!");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public UpdateTempPQPersonalViewModel GetTempPQPersonalForUpdateById(int TempPersonalRowID = 0)
        {
            try
            {
                UpdateTempPQPersonalViewModel model = new UpdateTempPQPersonalViewModel();
                var entity = db.TempPQPersonals.Find(TempPersonalRowID);

                if (entity != null)
                {
                    model.PersonalRowID = entity.PersonalRowID;
                    model.ClientRowID = entity.ClientRowID;
                    model.ClientUserRowID = entity.ClientUserRowID;
                    model.CandidateUserID = entity.CandidateUserID;
                    model.ClientRefID = entity.ClientRefID;
                    model.CompanyRefNo = entity.CompanyRefNo;
                    model.CandidateCode = entity.CandidateCode;
                    model.CandTitle = entity.CandTitle;
                    model.Name = entity.Name;
                    model.MiddleName = entity.MiddleName;
                    model.LastName = entity.LastName;
                    model.FatherTitle = entity.FatherTitle;
                    model.FatherName = entity.FatherName;
                    model.FatherMiddleName = entity.FatherMiddleName;
                    model.FatherLastName = entity.FatherLastName;
                    model.Gender = entity.Gender;
                    model.DOB = entity.DOB;
                    model.MaritalStatus = entity.MaritalStatus;
                    model.MobileNo = entity.MobileNo;
                    model.AlternateNo = entity.AlternateNo;
                    model.EmailID = entity.EmailID;
                    model.AlternateEmailID = entity.AlternateEmailID;
                    model.STD = entity.STD;
                    model.ResidencePhoneNo = entity.ResidencePhoneNo;
                    model.PanCard = entity.PanCard;
                    model.PanCardNo = entity.PanCardNo;
                    model.Passport = entity.Passport;
                    model.PassportNo = entity.PassportNo;
                    model.AadhaarCard = entity.AadhaarCard;
                    model.AadhaarCardNo = entity.AadhaarCardNo;
                    model.DrivingLicense = entity.DrivingLicense;
                    model.DrivingLicenseNo = entity.DrivingLicenseNo;
                    model.VoterIDCard = entity.VoterIDCard;
                    model.VoterIDCardNo = entity.VoterIDCardNo;
                    if (model.AadhaarCard == 1)
                        model.NationalIDProof = "Aadhaar Card";
                    if (model.PanCard == 1)
                        model.NationalIDProof = "Pan Card";
                    if (model.Passport == 1)
                        model.NationalIDProof = "Passport";
                    if (model.DrivingLicense == 1)
                        model.NationalIDProof = "Driving License";
                    if (model.VoterIDCard == 1)
                        model.NationalIDProof = "Voter ID Card";

                    model.CheckDateTime = entity.CheckDateTime;//No
                    model.CandidateType = entity.CandidateType;
                    model.Catagory = entity.Catagory;
                    model.CaseType = entity.CaseType;
                    model.TAT = entity.TAT;
                    model.Nationality = entity.Nationality;
                    model.EmployeeID = entity.EmployeeID;
                    model.Designation = entity.Designation;
                    model.PlaceofJoining = entity.PlaceofJoining;
                    model.DateofJoining = entity.DateofJoining;
                    model.NationalSkills = entity.NationalSkills;
                    model.SSNNo = entity.SSNNo;
                    model.FromType = entity.FromType;//No
                    model.DetailsEnteredBy = entity.DetailsEnteredBy;//No
                    model.ProcessName = entity.ProcessName;
                    model.BusinessName = entity.BusinessName;
                    model.Type = entity.Type;
                    model.SpouseName = entity.SpouseName;
                    model.SpouseMiddleName = entity.SpouseMiddleName;
                    model.SpouseLastName = entity.SpouseLastName;
                    //model.OrderDate = entity.OrderDate;
                    //model.DeliveryDate = entity.DeliveryDate;
                    model.CreatedBy = entity.CreatedBy;
                    model.CreatedDate = entity.CreatedDate;
                    model.AllocatedBy = entity.AllocatedBy;
                    //model.AllocatedToDE = entity.AllocatedToDE;
                    //model.AllocatedToDEDate = entity.AllocatedToDEDate;
                    //model.AllocatedToDEQC = entity.AllocatedToDEQC;
                    //model.AllocatedToDEQCDate = entity.AllocatedToDEQCDate;
                    model.Remark = entity.Remark;
                    //model.FinalColor = entity.FinalColor;
                    //model.ExitIntrvwApplicable = entity.ExitIntrvwApplicable;
                    model.CaseStatus = entity.CaseStatus;
                    // model.FinalStatus = entity.FinalStatus;
                    model.Status = entity.Status;
                    model.OtherDetails = entity.OtherDetails;
                    model.OtherDetails1 = entity.OtherDetails1;
                    model.OtherDetails2 = entity.OtherDetails2;

                    DataTable dt = cc.GetVisibleLOA(entity.ClientRowID);
                    if (dt.Rows.Count > 0)
                    {
                        model.VisibleLOA = Convert.ToByte(dt.Rows[0]["VisibleLOA"]);
                    }

                }
                else
                {
                    throw new Exception("Invalid Id!");
                }
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdatePQPersonalForQC(short PersonalRowID = 0, string OtherDetails = null)
        {
            throw new NotImplementedException();
        }

        public void UpdateCandidateBySelf(UpdateTempPQPersonalViewModel model)
        {
            try
            {
                if (model != null && model.PersonalRowID > 0)
                {
                    //db.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).PersonalRowID = model.PersonalRowID;
                    //db.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).ClientRowID = model.ClientRowID;
                    //db.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).ClientUserRowID = model.ClientUserRowID;
                    //db.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).CandidateUserID = model.CandidateUserID;
                    db.TempPQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).ClientRefID = model.ClientRefID;
                    //db.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).CompanyRefNo = model.CompanyRefNo;
                    //db.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).CandidateCode = model.CandidateCode;
                    db.TempPQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).CandTitle = model.CandTitle;
                    db.TempPQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).Name = model.Name;
                    db.TempPQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).MiddleName = model.MiddleName;
                    db.TempPQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).LastName = model.LastName;
                    db.TempPQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).FatherTitle = model.FatherTitle;
                    db.TempPQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).FatherName = model.FatherName;
                    db.TempPQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).FatherMiddleName = model.FatherMiddleName;
                    db.TempPQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).FatherLastName = model.FatherLastName;
                    db.TempPQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).Gender = model.Gender;
                    db.TempPQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).DOB = model.DOB;
                    db.TempPQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).MaritalStatus = model.MaritalStatus;
                    db.TempPQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).MobileNo = model.MobileNo;
                    db.TempPQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).AlternateNo = model.AlternateNo;
                    db.TempPQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).EmailID = model.EmailID;
                    db.TempPQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).AlternateEmailID = model.AlternateEmailID;
                    db.TempPQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).STD = model.STD;
                    db.TempPQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).ResidencePhoneNo = model.ResidencePhoneNo;
                    db.TempPQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).PanCard = model.PanCard;
                    db.TempPQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).PanCardNo = model.PanCardNo;
                    db.TempPQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).Passport = model.Passport;
                    db.TempPQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).PassportNo = model.PassportNo;
                    db.TempPQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).AadhaarCard = model.AadhaarCard;
                    db.TempPQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).AadhaarCardNo = model.AadhaarCardNo;
                    db.TempPQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).DrivingLicense = model.DrivingLicense;
                    db.TempPQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).DrivingLicenseNo = model.DrivingLicenseNo;
                    db.TempPQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).VoterIDCard = model.VoterIDCard;
                    db.TempPQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).VoterIDCardNo = model.VoterIDCardNo;
                    //dTempPQPersonalsls.Single(c => c.PersonalRowID == model.PersonalRowID).CheckDateTime = model.CheckDateTime;//No
                    db.TempPQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).CandidateType = model.CandidateType;
                    db.TempPQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).Catagory = model.Catagory;
                    db.TempPQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).CaseType = model.CaseType;
                    db.TempPQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).TAT = model.TAT;
                    db.TempPQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).Nationality = model.Nationality;
                    db.TempPQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).EmployeeID = model.EmployeeID;
                    db.TempPQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).Designation = model.Designation;
                    db.TempPQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).PlaceofJoining = model.PlaceofJoining;
                    db.TempPQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).DateofJoining = model.DateofJoining;
                    db.TempPQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).NationalSkills = model.NationalSkills;
                    db.TempPQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).SSNNo = model.SSNNo;
                    //db.TempPQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).FromType = model.FromType;//No
                    //db.TempPQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).DetailsEnteredBy = model.DetailsEnteredBy;//No
                    if (model.ClientRowID == 647)
                    {
                        db.TempPQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).ProcessName = model.ProcessName;
                    }
                    //db.TempPQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).ProcessName = model.ProcessName;
                    //db.TempPQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).BusinessName = model.BusinessName;
                    //db.TempPQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).Type = model.Type;
                    db.TempPQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).SpouseName = model.SpouseName;
                    db.TempPQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).SpouseMiddleName = model.SpouseMiddleName;
                    db.TempPQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).SpouseLastName = model.SpouseLastName;
                    //db.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).CreatedBy = model.CreatedBy;
                    //db.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).CreatedDate = model.CreatedDate;
                    //db.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).AllocatedBy = model.AllocatedBy;
                    //db.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).AllocatedToDE = model.AllocatedToDE;
                    //db.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).AllocatedToDEDate = model.AllocatedToDEDate;
                    //db.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).AllocatedToDEQC = model.AllocatedToDEQC;
                    //db.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).AllocatedToDEQCDate = model.AllocatedToDEQCDate;
                    //db.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).Remark = model.Remark;
                    //db.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).FinalColor = model.FinalColor;
                    //db.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).ExitIntrvwApplicable = model.ExitIntrvwApplicable;
                    //db.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).CaseStatus = model.CaseStatus;
                    //db.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).FinalStatus = model.FinalStatus;
                    //db.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).Status = model.Status;
                    //db.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).OtherDetails = model.OtherDetails;
                    //db.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).OtherDetails1 = model.OtherDetails1;
                    db.TempPQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).OtherDetails2 = model.OtherDetails2;

                }
                else
                {
                    throw new Exception("Candidate could not be blank!");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public TempPQPersonalInfoListPagedModel GetTempCandidateListBySelf(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short ClientRowID = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }
                IQueryable<TempPQPersonal> data = db.TempPQPersonals.Include("PQClientMaster").Where(p => p.ClientRowID == ClientRowID && p.OtherDetails == "Pending" && p.OtherDetails1 == "Self");

                if (!string.IsNullOrEmpty(Search))
                {
                    data = data.Where(b => (b.CandTitle + " " + b.Name + " " + b.MiddleName + " " + b.LastName).ToString().Replace(" ", "").Contains(Search) || b.ClientRefID == Search);
                }

                switch (sort)
                {
                    case "clientname":
                        data = sortDir == "asc" ? data.OrderBy(d => d.PQClientMaster.MasterAbbreviation.ClientName) : data.OrderByDescending(d => d.PQClientMaster.MasterAbbreviation.ClientName);
                        break;
                    case "name":
                        data = sortDir == "asc" ? data.OrderBy(d => (d.Name + " " + d.MiddleName + " " + d.LastName)) : data.OrderByDescending(d => (d.Name + " " + d.MiddleName + " " + d.LastName));
                        break;
                    default:
                        data = sortDir == "asc" ? data.OrderByDescending(d => d.PersonalRowID) : data.OrderBy(d => d.PersonalRowID);
                        break;
                }

                TempPQPersonalInfoListPagedModel model = new TempPQPersonalInfoListPagedModel();
                model.PageSize = pageSize;
                model.TotalRecords = data.Count();
                model.TempPQPersonalInfo = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new TempPQPersonalDetailViewModel
                {
                    PersonalRowID = item.PersonalRowID,
                    ClientRowID = item.ClientRowID,
                    ClientName = item.PQClientMaster.MasterAbbreviation.ClientName + " " + item.PQClientMaster.MasterClientSubGroup.ClientSubGroupName,
                    CandidateCode = item.CandidateCode,
                    CompanyRefNo = item.CompanyRefNo,
                    ClientRefID = item.ClientRefID,
                    CandTitle = item.CandTitle,
                    Name = item.CandTitle + " " + item.Name + " " + item.MiddleName + " " + item.LastName,
                    MiddleName = item.MiddleName,
                    LastName = item.LastName,
                    CreatedBy = item.CreatedBy,
                    CreatedDate = item.CreatedDate,
                    Status = item.Status,
                }).ToList();
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PQPersonalInfoListPagedModel GetCandidateListWhoHaveDoneDataEntry(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", int ClientUserRowID = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }
                IQueryable<PQPersonal> data = db.PQPersonals.Include("PQClientMaster").Where(p => p.ClientRowID == ClientRowID && !string.IsNullOrEmpty(p.CompanyRefNo));

                if (ClientUserRowID > 0)
                {
                    data = data.Where(p => p.ClientUserRowID == ClientUserRowID);
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    data = data.Where(b => (b.CandTitle + " " + b.Name + " " + b.MiddleName + " " + b.LastName).ToString().Replace(" ", "").Contains(Search.Replace(" ", "")) || b.ClientRefID == Search.Replace(" ", "") || b.CompanyRefNo == Search.Replace(" ", ""));
                }
                if (!string.IsNullOrEmpty(RecievingToDate) && !string.IsNullOrEmpty(RecievingFromDate))
                {
                    DateTime startDate = Convert.ToDateTime(RecievingFromDate);
                    DateTime EndDate = Convert.ToDateTime(RecievingToDate);

                    data = data.Where(b => DbFunctions.TruncateTime(b.CreatedDate) >= startDate.Date && DbFunctions.TruncateTime(b.CreatedDate) <= EndDate.Date);
                }

                if (!string.IsNullOrEmpty(CompletedToDate) && !string.IsNullOrEmpty(CompletedFromDate))
                {
                    DateTime startDate = Convert.ToDateTime(CompletedFromDate);
                    DateTime EndDate = Convert.ToDateTime(CompletedToDate);
                    data = data.Where(b => DbFunctions.TruncateTime(b.DeliveryDate) >= startDate.Date && DbFunctions.TruncateTime(b.DeliveryDate) <= EndDate.Date);
                }
                switch (sort)
                {
                    case "name":
                        data = sortDir == "asc" ? data.OrderBy(d => (d.Name + " " + d.MiddleName + " " + d.LastName)) : data.OrderByDescending(d => (d.Name + " " + d.MiddleName + " " + d.LastName));
                        break;
                    default:
                        data = sortDir == "desc" ? data.OrderByDescending(d => d.PersonalRowID) : data.OrderBy(d => d.PersonalRowID);
                        break;
                }

                PQPersonalInfoListPagedModel model = new PQPersonalInfoListPagedModel();
                model.PageSize = pageSize;
                model.TotalRecords = data.Count();
                model.PQPersonalInfo = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new PQPersonalDetailViewModel
                {
                    PersonalRowID = item.PersonalRowID,
                    ClientRowID = item.ClientRowID,
                    ClientName = item.PQClientMaster.MasterAbbreviation.ClientName + " " + item.PQClientMaster.MasterClientSubGroup.ClientSubGroupName,
                    CandidateCode = item.CandidateCode,
                    CompanyRefNo = item.CompanyRefNo,
                    ClientRefID = item.ClientRefID,
                    CandTitle = item.CandTitle,
                    Name = item.CandTitle + " " + item.Name + " " + item.MiddleName + " " + item.LastName,
                    CreatedDate = item.CreatedDate,
                    CaseStatus = item.CaseStatus,
                    DetailsEnteredBy = item.DetailsEnteredBy,
                }).ToList();
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public PQPersonalInfoListPagedModel GetCandidateListWhoHaveDoneDataEntry1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", int ClientUserRowID = 0, string CaseStatus = "", int allClients = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                //IQueryable<PQPersonal> data = db.PQPersonals.Include("PQClientMaster").Where(p => p.ClientRowID == ClientRowID);
                DataSet ds = new DataSet();
                if (!string.IsNullOrEmpty(Search))
                {
                    ds = cc.getCasewiseSummaryDetail(ClientRowID, CaseStatus, ClientUserRowID, Search, allClients);
                }
                else
                {
                    ds = cc.getCasewiseSummaryDetail(ClientRowID, CaseStatus, ClientUserRowID, "", allClients);
                }


                List<PQPersonalDetailViewModel> lmd = new List<PQPersonalDetailViewModel>();


                foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>
                {
                    PQPersonalDetailViewModel cc = new PQPersonalDetailViewModel();
                    //lmd.Add(new PQPersonalDetailViewModel
                    //{
                    cc.PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]);
                    cc.ClientRowID = Convert.ToInt16(dr["ClientRowID"]);
                    cc.Name = (dr["Candidate Name"]).ToString();
                    cc.CandidateCode = (dr["CandidateCode"]).ToString();
                    if (dr["Securitas ID"].ToString() != "")
                        cc.CompanyRefNo = (dr["Securitas ID"]).ToString();
                    else
                        cc.CompanyRefNo = "";
                    cc.CaseStatus = (dr["Case Status"]).ToString();
                    cc.CreatedDate = Convert.ToDateTime(dr["Created On"]);
                    cc.ClientRefID = (dr["ClientRefID"]).ToString();
                    cc.ReportVisibality = Convert.ToBoolean(dr["ReportVisibality"]);
                    cc.IsReportUploaded = Convert.ToBoolean(dr["IsReportUploaded"]);
                    cc.ClientUserRowId = Convert.ToInt32(dr["clientuserrowid"]);
                    cc.PackageNamee = dr.Table.Columns.Contains("PackageNamee") && dr["PackageNamee"] != DBNull.Value
              ? dr["PackageNamee"].ToString()
              : "";
                    cc.EntityNamee = dr.Table.Columns.Contains("EntityNamee") && dr["EntityNamee"] != DBNull.Value
              ? dr["EntityNamee"].ToString()
              : "";


                    if (dr["casedocument"].ToString() != "")
                        cc.OtherDetails2 = (dr["casedocument"]).ToString();
                    else
                        cc.OtherDetails2 = "";
                    if (dr["SSNNo"].ToString() != "")
                        cc.Entity = (dr["SSNNo"]).ToString();

                    if (dr["CandidateType"].ToString() != "")
                        cc.Package = (dr["CandidateType"]).ToString();

                    if (dr["EmailID"].ToString() != "")
                        cc.Emailid = (dr["EmailID"]).ToString();

                    if (dr["MobileNo"].ToString() != "")
                        cc.MobileNo = (dr["MobileNo"]).ToString();

                    lmd.Add(cc);
                    //});

                }

                PQPersonalInfoListPagedModel model = new PQPersonalInfoListPagedModel();
                model.PageSize = pageSize;
                model.TotalRecords = ds.Tables[0].Rows.Count;
                model.PQPersonalInfo = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdatePQPersonalForBySelfTL(int PersonalRowID = 0, string CaseStatus = "")
        {
            try
            {
                if (PersonalRowID > 0)
                {
                    db.TempPQPersonals.Single(c => c.PersonalRowID == PersonalRowID).OtherDetails = CaseStatus;
                }
            }
            catch
            {
                throw;
            }
        }

        public TempPQPersonalDetailViewModel GetTempCandidateNameOrCmpIDById(int PersonalRowID = 0)
        {
            try
            {
                TempPQPersonalDetailViewModel model = new TempPQPersonalDetailViewModel();
                var entity = db.TempPQPersonals.Find(PersonalRowID);

                if (entity != null)
                {
                    model.PersonalRowID = entity.PersonalRowID;
                    model.ClientRowID = entity.ClientRowID;
                    model.ClientRefID = entity.ClientRefID;
                    model.CompanyRefNo = entity.CompanyRefNo;
                    model.CandidateCode = entity.CandidateCode;
                    model.CandTitle = entity.CandTitle;
                    model.Name = entity.Name;
                    model.MiddleName = entity.MiddleName;
                    model.LastName = entity.LastName;
                    //model.CreatedBy = entity.CreatedBy;
                    //model.CreatedDate = entity.CreatedDate;
                    //model.AllocatedBy = entity.AllocatedBy;
                    //model.AllocatedToDE = entity.AllocatedToDE;
                    //model.AllocatedToDEDate = entity.AllocatedToDEDate;
                    //model.AllocatedToDEQC = entity.AllocatedToDEQC;
                    //model.AllocatedToDEQCDate = entity.AllocatedToDEQCDate;
                    //model.Status = entity.Status;
                    //model.OtherDetails = entity.OtherDetails;
                    //model.OtherDetails1 = entity.OtherDetails1;
                    //model.OtherDetails2 = entity.OtherDetails2;
                }
                else
                {
                    throw new Exception("Invalid Id!");
                }
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }


        #endregion

        public PQPersonalInfoListPagedModel GetCandidateListByClientRowID(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short ClientRowID = 0, int ClientUserRowID = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }
                IQueryable<PQPersonal> data = db.PQPersonals.Include("PQClientMaster").Where(p => p.ClientRowID == ClientRowID && !string.IsNullOrEmpty(p.OtherDetails) && p.Status == 1);

                if (ClientUserRowID > 0)
                {
                    data = data.Where(p => p.ClientUserRowID == ClientUserRowID);
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    data = data.Where(b => (b.CandTitle + " " + b.Name + " " + b.MiddleName + " " + b.LastName).ToString().Replace(" ", "").Contains(Search) || b.CompanyRefNo == Search);
                }

                switch (sort)
                {
                    case "clientname":
                        data = sortDir == "asc" ? data.OrderBy(d => d.PQClientMaster.MasterAbbreviation.ClientName) : data.OrderByDescending(d => d.PQClientMaster.MasterAbbreviation.ClientName);
                        break;
                    case "name":
                        data = sortDir == "asc" ? data.OrderBy(d => (d.Name + " " + d.MiddleName + " " + d.LastName)) : data.OrderByDescending(d => (d.Name + " " + d.MiddleName + " " + d.LastName));
                        break;
                    default:
                        data = sortDir == "asc" ? data.OrderByDescending(d => d.PersonalRowID) : data.OrderBy(d => d.PersonalRowID);
                        break;
                }

                PQPersonalInfoListPagedModel model = new PQPersonalInfoListPagedModel();
                model.PageSize = pageSize;
                model.TotalRecords = data.Count();
                model.PQPersonalInfo = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new PQPersonalDetailViewModel
                {
                    PersonalRowID = item.PersonalRowID,
                    ClientRowID = item.ClientRowID,
                    ClientName = item.PQClientMaster.MasterAbbreviation.ClientName + " " + item.PQClientMaster.MasterClientSubGroup.ClientSubGroupName,
                    CandidateCode = item.CandidateCode,
                    CompanyRefNo = item.CompanyRefNo,
                    ClientRefID = item.ClientRefID,
                    CandTitle = item.CandTitle,
                    Name = item.CandTitle + " " + item.Name + " " + item.MiddleName + " " + item.LastName,
                    MiddleName = item.MiddleName,
                    LastName = item.LastName,
                    CreatedBy = item.CreatedBy,
                    CreatedDate = item.CreatedDate,
                    Status = item.Status,
                }).ToList();
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool IsCandidateEmailExist(string CandidateEmail)
        {
            try
            {
                var Candidate = db.TempPQPersonals.Where(c => c.EmailID.Trim().ToLower() == CandidateEmail.Trim().ToLower() && c.Status == 1).FirstOrDefault();
                if (Candidate != null && Candidate.EmailID.Length > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool IsCandidateEmailExist(string CandidateEmail, int clientrowid)
        {
            try
            {
                var Candidate = db.TempPQPersonals.Where(c => c.EmailID.Trim().ToLower() == CandidateEmail.Trim().ToLower() && c.Status == 1 && c.ClientRowID == clientrowid).FirstOrDefault();
                //if(clientrowid== 559)
                //{
                //    return false;
                //}
                //else 
                if (Candidate != null && Candidate.EmailID.Length > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        public TempClientHrApprovalCasesListPagedModel GetClientHrApprovalAllCases(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short ClientRowID = 0, int ClientUserRowID = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                IQueryable<TempPQPersonal> data = db.TempPQPersonals.Include("PQClientMaster").Include("MasterSubCheckFamily").Where(p => p.ClientRowID == ClientRowID && p.CaseStatus == 2);

                if (ClientUserRowID > 0)
                {
                    data = data.Where(p => p.ClientUserRowID == ClientUserRowID);
                }

                if (!string.IsNullOrEmpty(Search))
                {
                    data = data.Where(b => (b.CandTitle + " " + b.Name + " " + b.MiddleName + " " + b.LastName).ToString().Replace(" ", "").Contains(Search) || b.ClientRefID == Search);
                }

                switch (sort)
                {
                    default:
                        data = sortDir == "asc" ? data.OrderByDescending(d => d.PersonalRowID) : data.OrderBy(d => d.PersonalRowID);
                        break;
                }

                TempClientHrApprovalCasesListPagedModel model = new TempClientHrApprovalCasesListPagedModel();
                model.PageSize = pageSize;
                model.TotalRecords = data.Count();
                model.TempHrAprvlCases = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new TempClientHrApprovalCasesViewModel
                {
                    PersonalRowID = item.PersonalRowID,
                    ClientRowID = item.ClientRowID,
                    CandidateName = item.CandTitle + " " + item.Name + " " + item.MiddleName + " " + item.LastName,
                    ClientRefID = item.ClientRefID,
                    CompanyRefNo = item.CompanyRefNo,
                    FromType = item.FromType,
                    DetailsEnterdBy = item.DetailsEnteredBy,
                    CreatedDate = item.CreatedDate,
                    OtherDetails = item.OtherDetails,
                }).ToList();
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public TempClientHrApprovalCasesListPagedModel GetClientHrApprovalAllCases1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short ClientRowID = 0, int ClientUserRowID = 0, string CaseStatus = "", int allclient = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }
                DataSet ds = new DataSet();
                TempClientHrApprovalCasesListPagedModel model = new TempClientHrApprovalCasesListPagedModel();
                if (!string.IsNullOrEmpty(Search))
                {
                    ds = cc.getCasewiseSummaryDetail(ClientRowID, CaseStatus, ClientUserRowID, Search, allclient);
                }
                else
                {
                    ds = cc.getCasewiseSummaryDetail(ClientRowID, CaseStatus, ClientUserRowID, "", allclient);
                }
                // ds = cc.getCasewiseSummaryDetail(ClientRowID, CaseStatus);
                List<TempClientHrApprovalCasesViewModel> lmd = new List<TempClientHrApprovalCasesViewModel>();
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        lmd.Add(new TempClientHrApprovalCasesViewModel
                        {
                            PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]),
                            ClientRowID = Convert.ToInt16(dr["ClientRowID"]),
                            CandidateName = (dr["Candidate Name"]).ToString(),
                            CandidateCode = (dr["CandidateCode"]).ToString(),
                            CompanyRefNo = (dr["Securitas ID"]).ToString(),
                            // CaseStatus = (dr["Case Status"]).ToString(),
                            CreatedDate = Convert.ToDateTime(dr["Created On"]),
                            SentMailDate = Convert.ToDateTime(dr["SentMailDate"]),
                            SentMailStatus = Convert.ToInt32(dr["SentMailStatus"]),
                            ClientRefID = (dr["ClientRefID"]).ToString(),
                            Entity = (dr["SSNNo"]).ToString(),
                            Emailid = (dr["EmailID"]).ToString(),
                            MobileNo = (dr["MobileNo"]).ToString(),
                            Package = (dr["CandidateType"]).ToString(),
                            PackageName = dr.Table.Columns.Contains("PackageNamee") && dr["PackageNamee"] != DBNull.Value
              ? dr["PackageNamee"].ToString()
              : "",
                            HRRecruiterName = dr.Table.Columns.Contains("RecruiterName") && dr["RecruiterName"] != DBNull.Value
              ? dr["RecruiterName"].ToString()
              : "",

                        });
                    }
                    model.PageSize = pageSize;
                    model.TotalRecords = ds.Tables[0].Rows.Count;
                    model.TempHrAprvlCases = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                }
                else
                {
                    model.TotalRecords = 0;
                }

                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public PQAllChecksCountViewModel GetAllInsuffChecksCountForDETL(short teamMemberRowID = 0)
        {
            PQAllChecksCountViewModel model = new PQAllChecksCountViewModel();
            try
            {
                model.AddressCount = db.PQAddressStatus.Where(a => a.DETLAllocatedTo == teamMemberRowID && (a.DETMQCCheckStatus == "Insufficent-QC" || a.TMQCCheckStatus == "Insufficent-QC") && a.InfSuffClearBy == 0 && !string.IsNullOrEmpty(a.InfSuffClearComments)).ToList().Count();

                model.EmploymentCount = db.PQEmploymentStatus.Where(a => a.DETLAllocatedTo == teamMemberRowID && (a.DETMQCCheckStatus == "Insufficent-QC" || a.TMQCCheckStatus == "Insufficent-QC") && a.InfSuffClearBy == 0 && !string.IsNullOrEmpty(a.InfSuffClearComments)).ToList().Count();

                model.EducationCount = db.PQEducationStatus.Where(a => a.DETLAllocatedTo == teamMemberRowID && (a.DETMQCCheckStatus == "Insufficent-QC" || a.TMQCCheckStatus == "Insufficent-QC") && a.InfSuffClearBy == 0 && !string.IsNullOrEmpty(a.InfSuffClearComments)).ToList().Count();

                model.CriminalCount = db.PQCriminalStatus.Where(a => a.DETLAllocatedTo == teamMemberRowID && (a.DETMQCCheckStatus == "Insufficent-QC" || a.TMQCCheckStatus == "Insufficent-QC") && a.InfSuffClearBy == 0 && !string.IsNullOrEmpty(a.InfSuffClearComments)).ToList().Count();

                model.ReferenceCount = db.PQReferenceStatus.Where(a => a.DETLAllocatedTo == teamMemberRowID && (a.DETMQCCheckStatus == "Insufficent-QC" || a.TMQCCheckStatus == "Insufficent-QC") && a.InfSuffClearBy == 0 && !string.IsNullOrEmpty(a.InfSuffClearComments)).ToList().Count();

                model.NatIdentityCount = db.PQNationalIdentityStatus.Where(a => a.DETLAllocatedTo == teamMemberRowID && (a.DETMQCCheckStatus == "Insufficent-QC" || a.TMQCCheckStatus == "Insufficent-QC") && a.InfSuffClearBy == 0 && !string.IsNullOrEmpty(a.InfSuffClearComments)).ToList().Count();

                model.SpecialcheckCount = db.PQSpecialCheckStatus.Where(a => a.DETLAllocatedTo == teamMemberRowID && (a.DETMQCCheckStatus == "Insufficent-QC" || a.TMQCCheckStatus == "Insufficent-QC") && a.InfSuffClearBy == 0 && !string.IsNullOrEmpty(a.InfSuffClearComments)).ToList().Count();

                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PQAllChecksCountViewModel GetAllInsuffChecksCountForDETL1()
        {
            PQAllChecksCountViewModel model = new PQAllChecksCountViewModel();
            ClientCustomize cc = new ClientCustomize();
            try
            {
                DataTable DT = cc.GetAllInsuffChecksCountForDETL();
                if (DT.Rows.Count > 0)
                {
                    model.AddressCount = Convert.ToInt32(DT.Rows[0]["Address Verification"].ToString());
                    model.EmploymentCount = Convert.ToInt32(DT.Rows[0]["Employment Verification"].ToString());
                    model.EducationCount = Convert.ToInt32(DT.Rows[0]["Education Verification"].ToString());
                    model.CriminalCount = Convert.ToInt32(DT.Rows[0]["Criminal Record Verification"].ToString());
                    model.ReferenceCount = Convert.ToInt32(DT.Rows[0]["Reference Verification"].ToString());
                    model.NatIdentityCount = Convert.ToInt32(DT.Rows[0]["National Identity Check"].ToString());
                    model.SpecialcheckCount = Convert.ToInt32(DT.Rows[0]["Special Checks"].ToString());
                }

                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public PQAllChecksCountViewModel GetAllInsuffChecksCountForClient(short clientRowId = 0, int ClientUserRowID = 0)
        {
            PQAllChecksCountViewModel model = new PQAllChecksCountViewModel();
            try
            {
                if (ClientUserRowID > 0)
                {
                    model.AddressCount = db.PQAddressStatus.Where(a => (a.DETMQCCheckStatus == "Insufficent-QC" || a.TMQCCheckStatus == "Insufficent-QC") && a.InfSuffClearBy == 0 && a.PQAddress.ClientRowID == clientRowId && string.IsNullOrEmpty(a.InfSuffClearComments) && a.PQAddress.PQPersonal.ClientUserRowID == ClientUserRowID).ToList().Count();

                    model.EmploymentCount = db.PQEmploymentStatus.Where(a => (a.DETMQCCheckStatus == "Insufficent-QC" || a.TMQCCheckStatus == "Insufficent-QC") && a.InfSuffClearBy == 0 && a.PQEmployment.ClientRowID == clientRowId && string.IsNullOrEmpty(a.InfSuffClearComments) && a.PQEmployment.PQPersonal.ClientUserRowID == ClientUserRowID).ToList().Count();

                    model.EducationCount = db.PQEducationStatus.Where(a => (a.DETMQCCheckStatus == "Insufficent-QC" || a.TMQCCheckStatus == "Insufficent-QC") && a.InfSuffClearBy == 0 && a.PQEducation.ClientRowID == clientRowId && string.IsNullOrEmpty(a.InfSuffClearComments) && a.PQEducation.PQPersonal.ClientUserRowID == ClientUserRowID).ToList().Count();

                    model.CriminalCount = db.PQCriminalStatus.Where(a => (a.DETMQCCheckStatus == "Insufficent-QC" || a.TMQCCheckStatus == "Insufficent-QC") && a.InfSuffClearBy == 0 && a.PQCriminal.ClientRowID == clientRowId && string.IsNullOrEmpty(a.InfSuffClearComments) && a.PQCriminal.PQPersonal.ClientUserRowID == ClientUserRowID).ToList().Count();

                    model.ReferenceCount = db.PQReferenceStatus.Where(a => (a.DETMQCCheckStatus == "Insufficent-QC" || a.TMQCCheckStatus == "Insufficent-QC") && a.InfSuffClearBy == 0 && a.PQReference.ClientRowID == clientRowId && string.IsNullOrEmpty(a.InfSuffClearComments) && a.PQReference.PQPersonal.ClientUserRowID == ClientUserRowID).ToList().Count();

                    model.NatIdentityCount = db.PQNationalIdentityStatus.Where(a => (a.DETMQCCheckStatus == "Insufficent-QC" || a.TMQCCheckStatus == "Insufficent-QC") && a.InfSuffClearBy == 0 && a.PQNationalIdentity.ClientRowID == clientRowId && string.IsNullOrEmpty(a.InfSuffClearComments) && a.PQNationalIdentity.PQPersonal.ClientUserRowID == ClientUserRowID).ToList().Count();

                    model.SpecialcheckCount = db.PQSpecialCheckStatus.Where(a => (a.DETMQCCheckStatus == "Insufficent-QC" || a.TMQCCheckStatus == "Insufficent-QC") && a.InfSuffClearBy == 0 && a.PQSpecialCheck.ClientRowID == clientRowId && string.IsNullOrEmpty(a.InfSuffClearComments) && a.PQSpecialCheck.PQPersonal.ClientUserRowID == ClientUserRowID).ToList().Count();
                }
                else
                {
                    model.AddressCount = db.PQAddressStatus.Where(a => (a.DETMQCCheckStatus == "Insufficent-QC" || a.TMQCCheckStatus == "Insufficent-QC") && a.InfSuffClearBy == 0 && a.PQAddress.ClientRowID == clientRowId && string.IsNullOrEmpty(a.InfSuffClearComments)).ToList().Count();

                    model.EmploymentCount = db.PQEmploymentStatus.Where(a => (a.DETMQCCheckStatus == "Insufficent-QC" || a.TMQCCheckStatus == "Insufficent-QC") && a.InfSuffClearBy == 0 && a.PQEmployment.ClientRowID == clientRowId && string.IsNullOrEmpty(a.InfSuffClearComments)).ToList().Count();

                    model.EducationCount = db.PQEducationStatus.Where(a => (a.DETMQCCheckStatus == "Insufficent-QC" || a.TMQCCheckStatus == "Insufficent-QC") && a.InfSuffClearBy == 0 && a.PQEducation.ClientRowID == clientRowId && string.IsNullOrEmpty(a.InfSuffClearComments)).ToList().Count();

                    model.CriminalCount = db.PQCriminalStatus.Where(a => (a.DETMQCCheckStatus == "Insufficent-QC" || a.TMQCCheckStatus == "Insufficent-QC") && a.InfSuffClearBy == 0 && a.PQCriminal.ClientRowID == clientRowId && string.IsNullOrEmpty(a.InfSuffClearComments)).ToList().Count();

                    model.ReferenceCount = db.PQReferenceStatus.Where(a => (a.DETMQCCheckStatus == "Insufficent-QC" || a.TMQCCheckStatus == "Insufficent-QC") && a.InfSuffClearBy == 0 && a.PQReference.ClientRowID == clientRowId && string.IsNullOrEmpty(a.InfSuffClearComments)).ToList().Count();

                    model.NatIdentityCount = db.PQNationalIdentityStatus.Where(a => (a.DETMQCCheckStatus == "Insufficent-QC" || a.TMQCCheckStatus == "Insufficent-QC") && a.InfSuffClearBy == 0 && a.PQNationalIdentity.ClientRowID == clientRowId && string.IsNullOrEmpty(a.InfSuffClearComments)).ToList().Count();

                    model.SpecialcheckCount = db.PQSpecialCheckStatus.Where(a => (a.DETMQCCheckStatus == "Insufficent-QC" || a.TMQCCheckStatus == "Insufficent-QC") && a.InfSuffClearBy == 0 && a.PQSpecialCheck.ClientRowID == clientRowId && string.IsNullOrEmpty(a.InfSuffClearComments)).ToList().Count();
                }

                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PQAllChecksCountViewModel GetAllInsuffChecksCountForClient1(short clientRowId = 0, int ClientUserRowID = 0, int allclints = 0)
        {
            PQAllChecksCountViewModel model = new PQAllChecksCountViewModel();
            try
            {
                DataSet ds = new DataSet();
                ds = cc.getCheckwiseinsuffSummaryDashboard(clientRowId, ClientUserRowID, allclints);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    return new PQAllChecksCountViewModel
                    {
                        AddressCount = int.Parse(ds.Tables[0].Rows[0]["Address Verification"].ToString()),
                        EmploymentCount = int.Parse(ds.Tables[0].Rows[0]["Employment Verification"].ToString()),
                        EducationCount = int.Parse(ds.Tables[0].Rows[0]["Education Verification"].ToString()),
                        CriminalCount = int.Parse(ds.Tables[0].Rows[0]["Criminal Record Verification"].ToString()),
                        NatIdentityCount = int.Parse(ds.Tables[0].Rows[0]["National Identity Check"].ToString()),
                        ReferenceCount = int.Parse(ds.Tables[0].Rows[0]["Reference Verification"].ToString()),
                        SpecialcheckCount = int.Parse(ds.Tables[0].Rows[0]["Special Checks"].ToString())
                    };
                }
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PQAllChecksCountViewModel GetAllInsuffChecksCountForQC()
        {
            PQAllChecksCountViewModel model = new PQAllChecksCountViewModel();
            try
            {
                DataSet ds = new DataSet();
                ds = cc.getCheckwiseinsuffSummaryDashboardDEQC();
                return new PQAllChecksCountViewModel
                {
                    AddressCount = int.Parse(ds.Tables[0].Rows[0]["Address Verification"].ToString()),
                    EmploymentCount = int.Parse(ds.Tables[0].Rows[0]["Employment Verification"].ToString()),
                    EducationCount = int.Parse(ds.Tables[0].Rows[0]["Education Verification"].ToString()),
                    CriminalCount = int.Parse(ds.Tables[0].Rows[0]["Criminal Record Verification"].ToString()),
                    NatIdentityCount = int.Parse(ds.Tables[0].Rows[0]["National Identity Check"].ToString()),
                    ReferenceCount = int.Parse(ds.Tables[0].Rows[0]["Reference Verification"].ToString()),
                    SpecialcheckCount = int.Parse(ds.Tables[0].Rows[0]["Special Checks"].ToString())
                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PQAllChecksCountViewModel GetAllInsuffChecksCountForCandidate(int PersonalId = 0)
        {
            PQAllChecksCountViewModel model = new PQAllChecksCountViewModel();
            try
            {
                model.AddressCount = db.PQAddressStatus.Where(a => (a.DETMQCCheckStatus == "Insufficent-QC" || a.TMQCCheckStatus == "Insufficent-QC") && a.InfSuffClearBy == 0 && a.PQAddress.PersonalRowID == PersonalId && string.IsNullOrEmpty(a.InfSuffClearComments)).ToList().Count();

                model.EmploymentCount = db.PQEmploymentStatus.Where(a => (a.DETMQCCheckStatus == "Insufficent-QC" || a.TMQCCheckStatus == "Insufficent-QC") && a.InfSuffClearBy == 0 && a.PQEmployment.PersonalRowID == PersonalId && string.IsNullOrEmpty(a.InfSuffClearComments)).ToList().Count();

                model.EducationCount = db.PQEducationStatus.Where(a => (a.DETMQCCheckStatus == "Insufficent-QC" || a.TMQCCheckStatus == "Insufficent-QC") && a.InfSuffClearBy == 0 && a.PQEducation.PersonalRowID == PersonalId && string.IsNullOrEmpty(a.InfSuffClearComments)).ToList().Count();

                model.CriminalCount = db.PQCriminalStatus.Where(a => (a.DETMQCCheckStatus == "Insufficent-QC" || a.TMQCCheckStatus == "Insufficent-QC") && a.InfSuffClearBy == 0 && a.PQCriminal.PersonalRowID == PersonalId && string.IsNullOrEmpty(a.InfSuffClearComments)).ToList().Count();

                model.ReferenceCount = db.PQReferenceStatus.Where(a => (a.DETMQCCheckStatus == "Insufficent-QC" || a.TMQCCheckStatus == "Insufficent-QC") && a.InfSuffClearBy == 0 && a.PQReference.PersonalRowID == PersonalId && string.IsNullOrEmpty(a.InfSuffClearComments)).ToList().Count();

                model.NatIdentityCount = db.PQNationalIdentityStatus.Where(a => (a.DETMQCCheckStatus == "Insufficent-QC" || a.TMQCCheckStatus == "Insufficent-QC") && a.InfSuffClearBy == 0 && a.PQNationalIdentity.PersonalRowID == PersonalId && string.IsNullOrEmpty(a.InfSuffClearComments)).ToList().Count();

                model.SpecialcheckCount = db.PQSpecialCheckStatus.Where(a => (a.DETMQCCheckStatus == "Insufficent-QC" || a.TMQCCheckStatus == "Insufficent-QC") && a.InfSuffClearBy == 0 && a.PQSpecialCheck.PersonalRowID == PersonalId && string.IsNullOrEmpty(a.InfSuffClearComments)).ToList().Count();

                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PQAllChecksCountViewModel GetAllInsuffChecksCountForCSSpoc(string SpocEmail = "")
        {
            PQAllChecksCountViewModel model = new PQAllChecksCountViewModel();
            try
            {
                var clientRowIds = db.PQClientMasters.Where(c => c.SpocEmailID.Contains(SpocEmail)).Select(c => c.ClientRowID).ToList();

                model.AddressCount = db.PQAddressStatus.Where(a => (a.DETMQCCheckStatus == "Insufficent-QC" || a.TMQCCheckStatus == "Insufficent-QC") && a.InfSuffClearBy == 0 && clientRowIds.Contains(a.PQAddress.ClientRowID) && string.IsNullOrEmpty(a.InfSuffClearComments)).ToList().Count();

                model.EmploymentCount = db.PQEmploymentStatus.Where(a => (a.DETMQCCheckStatus == "Insufficent-QC" || a.TMQCCheckStatus == "Insufficent-QC") && a.InfSuffClearBy == 0 && clientRowIds.Contains(a.PQEmployment.ClientRowID) && string.IsNullOrEmpty(a.InfSuffClearComments)).ToList().Count();

                model.EducationCount = db.PQEducationStatus.Where(a => (a.DETMQCCheckStatus == "Insufficent-QC" || a.TMQCCheckStatus == "Insufficent-QC") && a.InfSuffClearBy == 0 && clientRowIds.Contains(a.PQEducation.ClientRowID) && string.IsNullOrEmpty(a.InfSuffClearComments)).ToList().Count();

                model.CriminalCount = db.PQCriminalStatus.Where(a => (a.DETMQCCheckStatus == "Insufficent-QC" || a.TMQCCheckStatus == "Insufficent-QC") && a.InfSuffClearBy == 0 && clientRowIds.Contains(a.PQCriminal.ClientRowID) && string.IsNullOrEmpty(a.InfSuffClearComments)).ToList().Count();

                model.ReferenceCount = db.PQReferenceStatus.Where(a => (a.DETMQCCheckStatus == "Insufficent-QC" || a.TMQCCheckStatus == "Insufficent-QC") && a.InfSuffClearBy == 0 && clientRowIds.Contains(a.PQReference.ClientRowID) && string.IsNullOrEmpty(a.InfSuffClearComments)).ToList().Count();

                model.NatIdentityCount = db.PQNationalIdentityStatus.Where(a => (a.DETMQCCheckStatus == "Insufficent-QC" || a.TMQCCheckStatus == "Insufficent-QC") && a.InfSuffClearBy == 0 && clientRowIds.Contains(a.PQNationalIdentity.ClientRowID) && string.IsNullOrEmpty(a.InfSuffClearComments)).ToList().Count();

                model.SpecialcheckCount = db.PQSpecialCheckStatus.Where(a => (a.DETMQCCheckStatus == "Insufficent-QC" || a.TMQCCheckStatus == "Insufficent-QC") && a.InfSuffClearBy == 0 && clientRowIds.Contains(a.PQSpecialCheck.ClientRowID) && string.IsNullOrEmpty(a.InfSuffClearComments)).ToList().Count();

                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PQAllChecksCountViewModel GetAllInsuffChecksCountForCSSpoc1(int clientRowId, string SpocEmail = "")
        {
            PQAllChecksCountViewModel model = new PQAllChecksCountViewModel();
            ClientCustomize cc = new ClientCustomize();
            try
            {
                DataTable DT = cc.GetAllInsuffChecksCountForCsSpoc(SpocEmail, clientRowId);
                if (DT.Rows.Count > 0)
                {
                    model.AddressCount = Convert.ToInt32(DT.Rows[0]["Address Verification"].ToString());
                    model.EmploymentCount = Convert.ToInt32(DT.Rows[0]["Employment Verification"].ToString());
                    model.EducationCount = Convert.ToInt32(DT.Rows[0]["Education Verification"].ToString());
                    model.CriminalCount = Convert.ToInt32(DT.Rows[0]["Criminal Record Verification"].ToString());
                    model.ReferenceCount = Convert.ToInt32(DT.Rows[0]["Reference Verification"].ToString());
                    model.NatIdentityCount = Convert.ToInt32(DT.Rows[0]["National Identity Check"].ToString());
                    model.SpecialcheckCount = Convert.ToInt32(DT.Rows[0]["Special Checks"].ToString());
                }

                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public PQAllChecksCountViewModel GetAllInsuffChecksCountForCSSpoc1_MultipleClient(DataTable clientRowId, string SpocEmail = "")
        {
            PQAllChecksCountViewModel model = new PQAllChecksCountViewModel();
            ClientCustomize cc = new ClientCustomize();
            try
            {
                DataTable DT = cc.GetAllInsuffChecksCountForCsSpoc_MultipleClient(SpocEmail, clientRowId);
                if (DT.Rows.Count > 0)
                {
                    model.AddressCount = Convert.ToInt32(DT.Rows[0]["Address Verification"].ToString());
                    model.EmploymentCount = Convert.ToInt32(DT.Rows[0]["Employment Verification"].ToString());
                    model.EducationCount = Convert.ToInt32(DT.Rows[0]["Education Verification"].ToString());
                    model.CriminalCount = Convert.ToInt32(DT.Rows[0]["Criminal Record Verification"].ToString());
                    model.ReferenceCount = Convert.ToInt32(DT.Rows[0]["Reference Verification"].ToString());
                    model.NatIdentityCount = Convert.ToInt32(DT.Rows[0]["National Identity Check"].ToString());
                    model.SpecialcheckCount = Convert.ToInt32(DT.Rows[0]["Special Checks"].ToString());
                }

                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PQPersonalSessionViewModel GetPQPersonalInfoForSession(int PersonalRowID = 0)
        {
            try
            {
                PQPersonalSessionViewModel model = new PQPersonalSessionViewModel();
                ClientCustomize cc = new ClientCustomize();
                DataTable dataTable = new DataTable();
                dataTable = cc.GetDetailsFromPqPersonals(PersonalRowID);
                if (dataTable.Rows.Count > 0)
                {

                    model.ClientRowID = Convert.ToInt16(dataTable.Rows[0]["ClientRowID"]);
                    model.ClientRefID = Convert.ToString(dataTable.Rows[0]["ClientRefID"]);
                    model.CandidateCode = Convert.ToString(dataTable.Rows[0]["CandidateCode"]);
                    model.CandTitle = Convert.ToString(dataTable.Rows[0]["CandTitle"]);
                    model.CompanyRefNo = Convert.ToString(dataTable.Rows[0]["CompanyRefNo"]);
                    model.CandidateFullName = Convert.ToString(dataTable.Rows[0]["CandidateFullName"]);
                    model.Name = Convert.ToString(dataTable.Rows[0]["Name"]);
                    model.MiddleName = Convert.ToString(dataTable.Rows[0]["MiddleName"]);
                    model.LastName = Convert.ToString(dataTable.Rows[0]["LastName"]);
                    model.FatherName = Convert.ToString(dataTable.Rows[0]["FatherName"]);
                    model.FatherMiddleName = Convert.ToString(dataTable.Rows[0]["FatherMiddleName"]);
                    model.FatherLastName = Convert.ToString(dataTable.Rows[0]["FatherLastName"]);
                    model.SpouseName = Convert.ToString(dataTable.Rows[0]["SpouseName"]);
                    model.SpouseMiddleName = Convert.ToString(dataTable.Rows[0]["SpouseMiddleName"]);
                    model.SpouseLastName = Convert.ToString(dataTable.Rows[0]["SpouseLastName"]);
                    try
                    {
                        model.DOB = Convert.ToDateTime(dataTable.Rows[0]["DOB"]);
                    }
                    catch (Exception)
                    {

                    }
                    model.Gender = Convert.ToString(dataTable.Rows[0]["Gender"]);
                    model.MobileNo = Convert.ToString(dataTable.Rows[0]["MobileNo"]);
                    model.EmailID = Convert.ToString(dataTable.Rows[0]["EmailID"]);
                    model.PanCardNo = Convert.ToString(dataTable.Rows[0]["PanCardNo"]);
                    model.PassportNo = Convert.ToString(dataTable.Rows[0]["PassportNo"]);
                    model.AadhaarCardNo = Convert.ToString(dataTable.Rows[0]["AadhaarCardNo"]);
                    model.DrivingLicenseNo = Convert.ToString(dataTable.Rows[0]["DrivingLicenseNo"]);
                    model.VoterIDCardNo = Convert.ToString(dataTable.Rows[0]["VoterIDCardNo"]);
                    model.ClientUserRowId = Convert.ToInt32(dataTable.Rows[0]["ClientUserRowId"]);
                }
                //var entity = db.PQPersonals.Find(PersonalRowID);
                //if (entity != null)
                //{
                //    model.ClientRowID = entity.ClientRowID;
                //    model.ClientRefID = entity.ClientRefID;
                //    model.CandidateCode = entity.CandidateCode;
                //    model.CandTitle = entity.CandTitle;
                //    model.CompanyRefNo = entity.CompanyRefNo;
                //    model.CandidateFullName= entity.CandidateFullName;
                //    model.Name = entity.Name;
                //    model.MiddleName = entity.MiddleName;
                //    model.LastName = entity.LastName;
                //    model.FatherName = entity.FatherName;
                //    model.FatherMiddleName = entity.FatherMiddleName;
                //    model.FatherLastName = entity.FatherLastName;
                //    model.SpouseName = entity.SpouseName;
                //    model.SpouseMiddleName = entity.SpouseMiddleName;
                //    model.SpouseLastName = entity.SpouseLastName;
                //    model.DOB = entity.DOB;
                //    model.Gender = entity.Gender;
                //    model.MobileNo = entity.MobileNo;
                //    model.EmailID = entity.EmailID;
                //    model.PanCardNo = entity.PanCardNo;
                //    model.PassportNo = entity.PassportNo;
                //    model.AadhaarCardNo = entity.AadhaarCardNo;
                //    model.DrivingLicenseNo = entity.DrivingLicenseNo;
                //    model.VoterIDCardNo = entity.VoterIDCardNo;
                //    model.ClientUserRowId = entity.ClientUserRowID;                   
                //}
                //else
                //{
                //    throw new Exception("Invalid Id!");
                //}
                return model;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public List<BGVInstructionslist> BGVInstructionslist(int PersonalRowID = 0, int ClientRowID = 0)
        {
            List<BGVInstructionslist> list = new List<BGVInstructionslist>();
            try
            {
                ClientCustomize cc = new ClientCustomize();
                var dt = cc.GetDocumentForVerificationList(PersonalRowID, ClientRowID);

                if (dt.Rows.Count > 0)
                {
                    list = (from DataRow dr in dt.Rows
                            select new BGVInstructionslist()
                            {
                                SubCheckName = Convert.ToString(dr["SubCheckName"]),
                                DocumentForVerification = Convert.ToString(dr["DocumentForVerification"]),
                            }).ToList();

                }
            }
            catch (Exception)
            {

            }
            return list;


        }

        public TempPQPersonalSessionViewModel GetTempPQPersonalInfoForSession(int PersonalRowID = 0)
        {
            try
            {
                DataTable dt = cc.GetTempPqpersonalInfo(PersonalRowID);
                TempPQPersonalSessionViewModel model = new TempPQPersonalSessionViewModel();

                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        model.ClientRowID = Int16.Parse(dr["ClientRowID"].ToString());
                        model.ClientRefID = Convert.ToString(dr["ClientRefID"]);
                        //model.CandidateCode = entity.CandidateCode;
                        model.CandTitle = Convert.ToString(dr["CandTitle"]);
                        //model.CompanyRefNo = entity.CompanyRefNo;
                        model.Name = Convert.ToString(dr["Name"]);
                        //model.MiddleName = entity.MiddleName;
                        //model.LastName = entity.LastName;
                        //model.FatherName = entity.FatherName;
                        //model.FatherMiddleName = entity.FatherMiddleName;
                        //model.FatherLastName = entity.FatherLastName;
                        //model.SpouseName = entity.SpouseName;
                        //model.SpouseMiddleName = entity.SpouseMiddleName;
                        //model.SpouseLastName = entity.SpouseLastName;
                        //model.DOB = entity.DOB;
                        //model.Gender = entity.Gender;
                        //model.MobileNo = entity.MobileNo;
                        //model.EmailID = entity.EmailID;
                        //model.PanCardNo = entity.PanCardNo;
                        //model.PassportNo = entity.PassportNo;
                        //model.AadhaarCardNo = entity.AadhaarCardNo;
                        //model.DrivingLicenseNo = entity.DrivingLicenseNo;
                        //model.VoterIDCardNo = entity.VoterIDCardNo;

                    }

                }
                else
                {
                    throw new Exception("Invalid Id!");
                }
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public short GetDETLByClientID(short ClientID = 0, string Designation = "")
        {
            try
            {
                var TeamMember = db.PQClientTeamMembers.Include("TeamDepartment").Include("TeamMember").Where(t => t.ClientRowID == ClientID && t.TeamDepartment.MasterDesignation.DesignationName == Designation)
                     .Select(item => new { TeamMemberRowID = item.TeamDepartment.TeamMember.TeamMemberRowID }).FirstOrDefault();

                if (TeamMember != null && TeamMember.TeamMemberRowID > 0)
                {
                    return TeamMember.TeamMemberRowID;
                }
                else
                {
                    return 0;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateInsuffCheckByPeraonalIdAndSubCheckID(short CheckFamilyRowID, int CheckRowId, string Remarks)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {

                        ClientCustomize cc = new ClientCustomize();
                        DataSet dd = new DataSet();


                        if (CheckFamilyRowID == 2)
                        {
                            //"AddAddressInfo";
                            var PQAddressStatusId = context.PQAddressStatus.Where(c => c.AddressRowID == CheckRowId).FirstOrDefault().AddressStatusRowID;
                            context.PQAddressStatus.Single(c => c.AddressStatusRowID == PQAddressStatusId).InfSuffClearComments = Remarks;
                            context.PQAddressStatus.Single(c => c.AddressStatusRowID == PQAddressStatusId).ClientInfSuffClearComments = Remarks;
                            context.PQAddressStatus.Single(c => c.AddressStatusRowID == PQAddressStatusId).ClienInfSuffClearDate = DateTime.Now;
                            context.PQAddressStatus.Single(c => c.AddressStatusRowID == PQAddressStatusId).DETMCheckStatus = "WIP-Pending";
                        }
                        if (CheckFamilyRowID == 5)
                        {
                            //"AddCriminalInfo";
                            var CriminalStatusRowID = context.PQCriminalStatus.Where(c => c.CriminalRowId == CheckRowId).FirstOrDefault().CriminalStatusRowID;
                            context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == CriminalStatusRowID).InfSuffClearComments = Remarks;
                            context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == CriminalStatusRowID).ClientInfSuffClearComments = Remarks;
                            context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == CriminalStatusRowID).ClienInfSuffClearDate = DateTime.Now;
                            context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == CriminalStatusRowID).DETMCheckStatus = "WIP-Pending";
                        }
                        if (CheckFamilyRowID == 11)
                        {
                            //"AddEducationInfo";
                            var EducationStatusRowID = context.PQEducationStatus.Where(c => c.EducationRowId == CheckRowId).FirstOrDefault().EducationStatusRowID;
                            context.PQEducationStatus.Single(c => c.EducationStatusRowID == EducationStatusRowID).InfSuffClearComments = Remarks;
                            context.PQEducationStatus.Single(c => c.EducationStatusRowID == EducationStatusRowID).ClientInfSuffClearComments = Remarks;
                            context.PQEducationStatus.Single(c => c.EducationStatusRowID == EducationStatusRowID).ClienInfSuffClearDate = DateTime.Now;
                            context.PQEducationStatus.Single(c => c.EducationStatusRowID == EducationStatusRowID).DETMCheckStatus = "WIP-Pending";
                        }
                        if (CheckFamilyRowID == 12)
                        {
                            //"AddEmploymentInfo"; 
                            var EmploymentStatusRowID = context.PQEmploymentStatus.Where(c => c.EmploymentRowId == CheckRowId).FirstOrDefault().EmploymentStatusRowID;
                            context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == EmploymentStatusRowID).InfSuffClearComments = Remarks;
                            context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == EmploymentStatusRowID).ClientInfSuffClearComments = Remarks;
                            context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == EmploymentStatusRowID).ClienInfSuffClearDate = DateTime.Now;
                            context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == EmploymentStatusRowID).DETMCheckStatus = "WIP-Pending";
                        }
                        if (CheckFamilyRowID == 26)
                        {
                            //"AddNationalIdentityInfo";
                            var NationalIdentityStatusRowID = context.PQNationalIdentityStatus.Where(c => c.NationalIdentityRowID == CheckRowId).FirstOrDefault().NationalIdentityStatusRowID;
                            context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == NationalIdentityStatusRowID).InfSuffClearComments = Remarks;
                            context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == NationalIdentityStatusRowID).ClientInfSuffClearComments = Remarks;
                            context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == NationalIdentityStatusRowID).ClienInfSuffClearDate = DateTime.Now;
                            context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == NationalIdentityStatusRowID).DETMCheckStatus = "WIP-Pending";
                        }
                        if (CheckFamilyRowID == 30)
                        {
                            //"AddReferenceInfo";
                            var ReferenceStatusRowID = context.PQReferenceStatus.Where(c => c.ReferenceRowID == CheckRowId).FirstOrDefault().ReferenceStatusRowID;
                            context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == ReferenceStatusRowID).InfSuffClearComments = Remarks;
                            context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == ReferenceStatusRowID).ClientInfSuffClearComments = Remarks;
                            context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == ReferenceStatusRowID).ClienInfSuffClearDate = DateTime.Now;
                            context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == ReferenceStatusRowID).DETMCheckStatus = "WIP-Pending";
                        }

                        if (CheckFamilyRowID == 40)
                        {
                            //"AddSpecialCheckStatusRowID
                            var SpecialCheckStatusRowID = context.PQSpecialCheckStatus.Where(c => c.SpecialCheckRowId == CheckRowId).FirstOrDefault().SpecialCheckStatusRowID;
                            context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == SpecialCheckStatusRowID).InfSuffClearComments = Remarks;
                            context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == SpecialCheckStatusRowID).ClientInfSuffClearComments = Remarks;
                            context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == SpecialCheckStatusRowID).ClienInfSuffClearDate = DateTime.Now;
                            context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == SpecialCheckStatusRowID).DETMCheckStatus = "WIP-Pending";
                        }

                        context.SaveChanges();
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        //public bool UpdateCaseCompletedByPersonalId(int PersonalId)
        //{
        //    try
        //    {
        //        var AddData = db.PQAddressStatus.Where(p => p.PQAddress.PersonalRowID == PersonalId);
        //        int AddCount = AddData.Where(p => p.PQAddress.PQPersonal.DEQCCaseStatus == "Completed-QC").Count();
        //        int AddRwCount = AddData.Where(p => p.RWQCManagerBy > 0).Count();

        //        var EmpData = db.PQEmploymentStatus.Where(p => p.PQEmployment.PersonalRowID == PersonalId);
        //        int EmpCount = EmpData.Where(p => p.PQEmployment.PQPersonal.DEQCCaseStatus == "Completed-QC").Count();
        //        int EmpRwCount = EmpData.Where(p => p.RWQCManagerBy > 0).Count();

        //        var CriData = db.PQCriminalStatus.Where(p => p.PQCriminal.PersonalRowID == PersonalId);
        //        int CriCount = CriData.Where(p => p.PQCriminal.PQPersonal.DEQCCaseStatus == "Completed-QC").Count();
        //        int CriRwCount = CriData.Where(p => p.RWQCManagerBy > 0).Count();

        //        var EduData = db.PQEducationStatus.Where(p => p.PQEducation.PersonalRowID == PersonalId);
        //        int EduCount = EduData.Where(p => p.PQEducation.PQPersonal.DEQCCaseStatus == "Completed-QC").Count();
        //        int EduRwCount = EduData.Where(p => p.RWQCManagerBy > 0).Count();

        //        var IdeData = db.PQNationalIdentityStatus.Where(p => p.PQNationalIdentity.PersonalRowID == PersonalId);
        //        int IdeCount = IdeData.Where(p => p.PQNationalIdentity.PQPersonal.DEQCCaseStatus == "Completed-QC").Count();
        //        int IdeRwCount = IdeData.Where(p => p.RWQCManagerBy > 0).Count();

        //        var RefData = db.PQReferenceStatus.Where(p => p.PQReference.PersonalRowID == PersonalId);
        //        int RefCount = RefData.Where(p => p.PQReference.PQPersonal.DEQCCaseStatus == "Completed-QC").Count();
        //        int RefRwCount = RefData.Where(p => p.RWQCManagerBy > 0).Count();


        //        if ((AddCount == AddRwCount) && (EmpCount == EmpRwCount) && (CriCount == CriRwCount) && (EduCount == EduRwCount) && (IdeCount == IdeRwCount) && (RefCount == RefRwCount))
        //        {
        //            ArrayList Colorlist = new ArrayList();
        //            var AddIds = AddData.Where(p => p.RWQCManagerBy > 0).Select(p => p.AddressRowID).ToList();
        //            var EmpIds = EmpData.Where(p => p.RWQCManagerBy > 0).Select(p => p.EmploymentRowId).ToList();
        //            var CriIds = CriData.Where(p => p.RWQCManagerBy > 0).Select(p => p.CriminalRowId).ToList();
        //            var EduIds = EduData.Where(p => p.RWQCManagerBy > 0).Select(p => p.EducationRowId).ToList();
        //            var IdeIds = IdeData.Where(p => p.RWQCManagerBy > 0).Select(p => p.NationalIdentityRowID).ToList();
        //            var RefIds = RefData.Where(p => p.RWQCManagerBy > 0).Select(p => p.ReferenceRowID).ToList();

        //            Colorlist.AddRange(db.PQAddressVers.Where(p => AddIds.Contains(p.AddressRowID)).Select(p => p.ColorName).
        //                Union(db.PQEmploymentVers.Where(p => EmpIds.Contains(p.EmploymentRowID)).Select(p => p.ColorName)
        //                .Union(db.PQCriminalVers.Where(p => CriIds.Contains(p.CriminalRowID)).Select(p => p.ColorName)
        //                .Union(db.PQEducationVers.Where(p => EduIds.Contains(p.EducationRowID)).Select(p => p.ColorName)
        //                .Union(db.PQNationalIdentityVers.Where(p => IdeIds.Contains(p.NationalIdentityRowID)).Select(p => p.ColorName)
        //                .Union(db.PQReferenceVers.Where(p => RefIds.Contains(p.ReferenceCheckRowID)).Select(p => p.ColorName)))))).Distinct().ToList());

        //            string FinalColor = string.Empty;
        //            if (Colorlist.Contains("Red"))
        //            {
        //                FinalColor = "Red";
        //            }
        //            else if (string.IsNullOrEmpty(FinalColor) && Colorlist.Contains("Yellow"))
        //            {
        //                FinalColor = "Yellow";
        //            }
        //            else if (string.IsNullOrEmpty(FinalColor) && Colorlist.Contains("Orange"))
        //            {
        //                FinalColor = "Orange";
        //            }
        //            else if (string.IsNullOrEmpty(FinalColor) && Colorlist.Contains("Amber"))
        //            {
        //                FinalColor = "Amber";
        //            }
        //            else if (string.IsNullOrEmpty(FinalColor) && Colorlist.Contains("Magenta"))
        //            {
        //                FinalColor = "Magenta";
        //            }
        //            else if (string.IsNullOrEmpty(FinalColor) && Colorlist.Contains("Green"))
        //            {
        //                FinalColor = "Green";
        //            }
        //            else if (string.IsNullOrEmpty(FinalColor) && Colorlist.Contains("Blue"))
        //            {
        //                FinalColor = "Green";
        //            }

        //            db.PQPersonals.Single(p => p.PersonalRowID == PersonalId).FinalColor = FinalColor;
        //            db.PQPersonals.Single(p => p.PersonalRowID == PersonalId).FinalStatus = "Completed";
        //            db.PQPersonals.Single(p => p.PersonalRowID == PersonalId).CaseStatus = "Completed";
        //            db.SaveChanges();

        //            return true;
        //        }
        //        return false;
        //    }
        //    catch (Exception ex)
        //    {
        //        string str = ex.Message;
        //        throw;
        //    }
        //}

        public IEnumerable<ExportPQPersonalDetailViewModel> GetClientAreaForExport(short ClientRowID = 0)
        {
            try
            {
                return db.PQPersonals.Where(p => p.ClientRowID == ClientRowID && !string.IsNullOrEmpty(p.CompanyRefNo)).Select(item => new ExportPQPersonalDetailViewModel
                {
                    PersonalRowID = item.PersonalRowID,
                    CandidateName = item.CandTitle + " " + item.Name + " " + item.MiddleName + " " + item.LastName,
                    CandidateCode = item.CandidateCode,
                    SecuritasID = item.CompanyRefNo,
                    DetailsEnteredBy = item.DetailsEnteredBy,
                    CaseStatus = item.CaseStatus,
                }).ToList();

            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<ExportPQPersonalDetailViewModel> GetClientAreaForExport1(short ClientRowID = 0, string CaseStatus = "", int ClientUserRowID = 0, int allclient = 0)
        {
            try
            {

                DataSet ds = new DataSet();
                ds = cc.getCasewiseSummaryDetail(ClientRowID, CaseStatus, ClientUserRowID, null, allclient);
                List<ExportPQPersonalDetailViewModel> lmd = new List<ExportPQPersonalDetailViewModel>();
                foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>
                {
                    lmd.Add(new ExportPQPersonalDetailViewModel
                    {
                        PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]),
                        // ClientName = (dr["ClientName"]).ToString(),
                        CandidateName = (dr["Candidate Name"]).ToString(),
                        CandidateCode = (dr["CandidateCode"]).ToString(),
                        SecuritasID = (dr["Securitas ID"]).ToString(),
                        CaseStatus = (dr["Case Status"]).ToString(),
                        DetailsEnteredBy = (dr["DetailsEnteredBy"]).ToString(),
                        CreatedDate = Convert.ToDateTime(dr["Created On"]),
                        ClientRefID = (dr["ClientRefID"]).ToString(),
                        PositionID = (dr["PositionID"]).ToString(),
                        CostCenter = (dr["CostCenter"]).ToString(),
                        EmployeeID = Convert.ToString(dr["EmployeeID"]),
                        CheckType = Convert.ToString(dr["CheckType"]),
                        CheckID = Convert.ToString(dr["CheckID"]),
                        InternalStatus = Convert.ToString(dr["InternalStatus"]),
                        casestatusname = dr["casestatusName"] != DBNull.Value ? (dr["casestatusName"]).ToString() : "",
                    });

                }
                return lmd.ToList();

                //return db.PQPersonals.Where(p => p.ClientRowID == ClientRowID && !string.IsNullOrEmpty(p.CompanyRefNo)).Select(item => new ExportPQPersonalDetailViewModel
                //{
                //    PersonalRowID = item.PersonalRowID,
                //    CandidateName = item.CandTitle + " " + item.Name + " " + item.MiddleName + " " + item.LastName,
                //    CandidateCode = item.CandidateCode,
                //    SecuritasID = item.CompanyRefNo,
                //    DetailsEnteredBy = item.DetailsEnteredBy,
                //    CaseStatus = item.CaseStatus,
                //}).ToList();

            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<ExportPQPersonalCandidateViewModel> GetCandidateAreaForExport(short ClientRowID = 0)
        {
            try
            {
                return db.TempPQPersonals.Include("PQClientMaster").Where(p => p.ClientRowID == ClientRowID && p.OtherDetails != "DEC" && p.OtherDetails != "Approved" && p.OtherDetails == "Pending").Select(item => new ExportPQPersonalCandidateViewModel
                {
                    PersonalRowID = item.PersonalRowID,
                    ClientName = item.PQClientMaster.MasterAbbreviation.ClientName,
                    CandidateName = item.Name + " " + item.MiddleName + " " + item.LastName,
                    CandidateCode = item.CandidateCode,//copy
                    CompanyRefNo = item.CompanyRefNo,
                    CaseStatus = "DataEntry By Candidate",
                    CreatedDate = item.CreatedDate,
                }).ToList();

            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<ExportPQPersonalCandidateViewModel> GetCandidateAreaForExport1(short ClientRowID = 0, string CaseStatus = "", int allclients = 0, int clientuserRowid = 0, string fromDate = "", string toDate = "")
        {
            try
            {
                DataSet ds = new DataSet();
                ds = cc.getCasewiseSummaryDetail(ClientRowID, CaseStatus, clientuserRowid, "", allclients, fromDate, toDate);
                List<ExportPQPersonalCandidateViewModel> lmd = new List<ExportPQPersonalCandidateViewModel>();
                //foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>
                //{
                //    lmd.Add(new ExportPQPersonalCandidateViewModel
                //    {
                //        PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]),
                //        ClientName = (dr["ClientName"]).ToString(),
                //        CandidateName = (dr["Candidate Name"]).ToString(),
                //        CompanyRefNo = (dr["Securitas ID"]).ToString(),
                //        CandidateCode = (dr["CandidateCode"]).ToString(),                      
                //        CaseStatus = (dr["Case Status"]).ToString(),
                //        ReportType = (dr["ReportType"]).ToString(),
                //        ReportDate = Convert.ToDateTime(dr["ReportUploadedDate"]),
                //        CreatedDate = Convert.ToDateTime(dr["Created On"]),
                //    });                  

                //}               


                foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>
                {
                    //lmd.Add(new ClientCandidateViewModel
                    //{
                    ExportPQPersonalCandidateViewModel c = new ExportPQPersonalCandidateViewModel();
                    c.PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]);
                    c.ClientName = (dr["ClientName"]).ToString();
                    c.CandidateName = (dr["Candidate Name"]).ToString();
                    c.CompanyRefNo = (dr["Securitas ID"]).ToString();
                    c.CandidateCode = (dr["CandidateCode"]).ToString();
                    c.CaseStatus = (dr["Case Status"]).ToString();
                    c.ReportType = (dr["ReportType"]).ToString();
                    c.EntityName = (dr["SSNNo"]).ToString();
                    c.PackageOrCheckDetails = (dr["PackageOrCheckDetails"]).ToString();
                    if (dr["ReportUploadedDate"].ToString() != "")
                        c.ReportDate = Convert.ToDateTime(dr["ReportUploadedDate"]);
                    if (dr["Created On"].ToString() != "")
                        c.CreatedDate = Convert.ToDateTime(dr["Created On"]);
                    c.ClientRefID = (dr["ClientRefID"]).ToString();
                    if (CaseStatus == "ReportReceivedInterim")
                    {
                        c.InterimDate = (dr["ReportUploadedDate"]).ToString();
                        c.InterimColor = (dr["FinalColor"]).ToString();
                        //c.YTRdate = (dr["YTRdate"]).ToString();
                    }
                    else
                    {
                        c.InterimDate = (dr["InterimDate"]).ToString();
                        c.InterimColor = (dr["InterimColor"]).ToString();
                        c.YTRdate = (dr["YTRdate"]).ToString();
                    }


                    lmd.Add(c);

                    //});

                }
                return lmd.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }


        public IEnumerable<ExportPQPersonalCandidateViewModel> GetCandidateAreaForExportKRAZYBEE(short ClientRowID = 0, string CaseStatus = "", int allclients = 0, int clientuserRowid = 0)
        {
            try
            {
                DataSet ds = new DataSet();
                ds = cc.GetMISKrazybeeReportReceived(ClientRowID, CaseStatus, clientuserRowid, "", allclients);
                List<ExportPQPersonalCandidateViewModel> lmd = new List<ExportPQPersonalCandidateViewModel>();
                //foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>
                //{
                //    lmd.Add(new ExportPQPersonalCandidateViewModel
                //    {
                //        PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]),
                //        ClientName = (dr["ClientName"]).ToString(),
                //        CandidateName = (dr["Candidate Name"]).ToString(),
                //        CompanyRefNo = (dr["Securitas ID"]).ToString(),
                //        CandidateCode = (dr["CandidateCode"]).ToString(),                      
                //        CaseStatus = (dr["Case Status"]).ToString(),
                //        ReportType = (dr["ReportType"]).ToString(),
                //        ReportDate = Convert.ToDateTime(dr["ReportUploadedDate"]),
                //        CreatedDate = Convert.ToDateTime(dr["Created On"]),
                //    });                  

                //}               


                foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>
                {
                    //lmd.Add(new ClientCandidateViewModel
                    //{
                    ExportPQPersonalCandidateViewModel c = new ExportPQPersonalCandidateViewModel();
                    c.PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]);
                    c.ClientName = (dr["ClientName"]).ToString();
                    c.CandidateName = (dr["Candidate Name"]).ToString();
                    c.CompanyRefNo = (dr["Securitas ID"]).ToString();
                    c.CandidateCode = (dr["CandidateCode"]).ToString();
                    c.CaseStatus = (dr["Case Status"]).ToString();
                    c.ReportType = (dr["ReportType"]).ToString();
                    c.EntityName = (dr["SSNNo"]).ToString();
                    c.PackageOrCheckDetails = (dr["PackageOrCheckDetails"]).ToString();
                    if (dr["ReportUploadedDate"].ToString() != "")
                        c.ReportDate = Convert.ToDateTime(dr["ReportUploadedDate"]);
                    if (dr["Created On"].ToString() != "")
                        c.CreatedDate = Convert.ToDateTime(dr["Created On"]);
                    c.ClientRefID = (dr["ClientRefID"]).ToString();
                    if (CaseStatus == "ReportReceivedInterim")
                    {
                        c.InterimDate = (dr["ReportUploadedDate"]).ToString();
                        c.InterimColor = (dr["FinalColor"]).ToString();
                        //c.YTRdate = (dr["YTRdate"]).ToString();
                    }
                    else
                    {
                        c.InterimDate = (dr["InterimDate"]).ToString();
                        c.InterimColor = (dr["InterimColor"]).ToString();
                        c.YTRdate = (dr["YTRdate"]).ToString();
                    }


                    lmd.Add(c);

                    //});

                }
                return lmd.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public string GetPQPersonalCaseDocUploadedByCandidate(long PersonalRowId)
        {
            string result = string.Empty;
            try
            {
                result = db.TempPQPersonals.Find(PersonalRowId).OtherDetails2;
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<ExportPQPersonalCandidateViewModel> GetYTRExport1(short ClientRowID = 0, string CaseStatus = "", int clientUserRowId = 0, int allclients = 0)
        {
            try
            {
                DataSet ds = new DataSet();
                ds = cc.getCasewiseSummaryDetail(ClientRowID, CaseStatus, clientUserRowId, "", allclients);
                List<ExportPQPersonalCandidateViewModel> lmd = new List<ExportPQPersonalCandidateViewModel>();



                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        lmd.Add(new ExportPQPersonalCandidateViewModel
                        {
                            PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]),
                            //ClientRowID = Convert.ToInt16(dr["ClientRowID"]),
                            CandidateName = (dr["Candidate Name"]).ToString(),
                            CheckName = (dr["SubCheckName"]).ToString(),
                            // CandidateCode = (dr["CandidateCode"]).ToString(),
                            CompanyRefNo = (dr["Securitas ID"]).ToString(),
                            CaseStatus = (dr["Case Status"]).ToString(),
                            CreatedDate = Convert.ToDateTime(dr["Created On"]),
                            HrRecuiterName = (dr["HrRecuiterName"]).ToString(),

                            //CheckStatus = (dr["Status"]).ToString(),
                            //SubCheckName = (dr["SubCheckName"]).ToString(),
                            //EmploymentRowID = Convert.ToInt32(dr["EmploymentRowId"]),

                        });

                    }

                }
                return lmd.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<ExportPQPersonalCandidateViewModel> GetInsufficencyExport1(short ClientRowID = 0, string CaseStatus = "", int ClientUserRowID = 0, int allclints = 0)
        {
            try
            {
                DataSet ds = new DataSet();
                ds = cc.getCasewiseSummaryDetail(ClientRowID, CaseStatus, ClientUserRowID, "", allclints);
                List<ExportPQPersonalCandidateViewModel> lmd = new List<ExportPQPersonalCandidateViewModel>();



                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        ExportPQPersonalCandidateViewModel vv = new ExportPQPersonalCandidateViewModel();

                        vv.PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]);
                        vv.CandidateName = (dr["Candidate Name"]).ToString();
                        vv.CompanyRefNo = (dr["Securitas ID"]).ToString();
                        vv.CaseStatus = (dr["Case Status"]).ToString();
                        if (dr["Created On"].ToString() != "")
                            vv.CreatedDate = Convert.ToDateTime(dr["Created On"]);
                        vv.EmployeeID = (dr["EmployeeID"]).ToString();
                        vv.CheckName = (dr["CheckName"]).ToString();
                        vv.HrRecuiterName = (dr["HrRecuiterName"]).ToString();

                        vv.Remarks = (dr["Remarks"]).ToString();
                        vv.Reference = (dr["Reference"]).ToString();
                        vv.CheckComponentID = (dr["CheckComponentID"]).ToString();
                        vv.InternalStatus = (dr["InternalStatus"]).ToString();
                        vv.InsuffRaiseDate = (dr["InsuffRaiseDate"]).ToString();
                        vv.InsuffRemarks = (dr["InsuffReamrks"]).ToString();

                        lmd.Add(vv);




                    }

                }
                return lmd.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        // ankit
        public PQEmploymentInsuffListPagedModel GetEmploymentInsuffChecksDetailForDETL1(int pageNo, int pageSize, string Search = "", string CheckType = "")
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();

                dt = cc.getinsuffDetails_DETL(CheckType);
                List<PQEmploymentInsuffViewModel> lmd = new List<PQEmploymentInsuffViewModel>();
                PQEmploymentInsuffListPagedModel model = new PQEmploymentInsuffListPagedModel();
                foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                {
                    PQEmploymentInsuffViewModel mm = new PQEmploymentInsuffViewModel();
                    //lmd.Add(new PQPersonalDetailViewModel
                    //{
                    mm.EmploymentRowID = Convert.ToInt32(dr["VerificationRowId"]);
                    mm.ClientRowID = Convert.ToInt16(dr["clientrowid"]);
                    mm.ClientName = (dr["ClientName"]).ToString();
                    mm.PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]);
                    mm.CandidateName = (dr["CandidateName"]).ToString();
                    mm.SubCheckRowID = Convert.ToInt16(dr["SubCheckRowID"]);
                    mm.CheckFamilyRowID = Convert.ToInt16(dr["CheckFamilyRowID"]);
                    mm.SubCheckName = (dr["SubCheckName"]).ToString();
                    mm.CompanyRefNo = (dr["CompanyRefNo"]).ToString();
                    mm.InfSuffRaiseBy = (dr["InfSuffRaiseBy"]).ToString();
                    mm.InfSuffRaisedDate = Convert.ToDateTime(dr["InfSuffRaisedDate"]);
                    mm.INFRemarks = (dr["Remarks"]).ToString();
                    mm.CreatedDate = Convert.ToDateTime(dr["CreationDate"]);
                    mm.CheckStatus = (dr["CheckStatus"]).ToString();
                    mm.ReWorkCheckStatus = (dr["ReWorkCheckStatus"]).ToString();
                    lmd.Add(mm);
                    //});

                }

                model.PageSize = pageSize;
                model.TotalRecords = dt.Rows.Count;
                model.PQAllInsuffEmployments = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public PQEducationInsuffListPagedModel GetEducationInsuffChecksDetailForDETL1(int pageNo, int pageSize, string Search = "", string CheckType = "")
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();

                dt = cc.getinsuffDetails_DETL(CheckType);
                List<PQEducationInsuffViewModel> lmd = new List<PQEducationInsuffViewModel>();
                PQEducationInsuffListPagedModel model = new PQEducationInsuffListPagedModel();
                foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                {
                    PQEducationInsuffViewModel mm = new PQEducationInsuffViewModel();
                    //lmd.Add(new PQPersonalDetailViewModel
                    //{
                    mm.EducationRowID = Convert.ToInt32(dr["VerificationRowId"]);
                    mm.ClientRowID = Convert.ToInt16(dr["clientrowid"]);
                    mm.ClientName = (dr["ClientName"]).ToString();
                    mm.PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]);
                    mm.CandidateName = (dr["CandidateName"]).ToString();
                    mm.SubCheckRowID = Convert.ToInt16(dr["SubCheckRowID"]);
                    mm.CheckFamilyRowID = Convert.ToInt16(dr["CheckFamilyRowID"]);
                    mm.SubCheckName = (dr["SubCheckName"]).ToString();
                    mm.CompanyRefNo = (dr["CompanyRefNo"]).ToString();
                    mm.InfSuffRaiseBy = (dr["InfSuffRaiseBy"]).ToString();
                    mm.InfSuffRaisedDate = Convert.ToDateTime(dr["InfSuffRaisedDate"]);
                    mm.INFRemarks = (dr["Remarks"]).ToString();
                    mm.CreatedDate = Convert.ToDateTime(dr["CreationDate"]);
                    mm.CheckStatus = (dr["CheckStatus"]).ToString();
                    mm.ReWorkCheckStatus = (dr["ReWorkCheckStatus"]).ToString();
                    lmd.Add(mm);
                    //});

                }

                model.PageSize = pageSize;
                model.TotalRecords = dt.Rows.Count;
                model.PQAllInsuffEducations = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public PQCriminalInsuffListPagedModel GetCriminalInsuffChecksDetailForDETL1(int pageNo, int pageSize, string Search = "", string CheckType = "")
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();

                dt = cc.getinsuffDetails_DETL(CheckType);
                List<PQCriminalInsuffViewModel> lmd = new List<PQCriminalInsuffViewModel>();
                PQCriminalInsuffListPagedModel model = new PQCriminalInsuffListPagedModel();
                foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                {
                    PQCriminalInsuffViewModel mm = new PQCriminalInsuffViewModel();
                    //lmd.Add(new PQPersonalDetailViewModel
                    //{
                    mm.CriminalRowID = Convert.ToInt32(dr["VerificationRowId"]);
                    mm.ClientRowID = Convert.ToInt16(dr["clientrowid"]);
                    mm.ClientName = (dr["ClientName"]).ToString();
                    mm.PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]);
                    mm.CandidateName = (dr["CandidateName"]).ToString();
                    mm.SubCheckRowID = Convert.ToInt16(dr["SubCheckRowID"]);
                    mm.CheckFamilyRowID = Convert.ToInt16(dr["CheckFamilyRowID"]);
                    mm.SubCheckName = (dr["SubCheckName"]).ToString();
                    mm.CompanyRefNo = (dr["CompanyRefNo"]).ToString();
                    mm.InfSuffRaiseBy = (dr["InfSuffRaiseBy"]).ToString();
                    mm.InfSuffRaisedDate = Convert.ToDateTime(dr["InfSuffRaisedDate"]);
                    mm.INFRemarks = (dr["Remarks"]).ToString();
                    mm.CreatedDate = Convert.ToDateTime(dr["CreationDate"]);
                    mm.CheckStatus = (dr["CheckStatus"]).ToString();
                    mm.ReWorkCheckStatus = (dr["ReWorkCheckStatus"]).ToString();
                    lmd.Add(mm);
                    //});

                }

                model.PageSize = pageSize;
                model.TotalRecords = dt.Rows.Count;
                model.PQAllInsuffCriminals = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public PQReferenceInsuffListPagedModel GetReferenceInsuffChecksDetailForDETL1(int pageNo, int pageSize, string Search = "", string CheckType = "")
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();

                dt = cc.getinsuffDetails_DETL(CheckType);
                List<PQReferenceInsuffViewModel> lmd = new List<PQReferenceInsuffViewModel>();
                PQReferenceInsuffListPagedModel model = new PQReferenceInsuffListPagedModel();
                foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                {
                    PQReferenceInsuffViewModel mm = new PQReferenceInsuffViewModel();
                    //lmd.Add(new PQPersonalDetailViewModel
                    //{
                    mm.ReferenceRowID = Convert.ToInt32(dr["VerificationRowId"]);
                    mm.ClientRowID = Convert.ToInt16(dr["clientrowid"]);
                    mm.ClientName = (dr["ClientName"]).ToString();
                    mm.PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]);
                    mm.CandidateName = (dr["CandidateName"]).ToString();
                    mm.SubCheckRowID = Convert.ToInt16(dr["SubCheckRowID"]);
                    mm.CheckFamilyRowID = Convert.ToInt16(dr["CheckFamilyRowID"]);
                    mm.SubCheckName = (dr["SubCheckName"]).ToString();
                    mm.CompanyRefNo = (dr["CompanyRefNo"]).ToString();
                    mm.InfSuffRaiseBy = (dr["InfSuffRaiseBy"]).ToString();
                    mm.InfSuffRaisedDate = Convert.ToDateTime(dr["InfSuffRaisedDate"]);
                    mm.INFRemarks = (dr["Remarks"]).ToString();
                    mm.CreatedDate = Convert.ToDateTime(dr["CreationDate"]);
                    mm.CheckStatus = (dr["CheckStatus"]).ToString();
                    mm.ReWorkCheckStatus = (dr["ReWorkCheckStatus"]).ToString();
                    lmd.Add(mm);
                    //});

                }

                model.PageSize = pageSize;
                model.TotalRecords = dt.Rows.Count;
                model.PQAllInsuffReferences = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public PQAddressInsuffListPagedModel GetAddressInsuffChecksDetailForDETL1(int pageNo, int pageSize, string Search = "", string CheckType = "")
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();

                dt = cc.getinsuffDetails_DETL(CheckType);
                List<PQAddressInsuffViewModel> lmd = new List<PQAddressInsuffViewModel>();
                PQAddressInsuffListPagedModel model = new PQAddressInsuffListPagedModel();
                foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                {
                    PQAddressInsuffViewModel mm = new PQAddressInsuffViewModel();
                    //lmd.Add(new PQPersonalDetailViewModel
                    //{
                    mm.AddressRowID = Convert.ToInt32(dr["VerificationRowId"]);
                    mm.ClientRowID = Convert.ToInt16(dr["clientrowid"]);
                    mm.ClientName = (dr["ClientName"]).ToString();
                    mm.PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]);
                    mm.CandidateName = (dr["CandidateName"]).ToString();
                    mm.SubCheckRowID = Convert.ToInt16(dr["SubCheckRowID"]);
                    mm.CheckFamilyRowID = Convert.ToInt16(dr["CheckFamilyRowID"]);
                    mm.SubCheckName = (dr["SubCheckName"]).ToString();
                    mm.CompanyRefNo = (dr["CompanyRefNo"]).ToString();
                    mm.InfSuffRaiseBy = (dr["InfSuffRaiseBy"]).ToString();
                    mm.InfSuffRaisedDate = Convert.ToDateTime(dr["InfSuffRaisedDate"]);
                    mm.INFRemarks = (dr["Remarks"]).ToString();
                    mm.CreatedDate = Convert.ToDateTime(dr["CreationDate"]);
                    mm.CheckStatus = (dr["CheckStatus"]).ToString();
                    mm.ReWorkCheckStatus = (dr["ReWorkCheckStatus"]).ToString();
                    lmd.Add(mm);
                    //});

                }

                model.PageSize = pageSize;
                model.TotalRecords = dt.Rows.Count;
                model.PQAllInsuffAddresses = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public PQSpecialCheckInsuffListPagedModel GetSpecialInsuffChecksDetailForDETL1(int pageNo, int pageSize, string Search = "", string CheckType = "")
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();

                dt = cc.getinsuffDetails_DETL(CheckType);
                List<PQSpecialCheckInsuffViewModel> lmd = new List<PQSpecialCheckInsuffViewModel>();
                PQSpecialCheckInsuffListPagedModel model = new PQSpecialCheckInsuffListPagedModel();
                foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                {
                    PQSpecialCheckInsuffViewModel mm = new PQSpecialCheckInsuffViewModel();
                    //lmd.Add(new PQPersonalDetailViewModel
                    //{
                    mm.SpecialCheckRowId = Convert.ToInt32(dr["VerificationRowId"]);
                    mm.ClientRowID = Convert.ToInt16(dr["clientrowid"]);
                    mm.ClientName = (dr["ClientName"]).ToString();
                    mm.PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]);
                    mm.CandidateName = (dr["CandidateName"]).ToString();
                    mm.SubCheckRowID = Convert.ToInt16(dr["SubCheckRowID"]);
                    mm.CheckFamilyRowID = Convert.ToInt16(dr["CheckFamilyRowID"]);
                    mm.SubCheckName = (dr["SubCheckName"]).ToString();
                    mm.CompanyRefNo = (dr["CompanyRefNo"]).ToString();
                    mm.InfSuffRaiseBy = (dr["InfSuffRaiseBy"]).ToString();
                    mm.InfSuffRaisedDate = Convert.ToDateTime(dr["InfSuffRaisedDate"]);
                    mm.INFRemarks = (dr["Remarks"]).ToString();
                    mm.CreatedDate = Convert.ToDateTime(dr["CreationDate"]);
                    mm.CheckStatus = (dr["CheckStatus"]).ToString();
                    mm.ReWorkCheckStatus = (dr["ReWorkCheckStatus"]).ToString();
                    lmd.Add(mm);
                    //});

                }

                model.PageSize = pageSize;
                model.TotalRecords = dt.Rows.Count;
                model.PQAllInsuffSpecialCheckes = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public PQNationalIdentityInsuffListPagedModel GetNationalInsuffChecksDetailForDETL1(int pageNo, int pageSize, string Search = "", string CheckType = "")
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataTable dt = new DataTable();

                dt = cc.getinsuffDetails_DETL(CheckType);
                List<PQNationalIdentityInsuffViewModel> lmd = new List<PQNationalIdentityInsuffViewModel>();
                PQNationalIdentityInsuffListPagedModel model = new PQNationalIdentityInsuffListPagedModel();
                foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                {
                    PQNationalIdentityInsuffViewModel mm = new PQNationalIdentityInsuffViewModel();
                    //lmd.Add(new PQPersonalDetailViewModel
                    //{
                    mm.NationalIdentityRowID = Convert.ToInt32(dr["VerificationRowId"]);
                    mm.ClientRowID = Convert.ToInt16(dr["clientrowid"]);
                    mm.ClientName = (dr["ClientName"]).ToString();
                    mm.PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]);
                    mm.CandidateName = (dr["CandidateName"]).ToString();
                    mm.SubCheckRowID = Convert.ToInt16(dr["SubCheckRowID"]);
                    mm.CheckFamilyRowID = Convert.ToInt16(dr["CheckFamilyRowID"]);
                    mm.SubCheckName = (dr["SubCheckName"]).ToString();
                    mm.CompanyRefNo = (dr["CompanyRefNo"]).ToString();
                    mm.InfSuffRaiseBy = (dr["InfSuffRaiseBy"]).ToString();
                    mm.InfSuffRaisedDate = Convert.ToDateTime(dr["InfSuffRaisedDate"]);
                    mm.INFRemarks = (dr["Remarks"]).ToString();
                    mm.CreatedDate = Convert.ToDateTime(dr["CreationDate"]);
                    mm.CheckStatus = (dr["CheckStatus"]).ToString();
                    mm.ReWorkCheckStatus = (dr["ReWorkCheckStatus"]).ToString();
                    lmd.Add(mm);
                    //});

                }

                model.PageSize = pageSize;
                model.TotalRecords = dt.Rows.Count;
                model.PQAllInsuffNationalIdentities = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<SubcheckViewModel> getSubcheckdata(int ClientRowId, int PersonalRowID, string UsedataBase = "")
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.GetSubchecks(ClientRowId, PersonalRowID, UsedataBase);
                List<SubcheckViewModel> lmd = new List<SubcheckViewModel>();
                lmd.Add(new SubcheckViewModel { SubCheckRowID = "0", SubCheckName = "--Select--" });
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        SubcheckViewModel mm = new SubcheckViewModel();
                        mm.SubCheckRowID = dr["SubCheckRowID"].ToString();
                        mm.SubCheckName = dr["SubCheckName"].ToString();
                        mm.ClientCheckRowID = Convert.ToInt32(dr["ClientCheckRowID"]);
                        lmd.Add(mm);
                    }


                }
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IEnumerable<SubcheckViewModel> getSubcheckdataforDelete(int ClientRowId, int PersonalRowID)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.GetSubchecksForDel(ClientRowId, PersonalRowID);
                List<SubcheckViewModel> lmd = new List<SubcheckViewModel>();
                lmd.Add(new SubcheckViewModel { SubCheckRowID = "0", SubCheckName = "--Select--" });
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        SubcheckViewModel mm = new SubcheckViewModel();
                        mm.SubCheckRowID = dr["SubCheckRowID"].ToString();
                        mm.SubCheckName = dr["SubCheckName"].ToString();

                        lmd.Add(mm);
                    }


                }
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<SubcheckViewModel> GetSubchecksForSpocRemark(int PersonalRowID)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.GetSubchecksForSpocRemark(PersonalRowID);
                List<SubcheckViewModel> lmd = new List<SubcheckViewModel>();
                lmd.Add(new SubcheckViewModel { SubCheckRowID = "0", SubCheckName = "--Select--" });
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows)
                    {
                        SubcheckViewModel mm = new SubcheckViewModel();
                        mm.SubCheckRowID = dr["SubCheckRowID"].ToString();
                        mm.SubCheckName = dr["SubCheckName"].ToString();
                        lmd.Add(mm);
                    }
                }
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IEnumerable<SubcheckViewModel> GetMastercheckId()
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.GetMasterCheckStatus();
                List<SubcheckViewModel> lmd = new List<SubcheckViewModel>();
                lmd.Add(new SubcheckViewModel { CheckStatusID = 0, CheckStatus = "--Select--" });
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        SubcheckViewModel mm = new SubcheckViewModel();
                        mm.CheckStatusID = Convert.ToInt32(dr["CheckStatusID"]);
                        mm.CheckStatus = dr["CheckStatus"].ToString();

                        lmd.Add(mm);
                    }


                }
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public IEnumerable<SubcheckViewModel> GetMasterCaseId()
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.GetMasterCaseStatus();
                List<SubcheckViewModel> lmd = new List<SubcheckViewModel>();
                lmd.Add(new SubcheckViewModel { CheckStatusID = 0, CheckStatus = "--Select--" });
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        SubcheckViewModel mm = new SubcheckViewModel();
                        mm.CaseStatusID = Convert.ToInt32(dr["CaseStausID"]);
                        mm.CaseStatus = dr["CaseStatus"].ToString();

                        lmd.Add(mm);
                    }


                }
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IEnumerable<ExportPQPersonalCandidateViewModel> GetCandidateAreaForExportGoAir(short ClientRowID = 0, string CaseStatus = "")
        {
            try
            {
                DataSet ds = new DataSet();
                ds = cc.getCasewiseSummaryDetailForGoair(ClientRowID, CaseStatus);
                List<ExportPQPersonalCandidateViewModel> lmd = new List<ExportPQPersonalCandidateViewModel>();




                foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>
                {
                    //lmd.Add(new ClientCandidateViewModel
                    //{
                    ExportPQPersonalCandidateViewModel c = new ExportPQPersonalCandidateViewModel();

                    c.ClientName = (dr["ClientName"]).ToString();
                    c.ClientLocation = (dr["ClientLocation"]).ToString();
                    c.CandidateName = (dr["Candidate Name"]).ToString();
                    c.CompanyRefNo = (dr["Securitas ID"]).ToString();
                    c.EmployeeID = (dr["EmployeeID"]).ToString();
                    c.ContactNo = (dr["ContactNo"]).ToString();

                    if (dr["CaseInitiationdate"].ToString() != "")
                        c.CaseInitiationDate = Convert.ToDateTime(dr["CaseInitiationdate"]);

                    c.FinalStatus = (dr["FinalStatus"]).ToString();
                    if (dr["ReportSendDate"].ToString() != "")
                        c.reportSendDate = Convert.ToDateTime(dr["ReportSendDate"]);
                    c.CaseColorStatus = (dr["CaseColorStatus"]).ToString();
                    c.GSTColorName = (dr["GSTColorName"]).ToString();
                    c.GSTComments = (dr["GSTComments"]).ToString();
                    c.BusinessAddColorName = (dr["BusinessAddColorName"]).ToString();
                    c.BusinessAddComments = (dr["BusinessAddComments"]).ToString();
                    c.AddressProofColorName = (dr["AddressProofColorName"]).ToString();
                    c.AddressProofComments = (dr["AddressProofComments"]).ToString();

                    c.PANNoVerificationcolorCode = (dr["PANNoVerificationcolorCode"]).ToString();
                    c.PANNoVerificationComments = (dr["PANNoVerificationComments"]).ToString();


                    lmd.Add(c);

                    //});

                }
                return lmd.ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IEnumerable<SubcheckViewModel> GetMastercheckIdForSpoc(string Department)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.GetMasterCheckStatusSpoc(Department);
                List<SubcheckViewModel> lmd = new List<SubcheckViewModel>();
                lmd.Add(new SubcheckViewModel { CheckStatusID = 0, CheckStatus = "--Select--" });
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        SubcheckViewModel mm = new SubcheckViewModel();
                        mm.CheckStatusID = Convert.ToInt32(dr["CheckStatusID"]);
                        mm.CheckStatus = dr["CheckStatus"].ToString();

                        lmd.Add(mm);
                    }


                }
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IEnumerable<SubcheckViewModel> getSubcheckdataforaddress(int ClientRowId, int PersonalRowID, string Department)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.GetSubchecksForAddress(ClientRowId, PersonalRowID, Department);
                List<SubcheckViewModel> lmd = new List<SubcheckViewModel>();
                lmd.Add(new SubcheckViewModel { SubCheckRowID = "0", SubCheckName = "--Select--" });
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        SubcheckViewModel mm = new SubcheckViewModel();
                        mm.SubCheckRowID = dr["SubCheckRowID"].ToString();
                        mm.SubCheckName = dr["SubCheckName"].ToString();

                        lmd.Add(mm);
                    }


                }
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public bool GetCVValidation(string ClientCheckRowIDs)
        {
            bool valid = false;
            try
            {
                if (ClientCheckRowIDs.Contains("Check-"))
                {
                    string[] CVValid = ClientCheckRowIDs.Split(',');
                    foreach (var ChkPkg in CVValid)
                    {
                        string[] strIds = ChkPkg.Split('-');
                        if (!string.IsNullOrEmpty(strIds[1]))
                        {
                            DataTable dt = cc.GetCVValidation(strIds[1], strIds[0]);
                            if (dt.Rows.Count > 0)
                            {
                                int subcheckRowId = Convert.ToInt32(dt.Rows[0]["SubCheckRowID"].ToString());
                                if (subcheckRowId == 38)
                                {
                                    valid = true;


                                }

                            }



                        }

                    }
                }
                else
                {
                    string[] CVValid = ClientCheckRowIDs.Split('-');
                    DataTable dt = cc.GetCVValidation("", CVValid[1]);
                    if (dt.Rows.Count > 0)
                    {
                        int subcheckRowId = Convert.ToInt32(dt.Rows[0]["SubCheckRowID"].ToString());
                        if (subcheckRowId == 38)
                        {
                            valid = true;


                        }

                    }
                    else
                    {
                        valid = false;
                    }
                }

                return valid;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public ClientCandidateSearchListPagedModel ClientCandidateSearchList(int pageNo, int pageSize, string sort, string sortDir, string SearchType = "", short ClientRowID = 0, int ClientUserRowID = 0, string SearchValue = "", int allclints = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }
                DataSet ds = new DataSet();
                ClientCandidateSearchListPagedModel model = new ClientCandidateSearchListPagedModel();
                if (!string.IsNullOrEmpty(SearchType))
                {
                    ds = cc.ClientCandidateSearchList(ClientRowID, ClientUserRowID, SearchType, SearchValue, allclints);
                }
                else
                {
                    ds = cc.ClientCandidateSearchList(ClientRowID, ClientUserRowID, "", SearchValue, allclints);
                }
                // ds = cc.getCasewiseSummaryDetail(ClientRowID, CaseStatus);
                List<ClientCandidateSearch> lmd = new List<ClientCandidateSearch>();
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        //lmd.Add(new ClientCandidateSearch
                        //{
                        ClientCandidateSearch mm = new ClientCandidateSearch();
                        mm.PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]);
                        mm.ClientRowID = Convert.ToInt16(dr["ClientRowID"]);
                        mm.CandidateName = (dr["Candidate Name"]).ToString();
                        mm.FinalStatus = (dr["ReportType"]).ToString();
                        mm.CandidateCode = (dr["CandidateCode"]).ToString();
                        mm.CompanyRefNo = (dr["Securitas ID"]).ToString();
                        mm.fatherName = (dr["Father Name"]).ToString();
                        mm.dob = (dr["DOB"]).ToString();
                        // CaseStatus = (dr["Case Status"]).ToString(),
                        mm.CreatedDate = Convert.ToDateTime(dr["SLA_STARTDATE_INT"]);
                        mm.ClientRefID = (dr["ClientRefID"]).ToString();
                        mm.ClientPackageName = (dr["ClientPackageName"]).ToString();
                        mm.ActiveStatus = (dr["ActiveStatus"]).ToString();
                        mm.IsReportUploaded = Convert.ToBoolean(dr["IsReportUploaded"]);
                        mm.ReportVisibality = Convert.ToBoolean(dr["ReportVisibality"]);
                        mm.CaseStatus = (dr["CaseStatus"]).ToString();
                        mm.MobileNo = (dr["MobileNo"]).ToString();
                        mm.Emailid = (dr["Emailid"]).ToString();
                        mm.EmployeeID = (dr["EmployeeID"]).ToString();
                        mm.IntrimReportStatus = Convert.ToBoolean(dr["IntrimReportStatus"]);
                        mm.tempPersonalRowId = Convert.ToString(dr["TempPersonalRowId"]);
                        mm.IsReVerification = Convert.ToBoolean(dr["IsReVerification"]);
                        lmd.Add(mm);


                    }


                    model.PageSize = pageSize;
                    model.TotalRecords = ds.Tables[0].Rows.Count;
                    model.ClientCandidateSearch = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                }
                else
                {
                    model.TotalRecords = 0;
                }

                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public IEnumerable<SubcheckViewModel> GetSubSetchecks(string RefNo)
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.GetSubSetchecks(RefNo);
                List<SubcheckViewModel> lmd = new List<SubcheckViewModel>();
                ////lmd.Add(new SubcheckViewModel { SubCheckRowID = "0", SubCheckName = "--Select--" });
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        SubcheckViewModel mm = new SubcheckViewModel();
                        mm.SubCheckRowID = dr["SubCheckRowID"].ToString();
                        mm.SubCheckName = dr["SubCheckName"].ToString();
                        mm.ClientCheckRowID = Convert.ToInt32(dr["ClientCheckRowID"]);
                        mm.ReOpenBilling = Convert.ToBoolean(dr["ReOpenBilling"]);
                        lmd.Add(mm);
                    }


                }
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public IEnumerable<AddPQPersonalViewModel> CheckCandidateduplicateNameFromClient(int clientRowId, string Name, string MiddleName, string LastName)
        {

            try
            {
                DataTable dt = new DataTable();
                dt = cc.CheckCandidateduplicateName(clientRowId, Name, MiddleName, LastName);
                AddPQPersonalViewModel model = new AddPQPersonalViewModel();
                List<AddPQPersonalViewModel> lmd = new List<AddPQPersonalViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        lmd.Add(new AddPQPersonalViewModel
                        {
                            PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]),
                            CompanyRefNo = dr["CompanyRefNo"].ToString(),
                            CandidateName = (dr["Candidate Name"]).ToString(),
                            CandidateCode = (dr["CandidateCode"]).ToString(),
                            OrderDate = Convert.ToDateTime(dr["OrderDate"]),
                            MobileNo = (dr["MobileNo"]).ToString(),
                            EmailID = (dr["EmailID"]).ToString(),
                            ClientRefID = (dr["ClientRefID"]).ToString(),
                            FatherName = (dr["Father Name"]).ToString(),


                        });

                    }

                }

                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }



        public IEnumerable<AddTempPQPersonalForBulkUpViewModel> CheckCandidateduplicateNameFromCandidate(int clientRowId, string Name, string MiddleName, string LastName, string Email = "", string MobNo = "", string empId = "")
        {

            try
            {
                DataTable dt = new DataTable();
                dt = cc.CheckCandidateduplicateName(clientRowId, Name, MiddleName, LastName, Email, MobNo, empId);
                AddTempPQPersonalForBulkUpViewModel model = new AddTempPQPersonalForBulkUpViewModel();
                List<AddTempPQPersonalForBulkUpViewModel> lmd = new List<AddTempPQPersonalForBulkUpViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        AddTempPQPersonalForBulkUpViewModel mm = new AddTempPQPersonalForBulkUpViewModel();
                        //lmd.Add(new AddTempPQPersonalForBulkUpViewModel
                        //{
                        mm.PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]);
                        mm.CompanyRefNo = dr["CompanyRefNo"].ToString();
                        mm.CandidateName = (dr["Candidate Name"]).ToString();
                        mm.CandidateCode = (dr["CandidateCode"]).ToString();
                        mm.MobileNo = (dr["MobileNo"]).ToString();
                        mm.EmailID = (dr["EmailID"]).ToString();
                        mm.ClientRefID = (dr["ClientRefID"]).ToString();
                        mm.FatherName = (dr["Father Name"]).ToString();
                        lmd.Add(mm);


                        //});

                    }

                }

                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public ClientCandidateSearchListPagedModel GetDuplicateReferenceDetails(int pageNo, int pageSize, string sort, string sortDir, short ClientRowID = 0, int ClientUserRowID = 0, string TicketNo = "", string fromDate = "", string todate = "")
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }
                DataTable dt = new DataTable();
                ClientCandidateSearchListPagedModel model = new ClientCandidateSearchListPagedModel();
                //if (!string.IsNullOrEmpty(SearchType))
                //{
                //    dt = cc.GetDuplicateReferenceDetails();
                //}
                //else
                //{
                dt = cc.GetDuplicateReferenceDetails(ClientRowID, TicketNo);
                //}
                // ds = cc.getCasewiseSummaryDetail(ClientRowID, CaseStatus);
                List<CandidateDuplicateReferenceDetails> lmd = new List<CandidateDuplicateReferenceDetails>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        CandidateDuplicateReferenceDetails mm = new CandidateDuplicateReferenceDetails();
                        mm.Id = Convert.ToInt32(dr["Id"]);
                        mm.TicketId = dr["TicketId"].ToString();
                        mm.Name = (dr["Name"]).ToString();
                        mm.MiddleName = (dr["MiddleName"]).ToString();
                        mm.LastName = (dr["LastName"]).ToString();
                        mm.DOB = (dr["DOB"]).ToString();
                        mm.MobileNo = (dr["MobileNo"]).ToString();
                        mm.CreatedBy = (dr["CreatedBy"]).ToString();
                        mm.CreatedDate = Convert.ToDateTime(dr["CreatedDate"].ToString());
                        mm.EmailId = (dr["EmailId"]).ToString();
                        mm.DuplicateStatus = (dr["DuplicateStatus"]).ToString();
                        mm.FatherName = (dr["FatherName"]).ToString();
                        mm.DOB = (dr["dob"]).ToString();
                        lmd.Add(mm);
                    }
                    model.PageSize = pageSize;
                    model.TotalRecords = dt.Rows.Count;
                    model.CandidateDuplicateReferenceDetails = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                }
                else
                {
                    model.TotalRecords = 0;
                }

                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public IEnumerable<AddPQPersonalBulkImportViewModel> CheckCandidateduplicateNameFromDETL(int clientRowId, string Name, string MiddleName, string LastName, string Email = "", string MobNo = "", string EmpId = "")
        {

            try
            {
                DataTable dt = new DataTable();
                dt = cc.CheckCandidateduplicateName(clientRowId, Name, MiddleName, LastName, Email, MobNo,EmpId);
                AddPQPersonalBulkImportViewModel model = new AddPQPersonalBulkImportViewModel();
                List<AddPQPersonalBulkImportViewModel> lmd = new List<AddPQPersonalBulkImportViewModel>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        AddPQPersonalBulkImportViewModel mm = new AddPQPersonalBulkImportViewModel();

                        mm.PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]);
                        mm.CompanyRefNo = dr["CompanyRefNo"].ToString();
                        mm.Name = (dr["Candidate Name"]).ToString();
                        mm.CandidateCode = (dr["CandidateCode"]).ToString();
                        mm.MobileNo = (dr["MobileNo"]).ToString();
                        mm.EmailID = (dr["EmailID"]).ToString();
                        mm.ClientRefID = (dr["ClientRefID"]).ToString();
                        mm.FatherName = (dr["Father Name"]).ToString();
                        lmd.Add(mm);

                    }

                }

                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public APICandidateSummaryListPagedModel GetAPI_IntegrationCaseSummary(int pageNo, int pageSize, string sort, string sortDir, short ClientRowID = 0, int ClientUserRowID = 0, int status = 0, string referenceId = "")
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }
                DataTable dt = new DataTable();
                APICandidateSummaryListPagedModel model = new APICandidateSummaryListPagedModel();
                dt.DefaultView.RowFilter = "";
                if (referenceId == "Insuff Cleared")
                {
                    dt = cc.GetAPI_IntegrationCaseSummary(status, ClientRowID, "");
                    dt.DefaultView.RowFilter = "Remarks='Insuff Cleared'";
                }
                else if (referenceId == "New Check Added")
                {
                    dt = cc.GetAPI_IntegrationCaseSummary(status, ClientRowID, "");
                    dt.DefaultView.RowFilter = "Remarks='New Check Added'";
                }
                else
                {
                    dt = cc.GetAPI_IntegrationCaseSummary(status, ClientRowID, referenceId);
                }
                dt = dt.DefaultView.ToTable();
                List<APICandidateSummary> lmd = new List<APICandidateSummary>();
                if (dt.Rows.Count > 0)
                {
                    lmd = (from DataRow dr in dt.Rows
                           select new APICandidateSummary()
                           {
                               ClientName = Convert.ToString(dr["ClientName"]),
                               UniqueReferenceID = Convert.ToString(dr["UniqueReferenceID"]),
                               CompanyRefNo = Convert.ToString(dr["CompanyRefNo"]),
                               CreationDate = Convert.ToString(dr["CreationDate"]),
                               Status = Convert.ToString(dr["Status"]),
                               Remarks = Convert.ToString(dr["Remarks"]),
                               PersonalRowID = dr["PersonalRowID"] == DBNull.Value ? 0 : Convert.ToInt32(dr["PersonalRowID"])
                           }).ToList();

                    model.PageSize = pageSize;
                    model.TotalRecords = dt.Rows.Count;
                    model.APICandidateSummary = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                }
                else
                {
                    model.TotalRecords = 0;
                }

                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public APICandidateSummaryDetailListPagedModel GetAPI_IntegrationCaseSummaryDetail(string UniqueReferenceID = "")
        {
            try
            {

                DataTable dt = new DataTable();
                APICandidateSummaryDetailListPagedModel model = new APICandidateSummaryDetailListPagedModel();

                dt = cc.GetAPI_IntegrationCaseSummaryDetail(UniqueReferenceID);

                List<APICandidateSummaryDetail> lmd = new List<APICandidateSummaryDetail>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        APICandidateSummaryDetail mm = new APICandidateSummaryDetail();
                        mm.ClientID = dr["ClientID"].ToString();
                        mm.ClientName = dr["ClientName"].ToString();
                        mm.UniqueReferenceID = (dr["UniqueReferenceID"]).ToString();
                        mm.Antecedent = (dr["Antecedent"]).ToString();
                        mm.AntecedentValue = (dr["AntecedentValue"]).ToString();
                        mm.UserName = (dr["UserName"]).ToString();
                        mm.APIAutoID = Convert.ToInt32(dr["APIAutoID"]);
                        lmd.Add(mm);
                    }

                    model.TotalRecords = dt.Rows.Count;
                    model.APICandidateSummaryDetail = lmd;
                }
                else
                {
                    model.TotalRecords = 0;
                }

                return model;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public List<APICandidateSummaryDetail> GetAPI_InsuffCaseSummaryDetail(string UniqueReferenceID = "")
        {
            try
            {

                DataTable dt = new DataTable();
                dt = cc.GetAPI_InsuffCaseSummaryDetail(UniqueReferenceID);
                List<APICandidateSummaryDetail> lmd = new List<APICandidateSummaryDetail>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        APICandidateSummaryDetail mm = new APICandidateSummaryDetail();
                        mm.ClientID = dr["ClientID"].ToString();
                        mm.ClientName = dr["ClientName"].ToString();
                        mm.UniqueReferenceID = (dr["UniqueReferenceID"]).ToString();
                        mm.Antecedent = (dr["Antecedent"]).ToString();
                        mm.AntecedentValue = (dr["AntecedentValue"]).ToString();
                        mm.UserName = (dr["UserName"]).ToString();
                        lmd.Add(mm);
                    }

                }

                return lmd;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        public List<APICandidateSummaryDetail> GetAPI_ReverificationCaseSummaryDetail(string UniqueReferenceID = "")
        {
            try
            {

                DataTable dt = new DataTable();
                dt = cc.GetAPI_ReverificationCaseSummaryDetail(UniqueReferenceID);
                List<APICandidateSummaryDetail> lmd = new List<APICandidateSummaryDetail>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        APICandidateSummaryDetail mm = new APICandidateSummaryDetail();
                        mm.ClientID = dr["ClientID"].ToString();
                        mm.ClientName = dr["ClientName"].ToString();
                        mm.UniqueReferenceID = (dr["UniqueReferenceID"]).ToString();
                        mm.Antecedent = (dr["Antecedent"]).ToString();
                        mm.AntecedentValue = (dr["AntecedentValue"]).ToString();
                        mm.UserName = (dr["UserName"]).ToString();
                        lmd.Add(mm);
                    }

                }

                return lmd;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public PQPersonalDetailsVerViewModel GetPersonalDetailsForTM(int personalRowid = 0, int subcheckRowid = 0)
        {
            try
            {
                PQPersonalDetailsVerViewModel model = new PQPersonalDetailsVerViewModel();

                ClientCustomize cc = new ClientCustomize();
                DataTable dt = cc.GetPersonalDetailsForTM(personalRowid, subcheckRowid);
                if (dt.Rows.Count > 0)
                {
                    model.ClientRowID = Convert.ToInt16(dt.Rows[0]["ClientRowID"].ToString());
                    model.PersonalRowID = Convert.ToInt32(dt.Rows[0]["PersonalRowID"].ToString());
                    model.SubCheckRowID = Convert.ToInt16(dt.Rows[0]["SubCheckFamilyID"].ToString());
                    model.CheckFamilyRowID = Convert.ToInt16(dt.Rows[0]["CheckFamilyRowID"].ToString());
                    model.UniqueComponentID = dt.Rows[0]["CheckUniqueID"].ToString();
                    model.CandidateName = dt.Rows[0]["CandidateFullName"].ToString();
                    model.ClientName = dt.Rows[0]["clientName"].ToString();
                    model.CompanyReferenceNo = dt.Rows[0]["CompanyRefNo"].ToString();
                    model.SubReferenceID = dt.Rows[0]["SubCheckName"].ToString();
                    model.MobileNo = dt.Rows[0]["mobileno"].ToString();
                    model.SpecialInstructions = dt.Rows[0]["SpecialInstructions"].ToString();
                    model.CheckStatus = dt.Rows[0]["CheckStatus"].ToString();
                    model.AccountName = dt.Rows[0]["Account Name"].ToString();
                    model.ProcessName = dt.Rows[0]["Process Name"].ToString();
                    model.CheckStatusID = Convert.ToString(dt.Rows[0]["CheckStatusID"]);
                    model.EntityName = dt.Rows[0]["EntityName"].ToString();
                    model.PackageCheckDetails = dt.Rows[0]["PackageOrCheckDetails"].ToString();
                    model.ModeOfVerification = dt.Rows[0]["Mov"].ToString();
                    model.CreatedBySource = dt.Rows[0]["CreatedBySource"].ToString();
                    model.ClientRefId = dt.Rows[0]["ClientRefID"].ToString();
                    if (model.CheckFamilyRowID == 11)
                    {
                        model.collegeName = getCollgeName(model.PersonalRowID, model.SubCheckRowID);
                    }
                    else if (model.CheckFamilyRowID == 12)
                    {
                        model.employmentName = getEmploymentName(model.PersonalRowID, model.SubCheckRowID);
                    }
                    else
                    {
                        model.collegeName = "";
                        model.employmentName = "";
                    }
                    if (model.ClientRowID == 317 || model.ClientRowID == 327)
                    {
                        DataTable dw = new DataTable();
                        dw = cc.GetPersonalDetailsForTMWipro(personalRowid);
                        if (dw.Rows.Count > 0)
                        {
                            model.AccountName = dw.Rows[0]["AccountName"].ToString();
                            model.EntityName = dw.Rows[0]["EntityName"].ToString();
                            model.EmailID = dw.Rows[0]["EmailID"].ToString();
                            model.DOJ = dw.Rows[0]["Doj"].ToString();
                            model.CareerBandName = dw.Rows[0]["CareerBandName"].ToString();
                            model.PatternName = dw.Rows[0]["PatternName"].ToString();
                        }
                    }
                }
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<SpecialInstructionVerViewModel> GetSpecialInstructions(int ClientRowid = 0, int subcheckRowid = 0)
        {
            try
            {
                List<SpecialInstructionVerViewModel> model = new List<SpecialInstructionVerViewModel>();
                ClientCustomize cc = new ClientCustomize();
                DataTable dt = cc.GetSpecialInstructions(ClientRowid, subcheckRowid);
                model = (from DataRow dr in dt.Rows
                         select new SpecialInstructionVerViewModel()
                         {
                             Instructions = Convert.ToString(dr["SpecialInstruction"]),
                             Department = Convert.ToString(dr["DepartmentName"])
                         }).ToList();

                return model;
            } 
            catch (Exception)
            {
                throw;
            }
        }


        public IEnumerable<SubcheckViewModel> GetMasterStopcheckStatusId()
        {
            try
            {
                DataTable dt = new DataTable();
                dt = cc.GetMasterStopcheckStatusId();
                List<SubcheckViewModel> lmd = new List<SubcheckViewModel>();
                lmd.Add(new SubcheckViewModel { CheckStatusID = 0, CheckStatus = "--Select--" });
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        SubcheckViewModel mm = new SubcheckViewModel();
                        mm.CheckStatusID = Convert.ToInt32(dr["CheckStatusID"]);
                        mm.CheckStatus = dr["CheckStatus"].ToString();

                        lmd.Add(mm);
                    }


                }
                return lmd;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PQPersonalInfoViewModel GetTempPersonalInfoByPersonalRowID(int PersonalRowID = 0)
        {
            PQPersonalInfoViewModel model = new PQPersonalInfoViewModel();
            model.PersonalRowID = PersonalRowID;

            try
            {
                DataTable tbl = new DataTable();
                DataSet ds = new DataSet("ds");
                ds = DAS.ExecuteDataset(DAS.ConnectionString, CommandType.StoredProcedure, "usp_GetTempPQCandidatePersonalInfo",
                DAS.CreateParameter("@PersonalRowID", SqlDbType.Int, PersonalRowID, ParameterDirection.Input, 100, false)
                );

                tbl = ds.Tables[0];
                var loa = ds.Tables[1];
                if (tbl.Rows.Count > 0)
                {
                    model.CandTitle = Convert.ToString(tbl.Rows[0]["Title"]);
                    model.Name = Convert.ToString(tbl.Rows[0]["CandidateName"]);
                    model.Gender = Convert.ToString(tbl.Rows[0]["Gender"]);
                    model.DOB = Convert.ToString(tbl.Rows[0]["DOB"]);
                    model.FatherName = Convert.ToString(tbl.Rows[0]["FatherName"]);
                    model.MobileNo = Convert.ToString(tbl.Rows[0]["MobileNo"]);
                    model.AlternateNo = Convert.ToString(tbl.Rows[0]["AlternateNo"]);
                    model.EmailID = Convert.ToString(tbl.Rows[0]["Email"]);
                    model.AlternateEmailID = Convert.ToString(tbl.Rows[0]["AlternateEmailID"]);
                    if (Convert.ToString(tbl.Rows[0]["IDType"]).ToLower() == "passport")
                    {
                        model.PassportNo = Convert.ToString(tbl.Rows[0]["IDNumber"]);
                    }
                    if (Convert.ToString(tbl.Rows[0]["IDType"]).ToLower() == "pan")
                    {
                        model.PanCardNo = Convert.ToString(tbl.Rows[0]["IDNumber"]);
                    }
                    if (Convert.ToString(tbl.Rows[0]["IDType"]).ToLower() == "aadhar")
                    {
                        model.AadhaarCardNo = Convert.ToString(tbl.Rows[0]["IDNumber"]);
                    }
                    if (Convert.ToString(tbl.Rows[0]["IDType"]).ToLower() == "voter id")
                    {
                        model.VoterIDCardNo = Convert.ToString(tbl.Rows[0]["IDNumber"]);
                    }
                    model.Address = Convert.ToString(tbl.Rows[0]["Address"]);
                    model.DurationOfStay = Convert.ToString(tbl.Rows[0]["DurationOfStay"]);


                }
                if (loa.Rows.Count > 0)
                {
                    model.LOA_Name = Convert.ToString(loa.Rows[0]["LOA_Name"]);
                    model.hfSign = Convert.ToString(loa.Rows[0]["LOA_Signature"]);
                }

            }
            catch (Exception ex)
            {
                string str = ex.Message;

            }
            return model;
        }

        public void UpdateTempPQPersonalInfo(PQPersonalInfoViewModel model)
        {
            try
            {
                DAS.ExecuteNonQuery(DAS.ConnectionString, CommandType.StoredProcedure, "PersonalInfo_tblDataEntryTemp",
                DAS.CreateParameter("@PersonalRowID", SqlDbType.Int, model.PersonalRowID, ParameterDirection.Input, 10, false),
                 DAS.CreateParameter("@AlternateNo", SqlDbType.VarChar, model.AlternateNo, ParameterDirection.Input, 50, false),
                  DAS.CreateParameter("@Address", SqlDbType.VarChar, model.Address, ParameterDirection.Input, 2000, false),
                 DAS.CreateParameter("@AlternateEmailID", SqlDbType.VarChar, model.AlternateEmailID, ParameterDirection.Input, 100, false),
                  DAS.CreateParameter("@DurationOfStay", SqlDbType.VarChar, model.DurationOfStay, ParameterDirection.Input, 50, false)
                );
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
        }
        public string UpdatetempPQCandidateChecksData(int crid, long prid)
        {
            string result = "ok";
            try
            {

                DAS.ExecuteNonQuery(DAS.ConnectionString, CommandType.StoredProcedure, "usp_UpdatetempPQCandidateChecks",
                DAS.CreateParameter("@ClientRowID", SqlDbType.Int, crid, ParameterDirection.Input, 10, false),
                DAS.CreateParameter("@PersonalRowID", SqlDbType.BigInt, prid, ParameterDirection.Input, 10, false)
                );
            }
            catch (Exception ex)
            {
                string str = ex.Message;
                result = "";
            }
            return result; 
        }

        public QuickCaseSearch QuickCaseSearch(string id, int ClientRowID, string candidateName = "")
        {
            QuickCaseSearch model = new QuickCaseSearch();
            DataTable dt = new DataTable();
            dt = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "QuickCaseSearch",
                DAS.CreateParameter("@CompanyRefNo", SqlDbType.VarChar, id, ParameterDirection.Input, 50, false),
                DAS.CreateParameter("@CandidateFullName", SqlDbType.NVarChar, candidateName, ParameterDirection.Input, 50, false),
                    DAS.CreateParameter("@ClientRowId", SqlDbType.Int, ClientRowID, ParameterDirection.Input, 10, false)
                );
            if (dt.Rows.Count > 0)
            {
                model.CaseStatusID = Convert.ToInt32(dt.Rows[0]["CaseStausID"]);
                model.ColorCode = Convert.ToString(dt.Rows[0]["ReportColor"]);
                model.companyrefno = Convert.ToString(dt.Rows[0]["CompanyRefNo"]);
                model.PersonalRowID = Convert.ToString(dt.Rows[0]["PersonalRowID"]);
                //model.FinalReport = Convert.ToString(dt.Rows[0]["FinalReport"]);
                try
                {
                    model.UploadedDate = Convert.ToDateTime(dt.Rows[0]["UploadedDate"]).ToString("dd-MM-yyyy");

                }
                catch
                {
                    model.UploadedDate = Convert.ToString(dt.Rows[0]["UploadedDate"]).ToString();

                }
                model.CaseStatus = Convert.ToString(dt.Rows[0]["CaseStatus"]);
                try
                {
                    model.CreatedDate = Convert.ToDateTime(dt.Rows[0]["CreatedDate"]).ToString("dd-MM-yyyy");

                }
                catch
                {
                    model.CreatedDate = Convert.ToString(dt.Rows[0]["CreatedDate"]).ToString();

                }
            }
            return model;
        }

        public GetLocationByPin GetLocationByPincode(int Pincode)
        {
            GetLocationByPin model = new GetLocationByPin();
            DataTable dt = new DataTable();
            dt = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "GetLocationsByPincode",
                DAS.CreateParameter("@PinCode", SqlDbType.Int, Pincode, ParameterDirection.Input, 50, false)
                );
            if (dt.Rows.Count > 0)
            {
                model.DistrictName = Convert.ToString(dt.Rows[0]["DistrictName"]);
                model.StateName = Convert.ToString(dt.Rows[0]["StateName"]);
                model.CountryName = Convert.ToString(dt.Rows[0]["CountryName"]);
                model.DistrictRowID = Convert.ToInt32(dt.Rows[0]["DistrictRowID"]);
                model.StateRowID = Convert.ToInt32(dt.Rows[0]["StateRowID"]);
                model.CountryRowID = Convert.ToInt32(dt.Rows[0]["CountryRowID"]);
            }
            return model;
        }

        public IEnumerable<SubcheckViewModel> getAddresscheckName(int PersonalRowID)
        {
            try
            {
                List<SubcheckViewModel> modellist = new List<SubcheckViewModel>();
                DataTable dt = cc.GetAddressChecksName(PersonalRowID);
                modellist = (from DataRow dr in dt.Rows
                             select new SubcheckViewModel()
                             {
                                 SubCheckRowID = Convert.ToString(dr["SubCheckFamilyID"]),
                                 SubCheckName = Convert.ToString(dr["SubCheckName"])
                             }).ToList();
                return modellist;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public AddressCheckDetailsViewModel GetCandidateAddress(int PersonalRowId, int SubCheckRowId)
        {
            try
            {
                AddressCheckDetailsViewModel Objmodel = new AddressCheckDetailsViewModel();
                DataTable dt = cc.GetCandidateAddress(PersonalRowId, SubCheckRowId);
                if (dt.Rows.Count > 0)
                {
                    Objmodel.CandidateName = Convert.ToString(dt.Rows[0]["CandidateName"]);
                    Objmodel.Address = Convert.ToString(dt.Rows[0]["Address"]);
                    Objmodel.DurationOfStay = Convert.ToString(dt.Rows[0]["DurationofStay"]);
                    if (Convert.ToString(dt.Rows[0]["Country"]).ToLower() == "india")
                    {
                        Objmodel.Country = "India";
                    }
                    else
                    {
                        if (Convert.ToString(dt.Rows[0]["Country"]) == "")
                        {
                            Objmodel.Country = "";
                        }
                        else
                        {
                            Objmodel.Country = "Oberseas";
                        }
                    }
                }
                return Objmodel;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private string getCollgeName(int PersonalRowID, int SubCheckRowID)
        {
            string collegeName = "";
            DataTable dt = new DataTable();
            dt = cc.GetCollegeNameBasedonPersonalRowIDSubCheckFamilyID(PersonalRowID, SubCheckRowID);
            foreach (DataRow dr in dt.Rows)
            {
                collegeName = dr["DateEntryQCValue"].ToString();
            }
            return collegeName;
        }
        private string getEmploymentName(int PersonalRowID, int SubCheckRowID)
        {
            string employmentName = "";
            DataTable dt = new DataTable();
            dt = cc.GetEmploymentNameBasedonPersonalRowIDSubCheckFamilyID(PersonalRowID, SubCheckRowID);
            foreach (DataRow dr in dt.Rows)
            {
                employmentName = dr["DateEntryQCValue"].ToString();
            }
            return employmentName;
        }

        public PQPersonalDetailsVerViewModel GetEduHisContactEmail(int personalRowid = 0, int subcheckRowid = 0)
        {
            try
            {
                PQPersonalDetailsVerViewModel model = new PQPersonalDetailsVerViewModel();

                ClientCustomize cc = new ClientCustomize();
                DataTable dt = cc.GetEduHisContactEmail(personalRowid, subcheckRowid);
                if (dt.Rows.Count > 0)
                {
                    model.PersonalRowID = Convert.ToInt32(dt.Rows[0]["PersonalRowID"].ToString());
                    model.SubCheckRowID = Convert.ToInt16(dt.Rows[0]["SubCheckRowID"].ToString());
                    model.CheckFamilyRowID = Convert.ToInt16(dt.Rows[0]["CheckFamilyRowID"].ToString());
                    model.AntecedentRowID = dt.Rows[0]["AntecedentRowID"].ToString();
                    model.DataEntryRowID = dt.Rows[0]["DataEntryRowID"].ToString();
                    model.OfficialEmailID = dt.Rows[0]["OfficialEmailID"].ToString();
                    model.ContactNumber = dt.Rows[0]["ContactNumber"].ToString();
                    if (model.CheckFamilyRowID == 11)
                    {
                        model.collegeName = getCollgeName(model.PersonalRowID, model.SubCheckRowID);
                    }
                    else if (model.CheckFamilyRowID == 12)
                    {
                        model.employmentName = getEmploymentName(model.PersonalRowID, model.SubCheckRowID);
                    }
                    else
                    {
                        model.collegeName = "";
                        model.employmentName = "";
                    }
                }
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
