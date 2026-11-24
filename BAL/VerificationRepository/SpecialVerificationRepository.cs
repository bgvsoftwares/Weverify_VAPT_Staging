using DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.ProvidedInfoViewModel;
using ViewModels.VerificationViewModel;
using System.Data;

namespace BAL.VerificationRepository
{
    public class SpecialVerificationRepository : ISpecialVerificationRepository
    {
        DataContext db;
        public SpecialVerificationRepository()
        {
            db = new DataContext();
        }

        public CheckCommonInfoForAllocationViewModel GetCheckCommonInfoForAllocation(int SpecialCheckRowId = 0)
        {
            try
            {
                CheckCommonInfoForAllocationViewModel model = new CheckCommonInfoForAllocationViewModel();
                var entity = db.PQSpecialChecks.Find(SpecialCheckRowId);
                if (entity != null)
                {
                    model.SpecialCheckRowId = entity.SpecialCheckRowId;
                    model.ClientRowID = entity.ClientRowID;
                    model.PersonalRowID = entity.PersonalRowID;
                    model.CheckFamilyRowID = entity.CheckFamilyRowID;
                    model.SubCheckRowID = entity.SubCheckRowID;
                    model.UniqueComponentID = entity.UniqueComponentID;
                    model.CheckStatus = entity.CheckStatus;
                    model.ReWorkCheckStatus = entity.ReWorkCheckStatus;
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

        public void ManagerAllocateCheckToTeamLeader(ManagerAllocateCheckToTeamLeaderViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            context.PQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowId).CheckStatus = model.CheckStatus;
                            context.PQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowId).ModifiedBy = model.ModifiedBy;
                            context.PQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowId).ModifiedDate = model.ModifiedDate;
                            context.SaveChanges();

                            #region Save Check Status

                            int statusId = context.PQSpecialCheckStatus.Where(p => p.SpecialCheckRowId == model.SpecialCheckRowId).FirstOrDefault().SpecialCheckStatusRowID;

                            context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == statusId).TLCheckStatus = model.CheckStatus;
                            context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == statusId).TLAllocatedTo = model.TLAllocatedTo;
                            context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == statusId).TLAllocatedDate = model.ModifiedDate;
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

        public void TeamLeaderAllocateCheckToTeamMember(TeamLeaderAllocateCheckToTeamMemberViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            context.PQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowId).CheckStatus = model.CheckStatus;
                            context.PQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowId).ModifiedBy = model.ModifiedBy;
                            context.PQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowId).ModifiedDate = model.ModifiedDate;

                            context.SaveChanges();

                            #region Save Check Status

                            int statusId = context.PQSpecialCheckStatus.Where(p => p.SpecialCheckRowId == model.SpecialCheckRowId).FirstOrDefault().SpecialCheckStatusRowID;

                            context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == statusId).TMCheckStatus = model.CheckStatus;
                            context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == statusId).TMAllocatedTo = model.TMAllocatedTo;
                            context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == statusId).TMAllocatedDate = model.ModifiedDate;

                            context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == statusId).TMQCCheckStatus = model.CheckStatus;
                            context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == statusId).TMQCAllocatedTo = model.TMQCAllocatedTo;
                            context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == statusId).TMQCAllocatedDate = model.ModifiedDate;


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

        public PQSpecialCheckVerViewModel GetSpecialCheckInfoForDataEntry(int SpecialChecksRowId = 0)
        {
            try
            {
                PQSpecialCheckVerViewModel model = new PQSpecialCheckVerViewModel();
                var SpecialCheckVer = db.PQSpecialCheckVers.Where(a => a.SpecialCheckRowId == SpecialChecksRowId).FirstOrDefault();
                int SpecialCheckVerRowid = 0;
                if (SpecialCheckVer != null && SpecialCheckVer.SpecialCheckVerRowID > 0)
                {
                    SpecialCheckVerRowid = SpecialCheckVer.SpecialCheckVerRowID;
                }

                if (SpecialCheckVerRowid > 0)
                {
                    #region MyRegion
                    model = db.PQSpecialCheckVers.Where(a => a.SpecialCheckVerRowID == SpecialCheckVerRowid).Select(a => new PQSpecialCheckVerViewModel
                    {
                        SpecialCheckRowID = a.SpecialCheckRowId,
                        SpecialCheckVerRowID = a.SpecialCheckVerRowID,
                        ClientRowID = a.PQSpecialCheck.ClientRowID,
                        PersonalRowID = a.PQSpecialCheck.PersonalRowID,
                        CheckFamilyRowID = a.PQSpecialCheck.CheckFamilyRowID,
                        SubCheckRowID = a.PQSpecialCheck.SubCheckRowID,
                        SC_SecuritasID = a.PQSpecialCheck.SC_SecuritasID,
                        SC_Cand_Name = a.SC_Cand_Name,
                        SC_Father_Name = a.SC_Father_Name,
                        SC_DOB = a.SC_DOB,
                        SC_AnyOtherComments = a.SC_AnyOtherComments,
                        SC_Date_Verify = a.SC_Date_Verify,
                        SC_Mode_Verify = a.SC_Mode_Verify,
                        SC_Mode_VerOther = a.SC_Mode_VerOther,
                        ClientName = a.SC_Cand_Name,
                        SC_Others1 = a.SC_Others1,
                        SC_Others2 = a.SC_Others2,
                        SC_Others3 = a.SC_Others3,
                        SC_Others4 = a.SC_Others4,
                        SC_Others5 = a.SC_Others5,
                        TypeRevert = a.TypeRevert,
                        CheckStatus = a.PQSpecialCheck.CheckStatus,
                        Severity = !string.IsNullOrEmpty(a.Severity) ? db.PQClientSeverities.Where(c => c.ClientRowID == a.PQSpecialCheck.ClientRowID && c.MasterSeverityGrid.SeverityGrid.Replace(" ", "") == a.Severity.Replace(" ", "")).FirstOrDefault().SeverityGridRowId.ToString() : "",
                        ColorName = a.ColorName,
                        ReportComments = a.ReportComments

                    }).FirstOrDefault();
                    #endregion
                }
                else
                {
                    #region MyRegion
                    model = db.PQSpecialChecks.Where(a => a.SpecialCheckRowId == SpecialChecksRowId).Select(a => new PQSpecialCheckVerViewModel
                    {
                        SpecialCheckRowID = a.SpecialCheckRowId,
                        ClientRowID = a.ClientRowID,
                        PersonalRowID = a.PersonalRowID,
                        CheckFamilyRowID = a.CheckFamilyRowID,
                        SC_SecuritasID = a.SC_SecuritasID,
                        SC_Cand_Name = a.SC_Cand_Name,
                        SC_Father_Name = a.SC_Father_Name,
                        SC_DOB = a.SC_DOB,
                        SubCheckRowID = a.SubCheckRowID,
                        SC_Others1 = a.SC_Others1,
                        SC_Others2 = a.SC_Others2,
                        SC_Others3 = a.SC_Others3,
                        SC_Others4 = a.SC_Others4,
                        SC_Others5 = a.SC_Others5,
                        CheckStatus = a.CheckStatus,

                    }).FirstOrDefault();
                    #endregion
                }

                return model;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public PQPersonalDetailsVerViewModel GetPersonalDetailsForTM(int SpecialCheckRowId = 0)
        {
            try
            {
                PQPersonalDetailsVerViewModel model = new PQPersonalDetailsVerViewModel();
                model = db.PQSpecialChecks.Where(a => a.SpecialCheckRowId == SpecialCheckRowId).Select(a => new PQPersonalDetailsVerViewModel
                {
                    ClientRowID = a.ClientRowID,
                    PersonalRowID = a.PersonalRowID,
                    UniqueComponentID = a.UniqueComponentID,
                    //CandidateName = a.SC_Cand_Name,
                    CandidateName = db.PQPersonals.Where(T => T.PersonalRowID == a.PersonalRowID).Select(x => x.Name + " " + x.MiddleName + x.LastName).FirstOrDefault().ToString(),
                    ClientName = a.PQClientMaster.MasterAbbreviation.ClientName,
                    CompanyReferenceNo = a.SC_SecuritasID,
                    SubReferenceID = a.MasterSubCheckFamily.SubCheckName,
                    MobileNo = a.PQPersonal.MobileNo,
                    SpecialInstructions = a.PQClientMaster.SpecialInstructions,
                    CheckStatus = a.CheckStatus
                }).FirstOrDefault();

                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PQSpecialCheckVerViewModel GetSpecialCheckForTM(int SpecialCheckRowId = 0)
        {
            try
            {
                PQSpecialCheckVerViewModel model = new PQSpecialCheckVerViewModel();
                model = db.PQSpecialChecks.Where(a => a.SpecialCheckRowId == SpecialCheckRowId).Select(a => new PQSpecialCheckVerViewModel
                {
                    SpecialCheckRowID = a.SpecialCheckRowId,
                    ClientRowID = a.ClientRowID,
                    PersonalRowID = a.PersonalRowID,
                    CheckFamilyRowID = a.CheckFamilyRowID,
                    SC_Father_Name = a.SC_Father_Name,
                    SC_DOB = a.SC_DOB,
                    SubCheckRowID = a.SubCheckRowID,
                    //CandidateName = a.SC_Cand_Name,
                    CandidateName = db.PQPersonals.Where(T => T.PersonalRowID == a.PersonalRowID).Select(x => x.Name + " " + x.MiddleName + x.LastName).FirstOrDefault().ToString(),
                    ColorName = a.SC_Cand_Name,
                    SC_Others1 = a.SC_Others1,
                    SC_Others2 = a.SC_Others2,
                    SC_Others3 = a.SC_Others3,
                    SC_Others4 = a.SC_Others4,
                    SC_Others5 = a.SC_Others5,
                    CheckStatus = a.CheckStatus,
                }).FirstOrDefault();


                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddSpacelCheckInfoByDataEntryn(PQSpecialCheckVerViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            PQSpecialCheckVer entity = new PQSpecialCheckVer();
                            entity.SpecialCheckRowId = model.SpecialCheckRowID;
                            entity.SC_Cand_Name = model.SC_Cand_Name;
                            entity.SC_Father_Name = model.SC_Father_Name;
                            entity.SC_DOB = model.SC_DOB;
                            entity.SC_Mode_VerOther = model.SC_Mode_VerOther;
                            entity.SC_Mode_Verify = model.SC_Mode_Verify;
                            entity.SC_Date_Verify = model.SC_Date_Verify;
                            entity.SC_AnyOtherComments = model.SC_AnyOtherComments;

                            entity.SC_Others1 = model.SC_Others1;
                            entity.SC_Others2 = model.SC_Others2;
                            entity.SC_Others3 = model.SC_Others3;
                            entity.SC_Others4 = model.SC_Others4;
                            entity.SC_Others5 = model.SC_Others5;

                            entity.TypeRevert = model.TypeRevert;
                            entity.CheckStatus = model.CheckStatus;
                            entity.Severity = model.hdSeverity;
                            entity.ColorName = model.ColorName;
                            entity.ReportComments = model.ReportComments;

                            entity.VerifiedBy = model.VerifiedBy;
                            entity.VerifierDesignation = model.VerifierDesignation;
                            entity.VerifierContactNo = model.VerifierContactNo;
                            entity.VerifierMobileNo = model.VerifierMobileNo;
                            entity.VerifierEmailId = model.VerifierEmailId;
                            entity.VerifiedDate = model.VerifiedDate;
                            entity.CheckStatus = model.CheckStatus;
                            entity.Status = model.Status;

                            context.PQSpecialCheckVers.Add(entity);
                            context.SaveChanges();

                            context.PQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowID).CheckStatus = "WIP-Pending";
                            context.SaveChanges();

                            int statusId = context.PQSpecialCheckStatus.Where(p => p.SpecialCheckRowId == model.SpecialCheckRowID).FirstOrDefault().SpecialCheckStatusRowID;

                            ClientCustomize cc = new ClientCustomize();

                            if (model.CheckStatus == "Review Requested")
                            {
                                context.PQSpecialChecks.Single(p => p.SpecialCheckRowId == model.SpecialCheckRowID).CheckStatus = model.CheckStatus;
                                context.SaveChanges();

                                context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == statusId).TMCheckStatus = model.CheckStatus;
                                context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == statusId).Remarks = model.SC_AnyOtherComments;
                                context.SaveChanges();

                                cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 19, "Check", context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == statusId).TMAllocatedTo);
                            }
                            if (model.CheckStatus == "Completed")
                            {
                                context.PQSpecialChecks.Single(p => p.SpecialCheckRowId == model.SpecialCheckRowID).CheckStatus = "Completed-TM";
                                context.SaveChanges();

                                context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == statusId).TMQCCheckStatus = model.CheckStatus;
                                context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == statusId).TMCheckStatus = model.CheckStatus;
                                context.SaveChanges();
                                cc.UpdateCheckStatus(model.PersonalRowID, model.SubCheckRowID, 1, 24, "Check", context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == statusId).TMAllocatedTo);
                            }
                        }
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

        public void UpdateSpecialCheckInfoByDataEntry(PQSpecialCheckVerViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {

                            //context.PQSpecialCheckVers.Single(c => c.SpecialCheckVerRowID == model.SpecialCheckVerRowID).SC_Cand_Name = model.SC_Cand_Name;
                            context.PQSpecialCheckVers.Single(c => c.SpecialCheckVerRowID == model.SpecialCheckVerRowID).SC_Father_Name = model.SC_Father_Name;
                            context.PQSpecialCheckVers.Single(c => c.SpecialCheckVerRowID == model.SpecialCheckVerRowID).SC_DOB = model.SC_DOB;
                            context.PQSpecialCheckVers.Single(c => c.SpecialCheckVerRowID == model.SpecialCheckVerRowID).SC_Mode_VerOther = model.SC_Mode_VerOther;
                            context.PQSpecialCheckVers.Single(c => c.SpecialCheckVerRowID == model.SpecialCheckVerRowID).SC_Mode_Verify = model.SC_Mode_Verify;
                            context.PQSpecialCheckVers.Single(c => c.SpecialCheckVerRowID == model.SpecialCheckVerRowID).SC_Date_Verify = model.SC_Date_Verify;
                            context.PQSpecialCheckVers.Single(c => c.SpecialCheckVerRowID == model.SpecialCheckVerRowID).SC_AnyOtherComments = model.SC_AnyOtherComments;

                            context.PQSpecialCheckVers.Single(c => c.SpecialCheckVerRowID == model.SpecialCheckVerRowID).SC_Others1 = model.SC_Others1;
                            context.PQSpecialCheckVers.Single(c => c.SpecialCheckVerRowID == model.SpecialCheckVerRowID).SC_Others2 = model.SC_Others2;
                            context.PQSpecialCheckVers.Single(c => c.SpecialCheckVerRowID == model.SpecialCheckVerRowID).SC_Others3 = model.SC_Others3;
                            context.PQSpecialCheckVers.Single(c => c.SpecialCheckVerRowID == model.SpecialCheckVerRowID).SC_Others4 = model.SC_Others4;
                            context.PQSpecialCheckVers.Single(c => c.SpecialCheckVerRowID == model.SpecialCheckVerRowID).SC_Others5 = model.SC_Others5;

                            context.PQSpecialCheckVers.Single(c => c.SpecialCheckVerRowID == model.SpecialCheckVerRowID).TypeRevert = model.TypeRevert;
                            context.PQSpecialCheckVers.Single(c => c.SpecialCheckVerRowID == model.SpecialCheckVerRowID).CheckStatus = model.CheckStatus;
                            context.PQSpecialCheckVers.Single(c => c.SpecialCheckVerRowID == model.SpecialCheckVerRowID).Severity = model.hdSeverity;
                            context.PQSpecialCheckVers.Single(c => c.SpecialCheckVerRowID == model.SpecialCheckVerRowID).ColorName = model.ColorName;
                            context.PQSpecialCheckVers.Single(c => c.SpecialCheckVerRowID == model.SpecialCheckVerRowID).ReportComments = model.ReportComments;

                            context.PQSpecialCheckVers.Single(c => c.SpecialCheckVerRowID == model.SpecialCheckVerRowID).VerifiedBy = model.VerifiedBy;
                            context.PQSpecialCheckVers.Single(c => c.SpecialCheckVerRowID == model.SpecialCheckVerRowID).VerifierDesignation = model.VerifierDesignation;
                            context.PQSpecialCheckVers.Single(c => c.SpecialCheckVerRowID == model.SpecialCheckVerRowID).VerifierContactNo = model.VerifierContactNo;
                            context.PQSpecialCheckVers.Single(c => c.SpecialCheckVerRowID == model.SpecialCheckVerRowID).VerifierMobileNo = model.VerifierMobileNo;
                            context.PQSpecialCheckVers.Single(c => c.SpecialCheckVerRowID == model.SpecialCheckVerRowID).VerifierEmailId = model.VerifierEmailId;
                            context.PQSpecialCheckVers.Single(c => c.SpecialCheckVerRowID == model.SpecialCheckVerRowID).VerifiedDate = model.VerifiedDate;

                            context.SaveChanges();
                            ClientCustomize cc = new ClientCustomize();
                            DataSet dd = new DataSet();
                            dd = cc.getInformationFromDepartmentRowID(model.SpecialCheckRowID, "Special Checks");
                            int personalRowid = 0;
                            int subCheckRowID = 0;
                            int modifiedBy = 0;
                            if (dd.Tables[0].Rows.Count > 0)
                            {
                                personalRowid = int.Parse((Convert.ToString(dd.Tables[0].Rows[0]["PersonalRowID"])));
                                subCheckRowID = int.Parse((Convert.ToString(dd.Tables[0].Rows[0]["SubCheckRowID"])));
                                modifiedBy = int.Parse((Convert.ToString(dd.Tables[0].Rows[0]["modifiedBy"])));

                            }

                            int statusId = context.PQSpecialCheckStatus.Where(p => p.SpecialCheckRowId == model.SpecialCheckRowID).FirstOrDefault().SpecialCheckStatusRowID;

                            if (model.CheckStatus == "Review Requested")
                            {
                                context.PQSpecialChecks.Single(p => p.SpecialCheckRowId == model.SpecialCheckRowID).CheckStatus = model.CheckStatus;
                                context.SaveChanges();

                                context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == statusId).TMCheckStatus = model.CheckStatus;
                                context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == statusId).Remarks = model.SC_AnyOtherComments;
                                context.SaveChanges();
                            }
                            if (model.CheckStatus == "Completed")
                            {
                                cc.UpdateCheckStatus(personalRowid, subCheckRowID, 1, 36, "Check", modifiedBy);
                                context.PQSpecialChecks.Single(p => p.SpecialCheckRowId == model.SpecialCheckRowID).CheckStatus = "Completed-TM";
                                context.SaveChanges();

                                context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == statusId).TMQCCheckStatus = model.CheckStatus;
                                context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == statusId).TMCheckStatus = model.CheckStatus;
                                context.SaveChanges();
                            }
                        }
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

        public UpdatePQSpecialCheckVerViewModel GetSpecialCheckInfoForTMQC(int SpecialCheckRowId = 0,int CheckStatusId=0)
        {
            try
            {
                UpdatePQSpecialCheckVerViewModel model = new UpdatePQSpecialCheckVerViewModel();
                var SpecialCheckVer = db.PQSpecialCheckVers.Where(a => a.SpecialCheckRowId == SpecialCheckRowId).FirstOrDefault();
                int SpecialCheckVerRowId = 0;
                if (SpecialCheckVer != null && SpecialCheckVer.SpecialCheckVerRowID > 0)
                {
                    SpecialCheckVerRowId = SpecialCheckVer.SpecialCheckVerRowID;
                }

                if (SpecialCheckVerRowId > 0)
                {
                    #region Reference
                    model = db.PQSpecialCheckVers.Where(a => a.SpecialCheckVerRowID == SpecialCheckVerRowId).Select(a => new UpdatePQSpecialCheckVerViewModel
                    {
                        SpecialCheckRowID = a.SpecialCheckRowId,
                        SpecialCheckVerRowID = a.SpecialCheckVerRowID,
                        ClientRowID = a.PQSpecialCheck.ClientRowID,
                        PersonalRowID = a.PQSpecialCheck.PersonalRowID,
                        CheckFamilyRowID = a.PQSpecialCheck.CheckFamilyRowID,
                        SubCheckRowID = a.PQSpecialCheck.SubCheckRowID,
                        SC_SecuritasID = a.PQSpecialCheck.SC_SecuritasID,
                        //SC_Cand_Name = a.SC_Cand_Name,
                        SC_Cand_Name = db.PQPersonals.Where(T => T.PersonalRowID == a.PQSpecialCheck.PersonalRowID).Select(x => x.Name + " " + x.MiddleName + x.LastName).FirstOrDefault().ToString(),
                        SC_Father_Name = a.SC_Father_Name,
                        SC_DOB = a.SC_DOB,
                        SC_AnyOtherComments = a.SC_AnyOtherComments,
                        SC_Date_Verify = a.SC_Date_Verify,
                        SC_Mode_Verify = a.SC_Mode_Verify,
                        SC_Mode_VerOther = a.SC_Mode_VerOther,
                        ClientName = a.SC_Cand_Name,
                        SC_Others1 = a.SC_Others1,
                        SC_Others2 = a.SC_Others2,
                        SC_Others3 = a.SC_Others3,
                        SC_Others4 = a.SC_Others4,
                        SC_Others5 = a.SC_Others5,
                        TypeRevert = a.TypeRevert,
                        Severity = a.Severity,
                        ColorName = a.ColorName,
                        ReportComments = a.ReportComments,
                        CheckStatus = a.PQSpecialCheck.CheckStatus,
                        CheckStatusId= CheckStatusId
                    }).FirstOrDefault();
                    #endregion
                }
                else
                {
                    #region Reference
                    model = db.PQSpecialChecks.Where(a => a.SpecialCheckRowId == SpecialCheckRowId).Select(a => new UpdatePQSpecialCheckVerViewModel
                    {
                        SpecialCheckRowID = a.SpecialCheckRowId,
                        ClientRowID = a.ClientRowID,
                        PersonalRowID = a.PersonalRowID,
                        CheckFamilyRowID = a.CheckFamilyRowID,
                        SC_SecuritasID = a.SC_SecuritasID,
                        //SC_Cand_Name = a.SC_Cand_Name,
                        SC_Cand_Name = db.PQPersonals.Where(T => T.PersonalRowID == a.PersonalRowID).Select(x => x.Name + " " + x.MiddleName + x.LastName).FirstOrDefault().ToString(),
                        SC_Father_Name = a.SC_Father_Name,
                        SC_DOB = a.SC_DOB,
                        SubCheckRowID = a.SubCheckRowID,
                        SC_Others1 = a.SC_Others1,
                        SC_Others2 = a.SC_Others2,
                        SC_Others3 = a.SC_Others3,
                        SC_Others4 = a.SC_Others4,
                        SC_Others5 = a.SC_Others5,
                        CheckStatus = a.CheckStatus,
                        CheckStatusId = CheckStatusId
                    }).FirstOrDefault();
                    #endregion
                }

                return model;

            }
            catch (Exception)
            {
                throw;
            }
        }

        public void ViewSpecialCheckInfoByVrifierTL(ViewSpecialCheckViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            context.PQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowId).CheckStatus = model.CheckStatus;
                            context.SaveChanges();

                            int statusId = context.PQSpecialCheckStatus.Where(p => p.SpecialCheckRowId == model.SpecialCheckRowId).FirstOrDefault().SpecialCheckStatusRowID;

                            context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == statusId).TMCheckStatus = model.CheckStatus;
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

        public void UpdateSpecialCheckByTMQC(UpdatePQSpecialCheckVerViewModel model, short RptQCManagerId = 0)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            int statusId = context.PQSpecialCheckStatus.Where(p => p.SpecialCheckRowId == model.SpecialCheckRowID).FirstOrDefault().SpecialCheckStatusRowID;

                            if (model.CheckStatus == "Rejected")
                            {
                                #region Update Check Status

                                string ActualCheckStatus = "WIP-Pending";
                                context.PQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowID).CheckStatus = ActualCheckStatus;
                                context.SaveChanges();

                                context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == statusId).TMCheckStatus = ActualCheckStatus;
                                context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == statusId).TMQCCheckStatus = "";
                                context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == statusId).Remarks = model.SC_AnyOtherComments;
                                context.PQSpecialCheckVers.Single(c => c.SpecialCheckVerRowID == model.SpecialCheckVerRowID).SC_AnyOtherComments = model.ReportComments;
                                context.SaveChanges();

                                #endregion
                            }
                            else
                            {
                                context.PQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowID).CheckStatus = model.CheckStatus;
                                context.SaveChanges();

                                if (model.CheckStatus == "Insufficent")
                                {
                                    context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == statusId).TMQCCheckStatus = "Insufficent-QC";
                                    context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == statusId).InfSuffClearBy = 0;
                                    context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == statusId).InfSuffClearComments = "";
                                    context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == statusId).InfSuffClearDate = null;
                                    context.SaveChanges();

                                    context.PQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowID).CheckStatus = "Insufficent-QC";
                                    context.SaveChanges();

                                    context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).CaseStatus = model.CheckStatus;
                                    context.SaveChanges();
                                }



                                if (model.SpecialCheckVerRowID > 0 && model.CheckStatus != "Insufficent")
                                {
                                    #region Update SpecialCheck Ver Info By TMQC
                                    //context.PQSpecialCheckVers.Single(c => c.SpecialCheckVerRowID == model.SpecialCheckVerRowID).SC_Cand_Name = model.SC_Cand_Name;
                                    context.PQSpecialCheckVers.Single(c => c.SpecialCheckVerRowID == model.SpecialCheckVerRowID).SC_Father_Name = model.SC_Father_Name;
                                    context.PQSpecialCheckVers.Single(c => c.SpecialCheckVerRowID == model.SpecialCheckVerRowID).SC_DOB = model.SC_DOB;
                                    context.PQSpecialCheckVers.Single(c => c.SpecialCheckVerRowID == model.SpecialCheckVerRowID).SC_Mode_VerOther = model.SC_Mode_VerOther;
                                    context.PQSpecialCheckVers.Single(c => c.SpecialCheckVerRowID == model.SpecialCheckVerRowID).SC_Mode_Verify = model.SC_Mode_Verify;
                                    context.PQSpecialCheckVers.Single(c => c.SpecialCheckVerRowID == model.SpecialCheckVerRowID).SC_Date_Verify = model.SC_Date_Verify;
                                    context.PQSpecialCheckVers.Single(c => c.SpecialCheckVerRowID == model.SpecialCheckVerRowID).SC_AnyOtherComments = model.SC_AnyOtherComments;
                                    context.PQSpecialCheckVers.Single(c => c.SpecialCheckVerRowID == model.SpecialCheckVerRowID).SC_AnyOtherComments = model.ReportComments;

                                    context.PQSpecialCheckVers.Single(c => c.SpecialCheckVerRowID == model.SpecialCheckVerRowID).SC_Others1 = model.SC_Others1;
                                    context.PQSpecialCheckVers.Single(c => c.SpecialCheckVerRowID == model.SpecialCheckVerRowID).SC_Others2 = model.SC_Others2;
                                    context.PQSpecialCheckVers.Single(c => c.SpecialCheckVerRowID == model.SpecialCheckVerRowID).SC_Others3 = model.SC_Others3;
                                    context.PQSpecialCheckVers.Single(c => c.SpecialCheckVerRowID == model.SpecialCheckVerRowID).SC_Others4 = model.SC_Others4;
                                    context.PQSpecialCheckVers.Single(c => c.SpecialCheckVerRowID == model.SpecialCheckVerRowID).SC_Others5 = model.SC_Others5;


                                    #endregion

                                    //Update in ReferenceVer Table            
                                    context.PQSpecialCheckVers.Single(c => c.SpecialCheckVerRowID == model.SpecialCheckVerRowID).ReportComments = model.SC_AnyOtherComments; context.PQSpecialCheckVers.Single(c => c.SpecialCheckVerRowID == model.SpecialCheckVerRowID).CheckStatus = model.CheckStatus;
                                    context.SaveChanges();


                                    #region  ***** Completed-TM & Rejected-RWQC*****

                                    if (model.CheckStatus == "Completed-TM" || model.CheckStatus == "Rejected-RWQC")
                                    {
                                        string allocateMgr = "Allocated-Mgr";

                                        context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == statusId).TMQCCheckStatus = "Completed-QC";
                                        context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == statusId).RWQCMgrCheckStatus = allocateMgr;
                                        context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == statusId).RWQCManagerBy = RptQCManagerId;
                                        context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == statusId).RWQCManagerDate = DateTime.Now;
                                        context.SaveChanges();

                                        short TemaMember = context.PQSpecialChecks.Find(model.SpecialCheckRowID).PQPersonal.RWQCManagerTo;
                                        if (TemaMember == 0)
                                        {
                                            context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).RWQCMgrCaseStatus = allocateMgr;
                                            context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).RWQCManagerTo = RptQCManagerId;
                                            context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).RWQCManagerDate = DateTime.Now;
                                            context.SaveChanges();
                                        }

                                        context.PQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowID).CheckStatus = "Allocated-RWQC";
                                        context.SaveChanges();
                                    }
                                    #endregion
                                }
                            }
                            transaction.Commit();
                        }
                        else
                        {
                            throw new Exception("Address could not be blank!");
                        }
                    }
                    catch (Exception)
                    {
                        throw;
                    }
                }
            }
        }

        public UpdateSpecialCheckInsuffClearViewModel GetPQSpecialCheckDetailsById(int SpecialCheckRowID = 0)
        {
            try
            {
                UpdateSpecialCheckInsuffClearViewModel model = new UpdateSpecialCheckInsuffClearViewModel();
                var entity = db.PQSpecialChecks.Find(SpecialCheckRowID);
                if (entity != null)
                {
                    model.SpecialCheckRowID = entity.SpecialCheckRowId;
                    model.ClientRowID = entity.ClientRowID;
                    model.PersonalRowID = entity.PersonalRowID;
                    model.SubCheckRowID = entity.SubCheckRowID;
                    model.UniqueComponentID = entity.UniqueComponentID;
                    model.CheckStatus = entity.CheckStatus;
                    model.ReWorkCheckStatus = entity.ReWorkCheckStatus;
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

        public void UpdateSpecialCheckInfoByTeamMember(PQUpdateSpecialCheckTeamMemberViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            context.PQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowID).CheckStatus = model.CheckStatus;
                            context.PQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowID).ModifiedBy = model.ModifiedBy;
                            context.PQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowID).ModifiedDate = model.ModifiedDate;
                            context.SaveChanges();

                            ClientCustomize cc = new ClientCustomize();
                            DataSet dd = new DataSet();
                            dd = cc.getInformationFromDepartmentRowID(model.SpecialCheckRowID, "Special Checks");
                            int personalRowid = 0;
                            int subCheckRowID = 0;
                            if (dd.Tables[0].Rows.Count > 0)
                            {
                                personalRowid = int.Parse((Convert.ToString(dd.Tables[0].Rows[0]["PersonalRowID"])));
                                subCheckRowID = int.Parse((Convert.ToString(dd.Tables[0].Rows[0]["SubCheckRowID"])));
                            }

                            #region Save Check Status

                            var statusId = context.PQSpecialCheckStatus.Where(p => p.SpecialCheckRowId == model.SpecialCheckRowID).FirstOrDefault().SpecialCheckStatusRowID;

                            context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == statusId).TMCheckStatus = model.CheckStatus;
                            context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == statusId).TMQCCheckStatus = "";
                            if (model.CheckStatus == "PV Raised")
                            {
                                context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == statusId).PVMgrCheckStatus = "Allocate-Mgr";
                                context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == statusId).PVMgrAllocatedTo = model.PVMgrAllocatedTo;
                                context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == statusId).PVMgrAllocatedDate = model.ModifiedDate;
                            }
                            if (model.CheckStatus == "Insufficent")
                            {
                                cc.UpdateCheckStatus(personalRowid, subCheckRowID, 1, 22, "Check", model.ModifiedBy);
                                context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == statusId).TMQCCheckStatus = model.CheckStatus;
                                context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == statusId).InfSuffRaiseRemarks = model.ActionRemark;
                                context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == statusId).InfSuffRaiseBy = model.ModifiedBy;
                                context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == statusId).InfSuffRaisedDate = model.ModifiedDate;
                            }
                            if (model.CheckStatus == "Rejected")
                            {
                                cc.UpdateCheckStatus(personalRowid, subCheckRowID, 1, 20, "Check", model.ModifiedBy);
                                context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == statusId).Remarks = model.ActionRemark;
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

        public void UpdateSpecialCheckInfoByTeamLeaderReAllocated(PQUpdateSpecialCheckTeamLeaderViewModel model)
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

                            int statusId = context.PQSpecialCheckStatus.Where(p => p.SpecialCheckRowId == model.SpecialCheckRowID).FirstOrDefault().SpecialCheckStatusRowID;
                            if (model.TMAllocatedTo > 0)
                            {
                                context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == statusId).TMAllocatedTo = model.TMAllocatedTo;
                                context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == statusId).TMAllocatedDate = model.ModifiedDate;
                            }
                            if (model.TMQCAllocatedTo > 0)
                            {
                                context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == statusId).TMQCAllocatedTo = model.TMQCAllocatedTo;
                                context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == statusId).TMQCAllocatedDate = model.ModifiedDate;
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
