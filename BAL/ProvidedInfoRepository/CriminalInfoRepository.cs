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
using System.Data;

namespace BAL.ProvidedInfoRepository
{
    public class CriminalInfoRepository : ICriminalInfoRepository
    {
        DataContext db;
        ClientCustomize cc;
        public CriminalInfoRepository()
        {
            db = new DataContext();
            cc = new ClientCustomize();
        }

        public void AddCriminalInformation(AddPQCriminalViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {

                    try
                    {
                        if (model != null)
                        {
                            PQCriminal entity = new PQCriminal();
                            entity.ClientRowID = model.ClientRowID;
                            entity.PersonalRowID = model.PersonalRowID;
                            entity.ClientPackageRowID = model.ClientPackageRowID;
                            entity.CheckFamilyRowID = model.CheckFamilyRowID;
                            entity.SubCheckRowID = model.SubCheckRowID;
                            entity.UniqueComponentID = model.UniqueComponentID;
                            entity.CRV_Cand_Name = model.CRV_Cand_Name;
                            entity.CRV_Sec_Ref_No = model.CRV_Sec_Ref_No;
                            entity.CRV_Father_Name = model.CRV_Father_Name;
                            entity.CRV_DOB = model.CRV_DOB;
                            entity.CRV_Name_Chngd_Effect_Frm = model.CRV_Name_Chngd_Effect_Frm;
                            entity.CRV_First_Name = model.CRV_First_Name;
                            entity.CRV_Last_Name = model.CRV_Last_Name;
                            entity.CRV_Nationality = model.CRV_Nationality;
                            entity.CRV_Nationality_On_Passport = model.CRV_Nationality_On_Passport;
                            entity.CRV_Country_of_Issue = model.CRV_Country_of_Issue;
                            entity.CRV_Addr_Type = model.CRV_Addr_Type;
                            entity.CRV_Current_Addr = model.CRV_Current_Addr;
                            entity.CRV_Prev_Addr1 = model.CRV_Prev_Addr1;
                            entity.CRV_Prev_Addr2 = model.CRV_Prev_Addr2;
                            entity.CRV_Prev_Addr3 = model.CRV_Prev_Addr3;
                            entity.CRV_Prev_Addr4 = model.CRV_Prev_Addr4;
                            entity.CRV_Permanent_Addr = model.CRV_Permanent_Addr;
                            entity.CRV_Period_Of_Stay = model.CRV_Period_Of_Stay;
                            entity.CRV_Gender = model.CRV_Gender;
                            entity.CRV_Identity_No = model.CRV_Identity_No;
                            entity.CRV_Identity_Prov = model.CRV_Identity_Prov;
                            entity.CRV_Doc_Received = model.CRV_Doc_Received;
                            entity.CRV_Others = model.CRV_Others;
                            entity.CRV_Others2 = model.CRV_Others2;
                            entity.CRV_Others3 = model.CRV_Others3;
                            entity.CRV_Others4 = model.CRV_Others4;
                            entity.CRV_Others5 = model.CRV_Others5;
                            entity.CRV_Others6 = model.CRV_Others6;
                            entity.CRV_Others7 = model.CRV_Others7;
                            entity.CRV_Others8 = model.CRV_Others8;
                            entity.CRV_Others9 = model.CRV_Others9;
                            entity.CRV_Others10 = model.CRV_Others10;
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
                            context.PQCriminals.Add(entity);
                            context.SaveChanges();

                            #region Save Check Status

                            PQCriminalStatus chkEntity = new PQCriminalStatus();
                            var criModel = context.PQCriminals.Include("PQPersonal").Where(p => p.CriminalRowId == entity.CriminalRowId).FirstOrDefault();
                            chkEntity.CriminalRowId = criModel.CriminalRowId;
                            chkEntity.DETLAllocatedTo = criModel.PQPersonal.AllocatedBy;
                            chkEntity.DETMCheckStatus = model.CheckStatus;
                            chkEntity.DETMAllocatedTo = criModel.PQPersonal.AllocatedToDE;
                            chkEntity.DETMAllocatedDate = criModel.PQPersonal.AllocatedToDEDate;
                            chkEntity.DETMQCAllocatedTo = criModel.PQPersonal.AllocatedToDEQC;
                            chkEntity.DETMQCAllocatedDate = criModel.PQPersonal.AllocatedToDEQCDate;

                            if (model.CheckStatus == "Completed")
                            {
                                chkEntity.DETMQCCheckStatus = model.CheckStatus;
                            }
                            if (model.CheckStatus == "Insufficent")
                            {
                                chkEntity.DETMQCCheckStatus = model.CheckStatus;
                                chkEntity.InfSuffRaiseBy = criModel.PQPersonal.AllocatedToDE;
                                chkEntity.InfSuffRaisedDate = criModel.CreatedDate;
                                chkEntity.InfSuffRaiseRemarks = model.INFRemarks;
                            }
                            if (model.CheckStatus == "Rejected" || model.CheckStatus == "Review Requested")
                            {
                                chkEntity.Remarks = model.INFRemarks;
                            }
                            context.PQCriminalStatus.Add(chkEntity);
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

        public int GetAllCriminalByClientId(short ClientRowID = 0)
        {
            try
            {
                var data = db.PQCriminals.Where(p => p.ClientRowID == ClientRowID).ToList();
                return data.Count();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public AddPQCriminalViewModel GetCriminalInfoForDataEntry(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0)
        {
            try
            {
                return db.PQCriminals.Where(a => a.ClientRowID == ClientRowId && a.PersonalRowID == PersonalRowID
                && a.CheckFamilyRowID == ChFamilRowId && a.SubCheckRowID == subCheckId).Select(a => new AddPQCriminalViewModel
                {
                    CriminalRowID = a.CriminalRowId,
                    ClientRowID = a.ClientRowID,
                    PersonalRowID = a.PersonalRowID,
                    ClientPackageRowID = a.ClientPackageRowID,
                    //CRV_Cand_Name = a.CRV_Cand_Name,
                    CRV_Cand_Name = db.PQPersonals.Where(T => T.PersonalRowID == a.PersonalRowID).Select(x => x.Name + " " + x.MiddleName + x.LastName).FirstOrDefault().ToString(),
                    CheckFamilyRowID = a.CheckFamilyRowID,
                    SubCheckRowID = a.SubCheckRowID,
                    UniqueComponentID = a.UniqueComponentID,
                    CRV_Father_Name = a.CRV_Father_Name,
                    CRV_First_Name = a.CRV_First_Name,
                    CRV_Last_Name = a.CRV_Last_Name,
                    CRV_Nationality = a.CRV_Nationality,
                    CRV_Nationality_On_Passport = a.CRV_Nationality_On_Passport,
                    CRV_DOB = a.CRV_DOB,
                    CRV_Name_Chngd_Effect_Frm = a.CRV_Name_Chngd_Effect_Frm,
                    CRV_Country_of_Issue = a.CRV_Country_of_Issue,
                    CRV_Addr_Type = a.CRV_Addr_Type,
                    CRV_Current_Addr = a.CRV_Current_Addr,
                    CRV_Prev_Addr1 = a.CRV_Prev_Addr1,
                    CRV_Prev_Addr2 = a.CRV_Prev_Addr2,
                    CRV_Prev_Addr3 = a.CRV_Prev_Addr3,
                    CRV_Prev_Addr4 = a.CRV_Prev_Addr4,
                    CRV_Permanent_Addr = a.CRV_Permanent_Addr,
                    CRV_Period_Of_Stay = a.CRV_Period_Of_Stay,
                    CRV_Gender = a.CRV_Gender,
                    CRV_Identity_No = a.CRV_Identity_No,
                    CRV_Identity_Prov = a.CRV_Identity_Prov,
                    CRV_Doc_Received = a.CRV_Doc_Received,
                    CRV_Others = a.CRV_Others,
                    CRV_Others2 = a.CRV_Others2,
                    CRV_Others3 = a.CRV_Others3,
                    CRV_Others4 = a.CRV_Others4,
                    CRV_Others5 = a.CRV_Others5,
                    CRV_Others6 = a.CRV_Others6,
                    CRV_Others7 = a.CRV_Others7,
                    CRV_Others8 = a.CRV_Others8,
                    CRV_Others9 = a.CRV_Others9,
                    CRV_Others10 = a.CRV_Others10,
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

        public void UpdateCriminalInforByDataEntry(AddPQCriminalViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {

                    try
                    {
                        if (model != null)
                        {
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Father_Name = model.CRV_Father_Name;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_First_Name = model.CRV_First_Name;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Last_Name = model.CRV_Last_Name;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Nationality = model.CRV_Nationality;
                            //context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Cand_Name = model.CRV_Cand_Name;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_DOB = model.CRV_DOB;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Name_Chngd_Effect_Frm = model.CRV_Name_Chngd_Effect_Frm;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Nationality_On_Passport = model.CRV_Nationality_On_Passport;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Country_of_Issue = model.CRV_Country_of_Issue;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Addr_Type = model.CRV_Addr_Type;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Current_Addr = model.CRV_Current_Addr;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Prev_Addr1 = model.CRV_Prev_Addr1;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Prev_Addr2 = model.CRV_Prev_Addr2;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Prev_Addr3 = model.CRV_Prev_Addr3;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Prev_Addr4 = model.CRV_Prev_Addr4;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Permanent_Addr = model.CRV_Permanent_Addr;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Period_Of_Stay = model.CRV_Period_Of_Stay;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Gender = model.CRV_Gender;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Identity_No = model.CRV_Identity_No;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Identity_Prov = model.CRV_Identity_Prov;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Doc_Received = model.CRV_Doc_Received;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Others = model.CRV_Others;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Others2 = model.CRV_Others2;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Others3 = model.CRV_Others3;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Others4 = model.CRV_Others4;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Others5 = model.CRV_Others5;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Others6 = model.CRV_Others6;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Others7 = model.CRV_Others7;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Others8 = model.CRV_Others8;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Others9 = model.CRV_Others9;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Others10 = model.CRV_Others10;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).ATA_CID_No = model.ATA_CID_No;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).ATA_Cmpny_Addr = model.ATA_Cmpny_Addr;

                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CheckStatus = model.CheckStatus;
                            if (model.CheckStatus == "Completed")
                            {
                                context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CheckStatus = "Completed-DE";
                            }
                            else if(model.CheckStatus == "Not Applicable")
                            {
                                context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CheckStatus = "Not Applicable";
                            }

                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).ModifiedBy = model.CreatedBy;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).ModifiedDate = model.CreatedDate;
                            context.SaveChanges();

                            #region Save Check Status

                            var status = context.PQCriminalStatus.Where(p => p.CriminalRowId == model.CriminalRowID).FirstOrDefault();

                            //if (status.TMCheckStatus == "Insufficent" && status.TMQCCheckStatus == "Insufficent-QC" && model.CheckStatus == "Completed")
                            //{
                            //    context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CheckStatus = "WIP-Pending";
                            //    context.SaveChanges();

                            //    context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == status.CriminalStatusRowID).DETMCheckStatus = model.CheckStatus;
                            //    context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == status.CriminalStatusRowID).DETMAllocatedDate = model.CreatedDate;
                            //    context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == status.CriminalStatusRowID).TMCheckStatus = "WIP-Pending";
                            //    context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == status.CriminalStatusRowID).TMAllocatedDate = model.CreatedDate;
                            //    context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == status.CriminalStatusRowID).TMQCCheckStatus = "";
                            //    context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == status.CriminalStatusRowID).TMQCAllocatedDate = model.CreatedDate;
                            //}
                            if ( model.CheckStatus == "Not Applicable")
                            {
                                context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CheckStatus = "Not Applicable";
                                context.SaveChanges();
                                
                                
                            }
                            else
                            {
                                //context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == status.CriminalStatusRowID).DETMCheckStatus = model.CheckStatus;
                                context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == status.CriminalStatusRowID).DETMAllocatedDate = model.CreatedDate;

                                if (model.CheckStatus == "Completed")
                                {
                                    //context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == status.CriminalStatusRowID).DETMQCCheckStatus = model.CheckStatus;
                                    context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == status.CriminalStatusRowID).DETMQCAllocatedDate = model.CreatedDate;
                                }
                                if (model.CheckStatus == "Insufficent")
                                {
                                    //context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == status.CriminalStatusRowID).DETMQCCheckStatus = model.CheckStatus;
                                    context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == status.CriminalStatusRowID).DETMQCAllocatedDate = model.CreatedDate;
                                    context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == status.CriminalStatusRowID).InfSuffRaiseBy = model.CreatedBy;
                                    context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == status.CriminalStatusRowID).InfSuffRaisedDate = model.CreatedDate;
                                    context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == status.CriminalStatusRowID).InfSuffRaiseRemarks = model.INFRemarks;
                                }
                                if (model.CheckStatus == "Rejected" || model.CheckStatus == "Review Requested")
                                {
                                    context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == status.CriminalStatusRowID).Remarks = model.INFRemarks;
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

        public PQUpdateCriminalViewModel GetCriminalInfoForQualityCheck(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0)
        {
            try
            {
                return db.PQCriminals.Where(a => a.ClientRowID == ClientRowId && a.PersonalRowID == PersonalRowID
                && a.CheckFamilyRowID == ChFamilRowId && a.SubCheckRowID == subCheckId).Select(a => new PQUpdateCriminalViewModel
                {
                    CriminalRowID = a.CriminalRowId,
                    ClientRowID = a.ClientRowID,
                    PersonalRowID = a.PersonalRowID,
                    ClientPackageRowID = a.ClientPackageRowID,
                    CheckFamilyRowID = a.CheckFamilyRowID,
                    SubCheckRowID = a.SubCheckRowID,
                    UniqueComponentID = a.UniqueComponentID,
                    CRV_DOB = a.CRV_DOB,
                    CRV_Name_Chngd_Effect_Frm = a.CRV_Name_Chngd_Effect_Frm,
                    CRV_Sec_Ref_No = a.CRV_Sec_Ref_No,
                    CRV_Cand_Name = a.CRV_Cand_Name,
                    CRV_Father_Name = a.CRV_Father_Name,
                    CRV_First_Name = a.CRV_First_Name,
                    CRV_Last_Name = a.CRV_Last_Name,
                    CRV_Nationality = a.CRV_Nationality,
                    CRV_Nationality_On_Passport = a.CRV_Nationality_On_Passport,
                    CRV_Country_of_Issue = a.CRV_Country_of_Issue,
                    CRV_Addr_Type = a.CRV_Addr_Type,
                    CRV_Current_Addr = a.CRV_Current_Addr,
                    CRV_Prev_Addr1 = a.CRV_Prev_Addr1,
                    CRV_Prev_Addr2 = a.CRV_Prev_Addr2,
                    CRV_Prev_Addr3 = a.CRV_Prev_Addr3,
                    CRV_Prev_Addr4 = a.CRV_Prev_Addr4,
                    CRV_Permanent_Addr = a.CRV_Permanent_Addr,
                    CRV_Period_Of_Stay = a.CRV_Period_Of_Stay,
                    CRV_Gender = a.CRV_Gender,
                    CRV_Identity_No = a.CRV_Identity_No,
                    CRV_Identity_Prov = a.CRV_Identity_Prov,
                    CRV_Doc_Received = a.CRV_Doc_Received,
                    CRV_Others = a.CRV_Others,
                    CRV_Others2 = a.CRV_Others2,
                    CRV_Others3 = a.CRV_Others3,
                    CRV_Others4 = a.CRV_Others4,
                    CRV_Others5 = a.CRV_Others5,
                    CRV_Others6 = a.CRV_Others6,
                    CRV_Others7 = a.CRV_Others7,
                    CRV_Others8 = a.CRV_Others8,
                    CRV_Others9 = a.CRV_Others9,
                    CRV_Others10 = a.CRV_Others10,
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

        public PQUpdateCriminalViewModel GetCriminalInfoForQualityCheck(int CriminalRowID = 0)
        {
            try
            {
                return db.PQCriminals.Where(a => a.CriminalRowId == CriminalRowID).Select(a => new PQUpdateCriminalViewModel
                {
                    CriminalRowID = a.CriminalRowId,
                    ClientRowID = a.ClientRowID,
                    PersonalRowID = a.PersonalRowID,
                    ClientPackageRowID = a.ClientPackageRowID,
                    CheckFamilyRowID = a.CheckFamilyRowID,
                    SubCheckRowID = a.SubCheckRowID,
                    UniqueComponentID = a.UniqueComponentID,
                    CRV_DOB = a.CRV_DOB,
                    CRV_Name_Chngd_Effect_Frm = a.CRV_Name_Chngd_Effect_Frm,
                    CRV_Sec_Ref_No = a.CRV_Sec_Ref_No,
                    CRV_Cand_Name = a.CRV_Cand_Name,
                    CRV_Father_Name = a.CRV_Father_Name,
                    CRV_First_Name = a.CRV_First_Name,
                    CRV_Last_Name = a.CRV_Last_Name,
                    CRV_Nationality = a.CRV_Nationality,
                    CRV_Nationality_On_Passport = a.CRV_Nationality_On_Passport,
                    CRV_Country_of_Issue = a.CRV_Country_of_Issue,
                    CRV_Addr_Type = a.CRV_Addr_Type,
                    CRV_Current_Addr = a.CRV_Current_Addr,
                    CRV_Prev_Addr1 = a.CRV_Prev_Addr1,
                    CRV_Prev_Addr2 = a.CRV_Prev_Addr2,
                    CRV_Prev_Addr3 = a.CRV_Prev_Addr3,
                    CRV_Prev_Addr4 = a.CRV_Prev_Addr4,
                    CRV_Permanent_Addr = a.CRV_Permanent_Addr,
                    CRV_Period_Of_Stay = a.CRV_Period_Of_Stay,
                    CRV_Gender = a.CRV_Gender,
                    CRV_Identity_No = a.CRV_Identity_No,
                    CRV_Identity_Prov = a.CRV_Identity_Prov,
                    CRV_Doc_Received = a.CRV_Doc_Received,
                    CRV_Others = a.CRV_Others,
                    CRV_Others2 = a.CRV_Others2,
                    CRV_Others3 = a.CRV_Others3,
                    CRV_Others4 = a.CRV_Others4,
                    CRV_Others5 = a.CRV_Others5,
                    CRV_Others6 = a.CRV_Others6,
                    CRV_Others7 = a.CRV_Others7,
                    CRV_Others8 = a.CRV_Others8,
                    CRV_Others9 = a.CRV_Others9,
                    CRV_Others10 = a.CRV_Others10,
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

        public void UpdateCriminalInfoByQC(PQUpdateCriminalViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Father_Name = model.CRV_Father_Name;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_First_Name = model.CRV_First_Name;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Last_Name = model.CRV_Last_Name;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Nationality = model.CRV_Nationality;
                            //context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Cand_Name = model.CRV_Cand_Name;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Nationality_On_Passport = model.CRV_Nationality_On_Passport;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Country_of_Issue = model.CRV_Country_of_Issue;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Addr_Type = model.CRV_Addr_Type;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Current_Addr = model.CRV_Current_Addr;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Prev_Addr1 = model.CRV_Prev_Addr1;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Prev_Addr2 = model.CRV_Prev_Addr2;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Prev_Addr3 = model.CRV_Prev_Addr3;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Prev_Addr4 = model.CRV_Prev_Addr4;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Permanent_Addr = model.CRV_Permanent_Addr;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Period_Of_Stay = model.CRV_Period_Of_Stay;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Gender = model.CRV_Gender;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Identity_No = model.CRV_Identity_No;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Identity_Prov = model.CRV_Identity_Prov;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Doc_Received = model.CRV_Doc_Received;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Others = model.CRV_Others;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Others2 = model.CRV_Others2;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Others3 = model.CRV_Others3;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Others4 = model.CRV_Others4;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Others5 = model.CRV_Others5;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Others6 = model.CRV_Others6;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Others7 = model.CRV_Others7;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Others8 = model.CRV_Others8;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Others9 = model.CRV_Others9;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Others10 = model.CRV_Others10;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).ATA_CID_No = model.ATA_CID_No;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).ATA_Cmpny_Addr = model.ATA_Cmpny_Addr;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).ModifiedBy = model.ModifiedBy;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).ModifiedDate = model.ModifiedDate;

                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CheckStatus = model.CheckStatus;
                            if (model.CheckStatus == "Completed-DE")
                            {
                                context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CheckStatus = "Allocated-Mgr";
                            }

                            context.SaveChanges();

                            #region Save Check Status

                            var CurrentStatus = context.PQCriminalStatus.Where(p => p.CriminalRowId == model.CriminalRowID).FirstOrDefault();

                            if (model.CheckStatus == "Completed-DE")
                            {
                                if (CurrentStatus.TMCheckStatus == "Rejected")
                                {
                                    context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CheckStatus = "WIP-Pending";
                                    context.SaveChanges();

                                    context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == CurrentStatus.CriminalStatusRowID).DETMQCCheckStatus = "Completed-QC";
                                    context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == CurrentStatus.CriminalStatusRowID).TMCheckStatus = "WIP-Pending";
                                    context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == CurrentStatus.CriminalStatusRowID).TMAllocatedDate = model.ModifiedDate;
                                }
                                else
                                {
                                    context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == CurrentStatus.CriminalStatusRowID).DETMQCCheckStatus = "Completed-QC";
                                    context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == CurrentStatus.CriminalStatusRowID).MgrCheckStatus = "Allocated-Mgr";
                                    context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == CurrentStatus.CriminalStatusRowID).MgrAllocatedTo = model.MgrAllocatedTo;
                                    context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == CurrentStatus.CriminalStatusRowID).MgrAllocatedDate = model.ModifiedDate;
                                }

                            }

                            if (model.CheckStatus == "Insufficent")
                            {
                                context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == CurrentStatus.CriminalStatusRowID).DETMQCCheckStatus = "Insufficent-QC";
                                context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == CurrentStatus.CriminalStatusRowID).InfSuffClearBy = 0;
                                context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == CurrentStatus.CriminalStatusRowID).InfSuffClearComments = "";
                                context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == CurrentStatus.CriminalStatusRowID).InfSuffClearDate = null;
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

        public void UpdateCriminalInfoByManager(PQUpdateCriminalManagerViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CheckStatus = model.CheckStatus;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).ModifiedBy = model.ModifiedBy;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).ModifiedDate = model.ModifiedDate;
                            context.SaveChanges();

                            #region Save Check Status

                            int statusId = context.PQCriminalStatus.Where(p => p.CriminalRowId == model.CriminalRowID).FirstOrDefault().CriminalStatusRowID;

                            context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == statusId).TLCheckStatus = model.CheckStatus;
                            context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == statusId).TLAllocatedTo = model.TLAllocatedTo;
                            context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == statusId).TLAllocatedDate = model.ModifiedDate;
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

        public void UpdateCriminalInfoByTeamLeader(PQUpdateCriminalTeamLeaderViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CheckStatus = model.CheckStatus;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).ModifiedBy = model.ModifiedBy;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).ModifiedDate = model.ModifiedDate;

                            context.SaveChanges();

                            #region Save Check Status

                            int statusId = context.PQCriminalStatus.Where(p => p.CriminalRowId == model.CriminalRowID).FirstOrDefault().CriminalStatusRowID;

                            context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == statusId).TMCheckStatus = model.CheckStatus;
                            context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == statusId).TMAllocatedTo = model.TMAllocatedTo;
                            context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == statusId).TMAllocatedDate = model.ModifiedDate;

                            context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == statusId).TMQCCheckStatus = model.CheckStatus;
                            context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == statusId).TMQCAllocatedTo = model.TMQCAllocatedTo;
                            context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == statusId).TMQCAllocatedDate = model.ModifiedDate;


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

        public void UpdateCriminalInfoByTeamLeaderReAllocated(PQUpdateCriminalTeamLeaderViewModel model)
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

                            int statusId = context.PQCriminalStatus.Where(p => p.CriminalRowId == model.CriminalRowID).FirstOrDefault().CriminalStatusRowID;
                            if (model.TMAllocatedTo > 0)
                            {
                                context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == statusId).TMAllocatedTo = model.TMAllocatedTo;
                                context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == statusId).TMAllocatedDate = model.ModifiedDate;
                            }
                            if (model.TMQCAllocatedTo > 0)
                            {
                                context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == statusId).TMQCAllocatedTo = model.TMQCAllocatedTo;
                                context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == statusId).TMQCAllocatedDate = model.ModifiedDate;
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

        public void UpdateCriminalInfoByTeamMember(PQUpdateCriminalTeamMemberViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CheckStatus = model.CheckStatus;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).ModifiedBy = model.ModifiedBy;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).ModifiedDate = model.ModifiedDate;
                            context.SaveChanges();

                            #region Save Check Status

                            var statusId = context.PQCriminalStatus.Where(p => p.CriminalRowId == model.CriminalRowID).FirstOrDefault().CriminalStatusRowID;
                            ClientCustomize cc = new ClientCustomize();
                            DataSet dd = new DataSet();
                            dd = cc.getInformationFromDepartmentRowID(model.CriminalRowID, "Criminal");
                            int personalRowid = 0;
                            int subCheckRowID = 0;
                            if (dd.Tables[0].Rows.Count > 0)
                            {
                                personalRowid = int.Parse((Convert.ToString(dd.Tables[0].Rows[0]["PersonalRowID"])));
                                subCheckRowID = int.Parse((Convert.ToString(dd.Tables[0].Rows[0]["SubCheckRowID"])));
                            }

                            context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == statusId).TMCheckStatus = model.CheckStatus;
                            context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == statusId).TMQCCheckStatus = "";
                            if (model.CheckStatus == "PV Raised")
                            {
                                context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == statusId).PVMgrCheckStatus = "Allocate-Mgr";
                                context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == statusId).PVMgrAllocatedTo = model.PVMgrAllocatedTo;
                                context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == statusId).PVMgrAllocatedDate = model.ModifiedDate;
                            }
                            if (model.CheckStatus == "Insufficent")
                            {
                                cc.UpdateCheckStatus(personalRowid, subCheckRowID, 1, 23, "Check", model.ModifiedBy);
                                context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == statusId).TMQCCheckStatus = model.CheckStatus;
                                context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == statusId).InfSuffRaiseRemarks = model.ActionRemark;
                                context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == statusId).InfSuffRaiseBy = model.ModifiedBy;
                                context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == statusId).InfSuffRaisedDate = model.ModifiedDate;
                            }
                            if (model.CheckStatus == "Rejected")
                            {
                                cc.UpdateCheckStatus(personalRowid, subCheckRowID, 1, 14, "Check", model.ModifiedBy);
                                context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == statusId).Remarks = model.ActionRemark;
                            }
                            if (model.CheckStatus == "Funds Pending")
                            {
                                cc.UpdateCheckStatus(personalRowid, subCheckRowID, 1, 26, "Check", model.ModifiedBy);
                                context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == statusId).Remarks = model.ActionRemark;
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

        public void RejectionUpdateCriminalInfoByQC(UpdateCriminalRejectionViewModel model)
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
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CheckStatus = ActualCheckStatus;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).ModifiedBy = model.ModifiedBy;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).ModifiedDate = model.ModifiedDate;
                            context.SaveChanges();

                            #region Update Check Status

                            int statusId = context.PQCriminalStatus.Where(p => p.CriminalRowId == model.CriminalRowID).FirstOrDefault().CriminalStatusRowID;
                            context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == statusId).DETMCheckStatus = ActualCheckStatus;
                            context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == statusId).DETMQCCheckStatus = "";
                            context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == statusId).Remarks = model.INFRemarks;
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


        public void ViewCriminalInfoByVrifierTL(ViewCriminalViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CheckStatus = model.CheckStatus;
                            context.SaveChanges();

                            int statusId = context.PQCriminalStatus.Where(p => p.CriminalRowId == model.CriminalRowID).FirstOrDefault().CriminalStatusRowID;

                            context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == statusId).TMCheckStatus = model.CheckStatus;
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

        public bool IfExistCriminal(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0, string CheckStatus = "")
        {
            try
            {
                var listChecks = db.PQCriminals.Where(a => a.ClientRowID == ClientRowId && a.PersonalRowID == PersonalRowID
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

        public bool IfExistCriminalForDELeftMenu(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0)
        {
            try
            {
                var listChecks = db.PQCriminals.Where(a => a.ClientRowID == ClientRowId && a.PersonalRowID == PersonalRowID
                     && a.CheckFamilyRowID == ChFamilRowId && a.SubCheckRowID == subCheckId).FirstOrDefault();
                if (listChecks != null)
                {
                    var ChkStatus = db.PQCriminalStatus.Where(s => s.CriminalRowId == listChecks.CriminalRowId).FirstOrDefault();

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

        public bool IfExistCriminalForQCLeftMenu(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0, int CheckStatusid = 0)
        {
            try
            {
                //var listChecks = db.PQCriminals.Where(a => a.ClientRowID == ClientRowId && a.PersonalRowID == PersonalRowID
                //     && a.CheckFamilyRowID == ChFamilRowId && a.SubCheckRowID == subCheckId).FirstOrDefault();
                //if (listChecks != null)
                //{
                //    var ChkStatus = db.PQCriminalStatus.Where(s => s.CriminalRowId == listChecks.CriminalRowId).FirstOrDefault();

                    if ( CheckStatusid == 68 || CheckStatusid == 14 || CheckStatusid == 15 || CheckStatusid==42)
                        return true;
                    else
                        return false;
                //}
                //else
                //{
                //    return false;
                //}
            }
            catch (Exception)
            {

                throw;
            }
        }

        #region *********Temp Criminal************

        public int GetAllTempCriminalByClientId(short ClientRowID = 0)
        {
            try
            {
                var data = db.TempPQCriminals.Where(p => p.ClientRowID == ClientRowID).ToList();
                return data.Count();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddTempCriminalInformation(AddTempPQCriminalViewModel model)
        {
            try
            {
                if (model != null)
                {
                    TempPQCriminal entity = new TempPQCriminal();
                    entity.ClientRowID = model.ClientRowID;
                    entity.PersonalRowID = model.PersonalRowID;
                    entity.ClientPackageRowID = model.ClientPackageRowID;
                    entity.CheckFamilyRowID = model.CheckFamilyRowID;
                    entity.SubCheckRowID = model.SubCheckRowID;
                    entity.UniqueComponentID = model.UniqueComponentID;
                    entity.CRV_Cand_Name = model.CRV_Cand_Name;
                    entity.CRV_Sec_Ref_No = model.CRV_Sec_Ref_No;
                    entity.CRV_Father_Name = model.CRV_Father_Name;
                    entity.CRV_DOB = model.CRV_DOB;
                    entity.CRV_Name_Chngd_Effect_Frm = model.CRV_Name_Chngd_Effect_Frm;
                    entity.CRV_First_Name = model.CRV_First_Name;
                    entity.CRV_Last_Name = model.CRV_Last_Name;
                    entity.CRV_Nationality = model.CRV_Nationality;
                    entity.CRV_Nationality_On_Passport = model.CRV_Nationality_On_Passport;
                    entity.CRV_Country_of_Issue = model.CRV_Country_of_Issue;
                    entity.CRV_Addr_Type = model.CRV_Addr_Type;
                    entity.CRV_Current_Addr = model.CRV_Current_Addr;
                    entity.CRV_Prev_Addr1 = model.CRV_Prev_Addr1;
                    entity.CRV_Prev_Addr2 = model.CRV_Prev_Addr2;
                    entity.CRV_Prev_Addr3 = model.CRV_Prev_Addr3;
                    entity.CRV_Prev_Addr4 = model.CRV_Prev_Addr4;
                    entity.CRV_Permanent_Addr = model.CRV_Permanent_Addr;
                    entity.CRV_Period_Of_Stay = model.CRV_Period_Of_Stay;
                    entity.CRV_Gender = model.CRV_Gender;
                    entity.CRV_Identity_No = model.CRV_Identity_No;
                    entity.CRV_Identity_Prov = model.CRV_Identity_Prov;
                    entity.CRV_Doc_Received = model.CRV_Doc_Received;
                    //entity.CRV_IdentityProof = model.CRV_IdentityProof;
                    //entity.CRV_AddressProof = model.CRV_AddressProof;
                    //entity.CRV_AuthrizedLetter = model.CRV_AuthrizedLetter;
                    //entity.CRV_OtherProof = model.CRV_OtherProof;
                    entity.CRV_Others = model.CRV_Others;
                    entity.CRV_Others2 = model.CRV_Others2;
                    entity.CRV_Others3 = model.CRV_Others3;
                    entity.CRV_Others4 = model.CRV_Others4;
                    entity.CRV_Others5 = model.CRV_Others5;
                    entity.CRV_Others6 = model.CRV_Others6;
                    entity.CRV_Others7 = model.CRV_Others7;
                    entity.CRV_Others8 = model.CRV_Others8;
                    entity.CRV_Others9 = model.CRV_Others9;
                    entity.CRV_Others10 = model.CRV_Others10;
                    entity.ATA_CID_No = model.ATA_CID_No;
                    entity.ATA_Cmpny_Addr = model.ATA_Cmpny_Addr;
                    entity.CreatedBy = model.CreatedBy;
                    entity.CreatedDate = model.CreatedDate;
                    entity.CheckStatus = model.CheckStatus;
                    entity.Remarks = model.Remarks;
                    entity.Status = model.Status;
                    db.TempPQCriminals.Add(entity);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void UpdateTempCriminalInforBySelf(AddTempPQCriminalViewModel model)
        {
            try
            {
                if (model != null)
                {
                    db.TempPQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Father_Name = model.CRV_Father_Name;
                    db.TempPQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_First_Name = model.CRV_First_Name;
                    db.TempPQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Last_Name = model.CRV_Last_Name;
                    db.TempPQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Nationality = model.CRV_Nationality;
                    db.TempPQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_DOB = model.CRV_DOB;
                    db.TempPQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Name_Chngd_Effect_Frm = model.CRV_Name_Chngd_Effect_Frm;
                    db.TempPQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Nationality_On_Passport = model.CRV_Nationality_On_Passport;
                    db.TempPQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Country_of_Issue = model.CRV_Country_of_Issue;
                    db.TempPQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Addr_Type = model.CRV_Addr_Type;
                    db.TempPQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Current_Addr = model.CRV_Current_Addr;
                    db.TempPQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Prev_Addr1 = model.CRV_Prev_Addr1;
                    db.TempPQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Prev_Addr2 = model.CRV_Prev_Addr2;
                    db.TempPQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Prev_Addr3 = model.CRV_Prev_Addr3;
                    db.TempPQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Prev_Addr4 = model.CRV_Prev_Addr4;
                    db.TempPQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Permanent_Addr = model.CRV_Permanent_Addr;
                    db.TempPQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Period_Of_Stay = model.CRV_Period_Of_Stay;
                    db.TempPQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Gender = model.CRV_Gender;
                    db.TempPQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Identity_No = model.CRV_Identity_No;
                    db.TempPQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Identity_Prov = model.CRV_Identity_Prov;
                    db.TempPQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Doc_Received = model.CRV_Doc_Received;
                    //db.TempPQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_IdentityProof = model.CRV_IdentityProof;
                    //db.TempPQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_AddressProof = model.CRV_AddressProof;
                    //db.TempPQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_AuthrizedLetter = model.CRV_AuthrizedLetter;
                    //db.TempPQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_OtherProof = model.CRV_OtherProof;
                    db.TempPQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Others = model.CRV_Others;
                    db.TempPQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Others2 = model.CRV_Others2;
                    db.TempPQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Others3 = model.CRV_Others3;
                    db.TempPQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Others4 = model.CRV_Others4;
                    db.TempPQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Others5 = model.CRV_Others5;
                    db.TempPQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Others6 = model.CRV_Others6;
                    db.TempPQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Others7 = model.CRV_Others7;
                    db.TempPQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Others8 = model.CRV_Others8;
                    db.TempPQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Others9 = model.CRV_Others9;
                    db.TempPQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Others10 = model.CRV_Others10;
                    db.TempPQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).ATA_CID_No = model.ATA_CID_No;
                    db.TempPQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).ATA_Cmpny_Addr = model.ATA_Cmpny_Addr;
                    //db.TempPQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CheckStatus = model.CheckStatus;
                    db.TempPQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).ModifiedBy = model.ModifiedBy;
                    db.TempPQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).ModifiedDate = model.ModifiedDate;
                    db.TempPQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Cand_Name = model.CRV_Cand_Name;
                    db.TempPQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CRV_Gender = model.CRV_Gender;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public AddTempPQCriminalViewModel GetCriminalInfoBySelf(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0)
        {
            try
            {
                return db.TempPQCriminals.Where(a => a.ClientRowID == ClientRowId && a.PersonalRowID == PersonalRowID
                && a.CheckFamilyRowID == ChFamilRowId && a.SubCheckRowID == subCheckId).Select(a => new AddTempPQCriminalViewModel
                {
                    CriminalRowID = a.CriminalRowId,
                    ClientRowID = a.ClientRowID,
                    PersonalRowID = a.PersonalRowID,
                    ClientPackageRowID = a.ClientPackageRowID,
                    CheckFamilyRowID = a.CheckFamilyRowID,
                    SubCheckRowID = a.SubCheckRowID,
                    UniqueComponentID = a.UniqueComponentID,
                    CRV_Father_Name = a.CRV_Father_Name,
                    CRV_First_Name = a.CRV_First_Name,
                    CRV_Last_Name = a.CRV_Last_Name,
                    CRV_Nationality = a.CRV_Nationality,
                    CRV_Nationality_On_Passport = a.CRV_Nationality_On_Passport,
                    CRV_DOB = a.CRV_DOB,
                    CRV_Name_Chngd_Effect_Frm = a.CRV_Name_Chngd_Effect_Frm,
                    CRV_Country_of_Issue = a.CRV_Country_of_Issue,
                    CRV_Addr_Type = a.CRV_Addr_Type,
                    CRV_Current_Addr = a.CRV_Current_Addr,
                    CRV_Prev_Addr1 = a.CRV_Prev_Addr1,
                    CRV_Prev_Addr2 = a.CRV_Prev_Addr2,
                    CRV_Prev_Addr3 = a.CRV_Prev_Addr3,
                    CRV_Prev_Addr4 = a.CRV_Prev_Addr4,
                    CRV_Permanent_Addr = a.CRV_Permanent_Addr,
                    CRV_Period_Of_Stay = a.CRV_Period_Of_Stay,
                    CRV_Gender = a.CRV_Gender,
                    CRV_Identity_No = a.CRV_Identity_No,
                    CRV_Identity_Prov = a.CRV_Identity_Prov,
                    CRV_Doc_Received = a.CRV_Doc_Received,
                    //CRV_IdentityProof = a.CRV_IdentityProof,
                    //CRV_AddressProof = a.CRV_AddressProof,
                    //CRV_AuthrizedLetter = a.CRV_AuthrizedLetter,
                    //CRV_OtherProof = a.CRV_OtherProof,
                    CRV_Others = a.CRV_Others,
                    CRV_Others2 = a.CRV_Others2,
                    CRV_Others3 = a.CRV_Others3,
                    CRV_Others4 = a.CRV_Others4,
                    CRV_Others5 = a.CRV_Others5,
                    CRV_Others6 = a.CRV_Others6,
                    CRV_Others7 = a.CRV_Others7,
                    CRV_Others8 = a.CRV_Others8,
                    CRV_Others9 = a.CRV_Others9,
                    CRV_Others10 = a.CRV_Others10,
                    ATA_CID_No = a.ATA_CID_No,
                    ATA_Cmpny_Addr = a.ATA_Cmpny_Addr,
                    CheckStatus = a.CheckStatus,
                    CRV_Cand_Name = a.CRV_Cand_Name,

                }).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        public PQCriminalInsuffListPagedModel GetAllCriminalInsuffListForDETL(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", int PersonalId = 0, string spocEmail = "", short TeamMemberRowid = 0, int ClientUserRowID = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                IQueryable<PQCriminalStatus> data = db.PQCriminalStatus.Include("PQCriminal").Where(p => (p.DETMQCCheckStatus == "Insufficent-QC" || p.TMQCCheckStatus == "Insufficent-QC") && p.InfSuffClearBy == 0);

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
                    data = data.Where(b => b.PQCriminal.PQPersonal.ClientUserRowID == ClientUserRowID);
                }

                if (PersonalId == 0)
                {
                    if (ClientRowID > 0)
                    {
                        data = data.Where(b => b.PQCriminal.ClientRowID == ClientRowID);
                    }

                    if (!string.IsNullOrEmpty(spocEmail))
                    {
                        var Clients = db.PQClientMasters.Where(s => s.SpocEmailID.Contains(spocEmail)).Select(s => s.ClientRowID).ToList();
                        data = data.Where(a => Clients.Contains(a.PQCriminal.ClientRowID));
                    }

                    if (!string.IsNullOrEmpty(RecievingToDate) && !string.IsNullOrEmpty(RecievingFromDate))
                    {
                        DateTime startDate = Convert.ToDateTime(RecievingFromDate);
                        DateTime EndDate = Convert.ToDateTime(RecievingToDate);

                        data = data.Where(b => DbFunctions.TruncateTime(b.PQCriminal.PQPersonal.OrderDate) >= startDate.Date && DbFunctions.TruncateTime(b.PQCriminal.PQPersonal.OrderDate) <= EndDate.Date);
                    }

                    if (!string.IsNullOrEmpty(CompletedToDate) && !string.IsNullOrEmpty(CompletedFromDate))
                    {
                        DateTime startDate = Convert.ToDateTime(CompletedFromDate);
                        DateTime EndDate = Convert.ToDateTime(CompletedToDate);

                        data = data.Where(b => DbFunctions.TruncateTime(b.PQCriminal.PQPersonal.DeliveryDate) >= startDate.Date && DbFunctions.TruncateTime(b.PQCriminal.PQPersonal.DeliveryDate) <= EndDate.Date);
                    }

                    if (!string.IsNullOrEmpty(Search))
                    {
                        data = data.Where(b => (b.PQCriminal.PQPersonal.Name + " " + b.PQCriminal.PQPersonal.MiddleName + " " + b.PQCriminal.PQPersonal.LastName).ToString().Replace(" ", "").Contains(Search.Replace(" ", "")) || b.PQCriminal.PQPersonal.ClientRefID == Search);
                    }
                }
                else
                {
                    data = data.Where(b => b.PQCriminal.PersonalRowID == PersonalId);
                }

                switch (sort)
                {
                    default:
                        data = sortDir == "asc" ? data.OrderByDescending(d => d.CriminalStatusRowID) : data.OrderBy(d => d.CriminalStatusRowID);
                        break;
                }

                PQCriminalInsuffListPagedModel model = new PQCriminalInsuffListPagedModel();
                model.PageSize = pageSize;
                model.TotalRecords = data.Count();
                model.PQAllInsuffCriminals = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new PQCriminalInsuffViewModel
                {
                    CriminalRowID = item.CriminalRowId,
                    ClientRowID = item.PQCriminal.ClientRowID,
                    ClientName = item.PQCriminal.PQClientMaster.MasterAbbreviation.ClientName + " " + item.PQCriminal.PQClientMaster.MasterClientSubGroup.ClientSubGroupName,
                    PersonalRowID = item.PQCriminal.PersonalRowID,
                    CandidateName = item.PQCriminal.PQPersonal.CandTitle + " " + item.PQCriminal.PQPersonal.Name + " " + item.PQCriminal.PQPersonal.MiddleName + " " + item.PQCriminal.PQPersonal.LastName,
                    SubCheckRowID = item.PQCriminal.SubCheckRowID,
                    CheckFamilyRowID = item.PQCriminal.CheckFamilyRowID,
                    SubCheckName = item.PQCriminal.MasterSubCheckFamily.SubCheckName,
                    CompanyRefNo = item.PQCriminal.CRV_Sec_Ref_No,
                    InfSuffRaiseBy = db.TeamMembers.Where(t => t.TeamMemberRowID == item.InfSuffRaiseBy).Select(t => t.TMFirstName + " " + t.TMLastName).FirstOrDefault(),
                    InfSuffRaisedDate = item.InfSuffRaisedDate,
                    INFRemarks = item.InfSuffRaiseRemarks,
                    CreatedDate = item.PQCriminal.CreatedDate,
                    CheckStatus = item.PQCriminal.CheckStatus,
                    ReWorkCheckStatus = item.PQCriminal.ReWorkCheckStatus
                }).ToList();
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public PQCriminalInsuffListPagedModel GetAllCriminalInsuffListForDETL1(int pageNo, int pageSize, string sort, string sortDir, string subcheckType, string Search = "", short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", int PersonalId = 0, string spocEmail = "", short TeamMemberRowid = 0, int ClientUserRowID = 0,int allclints=0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataSet ds = new DataSet();
                PQCriminalInsuffListPagedModel model = new PQCriminalInsuffListPagedModel();
                ds = cc.getCheckwiseinsuffDetailSummary(ClientRowID, ClientUserRowID, subcheckType,Search,allclints);
                List<PQCriminalInsuffViewModel> lmd = new List<PQCriminalInsuffViewModel>();
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>
                {
                    lmd.Add(new PQCriminalInsuffViewModel
                    {
                        PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]),
                        CriminalRowID = Convert.ToInt32(dr["CriminalRowId"]),
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
                model.PQAllInsuffCriminals = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);


                //IQueryable<PQCriminalStatus> data = db.PQCriminalStatus.Include("PQCriminal").Where(p => (p.DETMQCCheckStatus == "Insufficent-QC" || p.TMQCCheckStatus == "Insufficent-QC") && p.InfSuffClearBy == 0);

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
                //    data = data.Where(b => b.PQCriminal.PQPersonal.ClientUserRowID == ClientUserRowID);
                //}

                //if (PersonalId == 0)
                //{
                //    if (ClientRowID > 0)
                //    {
                //        data = data.Where(b => b.PQCriminal.ClientRowID == ClientRowID);
                //    }

                //    if (!string.IsNullOrEmpty(spocEmail))
                //    {
                //        var Clients = db.PQClientMasters.Where(s => s.SpocEmailID.Contains(spocEmail)).Select(s => s.ClientRowID).ToList();
                //        data = data.Where(a => Clients.Contains(a.PQCriminal.ClientRowID));
                //    }

                //    if (!string.IsNullOrEmpty(RecievingToDate) && !string.IsNullOrEmpty(RecievingFromDate))
                //    {
                //        DateTime startDate = Convert.ToDateTime(RecievingFromDate);
                //        DateTime EndDate = Convert.ToDateTime(RecievingToDate);

                //        data = data.Where(b => DbFunctions.TruncateTime(b.PQCriminal.PQPersonal.OrderDate) >= startDate.Date && DbFunctions.TruncateTime(b.PQCriminal.PQPersonal.OrderDate) <= EndDate.Date);
                //    }

                //    if (!string.IsNullOrEmpty(CompletedToDate) && !string.IsNullOrEmpty(CompletedFromDate))
                //    {
                //        DateTime startDate = Convert.ToDateTime(CompletedFromDate);
                //        DateTime EndDate = Convert.ToDateTime(CompletedToDate);

                //        data = data.Where(b => DbFunctions.TruncateTime(b.PQCriminal.PQPersonal.DeliveryDate) >= startDate.Date && DbFunctions.TruncateTime(b.PQCriminal.PQPersonal.DeliveryDate) <= EndDate.Date);
                //    }

                //    if (!string.IsNullOrEmpty(Search))
                //    {
                //        data = data.Where(b => (b.PQCriminal.PQPersonal.Name + " " + b.PQCriminal.PQPersonal.MiddleName + " " + b.PQCriminal.PQPersonal.LastName).ToString().Replace(" ", "").Contains(Search.Replace(" ", "")) || b.PQCriminal.PQPersonal.ClientRefID == Search);
                //    }
                //}
                //else
                //{
                //    data = data.Where(b => b.PQCriminal.PersonalRowID == PersonalId);
                //}

                //switch (sort)
                //{
                //    default:
                //        data = sortDir == "asc" ? data.OrderByDescending(d => d.CriminalStatusRowID) : data.OrderBy(d => d.CriminalStatusRowID);
                //        break;
                //}

                //PQCriminalInsuffListPagedModel model = new PQCriminalInsuffListPagedModel();
                //model.PageSize = pageSize;
                //model.TotalRecords = data.Count();
                //model.PQAllInsuffCriminals = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new PQCriminalInsuffViewModel
                //{
                //    CriminalRowID = item.CriminalRowId,
                //    ClientRowID = item.PQCriminal.ClientRowID,
                //    ClientName = item.PQCriminal.PQClientMaster.MasterAbbreviation.ClientName + " " + item.PQCriminal.PQClientMaster.MasterClientSubGroup.ClientSubGroupName,
                //    PersonalRowID = item.PQCriminal.PersonalRowID,
                //    CandidateName = item.PQCriminal.PQPersonal.CandTitle + " " + item.PQCriminal.PQPersonal.Name + " " + item.PQCriminal.PQPersonal.MiddleName + " " + item.PQCriminal.PQPersonal.LastName,
                //    SubCheckRowID = item.PQCriminal.SubCheckRowID,
                //    CheckFamilyRowID = item.PQCriminal.CheckFamilyRowID,
                //    SubCheckName = item.PQCriminal.MasterSubCheckFamily.SubCheckName,
                //    CompanyRefNo = item.PQCriminal.CRV_Sec_Ref_No,
                //    InfSuffRaiseBy = db.TeamMembers.Where(t => t.TeamMemberRowID == item.InfSuffRaiseBy).Select(t => t.TMFirstName + " " + t.TMLastName).FirstOrDefault(),
                //    InfSuffRaisedDate = item.InfSuffRaisedDate,
                //    INFRemarks = item.InfSuffRaiseRemarks,
                //    CreatedDate = item.PQCriminal.CreatedDate,
                //    CheckStatus = item.PQCriminal.CheckStatus,
                //    ReWorkCheckStatus = item.PQCriminal.ReWorkCheckStatus
                //}).ToList();
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PQCriminalInsuffListPagedModel GetAllCriminalInsuffListForCSSpoc(int pageNo, int pageSize, string sort, string sortDir, string subcheckType, string Search = "", short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", int PersonalId = 0, string spocEmail = "", short TeamMemberRowid = 0, int ClientUserRowID = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataSet ds = new DataSet();
                PQCriminalInsuffListPagedModel model = new PQCriminalInsuffListPagedModel();
                ds = cc.getinsuffDetails_CSSpoc(subcheckType, spocEmail, ClientRowID,Search);
                List<PQCriminalInsuffViewModel> lmd = new List<PQCriminalInsuffViewModel>();
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        PQCriminalInsuffViewModel nn = new PQCriminalInsuffViewModel();
                        //lmd.Add(new PQEducationInsuffViewModel
                        //{
                        nn.PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]);
                        nn.CriminalRowID = Convert.ToInt32(dr["VerificationRowId"]);
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
                model.PQAllInsuffCriminals = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);                
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public UpdatePQCriminalInsuffClearViewModel GetPQCriminalDetailsById(int CriminalRowID = 0)
        {
            try
            {
                UpdatePQCriminalInsuffClearViewModel model = new UpdatePQCriminalInsuffClearViewModel();
                //var entity = db.PQCriminals.Find(CriminalRowID);

                var entity = db.PQCriminalStatus.Where(c => c.CriminalRowId == CriminalRowID).FirstOrDefault();
                if (entity != null)
                {
                    model.CriminalRowID = entity.CriminalRowId;
                    model.ClientRowID = entity.PQCriminal.ClientRowID;
                    model.PersonalRowID = entity.PQCriminal.PersonalRowID;
                    model.SubCheckRowID = entity.PQCriminal.SubCheckRowID;
                    model.UniqueComponentID = entity.PQCriminal.UniqueComponentID;
                    model.INFRemarks = entity.InfSuffClearComments;
                    model.CheckStatus = entity.PQCriminal.CheckStatus;
                    model.ReWorkCheckStatus = entity.PQCriminal.ReWorkCheckStatus;
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

        public void UpdatePQCriminalInsuffClear(UpdatePQCriminalInsuffClearViewModel model)
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

                            var CriminalStatusId = context.PQCriminalStatus.Where(c => c.CriminalRowId == model.CriminalRowID).FirstOrDefault().CriminalStatusRowID;

                            context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == CriminalStatusId).DETMCheckStatus = ScnCheckStatus;
                            context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == CriminalStatusId).InfSuffClearBy = model.InfSuffClearBy;
                            context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == CriminalStatusId).InfSuffClearDate = model.InfSuffClearDate;
                            context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == CriminalStatusId).InfSuffClearComments = model.INFRemarks;
                            context.SaveChanges();

                            //Check Status Update
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CheckStatus = ScnCheckStatus;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).ModifiedBy = model.ModifiedBy;
                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).ModifiedDate = model.ModifiedDate;
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

        public void ViewCriminaInfoDETL(ViewCriminalViewModel model)
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

                            context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CheckStatus = model.CheckStatus;
                            context.SaveChanges();

                            context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).DECaseStatus = "WIP";
                            context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).AllocatedToDEDate = DateTime.Now;
                            context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).DEQCCaseStatus = "WIP";
                            context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).AllocatedToDEQCDate = DateTime.Now;
                            context.SaveChanges();

                            int statusId = context.PQCriminalStatus.Where(p => p.CriminalRowId == model.CriminalRowID).FirstOrDefault().CriminalStatusRowID;

                            context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == statusId).DETMCheckStatus = model.CheckStatus;
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

        public void UpdateCriminalRejectionByDEQC(ViewCriminalViewModel model, bool flag = false)
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

                                context.PQCriminals.Single(c => c.CriminalRowId == model.CriminalRowID).CheckStatus = model.CheckStatus;

                                context.SaveChanges();

                                int statusId = context.PQCriminalStatus.Where(p => p.CriminalRowId == model.CriminalRowID).FirstOrDefault().CriminalStatusRowID;

                                if (flag)
                                {
                                    context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).DECaseStatus = "WIP";
                                    context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).AllocatedToDEDate = DateTime.Now;
                                    context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).DEQCCaseStatus = "WIP";
                                    context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).AllocatedToDEQCDate = DateTime.Now;
                                    context.SaveChanges();

                                    context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == statusId).DETMQCCheckStatus = "";
                                    context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == statusId).DETMCheckStatus = model.CheckStatus;
                                }
                                else
                                {
                                    context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == statusId).DETMQCCheckStatus = "Completed-QC";
                                    context.PQCriminalStatus.Single(c => c.CriminalStatusRowID == statusId).TMCheckStatus = model.CheckStatus;
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
    }
}
