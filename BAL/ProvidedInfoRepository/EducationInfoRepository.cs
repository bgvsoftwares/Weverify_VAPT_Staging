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
    public class EducationInfoRepository : IEducationInfoRepository
    {
        DataContext db;
        ClientCustomize cc ;
        public EducationInfoRepository()
        {
            db = new DataContext();
            cc = new ClientCustomize();
        }

        public void AddEducationInformation(AddPQEducationViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            PQEducation entity = new PQEducation();
                            entity.ClientRowID = model.ClientRowID;
                            entity.PersonalRowID = model.PersonalRowID;
                            entity.ClientPackageRowID = model.ClientPackageRowID;
                            entity.CheckFamilyRowID = model.CheckFamilyRowID;
                            entity.SubCheckRowID = model.SubCheckRowID;
                            entity.UniqueComponentID = model.UniqueComponentID;
                            entity.EV_Cand_Name = model.EV_Cand_Name;
                            entity.EV_Sec_Ref_Id = model.EV_Sec_Ref_Id;
                            entity.EV_Cand_Name_Supp_Edu_Doc = model.EV_Cand_Name_Supp_Edu_Doc;
                            entity.EV_Colg_Schl_Inst_Name = model.EV_Colg_Schl_Inst_Name;
                            entity.EV_Colg_Schl_Inst_Add_ConDet = model.EV_Colg_Schl_Inst_Add_ConDet;
                            entity.EV_Affl_University_Name = model.EV_Affl_University_Name;
                            entity.EV_Degree_Name = model.EV_Degree_Name;
                            entity.EV_Mode_Of_Edu = model.EV_Mode_Of_Edu;
                            entity.EV_Enroll_RollNo = model.EV_Enroll_RollNo;
                            entity.EV_RegNo_RegCode = model.EV_RegNo_RegCode;
                            entity.EV_Dur_Course = model.EV_Dur_Course;
                            entity.EV_Course_Status = model.EV_Course_Status;
                            entity.EV_Specialization = model.EV_Specialization;
                            entity.EV_Period_Course = model.EV_Period_Course;
                            entity.EV_Yr_Passing = model.EV_Yr_Passing;
                            entity.EV_Division_CGPA = model.EV_Division_CGPA;
                            entity.EV_Others = model.EV_Others;
                            entity.EV_Others2 = model.EV_Others2;
                            entity.EV_Others3 = model.EV_Others3;
                            entity.EV_Others4 = model.EV_Others4;
                            entity.EV_Others5 = model.EV_Others5;
                            entity.EV_Others6 = model.EV_Others6;
                            entity.EV_Others7 = model.EV_Others7;
                            entity.EV_Others8 = model.EV_Others8;
                            entity.EV_Others9 = model.EV_Others9;
                            entity.EV_Others10 = model.EV_Others10;
                            entity.ATA_CID_No = model.ATA_CID_No;
                            entity.ATA_Cmpny_Addr = model.ATA_Cmpny_Addr;
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

                            context.PQEducations.Add(entity);
                            context.SaveChanges();

                            #region Save Check Status

                            PQEducationStatus chkEntity = new PQEducationStatus();
                            var eduModel = context.PQEducations.Include("PQPersonal").Where(p => p.EducationRowId == entity.EducationRowId).FirstOrDefault();
                            chkEntity.EducationRowId = eduModel.EducationRowId;
                            chkEntity.DETLAllocatedTo = eduModel.PQPersonal.AllocatedBy;
                            chkEntity.DETMCheckStatus = model.CheckStatus;
                            chkEntity.DETMAllocatedTo = eduModel.PQPersonal.AllocatedToDE;
                            chkEntity.DETMAllocatedDate = eduModel.PQPersonal.AllocatedToDEDate;
                            chkEntity.DETMQCAllocatedTo = eduModel.PQPersonal.AllocatedToDEQC;
                            chkEntity.DETMQCAllocatedDate = eduModel.PQPersonal.AllocatedToDEQCDate;

                            if (model.CheckStatus == "Completed")
                            {
                                chkEntity.DETMQCCheckStatus = model.CheckStatus;
                            }
                            if (model.CheckStatus == "Insufficent")
                            {
                                chkEntity.DETMQCCheckStatus = model.CheckStatus;
                                chkEntity.InfSuffRaiseBy = eduModel.PQPersonal.AllocatedToDE;
                                chkEntity.InfSuffRaisedDate = eduModel.CreatedDate;
                                chkEntity.InfSuffRaiseRemarks = model.INFRemarks;
                            }
                            if (model.CheckStatus == "Rejected" || model.CheckStatus == "Review Requested")
                            {
                                chkEntity.Remarks = model.INFRemarks;
                            }
                            context.PQEducationStatus.Add(chkEntity);
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

        public int GetAllEducationsByClientId(short ClientRowID = 0)
        {
            try
            {
                var data = db.PQEducations.Where(p => p.ClientRowID == ClientRowID).ToList();
                return data.Count();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetEducationIDByUniqueComponentID(int PersonalRowID = 0, string UniqueComponentID = "")
        {
            try
            {
                return db.PQEducations.Where(p => p.PersonalRowID == PersonalRowID && p.UniqueComponentID == UniqueComponentID).FirstOrDefault().EducationRowId;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public CollegeAddressAndAffiliated GetAddressAndAffiliated(int CollegeRowID)
        {
            try
            {
                try
                {
                    CollegeAddressAndAffiliated model = new CollegeAddressAndAffiliated();
                    var entity = db.MasterColleges.Find(CollegeRowID);
                    
                    if (entity != null)
                    {
                        model.Address = entity.CollegeAddress;
                        model.Affiliated = entity.AffiliatedFrom;
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
            catch (Exception)
            {

                throw;
            }
        }

        public CollegeAddressAndAffiliated GetAddressAndAffiliated_New(int CollegeRowID)
        {
            try
            {
                try
                {
                    CollegeAddressAndAffiliated model = new CollegeAddressAndAffiliated();
                    DataTable dt = new DataTable();
                    dt = cc.GetUniversityName(CollegeRowID);
                    if (dt.Rows.Count>0)
                    {
                        model.Address = dt.Rows[0]["CollegeAddress"].ToString();
                        model.Affiliated = dt.Rows[0]["AffiliatedFrom"].ToString();
                        model.UniversityName = dt.Rows[0]["UniversityName"].ToString();
                    }
                    else
                    {
                        throw new Exception("Invalid Id!");
                    }
                    //var entity = db.MasterColleges.Find(CollegeRowID);

                    //if (entity != null)
                    //{
                    //    model.Address = entity.CollegeAddress;
                    //    model.Affiliated = entity.AffiliatedFrom;
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
            catch (Exception)
            {

                throw;
            }
        }



        public CollegeAddressAndAffiliated GetUniversityName(short CollegeRowID)
        {
            try
            {
                try
                {
                    CollegeAddressAndAffiliated model = new CollegeAddressAndAffiliated();
                    
                    DataTable dt = cc.GetUniversityName(CollegeRowID);
                    if (dt.Rows.Count > 0)
                    {

                        model.UniversityName = dt.Rows[0]["UniversityName"].ToString();



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
            catch (Exception)
            {

                throw;
            }
        }
        //public IEnumerable<EducationList> GetEducationList(short ClientRowId = 0, short ChFamilRowId = 0, short subCheckId = 0)
        //{
        //    try
        //    {
        //        var listEducation = db.PQClientFields.Where
        //          (
        //              a => a.ClientRowID == ClientRowId
        //              && a.CheckFamilyRowID == ChFamilRowId
        //              && a.SubCheckRowID == subCheckId
        //          ).Select(c => new EducationList
        //          {
        //              AntecedentName = !string.IsNullOrEmpty(c.CADisplayName) ? c.CADisplayName : c.MasterAntecedent.DisplayName,
        //              FieldName = c.MasterAntecedent.FieldName,
        //              AntecedentRowId = c.AntecedentRowId,
        //              AntecedentTypeRowId = c.MasterAntecedent.AntecedentTypeRowId
        //          })
        //          .ToList().Where(a => a.AntecedentTypeRowId == 1 || a.AntecedentTypeRowId == 3 || a.AntecedentTypeRowId == 4).DefaultIfEmpty();
        //        return listEducation;
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        public AddPQEducationViewModel GetEducationInfoForDataEntry(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0)
        {
            try
            {
                string EV_Colg_Schl_Inst_Name = "0";
                string OrgLabel = "";
                int orgId = 0;
                EV_Colg_Schl_Inst_Name = db.PQEducations.Where(a => a.ClientRowID == ClientRowId && a.PersonalRowID == PersonalRowID
                && a.CheckFamilyRowID == ChFamilRowId && a.SubCheckRowID == subCheckId).Select(a => a.EV_Colg_Schl_Inst_Name.ToString()).FirstOrDefault();
                if (!string.IsNullOrEmpty(EV_Colg_Schl_Inst_Name))
                {
                    orgId = int.Parse(EV_Colg_Schl_Inst_Name);
                    if (EV_Colg_Schl_Inst_Name != "0")
                    {
                        OrgLabel = db.MasterColleges.Where(M => M.CollegeRowID == orgId).Select(K => K.CollegeName).FirstOrDefault();
                    }
                }

                return db.PQEducations.Where(a => a.ClientRowID == ClientRowId && a.PersonalRowID == PersonalRowID
                && a.CheckFamilyRowID == ChFamilRowId && a.SubCheckRowID == subCheckId).Select(a => new AddPQEducationViewModel
                {
                    EducationRowID = a.EducationRowId,
                    ClientRowID = a.ClientRowID,
                    PersonalRowID = a.PersonalRowID,
                    ClientPackageRowID = a.ClientPackageRowID,
                    //EV_Cand_Name = a.EV_Cand_Name,
                    EV_Cand_Name = db.PQPersonals.Where(T => T.PersonalRowID == a.PersonalRowID).Select(x => x.Name + " " + x.MiddleName + x.LastName).FirstOrDefault().ToString(),
                    CheckFamilyRowID = a.CheckFamilyRowID,
                    SubCheckRowID = a.SubCheckRowID,
                    UniqueComponentID = a.UniqueComponentID,
                    EV_Cand_Name_Supp_Edu_Doc = a.EV_Cand_Name_Supp_Edu_Doc,
                    EV_Colg_Schl_Inst_Name = a.EV_Colg_Schl_Inst_Name,
                    EV_Colg_Schl_Inst_Add_ConDet = a.EV_Colg_Schl_Inst_Add_ConDet,
                    EV_Affl_University_Name = a.EV_Affl_University_Name,
                    EV_Degree_Name = a.EV_Degree_Name,
                    EV_Mode_Of_Edu = a.EV_Mode_Of_Edu,
                    EV_Enroll_RollNo = a.EV_Enroll_RollNo,
                    EV_RegNo_RegCode = a.EV_RegNo_RegCode,
                    EV_Dur_Course = a.EV_Dur_Course,
                    EV_Course_Status = a.EV_Course_Status,
                    EV_Specialization = a.EV_Specialization,
                    EV_Period_Course = a.EV_Period_Course,
                    EV_Yr_Passing = a.EV_Yr_Passing,
                    EV_Division_CGPA = a.EV_Division_CGPA,
                    EV_Others = a.EV_Others,
                    EV_Others2 = a.EV_Others2,
                    EV_Others3 = a.EV_Others3,
                    EV_Others4 = a.EV_Others4,
                    EV_Others5 = a.EV_Others5,
                    EV_Others6 = a.EV_Others6,
                    EV_Others7 = a.EV_Others7,
                    EV_Others8 = a.EV_Others8,
                    EV_Others9 = a.EV_Others9,
                    EV_Others10 = a.EV_Others10,
                    ATA_CID_No = a.ATA_CID_No,
                    ATA_Cmpny_Addr = a.ATA_Cmpny_Addr,
                    CheckStatus = a.CheckStatus,
                    ReWorkCheckStatus = a.ReWorkCheckStatus,
                    OtherInstitute = a.Remarks,
                    OrgNameLabel = OrgLabel,
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

        public void UpdateEducationInfoByDataEntry(AddPQEducationViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_Cand_Name_Supp_Edu_Doc = model.EV_Cand_Name_Supp_Edu_Doc;
                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_Colg_Schl_Inst_Name = model.EV_Colg_Schl_Inst_Name;
                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_Colg_Schl_Inst_Add_ConDet = model.EV_Colg_Schl_Inst_Add_ConDet;
                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_Affl_University_Name = model.EV_Affl_University_Name;
                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_Degree_Name = model.EV_Degree_Name;
                            //context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_Cand_Name = model.EV_Cand_Name;
                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_Mode_Of_Edu = model.EV_Mode_Of_Edu;
                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_Enroll_RollNo = model.EV_Enroll_RollNo;
                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_RegNo_RegCode = model.EV_RegNo_RegCode;
                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_Dur_Course = model.EV_Dur_Course;
                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_Course_Status = model.EV_Course_Status;
                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_Specialization = model.EV_Specialization;
                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_Period_Course = model.EV_Period_Course;
                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_Yr_Passing = model.EV_Yr_Passing;
                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_Division_CGPA = model.EV_Division_CGPA;
                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_Others = model.EV_Others;
                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_Others2 = model.EV_Others2;
                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_Others3 = model.EV_Others3;
                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_Others4 = model.EV_Others4;
                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_Others5 = model.EV_Others5;
                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_Others6 = model.EV_Others6;
                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_Others7 = model.EV_Others7;
                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_Others8 = model.EV_Others8;
                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_Others9 = model.EV_Others9;
                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_Others10 = model.EV_Others10;
                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).ATA_CID_No = model.ATA_CID_No;
                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).ATA_Cmpny_Addr = model.ATA_Cmpny_Addr;

                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).CheckStatus = model.CheckStatus;
                            if (model.CheckStatus == "Completed")
                            {
                                context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).CheckStatus = "Completed-DE";
                            }
                            else if(model.CheckStatus == "Not Applicable")
                            {
                                context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).CheckStatus = "Not Applicable";
                            }

                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).ModifiedBy = model.CreatedBy;
                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).ModifiedDate = model.CreatedDate;
                            context.SaveChanges();

                            #region Save Check Status

                            var status = context.PQEducationStatus.Where(p => p.EducationRowId == model.EducationRowID).FirstOrDefault();

                            if (status.TMCheckStatus == "Insufficent" && status.TMQCCheckStatus == "Insufficent-QC" && model.CheckStatus == "Completed")
                            {
                                context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).CheckStatus = "WIP-Pending";
                                context.SaveChanges();

                                context.PQEducationStatus.Single(c => c.EducationStatusRowID == status.EducationStatusRowID).DETMCheckStatus = model.CheckStatus;
                                context.PQEducationStatus.Single(c => c.EducationStatusRowID == status.EducationStatusRowID).DETMAllocatedDate = model.CreatedDate;
                                context.PQEducationStatus.Single(c => c.EducationStatusRowID == status.EducationStatusRowID).TMCheckStatus = "WIP-Pending";
                                context.PQEducationStatus.Single(c => c.EducationStatusRowID == status.EducationStatusRowID).TMAllocatedDate = model.CreatedDate;
                                context.PQEducationStatus.Single(c => c.EducationStatusRowID == status.EducationStatusRowID).TMQCCheckStatus = "";
                                context.PQEducationStatus.Single(c => c.EducationStatusRowID == status.EducationStatusRowID).TMQCAllocatedDate = model.CreatedDate;
                            }
                           else if (model.CheckStatus == "Not Applicable")
                            {
                                context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).CheckStatus = "Not Applicable";
                                context.SaveChanges();                              
                               
                            }
                            else
                            {
                                context.PQEducationStatus.Single(c => c.EducationStatusRowID == status.EducationStatusRowID).DETMCheckStatus = model.CheckStatus;
                                context.PQEducationStatus.Single(c => c.EducationStatusRowID == status.EducationStatusRowID).DETMAllocatedDate = model.CreatedDate;

                                if (model.CheckStatus == "Completed")
                                {
                                    context.PQEducationStatus.Single(c => c.EducationStatusRowID == status.EducationStatusRowID).DETMQCCheckStatus = model.CheckStatus;
                                    context.PQEducationStatus.Single(c => c.EducationStatusRowID == status.EducationStatusRowID).DETMQCAllocatedDate = model.CreatedDate;
                                }
                                if (model.CheckStatus == "Insufficent")
                                {
                                    context.PQEducationStatus.Single(c => c.EducationStatusRowID == status.EducationStatusRowID).DETMQCCheckStatus = model.CheckStatus;
                                    context.PQEducationStatus.Single(c => c.EducationStatusRowID == status.EducationStatusRowID).DETMQCAllocatedDate = model.CreatedDate;
                                    context.PQEducationStatus.Single(c => c.EducationStatusRowID == status.EducationStatusRowID).InfSuffRaiseBy = model.CreatedBy;
                                    context.PQEducationStatus.Single(c => c.EducationStatusRowID == status.EducationStatusRowID).InfSuffRaisedDate = model.CreatedDate;
                                    context.PQEducationStatus.Single(c => c.EducationStatusRowID == status.EducationStatusRowID).InfSuffRaiseRemarks = model.INFRemarks;
                                }
                                if (model.CheckStatus == "Rejected" || model.CheckStatus == "Review Requested")
                                {
                                    context.PQEducationStatus.Single(c => c.EducationStatusRowID == status.EducationStatusRowID).Remarks = model.INFRemarks;
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

        public PQUpdateEducationViewModel GetEducationInfoForQualityCheck(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0)
        {
            try
            {
                return db.PQEducations.Where(a => a.ClientRowID == ClientRowId && a.PersonalRowID == PersonalRowID
                && a.CheckFamilyRowID == ChFamilRowId && a.SubCheckRowID == subCheckId).Select(a => new PQUpdateEducationViewModel
                {
                    EducationRowID = a.EducationRowId,
                    ClientRowID = a.ClientRowID,
                    PersonalRowID = a.PersonalRowID,
                    ClientPackageRowID = a.ClientPackageRowID,
                    CheckFamilyRowID = a.CheckFamilyRowID,
                    SubCheckRowID = a.SubCheckRowID,
                    UniqueComponentID = a.UniqueComponentID,
                    EV_Cand_Name = a.EV_Cand_Name,
                    EV_Cand_Name_Supp_Edu_Doc = a.EV_Cand_Name_Supp_Edu_Doc,
                    EV_Colg_Schl_Inst_Name = a.EV_Colg_Schl_Inst_Name,
                    EV_Colg_Schl_Inst_Add_ConDet = a.EV_Colg_Schl_Inst_Add_ConDet,
                    EV_Affl_University_Name = a.EV_Affl_University_Name,
                    EV_Degree_Name = a.EV_Degree_Name,
                    EV_Mode_Of_Edu = a.EV_Mode_Of_Edu,
                    EV_Enroll_RollNo = a.EV_Enroll_RollNo,
                    EV_RegNo_RegCode = a.EV_RegNo_RegCode,
                    EV_Dur_Course = a.EV_Dur_Course,
                    EV_Course_Status = a.EV_Course_Status,
                    EV_Specialization = a.EV_Specialization,
                    EV_Period_Course = a.EV_Period_Course,
                    EV_Yr_Passing = a.EV_Yr_Passing,
                    EV_Division_CGPA = a.EV_Division_CGPA,
                    EV_Others = a.EV_Others,
                    EV_Others2 = a.EV_Others2,
                    EV_Others3 = a.EV_Others3,
                    EV_Others4 = a.EV_Others4,
                    EV_Others5 = a.EV_Others5,
                    EV_Others6 = a.EV_Others6,
                    EV_Others7 = a.EV_Others7,
                    EV_Others8 = a.EV_Others8,
                    EV_Others9 = a.EV_Others9,
                    EV_Others10 = a.EV_Others10,
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

        public PQUpdateEducationViewModel GetEducationInfoForQualityCheck(int EducationRowID = 0)
        {
            try
            {
                return db.PQEducations.Where(a => a.EducationRowId == EducationRowID).Select(a => new PQUpdateEducationViewModel
                {
                    EducationRowID = a.EducationRowId,
                    ClientRowID = a.ClientRowID,
                    PersonalRowID = a.PersonalRowID,
                    ClientPackageRowID = a.ClientPackageRowID,
                    CheckFamilyRowID = a.CheckFamilyRowID,
                    SubCheckRowID = a.SubCheckRowID,
                    UniqueComponentID = a.UniqueComponentID,
                    EV_Cand_Name = a.EV_Cand_Name,
                    EV_Cand_Name_Supp_Edu_Doc = a.EV_Cand_Name_Supp_Edu_Doc,
                    EV_Colg_Schl_Inst_Name = a.EV_Colg_Schl_Inst_Name,
                    EV_Colg_Schl_Inst_Add_ConDet = a.EV_Colg_Schl_Inst_Add_ConDet,
                    EV_Affl_University_Name = a.EV_Affl_University_Name,
                    EV_Degree_Name = a.EV_Degree_Name,
                    EV_Mode_Of_Edu = a.EV_Mode_Of_Edu,
                    EV_Enroll_RollNo = a.EV_Enroll_RollNo,
                    EV_RegNo_RegCode = a.EV_RegNo_RegCode,
                    EV_Dur_Course = a.EV_Dur_Course,
                    EV_Course_Status = a.EV_Course_Status,
                    EV_Specialization = a.EV_Specialization,
                    EV_Period_Course = a.EV_Period_Course,
                    EV_Yr_Passing = a.EV_Yr_Passing,
                    EV_Division_CGPA = a.EV_Division_CGPA,
                    EV_Others = a.EV_Others,
                    EV_Others2 = a.EV_Others2,
                    EV_Others3 = a.EV_Others3,
                    EV_Others4 = a.EV_Others4,
                    EV_Others5 = a.EV_Others5,
                    EV_Others6 = a.EV_Others6,
                    EV_Others7 = a.EV_Others7,
                    EV_Others8 = a.EV_Others8,
                    EV_Others9 = a.EV_Others9,
                    EV_Others10 = a.EV_Others10,
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

        public void UpdateEducationInfoByQualityCheck(PQUpdateEducationViewModel model)
        {

            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_Cand_Name_Supp_Edu_Doc = model.EV_Cand_Name_Supp_Edu_Doc;
                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_Colg_Schl_Inst_Name = model.EV_Colg_Schl_Inst_Name;
                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_Colg_Schl_Inst_Add_ConDet = model.EV_Colg_Schl_Inst_Add_ConDet;
                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_Affl_University_Name = model.EV_Affl_University_Name;
                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_Degree_Name = model.EV_Degree_Name;
                            //context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_Cand_Name = model.EV_Cand_Name;
                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_Mode_Of_Edu = model.EV_Mode_Of_Edu;
                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_Enroll_RollNo = model.EV_Enroll_RollNo;
                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_RegNo_RegCode = model.EV_RegNo_RegCode;
                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_Dur_Course = model.EV_Dur_Course;
                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_Course_Status = model.EV_Course_Status;
                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_Specialization = model.EV_Specialization;
                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_Period_Course = model.EV_Period_Course;
                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_Yr_Passing = model.EV_Yr_Passing;
                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_Division_CGPA = model.EV_Division_CGPA;
                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_Others = model.EV_Others;
                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_Others2 = model.EV_Others2;
                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_Others3 = model.EV_Others3;
                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_Others4 = model.EV_Others4;
                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_Others5 = model.EV_Others5;
                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_Others6 = model.EV_Others6;
                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_Others7 = model.EV_Others7;
                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_Others8 = model.EV_Others8;
                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_Others9 = model.EV_Others9;
                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_Others10 = model.EV_Others10;
                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).ATA_CID_No = model.ATA_CID_No;
                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).ATA_Cmpny_Addr = model.ATA_Cmpny_Addr;
                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).ModifiedBy = model.ModifiedBy;
                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).ModifiedDate = model.ModifiedDate;

                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).CheckStatus = model.CheckStatus;
                            if (model.CheckStatus == "Completed-DE")
                            {
                                context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).CheckStatus = "Allocated-Mgr";
                            }
                            context.SaveChanges();

                            #region Save Check Status

                            var CurrentStatus = context.PQEducationStatus.Where(p => p.EducationRowId == model.EducationRowID).FirstOrDefault();

                            if (model.CheckStatus == "Completed-DE")
                            {
                                if (CurrentStatus.TMCheckStatus == "Rejected")
                                {
                                    context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).CheckStatus = "WIP-Pending";
                                    context.SaveChanges();

                                    context.PQEducationStatus.Single(c => c.EducationStatusRowID == CurrentStatus.EducationStatusRowID).DETMQCCheckStatus = "Completed-QC";
                                    context.PQEducationStatus.Single(c => c.EducationStatusRowID == CurrentStatus.EducationStatusRowID).TMCheckStatus = "WIP-Pending";
                                    context.PQEducationStatus.Single(c => c.EducationStatusRowID == CurrentStatus.EducationStatusRowID).TMAllocatedDate = model.ModifiedDate;
                                }
                                else
                                {
                                    context.PQEducationStatus.Single(c => c.EducationStatusRowID == CurrentStatus.EducationStatusRowID).DETMQCCheckStatus = "Completed-QC";
                                    context.PQEducationStatus.Single(c => c.EducationStatusRowID == CurrentStatus.EducationStatusRowID).MgrCheckStatus = "Allocated-Mgr";
                                    context.PQEducationStatus.Single(c => c.EducationStatusRowID == CurrentStatus.EducationStatusRowID).MgrAllocatedTo = model.MgrAllocatedTo;
                                    context.PQEducationStatus.Single(c => c.EducationStatusRowID == CurrentStatus.EducationStatusRowID).MgrAllocatedDate = model.ModifiedDate;
                                }

                            }

                            if (model.CheckStatus == "Insufficent")
                            {
                                context.PQEducationStatus.Single(c => c.EducationStatusRowID == CurrentStatus.EducationStatusRowID).DETMQCCheckStatus = "Insufficent-QC";
                                context.PQEducationStatus.Single(c => c.EducationStatusRowID == CurrentStatus.EducationStatusRowID).InfSuffClearBy = 0;
                                context.PQEducationStatus.Single(c => c.EducationStatusRowID == CurrentStatus.EducationStatusRowID).InfSuffClearComments = "";
                                context.PQEducationStatus.Single(c => c.EducationStatusRowID == CurrentStatus.EducationStatusRowID).InfSuffClearDate = null;
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

        public void UpdateEducationInfoByManager(PQUpdateEducationManagerViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).CheckStatus = model.CheckStatus;
                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).ModifiedBy = model.ModifiedBy;
                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).ModifiedDate = model.ModifiedDate;
                            context.SaveChanges();

                            #region Save Check Status

                            int statusId = context.PQEducationStatus.Where(p => p.EducationRowId == model.EducationRowID).FirstOrDefault().EducationStatusRowID;

                            context.PQEducationStatus.Single(c => c.EducationStatusRowID == statusId).TLCheckStatus = model.CheckStatus;
                            context.PQEducationStatus.Single(c => c.EducationStatusRowID == statusId).TLAllocatedTo = model.TLAllocatedTo;
                            context.PQEducationStatus.Single(c => c.EducationStatusRowID == statusId).TLAllocatedDate = model.ModifiedDate;
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

        public void UpdateEducationInfoByTeamLeader(PQUpdateEducationTeamLeaderViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).CheckStatus = model.CheckStatus;
                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).ModifiedBy = model.ModifiedBy;
                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).ModifiedDate = model.ModifiedDate;
                            context.SaveChanges();

                            #region Save Check Status

                            int statusId = context.PQEducationStatus.Where(p => p.EducationRowId == model.EducationRowID).FirstOrDefault().EducationStatusRowID;

                            context.PQEducationStatus.Single(c => c.EducationStatusRowID == statusId).TMCheckStatus = model.CheckStatus;
                            context.PQEducationStatus.Single(c => c.EducationStatusRowID == statusId).TMAllocatedTo = model.TMAllocatedTo;
                            context.PQEducationStatus.Single(c => c.EducationStatusRowID == statusId).TMAllocatedDate = model.ModifiedDate;

                            context.PQEducationStatus.Single(c => c.EducationStatusRowID == statusId).TMQCCheckStatus = model.CheckStatus;
                            context.PQEducationStatus.Single(c => c.EducationStatusRowID == statusId).TMQCAllocatedTo = model.TMQCAllocatedTo;
                            context.PQEducationStatus.Single(c => c.EducationStatusRowID == statusId).TMQCAllocatedDate = model.ModifiedDate;

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
        public void UpdateEducationInfoByTeamLeaderReAllocated(PQUpdateEducationTeamLeaderViewModel model)
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

                            int statusId = context.PQEducationStatus.Where(p => p.EducationRowId == model.EducationRowID).FirstOrDefault().EducationStatusRowID;

                            if (model.TMAllocatedTo > 0)
                            {
                                context.PQEducationStatus.Single(c => c.EducationStatusRowID == statusId).TMAllocatedTo = model.TMAllocatedTo;
                                context.PQEducationStatus.Single(c => c.EducationStatusRowID == statusId).TMAllocatedDate = model.ModifiedDate;
                            }
                            if (model.TMQCAllocatedTo > 0)
                            {
                                context.PQEducationStatus.Single(c => c.EducationStatusRowID == statusId).TMQCAllocatedTo = model.TMQCAllocatedTo;
                                context.PQEducationStatus.Single(c => c.EducationStatusRowID == statusId).TMQCAllocatedDate = model.ModifiedDate;
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

        public void UpdateEducationInfoByTeamMember(PQUpdateEducationTeamMemberViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            ClientCustomize cc = new ClientCustomize();
                            DataSet dd = new DataSet();
                            dd = cc.getInformationFromDepartmentRowID(model.EducationRowID, "Education");
                            int personalRowid = 0;
                            int subCheckRowID = 0;
                            if (dd.Tables[0].Rows.Count > 0)
                            {
                                personalRowid = int.Parse((Convert.ToString(dd.Tables[0].Rows[0]["PersonalRowID"])));
                                subCheckRowID = int.Parse((Convert.ToString(dd.Tables[0].Rows[0]["SubCheckRowID"])));
                            }

                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).CheckStatus = model.CheckStatus;
                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).ModifiedBy = model.ModifiedBy;
                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).ModifiedDate = model.ModifiedDate;
                            context.SaveChanges();

                            #region Save Check Status

                            var statusId = context.PQEducationStatus.Where(p => p.EducationRowId == model.EducationRowID).FirstOrDefault().EducationStatusRowID;

                            context.PQEducationStatus.Single(c => c.EducationStatusRowID == statusId).TMCheckStatus = model.CheckStatus;
                            context.PQEducationStatus.Single(c => c.EducationStatusRowID == statusId).TMQCCheckStatus = "";
                            if (model.CheckStatus == "PV Raised")
                            {
                                context.PQEducationStatus.Single(c => c.EducationStatusRowID == statusId).PVMgrCheckStatus = "Allocate-Mgr";
                                context.PQEducationStatus.Single(c => c.EducationStatusRowID == statusId).PVMgrAllocatedTo = model.PVMgrAllocatedTo;
                                context.PQEducationStatus.Single(c => c.EducationStatusRowID == statusId).PVMgrAllocatedDate = model.ModifiedDate;
                            }
                            if (model.CheckStatus == "Insufficent")
                            {
                                cc.UpdateCheckStatus(personalRowid, subCheckRowID, 1, 22, "Check", model.ModifiedBy);
                                context.PQEducationStatus.Single(c => c.EducationStatusRowID == statusId).TMQCCheckStatus = model.CheckStatus;
                                context.PQEducationStatus.Single(c => c.EducationStatusRowID == statusId).InfSuffRaiseRemarks = model.ActionRemark;
                                context.PQEducationStatus.Single(c => c.EducationStatusRowID == statusId).InfSuffRaiseBy = model.ModifiedBy;
                                context.PQEducationStatus.Single(c => c.EducationStatusRowID == statusId).InfSuffRaisedDate = model.ModifiedDate;
                            }
                            if (model.CheckStatus == "Rejected")
                            {
                                cc.UpdateCheckStatus(personalRowid, subCheckRowID, 1, 15, "Check", model.ModifiedBy);
                                context.PQEducationStatus.Single(c => c.EducationStatusRowID == statusId).Remarks = model.ActionRemark;
                            }
                            if (model.CheckStatus == "Funds Pending")
                            {
                                cc.UpdateCheckStatus(personalRowid, subCheckRowID, 1, 26, "Check", model.ModifiedBy);
                                context.PQEducationStatus.Single(c => c.EducationStatusRowID == statusId).Remarks = model.ActionRemark;
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

        public void RejectionUpdateEducationInfoByQC(UpdateEducationRejectionViewModel model)
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
                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).CheckStatus = ActualCheckStatus;
                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).ModifiedBy = model.ModifiedBy;
                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).ModifiedDate = model.ModifiedDate;
                            context.SaveChanges();

                            #region Update Check Status

                            int statusId = context.PQEducationStatus.Where(p => p.EducationRowId == model.EducationRowID).FirstOrDefault().EducationStatusRowID;
                            context.PQEducationStatus.Single(c => c.EducationStatusRowID == statusId).DETMCheckStatus = ActualCheckStatus;
                            context.PQEducationStatus.Single(c => c.EducationStatusRowID == statusId).DETMQCCheckStatus = "";
                            context.PQEducationStatus.Single(c => c.EducationStatusRowID == statusId).Remarks = model.INFRemarks;
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

        public void ViewEducationInfoByVrifierTL(ViewEducationViewModel model)
        {

            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).CheckStatus = model.CheckStatus;
                            context.SaveChanges();

                            int statusId = context.PQEducationStatus.Where(p => p.EducationRowId == model.EducationRowID).FirstOrDefault().EducationStatusRowID;

                            context.PQEducationStatus.Single(c => c.EducationStatusRowID == statusId).TMCheckStatus = model.CheckStatus;
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

        public bool IfExistEducation(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0, string CheckStatus = "")
        {
            try
            {
                var listChecks = db.PQEducations.Where(a => a.ClientRowID == ClientRowId && a.PersonalRowID == PersonalRowID
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

        public bool IfExistEducationForDELeftMenu(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0)
        {
            try
            {
                var listChecks = db.PQEducations.Where(a => a.ClientRowID == ClientRowId && a.PersonalRowID == PersonalRowID
                     && a.CheckFamilyRowID == ChFamilRowId && a.SubCheckRowID == subCheckId).FirstOrDefault();
                if (listChecks != null)
                {
                    var ChkStatus = db.PQEducationStatus.Where(s => s.EducationRowId == listChecks.EducationRowId).FirstOrDefault();

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

        public bool IfExistEducationForQCLeftMenu(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0, int CheckStatusid = 0)
        {
            try
            {
                //var listChecks = db.PQEducations.Where(a => a.ClientRowID == ClientRowId && a.PersonalRowID == PersonalRowID
                //     && a.CheckFamilyRowID == ChFamilRowId && a.SubCheckRowID == subCheckId).FirstOrDefault();
                ////if (listChecks != null)
                ////{
                //    var ChkStatus = db.PQEducationStatus.Where(s => s.EducationRowId == listChecks.EducationRowId).FirstOrDefault();

                    if ( CheckStatusid == 68 || CheckStatusid == 14 || CheckStatusid == 15 || CheckStatusid == 42 || CheckStatusid == 113 || CheckStatusid == 114 || CheckStatusid == 52)
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

        //update Education research table and Get More ids for check status and action history ang log save 
        //public PQUpdateEducationResearcherViewModel UpdateEducationInfoByResearcher(PQUpdateEducationResearcherViewModel model)
        //{
        //    try
        //    {
        //        if (model != null)
        //        {
        //            db.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).CheckStatus = model.CheckStatus;
        //            db.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).ModifiedBy = model.ModifiedBy;
        //            db.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).ModifiedDate = model.ModifiedDate;
        //        }
        //        //Get More ids for check status and action history ang log save  
        //        var EduResearch = db.PQEducations.Where(c => c.EducationRowId == model.EducationRowID).FirstOrDefault();
        //        model.PersonalRowID = EduResearch.PersonalRowID;
        //        model.SubCheckRowID = EduResearch.SubCheckRowID;
        //        model.UniqueComponentID = EduResearch.UniqueComponentID;
        //        return model;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}

        #region ***** Temp *****

        public AddTempPQEducationViewModel GetTempEducationInfoByself(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0)
        {
            try
            {
                int EV_Colg_Schl_Inst_Name = 0;
                string CollegeLabel = "";
                int collegeid = 0;
                EV_Colg_Schl_Inst_Name = db.TempPQEducations.Where(a => a.ClientRowID == ClientRowId && a.PersonalRowID == PersonalRowID
                && a.CheckFamilyRowID == ChFamilRowId && a.SubCheckRowID == subCheckId).Select(a => a.EV_Colg_Schl_Inst_Name).FirstOrDefault();
                if (!string.IsNullOrEmpty(EV_Colg_Schl_Inst_Name.ToString()))
                {
                    collegeid =(EV_Colg_Schl_Inst_Name);
                    if (EV_Colg_Schl_Inst_Name != 0)
                    {
                        CollegeLabel = db.MasterColleges.Where(M => M.CollegeRowID == collegeid).Select(K => K.CollegeName).FirstOrDefault();
                    }
                }

                return db.TempPQEducations.Where(a => a.ClientRowID == ClientRowId && a.PersonalRowID == PersonalRowID
                && a.CheckFamilyRowID == ChFamilRowId && a.SubCheckRowID == subCheckId).Select(a => new AddTempPQEducationViewModel
                {
                    EducationRowID = a.EducationRowId,
                    ClientRowID = a.ClientRowID,
                    PersonalRowID = a.PersonalRowID,
                    ClientPackageRowID = a.ClientPackageRowID,
                    CheckFamilyRowID = a.CheckFamilyRowID,
                    SubCheckRowID = a.SubCheckRowID,
                    UniqueComponentID = a.UniqueComponentID,
                    EV_Cand_Name_Supp_Edu_Doc = a.EV_Cand_Name_Supp_Edu_Doc,
                    EV_Colg_Schl_Inst_Name = a.EV_Colg_Schl_Inst_Name,
                    EV_Colg_Schl_Inst_Add_ConDet = a.EV_Colg_Schl_Inst_Add_ConDet,
                    EV_Affl_University_Name = a.EV_Affl_University_Name,
                    EV_Degree_Name = a.EV_Degree_Name,
                    EV_Mode_Of_Edu = a.EV_Mode_Of_Edu,
                    EV_Enroll_RollNo = a.EV_Enroll_RollNo,
                    EV_RegNo_RegCode = a.EV_RegNo_RegCode,
                    EV_Dur_Course = a.EV_Dur_Course,
                    EV_Course_Status = a.EV_Course_Status,
                    EV_Specialization = a.EV_Specialization,
                    EV_Period_Course = a.EV_Period_Course,
                    EV_Yr_Passing = a.EV_Yr_Passing,
                    EV_Division_CGPA = a.EV_Division_CGPA,
                    //EV_DegreeCertfict = a.EV_DegreeCertfict,
                    //EV_ProvisionalCertfict = a.EV_ProvisionalCertfict,
                    //EV_GradeSheet = a.EV_GradeSheet,
                    //EV_AllMarksCards = a.EV_AllMarksCards,
                    //EV_DocStatColgNameNo = a.EV_DocStatColgNameNo,
                    //EV_OtherProof = a.EV_OtherProof,
                    EV_Others = a.EV_Others,
                    EV_Others2 = a.EV_Others2,
                    EV_Others3 = a.EV_Others3,
                    EV_Others4 = a.EV_Others4,
                    EV_Others5 = a.EV_Others5,
                    EV_Others6 = a.EV_Others6,
                    EV_Others7 = a.EV_Others7,
                    EV_Others8 = a.EV_Others8,
                    EV_Others9 = a.EV_Others9,
                    EV_Others10 = a.EV_Others10,
                    ATA_CID_No = a.ATA_CID_No,
                    ATA_Cmpny_Addr = a.ATA_Cmpny_Addr,
                    Remarks = a.Remarks,
                    CheckStatus = a.CheckStatus,
                    CreatedBy = a.CreatedBy,
                    EV_Cand_Name = a.EV_Cand_Name,
                    CollegeNameLabel= CollegeLabel
                    //EV_Dur_Course_DT = Convert.ToDateTime(a.EV_Dur_Course),
                }).FirstOrDefault();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public int GetAllTempEducationsByClientId(short ClientRowID = 0)
        {
            try
            {
                var data = db.TempPQEducations.Where(p => p.ClientRowID == ClientRowID).ToList();
                return data.Count();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void UpdateTempEducationInfoByself(AddTempPQEducationViewModel model)
        {
            try
            {
                if (model != null)
                {
                    db.TempPQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_Cand_Name_Supp_Edu_Doc = model.EV_Cand_Name_Supp_Edu_Doc;
                    db.TempPQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_Colg_Schl_Inst_Name = model.EV_Colg_Schl_Inst_Name;
                    db.TempPQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_Colg_Schl_Inst_Add_ConDet = model.EV_Colg_Schl_Inst_Add_ConDet;
                    db.TempPQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_Affl_University_Name = model.EV_Affl_University_Name;
                    db.TempPQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_Degree_Name = model.EV_Degree_Name;
                    db.TempPQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_Cand_Name = model.EV_Cand_Name;
                    db.TempPQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_Mode_Of_Edu = model.EV_Mode_Of_Edu;
                    db.TempPQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_Enroll_RollNo = model.EV_Enroll_RollNo;
                    db.TempPQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_RegNo_RegCode = model.EV_RegNo_RegCode;
                    db.TempPQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_Dur_Course = model.EV_Dur_Course;
                    db.TempPQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_Course_Status = model.EV_Course_Status;
                    db.TempPQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_Specialization = model.EV_Specialization;
                    db.TempPQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_Period_Course = model.EV_Period_Course;
                    db.TempPQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_Yr_Passing = model.EV_Yr_Passing;
                    db.TempPQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_Division_CGPA = model.EV_Division_CGPA;
                    db.TempPQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_Others = model.EV_Others;
                    db.TempPQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_Others2 = model.EV_Others2;
                    db.TempPQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_Others3 = model.EV_Others3;
                    db.TempPQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_Others4 = model.EV_Others4;
                    db.TempPQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_Others5 = model.EV_Others5;
                    db.TempPQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_Others6 = model.EV_Others6;
                    db.TempPQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_Others7 = model.EV_Others7;
                    db.TempPQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_Others8 = model.EV_Others8;
                    db.TempPQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_Others9 = model.EV_Others9;
                    db.TempPQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_Others10 = model.EV_Others10;
                    db.TempPQEducations.Single(c => c.EducationRowId == model.EducationRowID).ATA_CID_No = model.ATA_CID_No;
                    db.TempPQEducations.Single(c => c.EducationRowId == model.EducationRowID).ATA_Cmpny_Addr = model.ATA_Cmpny_Addr;
                    //db.TempPQEducations.Single(c => c.EducationRowId == model.EducationRowID).CheckStatus = model.CheckStatus;
                    db.TempPQEducations.Single(c => c.EducationRowId == model.EducationRowID).ModifiedBy = model.ModifiedBy;
                    db.TempPQEducations.Single(c => c.EducationRowId == model.EducationRowID).ModifiedDate = model.ModifiedDate;
                    db.TempPQEducations.Single(c => c.EducationRowId == model.EducationRowID).Remarks = model.Remarks;
                    if(model.EV_Dur_Course_DT!=null)
                    {
                        db.TempPQEducations.Single(c => c.EducationRowId == model.EducationRowID).EV_Dur_Course = model.EV_Dur_Course_DT.ToString();
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public void AddTempEducationInformation(AddTempPQEducationViewModel model)
        {
            try
            {
                if (model != null)
                {
                    TempPQEducation entity = new TempPQEducation();
                    entity.ClientRowID = model.ClientRowID;
                    entity.PersonalRowID = model.PersonalRowID;
                    entity.ClientPackageRowID = model.ClientPackageRowID;
                    entity.CheckFamilyRowID = model.CheckFamilyRowID;
                    entity.SubCheckRowID = model.SubCheckRowID;
                    entity.UniqueComponentID = model.UniqueComponentID;
                    if(model.EV_Cand_Name==null)
                    {
                        entity.EV_Cand_Name = db.PQCandidateLogins.Where(w => w.TempPersonalRowID == model.PersonalRowID).FirstOrDefault().CandidateName;
                    }
                    else
                    {
                        entity.EV_Cand_Name = model.EV_Cand_Name;
                    }
                    entity.EV_Sec_Ref_Id = model.EV_Sec_Ref_Id;
                    entity.EV_Cand_Name_Supp_Edu_Doc = model.EV_Cand_Name_Supp_Edu_Doc;
                    entity.EV_Colg_Schl_Inst_Name = model.EV_Colg_Schl_Inst_Name;
                    entity.EV_Colg_Schl_Inst_Add_ConDet = model.EV_Colg_Schl_Inst_Add_ConDet;
                    entity.EV_Affl_University_Name = model.EV_Affl_University_Name;
                    entity.EV_Degree_Name = model.EV_Degree_Name;
                    entity.EV_Mode_Of_Edu = model.EV_Mode_Of_Edu;
                    entity.EV_Enroll_RollNo = model.EV_Enroll_RollNo;
                    entity.EV_RegNo_RegCode = model.EV_RegNo_RegCode;
                    entity.EV_Dur_Course = model.EV_Dur_Course;
                    entity.EV_Course_Status = model.EV_Course_Status;
                    entity.EV_Specialization = model.EV_Specialization;
                    entity.EV_Period_Course = model.EV_Period_Course;
                    entity.EV_Yr_Passing = model.EV_Yr_Passing;
                    entity.EV_Division_CGPA = model.EV_Division_CGPA;
                    //entity.EV_DegreeCertfict = model.EV_DegreeCertfict;
                    //entity.EV_ProvisionalCertfict = model.EV_ProvisionalCertfict;
                    //entity.EV_GradeSheet = model.EV_GradeSheet;
                    //entity.EV_AllMarksCards = model.EV_AllMarksCards;
                    //entity.EV_DocStatColgNameNo = model.EV_DocStatColgNameNo;
                    //entity.EV_OtherProof = model.EV_OtherProof;
                    entity.EV_Others = model.EV_Others;
                    entity.EV_Others2 = model.EV_Others2;
                    entity.EV_Others3 = model.EV_Others3;
                    entity.EV_Others4 = model.EV_Others4;
                    entity.EV_Others5 = model.EV_Others5;
                    entity.EV_Others6 = model.EV_Others6;
                    entity.EV_Others7 = model.EV_Others7;
                    entity.EV_Others8 = model.EV_Others8;
                    entity.EV_Others9 = model.EV_Others9;
                    entity.EV_Others10 = model.EV_Others10;
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
                        if (model.EV_Dur_Course_DT != null)
                        {
                            entity.EV_Dur_Course = model.EV_Dur_Course_DT.ToString();
                        }                        
                    }
                    db.TempPQEducations.Add(entity);
                }


            }
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region *****  Education Insuff *****

        public PQEducationInsuffListPagedModel GetAllEducationInsuffListForDETL(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", int PersonalId = 0, string spocEmail = "", short TeamMemberRowid = 0, int ClientUserRowID = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                IQueryable<PQEducationStatus> data = db.PQEducationStatus.Include("PQEducation").Where(p => (p.DETMQCCheckStatus == "Insufficent-QC" || p.TMQCCheckStatus == "Insufficent-QC") && p.InfSuffClearBy == 0);

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
                    data = data.Where(b => b.PQEducation.PQPersonal.ClientUserRowID == ClientUserRowID);
                }

                if (PersonalId == 0)
                {
                    if (ClientRowID > 0)
                    {
                        data = data.Where(b => b.PQEducation.ClientRowID == ClientRowID);
                    }

                    if (!string.IsNullOrEmpty(spocEmail))
                    {
                        var Clients = db.PQClientMasters.Where(s => s.SpocEmailID.Contains(spocEmail)).Select(s => s.ClientRowID).ToList();
                        data = data.Where(a => Clients.Contains(a.PQEducation.ClientRowID));
                    }

                    if (!string.IsNullOrEmpty(RecievingToDate) && !string.IsNullOrEmpty(RecievingFromDate))
                    {
                        DateTime startDate = Convert.ToDateTime(RecievingFromDate);
                        DateTime EndDate = Convert.ToDateTime(RecievingToDate);

                        data = data.Where(b => DbFunctions.TruncateTime(b.PQEducation.PQPersonal.OrderDate) >= startDate.Date && DbFunctions.TruncateTime(b.PQEducation.PQPersonal.OrderDate) <= EndDate.Date);
                    }

                    if (!string.IsNullOrEmpty(CompletedToDate) && !string.IsNullOrEmpty(CompletedFromDate))
                    {
                        DateTime startDate = Convert.ToDateTime(CompletedFromDate);
                        DateTime EndDate = Convert.ToDateTime(CompletedToDate);

                        data = data.Where(b => DbFunctions.TruncateTime(b.PQEducation.PQPersonal.DeliveryDate) >= startDate.Date && DbFunctions.TruncateTime(b.PQEducation.PQPersonal.DeliveryDate) <= EndDate.Date);
                    }

                    if (!string.IsNullOrEmpty(Search))
                    {
                        data = data.Where(b => (b.PQEducation.PQPersonal.Name + " " + b.PQEducation.PQPersonal.MiddleName + " " + b.PQEducation.PQPersonal.LastName).ToString().Replace(" ", "").Contains(Search.Replace(" ", "")) || b.PQEducation.PQPersonal.ClientRefID == Search);
                    }
                }
                else
                {
                    data = data.Where(b => b.PQEducation.PersonalRowID == PersonalId);
                }

                switch (sort)
                {
                    default:
                        data = sortDir == "asc" ? data.OrderByDescending(d => d.EducationRowId) : data.OrderBy(d => d.EducationRowId);
                        break;
                }

                PQEducationInsuffListPagedModel model = new PQEducationInsuffListPagedModel();
                model.PageSize = pageSize;
                model.TotalRecords = data.Count();
                model.PQAllInsuffEducations = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new PQEducationInsuffViewModel
                {
                    EducationRowID = item.EducationRowId,
                    ClientRowID = item.PQEducation.ClientRowID,
                    ClientName = item.PQEducation.PQClientMaster.MasterAbbreviation.ClientName + " " + item.PQEducation.PQClientMaster.MasterClientSubGroup.ClientSubGroupName,
                    PersonalRowID = item.PQEducation.PersonalRowID,
                    CandidateName = item.PQEducation.PQPersonal.CandTitle + " " + item.PQEducation.PQPersonal.Name + " " + item.PQEducation.PQPersonal.MiddleName + " " + item.PQEducation.PQPersonal.LastName,
                    SubCheckRowID = item.PQEducation.SubCheckRowID,
                    CheckFamilyRowID = item.PQEducation.CheckFamilyRowID,
                    SubCheckName = item.PQEducation.MasterSubCheckFamily.SubCheckName,
                    CompanyRefNo = item.PQEducation.EV_Sec_Ref_Id,
                    InfSuffRaiseBy = db.TeamMembers.Where(t => t.TeamMemberRowID == item.InfSuffRaiseBy).Select(t => t.TMFirstName + " " + t.TMLastName).FirstOrDefault(),
                    InfSuffRaisedDate = item.InfSuffRaisedDate,
                    INFRemarks = item.InfSuffRaiseRemarks,
                    CreatedDate = item.PQEducation.CreatedDate,
                    CheckStatus = item.PQEducation.CheckStatus,
                    ReWorkCheckStatus = item.PQEducation.ReWorkCheckStatus
                }).ToList();
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public PQEducationInsuffListPagedModel GetAllEducationInsuffListForDETL1(int pageNo, int pageSize, string sort, string sortDir,string subcheckType, string Search = "", short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", int PersonalId = 0, string spocEmail = "", short TeamMemberRowid = 0, int ClientUserRowID = 0,int allclints=0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataSet ds = new DataSet();
                PQEducationInsuffListPagedModel model = new PQEducationInsuffListPagedModel();
                ds = cc.getCheckwiseinsuffDetailSummary(ClientRowID, ClientUserRowID, subcheckType,Search, allclints);
                List<PQEducationInsuffViewModel> lmd = new List<PQEducationInsuffViewModel>();
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>
                {
                        PQEducationInsuffViewModel nn = new PQEducationInsuffViewModel();
                        //lmd.Add(new PQEducationInsuffViewModel
                        //{
                        nn.PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]);
                        nn.EducationRowID = Convert.ToInt32(dr["EducationRowID"]);
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
                        nn.Remarks = (dr["Remarks"]).ToString();
                        nn.CollegeName = (dr["CollegeName"]).ToString();
                        nn.EntityName = (dr["EntityName"]).ToString();
                        nn.ClientPackageName = (dr["CandidateType"]).ToString();
                        lmd.Add(nn);
                    }

                //}
                
                model.PageSize = pageSize;
                model.TotalRecords = ds.Tables[0].Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PQAllInsuffEducations = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);

                //IQueryable<PQEducationStatus> data = db.PQEducationStatus.Include("PQEducation").Where(p => (p.DETMQCCheckStatus == "Insufficent-QC" || p.TMQCCheckStatus == "Insufficent-QC") && p.InfSuffClearBy == 0);

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
                //    data = data.Where(b => b.PQEducation.PQPersonal.ClientUserRowID == ClientUserRowID);
                //}

                //if (PersonalId == 0)
                //{
                //    if (ClientRowID > 0)
                //    {
                //        data = data.Where(b => b.PQEducation.ClientRowID == ClientRowID);
                //    }

                //    if (!string.IsNullOrEmpty(spocEmail))
                //    {
                //        var Clients = db.PQClientMasters.Where(s => s.SpocEmailID.Contains(spocEmail)).Select(s => s.ClientRowID).ToList();
                //        data = data.Where(a => Clients.Contains(a.PQEducation.ClientRowID));
                //    }

                //    if (!string.IsNullOrEmpty(RecievingToDate) && !string.IsNullOrEmpty(RecievingFromDate))
                //    {
                //        DateTime startDate = Convert.ToDateTime(RecievingFromDate);
                //        DateTime EndDate = Convert.ToDateTime(RecievingToDate);

                //        data = data.Where(b => DbFunctions.TruncateTime(b.PQEducation.PQPersonal.OrderDate) >= startDate.Date && DbFunctions.TruncateTime(b.PQEducation.PQPersonal.OrderDate) <= EndDate.Date);
                //    }

                //    if (!string.IsNullOrEmpty(CompletedToDate) && !string.IsNullOrEmpty(CompletedFromDate))
                //    {
                //        DateTime startDate = Convert.ToDateTime(CompletedFromDate);
                //        DateTime EndDate = Convert.ToDateTime(CompletedToDate);

                //        data = data.Where(b => DbFunctions.TruncateTime(b.PQEducation.PQPersonal.DeliveryDate) >= startDate.Date && DbFunctions.TruncateTime(b.PQEducation.PQPersonal.DeliveryDate) <= EndDate.Date);
                //    }

                //    if (!string.IsNullOrEmpty(Search))
                //    {
                //        data = data.Where(b => (b.PQEducation.PQPersonal.Name + " " + b.PQEducation.PQPersonal.MiddleName + " " + b.PQEducation.PQPersonal.LastName).ToString().Replace(" ", "").Contains(Search.Replace(" ", "")) || b.PQEducation.PQPersonal.ClientRefID == Search);
                //    }
                //}
                //else
                //{
                //    data = data.Where(b => b.PQEducation.PersonalRowID == PersonalId);
                //}

                //switch (sort)
                //{
                //    default:
                //        data = sortDir == "asc" ? data.OrderByDescending(d => d.EducationRowId) : data.OrderBy(d => d.EducationRowId);
                //        break;
                //}

                //PQEducationInsuffListPagedModel model = new PQEducationInsuffListPagedModel();
                //model.PageSize = pageSize;
                //model.TotalRecords = data.Count();
                //model.PQAllInsuffEducations = data.Skip((pageNo - 1) * pageSize).Take(pageSize).Select(item => new PQEducationInsuffViewModel
                //{
                //    EducationRowID = item.EducationRowId,
                //    ClientRowID = item.PQEducation.ClientRowID,
                //    ClientName = item.PQEducation.PQClientMaster.MasterAbbreviation.ClientName + " " + item.PQEducation.PQClientMaster.MasterClientSubGroup.ClientSubGroupName,
                //    PersonalRowID = item.PQEducation.PersonalRowID,
                //    CandidateName = item.PQEducation.PQPersonal.CandTitle + " " + item.PQEducation.PQPersonal.Name + " " + item.PQEducation.PQPersonal.MiddleName + " " + item.PQEducation.PQPersonal.LastName,
                //    SubCheckRowID = item.PQEducation.SubCheckRowID,
                //    CheckFamilyRowID = item.PQEducation.CheckFamilyRowID,
                //    SubCheckName = item.PQEducation.MasterSubCheckFamily.SubCheckName,
                //    CompanyRefNo = item.PQEducation.EV_Sec_Ref_Id,
                //    InfSuffRaiseBy = db.TeamMembers.Where(t => t.TeamMemberRowID == item.InfSuffRaiseBy).Select(t => t.TMFirstName + " " + t.TMLastName).FirstOrDefault(),
                //    InfSuffRaisedDate = item.InfSuffRaisedDate,
                //    INFRemarks = item.InfSuffRaiseRemarks,
                //    CreatedDate = item.PQEducation.CreatedDate,
                //    CheckStatus = item.PQEducation.CheckStatus,
                //    ReWorkCheckStatus = item.PQEducation.ReWorkCheckStatus
                //}).ToList();
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PQEducationInsuffListPagedModel GetAllEducationInsuffListForCSSpoc(int pageNo, int pageSize, string sort, string sortDir, string subcheckType, string Search = "", short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", int PersonalId = 0, string spocEmail = "", short TeamMemberRowid = 0, int ClientUserRowID = 0)
        {
            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }

                DataSet ds = new DataSet();
                PQEducationInsuffListPagedModel model = new PQEducationInsuffListPagedModel();
                ds = cc.getinsuffDetails_CSSpoc(subcheckType, spocEmail, ClientRowID,Search);
                List<PQEducationInsuffViewModel> lmd = new List<PQEducationInsuffViewModel>();
                if (ds.Tables.Count > 0)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        PQEducationInsuffViewModel nn = new PQEducationInsuffViewModel();
                        //lmd.Add(new PQEducationInsuffViewModel
                        //{
                        nn.PersonalRowID = Convert.ToInt32(dr["PersonalRowID"]);
                        nn.EducationRowID = Convert.ToInt32(dr["VerificationRowId"]);
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
                        nn.ClientName=(dr["ClientName"]).ToString();
                        nn.Remarks = (dr["Remarks"]).ToString();
                        lmd.Add(nn);
                    }

                    //}

                    model.PageSize = pageSize;
                    model.TotalRecords = ds.Tables[0].Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PQAllInsuffEducations = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);               
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }


        public UpdatePQEducationInsuffClearViewModel GetPQEducationDetailsById(int EducationRowID = 0)
        {
            try
            {
                UpdatePQEducationInsuffClearViewModel model = new UpdatePQEducationInsuffClearViewModel();
                //var entity = db.PQEducations.Find(EducationRowID);

                var entity = db.PQEducationStatus.Where(e => e.EducationRowId == EducationRowID).FirstOrDefault();
                if (entity != null)
                {
                    model.EducationRowID = entity.EducationRowId;
                    model.ClientRowID = entity.PQEducation.ClientRowID;
                    model.PersonalRowID = entity.PQEducation.PersonalRowID;
                    model.SubCheckRowID = entity.PQEducation.SubCheckRowID;
                    model.UniqueComponentID = entity.PQEducation.UniqueComponentID;
                    model.INFRemarks = entity.InfSuffClearComments;
                    model.CheckStatus = entity.PQEducation.CheckStatus;
                    model.ReWorkCheckStatus = entity.PQEducation.ReWorkCheckStatus;
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

        public void UpdatePQEducationInsuffClear(UpdatePQEducationInsuffClearViewModel model)
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

                            var EducationStatusId = context.PQEducationStatus.Where(c => c.EducationRowId == model.EducationRowID).FirstOrDefault().EducationStatusRowID;

                            context.PQEducationStatus.Single(c => c.EducationStatusRowID == EducationStatusId).DETMCheckStatus = ScnCheckStatus;
                            context.PQEducationStatus.Single(c => c.EducationStatusRowID == EducationStatusId).InfSuffClearBy = model.InfSuffClearBy;
                            context.PQEducationStatus.Single(c => c.EducationStatusRowID == EducationStatusId).InfSuffClearDate = model.InfSuffClearDate;
                            context.PQEducationStatus.Single(c => c.EducationStatusRowID == EducationStatusId).InfSuffClearComments = model.INFRemarks;
                            context.SaveChanges();

                            //Check Status Update
                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).CheckStatus = ScnCheckStatus;
                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).ModifiedBy = model.ModifiedBy;
                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).ModifiedDate = model.ModifiedDate;
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

        public void ViewEducationInfoDETL(PQUpdateEducationViewModel model)
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

                                context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).CheckStatus = model.CheckStatus;
                                context.SaveChanges();

                                context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).DECaseStatus = "WIP";
                                context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).AllocatedToDEDate = DateTime.Now;
                                context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).DEQCCaseStatus = "WIP";
                                context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).AllocatedToDEQCDate = DateTime.Now;
                                context.SaveChanges();

                                int statusId = context.PQEducationStatus.Where(p => p.EducationRowId == model.EducationRowID).FirstOrDefault().EducationStatusRowID;

                                context.PQEducationStatus.Single(c => c.EducationStatusRowID == statusId).DETMCheckStatus = model.CheckStatus;
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

        #endregion

        public void UpdateEducationRejectionByDEQC(PQUpdateEducationViewModel model, bool flag = false)
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

                                context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).CheckStatus = model.CheckStatus;
                                context.SaveChanges();

                                int statusId = context.PQEducationStatus.Where(p => p.EducationRowId == model.EducationRowID).FirstOrDefault().EducationStatusRowID;
                                if (flag)
                                {
                                    context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).DECaseStatus = "WIP";
                                    context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).AllocatedToDEDate = DateTime.Now;
                                    context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).DEQCCaseStatus = "WIP";
                                    context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).AllocatedToDEQCDate = DateTime.Now;
                                    context.SaveChanges();

                                    // Check move to data entry 
                                    context.PQEducationStatus.Single(c => c.EducationStatusRowID == statusId).DETMQCCheckStatus = "";
                                    context.PQEducationStatus.Single(c => c.EducationStatusRowID == statusId).DETMCheckStatus = model.CheckStatus;
                                }
                                else
                                {
                                    // Check move to Department TM 
                                    context.PQEducationStatus.Single(c => c.EducationStatusRowID == statusId).DETMQCCheckStatus = "Completed-QC";
                                    context.PQEducationStatus.Single(c => c.EducationStatusRowID == statusId).TMCheckStatus = model.CheckStatus;
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
