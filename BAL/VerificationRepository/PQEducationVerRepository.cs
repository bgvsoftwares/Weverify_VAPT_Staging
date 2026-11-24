using DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.VerificationViewModel;
using System.Data;

namespace BAL.VerificationRepository
{
    public class PQEducationVerRepository : IPQEducationVerRepository
    {
        DataContext db;
        public PQEducationVerRepository()
        {
            db = new DataContext();
        }

        public void AddEducationByTeamMember(PQEducationVerViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            PQEducationVer entity = new PQEducationVer();
                            entity.EducationRowID = model.EducationRowID;
                            entity.EV_Cand_Name = model.EV_Cand_Name;
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
                            entity.EV_Rslt_Obtain = model.EV_Rslt_Obtain;
                            entity.EV_Mode_of_Verf = model.EV_Mode_of_Verf;
                            entity.EV_Refre_Name_Dsg = model.EV_Refre_Name_Dsg;
                            entity.EV_Refre_Eid = model.EV_Refre_Eid;
                            entity.EV_Refre_ConNo = model.EV_Refre_ConNo;
                            entity.EV_Attch_Doc_Genuine = model.EV_Attch_Doc_Genuine;
                            entity.EV_Any_Other_Cmmnts_EduV = model.EV_Any_Other_Cmmnts_EduV;
                            entity.EV_Date_Time_Verf = model.EV_Date_Time_Verf;
                            entity.EV_Verb_Verf_Date = model.EV_Verb_Verf_Date;
                            entity.EV_Wrtn_Verf_Date = model.EV_Wrtn_Verf_Date;
                            entity.EV_Other_Remarks = model.EV_Other_Remarks;
                            entity.EV_Division_CGPA = model.EV_Division_CGPA;
                            entity.EV_Official_Fee_Proof = model.EV_Official_Fee_Proof;
                            entity.EV_Colg_Unvrsty_Affl = model.EV_Colg_Unvrsty_Affl;
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
                            entity.EV_Others11 = model.EV_Others11;
                            entity.EV_Others12 = model.EV_Others12;
                            entity.EV_Others13 = model.EV_Others13;
                            entity.EV_Others14 = model.EV_Others14;
                            entity.EV_Others15 = model.EV_Others15;
                            entity.ATA_Reverse_Directory = model.ATA_Reverse_Directory;
                            entity.ATA_Residcl_Commrcl = model.ATA_Residcl_Commrcl;
                            entity.ATA_Stock_Exchnge = model.ATA_Stock_Exchnge;
                            entity.ATA_Telphne_Directry_Srch = model.ATA_Telphne_Directry_Srch;
                            entity.ATA_Yellow_Pages = model.ATA_Yellow_Pages;
                            entity.ATA_Who_Domain = model.ATA_Who_Domain;
                            entity.ATA_GoogleSearch = model.ATA_GoogleSearch;
                            entity.ATA_InfrastructureOfCmp = model.ATA_InfrastructureOfCmp;
                            entity.ATA_Just_Dial = model.ATA_Just_Dial;
                            entity.ATA_Neighbor_Check1 = model.ATA_Neighbor_Check1;
                            entity.ATA_NASCOM_Empanelment = model.ATA_NASCOM_Empanelment;
                            entity.ATA_Photograph = model.ATA_Photograph;
                            entity.ATA_Physical_Site = model.ATA_Physical_Site;
                            entity.ATA_Reg_Company = model.ATA_Reg_Company;
                            entity.ATA_PO_Courier_Check = model.ATA_PO_Courier_Check;
                            entity.ATA_Network_Sol = model.ATA_Network_Sol;
                            entity.ATA_ApprxEmpWorking = model.ATA_ApprxEmpWorking;
                            entity.ATA_CID_No = model.ATA_CID_No;
                            entity.ATA_Cmpny_Addr = model.ATA_Cmpny_Addr;
                            entity.ATA_Decoy_Call = model.ATA_Decoy_Call;
                            entity.ATA_Cmpny_Website = model.ATA_Cmpny_Website;
                            entity.CheckStatus = model.CheckStatus;
                            entity.Status = model.Status;
                           entity.VerifiedDate = model.VerifiedDate;
                            context.PQEducationVers.Add(entity);
                            context.SaveChanges();

                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).CheckStatus = "WIP-Pending";
                            context.SaveChanges();

                            int statusId = context.PQEducationStatus.Where(p => p.EducationRowId == model.EducationRowID).FirstOrDefault().EducationStatusRowID;
                            context.PQEducationStatus.Single(c => c.EducationStatusRowID == statusId).TMCheckStatus = "WIP-Pending";
                            context.PQEducationStatus.Single(c => c.EducationStatusRowID == statusId).TMQCCheckStatus = "";
                            context.SaveChanges();

