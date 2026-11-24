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
    public class ReferenceInfoRepository : IReferenceInfoRepository
    {
        DataContext db;
        ClientCustomize cc;
        public ReferenceInfoRepository()
        {
            db = new DataContext();
            cc = new ClientCustomize();
        }

        public void AddReferenceInformation(PQReferenceViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            PQReference entity = new PQReference();
                            entity.ClientRowID = model.ClientRowID;
                            entity.PersonalRowID = model.PersonalRowID;
                            entity.ClientPackageRowID = model.ClientPackageRowID;
                            entity.CheckFamilyRowID = model.CheckFamilyRowID;
                            entity.SubCheckRowID = model.SubCheckRowID;
                            entity.UniqueComponentID = model.UniqueComponentID;
                            entity.RV_CandidateName = model.RV_CandidateName;
                            entity.RV_Sec_Ref_Id = model.RV_Sec_Ref_Id;
                            entity.RV_OrganisationName = model.RV_OrganisationName;
                            entity.RV_RefereeName = model.RV_RefereeName;
                            entity.RV_Refere_Desig = model.RV_Refere_Desig;
                            entity.RV_Refere_Add = model.RV_Refere_Add;
                            entity.RV_Refre_MobNo = model.RV_Refre_MobNo;
                            entity.RV_Refre_Eid = model.RV_Refre_Eid;
                            entity.RV_Know_Long = model.RV_Know_Long;
                            entity.RV_Tenure_Emp = model.RV_Tenure_Emp;
                            entity.RV_Dsg_RC = model.RV_Dsg_RC;
                            entity.RV_CTC = model.RV_CTC;
                            entity.RV_Reason_Leav = model.RV_Reason_Leav;
                            entity.RV_OtherDetails = model.RV_OtherDetails;
                            entity.RV_OtherDetails2 = model.RV_OtherDetails2;
                            entity.RV_OtherDetails3 = model.RV_OtherDetails3;
                            entity.RV_OtherDetails4 = model.RV_OtherDetails4;
                            entity.RV_OtherDetails5 = model.RV_OtherDetails5;
                            entity.RV_OtherDetails6 = model.RV_OtherDetails6;
                            entity.RV_OtherDetails7 = model.RV_OtherDetails7;
                            entity.RV_OtherDetails8 = model.RV_OtherDetails8;
                            entity.RV_OtherDetails9 = model.RV_OtherDetails9;
                            entity.RV_OtherDetails10 = model.RV_OtherDetails10;
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
                            context.PQReferences.Add(entity);
                            context.SaveChanges();

                            #region Save Check Status

                            PQReferenceStatus chkEntity = new PQReferenceStatus();
                            var refModel = context.PQReferences.Include("PQPersonal").Where(p => p.ReferenceRowID == entity.ReferenceRowID).FirstOrDefault();
                            chkEntity.ReferenceRowID = refModel.ReferenceRowID;
                            chkEntity.DETLAllocatedTo = refModel.PQPersonal.AllocatedBy;
                            chkEntity.DETMCheckStatus = model.CheckStatus;
                            chkEntity.DETMAllocatedTo = refModel.PQPersonal.AllocatedToDE;
                            chkEntity.DETMAllocatedDate = refModel.PQPersonal.AllocatedToDEDate;
                            chkEntity.DETMQCAllocatedTo = refModel.PQPersonal.AllocatedToDEQC;
                            chkEntity.DETMQCAllocatedDate = refModel.PQPersonal.AllocatedToDEQCDate;

                            if (model.CheckStatus == "Completed")
                            {
                                chkEntity.DETMQCCheckStatus = model.CheckStatus;
                            }
                            if (model.CheckStatus == "Insufficent")
                            {
                                chkEntity.DETMQCCheckStatus = model.CheckStatus;
                                chkEntity.InfSuffRaiseBy = refModel.PQPersonal.AllocatedToDE;
                                chkEntity.InfSuffRaisedDate = refModel.CreatedDate;
                                chkEntity.InfSuffRaiseRemarks = model.INFRemarks;
                            }
                            if (model.CheckStatus == "Rejected" || model.CheckStatus == "Review Requested")
                            {
                                chkEntity.Remarks = model.INFRemarks;
                            }
                            context.PQReferenceStatus.Add(chkEntity);
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

        public int GetAllReferenceByClientId(short ClientRowID = 0)
        {
            try
            {
                var data = db.PQReferences.Where(p => p.ClientRowID == ClientRowID).ToList();
                return data.Count();
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

        public PQReferenceViewModel GetReferenceInfoForDataEntry(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0)
        {
            try
            {
                return db.PQReferences.Where(a => a.ClientRowID == ClientRowId && a.PersonalRowID == PersonalRowID
                && a.CheckFamilyRowID == ChFamilRowId && a.SubCheckRowID == subCheckId).Select(a => new PQReferenceViewModel
                {
                    ReferenceRowID = a.ReferenceRowID,
                    ClientRowID = a.ClientRowID,
                    PersonalRowID = a.PersonalRowID,
                    ClientPackageRowID = a.ClientPackageRowID,
                    CheckFamilyRowID = a.CheckFamilyRowID,
                    SubCheckRowID = a.SubCheckRowID,
                    UniqueComponentID = a.UniqueComponentID,
                    //RV_CandidateName = a.RV_CandidateName,
                    RV_CandidateName = db.PQPersonals.Where(T => T.PersonalRowID == a.PersonalRowID).Select(x => x.Name + " " + x.MiddleName + x.LastName).FirstOrDefault().ToString(),
                    RV_OrganisationName = a.RV_OrganisationName,
                    RV_RefereeName = a.RV_RefereeName,
                    RV_Refere_Desig = a.RV_Refere_Desig,
                    RV_Refere_Add = a.RV_Refere_Add,
                    RV_Refre_MobNo = a.RV_Refre_MobNo,
                    RV_Refre_Eid = a.RV_Refre_Eid,
                    RV_Know_Long = a.RV_Know_Long,
                    RV_Tenure_Emp = a.RV_Tenure_Emp,
                    RV_Dsg_RC = a.RV_Dsg_RC,
                    RV_CTC = a.RV_CTC,
                    RV_Reason_Leav = a.RV_Reason_Leav,
                    RV_OtherDetails = a.RV_OtherDetails,
                    RV_OtherDetails2 = a.RV_OtherDetails2,
                    RV_OtherDetails3 = a.RV_OtherDetails3,
                    RV_OtherDetails4 = a.RV_OtherDetails4,
                    RV_OtherDetails5 = a.RV_OtherDetails5,
                    RV_OtherDetails6 = a.RV_OtherDetails6,
                    RV_OtherDetails7 = a.RV_OtherDetails7,
                    RV_OtherDetails8 = a.RV_OtherDetails8,
                    RV_OtherDetails9 = a.RV_OtherDetails9,
                    RV_OtherDetails10 = a.RV_OtherDetails10,
                    ATA_CID_No = a.ATA_CID_No,
                    ATA_Cmpny_Addr = a.ATA_Cmpny_Addr,
                    CheckStatus = a.CheckStatus,
                    ReWorkCheckStatus = a.ReWorkCheckStatus,
                    //InfSuffRaiseBy = a.InfSuffRaiseBy,
                    //InfSuffRaisedDate = a.InfSuffRaisedDate,
                    //INFRemarks = a.INFRemarks,
                }).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateReferenceInfoByDataEntry(PQReferenceViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).RV_OrganisationName = model.RV_OrganisationName;
                            context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).RV_RefereeName = model.RV_RefereeName;
                            context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).RV_Refere_Desig = model.RV_Refere_Desig;
                            context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).RV_Refere_Add = model.RV_Refere_Add;
                            context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).RV_Refre_MobNo = model.RV_Refre_MobNo;
                            //context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).RV_CandidateName = model.RV_CandidateName;
                            context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).RV_Refre_Eid = model.RV_Refre_Eid;
                            context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).RV_Know_Long = model.RV_Know_Long;
                            context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).RV_Tenure_Emp = model.RV_Tenure_Emp;
                            context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).RV_Dsg_RC = model.RV_Dsg_RC;
                            context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).RV_CTC = model.RV_CTC;
                            context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).RV_Reason_Leav = model.RV_Reason_Leav;
                            context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).RV_OtherDetails = model.RV_OtherDetails;
                            context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).RV_OtherDetails2 = model.RV_OtherDetails2;
                            context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).RV_OtherDetails3 = model.RV_OtherDetails3;
                            context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).RV_OtherDetails4 = model.RV_OtherDetails4;
                            context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).RV_OtherDetails5 = model.RV_OtherDetails5;
                            context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).RV_OtherDetails6 = model.RV_OtherDetails6;
                            context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).RV_OtherDetails7 = model.RV_OtherDetails7;
                            context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).RV_OtherDetails8 = model.RV_OtherDetails8;
                            context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).RV_OtherDetails9 = model.RV_OtherDetails9;
                            context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).RV_OtherDetails10 = model.RV_OtherDetails10;
                            context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).ATA_CID_No = model.ATA_CID_No;
                            context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).ATA_Cmpny_Addr = model.ATA_Cmpny_Addr;

                            context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).CheckStatus = model.CheckStatus;
                            if (model.CheckStatus == "Completed")
                            {
                                context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).CheckStatus = "Completed-DE";
                            }
                            else if (model.CheckStatus == "Not Applicable")
                            {
                                context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).CheckStatus = "Not Applicable";
                            }

                            context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).ModifiedBy = model.CreatedBy;
                            context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).ModifiedDate = model.CreatedDate;
                            context.SaveChanges();

                            #region Save Check Status

                            var status = context.PQReferenceStatus.Where(p => p.ReferenceRowID == model.ReferenceRowID).FirstOrDefault();

                            if (status.TMCheckStatus == "Insufficent" && status.TMQCCheckStatus == "Insufficent-QC" && model.CheckStatus == "Completed")
                            {
                                context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).CheckStatus = "WIP-Pending";
                                context.SaveChanges();

                                context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == status.ReferenceStatusRowID).DETMCheckStatus = model.CheckStatus;
                                context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == status.ReferenceStatusRowID).DETMAllocatedDate = model.CreatedDate;
                                context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == status.ReferenceStatusRowID).TMCheckStatus = "WIP-Pending";
                                context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == status.ReferenceStatusRowID).TMAllocatedDate = model.CreatedDate;
                                context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == status.ReferenceStatusRowID).TMQCCheckStatus = "";
                                context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == status.ReferenceStatusRowID).TMQCAllocatedDate = model.CreatedDate;
                            }
                            else if(model.CheckStatus == "Not Applicable")
                            {
                                context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).CheckStatus = "Not Applicable";
                                context.SaveChanges();                                
                            }
                            else
                            {
                                context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == status.ReferenceStatusRowID).DETMCheckStatus = model.CheckStatus;
                                context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == status.ReferenceStatusRowID).DETMAllocatedDate = model.CreatedDate;

                                if (model.CheckStatus == "Completed")
                                {
                                    context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == status.ReferenceStatusRowID).DETMQCCheckStatus = model.CheckStatus;
                                    context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == status.ReferenceStatusRowID).DETMQCAllocatedDate = model.CreatedDate;
                                }
                                if (model.CheckStatus == "Insufficent")
                                {
                                    context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == status.ReferenceStatusRowID).DETMQCCheckStatus = model.CheckStatus;
                                    context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == status.ReferenceStatusRowID).DETMQCAllocatedDate = model.CreatedDate;
                                    context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == status.ReferenceStatusRowID).InfSuffRaiseBy = model.CreatedBy;
                                    context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == status.ReferenceStatusRowID).InfSuffRaisedDate = model.CreatedDate;
                                    context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == status.ReferenceStatusRowID).InfSuffRaiseRemarks = model.INFRemarks;
                                }
                                if (model.CheckStatus == "Rejected" || model.CheckStatus == "Review Requested")
                                {
                                    context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == status.ReferenceStatusRowID).Remarks = model.INFRemarks;
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

        public PQUpdateReferenceViewModel GetReferenceInfoForQualityCheck(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0)
        {
            try
            {
                return db.PQReferences.Where(a => a.ClientRowID == ClientRowId && a.PersonalRowID == PersonalRowID
                && a.CheckFamilyRowID == ChFamilRowId && a.SubCheckRowID == subCheckId).Select(a => new PQUpdateReferenceViewModel
                {
                    ReferenceRowID = a.ReferenceRowID,
                    ClientRowID = a.ClientRowID,
                    PersonalRowID = a.PersonalRowID,
                    ClientPackageRowID = a.ClientPackageRowID,
                    CheckFamilyRowID = a.CheckFamilyRowID,
                    SubCheckRowID = a.SubCheckRowID,
                    UniqueComponentID = a.UniqueComponentID,
                    RV_CandidateName = a.RV_CandidateName,
                    RV_OrganisationName = a.RV_OrganisationName,
                    RV_RefereeName = a.RV_RefereeName,
                    RV_Refere_Desig = a.RV_Refere_Desig,
                    RV_Refere_Add = a.RV_Refere_Add,
                    RV_Refre_MobNo = a.RV_Refre_MobNo,
                    RV_Refre_Eid = a.RV_Refre_Eid,
                    RV_Know_Long = a.RV_Know_Long,
                    RV_Tenure_Emp = a.RV_Tenure_Emp,
                    RV_Dsg_RC = a.RV_Dsg_RC,
                    RV_CTC = a.RV_CTC,
                    RV_Reason_Leav = a.RV_Reason_Leav,
                    RV_OtherDetails = a.RV_OtherDetails,
                    RV_OtherDetails2 = a.RV_OtherDetails2,
                    RV_OtherDetails3 = a.RV_OtherDetails3,
                    RV_OtherDetails4 = a.RV_OtherDetails4,
                    RV_OtherDetails5 = a.RV_OtherDetails5,
                    RV_OtherDetails6 = a.RV_OtherDetails6,
                    RV_OtherDetails7 = a.RV_OtherDetails7,
                    RV_OtherDetails8 = a.RV_OtherDetails8,
                    RV_OtherDetails9 = a.RV_OtherDetails9,
                    RV_OtherDetails10 = a.RV_OtherDetails10,
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

        public PQUpdateReferenceViewModel GetReferenceInfoForQualityCheck(int ReferenceRowID = 0)
        {
            try
            {
                return db.PQReferences.Where(a => a.ReferenceRowID == ReferenceRowID).Select(a => new PQUpdateReferenceViewModel
                {
                    ReferenceRowID = a.ReferenceRowID,
                    ClientRowID = a.ClientRowID,
                    PersonalRowID = a.PersonalRowID,
                    ClientPackageRowID = a.ClientPackageRowID,
                    CheckFamilyRowID = a.CheckFamilyRowID,
                    SubCheckRowID = a.SubCheckRowID,
                    UniqueComponentID = a.UniqueComponentID,
                    RV_CandidateName = a.RV_CandidateName,
                    RV_OrganisationName = a.RV_OrganisationName,
                    RV_RefereeName = a.RV_RefereeName,
                    RV_Refere_Desig = a.RV_Refere_Desig,
                    RV_Refere_Add = a.RV_Refere_Add,
                    RV_Refre_MobNo = a.RV_Refre_MobNo,
                    RV_Refre_Eid = a.RV_Refre_Eid,
                    RV_Know_Long = a.RV_Know_Long,
                    RV_Tenure_Emp = a.RV_Tenure_Emp,
                    RV_Dsg_RC = a.RV_Dsg_RC,
                    RV_CTC = a.RV_CTC,
                    RV_Reason_Leav = a.RV_Reason_Leav,
                    RV_OtherDetails = a.RV_OtherDetails,
                    RV_OtherDetails2 = a.RV_OtherDetails2,
                    RV_OtherDetails3 = a.RV_OtherDetails3,
                    RV_OtherDetails4 = a.RV_OtherDetails4,
                    RV_OtherDetails5 = a.RV_OtherDetails5,
                    RV_OtherDetails6 = a.RV_OtherDetails6,
                    RV_OtherDetails7 = a.RV_OtherDetails7,
                    RV_OtherDetails8 = a.RV_OtherDetails8,
                    RV_OtherDetails9 = a.RV_OtherDetails9,
                    RV_OtherDetails10 = a.RV_OtherDetails10,
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="model"></param>
        public void UpdateReferenceInfoByQC(PQUpdateReferenceViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).RV_OrganisationName = model.RV_OrganisationName;
                            context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).RV_RefereeName = model.RV_RefereeName;
                            context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).RV_Refere_Desig = model.RV_Refere_Desig;
                            context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).RV_Refere_Add = model.RV_Refere_Add;
                            context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).RV_Refre_MobNo = model.RV_Refre_MobNo;
                            context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).RV_Refre_Eid = model.RV_Refre_Eid;
                            context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).RV_Know_Long = model.RV_Know_Long;
                            //context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).RV_CandidateName = model.RV_CandidateName;
                            context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).RV_Tenure_Emp = model.RV_Tenure_Emp;
                            context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).RV_Dsg_RC = model.RV_Dsg_RC;
                            context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).RV_CTC = model.RV_CTC;
                            context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).RV_Reason_Leav = model.RV_Reason_Leav;
                            context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).RV_OtherDetails = model.RV_OtherDetails;
                            context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).RV_OtherDetails2 = model.RV_OtherDetails2;
                            context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).RV_OtherDetails3 = model.RV_OtherDetails3;
                            context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).RV_OtherDetails4 = model.RV_OtherDetails4;
                            context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).RV_OtherDetails5 = model.RV_OtherDetails5;
                            context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).RV_OtherDetails6 = model.RV_OtherDetails6;
                            context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).RV_OtherDetails7 = model.RV_OtherDetails7;
                            context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).RV_OtherDetails8 = model.RV_OtherDetails8;
                            context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).RV_OtherDetails9 = model.RV_OtherDetails9;
                            context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).RV_OtherDetails10 = model.RV_OtherDetails10;
                            context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).ATA_CID_No = model.ATA_CID_No;
                            context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).ATA_Cmpny_Addr = model.ATA_Cmpny_Addr;
                            context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).ModifiedBy = model.ModifiedBy;
                            context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).ModifiedDate = model.ModifiedDate;

                            context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).CheckStatus = model.CheckStatus;
                            if (model.CheckStatus == "Completed-DE")
                            {
                                context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).CheckStatus = "Allocated-Mgr";
                            }

                            context.SaveChanges();

                            #region Save Check Status

                            var CurrentStatus = context.PQReferenceStatus.Where(p => p.ReferenceRowID == model.ReferenceRowID).FirstOrDefault();

                            if (model.CheckStatus == "Completed-DE")
                            {
                                if (CurrentStatus.TMCheckStatus == "Rejected")
                                {
                                    context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).CheckStatus = "WIP-Pending";
                                    context.SaveChanges();

                                    context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == CurrentStatus.ReferenceStatusRowID).DETMQCCheckStatus = "Completed-QC";
                                    context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == CurrentStatus.ReferenceStatusRowID).TMCheckStatus = "WIP-Pending";
                                    context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == CurrentStatus.ReferenceStatusRowID).TMAllocatedDate = model.ModifiedDate;
                                }
                                else
                                {
                                    context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == CurrentStatus.ReferenceStatusRowID).DETMQCCheckStatus = "Completed-QC";
                                    context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == CurrentStatus.ReferenceStatusRowID).MgrCheckStatus = "Allocated-Mgr";
                                    context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == CurrentStatus.ReferenceStatusRowID).MgrAllocatedTo = model.MgrAllocatedTo;
                                    context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == CurrentStatus.ReferenceStatusRowID).MgrAllocatedDate = model.ModifiedDate;
                                }

                            }

                            if (model.CheckStatus == "Insufficent")
                            {
                                context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == CurrentStatus.ReferenceStatusRowID).DETMQCCheckStatus = "Insufficent-QC";
                                context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == CurrentStatus.ReferenceStatusRowID).InfSuffClearBy = 0;
                                context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == CurrentStatus.ReferenceStatusRowID).InfSuffClearComments = "";
                                context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == CurrentStatus.ReferenceStatusRowID).InfSuffClearDate = null;
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

        public void UpdateReferenceInfoByManager(PQUpdateReferenceManagerViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).CheckStatus = model.CheckStatus;
                            context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).ModifiedBy = model.ModifiedBy;
                            context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).ModifiedDate = model.ModifiedDate;
                            context.SaveChanges();

                            #region Save Check Status

                            int statusId = context.PQReferenceStatus.Where(p => p.ReferenceRowID == model.ReferenceRowID).FirstOrDefault().ReferenceStatusRowID;

                            context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == statusId).TLCheckStatus = model.CheckStatus;
                            context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == statusId).TLAllocatedTo = model.TLAllocatedTo;
                            context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == statusId).TLAllocatedDate = model.ModifiedDate;
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

        public void UpdateReferenceInfoByTeamLeader(PQUpdateReferenceTeamLeaderViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).CheckStatus = model.CheckStatus;
                            context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).ModifiedBy = model.ModifiedBy;
                            context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).ModifiedDate = model.ModifiedDate;

                            context.SaveChanges();

                            #region Save Check Status

                            int statusId = context.PQReferenceStatus.Where(p => p.ReferenceRowID == model.ReferenceRowID).FirstOrDefault().ReferenceStatusRowID;

                            context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == statusId).TMCheckStatus = model.CheckStatus;
                            context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == statusId).TMAllocatedTo = model.TMAllocatedTo;
                            context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == statusId).TMAllocatedDate = model.ModifiedDate;

                            context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == statusId).TMQCCheckStatus = model.CheckStatus;
                            context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == statusId).TMQCAllocatedTo = model.TMQCAllocatedTo;
                            context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == statusId).TMQCAllocatedDate = model.ModifiedDate;

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

        public void UpdateReferenceInfoByTeamLeaderReAllocated(PQUpdateReferenceTeamLeaderViewModel model)
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

                            int statusId = context.PQReferenceStatus.Where(p => p.ReferenceRowID == model.ReferenceRowID).FirstOrDefault().ReferenceStatusRowID;
                            if (model.TMAllocatedTo > 0)
                            {
                                context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == statusId).TMAllocatedTo = model.TMAllocatedTo;
                                context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == statusId).TMAllocatedDate = model.ModifiedDate;
                            }
                            if (model.TMQCAllocatedTo > 0)
                            {
                                context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == statusId).TMQCAllocatedTo = model.TMQCAllocatedTo;
                                context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == statusId).TMQCAllocatedDate = model.ModifiedDate;
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

        public void UpdateReferenceInfoByTeamMember(PQUpdateReferenceTeamMemberViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).CheckStatus = model.CheckStatus;
                            context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).ModifiedBy = model.ModifiedBy;
                            context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).ModifiedDate = model.ModifiedDate;
                            context.SaveChanges();

                            #region Save Check Status

                            var statusId = context.PQReferenceStatus.Where(p => p.ReferenceRowID == model.ReferenceRowID).FirstOrDefault().ReferenceStatusRowID;

                            context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == statusId).TMCheckStatus = model.CheckStatus;
                            context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == statusId).TMQCCheckStatus = "";
                            if (model.CheckStatus == "PV Raised")
                            {
                                context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == statusId).PVMgrCheckStatus = "Allocate-Mgr";
                                context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == statusId).PVMgrAllocatedTo = model.PVMgrAllocatedTo;
                                context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == statusId).PVMgrAllocatedDate = model.ModifiedDate;
                            }
                            if (model.CheckStatus == "Insufficent")
                            {
                                context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == statusId).TMQCCheckStatus = model.CheckStatus;
                                context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == statusId).InfSuffRaiseRemarks = model.ActionRemark;
                                context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == statusId).InfSuffRaiseBy = model.ModifiedBy;
                                context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == statusId).InfSuffRaisedDate = model.ModifiedDate;
                            }
                            if (model.CheckStatus == "Rejected" || model.CheckStatus == "Funds Pending")
                            {
                                context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == statusId).Remarks = model.ActionRemark;
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

        public void RejectionUpdateRefrenceInfoByQC(UpdateRefrnceRejectionViewModel model)
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
                            context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).CheckStatus = ActualCheckStatus;
                            context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).ModifiedBy = model.ModifiedBy;
                            context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).ModifiedDate = model.ModifiedDate;
                            context.SaveChanges();

                            #region Update Check Status

                            int statusId = context.PQReferenceStatus.Where(p => p.ReferenceRowID == model.ReferenceRowID).FirstOrDefault().ReferenceStatusRowID;
                            context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == statusId).DETMCheckStatus = ActualCheckStatus;
                            context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == statusId).DETMQCCheckStatus = "";
                            context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == statusId).Remarks = model.INFRemarks;
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

        public void ViewReferenceInfoByVrifierTL(ViewReferenceViewModel model)
        {

            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).CheckStatus = model.CheckStatus;
                            context.SaveChanges();

                            int statusId = context.PQReferenceStatus.Where(p => p.ReferenceRowID == model.ReferenceRowID).FirstOrDefault().ReferenceStatusRowID;

                            context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == statusId).TMCheckStatus = model.CheckStatus;
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

        public bool IfExistReference(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0, string CheckStatus = "")
        {
            try
            {
                var listChecks = db.PQReferences.Where(a => a.ClientRowID == ClientRowId && a.PersonalRowID == PersonalRowID
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

        public bool IfExistReferenceForDELeftMenu(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0)
        {
            try
            {
                var listChecks = db.PQReferences.Where(a => a.ClientRowID == ClientRowId && a.PersonalRowID == PersonalRowID
                     && a.CheckFamilyRowID == ChFamilRowId && a.SubCheckRowID == subCheckId).FirstOrDefault();
                if (listChecks != null)
                {
                    var ChkStatus = db.PQReferenceStatus.Where(s => s.ReferenceRowID == listChecks.ReferenceRowID).FirstOrDefault();

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

        public bool IfExistReferenceForQCLeftMenu(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0, int CheckStatusid = 0)
        {
            try
            {
                //var listChecks = db.PQReferences.Where(a => a.ClientRowID == ClientRowId && a.PersonalRowID == PersonalRowID
                //     && a.CheckFamilyRowID == ChFamilRowId && a.SubCheckRowID == subCheckId).FirstOrDefault();
                //if (listChecks != null)
                //{
                //    var ChkStatus = db.PQReferenceStatus.Where(s => s.ReferenceRowID == listChecks.ReferenceRowID).FirstOrDefault();

                    if ( CheckStatusid == 68 || CheckStatusid == 14 || CheckStatusid == 15 || CheckStatusid == 42)
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

        #region ****** Temp ******

        public TempPQReferenceViewModel GetTempReferenceInfoForBySelf(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0)
        {
            try
            {
                return db.TempPQReferences.Where(a => a.ClientRowID == ClientRowId && a.PersonalRowID == PersonalRowID
                && a.CheckFamilyRowID == ChFamilRowId && a.SubCheckRowID == subCheckId).Select(a => new TempPQReferenceViewModel
                {
                    ReferenceRowID = a.ReferenceRowID,
                    ClientRowID = a.ClientRowID,
                    PersonalRowID = a.PersonalRowID,
                    ClientPackageRowID = a.ClientPackageRowID,
                    CheckFamilyRowID = a.CheckFamilyRowID,
                    SubCheckRowID = a.SubCheckRowID,
                    UniqueComponentID = a.UniqueComponentID,
                    RV_OrganisationName = a.RV_OrganisationName,
                    RV_RefereeName = a.RV_RefereeName,
                    RV_Refere_Desig = a.RV_Refere_Desig,
                    RV_Refere_Add = a.RV_Refere_Add,
                    RV_Refre_MobNo = a.RV_Refre_MobNo,
                    RV_Refre_Eid = a.RV_Refre_Eid,
                    RV_Know_Long = a.RV_Know_Long,
                    RV_Tenure_Emp = a.RV_Tenure_Emp,
                    RV_Dsg_RC = a.RV_Dsg_RC,
                    RV_CTC = a.RV_CTC,
                    RV_Reason_Leav = a.RV_Reason_Leav,
                    //RV_OtherProof = a.RV_OtherProof,
                    RV_OtherDetails = a.RV_OtherDetails,
                    RV_OtherDetails2 = a.RV_OtherDetails2,
                    RV_OtherDetails3 = a.RV_OtherDetails3,
                    RV_OtherDetails4 = a.RV_OtherDetails4,
                    RV_OtherDetails5 = a.RV_OtherDetails5,
                    RV_OtherDetails6 = a.RV_OtherDetails6,
                    RV_OtherDetails7 = a.RV_OtherDetails7,
                    RV_OtherDetails8 = a.RV_OtherDetails8,
                    RV_OtherDetails9 = a.RV_OtherDetails9,
                    RV_OtherDetails10 = a.RV_OtherDetails10,
                    ATA_CID_No = a.ATA_CID_No,
                    ATA_Cmpny_Addr = a.ATA_Cmpny_Addr,
                    CheckStatus = a.CheckStatus,
                    RV_CandidateName = a.RV_CandidateName,
                }).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetAllTempReferenceByClientId(short ClientRowID = 0)
        {
            try
            {
                var data = db.TempPQReferences.Where(p => p.ClientRowID == ClientRowID).ToList();
                return data.Count();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateTempReferenceInfoBySelf(TempPQReferenceViewModel model)
        {
            try
            {
                if (model != null)
                {
                    db.TempPQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).RV_OrganisationName = model.RV_OrganisationName;
                    db.TempPQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).RV_RefereeName = model.RV_RefereeName;
                    db.TempPQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).RV_Refere_Desig = model.RV_Refere_Desig;
                    db.TempPQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).RV_Refere_Add = model.RV_Refere_Add;
                    db.TempPQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).RV_Refre_MobNo = model.RV_Refre_MobNo;
                    db.TempPQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).RV_Refre_Eid = model.RV_Refre_Eid;
                    db.TempPQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).RV_Know_Long = model.RV_Know_Long;
                    db.TempPQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).RV_Tenure_Emp = model.RV_Tenure_Emp;
                    db.TempPQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).RV_Dsg_RC = model.RV_Dsg_RC;
                    db.TempPQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).RV_CTC = model.RV_CTC;
                    db.TempPQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).RV_Reason_Leav = model.RV_Reason_Leav;
                    //db.TempPQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).RV_OtherProof = model.RV_OtherProof;
                    db.TempPQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).RV_OtherDetails = model.RV_OtherDetails;
                    db.TempPQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).RV_OtherDetails2 = model.RV_OtherDetails2;
                    db.TempPQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).RV_OtherDetails3 = model.RV_OtherDetails3;
                    db.TempPQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).RV_OtherDetails4 = model.RV_OtherDetails4;
                    db.TempPQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).RV_OtherDetails5 = model.RV_OtherDetails5;
                    db.TempPQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).RV_OtherDetails6 = model.RV_OtherDetails6;
                    db.TempPQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).RV_OtherDetails7 = model.RV_OtherDetails7;
                    db.TempPQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).RV_OtherDetails8 = model.RV_OtherDetails8;
                    db.TempPQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).RV_OtherDetails9 = model.RV_OtherDetails9;
                    db.TempPQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).RV_OtherDetails10 = model.RV_OtherDetails10;
                    db.TempPQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).ATA_CID_No = model.ATA_CID_No;
                    db.TempPQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).ATA_Cmpny_Addr = model.ATA_Cmpny_Addr;
                    //db.TempPQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).CheckStatus = model.CheckStatus;
                    db.TempPQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).ModifiedBy = model.ModifiedBy;
                    db.TempPQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).ModifiedDate = model.ModifiedDate;
                    db.TempPQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).RV_CandidateName = model.RV_CandidateName;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddTempReferenceInformation(TempPQReferenceViewModel model)
        {
            try
            {
                if (model != null)
                {
                    TempPQReference entity = new TempPQReference();
                    entity.ClientRowID = model.ClientRowID;
                    entity.PersonalRowID = model.PersonalRowID;
                    entity.ClientPackageRowID = model.ClientPackageRowID;
                    entity.CheckFamilyRowID = model.CheckFamilyRowID;
                    entity.SubCheckRowID = model.SubCheckRowID;
                    entity.UniqueComponentID = model.UniqueComponentID;
                    entity.RV_CandidateName = model.RV_CandidateName;
                    entity.RV_Sec_Ref_Id = model.RV_Sec_Ref_Id;
                    entity.RV_OrganisationName = model.RV_OrganisationName;
                    entity.RV_RefereeName = model.RV_RefereeName;
                    entity.RV_Refere_Desig = model.RV_Refere_Desig;
                    entity.RV_Refere_Add = model.RV_Refere_Add;
                    entity.RV_Refre_MobNo = model.RV_Refre_MobNo;
                    entity.RV_Refre_Eid = model.RV_Refre_Eid;
                    entity.RV_Know_Long = model.RV_Know_Long;
                    entity.RV_Tenure_Emp = model.RV_Tenure_Emp;
                    entity.RV_Dsg_RC = model.RV_Dsg_RC;
                    entity.RV_CTC = model.RV_CTC;
                    entity.RV_Reason_Leav = model.RV_Reason_Leav;
                    //entity.RV_OtherProof = model.RV_OtherProof;
                    entity.RV_OtherDetails = model.RV_OtherDetails;
                    entity.RV_OtherDetails2 = model.RV_OtherDetails2;
                    entity.RV_OtherDetails3 = model.RV_OtherDetails3;
                    entity.RV_OtherDetails4 = model.RV_OtherDetails4;
                    entity.RV_OtherDetails5 = model.RV_OtherDetails5;
                    entity.RV_OtherDetails6 = model.RV_OtherDetails6;
                    entity.RV_OtherDetails7 = model.RV_OtherDetails7;
                    entity.RV_OtherDetails8 = model.RV_OtherDetails8;
                    entity.RV_OtherDetails9 = model.RV_OtherDetails9;
                    entity.RV_OtherDetails10 = model.RV_OtherDetails10;
                    entity.ATA_CID_No = model.ATA_CID_No;
                    entity.ATA_Cmpny_Addr = model.ATA_Cmpny_Addr;
                    entity.CreatedBy = model.CreatedBy;
                    entity.CreatedDate = model.CreatedDate;
                    entity.CheckStatus = model.CheckStatus;
                    entity.Remarks = model.Remarks;
                    entity.Status = model.Status;
                    db.TempPQReferences.Add(entity);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        public PQReferenceInsuffListPagedModel GetAllReferenceInsuffListForDETL(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", int PersonalId = 0, string spocEmail = "", short TeamMemberRowid = 0, int ClientUserRowID = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                IQueryable<PQReferenceStatus> data = db.PQReferenceStatus.Include("PQReference").Where(p => (p.DETMQCCheckStatus == "Insufficent-QC" || p.TMQCCheckStatus == "Insufficent-QC") && p.InfSuffClearBy == 0);

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
                    data = data.Where(b => b.PQReference.PQPersonal.ClientUserRowID == ClientUserRowID);
                }

                if (PersonalId == 0)
                {
                    if (ClientRowID > 0)
                    {
                        data = data.Where(b => b.PQReference.ClientRowID == ClientRowID);
                    }

                    if (!string.IsNullOrEmpty(spocEmail))
                    {
                        var Clients = db.PQClientMasters.Where(s => s.SpocEmailID.Contains(spocEmail)).Select(s => s.ClientRowID).ToList();
                        data = data.Where(a => Clients.Contains(a.PQReference.ClientRowID));
                    }

                    if (!string.IsNullOrEmpty(RecievingToDate) && !string.IsNullOrEmpty(RecievingFromDate))
                    {
                        DateTime startDate = Convert.ToDateTime(RecievingFromDate);
                        DateTime EndDate = Convert.ToDateTime(RecievingToDate);

                        data = data.Where(b => DbFunctions.TruncateTime(b.PQReference.PQPersonal.OrderDate) >= startDate.Date && DbFunctions.TruncateTime(b.PQReference.PQPersonal.OrderDate) <= EndDate.Date);
                    }

                    if (!string.IsNullOrEmpty(CompletedToDate) && !string.IsNullOrEmpty(CompletedFromDate))
                    {
                        DateTime startDate = Convert.ToDateTime(CompletedFromDate);
                        DateTime EndDate = Convert.ToDateTime(CompletedToDate);

                        data = data.Where(b => DbFunctions.TruncateTime(b.PQReference.PQPersonal.DeliveryDate) >= startDate.Date && DbFunctions.TruncateTime(b.PQReference.PQPersonal.DeliveryDate) <= EndDate.Date);
                    }

                    if (!string.IsNullOrEmpty(Search))
                    {
                        data = data.Where(b => (b.PQReference.PQPersonal.Name + " " + b.PQReference.PQPersonal.MiddleName + " " + b.PQReference.PQPersonal.LastName).ToString().Replace(" ", "").Contains(Search.Replace(" ", "")) || b.PQReference.PQPersonal.ClientRefID == Search);
                    }
                }
                else
                {
                    data = data.Where(b => b.PQReference.PersonalRowID == PersonalId);
                }

                switch (sort)
                {
                    default:
                        data = sortDir == "asc" ? data.OrderByDescending(d => d.ReferenceStatusRowID) : data.OrderBy(d => d.ReferenceStatusRowID);
                        break;
                }

                PQReferenceInsuffListPagedModel model = new PQReferenceInsuffListPagedModel();
                model.PageSize = pageSize;
                model.TotalRecords = data.Count();
                model.PQAllInsuffReferences = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new PQReferenceInsuffViewModel
                {
                    ReferenceRowID = item.ReferenceRowID,
                    ClientRowID = item.PQReference.ClientRowID,
                    ClientName = item.PQReference.PQClientMaster.MasterAbbreviation.ClientName + " " + item.PQReference.PQClientMaster.MasterClientSubGroup.ClientSubGroupName,
                    PersonalRowID = item.PQReference.PersonalRowID,
                    CandidateName = item.PQReference.PQPersonal.CandTitle + " " + item.PQReference.PQPersonal.Name + " " + item.PQReference.PQPersonal.MiddleName + " " + item.PQReference.PQPersonal.LastName,
                    CheckFamilyRowID = item.PQReference.CheckFamilyRowID,
                    SubCheckRowID = item.PQReference.SubCheckRowID,
                    SubCheckName = item.PQReference.MasterSubCheckFamily.SubCheckName,
                    CompanyRefNo = item.PQReference.RV_Sec_Ref_Id,
                    InfSuffRaiseBy = db.TeamMembers.Where(t => t.TeamMemberRowID == item.InfSuffRaiseBy).Select(t => t.TMFirstName + " " + t.TMLastName).FirstOrDefault(),
                    InfSuffRaisedDate = item.InfSuffRaisedDate,
                    INFRemarks = item.InfSuffRaiseRemarks,
                    CreatedDate = item.PQReference.CreatedDate,
                    CheckStatus = item.PQReference.CheckStatus,
                    ReWorkCheckStatus = item.PQReference.ReWorkCheckStatus
                }).ToList();
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PQReferenceInsuffListPagedModel GetAllReferenceInsuffListForDETL1(int pageNo, int pageSize, string sort, string sortDir, string subcheckType, string Search = "", short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", int PersonalId = 0, string spocEmail = "", short TeamMemberRowid = 0, int ClientUserRowID = 0,int allclints=0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }
                DataSet ds = new DataSet();
                PQReferenceInsuffListPagedModel model = new PQReferenceInsuffListPagedModel();
                ds = cc.getCheckwiseinsuffDetailSummary(ClientRowID, ClientUserRowID, subcheckType,Search,allclints);
                List<PQReferenceInsuffViewModel> lmd = new List<PQReferenceInsuffViewModel>();
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>
                {
                    lmd.Add(new PQReferenceInsuffViewModel
                    {
                        PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]),
                        ReferenceRowID = Convert.ToInt32(dr["ReferenceRowID"]),
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
                model.PQAllInsuffReferences = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);

                //IQueryable<PQReferenceStatus> data = db.PQReferenceStatus.Include("PQReference").Where(p => (p.DETMQCCheckStatus == "Insufficent-QC" || p.TMQCCheckStatus == "Insufficent-QC") && p.InfSuffClearBy == 0);

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
                //    data = data.Where(b => b.PQReference.PQPersonal.ClientUserRowID == ClientUserRowID);
                //}

                //if (PersonalId == 0)
                //{
                //    if (ClientRowID > 0)
                //    {
                //        data = data.Where(b => b.PQReference.ClientRowID == ClientRowID);
                //    }

                //    if (!string.IsNullOrEmpty(spocEmail))
                //    {
                //        var Clients = db.PQClientMasters.Where(s => s.SpocEmailID.Contains(spocEmail)).Select(s => s.ClientRowID).ToList();
                //        data = data.Where(a => Clients.Contains(a.PQReference.ClientRowID));
                //    }

                //    if (!string.IsNullOrEmpty(RecievingToDate) && !string.IsNullOrEmpty(RecievingFromDate))
                //    {
                //        DateTime startDate = Convert.ToDateTime(RecievingFromDate);
                //        DateTime EndDate = Convert.ToDateTime(RecievingToDate);

                //        data = data.Where(b => DbFunctions.TruncateTime(b.PQReference.PQPersonal.OrderDate) >= startDate.Date && DbFunctions.TruncateTime(b.PQReference.PQPersonal.OrderDate) <= EndDate.Date);
                //    }

                //    if (!string.IsNullOrEmpty(CompletedToDate) && !string.IsNullOrEmpty(CompletedFromDate))
                //    {
                //        DateTime startDate = Convert.ToDateTime(CompletedFromDate);
                //        DateTime EndDate = Convert.ToDateTime(CompletedToDate);

                //        data = data.Where(b => DbFunctions.TruncateTime(b.PQReference.PQPersonal.DeliveryDate) >= startDate.Date && DbFunctions.TruncateTime(b.PQReference.PQPersonal.DeliveryDate) <= EndDate.Date);
                //    }

                //    if (!string.IsNullOrEmpty(Search))
                //    {
                //        data = data.Where(b => (b.PQReference.PQPersonal.Name + " " + b.PQReference.PQPersonal.MiddleName + " " + b.PQReference.PQPersonal.LastName).ToString().Replace(" ", "").Contains(Search.Replace(" ", "")) || b.PQReference.PQPersonal.ClientRefID == Search);
                //    }
                //}
                //else
                //{
                //    data = data.Where(b => b.PQReference.PersonalRowID == PersonalId);
                //}

                //switch (sort)
                //{
                //    default:
                //        data = sortDir == "asc" ? data.OrderByDescending(d => d.ReferenceStatusRowID) : data.OrderBy(d => d.ReferenceStatusRowID);
                //        break;
                //}

                //PQReferenceInsuffListPagedModel model = new PQReferenceInsuffListPagedModel();
                //model.PageSize = pageSize;
                //model.TotalRecords = data.Count();
                //model.PQAllInsuffReferences = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new PQReferenceInsuffViewModel
                //{
                //    ReferenceRowID = item.ReferenceRowID,
                //    ClientRowID = item.PQReference.ClientRowID,
                //    ClientName = item.PQReference.PQClientMaster.MasterAbbreviation.ClientName + " " + item.PQReference.PQClientMaster.MasterClientSubGroup.ClientSubGroupName,
                //    PersonalRowID = item.PQReference.PersonalRowID,
                //    CandidateName = item.PQReference.PQPersonal.CandTitle + " " + item.PQReference.PQPersonal.Name + " " + item.PQReference.PQPersonal.MiddleName + " " + item.PQReference.PQPersonal.LastName,
                //    CheckFamilyRowID = item.PQReference.CheckFamilyRowID,
                //    SubCheckRowID = item.PQReference.SubCheckRowID,
                //    SubCheckName = item.PQReference.MasterSubCheckFamily.SubCheckName,
                //    CompanyRefNo = item.PQReference.RV_Sec_Ref_Id,
                //    InfSuffRaiseBy = db.TeamMembers.Where(t => t.TeamMemberRowID == item.InfSuffRaiseBy).Select(t => t.TMFirstName + " " + t.TMLastName).FirstOrDefault(),
                //    InfSuffRaisedDate = item.InfSuffRaisedDate,
                //    INFRemarks = item.InfSuffRaiseRemarks,
                //    CreatedDate = item.PQReference.CreatedDate,
                //    CheckStatus = item.PQReference.CheckStatus,
                //    ReWorkCheckStatus = item.PQReference.ReWorkCheckStatus
                //}).ToList();
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PQReferenceInsuffListPagedModel GetAllReferenceInsuffListForCSSpoc(int pageNo, int pageSize, string sort, string sortDir, string subcheckType, string Search = "", short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", int PersonalId = 0, string spocEmail = "", short TeamMemberRowid = 0, int ClientUserRowID = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }
                DataSet ds = new DataSet();
                PQReferenceInsuffListPagedModel model = new PQReferenceInsuffListPagedModel();
                ds = cc.getinsuffDetails_CSSpoc(subcheckType, spocEmail, ClientRowID,Search);
                List<PQReferenceInsuffViewModel> lmd = new List<PQReferenceInsuffViewModel>();
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        PQReferenceInsuffViewModel nn = new PQReferenceInsuffViewModel();
                        //lmd.Add(new PQEducationInsuffViewModel
                        //{
                        nn.PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]);
                        nn.ReferenceRowID = Convert.ToInt32(dr["VerificationRowId"]);
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
                model.PQAllInsuffReferences = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);                
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public UpdatePQReferenceInsuffClearViewModel GetPQReferenceDetailsById(int ReferenceRowID = 0)
        {
            try
            {
                UpdatePQReferenceInsuffClearViewModel model = new UpdatePQReferenceInsuffClearViewModel();
                //var entity = db.PQReferences.Find(ReferenceRowID);

                var entity = db.PQReferenceStatus.Where(r => r.ReferenceRowID == ReferenceRowID).FirstOrDefault();
                if (entity != null)
                {
                    model.ReferenceRowID = entity.ReferenceRowID;
                    model.ClientRowID = entity.PQReference.ClientRowID;
                    model.PersonalRowID = entity.PQReference.PersonalRowID;
                    model.SubCheckRowID = entity.PQReference.SubCheckRowID;
                    model.UniqueComponentID = entity.PQReference.UniqueComponentID;
                    model.INFRemarks = entity.InfSuffClearComments;
                    model.CheckStatus = entity.PQReference.CheckStatus;
                    model.ReWorkCheckStatus = entity.PQReference.ReWorkCheckStatus;
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

        public void UpdatePQReferenceInsuffClear(UpdatePQReferenceInsuffClearViewModel model)
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

                            var ReferenceStatusId = context.PQReferenceStatus.Where(c => c.ReferenceRowID == model.ReferenceRowID).FirstOrDefault().ReferenceStatusRowID;

                            context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == ReferenceStatusId).DETMCheckStatus = ScnCheckStatus;
                            context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == ReferenceStatusId).InfSuffClearBy = model.InfSuffClearBy;
                            context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == ReferenceStatusId).InfSuffClearDate = model.InfSuffClearDate;
                            context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == ReferenceStatusId).InfSuffClearComments = model.INFRemarks;
                            context.SaveChanges();

                            //Check Status Update
                            context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).CheckStatus = ScnCheckStatus;
                            context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).ModifiedBy = model.ModifiedBy;
                            context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).ModifiedDate = model.ModifiedDate;
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

        public void ViewReferenceInfoDETL(PQUpdateReferenceViewModel model)
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

                                context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).CheckStatus = model.CheckStatus;
                                context.SaveChanges();

                                int statusId = context.PQReferenceStatus.Where(p => p.ReferenceRowID == model.ReferenceRowID).FirstOrDefault().ReferenceStatusRowID;

                                context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == statusId).DETMCheckStatus = model.CheckStatus;
                                context.SaveChanges();

                                context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).DECaseStatus = "WIP";
                                context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).AllocatedToDEDate = DateTime.Now;
                                context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).DEQCCaseStatus = "WIP";
                                context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).AllocatedToDEQCDate = DateTime.Now;
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

        public void UpdateReferenceRejectionByDEQC(PQUpdateReferenceViewModel model, bool flag = false)
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

                                context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceRowID).CheckStatus = model.CheckStatus;
                                context.SaveChanges();

                                int statusId = context.PQReferenceStatus.Where(p => p.ReferenceRowID == model.ReferenceRowID).FirstOrDefault().ReferenceStatusRowID;
                                if (flag)
                                {
                                    context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).DECaseStatus = "WIP";
                                    context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).AllocatedToDEDate = DateTime.Now;
                                    context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).DEQCCaseStatus = "WIP";
                                    context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).AllocatedToDEQCDate = DateTime.Now;
                                    context.SaveChanges();

                                    context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == statusId).DETMQCCheckStatus = "";
                                    context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == statusId).DETMCheckStatus = model.CheckStatus;
                                }
                                else
                                {
                                    context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == statusId).DETMQCCheckStatus = "Completed-QC";
                                    context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == statusId).TMCheckStatus = model.CheckStatus;
                                };
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
