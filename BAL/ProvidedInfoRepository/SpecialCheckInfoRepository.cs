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
    public class SpecialCheckInfoRepository : ISpecialCheckInfoRepository
    {
        DataContext db;
        ClientCustomize cc;

        public SpecialCheckInfoRepository()
        {
            db = new DataContext();
            cc = new ClientCustomize();
        }

        public int GetAllSpacialByClientId(short ClientRowID = 0)
        {
            try
            {
                var data = db.PQSpecialChecks.Where(p => p.ClientRowID == ClientRowID).ToList();
                return data.Count();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetSpecialCheckIDByUniqueComponentID(int PersonalRowID = 0, string UniqueComponentID = "")
        {
            try
            {
                return db.PQSpecialChecks.Where(p => p.PersonalRowID == PersonalRowID && p.UniqueComponentID == UniqueComponentID).FirstOrDefault().SpecialCheckRowId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool IfExistSpecialCheckForDELeftMenu(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0)
        {
            try
            {
                var listChecks = db.PQSpecialChecks.Where(a => a.ClientRowID == ClientRowId && a.PersonalRowID == PersonalRowID
                     && a.CheckFamilyRowID == ChFamilRowId && a.SubCheckRowID == subCheckId).FirstOrDefault();
                if (listChecks != null)
                {
                    var ChkStatus = db.PQSpecialCheckStatus.Where(s => s.SpecialCheckRowId == listChecks.SpecialCheckRowId).FirstOrDefault();

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

        public bool IfExistSpecialCheckForDEQCLeftMenu(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0, int CheckStatusid = 0)
        {
            try
            {
                //var listChecks = db.PQSpecialChecks.Where(a => a.ClientRowID == ClientRowId && a.PersonalRowID == PersonalRowID
                //     && a.CheckFamilyRowID == ChFamilRowId && a.SubCheckRowID == subCheckId).FirstOrDefault();
                //if (listChecks != null)
                //{
                //    var ChkStatus = db.PQSpecialCheckStatus.Where(s => s.SpecialCheckRowId == listChecks.SpecialCheckRowId).FirstOrDefault();

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

        public AddSpecialCheckViewModel GetSpecialCheckInfoForDataEntry(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0)
        {
            try
            {
                return db.PQSpecialChecks.Where(a => a.ClientRowID == ClientRowId && a.PersonalRowID == PersonalRowID
                && a.CheckFamilyRowID == ChFamilRowId && a.SubCheckRowID == subCheckId).Select(a => new AddSpecialCheckViewModel
                {
                    SpecialCheckRowId = a.SpecialCheckRowId,
                    ClientRowID = a.ClientRowID,
                    PersonalRowID = a.PersonalRowID,
                    ClientPackageRowID = a.ClientPackageRowID,
                    //SC_Cand_Name = a.SC_Cand_Name,
                    SC_Cand_Name = db.PQPersonals.Where(T => T.PersonalRowID == a.PersonalRowID).Select(x => x.Name + " " + x.MiddleName + x.LastName).FirstOrDefault().ToString(),
                    CheckFamilyRowID = a.CheckFamilyRowID,
                    SubCheckRowID = a.SubCheckRowID,
                    UniqueComponentID = a.UniqueComponentID,
                    SC_Father_Name = a.PQPersonal.FatherName + "" + a.PQPersonal.FatherMiddleName + "" + a.PQPersonal.FatherLastName,
                    SC_SecuritasID = a.SC_SecuritasID,
                    SC_DOB = a.SC_DOB != null  ? a.SC_DOB : a.PQPersonal.DOB, 
                    SC_Others1 = a.SC_Others1,
                    SC_Others2 = a.SC_Others2,
                    SC_Others3 = a.SC_Others3,
                    SC_Others4 = a.SC_Others4,
                    SC_Others5 = a.SC_Others5,
                    CheckStatus = a.CheckStatus,
                    ReWorkCheckStatus = a.ReWorkCheckStatus,
                }).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddSpacelCheckInformation(AddSpecialCheckViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            PQSpecialCheck entity = new PQSpecialCheck();
                            entity.ClientRowID = model.ClientRowID;
                            entity.PersonalRowID = model.PersonalRowID;
                            entity.ClientPackageRowID = model.ClientPackageRowID;
                            entity.CheckFamilyRowID = model.CheckFamilyRowID;
                            entity.SubCheckRowID = model.SubCheckRowID;
                            entity.UniqueComponentID = model.UniqueComponentID;
                            entity.SC_Cand_Name = model.SC_Cand_Name;
                            entity.SC_SecuritasID = model.SC_SecuritasID;
                            entity.SC_Father_Name = model.SC_Father_Name;
                            entity.SC_DOB = model.SC_DOB;
                            entity.SC_Others1 = model.SC_Others1;
                            entity.SC_Others2 = model.SC_Others2;
                            entity.SC_Others3 = model.SC_Others3;
                            entity.SC_Others4 = model.SC_Others4;
                            entity.SC_Others5 = model.SC_Others5;

                            entity.CreatedBy = model.CreatedBy;
                            entity.CreatedDate = model.CreatedDate;
                            entity.OutDate = model.OutDate;
                            entity.InternalOutDate = model.InternalOutDate;

                            entity.CheckStatus = model.CheckStatus;
                            if (model.CheckStatus == "Completed")
                            {
                                entity.CheckStatus = "Completed-DE";
                            }

                            entity.ReWorkCheckStatus = model.ReWorkCheckStatus;
                            entity.Remarks = model.Remarks;
                            entity.Status = model.Status;

                            context.PQSpecialChecks.Add(entity);
                            context.SaveChanges();

                            #region Save Check Status

                            PQSpecialCheckStatus chkEntity = new PQSpecialCheckStatus();
                            var ScModel = context.PQSpecialChecks.Include("PQPersonal").Where(p => p.SpecialCheckRowId == entity.SpecialCheckRowId).FirstOrDefault();
                            chkEntity.SpecialCheckRowId = ScModel.SpecialCheckRowId;
                            chkEntity.DETLAllocatedTo = ScModel.PQPersonal.AllocatedBy;
                            chkEntity.DETMCheckStatus = model.CheckStatus;
                            chkEntity.DETMAllocatedTo = ScModel.PQPersonal.AllocatedToDE;
                            chkEntity.DETMAllocatedDate = ScModel.PQPersonal.AllocatedToDEDate;
                            chkEntity.DETMQCAllocatedTo = ScModel.PQPersonal.AllocatedToDEQC;
                            chkEntity.DETMQCAllocatedDate = ScModel.PQPersonal.AllocatedToDEQCDate;

                            if (model.CheckStatus == "Completed")
                            {
                                chkEntity.DETMQCCheckStatus = model.CheckStatus;
                            }
                            if (model.CheckStatus == "Insufficent")
                            {
                                chkEntity.DETMQCCheckStatus = model.CheckStatus;
                                chkEntity.InfSuffRaiseBy = ScModel.PQPersonal.AllocatedToDE;
                                chkEntity.InfSuffRaisedDate = ScModel.CreatedDate;
                                chkEntity.InfSuffRaiseRemarks = model.Remarks;
                            }
                            if (model.CheckStatus == "Rejected" || model.CheckStatus == "Review Requested")
                            {
                                chkEntity.Remarks = model.Remarks;
                            }
                            context.PQSpecialCheckStatus.Add(chkEntity);
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

        public void UpdateSpecialCheckInfoByDataEntry(AddSpecialCheckViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            //context.PQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowId).SC_Cand_Name = model.SC_Cand_Name;
                            context.PQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowId).SC_Father_Name = model.SC_Father_Name;
                            context.PQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowId).SC_DOB = model.SC_DOB;
                            context.PQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowId).SC_Others1 = model.SC_Others1;
                            context.PQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowId).SC_Others2 = model.SC_Others2;
                            context.PQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowId).SC_Others3 = model.SC_Others3;
                            context.PQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowId).SC_Others4 = model.SC_Others4;
                            context.PQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowId).SC_Others5 = model.SC_Others5;

                            context.PQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowId).CheckStatus = model.CheckStatus;
                            if (model.CheckStatus == "Completed")
                            {
                                context.PQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowId).CheckStatus = "Completed-DE";
                            }
                           else if (model.CheckStatus == "Not Applicable")
                            {
                                context.PQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowId).CheckStatus = "Not Applicable";
                            }

                            context.PQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowId).ModifiedBy = model.CreatedBy;
                            context.PQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowId).ModifiedDate = model.CreatedDate;
                            context.SaveChanges();

                            #region Save Check Status

                            var status = context.PQSpecialCheckStatus.Where(p => p.SpecialCheckRowId == model.SpecialCheckRowId).FirstOrDefault();

                            if (status.TMCheckStatus == "Insufficent" && status.TMQCCheckStatus == "Insufficent-QC" && model.CheckStatus == "Completed")
                            {
                                context.PQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowId).CheckStatus = "WIP-Pending";
                                context.SaveChanges();

                                context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == status.SpecialCheckStatusRowID).DETMCheckStatus = model.CheckStatus;
                                context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == status.SpecialCheckStatusRowID).DETMAllocatedDate = model.CreatedDate;
                                context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == status.SpecialCheckStatusRowID).TMCheckStatus = "WIP-Pending";
                                context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == status.SpecialCheckStatusRowID).TMAllocatedDate = model.CreatedDate;
                                context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == status.SpecialCheckStatusRowID).TMQCCheckStatus = "";
                                context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == status.SpecialCheckStatusRowID).TMQCAllocatedDate = model.CreatedDate;
                            }
                            else if ( model.CheckStatus == "Not Applicable")
                            {
                                context.PQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowId).CheckStatus = "Not Applicable";
                                context.SaveChanges();
                            }
                            else
                            {
                                context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == status.SpecialCheckStatusRowID).DETMCheckStatus = model.CheckStatus;
                                context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == status.SpecialCheckStatusRowID).DETMAllocatedDate = model.CreatedDate;

                                if (model.CheckStatus == "Completed")
                                {
                                    context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == status.SpecialCheckStatusRowID).DETMQCCheckStatus = model.CheckStatus;
                                    context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == status.SpecialCheckStatusRowID).DETMQCAllocatedDate = model.CreatedDate;
                                }
                                if (model.CheckStatus == "Insufficent")
                                {
                                    context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == status.SpecialCheckStatusRowID).DETMQCCheckStatus = model.CheckStatus;
                                    context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == status.SpecialCheckStatusRowID).DETMQCAllocatedDate = model.CreatedDate;
                                    context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == status.SpecialCheckStatusRowID).InfSuffRaiseBy = model.CreatedBy;
                                    context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == status.SpecialCheckStatusRowID).InfSuffRaisedDate = model.CreatedDate;
                                    context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == status.SpecialCheckStatusRowID).InfSuffRaiseRemarks = model.Remarks;
                                }
                                if (model.CheckStatus == "Rejected" || model.CheckStatus == "Review Requested")
                                {
                                    context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == status.SpecialCheckStatusRowID).Remarks = model.Remarks;
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

        public void RejectionUpdateSpecialCheckInfoByQC(UpdateSpecialCheckRejectionViewModel model)
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
                            context.PQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowId).CheckStatus = ActualCheckStatus;
                            context.PQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowId).ModifiedBy = model.ModifiedBy;
                            context.PQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowId).ModifiedDate = model.ModifiedDate;
                            context.SaveChanges();

                            #region Update Check Status

                            int statusId = context.PQSpecialCheckStatus.Where(p => p.SpecialCheckRowId == model.SpecialCheckRowId).FirstOrDefault().SpecialCheckStatusRowID;
                            context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == statusId).DETMCheckStatus = ActualCheckStatus;
                            context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == statusId).DETMQCCheckStatus = "";
                            context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == statusId).Remarks = model.Remarks;
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

        public UpdateSpecialCheckViewModel GetSpecialCheckInfoForDEQC(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0)
        {
            try
            {
                return db.PQSpecialChecks.Where(a => a.ClientRowID == ClientRowId && a.PersonalRowID == PersonalRowID
                && a.CheckFamilyRowID == ChFamilRowId && a.SubCheckRowID == subCheckId).Select(a => new UpdateSpecialCheckViewModel
                {
                    SpecialCheckRowId = a.SpecialCheckRowId,
                    ClientRowID = a.ClientRowID,
                    PersonalRowID = a.PersonalRowID,
                    ClientPackageRowID = a.ClientPackageRowID,
                    SC_Cand_Name = a.SC_Cand_Name,
                    CheckFamilyRowID = a.CheckFamilyRowID,
                    SubCheckRowID = a.SubCheckRowID,
                    UniqueComponentID = a.UniqueComponentID,
                    SC_Father_Name = a.PQPersonal.FatherName + "" + a.PQPersonal.FatherMiddleName + "" + a.PQPersonal.FatherLastName,
                    SC_SecuritasID = a.SC_SecuritasID,
                    SC_DOB = a.SC_DOB != null ? a.SC_DOB : a.PQPersonal.DOB,
                    SC_Others1 = a.SC_Others1,
                    SC_Others2 = a.SC_Others2,
                    SC_Others3 = a.SC_Others3,
                    SC_Others4 = a.SC_Others4,
                    SC_Others5 = a.SC_Others5,
                    CheckStatus = a.CheckStatus,
                    ReWorkCheckStatus = a.ReWorkCheckStatus,
                }).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateSpecialCheckInfoByQualityCheck(UpdateSpecialCheckViewModel model)
        {

            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            //context.PQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowId).SC_Cand_Name = model.SC_Cand_Name;
                            context.PQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowId).SC_Father_Name = model.SC_Father_Name;
                            context.PQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowId).SC_DOB = model.SC_DOB;
                            context.PQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowId).SC_Others1 = model.SC_Others1;
                            context.PQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowId).SC_Others2 = model.SC_Others2;
                            context.PQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowId).SC_Others3 = model.SC_Others3;
                            context.PQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowId).SC_Others4 = model.SC_Others4;
                            context.PQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowId).SC_Others5 = model.SC_Others5;

                            context.PQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowId).CheckStatus = model.CheckStatus;
                            context.PQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowId).ModifiedBy = model.ModifiedBy;
                            context.PQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowId).ModifiedDate = model.ModifiedDate;
                            context.PQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowId).CheckStatus = model.CheckStatus;
                            if (model.CheckStatus == "Completed-DE")
                            {
                                context.PQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowId).CheckStatus = "Allocated-Mgr";
                            }

                            context.SaveChanges();

                            #region Save Check Status

                            var CurrentStatus = context.PQSpecialCheckStatus.Where(p => p.SpecialCheckRowId == model.SpecialCheckRowId).FirstOrDefault();

                            if (model.CheckStatus == "Completed-DE")
                            {
                                if (CurrentStatus.TMCheckStatus == "Rejected")
                                {
                                    context.PQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowId).CheckStatus = "WIP-Pending";
                                    context.SaveChanges();

                                    context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == CurrentStatus.SpecialCheckStatusRowID).DETMQCCheckStatus = "Completed-QC";
                                    context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == CurrentStatus.SpecialCheckStatusRowID).TMCheckStatus = "WIP-Pending";
                                    context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == CurrentStatus.SpecialCheckStatusRowID).TMAllocatedDate = model.ModifiedDate;
                                }
                                else
                                {
                                    context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == CurrentStatus.SpecialCheckStatusRowID).DETMQCCheckStatus = "Completed-QC";
                                    context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == CurrentStatus.SpecialCheckStatusRowID).MgrCheckStatus = "Allocated-Mgr";
                                    context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == CurrentStatus.SpecialCheckStatusRowID).MgrAllocatedTo = model.MgrAllocatedTo;
                                    context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == CurrentStatus.SpecialCheckStatusRowID).MgrAllocatedDate = model.ModifiedDate;
                                }

                            }

                            if (model.CheckStatus == "Insufficent")
                            {
                                context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == CurrentStatus.SpecialCheckStatusRowID).DETMQCCheckStatus = "Insufficent-QC";
                                context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == CurrentStatus.SpecialCheckStatusRowID).InfSuffClearBy = 0;
                                context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == CurrentStatus.SpecialCheckStatusRowID).InfSuffClearComments = "";
                                context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == CurrentStatus.SpecialCheckStatusRowID).InfSuffClearDate = null;
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

        public PQSpecialCheckInsuffListPagedModel GetAllSpecialCheckInsuffListForDETL(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", int PersonalId = 0, string spocEmail = "", short TeamMemberRowid = 0, int ClientUserRowID = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                IQueryable<PQSpecialCheckStatus> data = db.PQSpecialCheckStatus.Include("PQSpecialCheck").Where(p => (p.DETMQCCheckStatus == "Insufficent-QC" || p.TMQCCheckStatus == "Insufficent-QC") && p.InfSuffClearBy == 0);

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
                    data = data.Where(b => b.PQSpecialCheck.PQPersonal.ClientUserRowID == ClientUserRowID);
                }

                if (PersonalId == 0)
                {
                    if (ClientRowID > 0)
                    {
                        data = data.Where(b => b.PQSpecialCheck.ClientRowID == ClientRowID);
                    }

                    if (!string.IsNullOrEmpty(spocEmail))
                    {
                        var Clients = db.PQClientMasters.Where(s => s.SpocEmailID.Contains(spocEmail)).Select(s => s.ClientRowID).ToList();
                        data = data.Where(a => Clients.Contains(a.PQSpecialCheck.ClientRowID));
                    }

                    if (!string.IsNullOrEmpty(RecievingToDate) && !string.IsNullOrEmpty(RecievingFromDate))
                    {
                        DateTime startDate = Convert.ToDateTime(RecievingFromDate);
                        DateTime EndDate = Convert.ToDateTime(RecievingToDate);

                        data = data.Where(b => DbFunctions.TruncateTime(b.PQSpecialCheck.PQPersonal.OrderDate) >= startDate.Date && DbFunctions.TruncateTime(b.PQSpecialCheck.PQPersonal.OrderDate) <= EndDate.Date);
                    }

                    if (!string.IsNullOrEmpty(CompletedToDate) && !string.IsNullOrEmpty(CompletedFromDate))
                    {
                        DateTime startDate = Convert.ToDateTime(CompletedFromDate);
                        DateTime EndDate = Convert.ToDateTime(CompletedToDate);

                        data = data.Where(b => DbFunctions.TruncateTime(b.PQSpecialCheck.PQPersonal.DeliveryDate) >= startDate.Date && DbFunctions.TruncateTime(b.PQSpecialCheck.PQPersonal.DeliveryDate) <= EndDate.Date);
                    }

                    if (!string.IsNullOrEmpty(Search))
                    {
                        data = data.Where(b => (b.PQSpecialCheck.PQPersonal.Name + " " + b.PQSpecialCheck.PQPersonal.MiddleName + " " + b.PQSpecialCheck.PQPersonal.LastName).ToString().Replace(" ", "").Contains(Search.Replace(" ", "")) || b.PQSpecialCheck.PQPersonal.ClientRefID == Search);
                    }
                }
                else
                {
                    data = data.Where(b => b.PQSpecialCheck.PersonalRowID == PersonalId);
                }

                switch (sort)
                {
                    default:
                        data = sortDir == "asc" ? data.OrderByDescending(d => d.SpecialCheckStatusRowID) : data.OrderBy(d => d.SpecialCheckStatusRowID);
                        break;
                }

                PQSpecialCheckInsuffListPagedModel model = new PQSpecialCheckInsuffListPagedModel();
                model.PageSize = pageSize;
                model.TotalRecords = data.Count();
                model.PQAllInsuffSpecialCheckes = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new PQSpecialCheckInsuffViewModel
                {
                    SpecialCheckRowId = item.SpecialCheckRowId,
                    ClientRowID = item.PQSpecialCheck.ClientRowID,
                    ClientName = item.PQSpecialCheck.PQClientMaster.MasterAbbreviation.ClientName + " " + item.PQSpecialCheck.PQClientMaster.MasterClientSubGroup.ClientSubGroupName,
                    PersonalRowID = item.PQSpecialCheck.PersonalRowID,
                    CandidateName = item.PQSpecialCheck.PQPersonal.CandTitle + " " + item.PQSpecialCheck.PQPersonal.Name + " " + item.PQSpecialCheck.PQPersonal.MiddleName + " " + item.PQSpecialCheck.PQPersonal.LastName,
                    CheckFamilyRowID = item.PQSpecialCheck.CheckFamilyRowID,
                    SubCheckRowID = item.PQSpecialCheck.SubCheckRowID,
                    SubCheckName = item.PQSpecialCheck.MasterSubCheckFamily.SubCheckName,
                    CompanyRefNo = item.PQSpecialCheck.SC_SecuritasID,
                    InfSuffRaiseBy = db.TeamMembers.Where(t => t.TeamMemberRowID == item.InfSuffRaiseBy).Select(t => t.TMFirstName + " " + t.TMLastName).FirstOrDefault(),
                    InfSuffRaisedDate = item.InfSuffRaisedDate,
                    INFRemarks = item.InfSuffRaiseRemarks,
                    CreatedDate = item.PQSpecialCheck.CreatedDate,
                    CheckStatus = item.PQSpecialCheck.CheckStatus,
                    ReWorkCheckStatus = item.PQSpecialCheck.ReWorkCheckStatus
                }).ToList();
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PQSpecialCheckInsuffListPagedModel GetAllSpecialCheckInsuffListForDETL1(int pageNo, int pageSize, string sort, string sortDir,  string subcheckType, string Search = "", short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", int PersonalId = 0, string spocEmail = "", short TeamMemberRowid = 0, int ClientUserRowID = 0,int allclints=0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataSet ds = new DataSet();
                PQSpecialCheckInsuffListPagedModel model = new PQSpecialCheckInsuffListPagedModel();
                ds = cc.getCheckwiseinsuffDetailSummary(ClientRowID, ClientUserRowID, subcheckType,Search,allclints);
                List<PQSpecialCheckInsuffViewModel> lmd = new List<PQSpecialCheckInsuffViewModel>();
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>
                {
                    lmd.Add(new PQSpecialCheckInsuffViewModel
                    {
                        PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]),
                        SpecialCheckRowId = Convert.ToInt32(dr["SpecialCheckRowId"]),
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
                model.PQAllInsuffSpecialCheckes = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);

                //IQueryable<PQSpecialCheckStatus> data = db.PQSpecialCheckStatus.Include("PQSpecialCheck").Where(p => (p.DETMQCCheckStatus == "Insufficent-QC" || p.TMQCCheckStatus == "Insufficent-QC") && p.InfSuffClearBy == 0);

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
                //    data = data.Where(b => b.PQSpecialCheck.PQPersonal.ClientUserRowID == ClientUserRowID);
                //}

                //if (PersonalId == 0)
                //{
                //    if (ClientRowID > 0)
                //    {
                //        data = data.Where(b => b.PQSpecialCheck.ClientRowID == ClientRowID);
                //    }

                //    if (!string.IsNullOrEmpty(spocEmail))
                //    {
                //        var Clients = db.PQClientMasters.Where(s => s.SpocEmailID.Contains(spocEmail)).Select(s => s.ClientRowID).ToList();
                //        data = data.Where(a => Clients.Contains(a.PQSpecialCheck.ClientRowID));
                //    }

                //    if (!string.IsNullOrEmpty(RecievingToDate) && !string.IsNullOrEmpty(RecievingFromDate))
                //    {
                //        DateTime startDate = Convert.ToDateTime(RecievingFromDate);
                //        DateTime EndDate = Convert.ToDateTime(RecievingToDate);

                //        data = data.Where(b => DbFunctions.TruncateTime(b.PQSpecialCheck.PQPersonal.OrderDate) >= startDate.Date && DbFunctions.TruncateTime(b.PQSpecialCheck.PQPersonal.OrderDate) <= EndDate.Date);
                //    }

                //    if (!string.IsNullOrEmpty(CompletedToDate) && !string.IsNullOrEmpty(CompletedFromDate))
                //    {
                //        DateTime startDate = Convert.ToDateTime(CompletedFromDate);
                //        DateTime EndDate = Convert.ToDateTime(CompletedToDate);

                //        data = data.Where(b => DbFunctions.TruncateTime(b.PQSpecialCheck.PQPersonal.DeliveryDate) >= startDate.Date && DbFunctions.TruncateTime(b.PQSpecialCheck.PQPersonal.DeliveryDate) <= EndDate.Date);
                //    }

                //    if (!string.IsNullOrEmpty(Search))
                //    {
                //        data = data.Where(b => (b.PQSpecialCheck.PQPersonal.Name + " " + b.PQSpecialCheck.PQPersonal.MiddleName + " " + b.PQSpecialCheck.PQPersonal.LastName).ToString().Replace(" ", "").Contains(Search.Replace(" ", "")) || b.PQSpecialCheck.PQPersonal.ClientRefID == Search);
                //    }
                //}
                //else
                //{
                //    data = data.Where(b => b.PQSpecialCheck.PersonalRowID == PersonalId);
                //}

                //switch (sort)
                //{
                //    default:
                //        data = sortDir == "asc" ? data.OrderByDescending(d => d.SpecialCheckStatusRowID) : data.OrderBy(d => d.SpecialCheckStatusRowID);
                //        break;
                //}

                //PQSpecialCheckInsuffListPagedModel model = new PQSpecialCheckInsuffListPagedModel();
                //model.PageSize = pageSize;
                //model.TotalRecords = data.Count();
                //model.PQAllInsuffSpecialCheckes = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new PQSpecialCheckInsuffViewModel
                //{
                //    SpecialCheckRowId = item.SpecialCheckRowId,
                //    ClientRowID = item.PQSpecialCheck.ClientRowID,
                //    ClientName = item.PQSpecialCheck.PQClientMaster.MasterAbbreviation.ClientName + " " + item.PQSpecialCheck.PQClientMaster.MasterClientSubGroup.ClientSubGroupName,
                //    PersonalRowID = item.PQSpecialCheck.PersonalRowID,
                //    CandidateName = item.PQSpecialCheck.PQPersonal.CandTitle + " " + item.PQSpecialCheck.PQPersonal.Name + " " + item.PQSpecialCheck.PQPersonal.MiddleName + " " + item.PQSpecialCheck.PQPersonal.LastName,
                //    CheckFamilyRowID = item.PQSpecialCheck.CheckFamilyRowID,
                //    SubCheckRowID = item.PQSpecialCheck.SubCheckRowID,
                //    SubCheckName = item.PQSpecialCheck.MasterSubCheckFamily.SubCheckName,
                //    CompanyRefNo = item.PQSpecialCheck.SC_SecuritasID,
                //    InfSuffRaiseBy = db.TeamMembers.Where(t => t.TeamMemberRowID == item.InfSuffRaiseBy).Select(t => t.TMFirstName + " " + t.TMLastName).FirstOrDefault(),
                //    InfSuffRaisedDate = item.InfSuffRaisedDate,
                //    INFRemarks = item.InfSuffRaiseRemarks,
                //    CreatedDate = item.PQSpecialCheck.CreatedDate,
                //    CheckStatus = item.PQSpecialCheck.CheckStatus,
                //    ReWorkCheckStatus = item.PQSpecialCheck.ReWorkCheckStatus
                //}).ToList();
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PQSpecialCheckInsuffListPagedModel GetAllSpecialCheckInsuffListForCSSpoc(int pageNo, int pageSize, string sort, string sortDir, string subcheckType, string Search = "", short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", int PersonalId = 0, string spocEmail = "", short TeamMemberRowid = 0, int ClientUserRowID = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataSet ds = new DataSet();
                PQSpecialCheckInsuffListPagedModel model = new PQSpecialCheckInsuffListPagedModel();
                ds = cc.getinsuffDetails_CSSpoc(subcheckType, spocEmail, ClientRowID,Search);
                List<PQSpecialCheckInsuffViewModel> lmd = new List<PQSpecialCheckInsuffViewModel>();
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        PQSpecialCheckInsuffViewModel nn = new PQSpecialCheckInsuffViewModel();
                        //lmd.Add(new PQEducationInsuffViewModel
                        //{
                        nn.PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]);
                        nn.SpecialCheckRowId = Convert.ToInt32(dr["VerificationRowId"]);
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
                model.PQAllInsuffSpecialCheckes = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);               
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }



        public UpdatePQSpecialCheckInsuffClearViewModel GetPQSpecialCheckDetailsById(int SpecialCheckRowID = 0)
        {
            try
            {
                UpdatePQSpecialCheckInsuffClearViewModel model = new UpdatePQSpecialCheckInsuffClearViewModel();
                //var entity = db.PQSpecialChecks.Find(SpecialCheckRowID);

                var entity = db.PQSpecialCheckStatus.Where(s => s.SpecialCheckRowId == SpecialCheckRowID).FirstOrDefault();
                if (entity != null)
                {
                    model.SpecialCheckRowId = entity.SpecialCheckRowId;
                    model.ClientRowID = entity.PQSpecialCheck.ClientRowID;
                    model.PersonalRowID = entity.PQSpecialCheck.PersonalRowID;
                    model.SubCheckRowID = entity.PQSpecialCheck.SubCheckRowID;
                    model.UniqueComponentID = entity.PQSpecialCheck.UniqueComponentID;
                    model.INFRemarks = entity.InfSuffClearComments;
                    model.CheckStatus = entity.PQSpecialCheck.CheckStatus;
                    model.ReWorkCheckStatus = entity.PQSpecialCheck.ReWorkCheckStatus;
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

        public void UpdatePQSpecialChackInsuffClear(UpdatePQSpecialCheckInsuffClearViewModel model)
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

                            var SpecialChackStatusId = context.PQSpecialCheckStatus.Where(c => c.SpecialCheckRowId == model.SpecialCheckRowId).FirstOrDefault().SpecialCheckStatusRowID;

                            context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == SpecialChackStatusId).DETMCheckStatus = ScnCheckStatus;
                            context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == SpecialChackStatusId).InfSuffClearBy = model.InfSuffClearBy;
                            context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == SpecialChackStatusId).InfSuffClearDate = model.InfSuffClearDate;
                            context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == SpecialChackStatusId).InfSuffClearComments = model.INFRemarks;
                            context.SaveChanges();

                            //Check Status Update
                            context.PQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowId).CheckStatus = ScnCheckStatus;
                            context.PQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowId).ModifiedBy = model.ModifiedBy;
                            context.PQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowId).ModifiedDate = model.ModifiedDate;
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

        public UpdateSpecialCheckViewModel GetSpecialCheckInfoForQualityCheck(int SpecialCheckRowId = 0)
        {
            try
            {
                return db.PQSpecialChecks.Where(a => a.SpecialCheckRowId == SpecialCheckRowId).Select(a => new UpdateSpecialCheckViewModel
                {
                    SpecialCheckRowId = a.SpecialCheckRowId,
                    ClientRowID = a.ClientRowID,
                    PersonalRowID = a.PersonalRowID,
                    ClientPackageRowID = a.ClientPackageRowID,
                    CheckFamilyRowID = a.CheckFamilyRowID,
                    SubCheckRowID = a.SubCheckRowID,
                    UniqueComponentID = a.UniqueComponentID,
                    SC_Cand_Name = a.SC_Cand_Name,
                    SC_Father_Name = a.SC_Father_Name,
                    SC_DOB = a.SC_DOB,
                    SC_Others2 = a.SC_Others2,
                    SC_Others1 = a.SC_Others1,
                    SC_Others3 = a.SC_Others3,
                    SC_Others4 = a.SC_Others4,
                    SC_Others5 = a.SC_Others5,
                    CheckStatus = a.CheckStatus,
                    ReWorkCheckStatus = a.ReWorkCheckStatus,
                }).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void ViewSpecialCheckInfoDETL(UpdateSpecialCheckViewModel model)
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

                                context.PQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowId).CheckStatus = model.CheckStatus;
                                context.SaveChanges();

                                context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).DECaseStatus = "WIP";
                                context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).AllocatedToDEDate = DateTime.Now;
                                context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).DEQCCaseStatus = "WIP";
                                context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).AllocatedToDEQCDate = DateTime.Now;
                                context.SaveChanges();

                                int statusId = context.PQSpecialCheckStatus.Where(p => p.SpecialCheckRowId == model.SpecialCheckRowId).FirstOrDefault().SpecialCheckStatusRowID;

                                context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == statusId).DETMCheckStatus = model.CheckStatus;
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

        public void UpdateSpecialCheckRejectionByDEQC(UpdateSpecialCheckViewModel model, bool flag = false)
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

                                context.PQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowId).CheckStatus = model.CheckStatus;
                                context.SaveChanges();

                                int statusId = context.PQSpecialCheckStatus.Where(p => p.SpecialCheckRowId == model.SpecialCheckRowId).FirstOrDefault().SpecialCheckStatusRowID;
                                if (flag)
                                {
                                    context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).DECaseStatus = "WIP";
                                    context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).AllocatedToDEDate = DateTime.Now;
                                    context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).DEQCCaseStatus = "WIP";
                                    context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).AllocatedToDEQCDate = DateTime.Now;
                                    context.SaveChanges();

                                    context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == statusId).DETMQCCheckStatus = "";
                                    context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == statusId).DETMCheckStatus = model.CheckStatus;
                                }
                                else
                                {
                                    context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == statusId).DETMQCCheckStatus = "Completed-QC";
                                    context.PQSpecialCheckStatus.Single(c => c.SpecialCheckStatusRowID == statusId).TMCheckStatus = model.CheckStatus;
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


        #region TempSpecialChecks

        public AddTempSpecialCheckViewModel GetTempSpecialCheckInfoForDataEntry(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0)
        {
            try
            {
                return db.TempPQSpecialChecks.Where(a => a.ClientRowID == ClientRowId && a.PersonalRowID == PersonalRowID
                && a.CheckFamilyRowID == ChFamilRowId && a.SubCheckRowID == subCheckId).Select(a => new AddTempSpecialCheckViewModel
                {
                    SpecialCheckRowId = a.SpecialCheckRowId,
                    ClientRowID = a.ClientRowID,
                    PersonalRowID = a.PersonalRowID,
                    ClientPackageRowID = a.ClientPackageRowID,
                    SC_Cand_Name = a.SC_Cand_Name,
                    CheckFamilyRowID = a.CheckFamilyRowID,
                    SubCheckRowID = a.SubCheckRowID,
                    UniqueComponentID = a.UniqueComponentID,
                    SC_Father_Name = a.SC_Father_Name,
                    SC_SecuritasID = a.SC_SecuritasID,
                    SC_DOB = a.SC_DOB,
                    SC_Others1 = a.SC_Others1,
                    SC_Others2 = a.SC_Others2,
                    SC_Others3 = a.SC_Others3,
                    SC_Others4 = a.SC_Others4,
                    SC_Others5 = a.SC_Others5,
                    CheckStatus = a.CheckStatus,
                }).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddTempSpacelCheckInformation(AddTempSpecialCheckViewModel model)
        {
            try
            {
                if (model != null)
                {
                    TempPQSpecialCheck entity = new TempPQSpecialCheck();
                    entity.ClientRowID = model.ClientRowID;
                    entity.PersonalRowID = model.PersonalRowID;
                    entity.ClientPackageRowID = model.ClientPackageRowID;
                    entity.CheckFamilyRowID = model.CheckFamilyRowID;
                    entity.SubCheckRowID = model.SubCheckRowID;
                    entity.UniqueComponentID = model.UniqueComponentID;
                    //entity.SC_Cand_Name = model.SC_Cand_Name;
                    if (model.SC_Cand_Name == null)
                    {
                        entity.SC_Cand_Name = db.PQCandidateLogins.Where(w => w.TempPersonalRowID == model.PersonalRowID).FirstOrDefault().CandidateName;
                    }
                    else
                    {
                        entity.SC_Cand_Name = model.SC_Cand_Name;
                    }
                    entity.SC_SecuritasID = model.SC_SecuritasID;
                    entity.SC_Father_Name = model.SC_Father_Name;
                    entity.SC_DOB = model.SC_DOB;
                    entity.SC_Others1 = model.SC_Others1;
                    entity.SC_Others2 = model.SC_Others2;
                    entity.SC_Others3 = model.SC_Others3;
                    entity.SC_Others4 = model.SC_Others4;
                    entity.SC_Others5 = model.SC_Others5;
                    entity.CreatedBy = model.CreatedBy;
                    entity.CreatedDate = model.CreatedDate;
                    entity.CheckStatus = model.CheckStatus;
                    entity.Remarks = model.Remarks;
                    entity.Status = model.Status;

                    db.TempPQSpecialChecks.Add(entity);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateTempSpecialInfoByCandidate(AddTempSpecialCheckViewModel model)
        {
            try
            {
                if (model != null)
                {
                    db.TempPQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowId).SC_Cand_Name = model.SC_Cand_Name;
                    db.TempPQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowId).SC_Father_Name = model.SC_Father_Name;
                    db.TempPQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowId).SC_DOB = model.SC_DOB;
                    db.TempPQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowId).SC_Others1 = model.SC_Others1;
                    db.TempPQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowId).SC_Others2 = model.SC_Others2;
                    db.TempPQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowId).SC_Others3 = model.SC_Others3;
                    db.TempPQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowId).SC_Others4 = model.SC_Others4;
                    db.TempPQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowId).SC_Others5 = model.SC_Others5;
                    db.TempPQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowId).ModifiedBy = model.CreatedBy;
                    db.TempPQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowId).ModifiedDate = model.CreatedDate;
                    db.TempPQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowId).CheckStatus = model.CheckStatus;
                    db.TempPQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowId).Remarks = model.Remarks;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetAllTempPQSpecialByClientId(short ClientRowID = 0)
        {
            try
            {
                var data = db.TempPQSpecialChecks.Where(p => p.ClientRowID == ClientRowID).ToList();
                return data.Count();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateTempSpecialCheckInfoByDataEntry(AddTempSpecialCheckViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            context.TempPQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowId).SC_Cand_Name = model.SC_Cand_Name;
                            context.TempPQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowId).SC_Father_Name = model.SC_Father_Name;
                            context.TempPQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowId).SC_DOB = model.SC_DOB;
                            context.TempPQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowId).SC_Others1 = model.SC_Others1;
                            context.TempPQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowId).SC_Others2 = model.SC_Others2;
                            context.TempPQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowId).SC_Others3 = model.SC_Others3;
                            context.TempPQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowId).SC_Others4 = model.SC_Others4;
                            context.TempPQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowId).SC_Others5 = model.SC_Others5;

                            context.TempPQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowId).CheckStatus = model.CheckStatus;
                            if (model.CheckStatus == "Completed")
                            {
                                context.TempPQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowId).CheckStatus = "Completed-DE";
                            }

                            context.TempPQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowId).ModifiedBy = model.CreatedBy;
                            context.TempPQSpecialChecks.Single(c => c.SpecialCheckRowId == model.SpecialCheckRowId).ModifiedDate = model.CreatedDate;
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

        #endregion
    }
}
