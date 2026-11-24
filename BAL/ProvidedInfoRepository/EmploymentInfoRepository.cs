using DAL;
using System;
using System.Data;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.ProvidedInfoViewModel;
using ViewModels.TempViewModel;

namespace BAL.ProvidedInfoRepository
{
    public class EmploymentInfoRepository : IEmploymentInfoRepository
    {
        DataContext db;
        ClientCustomize cc = new ClientCustomize();
        public EmploymentInfoRepository()
        {
            db = new DataContext();
        }

        public void AddEmploymentInformation(AddEmploymentViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            PQEmployment entity = new PQEmployment();
                            entity.ClientRowID = model.ClientRowID;
                            entity.PersonalRowID = model.PersonalRowID;
                            entity.ClientPackageRowID = model.ClientPackageRowID;
                            entity.CheckFamilyRowID = model.CheckFamilyRowID;
                            entity.SubCheckRowID = model.SubCheckRowID;
                            entity.UniqueComponentID = model.UniqueComponentID;
                            entity.EV_CandidateName = model.EV_CandidateName;
                            entity.EV_SecuritasReferenceID = model.EV_SecuritasReferenceID;
                            entity.EV_OrganisationName = model.EV_OrganisationName;
                            entity.EV_CmpLocation = model.EV_CmpLocation;
                            entity.EV_CmpNameAddress = model.EV_CmpNameAddress;
                            entity.EV_EmployeeName = model.EV_EmployeeName;
                            entity.EV_EmployeeCode = model.EV_EmployeeCode;
                            entity.EV_EmploymentPeriod = model.EV_EmploymentPeriod;
                            entity.EV_CandidateTenureWorked = model.EV_CandidateTenureWorked;
                            entity.EV_EmployeeTenure = model.EV_EmployeeTenure;
                            entity.EV_Designation = model.EV_Designation;
                            entity.EV_EmpType = model.EV_EmpType;
                            entity.EV_Department = model.EV_Department;
                            entity.EV_CmpCost = model.EV_CmpCost;
                            entity.EV_last_sal = model.EV_last_sal;
                            entity.EV_leavingReason = model.EV_leavingReason;
                            entity.EV_DiscontinueServiceReason = model.EV_DiscontinueServiceReason;
                            entity.EV_ReportingManagerName = model.EV_ReportingManagerName;
                            entity.EV_Rptng_Mgr_Name_Ph = model.EV_Rptng_Mgr_Name_Ph;
                            entity.EV_Rptng_Mgr_Name_ConDet = model.EV_Rptng_Mgr_Name_ConDet;
                            entity.EV_Sub_Rptng_Mgr_Name_Dsg_EidPh = model.EV_Sub_Rptng_Mgr_Name_Dsg_EidPh;
                            entity.EV_Supervisor = model.EV_Supervisor;
                            entity.EV_Sup_Name_Dsg = model.EV_Sup_Name_Dsg;
                            entity.EV_FormalitiesExit = model.EV_FormalitiesExit;
                            entity.EV_FullFinalFormalities = model.EV_FullFinalFormalities;
                            entity.EV_ExitFormalitiesPending = model.EV_ExitFormalitiesPending;
                            entity.EV_HandledResponsibilities = model.EV_HandledResponsibilities;
                            entity.EV_ResponsibilitiesHandled = model.EV_ResponsibilitiesHandled;
                            entity.EV_OtherDetails = model.EV_OtherDetails;
                            entity.EV_OtherDetails2 = model.EV_OtherDetails2;
                            entity.EV_OtherDetails3 = model.EV_OtherDetails3;
                            entity.EV_OtherDetails4 = model.EV_OtherDetails4;
                            entity.EV_OtherDetails5 = model.EV_OtherDetails5;
                            entity.EV_OtherDetails6 = model.EV_OtherDetails6;
                            entity.EV_OtherDetails7 = model.EV_OtherDetails7;
                            entity.EV_OtherDetails8 = model.EV_OtherDetails8;
                            entity.EV_OtherDetails9 = model.EV_OtherDetails9;
                            entity.EV_OtherDetails10 = model.EV_OtherDetails10;
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

                            entity.Status = model.Status;
                            context.PQEmployments.Add(entity);
                            context.SaveChanges();

                            #region Save Check Status

                            PQEmploymentStatus chkEntity = new PQEmploymentStatus();
                            var empModel = context.PQEmployments.Include("PQPersonal").Where(p => p.EmploymentRowId == entity.EmploymentRowId).FirstOrDefault();
                            chkEntity.EmploymentRowId = empModel.EmploymentRowId;
                            chkEntity.DETLAllocatedTo = empModel.PQPersonal.AllocatedBy;
                            chkEntity.DETMCheckStatus = model.CheckStatus;
                            chkEntity.DETMAllocatedTo = empModel.PQPersonal.AllocatedToDE;
                            chkEntity.DETMAllocatedDate = empModel.PQPersonal.AllocatedToDEDate;
                            chkEntity.DETMQCAllocatedTo = empModel.PQPersonal.AllocatedToDEQC;
                            chkEntity.DETMQCAllocatedDate = empModel.PQPersonal.AllocatedToDEQCDate;

                            if (model.CheckStatus == "Completed")
                            {
                                chkEntity.DETMQCCheckStatus = model.CheckStatus;
                            }
                            if (model.CheckStatus == "Insufficent")
                            {
                                chkEntity.DETMQCCheckStatus = model.CheckStatus;
                                chkEntity.InfSuffRaiseBy = empModel.PQPersonal.AllocatedToDE;
                                chkEntity.InfSuffRaisedDate = empModel.CreatedDate;
                                chkEntity.InfSuffRaiseRemarks = model.INFRemarks;
                            }
                            if (model.CheckStatus == "Rejected" || model.CheckStatus == "Review Requested")
                            {
                                chkEntity.Remarks = model.INFRemarks;
                            }
                            context.PQEmploymentStatus.Add(chkEntity);
                            context.SaveChanges();

                            if (model.CheckStatus == "YTR")
                            {
                                context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).CaseStatus = "YTR";
                                context.SaveChanges();
                            }

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