                            transaction.Commit();
                        }
                        else
                        {
                            throw new Exception("All * fields are mandatory!");
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

        public void AddEducationVerCloserDetails(EducationVerCloserViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model.DispositionID != null)
                        {
                            #region Disposition
                            string[] DispositionIds = model.DispositionID.Split(',');
                            byte[] SeverityRows = new byte[DispositionIds.Length];
                            List<PQEducationVerDisposition> VerDispositionsList = new List<PQEducationVerDisposition>();
                            var FindEducationVerDisposition = context.PQEducationVerDispositions.Where(a => a.EducationVerRowID == model.EducationVerRowID).ToList();

                            if (FindEducationVerDisposition != null && FindEducationVerDisposition.Count > 0)
                            {
                                context.PQEducationVerDispositions.RemoveRange(FindEducationVerDisposition);
                                context.SaveChanges();
                            }
                            foreach (var item in DispositionIds)
                            {
                                PQEducationVerDisposition VerEntity = new PQEducationVerDisposition
                                {
                                    EducationVerRowID = model.EducationVerRowID,
                                    ClientDispositionRowId = Convert.ToInt32(item)
                                };
                                VerDispositionsList.Add(VerEntity);
                            }
                            context.PQEducationVerDispositions.AddRange(VerDispositionsList);
                            context.SaveChanges();
                            #endregion
                        }
                        //
                        //Update in EducationVer Table            
                        context.PQEducationVers.Single(c => c.EducationVerRowID == model.EducationVerRowID).ReportComments = model.Remarks;
                        context.PQEducationVers.Single(c => c.EducationVerRowID == model.EducationVerRowID).Severity = model.Severity;
                        context.PQEducationVers.Single(c => c.EducationVerRowID == model.EducationVerRowID).ColorName = model.ColorName;
                        context.PQEducationVers.Single(c => c.EducationVerRowID == model.EducationVerRowID).CheckStatus = model.CheckStatus;
                        context.SaveChanges();

                        int statusId = context.PQEducationStatus.Where(p => p.EducationRowId == model.EducationRowID).FirstOrDefault().EducationStatusRowID;

                        if (model.CheckStatus == "Review Requested")
                        {
                            context.PQEducations.Single(p => p.EducationRowId == model.EducationRowID).CheckStatus = model.CheckStatus;
                            context.SaveChanges();

                            context.PQEducationStatus.Single(p => p.EducationStatusRowID == statusId).TMCheckStatus = model.CheckStatus;
                            context.PQEducationStatus.Single(p => p.EducationStatusRowID == statusId).Remarks = model.ActionHistory;
                            context.SaveChanges();
                        }
                        if (model.CheckStatus == "Completed")
                        {
                            ClientCustomize cc = new ClientCustomize();
                            DataSet dd = new DataSet();
                            dd = cc.getInformationFromDepartmentRowID(model.EducationRowID, "Education");
                            int personalRowid = 0;
                            int subCheckRowID = 0;
                            int modifiedBy = 0;
                            if (dd.Tables[0].Rows.Count > 0)
                            {
                                personalRowid = int.Parse((Convert.ToString(dd.Tables[0].Rows[0]["PersonalRowID"])));
                                subCheckRowID = int.Parse((Convert.ToString(dd.Tables[0].Rows[0]["SubCheckRowID"])));
                                modifiedBy = int.Parse((Convert.ToString(dd.Tables[0].Rows[0]["ModifiedBy"])));
                            }
                            cc.UpdateCheckStatus(personalRowid, subCheckRowID, 1, 24, "Check", modifiedBy);
                            context.PQEducations.Single(p => p.EducationRowId == model.EducationRowID).CheckStatus = "Completed-TM";
                            context.SaveChanges();

                            context.PQEducationStatus.Single(p => p.EducationStatusRowID == statusId).TMCheckStatus = model.CheckStatus;
                            context.PQEducationStatus.Single(p => p.EducationStatusRowID == statusId).TMQCCheckStatus = model.CheckStatus;
                            context.SaveChanges();
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

        public PQEducationVerViewModel GetEducationForTM(int EducationRowId = 0)
        {
            try
            {
                PQEducationVerViewModel model = new PQEducationVerViewModel();
                var m = db.PQEducations.Where(b => b.EducationRowId == EducationRowId).FirstOrDefault();
                int DegreeRowID = Convert.ToInt32(m.EV_Degree_Name);
                string degreeNM = string.Empty;
                if (DegreeRowID > 0)
                {
                    degreeNM = db.MasterDegreeTypes.Where(d => d.DegreeRowID == DegreeRowID).FirstOrDefault().DegreeType;
                }

                int CollegeID = Convert.ToInt32(m.EV_Colg_Schl_Inst_Name);
                string ColgNameLabel1 = string.Empty;
                if (CollegeID > 0)
                {
                    ColgNameLabel1 = db.MasterColleges.Where(d => d.CollegeRowID == CollegeID).FirstOrDefault().CollegeName;
                }
                string UniversityNM = string.Empty;
                int univerId=db.MasterColleges.Where(t=>t.CollegeRowID== CollegeID).FirstOrDefault().UniversityRowID;
                if (univerId > 0)
                {
                    UniversityNM = db.MasterUniversities.Where(d => d.UniversityRowID == univerId).FirstOrDefault().UniversityName;
                }


                model = db.PQEducations.Where(a => a.EducationRowId == EducationRowId).Select(a => new PQEducationVerViewModel
                {
                    EducationRowID = a.EducationRowId,
                    // EV_Cand_Name = a.EV_Cand_Name,


                    EV_Cand_Name = db.PQPersonals.Where(T => T.PersonalRowID == a.PersonalRowID).Select(x => x.Name + " " + x.MiddleName + x.LastName).FirstOrDefault().ToString(),

                    EV_Colg_Schl_Inst_NameId = a.EV_Colg_Schl_Inst_Name,
                    EV_Colg_Schl_Inst_Name = db.MasterColleges.Where(c => c.CollegeRowID == a.EV_Colg_Schl_Inst_Name).FirstOrDefault().CollegeName,
                    EV_Cand_Name_Supp_Edu_Doc = a.EV_Cand_Name_Supp_Edu_Doc,
                    EV_Colg_Schl_Inst_Add_ConDet = a.EV_Colg_Schl_Inst_Add_ConDet,
                    EV_Mode_Of_Edu = a.EV_Mode_Of_Edu,
                    EV_Enroll_RollNo = a.EV_Enroll_RollNo,
                    EV_RegNo_RegCode = a.EV_RegNo_RegCode,
                    EV_Degree_Name= degreeNM,
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
                    EV_Affl_University_Name= UniversityNM,
                    ColgNameLabel = ColgNameLabel1,
                }).FirstOrDefault();


                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PQEducationVerViewModel GetEducationInfoForDataEntry(int EducationRowId = 0)
        {
            try
            {
                PQEducationVerViewModel model = new PQEducationVerViewModel();
                var EducationVer = db.PQEducationVers.Where(a => a.EducationRowID == EducationRowId).FirstOrDefault();
                int EducationVerRowId = 0;
                if (EducationVer != null && EducationVer.EducationVerRowID > 0)
                {
                    EducationVerRowId = EducationVer.EducationVerRowID;
                }
            
                if (EducationVerRowId > 0)
                {

                    string ColgLabel = "";
                    string uniLabel = "";
                    ClientCustomize cc = new ClientCustomize();
                    DataTable dt = cc.GetCollegeNameForVer(EducationVerRowId);
                    if(dt.Rows.Count>0)
                    {
                        ColgLabel = dt.Rows[0]["CollegeName"].ToString();
                        uniLabel = dt.Rows[0]["UniversityName"].ToString();
                    }
                    else
                    {
                        ColgLabel = "";
                        uniLabel = "";

                    }
                 

                    #region MyRegion
                    model = db.PQEducationVers.Where(a => a.EducationVerRowID == EducationVerRowId).Select(a => new PQEducationVerViewModel
                    {
                        EducationRowID = a.EducationRowID,
                        EducationVerRowID = a.EducationVerRowID,
                        ClientRowID = a.PQEducation.ClientRowID,
                        PersonalRowID = a.PQEducation.PersonalRowID,
                        CheckFamilyRowID = a.PQEducation.CheckFamilyRowID,
                        SubCheckRowID = a.PQEducation.SubCheckRowID,
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
                        EV_Rslt_Obtain = a.EV_Rslt_Obtain,
                        EV_Mode_of_Verf = a.EV_Mode_of_Verf,
                        EV_Refre_Name_Dsg = a.EV_Refre_Name_Dsg,
                        EV_Refre_Eid = a.EV_Refre_Eid,
                        EV_Refre_ConNo = a.EV_Refre_ConNo,
                        EV_Attch_Doc_Genuine = a.EV_Attch_Doc_Genuine,
                        EV_Any_Other_Cmmnts_EduV = a.EV_Any_Other_Cmmnts_EduV,
                        EV_Date_Time_Verf = a.EV_Date_Time_Verf,       
                        EV_Verb_Verf_Date = a.EV_Verb_Verf_Date,
                        EV_Wrtn_Verf_Date = a.EV_Wrtn_Verf_Date,
                        EV_Other_Remarks = a.EV_Other_Remarks,
                        EV_Division_CGPA = a.EV_Division_CGPA,
                        EV_Official_Fee_Proof = a.EV_Official_Fee_Proof,
                        EV_Colg_Unvrsty_Affl = a.EV_Colg_Unvrsty_Affl,
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
                        EV_Others11 = a.EV_Others11,
                        EV_Others12 = a.EV_Others12,
                        EV_Others13 = a.EV_Others13,
                        EV_Others14 = a.EV_Others14,
                        EV_Others15 = a.EV_Others15,
                        ATA_Reverse_Directory = a.ATA_Reverse_Directory,
                        ATA_Residcl_Commrcl = a.ATA_Residcl_Commrcl,
                        ATA_Stock_Exchnge = a.ATA_Stock_Exchnge,
                        ATA_Telphne_Directry_Srch = a.ATA_Telphne_Directry_Srch,
                        ATA_Yellow_Pages = a.ATA_Yellow_Pages,
                        ATA_Who_Domain = a.ATA_Who_Domain,
                        ATA_GoogleSearch = a.ATA_GoogleSearch,
                        ATA_InfrastructureOfCmp = a.ATA_InfrastructureOfCmp,
                        ATA_Just_Dial = a.ATA_Just_Dial,
                        ATA_Neighbor_Check1 = a.ATA_Neighbor_Check1,
                        ATA_NASCOM_Empanelment = a.ATA_NASCOM_Empanelment,
                        ATA_Photograph = a.ATA_Photograph,
                        ATA_Physical_Site = a.ATA_Physical_Site,
                        ATA_Reg_Company = a.ATA_Reg_Company,
                        ATA_PO_Courier_Check = a.ATA_PO_Courier_Check,
                        ATA_Network_Sol = a.ATA_Network_Sol,
                        ATA_ApprxEmpWorking = a.ATA_ApprxEmpWorking,
                        ATA_CID_No = a.ATA_CID_No,
                        ATA_Cmpny_Addr = a.ATA_Cmpny_Addr,
                        ATA_Decoy_Call = a.ATA_Decoy_Call,
                        ATA_Cmpny_Website = a.ATA_Cmpny_Website,
                        CheckStatus = a.PQEducation.CheckStatus,
                        ColgNameLabel = ColgLabel,
                        UniversityNameLabel= uniLabel,
                    }).FirstOrDefault();
                    #endregion
                }
                else
                {
                    #region MyRegion
                    model = db.PQEducations.Where(a => a.EducationRowId == EducationRowId).Select(a => new PQEducationVerViewModel
                    {
                        EducationRowID = a.EducationRowId,
                        ClientRowID = a.ClientRowID,
                        PersonalRowID = a.PersonalRowID,
                        CheckFamilyRowID = a.CheckFamilyRowID,
                        SubCheckRowID = a.SubCheckRowID,
                        EV_Cand_Name = a.EV_Cand_Name,
                        EV_Cand_Name_Supp_Edu_Doc = a.EV_Cand_Name_Supp_Edu_Doc,
                        EV_Colg_Schl_Inst_Add_ConDet = a.EV_Colg_Schl_Inst_Add_ConDet,
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

        public UpdatePQEducationVerViewModel GetEducationInfoForTMQC(int EducationRowId = 0)
        {
            try
            {
                UpdatePQEducationVerViewModel model = new UpdatePQEducationVerViewModel();
                var EducationVer = db.PQEducationVers.Where(a => a.EducationRowID == EducationRowId).FirstOrDefault();
                int EducationVerRowId = 0;
                if (EducationVer != null && EducationVer.EducationVerRowID > 0)
                {
                    EducationVerRowId = EducationVer.EducationVerRowID;
                }
                if (EducationVerRowId > 0)
                {
                    #region ***** PQEducationVers *****
                    var a = db.PQEducationVers.Where(b => b.EducationVerRowID == EducationVerRowId).FirstOrDefault();

                    if (a != null)
                    {
                        int CollegeRowID = Convert.ToInt32(a.EV_Colg_Schl_Inst_Name);
                        int DegreeRowID = Convert.ToInt32(a.EV_Degree_Name);
                        if (CollegeRowID > 0)
                        {
                            model.EV_Colg_Schl_Inst_Name = db.MasterColleges.Where(d => d.CollegeRowID == CollegeRowID).FirstOrDefault().CollegeName;
                        }
                        if (DegreeRowID > 0)
                        {
                            model.EV_Degree_Name = db.MasterDegreeTypes.Where(d => d.DegreeRowID == DegreeRowID).FirstOrDefault().DegreeType;
                        }
                        model.CollegeRowID = Convert.ToInt32(a.EV_Colg_Schl_Inst_Name);
                        model.DegreeRowID = Convert.ToInt32(a.EV_Degree_Name);

                        model.EducationRowID = a.EducationRowID;
                        model.EducationVerRowID = a.EducationVerRowID;
                        model.ClientRowID = a.PQEducation.ClientRowID;
                        model.PersonalRowID = a.PQEducation.PersonalRowID;
                        model.CheckFamilyRowID = a.PQEducation.CheckFamilyRowID;
                        model.SubCheckRowID = a.PQEducation.SubCheckRowID;
                        //model.EV_Cand_Name = a.EV_Cand_Name;


                        model.EV_Cand_Name = db.PQPersonals.Where(T => T.PersonalRowID == a.PQEducation.PersonalRowID).Select(x => x.Name + " " + x.MiddleName + x.LastName).FirstOrDefault().ToString();

                        model.EV_Cand_Name_Supp_Edu_Doc = a.EV_Cand_Name_Supp_Edu_Doc;
                        model.EV_Colg_Schl_Inst_Add_ConDet = a.EV_Colg_Schl_Inst_Add_ConDet;
                        model.EV_Affl_University_Name = a.EV_Affl_University_Name;
                        model.EV_Mode_Of_Edu = a.EV_Mode_Of_Edu;
                        model.EV_Enroll_RollNo = a.EV_Enroll_RollNo;
                        model.EV_RegNo_RegCode = a.EV_RegNo_RegCode;
                        model.EV_Dur_Course = a.EV_Dur_Course;
                        model.EV_Course_Status = a.EV_Course_Status;
                        model.EV_Specialization = a.EV_Specialization;
                        model.EV_Period_Course = a.EV_Period_Course;
                        model.EV_Yr_Passing = a.EV_Yr_Passing;
                        model.EV_Rslt_Obtain = a.EV_Rslt_Obtain;
                        model.EV_Mode_of_Verf = a.EV_Mode_of_Verf;
                        model.EV_Refre_Name_Dsg = a.EV_Refre_Name_Dsg;
                        model.EV_Refre_Eid = a.EV_Refre_Eid;
                        model.EV_Refre_ConNo = a.EV_Refre_ConNo;
                        model.EV_Attch_Doc_Genuine = a.EV_Attch_Doc_Genuine;
                        model.EV_Any_Other_Cmmnts_EduV = a.EV_Any_Other_Cmmnts_EduV;
                        model.EV_Date_Time_Verf = a.EV_Date_Time_Verf;
                        model.EV_Verb_Verf_Date = a.EV_Verb_Verf_Date;
                        model.EV_Wrtn_Verf_Date = a.EV_Wrtn_Verf_Date;
                        model.EV_Other_Remarks = a.EV_Other_Remarks;
                        model.EV_Division_CGPA = a.EV_Division_CGPA;
                        model.EV_Official_Fee_Proof = a.EV_Official_Fee_Proof;
                        //model.EV_Colg_Unvrsty_Affl = a.EV_Colg_Unvrsty_Affl;
                        int n;
                        bool isNumeric = int.TryParse(a.EV_Colg_Unvrsty_Affl, out n);
                        if (a.EV_Colg_Unvrsty_Affl != null && isNumeric == true)
                        {
                            short EV_Colg_Unvrsty_Affl1 = Convert.ToInt16(a.EV_Colg_Unvrsty_Affl);
                            model.EV_Colg_Unvrsty_Affl = db.MasterUniversities.Where(c => c.UniversityRowID == EV_Colg_Unvrsty_Affl1).FirstOrDefault().UniversityName;
                        }
                        else
                        {
                            model.EV_Colg_Unvrsty_Affl = a.EV_Colg_Unvrsty_Affl;
                        }
                        model.EV_Others = a.EV_Others;
                        model.EV_Others2 = a.EV_Others2;
                        model.EV_Others3 = a.EV_Others3;
                        model.EV_Others4 = a.EV_Others4;
                        model.EV_Others5 = a.EV_Others5;
                        model.EV_Others6 = a.EV_Others6;
                        model.EV_Others7 = a.EV_Others7;
                        model.EV_Others8 = a.EV_Others8;
                        model.EV_Others9 = a.EV_Others9;
                        model.EV_Others10 = a.EV_Others10;
                        model.EV_Others11 = a.EV_Others11;
                        model.EV_Others12 = a.EV_Others12;
                        model.EV_Others13 = a.EV_Others13;
                        model.EV_Others14 = a.EV_Others14;
                        model.EV_Others15 = a.EV_Others15;
                        model.ATA_Reverse_Directory = a.ATA_Reverse_Directory;
                        model.ATA_Residcl_Commrcl = a.ATA_Residcl_Commrcl;
                        model.ATA_Stock_Exchnge = a.ATA_Stock_Exchnge;
                        model.ATA_Telphne_Directry_Srch = a.ATA_Telphne_Directry_Srch;
                        model.ATA_Yellow_Pages = a.ATA_Yellow_Pages;
                        model.ATA_Who_Domain = a.ATA_Who_Domain;
                        model.ATA_GoogleSearch = a.ATA_GoogleSearch;
                        model.ATA_InfrastructureOfCmp = a.ATA_InfrastructureOfCmp;
                        model.ATA_Just_Dial = a.ATA_Just_Dial;
                        model.ATA_Neighbor_Check1 = a.ATA_Neighbor_Check1;
                        model.ATA_NASCOM_Empanelment = a.ATA_NASCOM_Empanelment;
                        model.ATA_Photograph = a.ATA_Photograph;
                        model.ATA_Physical_Site = a.ATA_Physical_Site;
                        model.ATA_Reg_Company = a.ATA_Reg_Company;
                        model.ATA_PO_Courier_Check = a.ATA_PO_Courier_Check;
                        model.ATA_Network_Sol = a.ATA_Network_Sol;
                        model.ATA_ApprxEmpWorking = a.ATA_ApprxEmpWorking;
                        model.ATA_CID_No = a.ATA_CID_No;
                        model.ATA_Cmpny_Addr = a.ATA_Cmpny_Addr;
                        model.ATA_Decoy_Call = a.ATA_Decoy_Call;
                        model.ATA_Cmpny_Website = a.ATA_Cmpny_Website;
                        model.CheckStatus = a.PQEducation.CheckStatus;
                    }

                    #endregion
                }
                else
                {
                    #region  ***** PQEducations *****

                    model = db.PQEducations.Where(a => a.EducationRowId == EducationRowId).Select(a => new UpdatePQEducationVerViewModel
                    {
                        EducationRowID = a.EducationRowId,
                        ClientRowID = a.ClientRowID,
                        CollegeRowID = a.EV_Colg_Schl_Inst_Name,
                        DegreeRowID = a.EV_Degree_Name,
                        PersonalRowID = a.PersonalRowID,
                        CheckFamilyRowID = a.CheckFamilyRowID,
                        SubCheckRowID = a.SubCheckRowID,
                        //EV_Cand_Name = a.EV_Cand_Name,
                        EV_Cand_Name = db.PQPersonals.Where(T => T.PersonalRowID == a.PersonalRowID).Select(x => x.Name + " " + x.MiddleName + x.LastName).FirstOrDefault().ToString(),
                    EV_Cand_Name_Supp_Edu_Doc = a.EV_Cand_Name_Supp_Edu_Doc,
                        EV_Colg_Schl_Inst_Add_ConDet = a.EV_Colg_Schl_Inst_Add_ConDet,
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

        public EducationVerCloserViewModel GetEducationVerCloserDetails(int EduVerId = 0)
        {
            try
            {

                EducationVerCloserViewModel Dispositions = new EducationVerCloserViewModel();
                var Disposition = db.PQEducationVerDispositions.Where(s => s.EducationVerRowID == EduVerId).ToList();
                List<int> selectList = new List<int>();
                foreach (var item in Disposition)
                {
                    selectList.Add(Convert.ToInt32(item.ClientDispositionRowId));
                }
                Dispositions = db.PQEducationVerDispositions.Where(d => d.EducationVerRowID == EduVerId).Select(a => new EducationVerCloserViewModel
                {
                    CDispositionRowId = selectList,
                    ColorName = a.PQEducationVer.ColorName,
                    Severity = a.PQEducationVer.Severity,
                    Remarks = a.PQEducationVer.ReportComments,
                    CheckStatus = a.PQEducationVer.CheckStatus,
                }).FirstOrDefault();

                var entity = db.PQEducationVers.Find(EduVerId);
                if (entity != null)
                {
                    if (Dispositions == null)
                    {
                        Dispositions = new EducationVerCloserViewModel();
                        Dispositions.EducationRowID = entity.EducationRowID;
                        Dispositions.EducationVerRowID = entity.EducationVerRowID;
                        Dispositions.ClientRowID = entity.PQEducation.ClientRowID;
                        Dispositions.CheckFamilyRowID = entity.PQEducation.CheckFamilyRowID;
                        Dispositions.SubCheckRowID = entity.PQEducation.SubCheckRowID;
                        Dispositions.PersonalRowID = entity.PQEducation.PersonalRowID;
                    }
                    else
                    {
                        Dispositions.EducationRowID = entity.EducationRowID;
                        Dispositions.EducationVerRowID = entity.EducationVerRowID;
                        Dispositions.ClientRowID = entity.PQEducation.ClientRowID;
                        Dispositions.CheckFamilyRowID = entity.PQEducation.CheckFamilyRowID;
                        Dispositions.SubCheckRowID = entity.PQEducation.SubCheckRowID;
                        Dispositions.PersonalRowID = entity.PQEducation.PersonalRowID;
                    }
                }
                return Dispositions;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public PQPersonalDetailsVerViewModel GetPersonalDetailsForTM(int EducationRowId = 0)
        {
            try
            {
                PQPersonalDetailsVerViewModel model = new PQPersonalDetailsVerViewModel();
                model = db.PQEducations.Where(a => a.EducationRowId == EducationRowId).Select(a => new PQPersonalDetailsVerViewModel
                {
                    ClientRowID = a.ClientRowID,
                    PersonalRowID = a.PersonalRowID,
                    UniqueComponentID = a.UniqueComponentID,
                    //CandidateName = a.EV_Cand_Name,


                    CandidateName = db.PQPersonals.Where(T => T.PersonalRowID == a.PersonalRowID).Select(x => x.Name + " " + x.MiddleName + x.LastName).FirstOrDefault().ToString(),

                    ClientName = a.PQClientMaster.MasterAbbreviation.ClientName,
                    CompanyReferenceNo = a.EV_Sec_Ref_Id,
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

        public void UpdateEducationByTeamMember(PQEducationVerViewModel model)
        {
            try
            {
                if (model != null)
                {
                    db.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EducationRowID = model.EducationRowID;
                    //db.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Cand_Name = model.EV_Cand_Name;
                    db.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Cand_Name_Supp_Edu_Doc = model.EV_Cand_Name_Supp_Edu_Doc;
                    db.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Colg_Schl_Inst_Name = model.EV_Colg_Schl_Inst_Name;
                    db.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Colg_Schl_Inst_Add_ConDet = model.EV_Colg_Schl_Inst_Add_ConDet;
                    db.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Affl_University_Name = model.EV_Affl_University_Name;
                    db.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Degree_Name = model.EV_Degree_Name;
                    db.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Mode_Of_Edu = model.EV_Mode_Of_Edu;
                    db.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Enroll_RollNo = model.EV_Enroll_RollNo;
                    db.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_RegNo_RegCode = model.EV_RegNo_RegCode;
                    db.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Dur_Course = model.EV_Dur_Course;
                    db.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Course_Status = model.EV_Course_Status;
                    db.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Specialization = model.EV_Specialization;
                    db.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Period_Course = model.EV_Period_Course;
                    db.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Yr_Passing = model.EV_Yr_Passing;
                    db.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Rslt_Obtain = model.EV_Rslt_Obtain;
                    db.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Mode_of_Verf = model.EV_Mode_of_Verf;
                    db.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Refre_Name_Dsg = model.EV_Refre_Name_Dsg;
                    db.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Refre_Eid = model.EV_Refre_Eid;
                    db.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Refre_ConNo = model.EV_Refre_ConNo;
                    db.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Attch_Doc_Genuine = model.EV_Attch_Doc_Genuine;
                    db.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Any_Other_Cmmnts_EduV = model.EV_Any_Other_Cmmnts_EduV;
                    db.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Date_Time_Verf = model.EV_Date_Time_Verf;
                    db.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Verb_Verf_Date = model.EV_Verb_Verf_Date;
                    db.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Wrtn_Verf_Date = model.EV_Wrtn_Verf_Date;
                    db.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Other_Remarks = model.EV_Other_Remarks;
                    db.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Division_CGPA = model.EV_Division_CGPA;
                    db.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Official_Fee_Proof = model.EV_Official_Fee_Proof;
                    db.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Colg_Unvrsty_Affl = model.EV_Colg_Unvrsty_Affl;
                    db.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Others = model.EV_Others;
                    db.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Others2 = model.EV_Others2;
                    db.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Others3 = model.EV_Others3;
                    db.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Others4 = model.EV_Others4;
                    db.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Others5 = model.EV_Others5;
                    db.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Others6 = model.EV_Others6;
                    db.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Others7 = model.EV_Others7;
                    db.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Others8 = model.EV_Others8;
                    db.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Others9 = model.EV_Others9;
                    db.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Others10 = model.EV_Others10;
                    db.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Others11 = model.EV_Others11;
                    db.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Others12 = model.EV_Others12;
                    db.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Others13 = model.EV_Others13;
                    db.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Others14 = model.EV_Others14;
                    db.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Others15 = model.EV_Others15;
                    db.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).ATA_Reverse_Directory = model.ATA_Reverse_Directory;
                    db.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).ATA_Residcl_Commrcl = model.ATA_Residcl_Commrcl;
                    db.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).ATA_Stock_Exchnge = model.ATA_Stock_Exchnge;
                    db.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).ATA_Telphne_Directry_Srch = model.ATA_Telphne_Directry_Srch;
                    db.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).ATA_Yellow_Pages = model.ATA_Yellow_Pages;
                    db.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).ATA_Who_Domain = model.ATA_Who_Domain;
                    db.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).ATA_GoogleSearch = model.ATA_GoogleSearch;
                    db.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).ATA_InfrastructureOfCmp = model.ATA_InfrastructureOfCmp;
                    db.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).ATA_Just_Dial = model.ATA_Just_Dial;
                    db.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).ATA_Neighbor_Check1 = model.ATA_Neighbor_Check1;
                    db.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).ATA_NASCOM_Empanelment = model.ATA_NASCOM_Empanelment;
                    db.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).ATA_Photograph = model.ATA_Photograph;
                    db.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).ATA_Physical_Site = model.ATA_Physical_Site;
                    db.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).ATA_Reg_Company = model.ATA_Reg_Company;
                    db.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).ATA_PO_Courier_Check = model.ATA_PO_Courier_Check;
                    db.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).ATA_Network_Sol = model.ATA_Network_Sol;
                    db.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).ATA_ApprxEmpWorking = model.ATA_ApprxEmpWorking;
                    db.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).ATA_CID_No = model.ATA_CID_No;
                    db.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).ATA_Cmpny_Addr = model.ATA_Cmpny_Addr;
                    db.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).ATA_Decoy_Call = model.ATA_Decoy_Call;
                    db.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).ATA_Cmpny_Website = model.ATA_Cmpny_Website;
                    db.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).VerifiedDate = model.VerifiedDate;
                }
                else
                {
                    throw new Exception("Education could not be blank!");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void UpdateEducationByTMQC(UpdatePQEducationVerViewModel model, short RptQCManagerId = 0)
        {

            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            int statusId = context.PQEducationStatus.Where(p => p.EducationRowId == model.EducationRowID).FirstOrDefault().EducationStatusRowID;

                            if (model.CheckStatus == "Rejected")
                            {
                                #region Update Check Status

                                string ActualCheckStatus = "WIP-Pending";
                                context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).CheckStatus = ActualCheckStatus;
                                context.SaveChanges();

                                context.PQEducationStatus.Single(c => c.EducationStatusRowID == statusId).TMCheckStatus = ActualCheckStatus;
                                context.PQEducationStatus.Single(c => c.EducationStatusRowID == statusId).TMQCCheckStatus = "";
                                context.PQEducationStatus.Single(c => c.EducationStatusRowID == statusId).Remarks = model.CloserViewModel.ActionHistory;
                                context.SaveChanges();

                                #endregion
                            }
                            else
                            {
                                context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).CheckStatus = model.CheckStatus;
                                context.SaveChanges();

                                if (model.CheckStatus == "Insufficent")
                                {
                                    context.PQEducationStatus.Single(c => c.EducationStatusRowID == statusId).TMQCCheckStatus = "Insufficent-QC";
                                    context.PQEducationStatus.Single(c => c.EducationStatusRowID == statusId).InfSuffClearBy = 0;
                                    context.PQEducationStatus.Single(c => c.EducationStatusRowID == statusId).InfSuffClearComments = "";
                                    context.PQEducationStatus.Single(c => c.EducationStatusRowID == statusId).InfSuffClearDate = null;
                                    context.SaveChanges();

                                    context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).CheckStatus = "Insufficent-QC";
                                    context.SaveChanges();

                                    context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).CaseStatus = model.CheckStatus;
                                    context.SaveChanges();
                                }

                                if (model.CheckStatus == "Funds Pending")
                                {
                                    context.PQEducationStatus.Single(c => c.EducationStatusRowID == statusId).TMQCCheckStatus = "Funds Pending-QC";
                                    context.SaveChanges();

                                    context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).CheckStatus = "Funds Pending-QC";
                                    context.SaveChanges();

                                    context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).CaseStatus = model.CheckStatus;
                                    context.SaveChanges();
                                }

                                if (model.EducationVerRowID > 0 && model.CheckStatus != "Insufficent")
                                {
                                    #region Update Education Ver Info By TMQC

                                    context.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EducationRowID = model.EducationRowID;
                                    //context.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Cand_Name = model.EV_Cand_Name;
                                    context.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Cand_Name_Supp_Edu_Doc = model.EV_Cand_Name_Supp_Edu_Doc;
                                    context.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Colg_Schl_Inst_Name = Convert.ToString(model.CollegeRowID);
                                    context.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Colg_Schl_Inst_Add_ConDet = model.EV_Colg_Schl_Inst_Add_ConDet;
                                    context.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Affl_University_Name = model.EV_Affl_University_Name;
                                    context.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Degree_Name = Convert.ToString(model.DegreeRowID);
                                    context.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Mode_Of_Edu = model.EV_Mode_Of_Edu;
                                    context.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Enroll_RollNo = model.EV_Enroll_RollNo;
                                    context.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_RegNo_RegCode = model.EV_RegNo_RegCode;
                                    context.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Dur_Course = model.EV_Dur_Course;
                                    context.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Course_Status = model.EV_Course_Status;
                                    context.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Specialization = model.EV_Specialization;
                                    context.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Period_Course = model.EV_Period_Course;
                                    context.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Yr_Passing = model.EV_Yr_Passing;
                                    context.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Rslt_Obtain = model.EV_Rslt_Obtain;
                                    context.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Mode_of_Verf = model.EV_Mode_of_Verf;
                                    context.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Refre_Name_Dsg = model.EV_Refre_Name_Dsg;
                                    context.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Refre_Eid = model.EV_Refre_Eid;
                                    context.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Refre_ConNo = model.EV_Refre_ConNo;
                                    context.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Attch_Doc_Genuine = model.EV_Attch_Doc_Genuine;
                                    context.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Any_Other_Cmmnts_EduV = model.EV_Any_Other_Cmmnts_EduV;
                                    context.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Date_Time_Verf = model.EV_Date_Time_Verf;
                                    context.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Verb_Verf_Date = model.EV_Verb_Verf_Date;
                                    context.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Wrtn_Verf_Date = model.EV_Wrtn_Verf_Date;
                                    context.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Other_Remarks = model.EV_Other_Remarks;
                                    context.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Division_CGPA = model.EV_Division_CGPA;
                                    context.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Official_Fee_Proof = model.EV_Official_Fee_Proof;
                                    context.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Colg_Unvrsty_Affl = model.EV_Colg_Unvrsty_Affl;
                                    context.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Others = model.EV_Others;
                                    context.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Others2 = model.EV_Others2;
                                    context.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Others3 = model.EV_Others3;
                                    context.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Others4 = model.EV_Others4;
                                    context.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Others5 = model.EV_Others5;
                                    context.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Others6 = model.EV_Others6;
                                    context.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Others7 = model.EV_Others7;
                                    context.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Others8 = model.EV_Others8;
                                    context.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Others9 = model.EV_Others9;
                                    context.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Others10 = model.EV_Others10;
                                    context.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Others11 = model.EV_Others11;
                                    context.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Others12 = model.EV_Others12;
                                    context.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Others13 = model.EV_Others13;
                                    context.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Others14 = model.EV_Others14;
                                    context.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Others15 = model.EV_Others15;
                                    context.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).ATA_Reverse_Directory = model.ATA_Reverse_Directory;
                                    context.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).ATA_Residcl_Commrcl = model.ATA_Residcl_Commrcl;
                                    context.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).ATA_Stock_Exchnge = model.ATA_Stock_Exchnge;
                                    context.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).ATA_Telphne_Directry_Srch = model.ATA_Telphne_Directry_Srch;
                                    context.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).ATA_Yellow_Pages = model.ATA_Yellow_Pages;
                                    context.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).ATA_Who_Domain = model.ATA_Who_Domain;
                                    context.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).ATA_GoogleSearch = model.ATA_GoogleSearch;
                                    context.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).ATA_InfrastructureOfCmp = model.ATA_InfrastructureOfCmp;
                                    context.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).ATA_Just_Dial = model.ATA_Just_Dial;
                                    context.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).ATA_Neighbor_Check1 = model.ATA_Neighbor_Check1;
                                    context.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).ATA_NASCOM_Empanelment = model.ATA_NASCOM_Empanelment;
                                    context.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).ATA_Photograph = model.ATA_Photograph;
                                    context.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).ATA_Physical_Site = model.ATA_Physical_Site;
                                    context.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).ATA_Reg_Company = model.ATA_Reg_Company;
                                    context.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).ATA_PO_Courier_Check = model.ATA_PO_Courier_Check;
                                    context.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).ATA_Network_Sol = model.ATA_Network_Sol;
                                    context.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).ATA_ApprxEmpWorking = model.ATA_ApprxEmpWorking;
                                    context.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).ATA_CID_No = model.ATA_CID_No;
                                    context.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).ATA_Cmpny_Addr = model.ATA_Cmpny_Addr;
                                    context.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).ATA_Decoy_Call = model.ATA_Decoy_Call;
                                    context.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).ATA_Cmpny_Website = model.ATA_Cmpny_Website;
                                    context.PQEducationVers.Single(a => a.EducationVerRowID == model.EducationVerRowID).EV_Date_Time_Verf = model.EV_Date_Time_Verf;

                                    context.SaveChanges();

                                    #endregion

                                    #region Save Disposition/Severity
                                    if (model.CloserViewModel.DispositionID != null)
                                    {
                                        #region Disposition
                                        string[] DispositionIds = model.CloserViewModel.DispositionID.Split(',');
                                        byte[] SeverityRows = new byte[DispositionIds.Length];
                                        List<PQEducationVerDisposition> VerDispositionsList = new List<PQEducationVerDisposition>();
                                        var FindVerDisposition = context.PQEducationVerDispositions.Where(a => a.EducationVerRowID == model.EducationVerRowID).ToList();

                                        if (FindVerDisposition != null && FindVerDisposition.Count > 0)
                                        {
                                            context.PQEducationVerDispositions.RemoveRange(FindVerDisposition);
                                            context.SaveChanges();
                                        }
                                        foreach (var item in DispositionIds)
                                        {
                                            PQEducationVerDisposition VerEntity = new PQEducationVerDisposition
                                            {
                                                EducationVerRowID = model.EducationVerRowID,
                                                ClientDispositionRowId = Convert.ToInt32(item)
                                            };
                                            VerDispositionsList.Add(VerEntity);
                                        }
                                        context.PQEducationVerDispositions.AddRange(VerDispositionsList);
                                        context.SaveChanges();
                                        #endregion
                                    }

                                    //Update in EducationVer Table            
                                    context.PQEducationVers.Single(c => c.EducationVerRowID == model.EducationVerRowID).ReportComments = model.CloserViewModel.Remarks;
                                    context.PQEducationVers.Single(c => c.EducationVerRowID == model.EducationVerRowID).Severity = model.CloserViewModel.Severity;
                                    context.PQEducationVers.Single(c => c.EducationVerRowID == model.EducationVerRowID).ColorName = model.CloserViewModel.ColorName;
                                    context.PQEducationVers.Single(c => c.EducationVerRowID == model.EducationVerRowID).CheckStatus = model.CheckStatus;
                                    context.SaveChanges();
                                    #endregion

                                    #region ***** Completed-TM & Rejected-RWQC*****
                                    // This code is added for Rejected checks by RW-TM          || model.CheckStatus == "Rejected-RWQC"
                                    if (model.CheckStatus == "Completed-TM" || model.CheckStatus == "Rejected-RWQC")
                                    {
                                        string allocateMgr = "Allocated-Mgr";
                                        if (model.CloserViewModel.ColorName.ToLower() == "green")
                                        {
                                            context.PQEducationStatus.Single(c => c.EducationStatusRowID == statusId).TMQCCheckStatus = "Completed-QC";
                                            context.PQEducationStatus.Single(c => c.EducationStatusRowID == statusId).RWQCMgrCheckStatus = allocateMgr;
                                            context.PQEducationStatus.Single(c => c.EducationStatusRowID == statusId).RWQCManagerBy = RptQCManagerId;
                                            context.PQEducationStatus.Single(c => c.EducationStatusRowID == statusId).RWQCManagerDate = DateTime.Now;
                                            context.SaveChanges();

                                            short TemaMember = context.PQEducations.Find(model.EducationRowID).PQPersonal.RWQCManagerTo;
                                            if (TemaMember == 0)
                                            {
                                                context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).RWQCMgrCaseStatus = allocateMgr;
                                                context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).RWQCManagerTo = RptQCManagerId;
                                                context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).RWQCManagerDate = DateTime.Now;
                                                context.SaveChanges();
                                            }

                                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).CheckStatus = "Allocated-RWQC";
                                            context.SaveChanges();
                                        }
                                        else
                                        {

                                            allocateMgr = "RW-Approval-TL";
                                            context.PQEducationStatus.Single(c => c.EducationStatusRowID == statusId).TMQCCheckStatus = "Completed-QC";
                                            context.PQEducationStatus.Single(c => c.EducationStatusRowID == statusId).TLCheckStatus = allocateMgr;
                                            context.SaveChanges();

                                            context.PQEducations.Single(c => c.EducationRowId == model.EducationRowID).CheckStatus = allocateMgr;
                                            context.SaveChanges();
                                        }

                                    }

                                    #endregion
                                }
                            }
                            transaction.Commit();
                        }
                        else
                        {
                            throw new Exception("Education could not be blank!");
                        }
                    }

                    catch (Exception)
                    {

                        throw;
                    }
                }
            }
        }

        public string GetCollegeNameForResearch(int EducationRowId = 0)
        {
            try
            {
                var model = db.PQEducations.Where(a => a.EducationRowId == EducationRowId).FirstOrDefault();
                if (model != null && model.EV_Colg_Schl_Inst_Name > 0)
                {
                    return db.MasterColleges.Find(model.EV_Colg_Schl_Inst_Name).CollegeName;
                }
                else
                {
                    return "";
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
