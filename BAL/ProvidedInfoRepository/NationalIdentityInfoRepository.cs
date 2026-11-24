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
using ViewModels.VerificationViewModel;
using System.Data;

namespace BAL.ProvidedInfoRepository
{
    public class NationalIdentityInfoRepository : INationalIdentityInfoRepository
    {
        DataContext db;
        ClientCustomize cc;
        public NationalIdentityInfoRepository()
        {
            db = new DataContext();
            cc = new ClientCustomize();
        }

        public void AddNationalIdentityInformation(AddPQNationalIdentityViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            PQNationalIdentity entity = new PQNationalIdentity();
                            entity.ClientRowID = model.ClientRowID;
                            entity.PersonalRowID = model.PersonalRowID;
                            entity.ClientPackageRowID = model.ClientPackageRowID;
                            entity.CheckFamilyRowID = model.CheckFamilyRowID;
                            entity.SubCheckRowID = model.SubCheckRowID;
                            entity.UniqueComponentID = model.UniqueComponentID;
                            entity.NIC_Cand_Name = model.NIC_Cand_Name;
                            entity.NIC_Sec_Ref_Id = model.NIC_Sec_Ref_Id;
                            entity.NIC_DOB = model.NIC_DOB;
                            entity.NIC_DOB1 = model.NIC_DOB1;
                            entity.NIC_ID_No = model.NIC_ID_No;
                            entity.NIC_Date_Issue = model.NIC_Date_Issue;
                            entity.NIC_Expiry_Date = model.NIC_Expiry_Date;
                            entity.NIC_Passport_No = model.NIC_Passport_No;
                            entity.NIC_SSN_NO = model.NIC_SSN_NO;
                            entity.NIC_Pan_No = model.NIC_Pan_No;
                            entity.NIC_Voter_Id_No = model.NIC_Voter_Id_No;
                            entity.NIC_Dvng_Liscense_No = model.NIC_Dvng_Liscense_No;
                            entity.NIC_Aadhar_Card_No = model.NIC_Aadhar_Card_No;
                            entity.NIC_Place_Issue = model.NIC_Place_Issue;
                            entity.NIC_Place_Residnce = model.NIC_Place_Residnce;
                            entity.NIC_Type_Id_Card = model.NIC_Type_Id_Card;
                            entity.NIC_Reg_Doc_Type = model.NIC_Reg_Doc_Type;
                            entity.NIC_RegNo = model.NIC_RegNo;
                            entity.NIC_PinCode = model.NIC_PinCode;
                            entity.NIC_Name_Key_Personnel = model.NIC_Name_Key_Personnel;
                            entity.NIC_AliasFname = model.NIC_AliasFname;
                            entity.NIC_Alias_Lname = model.NIC_Alias_Lname;
                            entity.NIC_Country_Iss_Passport = model.NIC_Country_Iss_Passport;
                            entity.NIC_Gender = model.NIC_Gender;
                            entity.NIC_Id_CardNo = model.NIC_Id_CardNo;
                            entity.NIC_Identity_Prov = model.NIC_Identity_Prov;
                            entity.NIC_Name_Changed_Effect_Frm = model.NIC_Name_Changed_Effect_Frm;
                            entity.NIC_Nationality = model.NIC_Nationality;
                            entity.NIC_OtherDetails = model.NIC_OtherDetails;
                            entity.NIC_OtherDetails2 = model.NIC_OtherDetails2;
                            entity.NIC_OtherDetails3 = model.NIC_OtherDetails3;
                            entity.NIC_OtherDetails4 = model.NIC_OtherDetails4;
                            entity.NIC_OtherDetails5 = model.NIC_OtherDetails5;
                            entity.NIC_OtherDetails6 = model.NIC_OtherDetails6;
                            entity.NIC_OtherDetails7 = model.NIC_OtherDetails7;
                            entity.NIC_OtherDetails8 = model.NIC_OtherDetails8;
                            entity.NIC_OtherDetails9 = model.NIC_OtherDetails9;
                            entity.NIC_OtherDetails10 = model.NIC_OtherDetails10;
                            entity.ATA_CID_No = model.ATA_CID_No;
                            entity.ATA_Cmpny_Addr = model.ATA_Cmpny_Addr;
                            entity.CreatedBy = model.CreatedBy;
                            entity.CreatedDate = model.CreatedDate;
                            entity.OutDate = model.OutDate;
                            entity.InternalOutDate = model.InternalOutDate;
                            entity.ReWorkCheckStatus = model.ReWorkCheckStatus;

                            entity.CheckStatus = model.CheckStatus;
                            if (model.CheckStatus == "Completed")
                            {
                                entity.CheckStatus = "Completed-DE";
                            }

                            entity.Remarks = model.Remarks;
                            entity.Status = model.Status;
                            context.PQNationalIdentities.Add(entity);
                            context.SaveChanges();

                            #region Save Check Status

                            PQNationalIdentityStatus chkEntity = new PQNationalIdentityStatus();
                            var nicModel = context.PQNationalIdentities.Include("PQPersonal").Where(p => p.NationalIdentityRowID == entity.NationalIdentityRowID).FirstOrDefault();
                            chkEntity.NationalIdentityRowID = nicModel.NationalIdentityRowID;
                            chkEntity.DETLAllocatedTo = nicModel.PQPersonal.AllocatedBy;
                            chkEntity.DETMCheckStatus = model.CheckStatus;
                            chkEntity.DETMAllocatedTo = nicModel.PQPersonal.AllocatedToDE;
                            chkEntity.DETMAllocatedDate = nicModel.PQPersonal.AllocatedToDEDate;
                            chkEntity.DETMQCAllocatedTo = nicModel.PQPersonal.AllocatedToDEQC;
                            chkEntity.DETMQCAllocatedDate = nicModel.PQPersonal.AllocatedToDEQCDate;

                            if (model.CheckStatus == "Completed")
                            {
                                chkEntity.DETMQCCheckStatus = model.CheckStatus;
                            }
                            if (model.CheckStatus == "Insufficent")
                            {
                                chkEntity.DETMQCCheckStatus = model.CheckStatus;
                                chkEntity.InfSuffRaiseBy = nicModel.PQPersonal.AllocatedToDE;
                                chkEntity.InfSuffRaisedDate = nicModel.CreatedDate;
                                chkEntity.InfSuffRaiseRemarks = model.INFRemarks;
                            }
                            if (model.CheckStatus == "Rejected" || model.CheckStatus == "Review Requested")
                            {
                                chkEntity.Remarks = model.INFRemarks;
                            }
                            context.PQNationalIdentityStatus.Add(chkEntity);
                            context.SaveChanges();

                            #endregion

                            transaction.Commit();
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

        public int GetAllNationalIdentityByClientId(short ClientRowID = 0)
        {
            try
            {
                var data = db.PQNationalIdentities.Where(p => p.ClientRowID == ClientRowID).ToList();
                return data.Count();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public AddPQNationalIdentityViewModel GetNationalIdentityInfoForDataEntry(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0)
        {
            try
            {
                return db.PQNationalIdentities.Where(a => a.ClientRowID == ClientRowId && a.PersonalRowID == PersonalRowID
                && a.CheckFamilyRowID == ChFamilRowId && a.SubCheckRowID == subCheckId).Select(a => new AddPQNationalIdentityViewModel
                {
                    NationalIdentityRowID = a.NationalIdentityRowID,
                    ClientRowID = a.ClientRowID,
                    PersonalRowID = a.PersonalRowID,
                    ClientPackageRowID = a.ClientPackageRowID,
                    CheckFamilyRowID = a.CheckFamilyRowID,
                    SubCheckRowID = a.SubCheckRowID,
                    UniqueComponentID = a.UniqueComponentID,
                    //NIC_Cand_Name = a.NIC_Cand_Name,
                    NIC_Cand_Name = db.PQPersonals.Where(T => T.PersonalRowID == a.PersonalRowID).Select(x => x.Name + " " + x.MiddleName + x.LastName).FirstOrDefault().ToString(),
                    NIC_DOB = a.NIC_DOB,
                    NIC_DOB1 = a.NIC_DOB1,
                    NIC_ID_No = a.NIC_ID_No,
                    NIC_Date_Issue = a.NIC_Date_Issue,
                    NIC_Expiry_Date = a.NIC_Expiry_Date,
                    NIC_Passport_No = a.NIC_Passport_No,
                    NIC_SSN_NO = a.NIC_SSN_NO,
                    NIC_Pan_No = a.NIC_Pan_No,
                    NIC_Voter_Id_No = a.NIC_Voter_Id_No,
                    NIC_Dvng_Liscense_No = a.NIC_Dvng_Liscense_No,
                    NIC_Aadhar_Card_No = a.NIC_Aadhar_Card_No,
                    NIC_Place_Issue = a.NIC_Place_Issue,
                    NIC_Place_Residnce = a.NIC_Place_Residnce,
                    NIC_Type_Id_Card = a.NIC_Type_Id_Card,
                    NIC_Reg_Doc_Type = a.NIC_Reg_Doc_Type,
                    NIC_RegNo = a.NIC_RegNo,
                    NIC_PinCode = a.NIC_PinCode,
                    NIC_Name_Key_Personnel = a.NIC_Name_Key_Personnel,
                    NIC_AliasFname = a.NIC_AliasFname,
                    NIC_Alias_Lname = a.NIC_Alias_Lname,
                    NIC_Country_Iss_Passport = a.NIC_Country_Iss_Passport,
                    NIC_Gender = a.NIC_Gender,
                    NIC_Id_CardNo = a.NIC_Id_CardNo,
                    NIC_Identity_Prov = a.NIC_Identity_Prov,
                    NIC_Name_Changed_Effect_Frm = a.NIC_Name_Changed_Effect_Frm,
                    NIC_Nationality = a.NIC_Nationality,
                    NIC_OtherDetails = a.NIC_OtherDetails,
                    NIC_OtherDetails2 = a.NIC_OtherDetails2,
                    NIC_OtherDetails3 = a.NIC_OtherDetails3,
                    NIC_OtherDetails4 = a.NIC_OtherDetails4,
                    NIC_OtherDetails5 = a.NIC_OtherDetails5,
                    NIC_OtherDetails6 = a.NIC_OtherDetails6,
                    NIC_OtherDetails7 = a.NIC_OtherDetails7,
                    NIC_OtherDetails8 = a.NIC_OtherDetails8,
                    NIC_OtherDetails9 = a.NIC_OtherDetails9,
                    NIC_OtherDetails10 = a.NIC_OtherDetails10,
                    ATA_CID_No = a.ATA_CID_No,
                    ATA_Cmpny_Addr = a.ATA_Cmpny_Addr,
                    CheckStatus = a.CheckStatus,
                    ReWorkCheckStatus = a.ReWorkCheckStatus,
                }).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateNationalIdentityInfoByDataEntry(AddPQNationalIdentityViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_DOB = model.NIC_DOB;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_DOB1 = model.NIC_DOB1;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_ID_No = model.NIC_ID_No;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_Date_Issue = model.NIC_Date_Issue;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_Expiry_Date = model.NIC_Expiry_Date;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_Passport_No = model.NIC_Passport_No;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_SSN_NO = model.NIC_SSN_NO;
                            //context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_Cand_Name = model.NIC_Cand_Name;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_Pan_No = model.NIC_Pan_No;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_Voter_Id_No = model.NIC_Voter_Id_No;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_Dvng_Liscense_No = model.NIC_Dvng_Liscense_No;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_Aadhar_Card_No = model.NIC_Aadhar_Card_No;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_Place_Issue = model.NIC_Place_Issue;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_Place_Residnce = model.NIC_Place_Residnce;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_Type_Id_Card = model.NIC_Type_Id_Card;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_Reg_Doc_Type = model.NIC_Reg_Doc_Type;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_RegNo = model.NIC_RegNo;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_PinCode = model.NIC_PinCode;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_Name_Key_Personnel = model.NIC_Name_Key_Personnel;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_AliasFname = model.NIC_AliasFname;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_Alias_Lname = model.NIC_Alias_Lname;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_Country_Iss_Passport = model.NIC_Country_Iss_Passport;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_Gender = model.NIC_Gender;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_Id_CardNo = model.NIC_Id_CardNo;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_Identity_Prov = model.NIC_Identity_Prov;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_Name_Changed_Effect_Frm = model.NIC_Name_Changed_Effect_Frm;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_Nationality = model.NIC_Nationality;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_OtherDetails = model.NIC_OtherDetails;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_OtherDetails2 = model.NIC_OtherDetails2;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_OtherDetails3 = model.NIC_OtherDetails3;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_OtherDetails4 = model.NIC_OtherDetails4;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_OtherDetails5 = model.NIC_OtherDetails5;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_OtherDetails6 = model.NIC_OtherDetails6;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_OtherDetails7 = model.NIC_OtherDetails7;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_OtherDetails8 = model.NIC_OtherDetails8;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_OtherDetails9 = model.NIC_OtherDetails9;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_OtherDetails10 = model.NIC_OtherDetails10;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).ATA_CID_No = model.ATA_CID_No;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).ATA_Cmpny_Addr = model.ATA_Cmpny_Addr;

                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).CheckStatus = model.CheckStatus;
                            if (model.CheckStatus == "Completed")
                            {
                                context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).CheckStatus = "Completed-DE";
                            }
                            else if (model.CheckStatus == "Not Applicable")
                            {
                                context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).CheckStatus = "Not Applicable";
                            }

                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).ModifiedBy = model.CreatedBy;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).ModifiedDate = model.CreatedDate;
                            context.SaveChanges();

                            #region Save Check Status

                            var status = context.PQNationalIdentityStatus.Where(p => p.NationalIdentityRowID == model.NationalIdentityRowID).FirstOrDefault();

                            if (status.TMCheckStatus == "Insufficent" && status.TMQCCheckStatus == "Insufficent-QC" && model.CheckStatus == "Completed")
                            {
                                context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).CheckStatus = "WIP-Pending";
                                context.SaveChanges();

                                context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == status.NationalIdentityStatusRowID).DETMCheckStatus = model.CheckStatus;
                                context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == status.NationalIdentityStatusRowID).DETMAllocatedDate = model.CreatedDate;
                                context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == status.NationalIdentityStatusRowID).TMCheckStatus = "WIP-Pending";
                                context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == status.NationalIdentityStatusRowID).TMAllocatedDate = model.CreatedDate;
                                context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == status.NationalIdentityStatusRowID).TMQCCheckStatus = "";
                                context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == status.NationalIdentityStatusRowID).TMQCAllocatedDate = model.CreatedDate;
                            }
                           else if ( model.CheckStatus == "Not Applicable")
                            {
                                context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).CheckStatus = "Not Applicable";
                                context.SaveChanges();
                            }
                            else
                            {
                                context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == status.NationalIdentityStatusRowID).DETMCheckStatus = model.CheckStatus;
                                context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == status.NationalIdentityStatusRowID).DETMAllocatedDate = model.CreatedDate;

                                if (model.CheckStatus == "Completed")
                                {
                                    context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == status.NationalIdentityStatusRowID).DETMQCCheckStatus = model.CheckStatus;
                                    context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == status.NationalIdentityStatusRowID).DETMQCAllocatedDate = model.CreatedDate;
                                }
                                if (model.CheckStatus == "Insufficent")
                                {
                                    context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == status.NationalIdentityStatusRowID).DETMQCCheckStatus = model.CheckStatus;
                                    context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == status.NationalIdentityStatusRowID).DETMQCAllocatedDate = model.CreatedDate;
                                    context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == status.NationalIdentityStatusRowID).InfSuffRaiseBy = model.CreatedBy;
                                    context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == status.NationalIdentityStatusRowID).InfSuffRaisedDate = model.CreatedDate;
                                    context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == status.NationalIdentityStatusRowID).InfSuffRaiseRemarks = model.INFRemarks;
                                }
                                if (model.CheckStatus == "Rejected" || model.CheckStatus == "Review Requested")
                                {
                                    context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == status.NationalIdentityStatusRowID).Remarks = model.INFRemarks;
                                }
                            }
                            context.SaveChanges();

                            #endregion

                            transaction.Commit();
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

        public UpdatePQNationalIdentityViewModel GetNationalIdentityInfoForQC(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0)
        {
            try
            {
                return db.PQNationalIdentities.Where(a => a.ClientRowID == ClientRowId && a.PersonalRowID == PersonalRowID
                && a.CheckFamilyRowID == ChFamilRowId && a.SubCheckRowID == subCheckId).Select(a => new UpdatePQNationalIdentityViewModel
                {
                    NationalIdentityRowID = a.NationalIdentityRowID,
                    ClientRowID = a.ClientRowID,
                    PersonalRowID = a.PersonalRowID,
                    ClientPackageRowID = a.ClientPackageRowID,
                    CheckFamilyRowID = a.CheckFamilyRowID,
                    SubCheckRowID = a.SubCheckRowID,
                    NIC_Cand_Name = a.NIC_Cand_Name,
                    UniqueComponentID = a.UniqueComponentID,
                    NIC_DOB = a.NIC_DOB,
                    NIC_DOB1 = a.NIC_DOB1,
                    NIC_ID_No = a.NIC_ID_No,
                    NIC_Date_Issue = a.NIC_Date_Issue,
                    NIC_Expiry_Date = a.NIC_Expiry_Date,
                    NIC_Passport_No = a.NIC_Passport_No,
                    NIC_SSN_NO = a.NIC_SSN_NO,
                    NIC_Pan_No = a.NIC_Pan_No,
                    NIC_Voter_Id_No = a.NIC_Voter_Id_No,
                    NIC_Dvng_Liscense_No = a.NIC_Dvng_Liscense_No,
                    NIC_Aadhar_Card_No = a.NIC_Aadhar_Card_No,
                    NIC_Place_Issue = a.NIC_Place_Issue,
                    NIC_Place_Residnce = a.NIC_Place_Residnce,
                    NIC_Type_Id_Card = a.NIC_Type_Id_Card,
                    NIC_Reg_Doc_Type = a.NIC_Reg_Doc_Type,
                    NIC_RegNo = a.NIC_RegNo,
                    NIC_PinCode = a.NIC_PinCode,
                    NIC_Name_Key_Personnel = a.NIC_Name_Key_Personnel,
                    NIC_AliasFname = a.NIC_AliasFname,
                    NIC_Alias_Lname = a.NIC_Alias_Lname,
                    NIC_Country_Iss_Passport = a.NIC_Country_Iss_Passport,
                    NIC_Gender = a.NIC_Gender,
                    NIC_Id_CardNo = a.NIC_Id_CardNo,
                    NIC_Identity_Prov = a.NIC_Identity_Prov,
                    NIC_Name_Changed_Effect_Frm = a.NIC_Name_Changed_Effect_Frm,
                    NIC_Nationality = a.NIC_Nationality,
                    NIC_OtherDetails = a.NIC_OtherDetails,
                    NIC_OtherDetails2 = a.NIC_OtherDetails2,
                    NIC_OtherDetails3 = a.NIC_OtherDetails3,
                    NIC_OtherDetails4 = a.NIC_OtherDetails4,
                    NIC_OtherDetails5 = a.NIC_OtherDetails5,
                    NIC_OtherDetails6 = a.NIC_OtherDetails6,
                    NIC_OtherDetails7 = a.NIC_OtherDetails7,
                    NIC_OtherDetails8 = a.NIC_OtherDetails8,
                    NIC_OtherDetails9 = a.NIC_OtherDetails9,
                    NIC_OtherDetails10 = a.NIC_OtherDetails10,
                    ATA_CID_No = a.ATA_CID_No,
                    ATA_Cmpny_Addr = a.ATA_Cmpny_Addr,
                    CheckStatus = a.CheckStatus,
                    ReWorkCheckStatus = a.ReWorkCheckStatus,
                    //RejectionReason = a.INFRemarks,
                }).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public UpdatePQNationalIdentityViewModel GetNationalIdentityInfoForQC(int NationalIdentityRowID = 0)
        {
            try
            {
                return db.PQNationalIdentities.Where(a => a.NationalIdentityRowID == NationalIdentityRowID).Select(a => new UpdatePQNationalIdentityViewModel
                {
                    NationalIdentityRowID = a.NationalIdentityRowID,
                    ClientRowID = a.ClientRowID,
                    PersonalRowID = a.PersonalRowID,
                    ClientPackageRowID = a.ClientPackageRowID,
                    CheckFamilyRowID = a.CheckFamilyRowID,
                    SubCheckRowID = a.SubCheckRowID,
                    NIC_Cand_Name = a.NIC_Cand_Name,
                    UniqueComponentID = a.UniqueComponentID,
                    NIC_DOB = a.NIC_DOB,
                    NIC_DOB1 = a.NIC_DOB1,
                    NIC_ID_No = a.NIC_ID_No,
                    NIC_Date_Issue = a.NIC_Date_Issue,
                    NIC_Expiry_Date = a.NIC_Expiry_Date,
                    NIC_Passport_No = a.NIC_Passport_No,
                    NIC_SSN_NO = a.NIC_SSN_NO,
                    NIC_Pan_No = a.NIC_Pan_No,
                    NIC_Voter_Id_No = a.NIC_Voter_Id_No,
                    NIC_Dvng_Liscense_No = a.NIC_Dvng_Liscense_No,
                    NIC_Aadhar_Card_No = a.NIC_Aadhar_Card_No,
                    NIC_Place_Issue = a.NIC_Place_Issue,
                    NIC_Place_Residnce = a.NIC_Place_Residnce,
                    NIC_Type_Id_Card = a.NIC_Type_Id_Card,
                    NIC_Reg_Doc_Type = a.NIC_Reg_Doc_Type,
                    NIC_RegNo = a.NIC_RegNo,
                    NIC_PinCode = a.NIC_PinCode,
                    NIC_Name_Key_Personnel = a.NIC_Name_Key_Personnel,
                    NIC_AliasFname = a.NIC_AliasFname,
                    NIC_Alias_Lname = a.NIC_Alias_Lname,
                    NIC_Country_Iss_Passport = a.NIC_Country_Iss_Passport,
                    NIC_Gender = a.NIC_Gender,
                    NIC_Id_CardNo = a.NIC_Id_CardNo,
                    NIC_Identity_Prov = a.NIC_Identity_Prov,
                    NIC_Name_Changed_Effect_Frm = a.NIC_Name_Changed_Effect_Frm,
                    NIC_Nationality = a.NIC_Nationality,
                    NIC_OtherDetails = a.NIC_OtherDetails,
                    NIC_OtherDetails2 = a.NIC_OtherDetails2,
                    NIC_OtherDetails3 = a.NIC_OtherDetails3,
                    NIC_OtherDetails4 = a.NIC_OtherDetails4,
                    NIC_OtherDetails5 = a.NIC_OtherDetails5,
                    NIC_OtherDetails6 = a.NIC_OtherDetails6,
                    NIC_OtherDetails7 = a.NIC_OtherDetails7,
                    NIC_OtherDetails8 = a.NIC_OtherDetails8,
                    NIC_OtherDetails9 = a.NIC_OtherDetails9,
                    NIC_OtherDetails10 = a.NIC_OtherDetails10,
                    ATA_CID_No = a.ATA_CID_No,
                    ATA_Cmpny_Addr = a.ATA_Cmpny_Addr,
                    CheckStatus = a.CheckStatus,
                    ReWorkCheckStatus = a.ReWorkCheckStatus,
                    //RejectionReason = a.INFRemarks,
                }).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateNationalIdentityInfoByQC(UpdatePQNationalIdentityViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_DOB = model.NIC_DOB;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_DOB1 = model.NIC_DOB1;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_ID_No = model.NIC_ID_No;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_Date_Issue = model.NIC_Date_Issue;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_Expiry_Date = model.NIC_Expiry_Date;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_Passport_No = model.NIC_Passport_No;
                            //context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_Cand_Name = model.NIC_Cand_Name;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_SSN_NO = model.NIC_SSN_NO;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_Pan_No = model.NIC_Pan_No;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_Voter_Id_No = model.NIC_Voter_Id_No;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_Dvng_Liscense_No = model.NIC_Dvng_Liscense_No;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_Aadhar_Card_No = model.NIC_Aadhar_Card_No;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_Place_Issue = model.NIC_Place_Issue;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_Place_Residnce = model.NIC_Place_Residnce;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_Type_Id_Card = model.NIC_Type_Id_Card;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_Reg_Doc_Type = model.NIC_Reg_Doc_Type;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_RegNo = model.NIC_RegNo;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_PinCode = model.NIC_PinCode;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_Name_Key_Personnel = model.NIC_Name_Key_Personnel;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_AliasFname = model.NIC_AliasFname;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_Alias_Lname = model.NIC_Alias_Lname;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_Country_Iss_Passport = model.NIC_Country_Iss_Passport;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_Gender = model.NIC_Gender;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_Id_CardNo = model.NIC_Id_CardNo;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_Identity_Prov = model.NIC_Identity_Prov;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_Name_Changed_Effect_Frm = model.NIC_Name_Changed_Effect_Frm;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_Nationality = model.NIC_Nationality;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_OtherDetails = model.NIC_OtherDetails;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_OtherDetails2 = model.NIC_OtherDetails2;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_OtherDetails3 = model.NIC_OtherDetails3;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_OtherDetails4 = model.NIC_OtherDetails4;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_OtherDetails5 = model.NIC_OtherDetails5;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_OtherDetails6 = model.NIC_OtherDetails6;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_OtherDetails7 = model.NIC_OtherDetails7;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_OtherDetails8 = model.NIC_OtherDetails8;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_OtherDetails9 = model.NIC_OtherDetails9;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_OtherDetails10 = model.NIC_OtherDetails10;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).ATA_CID_No = model.ATA_CID_No;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).ATA_Cmpny_Addr = model.ATA_Cmpny_Addr;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).ModifiedBy = model.ModifiedBy;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).ModifiedDate = model.ModifiedDate;

                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).CheckStatus = model.CheckStatus;
                            if (model.CheckStatus == "Completed-DE")
                            {
                                context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).CheckStatus = "Allocated-Mgr";
                            }

                            context.SaveChanges();

                            #region Save Check Status

                            var CurrentStatus = context.PQNationalIdentityStatus.Where(p => p.NationalIdentityRowID == model.NationalIdentityRowID).FirstOrDefault();

                            if (model.CheckStatus == "Completed-DE")
                            {
                                if (CurrentStatus.TMCheckStatus == "Rejected")
                                {
                                    context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).CheckStatus = "WIP-Pending";
                                    context.SaveChanges();
                                    context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == CurrentStatus.NationalIdentityStatusRowID).DETMQCCheckStatus = "Completed-QC";
                                    context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == CurrentStatus.NationalIdentityStatusRowID).TMCheckStatus = "WIP-Pending";
                                    context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == CurrentStatus.NationalIdentityStatusRowID).TMAllocatedDate = model.ModifiedDate;
                                }
                                else
                                {
                                    context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == CurrentStatus.NationalIdentityStatusRowID).DETMQCCheckStatus = "Completed-QC";
                                    context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == CurrentStatus.NationalIdentityStatusRowID).MgrCheckStatus = "Allocated-Mgr";
                                    context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == CurrentStatus.NationalIdentityStatusRowID).MgrAllocatedTo = model.MgrAllocatedTo;
                                    context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == CurrentStatus.NationalIdentityStatusRowID).MgrAllocatedDate = model.ModifiedDate;
                                }

                            }

                            if (model.CheckStatus == "Insufficent")
                            {
                                context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == CurrentStatus.NationalIdentityStatusRowID).DETMQCCheckStatus = "Insufficent-QC";
                                context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == CurrentStatus.NationalIdentityStatusRowID).InfSuffClearBy = 0;
                                context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == CurrentStatus.NationalIdentityStatusRowID).InfSuffClearComments = "";
                                context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == CurrentStatus.NationalIdentityStatusRowID).InfSuffClearDate = null;
                            }

                            context.SaveChanges();

                            if (model.CheckStatus == "Insufficent")
                            {
                                context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).CaseStatus = "Insufficent";
                            }

                            context.SaveChanges();

                            #endregion

                            transaction.Commit();
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

        public void UpdateNationalIdentityInfoByManager(PQUpdateNationalIdentityManagerViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).CheckStatus = model.CheckStatus;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).ModifiedBy = model.ModifiedBy;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).ModifiedDate = model.ModifiedDate;
                            context.SaveChanges();

                            #region Save Check Status

                            int statusId = context.PQNationalIdentityStatus.Where(p => p.NationalIdentityRowID == model.NationalIdentityRowID).FirstOrDefault().NationalIdentityStatusRowID;

                            context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == statusId).TLCheckStatus = model.CheckStatus;
                            context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == statusId).TLAllocatedTo = model.TLAllocatedTo;
                            context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == statusId).TLAllocatedDate = model.ModifiedDate;
                            context.SaveChanges();

                            #endregion

                            transaction.Commit();
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

        public void UpdateNationalIdentityInfoByTeamLeader(PQUpdateNationalIdentityTeamLeaderViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).CheckStatus = model.CheckStatus;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).ModifiedBy = model.ModifiedBy;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).ModifiedDate = model.ModifiedDate;

                            context.SaveChanges();

                            #region Save Check Status

                            int statusId = context.PQNationalIdentityStatus.Where(p => p.NationalIdentityRowID == model.NationalIdentityRowID).FirstOrDefault().NationalIdentityStatusRowID;

                            context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == statusId).TMCheckStatus = model.CheckStatus;
                            context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == statusId).TMAllocatedTo = model.TMAllocatedTo;
                            context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == statusId).TMAllocatedDate = model.ModifiedDate;

                            context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == statusId).TMQCCheckStatus = model.CheckStatus;
                            context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == statusId).TMQCAllocatedTo = model.TMQCAllocatedTo;
                            context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == statusId).TMQCAllocatedDate = model.ModifiedDate;


                            context.SaveChanges();

                            #endregion

                            transaction.Commit();
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

        public void UpdateNationalIdentityInfoByTeamLeaderReAllocated(PQUpdateNationalIdentityTeamLeaderViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {

                            #region Save Check Status

                            int statusId = context.PQNationalIdentityStatus.Where(p => p.NationalIdentityRowID == model.NationalIdentityRowID).FirstOrDefault().NationalIdentityStatusRowID;
                            if (model.TMAllocatedTo > 0)
                            {
                                context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == statusId).TMAllocatedTo = model.TMAllocatedTo;
                                context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == statusId).TMAllocatedDate = model.ModifiedDate;
                            }
                            if (model.TMQCAllocatedTo > 0)
                            {
                                context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == statusId).TMQCAllocatedTo = model.TMQCAllocatedTo;
                                context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == statusId).TMQCAllocatedDate = model.ModifiedDate;
                            }

                            context.SaveChanges();

                            #endregion

                            transaction.Commit();
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

        public void UpdateNationalIdentityInfoByTeamMember(PQUpdateNationalIdentityTeamMemberViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).CheckStatus = model.CheckStatus;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).ModifiedBy = model.ModifiedBy;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).ModifiedDate = model.ModifiedDate;
                            context.SaveChanges();

                            #region Save Check Status

                            var statusId = context.PQNationalIdentityStatus.Where(p => p.NationalIdentityRowID == model.NationalIdentityRowID).FirstOrDefault().NationalIdentityStatusRowID;

                            context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == statusId).TMCheckStatus = model.CheckStatus;
                            context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == statusId).TMQCCheckStatus = "";
                            if (model.CheckStatus == "PV Raised")
                            {
                                context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == statusId).PVMgrCheckStatus = "Allocate-Mgr";
                                context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == statusId).PVMgrAllocatedTo = model.PVMgrAllocatedTo;
                                context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == statusId).PVMgrAllocatedDate = model.ModifiedDate;
                            }
                            if (model.CheckStatus == "Insufficent")
                            {
                                context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == statusId).TMQCCheckStatus = model.CheckStatus;
                                context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == statusId).InfSuffRaiseRemarks = model.ActionRemark;
                                context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == statusId).InfSuffRaiseBy = model.ModifiedBy;
                                context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == statusId).InfSuffRaisedDate = model.ModifiedDate;
                            }
                            if (model.CheckStatus == "Rejected" || model.CheckStatus == "Funds Pending")
                            {
                                context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == statusId).Remarks = model.ActionRemark;
                            }
                            context.SaveChanges();
                            #endregion

                            transaction.Commit();
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

        public void RejectionUpdateNationalIInfoByQC(UpdateNationalIRejectionViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        string ActualCheckStatus = "WIP-Pending";
                        if (model != null)
                        {
                            db.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).CheckStatus = ActualCheckStatus;
                            db.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).ModifiedBy = model.ModifiedBy;
                            db.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).ModifiedDate = model.ModifiedDate;
                            context.SaveChanges();

                            #region Update Check Status

                            int statusId = context.PQNationalIdentityStatus.Where(p => p.NationalIdentityRowID == model.NationalIdentityRowID).FirstOrDefault().NationalIdentityStatusRowID;
                            context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == statusId).DETMCheckStatus = ActualCheckStatus;
                            context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == statusId).DETMQCCheckStatus = "";
                            context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == statusId).Remarks = model.INFRemarks;
                            context.SaveChanges();

                            #endregion

                            transaction.Commit();
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

        public bool IfExistNationalIdentityInfo(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0, string CheckStatus = "")
        {
            try
            {
                var listChecks = db.PQNationalIdentities.Where(a => a.ClientRowID == ClientRowId && a.PersonalRowID == PersonalRowID
                     && a.CheckFamilyRowID == ChFamilRowId && a.SubCheckRowID == subCheckId && a.CheckStatus == CheckStatus).FirstOrDefault();
                if (listChecks != null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public bool IfExistNationalIdentityForDELeftMenu(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0)
        {
            try
            {
                var listChecks = db.PQNationalIdentities.Where(a => a.ClientRowID == ClientRowId && a.PersonalRowID == PersonalRowID
                     && a.CheckFamilyRowID == ChFamilRowId && a.SubCheckRowID == subCheckId).FirstOrDefault();
                if (listChecks != null)
                {
                    var ChkStatus = db.PQNationalIdentityStatus.Where(s => s.NationalIdentityRowID == listChecks.NationalIdentityRowID).FirstOrDefault();

                    if (ChkStatus.DETMCheckStatus == "WIP-Pending")
                        return true;
                    else
                        return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool IfExistNationalIdentityForQCLeftMenu(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0, int CheckStatusid = 0)
        {
            try
            {
               
               

                    if ( CheckStatusid == 68 || CheckStatusid == 14 || CheckStatusid == 15 || CheckStatusid == 42)
                        return true;
                    else
                        return false;
               
            }
            catch (Exception)
            {
                throw;
            }
        }

        #region ****** Temp NationalIdentity ********

        public void AddTempNationalIdentityInformation(AddTempPQNationalIdentityViewModel model)
        {
            try
            {
                if (model != null)
                {
                    TempPQNationalIdentity entity = new TempPQNationalIdentity();
                    entity.ClientRowID = model.ClientRowID;
                    entity.PersonalRowID = model.PersonalRowID;
                    entity.ClientPackageRowID = model.ClientPackageRowID;
                    entity.CheckFamilyRowID = model.CheckFamilyRowID;
                    entity.SubCheckRowID = model.SubCheckRowID;
                    entity.UniqueComponentID = model.UniqueComponentID;
                    //entity.NIC_Cand_Name = model.NIC_Cand_Name;
                    if (model.NIC_Cand_Name == "")
                    {
                        entity.NIC_Cand_Name = db.PQCandidateLogins.Where(w => w.TempPersonalRowID == model.PersonalRowID).FirstOrDefault().CandidateName;
                    }
                    else
                    {
                        entity.NIC_Cand_Name = model.NIC_Cand_Name;
                    }
                    entity.NIC_Sec_Ref_Id = model.NIC_Sec_Ref_Id;
                    entity.NIC_DOB = model.NIC_DOB;
                    entity.NIC_DOB1 = model.NIC_DOB1;
                    entity.NIC_ID_No = model.NIC_ID_No;
                    entity.NIC_Date_Issue = model.NIC_Date_Issue;
                    entity.NIC_Expiry_Date = model.NIC_Expiry_Date;
                    entity.NIC_Passport_No = model.NIC_Passport_No;
                    entity.NIC_SSN_NO = model.NIC_SSN_NO;
                    entity.NIC_Pan_No = model.NIC_Pan_No;
                    entity.NIC_Voter_Id_No = model.NIC_Voter_Id_No;
                    entity.NIC_Dvng_Liscense_No = model.NIC_Dvng_Liscense_No;
                    entity.NIC_Aadhar_Card_No = model.NIC_Aadhar_Card_No;
                    entity.NIC_Place_Issue = model.NIC_Place_Issue;
                    entity.NIC_Place_Residnce = model.NIC_Place_Residnce;
                    entity.NIC_Type_Id_Card = model.NIC_Type_Id_Card;
                    entity.NIC_Reg_Doc_Type = model.NIC_Reg_Doc_Type;
                    entity.NIC_RegNo = model.NIC_RegNo;
                    entity.NIC_PinCode = model.NIC_PinCode;
                    entity.NIC_Name_Key_Personnel = model.NIC_Name_Key_Personnel;
                    entity.NIC_AliasFname = model.NIC_AliasFname;
                    entity.NIC_Alias_Lname = model.NIC_Alias_Lname;
                    entity.NIC_Country_Iss_Passport = model.NIC_Country_Iss_Passport;
                    entity.NIC_Gender = model.NIC_Gender;
                    entity.NIC_Id_CardNo = model.NIC_Id_CardNo;
                    entity.NIC_Identity_Prov = model.NIC_Identity_Prov;
                    entity.NIC_Name_Changed_Effect_Frm = model.NIC_Name_Changed_Effect_Frm;
                    entity.NIC_Nationality = model.NIC_Nationality;
                    //entity.NIC_AadhaarCard = model.NIC_AadhaarCard;
                    //entity.NIC_DrivingLicense = model.NIC_DrivingLicense;
                    //entity.NIC_PANCard = model.NIC_PANCard;
                    //entity.NIC_Passport = model.NIC_Passport;
                    //entity.NIC_VoterID = model.NIC_VoterID;
                    //entity.NIC_OtherProof = model.NIC_OtherProof;
                    entity.NIC_OtherDetails = model.NIC_OtherDetails;
                    entity.NIC_OtherDetails2 = model.NIC_OtherDetails2;
                    entity.NIC_OtherDetails3 = model.NIC_OtherDetails3;
                    entity.NIC_OtherDetails4 = model.NIC_OtherDetails4;
                    entity.NIC_OtherDetails5 = model.NIC_OtherDetails5;
                    entity.NIC_OtherDetails6 = model.NIC_OtherDetails6;
                    entity.NIC_OtherDetails7 = model.NIC_OtherDetails7;
                    entity.NIC_OtherDetails8 = model.NIC_OtherDetails8;
                    entity.NIC_OtherDetails9 = model.NIC_OtherDetails9;
                    entity.NIC_OtherDetails10 = model.NIC_OtherDetails10;
                    entity.ATA_CID_No = model.ATA_CID_No;
                    entity.ATA_Cmpny_Addr = model.ATA_Cmpny_Addr;
                    entity.CreatedBy = model.CreatedBy;
                    entity.CreatedDate = model.CreatedDate;
                    entity.CheckStatus = model.CheckStatus;
                    entity.Remarks = model.Remarks;
                    entity.Status = model.Status;
                    db.TempPQNationalIdentities.Add(entity);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public int GetAllTempNationalIdentityByClientId(short ClientRowID = 0)
        {
            try
            {
                var data = db.TempPQNationalIdentities.Where(p => p.ClientRowID == ClientRowID).ToList();
                return data.Count();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public AddTempPQNationalIdentityViewModel GetTempNationalIdentityInfoForBySelf(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0)
        {
            try
            {
                return db.TempPQNationalIdentities.Where(a => a.ClientRowID == ClientRowId && a.PersonalRowID == PersonalRowID
                && a.CheckFamilyRowID == ChFamilRowId && a.SubCheckRowID == subCheckId).Select(a => new AddTempPQNationalIdentityViewModel
                {
                    NationalIdentityRowID = a.NationalIdentityRowID,
                    ClientRowID = a.ClientRowID,
                    PersonalRowID = a.PersonalRowID,
                    ClientPackageRowID = a.ClientPackageRowID,
                    CheckFamilyRowID = a.CheckFamilyRowID,
                    SubCheckRowID = a.SubCheckRowID,
                    UniqueComponentID = a.UniqueComponentID,
                    NIC_DOB = a.NIC_DOB,
                    NIC_DOB1 = a.NIC_DOB1,
                    NIC_ID_No = a.NIC_ID_No,
                    NIC_Date_Issue = a.NIC_Date_Issue,
                    NIC_Expiry_Date = a.NIC_Expiry_Date,
                    NIC_Passport_No = a.NIC_Passport_No,
                    NIC_SSN_NO = a.NIC_SSN_NO,
                    NIC_Pan_No = a.NIC_Pan_No,
                    NIC_Voter_Id_No = a.NIC_Voter_Id_No,
                    NIC_Dvng_Liscense_No = a.NIC_Dvng_Liscense_No,
                    NIC_Aadhar_Card_No = a.NIC_Aadhar_Card_No,
                    NIC_Place_Issue = a.NIC_Place_Issue,
                    NIC_Place_Residnce = a.NIC_Place_Residnce,
                    NIC_Type_Id_Card = a.NIC_Type_Id_Card,
                    NIC_Reg_Doc_Type = a.NIC_Reg_Doc_Type,
                    NIC_RegNo = a.NIC_RegNo,
                    NIC_PinCode = a.NIC_PinCode,
                    NIC_Name_Key_Personnel = a.NIC_Name_Key_Personnel,
                    NIC_AliasFname = a.NIC_AliasFname,
                    NIC_Alias_Lname = a.NIC_Alias_Lname,
                    NIC_Country_Iss_Passport = a.NIC_Country_Iss_Passport,
                    NIC_Gender = a.NIC_Gender,
                    NIC_Id_CardNo = a.NIC_Id_CardNo,
                    NIC_Identity_Prov = a.NIC_Identity_Prov,
                    NIC_Name_Changed_Effect_Frm = a.NIC_Name_Changed_Effect_Frm,
                    NIC_Nationality = a.NIC_Nationality,
                    //NIC_AadhaarCard = a.NIC_AadhaarCard,
                    //NIC_DrivingLicense = a.NIC_DrivingLicense,
                    //NIC_PANCard = a.NIC_PANCard,
                    //NIC_Passport = a.NIC_Passport,
                    //NIC_VoterID = a.NIC_VoterID,
                    //NIC_OtherProof = a.NIC_OtherProof,
                    NIC_OtherDetails = a.NIC_OtherDetails,
                    NIC_OtherDetails2 = a.NIC_OtherDetails2,
                    NIC_OtherDetails3 = a.NIC_OtherDetails3,
                    NIC_OtherDetails4 = a.NIC_OtherDetails4,
                    NIC_OtherDetails5 = a.NIC_OtherDetails5,
                    NIC_OtherDetails6 = a.NIC_OtherDetails6,
                    NIC_OtherDetails7 = a.NIC_OtherDetails7,
                    NIC_OtherDetails8 = a.NIC_OtherDetails8,
                    NIC_OtherDetails9 = a.NIC_OtherDetails9,
                    NIC_OtherDetails10 = a.NIC_OtherDetails10,
                    ATA_CID_No = a.ATA_CID_No,
                    ATA_Cmpny_Addr = a.ATA_Cmpny_Addr,
                    CheckStatus = a.CheckStatus,
                    NIC_Cand_Name = a.NIC_Cand_Name,
                }).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateTempNationalIdentityInfoBySelf(AddTempPQNationalIdentityViewModel model)
        {
            try
            {
                if (model != null)
                {
                    db.TempPQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_DOB = model.NIC_DOB;
                    db.TempPQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_DOB1 = model.NIC_DOB1;
                    db.TempPQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_ID_No = model.NIC_ID_No;
                    db.TempPQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_Date_Issue = model.NIC_Date_Issue;
                    db.TempPQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_Expiry_Date = model.NIC_Expiry_Date;
                    db.TempPQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_Passport_No = model.NIC_Passport_No;
                    db.TempPQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_SSN_NO = model.NIC_SSN_NO;
                    db.TempPQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_Pan_No = model.NIC_Pan_No;
                    db.TempPQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_Voter_Id_No = model.NIC_Voter_Id_No;
                    db.TempPQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_Dvng_Liscense_No = model.NIC_Dvng_Liscense_No;
                    db.TempPQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_Aadhar_Card_No = model.NIC_Aadhar_Card_No;
                    db.TempPQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_Place_Issue = model.NIC_Place_Issue;
                    db.TempPQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_Place_Residnce = model.NIC_Place_Residnce;
                    db.TempPQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_Type_Id_Card = model.NIC_Type_Id_Card;
                    db.TempPQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_Reg_Doc_Type = model.NIC_Reg_Doc_Type;
                    db.TempPQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_RegNo = model.NIC_RegNo;
                    db.TempPQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_PinCode = model.NIC_PinCode;
                    db.TempPQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_Name_Key_Personnel = model.NIC_Name_Key_Personnel;
                    db.TempPQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_AliasFname = model.NIC_AliasFname;
                    db.TempPQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_Alias_Lname = model.NIC_Alias_Lname;
                    db.TempPQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_Country_Iss_Passport = model.NIC_Country_Iss_Passport;
                    db.TempPQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_Gender = model.NIC_Gender;
                    db.TempPQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_Id_CardNo = model.NIC_Id_CardNo;
                    db.TempPQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_Identity_Prov = model.NIC_Identity_Prov;
                    db.TempPQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_Name_Changed_Effect_Frm = model.NIC_Name_Changed_Effect_Frm;
                    db.TempPQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_Nationality = model.NIC_Nationality;
                    //db.TempPQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_AadhaarCard = model.NIC_AadhaarCard;
                    //db.TempPQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_DrivingLicense = model.NIC_DrivingLicense;
                    //db.TempPQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_PANCard = model.NIC_PANCard;
                    //db.TempPQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_Passport = model.NIC_Passport;
                    //db.TempPQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_VoterID = model.NIC_VoterID;
                    //db.TempPQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_OtherProof = model.NIC_OtherProof;
                    db.TempPQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_OtherDetails = model.NIC_OtherDetails;
                    db.TempPQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_OtherDetails2 = model.NIC_OtherDetails2;
                    db.TempPQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_OtherDetails3 = model.NIC_OtherDetails3;
                    db.TempPQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_OtherDetails4 = model.NIC_OtherDetails4;
                    db.TempPQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_OtherDetails5 = model.NIC_OtherDetails5;
                    db.TempPQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_OtherDetails6 = model.NIC_OtherDetails6;
                    db.TempPQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_OtherDetails7 = model.NIC_OtherDetails7;
                    db.TempPQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_OtherDetails8 = model.NIC_OtherDetails8;
                    db.TempPQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_OtherDetails9 = model.NIC_OtherDetails9;
                    db.TempPQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_OtherDetails10 = model.NIC_OtherDetails10;
                    db.TempPQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).ATA_CID_No = model.ATA_CID_No;
                    db.TempPQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).ATA_Cmpny_Addr = model.ATA_Cmpny_Addr;
                    //db.TempPQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).CheckStatus = model.CheckStatus;
                    db.TempPQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).ModifiedBy = model.ModifiedBy;
                    db.TempPQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).ModifiedDate = model.ModifiedDate;
                    db.TempPQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).NIC_Cand_Name = model.NIC_Cand_Name;

                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        public PQNationalIdentityInsuffListPagedModel GetAllNationalIdentityInsuffListForDETL(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", int PersonalId = 0, string spocEmail = "", short TeamMemberRowid = 0, int ClientUserRowID = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                IQueryable<PQNationalIdentityStatus> data = db.PQNationalIdentityStatus.Include("PQNationalIdentity").Where(p => (p.DETMQCCheckStatus == "Insufficent-QC" || p.TMQCCheckStatus == "Insufficent-QC") && p.InfSuffClearBy == 0);

                if (TeamMemberRowid > 0)
                {
                    data = data.Where(b => b.DETLAllocatedTo == TeamMemberRowid && !string.IsNullOrEmpty(b.InfSuffClearComments));
                }
                else
                {
                    data = data.Where(b => string.IsNullOrEmpty(b.InfSuffClearComments));
                }

                if (ClientUserRowID > 0)
                {
                    data = data.Where(b => b.PQNationalIdentity.PQPersonal.ClientUserRowID == ClientUserRowID);
                }

                if (PersonalId == 0)
                {
                    if (ClientRowID > 0)
                    {
                        data = data.Where(b => b.PQNationalIdentity.ClientRowID == ClientRowID);
                    }

                    if (!string.IsNullOrEmpty(spocEmail))
                    {
                        var Clients = db.PQClientMasters.Where(s => s.SpocEmailID.Contains(spocEmail)).Select(s => s.ClientRowID).ToList();
                        data = data.Where(a => Clients.Contains(a.PQNationalIdentity.ClientRowID));
                    }

                    if (!string.IsNullOrEmpty(RecievingToDate) && !string.IsNullOrEmpty(RecievingFromDate))
                    {
                        DateTime startDate = Convert.ToDateTime(RecievingFromDate);
                        DateTime EndDate = Convert.ToDateTime(RecievingToDate);

                        data = data.Where(b => DbFunctions.TruncateTime(b.PQNationalIdentity.PQPersonal.OrderDate) >= startDate.Date && DbFunctions.TruncateTime(b.PQNationalIdentity.PQPersonal.OrderDate) <= EndDate.Date);
                    }

                    if (!string.IsNullOrEmpty(CompletedToDate) && !string.IsNullOrEmpty(CompletedFromDate))
                    {
                        DateTime startDate = Convert.ToDateTime(CompletedFromDate);
                        DateTime EndDate = Convert.ToDateTime(CompletedToDate);

                        data = data.Where(b => DbFunctions.TruncateTime(b.PQNationalIdentity.PQPersonal.DeliveryDate) >= startDate.Date && DbFunctions.TruncateTime(b.PQNationalIdentity.PQPersonal.DeliveryDate) <= EndDate.Date);
                    }
                    if (!string.IsNullOrEmpty(Search))
                    {
                        data = data.Where(b => (b.PQNationalIdentity.PQPersonal.Name + " " + b.PQNationalIdentity.PQPersonal.MiddleName + " " + b.PQNationalIdentity.PQPersonal.LastName).ToString().Replace(" ", "").Contains(Search.Replace(" ", "")) || b.PQNationalIdentity.PQPersonal.ClientRefID == Search);
                    }
                }
                else
                {
                    data = data.Where(b => b.PQNationalIdentity.PersonalRowID == PersonalId);
                }

                switch (sort)
                {
                    default:
                        data = sortDir == "asc" ? data.OrderByDescending(d => d.NationalIdentityStatusRowID) : data.OrderBy(d => d.NationalIdentityStatusRowID);
                        break;
                }

                PQNationalIdentityInsuffListPagedModel model = new PQNationalIdentityInsuffListPagedModel();
                model.PageSize = pageSize;
                model.TotalRecords = data.Count();
                model.PQAllInsuffNationalIdentities = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new PQNationalIdentityInsuffViewModel
                {
                    NationalIdentityRowID = item.NationalIdentityRowID,
                    ClientRowID = item.PQNationalIdentity.ClientRowID,
                    ClientName = item.PQNationalIdentity.PQClientMaster.MasterAbbreviation.ClientName + " " + item.PQNationalIdentity.PQClientMaster.MasterClientSubGroup.ClientSubGroupName,
                    PersonalRowID = item.PQNationalIdentity.PersonalRowID,
                    CandidateName = item.PQNationalIdentity.PQPersonal.CandTitle + " " + item.PQNationalIdentity.PQPersonal.Name + " " + item.PQNationalIdentity.PQPersonal.MiddleName + " " + item.PQNationalIdentity.PQPersonal.LastName,
                    SubCheckRowID = item.PQNationalIdentity.SubCheckRowID,
                    CheckFamilyRowID = item.PQNationalIdentity.CheckFamilyRowID,
                    SubCheckName = item.PQNationalIdentity.MasterSubCheckFamily.SubCheckName,
                    CompanyRefNo = item.PQNationalIdentity.NIC_Sec_Ref_Id,
                    InfSuffRaiseBy = db.TeamMembers.Where(t => t.TeamMemberRowID == item.InfSuffRaiseBy).Select(t => t.TMFirstName + " " + t.TMLastName).FirstOrDefault(),
                    InfSuffRaisedDate = item.InfSuffRaisedDate,
                    INFRemarks = item.InfSuffRaiseRemarks,
                    CreatedDate = item.PQNationalIdentity.CreatedDate,
                    CheckStatus = item.PQNationalIdentity.CheckStatus,
                    ReWorkCheckStatus = item.PQNationalIdentity.ReWorkCheckStatus
                }).ToList();
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public PQNationalIdentityInsuffListPagedModel GetAllNationalIdentityInsuffListForDETL1(int pageNo, int pageSize, string sort, string sortDir, string subcheckType, string Search = "", short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", int PersonalId = 0, string spocEmail = "", short TeamMemberRowid = 0, int ClientUserRowID = 0,int allclints=0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataSet ds = new DataSet();
                PQNationalIdentityInsuffListPagedModel model = new PQNationalIdentityInsuffListPagedModel();
                ds = cc.getCheckwiseinsuffDetailSummary(ClientRowID, ClientUserRowID, subcheckType,Search,allclints);
                List<PQNationalIdentityInsuffViewModel> lmd = new List<PQNationalIdentityInsuffViewModel>();
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>
                {
                    lmd.Add(new PQNationalIdentityInsuffViewModel
                    {
                        PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]),
                        NationalIdentityRowID = Convert.ToInt32(dr["NationalIdentityRowID"]),
                        ClientRowID = Convert.ToInt16(dr["ClientRowID"]),
                        CandidateName = (dr["CandidateName"]).ToString(),
                        //ClientName = (dr["CandidateCode"]).ToString(),
                        CompanyRefNo = (dr["CompanyRefNo"]).ToString(),
                        CreatedDate = Convert.ToDateTime(dr["CreationDate"]),
                        CheckStatus = (dr["CheckStatus"]).ToString(),
                        SubCheckName = (dr["SubCheckName"]).ToString(),
                        CheckFamilyRowID = Convert.ToInt16(dr["CheckFamilyRowID"]),
                        SubCheckRowID = Convert.ToInt16(dr["SubCheckRowID"]),
                        Remarks = (dr["Remarks"]).ToString(),
                        EntityName = (dr["EntityName"]).ToString(),
                        ClientPackageName = (dr["CandidateType"]).ToString(),
                    });

                }
                
                model.PageSize = pageSize;
                model.TotalRecords = ds.Tables[0].Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PQAllInsuffNationalIdentities = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);

                //IQueryable<PQNationalIdentityStatus> data = db.PQNationalIdentityStatus.Include("PQNationalIdentity").Where(p => (p.DETMQCCheckStatus == "Insufficent-QC" || p.TMQCCheckStatus == "Insufficent-QC") && p.InfSuffClearBy == 0);

                //if (TeamMemberRowid > 0)
                //{
                //    data = data.Where(b => b.DETLAllocatedTo == TeamMemberRowid && !string.IsNullOrEmpty(b.InfSuffClearComments));
                //}
                //else
                //{
                //    data = data.Where(b => string.IsNullOrEmpty(b.InfSuffClearComments));
                //}

                //if (ClientUserRowID > 0)
                //{
                //    data = data.Where(b => b.PQNationalIdentity.PQPersonal.ClientUserRowID == ClientUserRowID);
                //}

                //if (PersonalId == 0)
                //{
                //    if (ClientRowID > 0)
                //    {
                //        data = data.Where(b => b.PQNationalIdentity.ClientRowID == ClientRowID);
                //    }

                //    if (!string.IsNullOrEmpty(spocEmail))
                //    {
                //        var Clients = db.PQClientMasters.Where(s => s.SpocEmailID.Contains(spocEmail)).Select(s => s.ClientRowID).ToList();
                //        data = data.Where(a => Clients.Contains(a.PQNationalIdentity.ClientRowID));
                //    }

                //    if (!string.IsNullOrEmpty(RecievingToDate) && !string.IsNullOrEmpty(RecievingFromDate))
                //    {
                //        DateTime startDate = Convert.ToDateTime(RecievingFromDate);
                //        DateTime EndDate = Convert.ToDateTime(RecievingToDate);

                //        data = data.Where(b => DbFunctions.TruncateTime(b.PQNationalIdentity.PQPersonal.OrderDate) >= startDate.Date && DbFunctions.TruncateTime(b.PQNationalIdentity.PQPersonal.OrderDate) <= EndDate.Date);
                //    }

                //    if (!string.IsNullOrEmpty(CompletedToDate) && !string.IsNullOrEmpty(CompletedFromDate))
                //    {
                //        DateTime startDate = Convert.ToDateTime(CompletedFromDate);
                //        DateTime EndDate = Convert.ToDateTime(CompletedToDate);

                //        data = data.Where(b => DbFunctions.TruncateTime(b.PQNationalIdentity.PQPersonal.DeliveryDate) >= startDate.Date && DbFunctions.TruncateTime(b.PQNationalIdentity.PQPersonal.DeliveryDate) <= EndDate.Date);
                //    }
                //    if (!string.IsNullOrEmpty(Search))
                //    {
                //        data = data.Where(b => (b.PQNationalIdentity.PQPersonal.Name + " " + b.PQNationalIdentity.PQPersonal.MiddleName + " " + b.PQNationalIdentity.PQPersonal.LastName).ToString().Replace(" ", "").Contains(Search.Replace(" ", "")) || b.PQNationalIdentity.PQPersonal.ClientRefID == Search);
                //    }
                //}
                //else
                //{
                //    data = data.Where(b => b.PQNationalIdentity.PersonalRowID == PersonalId);
                //}

                //switch (sort)
                //{
                //    default:
                //        data = sortDir == "asc" ? data.OrderByDescending(d => d.NationalIdentityStatusRowID) : data.OrderBy(d => d.NationalIdentityStatusRowID);
                //        break;
                //}

                //PQNationalIdentityInsuffListPagedModel model = new PQNationalIdentityInsuffListPagedModel();
                //model.PageSize = pageSize;
                //model.TotalRecords = data.Count();
                //model.PQAllInsuffNationalIdentities = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new PQNationalIdentityInsuffViewModel
                //{
                //    NationalIdentityRowID = item.NationalIdentityRowID,
                //    ClientRowID = item.PQNationalIdentity.ClientRowID,
                //    ClientName = item.PQNationalIdentity.PQClientMaster.MasterAbbreviation.ClientName + " " + item.PQNationalIdentity.PQClientMaster.MasterClientSubGroup.ClientSubGroupName,
                //    PersonalRowID = item.PQNationalIdentity.PersonalRowID,
                //    CandidateName = item.PQNationalIdentity.PQPersonal.CandTitle + " " + item.PQNationalIdentity.PQPersonal.Name + " " + item.PQNationalIdentity.PQPersonal.MiddleName + " " + item.PQNationalIdentity.PQPersonal.LastName,
                //    SubCheckRowID = item.PQNationalIdentity.SubCheckRowID,
                //    CheckFamilyRowID = item.PQNationalIdentity.CheckFamilyRowID,
                //    SubCheckName = item.PQNationalIdentity.MasterSubCheckFamily.SubCheckName,
                //    CompanyRefNo = item.PQNationalIdentity.NIC_Sec_Ref_Id,
                //    InfSuffRaiseBy = db.TeamMembers.Where(t => t.TeamMemberRowID == item.InfSuffRaiseBy).Select(t => t.TMFirstName + " " + t.TMLastName).FirstOrDefault(),
                //    InfSuffRaisedDate = item.InfSuffRaisedDate,
                //    INFRemarks = item.InfSuffRaiseRemarks,
                //    CreatedDate = item.PQNationalIdentity.CreatedDate,
                //    CheckStatus = item.PQNationalIdentity.CheckStatus,
                //    ReWorkCheckStatus = item.PQNationalIdentity.ReWorkCheckStatus
                //}).ToList();
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PQNationalIdentityInsuffListPagedModel GetAllNationalIdentityInsuffListForCSSpoc(int pageNo, int pageSize, string sort, string sortDir, string subcheckType, string Search = "", short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", int PersonalId = 0, string spocEmail = "", short TeamMemberRowid = 0, int ClientUserRowID = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataSet ds = new DataSet();
                PQNationalIdentityInsuffListPagedModel model = new PQNationalIdentityInsuffListPagedModel();
                ds = cc.getinsuffDetails_CSSpoc(subcheckType, spocEmail, ClientRowID,Search);
                List<PQNationalIdentityInsuffViewModel> lmd = new List<PQNationalIdentityInsuffViewModel>();
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        PQNationalIdentityInsuffViewModel nn = new PQNationalIdentityInsuffViewModel();
                        //lmd.Add(new PQEducationInsuffViewModel
                        //{
                        nn.PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]);
                        nn.NationalIdentityRowID = Convert.ToInt32(dr["VerificationRowId"]);
                        nn.ClientRowID = Convert.ToInt16(dr["ClientRowID"]);
                        nn.CandidateName = (dr["CandidateName"]).ToString();
                        //ClientName = (dr["CandidateCode"]).ToString(),
                        nn.CompanyRefNo = (dr["CompanyRefNo"]).ToString();

                        if (dr["CreationDate"] == DBNull.Value)
                            nn.CreatedDate = null;
                        else
                            nn.CreatedDate = Convert.ToDateTime(dr["CreationDate"].ToString());
                        //nn.CreatedDate = Convert.ToDateTime(dr["CreationDate"]);
                        nn.CheckStatus = (dr["CheckStatus"]).ToString();
                        nn.SubCheckName = (dr["SubCheckName"]).ToString();
                        nn.CheckFamilyRowID = Convert.ToInt16(dr["CheckFamilyRowID"]);
                        nn.SubCheckRowID = Convert.ToInt16(dr["SubCheckRowID"]);
                        nn.ClientName = (dr["ClientName"]).ToString();
                        nn.Remarks = (dr["Remarks"]).ToString();
                        lmd.Add(nn);
                    }

                    model.PageSize = pageSize;
                    model.TotalRecords = ds.Tables[0].Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PQAllInsuffNationalIdentities = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);                
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public UpdatePQNationalIdentityInsuffClearViewModel GetPQNationalIdentityDetailsById(int NationalIdentityRowID = 0)
        {
            try
            {
                UpdatePQNationalIdentityInsuffClearViewModel model = new UpdatePQNationalIdentityInsuffClearViewModel();
                //var entity = db.PQNationalIdentities.Find(NationalIdentityRowID);

                var entity = db.PQNationalIdentityStatus.Where(ni => ni.NationalIdentityRowID == NationalIdentityRowID).FirstOrDefault();
                if (entity != null)
                {
                    model.NationalIdentityRowID = entity.NationalIdentityRowID;
                    model.ClientRowID = entity.PQNationalIdentity.ClientRowID;
                    model.PersonalRowID = entity.PQNationalIdentity.PersonalRowID;
                    model.SubCheckRowID = entity.PQNationalIdentity.SubCheckRowID;
                    model.UniqueComponentID = entity.PQNationalIdentity.UniqueComponentID;
                    model.INFRemarks = entity.InfSuffClearComments;
                    model.CheckStatus = entity.PQNationalIdentity.CheckStatus;
                    model.ReWorkCheckStatus = entity.PQNationalIdentity.ReWorkCheckStatus;
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

        public void UpdatePQNationalIdentityInsuffClear(UpdatePQNationalIdentityInsuffClearViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            string ScnCheckStatus = "WIP-Pending";

                            var NationalIdentityStatusId = context.PQNationalIdentityStatus.Where(c => c.NationalIdentityRowID == model.NationalIdentityRowID).FirstOrDefault().NationalIdentityStatusRowID;

                            context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == NationalIdentityStatusId).DETMCheckStatus = ScnCheckStatus;
                            context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == NationalIdentityStatusId).InfSuffClearBy = model.InfSuffClearBy;
                            context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == NationalIdentityStatusId).InfSuffClearDate = model.InfSuffClearDate;
                            context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == NationalIdentityStatusId).InfSuffClearComments = model.INFRemarks;
                            context.SaveChanges();

                            //Check Status Update
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).CheckStatus = ScnCheckStatus;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).ModifiedBy = model.ModifiedBy;
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).ModifiedDate = model.ModifiedDate;
                            context.SaveChanges();

                            context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).DECaseStatus = "WIP";
                            context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).AllocatedToDEDate = model.InfSuffClearDate;
                            context.SaveChanges();
                        }

                        transaction.Commit();

                        if (new AddressInfoRepository().IsCheckhasInfsuff(model.ClientRowID, model.PersonalRowID))
                        {
                            db.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).CaseStatus = "WIP";
                            SaveChanges();
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

        public void ViewNationalIdentityInfoDETL(ViewNationalIdentityViewModel model)
        {
            {
                using (DataContext context = new DataContext())
                {
                    using (DbContextTransaction transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            if (model != null)
                            {
                                #region Save Check Status

                                context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).CheckStatus = model.CheckStatus;
                                context.SaveChanges();

                                context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).DECaseStatus = "WIP";
                                context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).AllocatedToDEDate = DateTime.Now;
                                context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).DEQCCaseStatus = "WIP";
                                context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).AllocatedToDEQCDate = DateTime.Now;
                                context.SaveChanges();

                                int statusId = context.PQNationalIdentityStatus.Where(p => p.NationalIdentityRowID == model.NationalIdentityRowID).FirstOrDefault().NationalIdentityStatusRowID;

                                context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == statusId).DETMCheckStatus = model.CheckStatus;
                                context.SaveChanges();
                                #endregion

                                transaction.Commit();
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
        }

        public void UpdateNationalIdentityRejectionByDEQC(ViewNationalIdentityViewModel model, bool flag = false)
        {
            {
                using (DataContext context = new DataContext())
                {
                    using (DbContextTransaction transaction = context.Database.BeginTransaction())
                    {
                        try
                        {
                            if (model != null)
                            {
                                #region Save Check Status

                                context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).CheckStatus = model.CheckStatus;
                                context.SaveChanges();

                                int statusId = context.PQNationalIdentityStatus.Where(p => p.NationalIdentityRowID == model.NationalIdentityRowID).FirstOrDefault().NationalIdentityStatusRowID;
                                if (flag)
                                {
                                    context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).DECaseStatus = "WIP";
                                    context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).AllocatedToDEDate = DateTime.Now;
                                    context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).DEQCCaseStatus = "WIP";
                                    context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).AllocatedToDEQCDate = DateTime.Now;
                                    context.SaveChanges();

                                    context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == statusId).DETMQCCheckStatus = "";
                                    context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == statusId).DETMCheckStatus = model.CheckStatus;
                                }
                                else
                                {
                                    context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == statusId).DETMQCCheckStatus = "Completed-QC";
                                    context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == statusId).TMCheckStatus = model.CheckStatus;
                                }                              
                                context.SaveChanges();
                                #endregion

                                transaction.Commit();
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
        }

        public void ViewNationalIdentityInfoByVrifierTL(ViewNationalIdentityViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            context.PQNationalIdentities.Single(c => c.NationalIdentityRowID == model.NationalIdentityRowID).CheckStatus = model.CheckStatus;
                            context.SaveChanges();

                            int statusId = context.PQNationalIdentityStatus.Where(p => p.NationalIdentityRowID == model.NationalIdentityRowID).FirstOrDefault().NationalIdentityStatusRowID;

                            context.PQNationalIdentityStatus.Single(c => c.NationalIdentityStatusRowID == statusId).TMCheckStatus = model.CheckStatus;
                            context.SaveChanges();

                            transaction.Commit();
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
    }
}