        public EmployeeAddressAndLocation GetAddressAndLocation(short EmploymentRowID)
        {
            try
            {
                EmployeeAddressAndLocation model = new EmployeeAddressAndLocation();
                var entity = db.MasterEmployers.Find(EmploymentRowID);

                if (entity != null)
                {
                    model.Address = entity.CompanyAddress;
                    model.Location = entity.MasterLocation.LocationName;
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

        public int GetAllEmploymentByClientId(short ClientRowID = 0)
        {
            try
            {
                var data = db.PQEmployments.Where(p => p.ClientRowID == ClientRowID).ToList();
                return data.Count();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetEmploymentIDByUniqueComponentID(int PersonalRowID = 0, string UniqueComponentID = "")
        {
            try
            {
                return db.PQEmployments.Where(p => p.PersonalRowID == PersonalRowID && p.UniqueComponentID == UniqueComponentID).FirstOrDefault().EmploymentRowId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public AddEmploymentViewModel GetEmploymentInfoForDataEntry(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0)
        {
            try
            {

                string EV_OrganisationName = "0";
                string OrgLabel = "";
                int orgId = 0;
                EV_OrganisationName = db.PQEmployments.Where(a => a.ClientRowID == ClientRowId && a.PersonalRowID == PersonalRowID
                && a.CheckFamilyRowID == ChFamilRowId && a.SubCheckRowID == subCheckId).Select(a => a.EV_OrganisationName).FirstOrDefault();
                if (!string.IsNullOrEmpty(EV_OrganisationName))
                {
                    orgId = int.Parse(EV_OrganisationName);
                    if (EV_OrganisationName != "0")
                    {
                        OrgLabel = db.MasterEmployers.Where(M => M.EmployerRowID == orgId).Select(K => K.CompanyName).FirstOrDefault();
                    }
                }


                return db.PQEmployments.Where(a => a.ClientRowID == ClientRowId && a.PersonalRowID == PersonalRowID
                && a.CheckFamilyRowID == ChFamilRowId && a.SubCheckRowID == subCheckId).Select(a => new AddEmploymentViewModel
                {
                    EmploymentRowID = a.EmploymentRowId,
                    ClientRowID = a.ClientRowID,
                    PersonalRowID = a.PersonalRowID,
                    ClientPackageRowID = a.ClientPackageRowID,
                    CheckFamilyRowID = a.CheckFamilyRowID,
                    SubCheckRowID = a.SubCheckRowID,
                    UniqueComponentID = a.UniqueComponentID,
                    //EV_CandidateName = a.EV_CandidateName,
                    EV_CandidateName = db.PQPersonals.Where(T => T.PersonalRowID == a.PersonalRowID).Select(x => x.Name + " " + x.MiddleName + x.LastName).FirstOrDefault().ToString(),
                    EV_OrganisationName = a.EV_OrganisationName,
                    EV_SecuritasReferenceID = a.EV_SecuritasReferenceID,
                    EV_CmpLocation = a.EV_CmpLocation,
                    // EV_CmpNameAddress = a.EV_CmpNameAddress,
                    EV_EmployeeName = a.EV_EmployeeName,
                    EV_EmployeeCode = a.EV_EmployeeCode,
                    EV_EmploymentPeriod = a.EV_EmploymentPeriod,
                    EV_CandidateTenureWorked = a.EV_CandidateTenureWorked,
                    EV_EmployeeTenure = a.EV_EmployeeTenure,
                    EV_Designation = a.EV_Designation,
                    EV_EmpType = a.EV_EmpType,
                    EV_Department = a.EV_Department,
                    EV_CmpCost = a.EV_CmpCost,
                    EV_last_sal = a.EV_last_sal,
                    EV_leavingReason = a.EV_leavingReason,
                    EV_DiscontinueServiceReason = a.EV_DiscontinueServiceReason,
                    EV_ReportingManagerName = a.EV_ReportingManagerName,
                    EV_Rptng_Mgr_Name_Ph = a.EV_Rptng_Mgr_Name_Ph,
                    EV_Rptng_Mgr_Name_ConDet = a.EV_Rptng_Mgr_Name_ConDet,
                    EV_Sub_Rptng_Mgr_Name_Dsg_EidPh = a.EV_Sub_Rptng_Mgr_Name_Dsg_EidPh,
                    EV_Supervisor = a.EV_Supervisor,
                    EV_Sup_Name_Dsg = a.EV_Sup_Name_Dsg,
                    EV_FormalitiesExit = a.EV_FormalitiesExit,
                    EV_FullFinalFormalities = a.EV_FullFinalFormalities,
                    EV_ExitFormalitiesPending = a.EV_ExitFormalitiesPending,
                    EV_HandledResponsibilities = a.EV_HandledResponsibilities,
                    EV_ResponsibilitiesHandled = a.EV_ResponsibilitiesHandled,
                    EV_OtherDetails = a.EV_OtherDetails,
                    EV_OtherDetails2 = a.EV_OtherDetails2,
                    EV_OtherDetails3 = a.EV_OtherDetails3,
                    EV_OtherDetails4 = a.EV_OtherDetails4,
                    EV_OtherDetails5 = a.EV_OtherDetails5,
                    EV_OtherDetails6 = a.EV_OtherDetails6,
                    EV_OtherDetails7 = a.EV_OtherDetails7,
                    EV_OtherDetails8 = a.EV_OtherDetails8,
                    EV_OtherDetails9 = a.EV_OtherDetails9,
                    EV_OtherDetails10 = a.EV_OtherDetails10,
                    ATA_CID_No = a.ATA_CID_No,
                    ATA_Cmpny_Addr = a.ATA_Cmpny_Addr,
                    CheckStatus = a.CheckStatus,
                    ReWorkCheckStatus = a.ReWorkCheckStatus,
                    //InfSuffRaiseBy = a.InfSuffRaiseBy,
                    //InfSuffRaisedDate = a.InfSuffRaisedDate,
                    //INFRemarks = a.INFRemarks,
                    OtherEmployer = a.Remarks,
                    OrgNameLabel = OrgLabel,


                }).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateEmploymentInfoByDataEntry(AddEmploymentViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_OrganisationName = model.EV_OrganisationName;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_CmpLocation = model.EV_CmpLocation;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_CmpNameAddress = model.EV_CmpNameAddress;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_EmployeeName = model.EV_EmployeeName;
                            //context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_CandidateName = model.EV_CandidateName;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_EmployeeCode = model.EV_EmployeeCode;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_EmploymentPeriod = model.EV_EmploymentPeriod;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_CandidateTenureWorked = model.EV_CandidateTenureWorked;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_EmployeeTenure = model.EV_EmployeeTenure;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_Designation = model.EV_Designation;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_EmpType = model.EV_EmpType;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_Department = model.EV_Department;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_CmpCost = model.EV_CmpCost;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_last_sal = model.EV_last_sal;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_leavingReason = model.EV_leavingReason;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_DiscontinueServiceReason = model.EV_DiscontinueServiceReason;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_ReportingManagerName = model.EV_ReportingManagerName;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_Rptng_Mgr_Name_Ph = model.EV_Rptng_Mgr_Name_Ph;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_Rptng_Mgr_Name_ConDet = model.EV_Rptng_Mgr_Name_ConDet;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_Sub_Rptng_Mgr_Name_Dsg_EidPh = model.EV_Sub_Rptng_Mgr_Name_Dsg_EidPh;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_Supervisor = model.EV_Supervisor;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_Sup_Name_Dsg = model.EV_Sup_Name_Dsg;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_FormalitiesExit = model.EV_FormalitiesExit;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_FullFinalFormalities = model.EV_FullFinalFormalities;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_ExitFormalitiesPending = model.EV_ExitFormalitiesPending;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_HandledResponsibilities = model.EV_HandledResponsibilities;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_ResponsibilitiesHandled = model.EV_ResponsibilitiesHandled;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_OtherDetails = model.EV_OtherDetails;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_OtherDetails2 = model.EV_OtherDetails2;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_OtherDetails3 = model.EV_OtherDetails3;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_OtherDetails4 = model.EV_OtherDetails4;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_OtherDetails5 = model.EV_OtherDetails5;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_OtherDetails6 = model.EV_OtherDetails6;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_OtherDetails7 = model.EV_OtherDetails7;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_OtherDetails8 = model.EV_OtherDetails8;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_OtherDetails9 = model.EV_OtherDetails9;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_OtherDetails10 = model.EV_OtherDetails10;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).ATA_CID_No = model.ATA_CID_No;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).ATA_Cmpny_Addr = model.ATA_Cmpny_Addr;

                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).CheckStatus = model.CheckStatus;
                            if (model.CheckStatus == "Completed")
                            {
                                context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).CheckStatus = "Completed-DE";
                            }
                            else if (model.CheckStatus == "Not Applicable")
                            {
                                context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).CheckStatus = "Not Applicable";
                            }


                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).ModifiedBy = model.CreatedBy;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).ModifiedDate = model.CreatedDate;
                            context.SaveChanges();

                            #region Save Check Status

                            var status = context.PQEmploymentStatus.Where(p => p.EmploymentRowId == model.EmploymentRowID).FirstOrDefault();
                            if (status.TMCheckStatus == "Insufficent" && status.TMQCCheckStatus == "Insufficent-QC" && model.CheckStatus == "Completed")
                            {
                                context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).CheckStatus = "WIP-Pending";
                                context.SaveChanges();

                                context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == status.EmploymentStatusRowID).DETMCheckStatus = model.CheckStatus;
                                context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == status.EmploymentStatusRowID).DETMAllocatedDate = model.CreatedDate;
                                context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == status.EmploymentStatusRowID).TMCheckStatus = "WIP-Pending";
                                context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == status.EmploymentStatusRowID).TMAllocatedDate = model.CreatedDate;
                                context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == status.EmploymentStatusRowID).TMQCCheckStatus = "";
                                context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == status.EmploymentStatusRowID).TMQCAllocatedDate = model.CreatedDate;
                            }

                            else if (model.CheckStatus == "Not Applicable")
                            {
                                context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).CheckStatus = "Not Applicable";
                                context.SaveChanges();
                            }
                            else
                            {
                                context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == status.EmploymentStatusRowID).DETMCheckStatus = model.CheckStatus;
                                context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == status.EmploymentStatusRowID).DETMAllocatedDate = model.CreatedDate;

                                if (model.CheckStatus == "Completed")
                                {
                                    context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == status.EmploymentStatusRowID).DETMQCCheckStatus = model.CheckStatus;
                                    context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == status.EmploymentStatusRowID).DETMQCAllocatedDate = model.CreatedDate;
                                }
                                if (model.CheckStatus == "Insufficent")
                                {
                                    context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == status.EmploymentStatusRowID).DETMQCCheckStatus = model.CheckStatus;
                                    context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == status.EmploymentStatusRowID).DETMQCAllocatedDate = model.CreatedDate;
                                    context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == status.EmploymentStatusRowID).InfSuffRaiseBy = model.CreatedBy;
                                    context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == status.EmploymentStatusRowID).InfSuffRaisedDate = model.CreatedDate;
                                    context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == status.EmploymentStatusRowID).InfSuffRaiseRemarks = model.INFRemarks;
                                }
                                if (model.CheckStatus == "Rejected" || model.CheckStatus == "Review Requested")
                                {
                                    context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == status.EmploymentStatusRowID).Remarks = model.INFRemarks;
                                }
                            }
                            context.SaveChanges();

                            if (model.CheckStatus == "YTR")
                            {
                                context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).CaseStatus = "YTR";
                                context.SaveChanges();
                            }

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

        public UpdateEmploymentViewModel GetEmploymentInfoForQC(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0)
        {
            try
            {
                UpdateEmploymentViewModel model = new UpdateEmploymentViewModel();
                DataTable dt = new DataTable();
                ClientCustomize cc = new ClientCustomize();

                var entity = db.PQEmployments.Where(b => b.ClientRowID == ClientRowId && b.PersonalRowID == PersonalRowID
                 && b.CheckFamilyRowID == ChFamilRowId && b.SubCheckRowID == subCheckId).FirstOrDefault();
                {

                    int CompanyId = Convert.ToInt32(entity.EV_OrganisationName);
                    if (CompanyId > 0)
                    {
                        model.OrganisationName = db.MasterEmployers.Find(CompanyId).CompanyName;
                    }
                    model.EmploymentRowID = entity.EmploymentRowId;
                    model.ClientRowID = entity.ClientRowID;
                    model.PersonalRowID = entity.PersonalRowID;
                    model.ClientPackageRowID = entity.ClientPackageRowID;
                    model.CheckFamilyRowID = entity.CheckFamilyRowID;
                    model.SubCheckRowID = entity.SubCheckRowID;
                    model.EV_CandidateName = entity.EV_CandidateName;
                    model.UniqueComponentID = entity.UniqueComponentID;
                    model.EV_OrganisationName = entity.EV_OrganisationName;
                    model.EV_CmpLocation = entity.EV_CmpLocation;

                    dt = cc.GetComapnyAddress(Convert.ToInt32(model.EV_OrganisationName));
                    if (dt.Rows.Count > 0)
                    {
                        model.EV_CmpNameAddress = dt.Rows[0]["CompanyAddress"].ToString();
                    }
                    //model.EV_CmpNameAddress = entity.EV_CmpNameAddress;
                    model.EV_EmployeeName = entity.EV_EmployeeName;
                    model.EV_EmployeeCode = entity.EV_EmployeeCode;
                    model.EV_EmploymentPeriod = entity.EV_EmploymentPeriod;
                    model.EV_CandidateTenureWorked = entity.EV_CandidateTenureWorked;
                    model.EV_EmployeeTenure = entity.EV_EmployeeTenure;
                    model.EV_Designation = entity.EV_Designation;
                    model.EV_EmpType = entity.EV_EmpType;
                    model.EV_Department = entity.EV_Department;
                    model.EV_CmpCost = entity.EV_CmpCost;
                    model.EV_last_sal = entity.EV_last_sal;
                    model.EV_leavingReason = entity.EV_leavingReason;
                    model.EV_DiscontinueServiceReason = entity.EV_DiscontinueServiceReason;
                    model.EV_ReportingManagerName = entity.EV_ReportingManagerName;
                    model.EV_Rptng_Mgr_Name_Ph = entity.EV_Rptng_Mgr_Name_Ph;
                    model.EV_Rptng_Mgr_Name_ConDet = entity.EV_Rptng_Mgr_Name_ConDet;
                    model.EV_Sub_Rptng_Mgr_Name_Dsg_EidPh = entity.EV_Sub_Rptng_Mgr_Name_Dsg_EidPh;
                    model.EV_Supervisor = entity.EV_Supervisor;
                    model.EV_Sup_Name_Dsg = entity.EV_Sup_Name_Dsg;
                    model.EV_FormalitiesExit = entity.EV_FormalitiesExit;
                    model.EV_FullFinalFormalities = entity.EV_FullFinalFormalities;
                    model.EV_ExitFormalitiesPending = entity.EV_ExitFormalitiesPending;
                    model.EV_HandledResponsibilities = entity.EV_HandledResponsibilities;
                    model.EV_ResponsibilitiesHandled = entity.EV_ResponsibilitiesHandled;
                    model.EV_OtherDetails = entity.EV_OtherDetails;
                    model.EV_OtherDetails2 = entity.EV_OtherDetails2;
                    model.EV_OtherDetails3 = entity.EV_OtherDetails3;
                    model.EV_OtherDetails4 = entity.EV_OtherDetails4;
                    model.EV_OtherDetails5 = entity.EV_OtherDetails5;
                    model.EV_OtherDetails6 = entity.EV_OtherDetails6;
                    model.EV_OtherDetails7 = entity.EV_OtherDetails7;
                    model.EV_OtherDetails8 = entity.EV_OtherDetails8;
                    model.EV_OtherDetails9 = entity.EV_OtherDetails9;
                    model.EV_OtherDetails10 = entity.EV_OtherDetails10;
                    model.ATA_CID_No = entity.ATA_CID_No;
                    model.ATA_Cmpny_Addr = entity.ATA_Cmpny_Addr;
                    model.CheckStatus = entity.CheckStatus;
                    model.ReWorkCheckStatus = entity.ReWorkCheckStatus;

                }
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public UpdateEmploymentViewModel GetEmploymentInfoForQC(int EmploymentRowID = 0)
        {
            try
            {
                UpdateEmploymentViewModel model = new UpdateEmploymentViewModel();
                DataTable dt = new DataTable();

                ClientCustomize cc = new ClientCustomize();
                var entity = db.PQEmployments.Where(b => b.EmploymentRowId == EmploymentRowID).FirstOrDefault();
                {
                    short CompanyId = Convert.ToInt16(entity.EV_OrganisationName);
                    if (CompanyId > 0)
                    {
                        model.OrganisationName = db.MasterEmployers.Find(CompanyId).CompanyName;
                    }
                    model.EmploymentRowID = entity.EmploymentRowId;
                    model.ClientRowID = entity.ClientRowID;
                    model.PersonalRowID = entity.PersonalRowID;
                    model.EV_SecuritasReferenceID = entity.PQPersonal.CompanyRefNo;
                    model.ClientPackageRowID = entity.ClientPackageRowID;
                    model.CheckFamilyRowID = entity.CheckFamilyRowID;
                    model.SubCheckRowID = entity.SubCheckRowID;
                    model.EV_CandidateName = entity.EV_CandidateName;
                    model.UniqueComponentID = entity.UniqueComponentID;
                    model.EV_OrganisationName = entity.EV_OrganisationName;
                    model.EV_CmpLocation = entity.EV_CmpLocation;

                    dt = cc.GetComapnyAddress(Convert.ToInt32(model.EV_OrganisationName));
                    if (dt.Rows.Count > 0)
                    {
                        model.EV_CmpNameAddress = dt.Rows[0]["CompanyAddress"].ToString();
                    }

                    //model.EV_CmpNameAddress = entity.EV_CmpNameAddress;
                    model.EV_EmployeeName = entity.EV_EmployeeName;
                    model.EV_EmployeeCode = entity.EV_EmployeeCode;
                    model.EV_EmploymentPeriod = entity.EV_EmploymentPeriod;
                    model.EV_CandidateTenureWorked = entity.EV_CandidateTenureWorked;
                    model.EV_EmployeeTenure = entity.EV_EmployeeTenure;
                    model.EV_Designation = entity.EV_Designation;
                    model.EV_EmpType = entity.EV_EmpType;
                    model.EV_Department = entity.EV_Department;
                    model.EV_CmpCost = entity.EV_CmpCost;
                    model.EV_last_sal = entity.EV_last_sal;
                    model.EV_leavingReason = entity.EV_leavingReason;
                    model.EV_DiscontinueServiceReason = entity.EV_DiscontinueServiceReason;
                    model.EV_ReportingManagerName = entity.EV_ReportingManagerName;
                    model.EV_Rptng_Mgr_Name_Ph = entity.EV_Rptng_Mgr_Name_Ph;
                    model.EV_Rptng_Mgr_Name_ConDet = entity.EV_Rptng_Mgr_Name_ConDet;
                    model.EV_Sub_Rptng_Mgr_Name_Dsg_EidPh = entity.EV_Sub_Rptng_Mgr_Name_Dsg_EidPh;
                    model.EV_Supervisor = entity.EV_Supervisor;
                    model.EV_Sup_Name_Dsg = entity.EV_Sup_Name_Dsg;
                    model.EV_FormalitiesExit = entity.EV_FormalitiesExit;
                    model.EV_FullFinalFormalities = entity.EV_FullFinalFormalities;
                    model.EV_ExitFormalitiesPending = entity.EV_ExitFormalitiesPending;
                    model.EV_HandledResponsibilities = entity.EV_HandledResponsibilities;
                    model.EV_ResponsibilitiesHandled = entity.EV_ResponsibilitiesHandled;
                    model.EV_OtherDetails = entity.EV_OtherDetails;
                    model.EV_OtherDetails2 = entity.EV_OtherDetails2;
                    model.EV_OtherDetails3 = entity.EV_OtherDetails3;
                    model.EV_OtherDetails4 = entity.EV_OtherDetails4;
                    model.EV_OtherDetails5 = entity.EV_OtherDetails5;
                    model.EV_OtherDetails6 = entity.EV_OtherDetails6;
                    model.EV_OtherDetails7 = entity.EV_OtherDetails7;
                    model.EV_OtherDetails8 = entity.EV_OtherDetails8;
                    model.EV_OtherDetails9 = entity.EV_OtherDetails9;
                    model.EV_OtherDetails10 = entity.EV_OtherDetails10;
                    model.ATA_CID_No = entity.ATA_CID_No;
                    model.ATA_Cmpny_Addr = entity.ATA_Cmpny_Addr;
                    model.CheckStatus = entity.CheckStatus;
                    model.ReWorkCheckStatus = entity.ReWorkCheckStatus;
                }
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateEmploymentInfoByQC(UpdateEmploymentViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_OrganisationName = model.EV_OrganisationName;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_CmpLocation = model.EV_CmpLocation;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_CmpNameAddress = model.EV_CmpNameAddress;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_EmployeeName = model.EV_EmployeeName;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_EmployeeCode = model.EV_EmployeeCode;
                            //context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_CandidateName = model.EV_CandidateName;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_EmploymentPeriod = model.EV_EmploymentPeriod;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_CandidateTenureWorked = model.EV_CandidateTenureWorked;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_EmployeeTenure = model.EV_EmployeeTenure;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_Designation = model.EV_Designation;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_EmpType = model.EV_EmpType;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_Department = model.EV_Department;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_CmpCost = model.EV_CmpCost;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_last_sal = model.EV_last_sal;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_leavingReason = model.EV_leavingReason;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_DiscontinueServiceReason = model.EV_DiscontinueServiceReason;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_ReportingManagerName = model.EV_ReportingManagerName;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_Rptng_Mgr_Name_Ph = model.EV_Rptng_Mgr_Name_Ph;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_Rptng_Mgr_Name_ConDet = model.EV_Rptng_Mgr_Name_ConDet;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_Sub_Rptng_Mgr_Name_Dsg_EidPh = model.EV_Sub_Rptng_Mgr_Name_Dsg_EidPh;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_Supervisor = model.EV_Supervisor;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_Sup_Name_Dsg = model.EV_Sup_Name_Dsg;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_FormalitiesExit = model.EV_FormalitiesExit;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_FullFinalFormalities = model.EV_FullFinalFormalities;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_ExitFormalitiesPending = model.EV_ExitFormalitiesPending;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_HandledResponsibilities = model.EV_HandledResponsibilities;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_ResponsibilitiesHandled = model.EV_ResponsibilitiesHandled;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_OtherDetails = model.EV_OtherDetails;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_OtherDetails2 = model.EV_OtherDetails2;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_OtherDetails3 = model.EV_OtherDetails3;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_OtherDetails4 = model.EV_OtherDetails4;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_OtherDetails5 = model.EV_OtherDetails5;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_OtherDetails6 = model.EV_OtherDetails6;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_OtherDetails7 = model.EV_OtherDetails7;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_OtherDetails8 = model.EV_OtherDetails8;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_OtherDetails9 = model.EV_OtherDetails9;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_OtherDetails10 = model.EV_OtherDetails10;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).ATA_CID_No = model.ATA_CID_No;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).ATA_Cmpny_Addr = model.ATA_Cmpny_Addr;

                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).CheckStatus = model.CheckStatus;
                            if (model.CheckStatus == "Completed-DE")
                            {
                                context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).CheckStatus = "Allocated-Mgr";
                            }
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).ModifiedBy = model.ModifiedBy;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).ModifiedDate = model.ModifiedDate;
                            context.SaveChanges();

                            #region Save Check Status

                            var CurrentStatus = context.PQEmploymentStatus.Where(p => p.EmploymentRowId == model.EmploymentRowID).FirstOrDefault();

                            if (model.CheckStatus == "Completed-DE")
                            {

                                if (CurrentStatus.TMCheckStatus == "Rejected")
                                {
                                    context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).CheckStatus = "WIP-Pending";
                                    context.SaveChanges();

                                    context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == CurrentStatus.EmploymentStatusRowID).DETMQCCheckStatus = "Completed-QC";
                                    context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == CurrentStatus.EmploymentStatusRowID).TMCheckStatus = "WIP-Pending";
                                    context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == CurrentStatus.EmploymentStatusRowID).TMAllocatedDate = model.ModifiedDate;
                                }
                                else
                                {
                                    context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == CurrentStatus.EmploymentStatusRowID).DETMQCCheckStatus = "Completed-QC";
                                    context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == CurrentStatus.EmploymentStatusRowID).MgrCheckStatus = "Allocated-Mgr";
                                    context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == CurrentStatus.EmploymentStatusRowID).MgrAllocatedTo = model.MgrAllocatedTo;
                                    context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == CurrentStatus.EmploymentStatusRowID).MgrAllocatedDate = model.ModifiedDate;
                                }

                            }

                            if (model.CheckStatus == "Insufficent")
                            {
                                context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == CurrentStatus.EmploymentStatusRowID).DETMQCCheckStatus = "Insufficent-QC";
                                context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == CurrentStatus.EmploymentStatusRowID).InfSuffClearBy = 0;
                                context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == CurrentStatus.EmploymentStatusRowID).InfSuffClearComments = "";
                                context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == CurrentStatus.EmploymentStatusRowID).InfSuffClearDate = null;
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

        public void UpdateEmploymentInfoByManager(PQUpdateEmploymentManagerViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).CheckStatus = model.CheckStatus;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).ModifiedBy = model.ModifiedBy;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).ModifiedDate = model.ModifiedDate;
                            context.SaveChanges();

                            #region Save Check Status

                            int statusId = context.PQEmploymentStatus.Where(p => p.EmploymentRowId == model.EmploymentRowID).FirstOrDefault().EmploymentStatusRowID;

                            context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == statusId).TLCheckStatus = model.CheckStatus;
                            context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == statusId).TLAllocatedTo = model.TLAllocatedTo;
                            context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == statusId).TLAllocatedDate = model.ModifiedDate;
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

        public void UpdateEmploymentInfoByTeamLeader(PQUpdateEmploymentTeamLeaderViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).CheckStatus = model.CheckStatus;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).ModifiedBy = model.ModifiedBy;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).ModifiedDate = model.ModifiedDate;
                            context.SaveChanges();

                            #region Save Check Status

                            int statusId = context.PQEmploymentStatus.Where(p => p.EmploymentRowId == model.EmploymentRowID).FirstOrDefault().EmploymentStatusRowID;

                            context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == statusId).TMCheckStatus = model.CheckStatus;
                            context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == statusId).TMAllocatedTo = model.TMAllocatedTo;
                            context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == statusId).TMAllocatedDate = model.ModifiedDate;

                            context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == statusId).TMQCCheckStatus = model.CheckStatus;
                            context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == statusId).TMQCAllocatedTo = model.TMQCAllocatedTo;
                            context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == statusId).TMQCAllocatedDate = model.ModifiedDate;

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
        public void UpdateEmploymentInfoByTeamLeaderReAllocated(PQUpdateEmploymentTeamLeaderViewModel model)
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

                            int statusId = context.PQEmploymentStatus.Where(p => p.EmploymentRowId == model.EmploymentRowID).FirstOrDefault().EmploymentStatusRowID;

                            if (model.TMAllocatedTo > 0)
                            {
                                context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == statusId).TMAllocatedTo = model.TMAllocatedTo;
                                context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == statusId).TMAllocatedDate = model.ModifiedDate;
                            }
                            if (model.TMQCAllocatedTo > 0)
                            {
                                context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == statusId).TMQCAllocatedTo = model.TMQCAllocatedTo;
                                context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == statusId).TMQCAllocatedDate = model.ModifiedDate;
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
        public void UpdateEmploymentInfoByTeamMember(PQUpdateEmploymentTeamMemberViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).CheckStatus = model.CheckStatus;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).ModifiedBy = model.ModifiedBy;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).ModifiedDate = model.ModifiedDate;
                            context.SaveChanges();

                            ClientCustomize cc = new ClientCustomize();
                            DataSet dd = new DataSet();
                            dd = cc.getInformationFromDepartmentRowID(model.EmploymentRowID, "Employment");
                            int personalRowid = 0;
                            int subCheckRowID = 0;
                            if (dd.Tables[0].Rows.Count > 0)
                            {
                                personalRowid = int.Parse((Convert.ToString(dd.Tables[0].Rows[0]["PersonalRowID"])));
                                subCheckRowID = int.Parse((Convert.ToString(dd.Tables[0].Rows[0]["SubCheckRowID"])));
                            }


                            #region Save Check Status

                            int statusId = context.PQEmploymentStatus.Where(p => p.EmploymentRowId == model.EmploymentRowID).FirstOrDefault().EmploymentStatusRowID;

                            context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == statusId).TMCheckStatus = model.CheckStatus;
                            context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == statusId).TMQCCheckStatus = "";
                            if (model.CheckStatus == "PV Raised")
                            {
                                context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == statusId).PVMgrCheckStatus = "Allocate-Mgr";
                                context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == statusId).PVMgrAllocatedTo = model.PVMgrAllocatedTo;
                                context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == statusId).PVMgrAllocatedDate = model.ModifiedDate;
                            }
                            if (model.CheckStatus == "Insufficent")
                            {
                                cc.UpdateCheckStatus(personalRowid, subCheckRowID, 1, 22, "Check", int.Parse(Convert.ToString(model.ModifiedBy)));

                                context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == statusId).TMQCCheckStatus = model.CheckStatus;
                                context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == statusId).InfSuffRaiseBy = model.ModifiedBy;
                                context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == statusId).InfSuffRaisedDate = model.ModifiedDate;
                                context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == statusId).InfSuffRaiseRemarks = model.ActionRemark;
                            }
                            if (model.CheckStatus == "Rejected")
                            {
                                cc.UpdateCheckStatus(personalRowid, subCheckRowID, 1, 14, "Check", int.Parse(Convert.ToString(model.ModifiedBy)));
                                context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == statusId).Remarks = model.ActionRemark;
                            }
                            if (model.CheckStatus == "Funds Pending")
                            {
                                cc.UpdateCheckStatus(personalRowid, subCheckRowID, 1, 26, "Check", int.Parse(Convert.ToString(model.ModifiedBy)));
                                context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == statusId).Remarks = model.ActionRemark;
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

        public void RejectionUpdateEmploymentInfoByQC(UpdateEmploymentRejectionViewModel model)
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
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).CheckStatus = ActualCheckStatus;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).ModifiedBy = model.ModifiedBy;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).ModifiedDate = model.ModifiedDate;
                            context.SaveChanges();

                            #region Update Check Status

                            int statusId = context.PQEmploymentStatus.Where(p => p.EmploymentRowId == model.EmploymentRowID).FirstOrDefault().EmploymentStatusRowID;
                            context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == statusId).DETMCheckStatus = ActualCheckStatus;
                            context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == statusId).DETMQCCheckStatus = "";
                            context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == statusId).Remarks = model.INFRemarks;
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

        public bool IfExistEmployment(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0, string CheckStatus = "")
        {
            try
            {
                var listChecks = db.PQEmployments.Where(a => a.ClientRowID == ClientRowId && a.PersonalRowID == PersonalRowID
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

        public bool IfExistEmploymentForDELeftMenu(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0)
        {
            try
            {
                var listChecks = db.PQEmployments.Where(a => a.ClientRowID == ClientRowId && a.PersonalRowID == PersonalRowID
                     && a.CheckFamilyRowID == ChFamilRowId && a.SubCheckRowID == subCheckId).FirstOrDefault();
                if (listChecks != null)
                {
                    var ChkStatus = db.PQEmploymentStatus.Where(s => s.EmploymentRowId == listChecks.EmploymentRowId).FirstOrDefault();

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

        public bool IfExistEmploymentForQCLeftMenu(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0, int CheckStatusid = 0)
        {
            try
            {


                if (CheckStatusid == 68 || CheckStatusid == 14 || CheckStatusid == 15 || CheckStatusid == 42 || CheckStatusid == 52)
                    return true;
                else
                    return false;

            }
            catch (Exception)
            {

                throw;
            }
        }

        //update Employment research table and Get More ids for check status and action history ang log save 
        public PQUpdateEmploymentResearcherViewModel UpdateEmploymentInfoByResearcher(PQUpdateEmploymentResearcherViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).CheckStatus = model.CheckStatus;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).ModifiedBy = model.ModifiedBy;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).ModifiedDate = model.ModifiedDate;
                            context.SaveChanges();
                        }

                        int StatusId = context.PQEmploymentStatus.Where(c => c.EmploymentRowId == model.EmploymentRowID).FirstOrDefault().EmploymentStatusRowID;

                        context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == StatusId).DETMCheckStatus = model.CheckStatus;
                        context.SaveChanges();

                        context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).CaseStatus = "WIP-Pending";
                        context.SaveChanges();

                        transaction.Commit();

                        //Get More ids for check status and action history ang log save  
                        var EmpResearch = db.PQEmployments.Where(c => c.EmploymentRowId == model.EmploymentRowID).FirstOrDefault();
                        model.PersonalRowID = EmpResearch.PersonalRowID;
                        model.SubCheckRowID = EmpResearch.SubCheckRowID;
                        model.UniqueComponentID = EmpResearch.UniqueComponentID;
                        return model;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
        }

        //update YTR Checks Employment table and Get More ids for check status and action history ang log save 
        public PQEmploymentYTRClearViewModel UpdateEmploymentYTRChecksByClient(PQEmploymentYTRClearViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).CheckStatus = model.CheckStatus;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_EmploymentPeriod = Convert.ToString(model.DOL);
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).ModifiedBy = model.ModifiedBy;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).ModifiedDate = model.ModifiedDate;
                            context.SaveChanges();
                        }

                        var StatusId = context.PQEmploymentStatus.Where(c => c.EmploymentRowId == model.EmploymentRowID).FirstOrDefault();

                        context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == StatusId.EmploymentStatusRowID).DETMCheckStatus = model.CheckStatus;
                        context.SaveChanges();

                        context.PQPersonals.Single(c => c.PersonalRowID == StatusId.PQEmployment.PersonalRowID).CaseStatus = "WIP-Pending";
                        context.SaveChanges();

                        transaction.Commit();

                        //Get More ids for check status and action history ang log save  
                        var EmpData = db.PQEmployments.Where(c => c.EmploymentRowId == model.EmploymentRowID).FirstOrDefault();
                        model.PersonalRowID = EmpData.PersonalRowID;
                        model.SubCheckRowID = EmpData.SubCheckRowID;
                        model.UniqueComponentID = EmpData.UniqueComponentID;
                        model.SecuritasRefID = EmpData.PQPersonal.CompanyRefNo;
                        return model;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
        }
        public PQEmploymentYTRClearViewModel UpdateRefrenceYTRChecksByClient(PQEmploymentYTRClearViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            context.PQReferences.Single(c => c.ReferenceRowID == model.EmploymentRowID).CheckStatus = model.CheckStatus;
                            context.PQReferences.Single(c => c.ReferenceRowID == model.EmploymentRowID).RV_OtherDetails10 = Convert.ToString(model.DOL);
                            context.PQReferences.Single(c => c.ReferenceRowID == model.EmploymentRowID).ModifiedBy = model.ModifiedBy;
                            context.PQReferences.Single(c => c.ReferenceRowID == model.EmploymentRowID).ModifiedDate = model.ModifiedDate;
                            context.SaveChanges();
                        }

                        var StatusId = context.PQReferenceStatus.Where(c => c.ReferenceRowID == model.EmploymentRowID).FirstOrDefault();

                        context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == StatusId.ReferenceStatusRowID).DETMCheckStatus = model.CheckStatus;
                        context.SaveChanges();

                        context.PQPersonals.Single(c => c.PersonalRowID == StatusId.PQReference.PersonalRowID).CaseStatus = "WIP-Pending";
                        context.SaveChanges();

                        transaction.Commit();

                        //Get More ids for check status and action history ang log save  
                        var EmpData = db.PQReferences.Where(c => c.ReferenceRowID == model.EmploymentRowID).FirstOrDefault();
                        model.PersonalRowID = EmpData.PersonalRowID;
                        model.SubCheckRowID = EmpData.SubCheckRowID;
                        model.UniqueComponentID = EmpData.UniqueComponentID;
                        model.SecuritasRefID = EmpData.PQPersonal.CompanyRefNo;
                        return model;
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }
        }

        public PQEmploymentListPagedModel GetAllYTRChecksListForDETL(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string YTRDate = "", string CheckStatus = "", int ClientUserRowID = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                //IQueryable<PQEmploymentStatus> data = db.PQEmploymentStatus.Include("PQEmployment").Where(p => p.DETMCheckStatus == "YTR");

                DataTable data = new DataTable();
                data = DAS.ExecuteDataTable(DAS.ConnectionString, CommandType.StoredProcedure, "YTRCheckList");



                PQEmploymentListPagedModel model = new PQEmploymentListPagedModel();
                model.PageSize = pageSize;
                model.TotalRecords = data.Rows.Count;
                DataTable resultDataTable = data.Rows.Cast<DataRow>().Skip((pageNo - 1) * pageSize).Take(pageSize).CopyToDataTable();

                List<PQEmploymentViewModel> YTRList = new List<PQEmploymentViewModel>();
                foreach (DataRow dr in resultDataTable.Rows)
                {
                    PQEmploymentViewModel emp = new PQEmploymentViewModel();
                    emp.EmploymentRowID = int.Parse(dr["EmploymentRowId"].ToString());
                    emp.ClientRowID = Int16.Parse(dr["ClientRowID"].ToString());
                    emp.ClientName = Convert.ToString(dr["ClientName"]);
                    emp.PersonalRowID = int.Parse(dr["PersonalRowID"].ToString());
                    emp.CandidateName = Convert.ToString(dr["CandidateFullName"]);
                    emp.SubCheckRowID = Int16.Parse(dr["SubCheckRowID"].ToString());
                    emp.SubCheckName = Convert.ToString(dr["SubCheckName"]);
                    emp.CompanyRefNo = Convert.ToString(dr["CompanyRefNo"]);
                    emp.CreatedDate = DateTime.Parse(dr["CreatedDate"].ToString());
                    emp.CheckStatus = "YTR";

                    YTRList.Add(emp);
                }
                model.PQEmployments = YTRList;
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PQEmploymentListPagedModel GetAllYTRChecksListForDETL1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string YTRDate = "", string CheckStatus = "", int ClientUserRowID = 0, int allClients = 0)
        {
            try
            {
                string CaseStatus = "YTR";
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataSet ds = new DataSet();
                PQEmploymentListPagedModel model = new PQEmploymentListPagedModel();
                if (!string.IsNullOrEmpty(Search))
                {
                    ds = cc.getCasewiseSummaryDetail(ClientRowID, CaseStatus, ClientUserRowID, Search, allClients);
                }
                else
                {
                    ds = cc.getCasewiseSummaryDetail(ClientRowID, CaseStatus, ClientUserRowID, "", allClients);
                }
                //ds = cc.getCasewiseSummaryDetail(ClientRowID, CaseStatus,ClientUserRowID);
                List<PQEmploymentViewModel> lmd = new List<PQEmploymentViewModel>();
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        lmd.Add(new PQEmploymentViewModel
                        {
                            PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]),
                            ClientRowID = Convert.ToInt16(dr["ClientRowID"]),
                            CandidateName = (dr["Candidate Name"]).ToString(),
                            // CandidateCode = (dr["CandidateCode"]).ToString(),
                            CompanyRefNo = (dr["Securitas ID"]).ToString(),
                            CaseStatus = (dr["Case Status"]).ToString(),
                            CreatedDate = Convert.ToDateTime(dr["Created On"]),
                            CheckStatus = (dr["Status"]).ToString(),
                            SubCheckName = (dr["SubCheckName"]).ToString(),
                            EmploymentRowID = Convert.ToInt32(dr["departmentrowid"]),
                            ClientRefID = (dr["ClientRefID"]).ToString(),
                            ReportVisibality = Convert.ToBoolean(dr["ReportVisibality"]),
                            IsReportUploaded = Convert.ToBoolean(dr["IsReportUploaded"]),
                            SubCheckRowID = Convert.ToInt16(dr["SubCheckFamilyID"]),
                            CheckFamilyRowID = Convert.ToInt16(dr["CheckFamilyRowID"]),
                            Entity = (dr["SSNNo"]).ToString(),
                            Package = (dr["CandidateType"]).ToString(),
                            Emailid = (dr["EmailID"]).ToString(),
                            MobileNo = (dr["MobileNo"]).ToString(),
                            CompanyName = (dr["Company Name"]).ToString(),
                            PackageName = dr.Table.Columns.Contains("PackageNamee") && dr["PackageNamee"] != DBNull.Value
                            ? dr["PackageNamee"].ToString()
                            : "",
                            HRRecruiterName = dr.Table.Columns.Contains("RecruiterName") && dr["RecruiterName"] != DBNull.Value
                            ? dr["RecruiterName"].ToString()
                            : ""


                        });

                    }


                    model.PageSize = pageSize;
                    model.TotalRecords = ds.Tables[0].Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PQEmployments = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                //model.PQEmployments = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new PQEmploymentViewModel
                //{
                //    EmploymentRowID = item.EmploymentRowId,
                //    ClientRowID = item.PQEmployment.ClientRowID,
                //    ClientName = item.PQEmployment.PQClientMaster.MasterAbbreviation.ClientName + " " + item.PQEmployment.PQClientMaster.MasterClientSubGroup.ClientSubGroupName,
                //    PersonalRowID = item.PQEmployment.PersonalRowID,
                //    CandidateName = item.PQEmployment.PQPersonal.CandTitle + " " + item.PQEmployment.PQPersonal.Name + " " + item.PQEmployment.PQPersonal.MiddleName + " " + item.PQEmployment.PQPersonal.LastName,
                //    SubCheckRowID = item.PQEmployment.SubCheckRowID,
                //    SubCheckName = item.PQEmployment.MasterSubCheckFamily.SubCheckName,
                //    CompanyRefNo = item.PQEmployment.EV_SecuritasReferenceID,
                //    CreatedDate = item.PQEmployment.CreatedDate,
                //    CheckStatus = item.PQEmployment.CheckStatus,
                //}).ToList();
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public PQEmploymentListPagedModel GetAllResultAwaitedChecksListForDETL1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string YTRDate = "", string CheckStatus = "", int ClientUserRowID = 0, int allClients = 0)
        {
            try
            {
                string CaseStatus = "ResultAwaited";
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataSet ds = new DataSet();
                PQEmploymentListPagedModel model = new PQEmploymentListPagedModel();
                if (!string.IsNullOrEmpty(Search))
                {
                    ds = cc.getCasewiseSummaryDetail(ClientRowID, CaseStatus, ClientUserRowID, Search, allClients);
                }
                else
                {
                    ds = cc.getCasewiseSummaryDetail(ClientRowID, CaseStatus, ClientUserRowID, "", allClients);
                }
                //ds = cc.getCasewiseSummaryDetail(ClientRowID, CaseStatus,ClientUserRowID);
                List<PQEmploymentViewModel> lmd = new List<PQEmploymentViewModel>();
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        lmd.Add(new PQEmploymentViewModel
                        {
                            PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]),
                            ClientRowID = Convert.ToInt16(dr["ClientRowID"]),
                            CandidateName = (dr["Candidate Name"]).ToString(),
                            // CandidateCode = (dr["CandidateCode"]).ToString(),
                            CompanyRefNo = (dr["Securitas ID"]).ToString(),
                            CaseStatus = (dr["Case Status"]).ToString(),
                            CreatedDate = Convert.ToDateTime(dr["Created On"]),
                            CheckStatus = (dr["Status"]).ToString(),
                            SubCheckName = (dr["SubCheckName"]).ToString(),
                            EmploymentRowID = Convert.ToInt32(dr["departmentrowid"]),
                            ClientRefID = (dr["ClientRefID"]).ToString(),
                            ReportVisibality = Convert.ToBoolean(dr["ReportVisibality"]),
                            IsReportUploaded = Convert.ToBoolean(dr["IsReportUploaded"]),
                            SubCheckRowID = Convert.ToInt16(dr["SubCheckFamilyID"]),
                            CheckFamilyRowID = Convert.ToInt16(dr["CheckFamilyRowID"]),
                            Entity = (dr["SSNNo"]).ToString(),
                            Package = (dr["CandidateType"]).ToString(),
                            Emailid = (dr["EmailID"]).ToString(),
                            MobileNo = (dr["MobileNo"]).ToString(),
                            CompanyName = (dr["Company Name"]).ToString()


                        });

                    }


                    model.PageSize = pageSize;
                    model.TotalRecords = ds.Tables[0].Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PQEmployments = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                //model.PQEmployments = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new PQEmploymentViewModel
                //{
                //    EmploymentRowID = item.EmploymentRowId,
                //    ClientRowID = item.PQEmployment.ClientRowID,
                //    ClientName = item.PQEmployment.PQClientMaster.MasterAbbreviation.ClientName + " " + item.PQEmployment.PQClientMaster.MasterClientSubGroup.ClientSubGroupName,
                //    PersonalRowID = item.PQEmployment.PersonalRowID,
                //    CandidateName = item.PQEmployment.PQPersonal.CandTitle + " " + item.PQEmployment.PQPersonal.Name + " " + item.PQEmployment.PQPersonal.MiddleName + " " + item.PQEmployment.PQPersonal.LastName,
                //    SubCheckRowID = item.PQEmployment.SubCheckRowID,
                //    SubCheckName = item.PQEmployment.MasterSubCheckFamily.SubCheckName,
                //    CompanyRefNo = item.PQEmployment.EV_SecuritasReferenceID,
                //    CreatedDate = item.PQEmployment.CreatedDate,
                //    CheckStatus = item.PQEmployment.CheckStatus,
                //}).ToList();
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PQEmploymentListPagedModel GetAllClientReviewChecksList(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string YTRDate = "", string CheckStatus = "", int ClientUserRowID = 0, int allClients = 0)
        {
            try
            {
                string CaseStatus = "ClientReview";
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataSet ds = new DataSet();
                PQEmploymentListPagedModel model = new PQEmploymentListPagedModel();
                if (!string.IsNullOrEmpty(Search))
                {
                    ds = cc.getCasewiseSummaryDetail(ClientRowID, CaseStatus, ClientUserRowID, Search, allClients);
                }
                else
                {
                    ds = cc.getCasewiseSummaryDetail(ClientRowID, CaseStatus, ClientUserRowID, "", allClients);
                }
                //ds = cc.getCasewiseSummaryDetail(ClientRowID, CaseStatus,ClientUserRowID);
                List<PQEmploymentViewModel> lmd = new List<PQEmploymentViewModel>();
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        lmd.Add(new PQEmploymentViewModel
                        {
                            PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]),
                            ClientRowID = Convert.ToInt16(dr["ClientRowID"]),
                            CandidateName = (dr["Candidate Name"]).ToString(),
                            // CandidateCode = (dr["CandidateCode"]).ToString(),
                            CompanyRefNo = (dr["Securitas ID"]).ToString(),
                            CaseStatus = (dr["Case Status"]).ToString(),
                            CreatedDate = Convert.ToDateTime(dr["Created On"]),
                            CheckStatus = (dr["Status"]).ToString(),
                            SubCheckName = (dr["SubCheckName"]).ToString(),
                            EmploymentRowID = Convert.ToInt32(dr["departmentrowid"]),
                            ClientRefID = (dr["ClientRefID"]).ToString(),
                            ReportVisibality = Convert.ToBoolean(dr["ReportVisibality"]),
                            IsReportUploaded = Convert.ToBoolean(dr["IsReportUploaded"]),
                            SubCheckRowID = Convert.ToInt16(dr["SubCheckFamilyID"]),
                            CheckFamilyRowID = Convert.ToInt16(dr["CheckFamilyRowID"]),
                            Entity = (dr["SSNNo"]).ToString(),
                            Package = (dr["CandidateType"]).ToString(),
                            Emailid = (dr["EmailID"]).ToString(),
                            MobileNo = (dr["MobileNo"]).ToString(),
                            CompanyName = (dr["Company Name"]).ToString()


                        });

                    }


                    model.PageSize = pageSize;
                    model.TotalRecords = ds.Tables[0].Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PQEmployments = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                //model.PQEmployments = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new PQEmploymentViewModel
                //{
                //    EmploymentRowID = item.EmploymentRowId,
                //    ClientRowID = item.PQEmployment.ClientRowID,
                //    ClientName = item.PQEmployment.PQClientMaster.MasterAbbreviation.ClientName + " " + item.PQEmployment.PQClientMaster.MasterClientSubGroup.ClientSubGroupName,
                //    PersonalRowID = item.PQEmployment.PersonalRowID,
                //    CandidateName = item.PQEmployment.PQPersonal.CandTitle + " " + item.PQEmployment.PQPersonal.Name + " " + item.PQEmployment.PQPersonal.MiddleName + " " + item.PQEmployment.PQPersonal.LastName,
                //    SubCheckRowID = item.PQEmployment.SubCheckRowID,
                //    SubCheckName = item.PQEmployment.MasterSubCheckFamily.SubCheckName,
                //    CompanyRefNo = item.PQEmployment.EV_SecuritasReferenceID,
                //    CreatedDate = item.PQEmployment.CreatedDate,
                //    CheckStatus = item.PQEmployment.CheckStatus,
                //}).ToList();
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }


        #region ***************** Temp Employment *****************

        public void AddTempEmploymentInformation(AddTempEmploymentViewModel model)
        {
            try
            {
                if (model != null)
                {
                    TempPQEmployment entity = new TempPQEmployment();
                    entity.ClientRowID = model.ClientRowID;
                    entity.PersonalRowID = model.PersonalRowID;
                    entity.ClientPackageRowID = model.ClientPackageRowID;
                    entity.CheckFamilyRowID = model.CheckFamilyRowID;
                    entity.SubCheckRowID = model.SubCheckRowID;
                    entity.UniqueComponentID = model.UniqueComponentID;
                    //entity.EV_CandidateName = model.EV_CandidateName;
                    if (model.EV_CandidateName == null)
                    {
                        entity.EV_CandidateName = db.PQCandidateLogins.Where(w => w.TempPersonalRowID == model.PersonalRowID).FirstOrDefault().CandidateName;
                    }
                    else
                    {
                        entity.EV_CandidateName = model.EV_CandidateName;
                    }
                    // entity.EV_SecuritasReferenceID = model.EV_SecuritasReferenceID;
                    entity.EV_OrganisationName = model.EV_OrganisationName;
                    entity.EV_CmpLocation = model.EV_CmpLocation;
                    entity.EV_CmpNameAddress = model.EV_CmpNameAddress;
                    entity.EV_EmployeeName = model.EV_EmployeeName;
                    entity.EV_EmployeeCode = model.EV_EmployeeCode;
                    entity.EV_EmploymentPeriod = model.EV_EmploymentPeriod;
                    entity.EV_CandidateTenureWorked = model.EV_CandidateTenureWorked;
                    entity.EV_EmployeeTenure = model.EV_EmployeeTenure;
                    entity.EV_Designation = model.EV_Designation;
                    entity.EV_EmpType = model.EV_EmpType;
                    entity.EV_Department = model.EV_Department;
                    entity.EV_CmpCost = model.EV_CmpCost;
                    entity.EV_last_sal = model.EV_last_sal;
                    entity.EV_leavingReason = model.EV_leavingReason;
                    entity.EV_DiscontinueServiceReason = model.EV_DiscontinueServiceReason;
                    entity.EV_ReportingManagerName = model.EV_ReportingManagerName;
                    entity.EV_Rptng_Mgr_Name_Ph = model.EV_Rptng_Mgr_Name_Ph;
                    entity.EV_Rptng_Mgr_Name_ConDet = model.EV_Rptng_Mgr_Name_ConDet;
                    entity.EV_Sub_Rptng_Mgr_Name_Dsg_EidPh = model.EV_Sub_Rptng_Mgr_Name_Dsg_EidPh;
                    entity.EV_Supervisor = model.EV_Supervisor;
                    entity.EV_Sup_Name_Dsg = model.EV_Sup_Name_Dsg;
                    entity.EV_FormalitiesExit = model.EV_FormalitiesExit;
                    entity.EV_FullFinalFormalities = model.EV_FullFinalFormalities;
                    entity.EV_ExitFormalitiesPending = model.EV_ExitFormalitiesPending;
                    entity.EV_HandledResponsibilities = model.EV_HandledResponsibilities;
                    entity.EV_ResponsibilitiesHandled = model.EV_ResponsibilitiesHandled;
                    //entity.EV_ExprLetter = model.EV_ExprLetter;
                    //entity.EV_RelievLetter = model.EV_RelievLetter;
                    //entity.EV_OfferLetter = model.EV_OfferLetter;
                    //entity.EV_AppointLetter = model.EV_AppointLetter;
                    //entity.EV_OtherProof = model.EV_OtherProof;
                    entity.EV_OtherDetails = model.EV_OtherDetails;
                    entity.EV_OtherDetails2 = model.EV_OtherDetails2;
                    entity.EV_OtherDetails3 = model.EV_OtherDetails3;
                    entity.EV_OtherDetails4 = model.EV_OtherDetails4;
                    entity.EV_OtherDetails5 = model.EV_OtherDetails5;
                    entity.EV_OtherDetails6 = model.EV_OtherDetails6;
                    entity.EV_OtherDetails7 = model.EV_OtherDetails7;
                    entity.EV_OtherDetails8 = model.EV_OtherDetails8;
                    entity.EV_OtherDetails9 = model.EV_OtherDetails9;
                    entity.EV_OtherDetails10 = model.EV_OtherDetails10;
                    entity.ATA_CID_No = model.ATA_CID_No;
                    entity.ATA_Cmpny_Addr = model.ATA_Cmpny_Addr;
                    entity.CreatedBy = model.CreatedBy;
                    entity.CreatedDate = model.CreatedDate;
                    entity.CheckStatus = model.CheckStatus;
                    entity.Remarks = model.Remarks;
                    entity.Status = model.Status;
                    // For TATA Elxsi 
                    if (model.ClientRowID == 635)
                    {
                        if (model.EV_Date_Joining != null)
                        {
                            entity.EV_EmploymentPeriod = model.EV_Date_Joining.ToString();
                        }

                        if (model.EV_Date_Leaving != null)
                        {
                            entity.EV_EmpType = model.EV_Date_Leaving.ToString();
                        }
                    }

                    db.TempPQEmployments.Add(entity);
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public IEnumerable<EmploymentDDLViewModel> OrganizationNames(string id)
        {
            try
            {
                return db.MasterEmployers.Where(c => c.Status == 1 && c.CompanyName.Contains(id)).Take(5).Select(c => new EmploymentDDLViewModel { EmploymentRowID = c.EmployerRowID, EmploymentName = c.CompanyName }).ToList().OrderBy(c => c.EmploymentName);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public AddTempEmploymentViewModel GetTempEmploymentInfoBySelf(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0)
        {
            try
            {
                string EV_OrganisationName = "0";
                string OrgLabel = "";
                int orgId = 0;
                EV_OrganisationName = db.TempPQEmployments.Where(a => a.ClientRowID == ClientRowId && a.PersonalRowID == PersonalRowID
                && a.CheckFamilyRowID == ChFamilRowId && a.SubCheckRowID == subCheckId).Select(a => a.EV_OrganisationName).FirstOrDefault();
                if (!string.IsNullOrEmpty(EV_OrganisationName))
                {
                    orgId = int.Parse(EV_OrganisationName);
                    if (EV_OrganisationName != "0")
                    {
                        OrgLabel = db.MasterEmployers.Where(M => M.EmployerRowID == orgId).Select(K => K.CompanyName).FirstOrDefault();
                    }
                }


                return db.TempPQEmployments.Where(a => a.ClientRowID == ClientRowId && a.PersonalRowID == PersonalRowID
                && a.CheckFamilyRowID == ChFamilRowId && a.SubCheckRowID == subCheckId).Select(a => new AddTempEmploymentViewModel
                {
                    EmploymentRowID = a.EmploymentRowId,
                    ClientRowID = a.ClientRowID,
                    PersonalRowID = a.PersonalRowID,
                    ClientPackageRowID = a.ClientPackageRowID,
                    CheckFamilyRowID = a.CheckFamilyRowID,
                    SubCheckRowID = a.SubCheckRowID,
                    UniqueComponentID = a.UniqueComponentID,
                    EV_OrganisationName = a.EV_OrganisationName,
                    EV_CmpLocation = a.EV_CmpLocation,
                    EV_CmpNameAddress = a.EV_CmpNameAddress,
                    EV_EmployeeName = a.EV_EmployeeName,
                    EV_EmployeeCode = a.EV_EmployeeCode,
                    EV_EmploymentPeriod = a.EV_EmploymentPeriod,
                    EV_CandidateTenureWorked = a.EV_CandidateTenureWorked,
                    EV_EmployeeTenure = a.EV_EmployeeTenure,
                    EV_Designation = a.EV_Designation,
                    EV_EmpType = a.EV_EmpType,
                    EV_Department = a.EV_Department,
                    EV_CmpCost = a.EV_CmpCost,
                    EV_last_sal = a.EV_last_sal,
                    EV_leavingReason = a.EV_leavingReason,
                    EV_DiscontinueServiceReason = a.EV_DiscontinueServiceReason,
                    EV_ReportingManagerName = a.EV_ReportingManagerName,
                    EV_Rptng_Mgr_Name_Ph = a.EV_Rptng_Mgr_Name_Ph,
                    EV_Rptng_Mgr_Name_ConDet = a.EV_Rptng_Mgr_Name_ConDet,
                    EV_Sub_Rptng_Mgr_Name_Dsg_EidPh = a.EV_Sub_Rptng_Mgr_Name_Dsg_EidPh,
                    EV_Supervisor = a.EV_Supervisor,
                    EV_Sup_Name_Dsg = a.EV_Sup_Name_Dsg,
                    EV_FormalitiesExit = a.EV_FormalitiesExit,
                    EV_FullFinalFormalities = a.EV_FullFinalFormalities,
                    EV_ExitFormalitiesPending = a.EV_ExitFormalitiesPending,
                    EV_HandledResponsibilities = a.EV_HandledResponsibilities,
                    EV_ResponsibilitiesHandled = a.EV_ResponsibilitiesHandled,
                    //EV_ExprLetter = a.EV_ExprLetter,
                    //EV_RelievLetter = a.EV_RelievLetter,
                    //EV_OfferLetter = a.EV_OfferLetter,
                    //EV_AppointLetter = a.EV_AppointLetter,
                    //EV_OtherProof = a.EV_OtherProof,
                    EV_OtherDetails = a.EV_OtherDetails,
                    EV_OtherDetails2 = a.EV_OtherDetails2,
                    EV_OtherDetails3 = a.EV_OtherDetails3,
                    EV_OtherDetails4 = a.EV_OtherDetails4,
                    EV_OtherDetails5 = a.EV_OtherDetails5,
                    EV_OtherDetails6 = a.EV_OtherDetails6,
                    EV_OtherDetails7 = a.EV_OtherDetails7,
                    EV_OtherDetails8 = a.EV_OtherDetails8,
                    EV_OtherDetails9 = a.EV_OtherDetails9,
                    EV_OtherDetails10 = a.EV_OtherDetails10,
                    ATA_CID_No = a.ATA_CID_No,
                    ATA_Cmpny_Addr = a.ATA_Cmpny_Addr,
                    Remarks = a.Remarks,
                    CheckStatus = a.CheckStatus,
                    EV_CandidateName = a.EV_CandidateName,
                    OrgNameLabel = OrgLabel,
                }).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public int GetAllTempEmploymentByClientId(short ClientRowID = 0)
        {
            try
            {
                var data = db.TempPQEmployments.Where(p => p.ClientRowID == ClientRowID).ToList();
                return data.Count();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void UpdateTempEmploymentInfoByDataEntry(AddTempEmploymentViewModel model)
        {
            try
            {
                if (model != null)
                {
                    db.TempPQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_OrganisationName = model.EV_OrganisationName;
                    db.TempPQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_CmpLocation = model.EV_CmpLocation;
                    db.TempPQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_CmpNameAddress = model.EV_CmpNameAddress;
                    db.TempPQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_EmployeeName = model.EV_EmployeeName;
                    db.TempPQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_EmployeeCode = model.EV_EmployeeCode;
                    db.TempPQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_EmploymentPeriod = model.EV_EmploymentPeriod;
                    db.TempPQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_CandidateTenureWorked = model.EV_CandidateTenureWorked;
                    db.TempPQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_EmployeeTenure = model.EV_EmployeeTenure;
                    db.TempPQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_Designation = model.EV_Designation;
                    db.TempPQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_EmpType = model.EV_EmpType;
                    db.TempPQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_Department = model.EV_Department;
                    db.TempPQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_CmpCost = model.EV_CmpCost;
                    db.TempPQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_last_sal = model.EV_last_sal;
                    db.TempPQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_leavingReason = model.EV_leavingReason;
                    db.TempPQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_DiscontinueServiceReason = model.EV_DiscontinueServiceReason;
                    db.TempPQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_ReportingManagerName = model.EV_ReportingManagerName;
                    db.TempPQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_Rptng_Mgr_Name_Ph = model.EV_Rptng_Mgr_Name_Ph;
                    db.TempPQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_Rptng_Mgr_Name_ConDet = model.EV_Rptng_Mgr_Name_ConDet;
                    db.TempPQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_Sub_Rptng_Mgr_Name_Dsg_EidPh = model.EV_Sub_Rptng_Mgr_Name_Dsg_EidPh;
                    db.TempPQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_Supervisor = model.EV_Supervisor;
                    db.TempPQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_Sup_Name_Dsg = model.EV_Sup_Name_Dsg;
                    db.TempPQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_FormalitiesExit = model.EV_FormalitiesExit;
                    db.TempPQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_FullFinalFormalities = model.EV_FullFinalFormalities;
                    db.TempPQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_ExitFormalitiesPending = model.EV_ExitFormalitiesPending;
                    db.TempPQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_HandledResponsibilities = model.EV_HandledResponsibilities;
                    db.TempPQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_ResponsibilitiesHandled = model.EV_ResponsibilitiesHandled;
                    db.TempPQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_OtherDetails = model.EV_OtherDetails;
                    db.TempPQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_OtherDetails2 = model.EV_OtherDetails2;
                    db.TempPQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_OtherDetails3 = model.EV_OtherDetails3;
                    db.TempPQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_OtherDetails4 = model.EV_OtherDetails4;
                    db.TempPQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_OtherDetails5 = model.EV_OtherDetails5;
                    db.TempPQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_OtherDetails6 = model.EV_OtherDetails6;
                    db.TempPQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_OtherDetails7 = model.EV_OtherDetails7;
                    db.TempPQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_OtherDetails8 = model.EV_OtherDetails8;
                    db.TempPQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_OtherDetails9 = model.EV_OtherDetails9;
                    db.TempPQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_OtherDetails10 = model.EV_OtherDetails10;
                    db.TempPQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).ATA_CID_No = model.ATA_CID_No;
                    db.TempPQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).ATA_Cmpny_Addr = model.ATA_Cmpny_Addr;
                    //db.TempPQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).CheckStatus = model.CheckStatus;
                    db.TempPQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).ModifiedBy = model.ModifiedBy;
                    db.TempPQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).ModifiedDate = model.ModifiedDate;
                    db.TempPQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).CheckStatus = model.CheckStatus;
                    db.TempPQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).Remarks = model.Remarks;
                    // For TATA Elxsi 
                    if (model.ClientRowID == 635)
                    {
                        if (model.EV_Date_Joining != null)
                        {
                            db.TempPQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_EmploymentPeriod = model.EV_Date_Joining.ToString();
                        }

                        if (model.EV_Date_Leaving != null)
                        {
                            db.TempPQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).EV_EmpType = model.EV_Date_Leaving.ToString();
                        }
                    }

                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        public PQEmploymentInsuffListPagedModel GetAllEmploymentInsuffListForDETL(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", int PersonalId = 0, string spocEmail = "", short TeamMemberRowid = 0, int ClientUserRowID = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                IQueryable<PQEmploymentStatus> data = db.PQEmploymentStatus.Include("PQEmployment").Where(p => (p.DETMQCCheckStatus == "Insufficent-QC" || p.TMQCCheckStatus == "Insufficent-QC") && p.InfSuffClearBy == 0);

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
                    data = data.Where(b => b.PQEmployment.PQPersonal.ClientUserRowID == ClientUserRowID);
                }

                if (PersonalId == 0)
                {
                    if (ClientRowID > 0)
                    {
                        data = data.Where(b => b.PQEmployment.ClientRowID == ClientRowID);
                    }

                    if (!string.IsNullOrEmpty(spocEmail))
                    {
                        var Clients = db.PQClientMasters.Where(s => s.SpocEmailID.Contains(spocEmail)).Select(s => s.ClientRowID).ToList();
                        data = data.Where(a => Clients.Contains(a.PQEmployment.ClientRowID));
                    }

                    if (!string.IsNullOrEmpty(RecievingToDate) && !string.IsNullOrEmpty(RecievingFromDate))
                    {
                        DateTime startDate = Convert.ToDateTime(RecievingFromDate);
                        DateTime EndDate = Convert.ToDateTime(RecievingToDate);

                        data = data.Where(b => DbFunctions.TruncateTime(b.PQEmployment.PQPersonal.OrderDate) >= startDate.Date && DbFunctions.TruncateTime(b.PQEmployment.PQPersonal.OrderDate) <= EndDate.Date);
                    }

                    if (!string.IsNullOrEmpty(CompletedToDate) && !string.IsNullOrEmpty(CompletedFromDate))
                    {
                        DateTime startDate = Convert.ToDateTime(CompletedFromDate);
                        DateTime EndDate = Convert.ToDateTime(CompletedToDate);

                        data = data.Where(b => DbFunctions.TruncateTime(b.PQEmployment.PQPersonal.OrderDate) >= startDate.Date && DbFunctions.TruncateTime(b.PQEmployment.PQPersonal.OrderDate) <= EndDate.Date);
                    }

                    if (!string.IsNullOrEmpty(Search))
                    {
                        data = data.Where(b => (b.PQEmployment.PQPersonal.Name + " " + b.PQEmployment.PQPersonal.MiddleName + " " + b.PQEmployment.PQPersonal.LastName).ToString().Replace(" ", "").Contains(Search.Replace(" ", "")) || b.PQEmployment.PQPersonal.ClientRefID == Search);
                    }
                }
                else
                {
                    data = data.Where(b => b.PQEmployment.PersonalRowID == PersonalId);
                }

                switch (sort)
                {
                    default:
                        data = sortDir == "asc" ? data.OrderByDescending(d => d.EmploymentStatusRowID) : data.OrderBy(d => d.EmploymentStatusRowID);
                        break;
                }

                PQEmploymentInsuffListPagedModel model = new PQEmploymentInsuffListPagedModel();
                model.PageSize = pageSize;
                model.TotalRecords = data.Count();
                model.PQAllInsuffEmployments = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new PQEmploymentInsuffViewModel
                {
                    EmploymentRowID = item.EmploymentRowId,
                    ClientRowID = item.PQEmployment.ClientRowID,
                    ClientName = item.PQEmployment.PQClientMaster.MasterAbbreviation.ClientName + " " + item.PQEmployment.PQClientMaster.MasterClientSubGroup.ClientSubGroupName,
                    PersonalRowID = item.PQEmployment.PersonalRowID,
                    CandidateName = item.PQEmployment.PQPersonal.CandTitle + " " + item.PQEmployment.PQPersonal.Name + " " + item.PQEmployment.PQPersonal.MiddleName + " " + item.PQEmployment.PQPersonal.LastName,
                    SubCheckRowID = item.PQEmployment.SubCheckRowID,
                    CheckFamilyRowID = item.PQEmployment.CheckFamilyRowID,
                    SubCheckName = item.PQEmployment.MasterSubCheckFamily.SubCheckName,
                    CompanyRefNo = item.PQEmployment.EV_SecuritasReferenceID,
                    InfSuffRaiseBy = db.TeamMembers.Where(t => t.TeamMemberRowID == item.InfSuffRaiseBy).Select(t => t.TMFirstName + " " + t.TMLastName).FirstOrDefault(),
                    InfSuffRaisedDate = item.InfSuffRaisedDate,
                    INFRemarks = item.InfSuffRaiseRemarks,
                    CreatedDate = item.PQEmployment.CreatedDate,
                    CheckStatus = item.PQEmployment.CheckStatus,
                    ReWorkCheckStatus = item.PQEmployment.ReWorkCheckStatus,
                }).ToList();
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }



        public PQEmploymentInsuffListPagedModel GetAllEmploymentInsuffListForDETL1(int pageNo, int pageSize, string sort, string sortDir, string subcheckType, string Search = "", short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", int PersonalId = 0, string spocEmail = "", short TeamMemberRowid = 0, int ClientUserRowID = 0, int allclints = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }
                DataSet ds = new DataSet();
                PQEmploymentInsuffListPagedModel model = new PQEmploymentInsuffListPagedModel();
                ds = cc.getCheckwiseinsuffDetailSummary(ClientRowID, ClientUserRowID, subcheckType, Search, allclints);
                List<PQEmploymentInsuffViewModel> lmd = new List<PQEmploymentInsuffViewModel>();
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        lmd.Add(new PQEmploymentInsuffViewModel
                        {
                            PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]),
                            EmploymentRowID = Convert.ToInt32(dr["EmploymentRowID"]),
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
                            CompanyName = (dr["CompanyName"]).ToString(),
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
                model.PQAllInsuffEmployments = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PQEmploymentInsuffListPagedModel GetAllEmploymentInsuffListForCSSpoc(int pageNo, int pageSize, string sort, string sortDir, string subcheckType, string Search = "", short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", int PersonalId = 0, string spocEmail = "", short TeamMemberRowid = 0, int ClientUserRowID = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }
                DataSet ds = new DataSet();
                PQEmploymentInsuffListPagedModel model = new PQEmploymentInsuffListPagedModel();
                ds = cc.getinsuffDetails_CSSpoc(subcheckType, spocEmail, ClientRowID, Search);
                List<PQEmploymentInsuffViewModel> lmd = new List<PQEmploymentInsuffViewModel>();
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        PQEmploymentInsuffViewModel nn = new PQEmploymentInsuffViewModel();
                        //lmd.Add(new PQEducationInsuffViewModel
                        //{
                        nn.PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]);
                        nn.EmploymentRowID = Convert.ToInt32(dr["VerificationRowId"]);
                        nn.ClientRowID = Convert.ToInt16(dr["ClientRowID"]);
                        nn.CandidateName = (dr["CandidateName"]).ToString();
                        //ClientName = (dr["CandidateCode"]).ToString(),
                        nn.CompanyRefNo = (dr["CompanyRefNo"]).ToString();
                        nn.InsuffType = (dr["InsuffType"]).ToString();

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
                model.PQAllInsuffEmployments = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public UpdatePQEmploymentInsuffClearViewModel GetPQEmploymentDetailsById(int EmploymentRowID = 0)
        {
            try
            {
                UpdatePQEmploymentInsuffClearViewModel model = new UpdatePQEmploymentInsuffClearViewModel();
                //var entity = db.PQEmployments.Find(EmploymentRowID);

                var entity = db.PQEmploymentStatus.Where(e => e.EmploymentRowId == EmploymentRowID).FirstOrDefault();
                if (entity != null)
                {
                    model.EmploymentRowID = entity.EmploymentRowId;
                    model.ClientRowID = entity.PQEmployment.ClientRowID;
                    model.PersonalRowID = entity.PQEmployment.PersonalRowID;
                    model.SubCheckRowID = entity.PQEmployment.SubCheckRowID;
                    model.UniqueComponentID = entity.PQEmployment.UniqueComponentID;
                    model.INFRemarks = entity.InfSuffClearComments;
                    model.CheckStatus = entity.PQEmployment.CheckStatus;
                    model.ReWorkCheckStatus = entity.PQEmployment.ReWorkCheckStatus;
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

        public void UpdatePQEmploymentInsuffClear(UpdatePQEmploymentInsuffClearViewModel model)
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

                            var PQEmploymentStatusId = context.PQEmploymentStatus.Where(c => c.EmploymentRowId == model.EmploymentRowID).FirstOrDefault().EmploymentStatusRowID;

                            context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == PQEmploymentStatusId).DETMCheckStatus = ScnCheckStatus;
                            context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == PQEmploymentStatusId).InfSuffClearBy = model.InfSuffClearBy;
                            context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == PQEmploymentStatusId).InfSuffClearDate = model.InfSuffClearDate;
                            context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == PQEmploymentStatusId).InfSuffClearComments = model.INFRemarks;
                            context.SaveChanges();

                            //Check Status Update
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).CheckStatus = ScnCheckStatus;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).ModifiedBy = model.ModifiedBy;
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).ModifiedDate = model.ModifiedDate;
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

        public void ViewEmploymentInfoByDataEntry(ViewEmploymentViewModel model, bool flag = false)
        {

            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).CheckStatus = model.CheckStatus;
                            context.SaveChanges();

                            int statusId = context.PQEmploymentStatus.Where(p => p.EmploymentRowId == model.EmploymentRowID).FirstOrDefault().EmploymentStatusRowID;

                            if (flag)
                            {
                                context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).DECaseStatus = "WIP";
                                context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).AllocatedToDEDate = DateTime.Now;
                                context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).DEQCCaseStatus = "WIP";
                                context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).AllocatedToDEQCDate = DateTime.Now;
                                context.SaveChanges();

                                context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == statusId).DETMQCCheckStatus = "";
                                context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == statusId).DETMCheckStatus = model.CheckStatus;
                            }
                            else
                            {
                                context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == statusId).DETMQCCheckStatus = "Completed-QC";
                                context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == statusId).TMCheckStatus = model.CheckStatus;
                            }
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

        public void ViewEmploymentInfoByVrifierTL(ViewEmploymentViewModel model)
        {

            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).CheckStatus = model.CheckStatus;
                            context.SaveChanges();

                            int statusId = context.PQEmploymentStatus.Where(p => p.EmploymentRowId == model.EmploymentRowID).FirstOrDefault().EmploymentStatusRowID;

                            context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == statusId).TMCheckStatus = model.CheckStatus;
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

        public void ViewEmploymentInfoDETL(ViewEmploymentViewModel model)
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

                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).CheckStatus = model.CheckStatus;
                            context.SaveChanges();

                            context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).DECaseStatus = "WIP";
                            context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).AllocatedToDEDate = DateTime.Now;
                            context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).DEQCCaseStatus = "WIP";
                            context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).AllocatedToDEQCDate = DateTime.Now;
                            context.SaveChanges();

                            int statusId = context.PQEmploymentStatus.Where(p => p.EmploymentRowId == model.EmploymentRowID).FirstOrDefault().EmploymentStatusRowID;

                            context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == statusId).DETMCheckStatus = model.CheckStatus;
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

        public void UpdateEmploymentRejectionByDEQC(ViewEmploymentViewModel model)
        {

            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).CheckStatus = model.CheckStatus;
                            context.SaveChanges();

                            int statusId = context.PQEmploymentStatus.Where(p => p.EmploymentRowId == model.EmploymentRowID).FirstOrDefault().EmploymentStatusRowID;

                            context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == statusId).DETMQCCheckStatus = "Completed-QC";
                            context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == statusId).TMCheckStatus = model.CheckStatus;
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
