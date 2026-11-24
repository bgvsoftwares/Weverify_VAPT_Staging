using DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.ProvidedInfoViewModel;
using ViewModels.VerificationViewModel;
using System.Data;

namespace BAL.VerificationRepository
{
    public class PQEmploymentVerRepository : IPQEmploymentVerRepository
    {
        DataContext db;

        public PQEmploymentVerRepository()
        {
            db = new DataContext();
        }

        public void AddEmploymentByTeamMember(PQEmploymentVerViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            PQEmploymentVer entity = new PQEmploymentVer();

                            #region Antecedent Fields Added Into PQEmploymentVer

                            entity.EmploymentRowID = model.EmploymentRowID;
                            entity.EV_CandidateName = model.EV_CandidateName;
                            entity.EV_OrganisationName = model.EV_OrganisationName;
                            entity.EV_CmpLocation = model.EV_CmpLocation;
                            entity.EV_CmpNameAddress = model.EV_CmpNameAddress;
                            entity.EV_EmployeeName = model.EV_EmployeeName;
                            entity.EV_EmployeeCode = model.EV_EmployeeCode;
                            entity.EV_EmploymentPeriod = model.EV_EmploymentPeriod;
                            entity.EV_EmploymentDual = model.EV_EmploymentDual;
                            entity.EV_EmployedSelf = model.EV_EmployedSelf;
                            entity.EV_lappingOver = model.EV_lappingOver;
                            entity.EV_CandidateTenureWorked = model.EV_CandidateTenureWorked;
                            entity.EV_EmployeeTenure = model.EV_EmployeeTenure;
                            entity.EV_Designation = model.EV_Designation;
                            entity.EV_EmployedEarlierAlso = model.EV_EmployedEarlierAlso;
                            entity.EV_nature_emplo = model.EV_nature_emplo;
                            entity.EV_typ_employ = model.EV_typ_employ;
                            entity.EV_EmpType = model.EV_EmpType;
                            entity.EV_Department = model.EV_Department;
                            entity.EV_CmpCost = model.EV_CmpCost;
                            entity.EV_last_sal = model.EV_last_sal;
                            entity.EV_leavingReason = model.EV_leavingReason;
                            entity.EV_termina = model.EV_termina;
                            entity.EV_DiscontinueServiceReason = model.EV_DiscontinueServiceReason;
                            entity.EV_EligibilityforRehire = model.EV_EligibilityforRehire;
                            entity.EV_EligibilityOfRehire = model.EV_EligibilityOfRehire;
                            entity.EV_EligibileForRehire = model.EV_EligibileForRehire;
                            entity.EV_Rhre_Aplcnt_Situt_Ars_Ftre = model.EV_Rhre_Aplcnt_Situt_Ars_Ftre;
                            entity.EV_Reported = model.EV_Reported;
                            entity.EV_ReportingManagerName = model.EV_ReportingManagerName;
                            entity.EV_Rptng_Mgr_Dsg_Eid_Tno = model.EV_Rptng_Mgr_Dsg_Eid_Tno;
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
                            entity.EV_OverallJobPerformance = model.EV_OverallJobPerformance;
                            entity.EV_PerfromanceLevel = model.EV_PerfromanceLevel;
                            entity.EV_KnowledgeProcess = model.EV_KnowledgeProcess;
                            entity.EV_ValidationBusinessOperation = model.EV_ValidationBusinessOperation;
                            entity.EV_DutiesNature = model.EV_DutiesNature;
                            entity.EV_Prof_Ablts_Aplcnt = model.EV_Prof_Ablts_Aplcnt;
                            entity.EV_Dsplnry_Perf_Iss_Rptd = model.EV_Dsplnry_Perf_Iss_Rptd;
                            entity.EV_DisciplinaryIssue = model.EV_DisciplinaryIssue;
                            entity.EV_Cmplnts_Rslt_Dsplnry_Sub = model.EV_Cmplnts_Rslt_Dsplnry_Sub;
                            entity.EV_Emp_Invol_Fraud_Actvty = model.EV_Emp_Invol_Fraud_Actvty;
                            entity.EV_PerformanceIssues = model.EV_PerformanceIssues;
                            entity.EV_TeamConduct = model.EV_TeamConduct;
                            entity.EV_TeamPlayerNot = model.EV_TeamPlayerNot;
                            entity.EV_TechnologicalKnowledge = model.EV_TechnologicalKnowledge;
                            entity.EV_Fback_Accnt_Cduct_Jb = model.EV_Fback_Accnt_Cduct_Jb;
                            entity.EV_DisciplinedHowHe = model.EV_DisciplinedHowHe;
                            entity.EV_LongKnown = model.EV_LongKnown;
                            entity.EV_LongWork = model.EV_LongWork;
                            entity.EV_LeavesTook = model.EV_LeavesTook;
                            entity.EV_People_Rptd = model.EV_People_Rptd;
                            entity.EV_Conduct = model.EV_Conduct;
                            entity.EV_Performance = model.EV_Performance;
                            entity.EV_Perf_Rate = model.EV_Perf_Rate;
                            entity.EV_Mtng_Deadline = model.EV_Mtng_Deadline;
                            entity.EV_Comm_Skills = model.EV_Comm_Skills;
                            entity.EV_Wrtn_Comm_Skills = model.EV_Wrtn_Comm_Skills;
                            entity.EV_Verb_Comm_Skills = model.EV_Verb_Comm_Skills;
                            entity.EV_Procs_Knowlg = model.EV_Procs_Knowlg;
                            entity.EV_Attdnce_Punct = model.EV_Attdnce_Punct;
                            entity.EV_Ldrshp_Qualty = model.EV_Ldrshp_Qualty;
                            entity.EV_Mngng_Mtng_Int_Cust_Expect = model.EV_Mngng_Mtng_Int_Cust_Expect;
                            entity.EV_OtherIssues = model.EV_OtherIssues;
                            entity.EV_AdherenceTAT = model.EV_AdherenceTAT;
                            entity.EV_CompletesTATWork = model.EV_CompletesTATWork;
                            entity.EV_Atude_Hon_Per_Aplcnt = model.EV_Atude_Hon_Per_Aplcnt;
                            entity.EV_WorkAttitude = model.EV_WorkAttitude;
                            entity.EV_RatingTeaming = model.EV_RatingTeaming;
                            entity.EV_Rate_Tech = model.EV_Rate_Tech;
                            entity.EV_AttendancePunctuality = model.EV_AttendancePunctuality;
                            entity.EV_Cmnts_Abt_Sub = model.EV_Cmnts_Abt_Sub;
                            entity.EV_SubjectComments = model.EV_SubjectComments;
                            entity.EV_AdditionalComments = model.EV_AdditionalComments;
                            entity.EV_OtherComments = model.EV_OtherComments;
                            entity.EV_OtherRemarks = model.EV_OtherRemarks;
                            entity.EV_AttachedDocumentGenuine = model.EV_AttachedDocumentGenuine;
                            entity.EV_ResponseMode = model.EV_ResponseMode;
                            entity.EV_OnlineVerificationCondctedOn = model.EV_OnlineVerificationCondctedOn;
                            entity.EV_RefereeResponse = model.EV_RefereeResponse;
                            entity.EV_RespondentName = model.EV_RespondentName;
                            entity.EV_RefereeDetails = model.EV_RefereeDetails;
                            entity.EV_RespondentDesignation = model.EV_RespondentDesignation;
                            entity.EV_RefereeNameDetails = model.EV_RefereeNameDetails;
                            entity.EV_DetailsReferee = model.EV_DetailsReferee;
                            entity.EV_ContactDetail = model.EV_ContactDetail;
                            entity.EV_DateTime = model.EV_DateTime;
                            entity.EV_Verfson_Poss_Phy_Visit = model.EV_Verfson_Poss_Phy_Visit;
                            entity.EV_Details_Verfson_EidPh_Phyvst = model.EV_Details_Verfson_EidPh_Phyvst;
                            entity.EV_Nme_Add_Cmpny_Exist_Now = model.EV_Nme_Add_Cmpny_Exist_Now;
                            entity.EV_Name_Dsg_Verfyng_Auth = model.EV_Name_Dsg_Verfyng_Auth;
                            entity.EV_Verified = model.EV_Verified;
                            entity.EV_Verified_by = model.EV_Verified_by;
                            entity.EV_Verified_by_dsg_ConNo = model.EV_Verified_by_dsg_ConNo;
                            entity.EV_Date_Time_Contacted = model.EV_Date_Time_Contacted;
                            entity.EV_OtherDetails6 = model.EV_OtherDetails6;
                            entity.EV_OtherDetails7 = model.EV_OtherDetails7;
                            entity.EV_OtherDetails8 = model.EV_OtherDetails8;
                            entity.EV_OtherDetails9 = model.EV_OtherDetails9;
                            entity.EV_OtherDetails10 = model.EV_OtherDetails10;
                            entity.EV_OtherDetails11 = model.EV_OtherDetails11;
                            entity.EV_OtherDetails12 = model.EV_OtherDetails12;
                            entity.EV_OtherDetails13 = model.EV_OtherDetails13;
                            entity.EV_OtherDetails14 = model.EV_OtherDetails14;
                            entity.EV_OtherDetails15 = model.EV_OtherDetails15;
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
                            entity.Status = model.Status;
                            entity.CheckStatus = model.CheckStatus;
                            entity.VerifiedDate = model.VerifiedDate;

                            #endregion

                            context.PQEmploymentVers.Add(entity);
                            context.SaveChanges();

                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).CheckStatus = "WIP-Pending";
                            context.SaveChanges();

                            int statusId = context.PQEmploymentStatus.Where(p => p.EmploymentRowId == model.EmploymentRowID).FirstOrDefault().EmploymentStatusRowID;

                            context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == statusId).TMCheckStatus = "WIP-Pending";
                            context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == statusId).TMQCCheckStatus = "";
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

        public PQEmploymentVerViewModel GetEmploymentForTM(int EmploymentRowId = 0)
        {
            try
            {
                DataTable dt = new DataTable();

                ClientCustomize cc = new ClientCustomize();

                PQEmploymentVerViewModel model = new PQEmploymentVerViewModel();
                var entity = db.PQEmployments.Where(a => a.EmploymentRowId == EmploymentRowId).FirstOrDefault();
                if (entity != null)
                {
                    int CompanyId = Convert.ToInt32(entity.EV_OrganisationName);
                    string OrganisationName = string.Empty;
                    if (CompanyId > 0)
                    {
                        OrganisationName = db.MasterEmployers.Find(CompanyId).CompanyName;
                    }
                    model.EmploymentRowID = entity.EmploymentRowId;
                    //model.EV_CandidateName = entity.EV_CandidateName;


                    model.EV_CandidateName = db.PQPersonals.Where(T => T.PersonalRowID == entity.PersonalRowID).Select(x => x.Name + " " + x.MiddleName + x.LastName).FirstOrDefault().ToString();

                    model.EV_OrganisationName = entity.EV_OrganisationName;
                    model.EV_OrganisationNameId = OrganisationName;
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
                    model.EV_OtherDetails6 = entity.EV_OtherDetails6;
                    model.EV_OtherDetails7 = entity.EV_OtherDetails7;
                    model.EV_OtherDetails8 = entity.EV_OtherDetails8;
                    model.EV_OtherDetails9 = entity.EV_OtherDetails9;
                    model.EV_OtherDetails10 = entity.EV_OtherDetails10;
                    model.ATA_CID_No = entity.ATA_CID_No;
                    model.ATA_Cmpny_Addr = entity.ATA_Cmpny_Addr;
                }
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PQEmploymentVerViewModel GetEmploymentInfoForDataEntry(int EmploymentRowId = 0)
        {
            try
            {
                DataTable dt = new DataTable();
                ClientCustomize cc = new ClientCustomize();
                PQEmploymentVerViewModel model = new PQEmploymentVerViewModel();
                var EmploymentVer = db.PQEmploymentVers.Where(a => a.EmploymentRowID == EmploymentRowId).FirstOrDefault();
                int EmploymentVerRowId = 0;
                if (EmploymentVer != null && EmploymentVer.EmploymentVerRowID > 0)
                {
                    EmploymentVerRowId = EmploymentVer.EmploymentVerRowID;
                }

                if (EmploymentVerRowId > 0)
                {
                    #region ***** PQEmploymentVers *****

                    var a = db.PQEmploymentVers.Where(e => e.EmploymentVerRowID == EmploymentVerRowId).FirstOrDefault();
                    int CompanyId = Convert.ToInt32(a.EV_OrganisationName);
                    string OrganisationName = string.Empty;
                    if (CompanyId > 0)
                    {
                        OrganisationName = db.MasterEmployers.Find(CompanyId).CompanyName;
                    }
                    model.EmploymentRowID = a.EmploymentRowID;
                    model.EmploymentVerRowID = a.EmploymentVerRowID;
                    model.ClientRowID = a.PQEmployment.ClientRowID;
                    model.PersonalRowID = a.PQEmployment.PersonalRowID;
                    model.CheckFamilyRowID = a.PQEmployment.CheckFamilyRowID;
                    model.SubCheckRowID = a.PQEmployment.SubCheckRowID;
                    model.EV_CandidateName = a.EV_CandidateName;
                    model.EV_OrganisationName = a.EV_OrganisationName;
                    model.EV_OrganisationNameId = OrganisationName;
                    model.EV_CmpLocation = a.EV_CmpLocation;
                    dt = cc.GetComapnyAddress(Convert.ToInt32(model.EV_OrganisationName));
                    if (dt.Rows.Count > 0)
                    {
                        model.EV_CmpNameAddress = dt.Rows[0]["CompanyAddress"].ToString();
                    }

                   // model.EV_CmpNameAddress = a.EV_CmpNameAddress;
                    model.EV_EmployeeName = a.EV_EmployeeName;
                    model.EV_EmployeeCode = a.EV_EmployeeCode;
                    model.EV_EmploymentPeriod = a.EV_EmploymentPeriod;
                    model.EV_EmploymentDual = a.EV_EmploymentDual;
                    model.EV_EmployedSelf = a.EV_EmployedSelf;
                    model.EV_lappingOver = a.EV_lappingOver;
                    model.EV_CandidateTenureWorked = a.EV_CandidateTenureWorked;
                    model.EV_EmployeeTenure = a.EV_EmployeeTenure;
                    model.EV_Designation = a.EV_Designation;
                    model.EV_EmployedEarlierAlso = a.EV_EmployedEarlierAlso;
                    model.EV_nature_emplo = a.EV_nature_emplo;
                    model.EV_typ_employ = a.EV_typ_employ;
                    model.EV_EmpType = a.EV_EmpType;
                    model.EV_Department = a.EV_Department;
                    model.EV_CmpCost = a.EV_CmpCost;
                    model.EV_last_sal = a.EV_last_sal;
                    model.EV_leavingReason = a.EV_leavingReason;
                    model.EV_termina = a.EV_termina;
                    model.EV_DiscontinueServiceReason = a.EV_DiscontinueServiceReason;
                    model.EV_EligibilityforRehire = a.EV_EligibilityforRehire;
                    model.EV_EligibilityOfRehire = a.EV_EligibilityOfRehire;
                    model.EV_EligibileForRehire = a.EV_EligibileForRehire;
                    model.EV_Rhre_Aplcnt_Situt_Ars_Ftre = a.EV_Rhre_Aplcnt_Situt_Ars_Ftre;
                    model.EV_Reported = a.EV_Reported;
                    model.EV_ReportingManagerName = a.EV_ReportingManagerName;
                    model.EV_Rptng_Mgr_Dsg_Eid_Tno = a.EV_Rptng_Mgr_Dsg_Eid_Tno;
                    model.EV_Rptng_Mgr_Name_Ph = a.EV_Rptng_Mgr_Name_Ph;
                    model.EV_Rptng_Mgr_Name_ConDet = a.EV_Rptng_Mgr_Name_ConDet;
                    model.EV_Sub_Rptng_Mgr_Name_Dsg_EidPh = a.EV_Sub_Rptng_Mgr_Name_Dsg_EidPh;
                    model.EV_Supervisor = a.EV_Supervisor;
                    model.EV_Sup_Name_Dsg = a.EV_Sup_Name_Dsg;
                    model.EV_FormalitiesExit = a.EV_FormalitiesExit;
                    model.EV_FullFinalFormalities = a.EV_FullFinalFormalities;
                    model.EV_ExitFormalitiesPending = a.EV_ExitFormalitiesPending;
                    model.EV_HandledResponsibilities = a.EV_HandledResponsibilities;
                    model.EV_ResponsibilitiesHandled = a.EV_ResponsibilitiesHandled;
                    model.EV_OverallJobPerformance = a.EV_OverallJobPerformance;
                    model.EV_PerfromanceLevel = a.EV_PerfromanceLevel;
                    model.EV_KnowledgeProcess = a.EV_KnowledgeProcess;
                    model.EV_ValidationBusinessOperation = a.EV_ValidationBusinessOperation;
                    model.EV_DutiesNature = a.EV_DutiesNature;
                    model.EV_Prof_Ablts_Aplcnt = a.EV_Prof_Ablts_Aplcnt;
                    model.EV_Dsplnry_Perf_Iss_Rptd = a.EV_Dsplnry_Perf_Iss_Rptd;
                    model.EV_DisciplinaryIssue = a.EV_DisciplinaryIssue;
                    model.EV_Cmplnts_Rslt_Dsplnry_Sub = a.EV_Cmplnts_Rslt_Dsplnry_Sub;
                    model.EV_Emp_Invol_Fraud_Actvty = a.EV_Emp_Invol_Fraud_Actvty;
                    model.EV_PerformanceIssues = a.EV_PerformanceIssues;
                    model.EV_TeamConduct = a.EV_TeamConduct;
                    model.EV_TeamPlayerNot = a.EV_TeamPlayerNot;
                    model.EV_TechnologicalKnowledge = a.EV_TechnologicalKnowledge;
                    model.EV_Fback_Accnt_Cduct_Jb = a.EV_Fback_Accnt_Cduct_Jb;
                    model.EV_DisciplinedHowHe = a.EV_DisciplinedHowHe;
                    model.EV_LongKnown = a.EV_LongKnown;
                    model.EV_LongWork = a.EV_LongWork;
                    model.EV_LeavesTook = a.EV_LeavesTook;
                    model.EV_People_Rptd = a.EV_People_Rptd;
                    model.EV_Conduct = a.EV_Conduct;
                    model.EV_Performance = a.EV_Performance;
                    model.EV_Perf_Rate = a.EV_Perf_Rate;
                    model.EV_Mtng_Deadline = a.EV_Mtng_Deadline;
                    model.EV_Comm_Skills = a.EV_Comm_Skills;
                    model.EV_Wrtn_Comm_Skills = a.EV_Wrtn_Comm_Skills;
                    model.EV_Verb_Comm_Skills = a.EV_Verb_Comm_Skills;
                    model.EV_Procs_Knowlg = a.EV_Procs_Knowlg;
                    model.EV_Attdnce_Punct = a.EV_Attdnce_Punct;
                    model.EV_Ldrshp_Qualty = a.EV_Ldrshp_Qualty;
                    model.EV_Mngng_Mtng_Int_Cust_Expect = a.EV_Mngng_Mtng_Int_Cust_Expect;
                    model.EV_OtherIssues = a.EV_OtherIssues;
                    model.EV_AdherenceTAT = a.EV_AdherenceTAT;
                    model.EV_CompletesTATWork = a.EV_CompletesTATWork;
                    model.EV_Atude_Hon_Per_Aplcnt = a.EV_Atude_Hon_Per_Aplcnt;
                    model.EV_WorkAttitude = a.EV_WorkAttitude;
                    model.EV_RatingTeaming = a.EV_RatingTeaming;
                    model.EV_Rate_Tech = a.EV_Rate_Tech;
                    model.EV_AttendancePunctuality = a.EV_AttendancePunctuality;
                    model.EV_Cmnts_Abt_Sub = a.EV_Cmnts_Abt_Sub;
                    model.EV_SubjectComments = a.EV_SubjectComments;
                    model.EV_AdditionalComments = a.EV_AdditionalComments;
                    model.EV_OtherComments = a.EV_OtherComments;
                    model.EV_OtherRemarks = a.EV_OtherRemarks;
                    model.EV_AttachedDocumentGenuine = a.EV_AttachedDocumentGenuine;
                    model.EV_ResponseMode = a.EV_ResponseMode;
                    model.EV_OnlineVerificationCondctedOn = a.EV_OnlineVerificationCondctedOn;
                    model.EV_RefereeResponse = a.EV_RefereeResponse;
                    model.EV_RespondentName = a.EV_RespondentName;
                    model.EV_RefereeDetails = a.EV_RefereeDetails;
                    model.EV_RespondentDesignation = a.EV_RespondentDesignation;
                    model.EV_RefereeNameDetails = a.EV_RefereeNameDetails;
                    model.EV_DetailsReferee = a.EV_DetailsReferee;
                    model.EV_ContactDetail = a.EV_ContactDetail;
                    model.EV_DateTime = a.EV_DateTime;
                    model.EV_Verfson_Poss_Phy_Visit = a.EV_Verfson_Poss_Phy_Visit;
                    model.EV_Details_Verfson_EidPh_Phyvst = a.EV_Details_Verfson_EidPh_Phyvst;
                    model.EV_Nme_Add_Cmpny_Exist_Now = a.EV_Nme_Add_Cmpny_Exist_Now;
                    model.EV_Name_Dsg_Verfyng_Auth = a.EV_Name_Dsg_Verfyng_Auth;
                    model.EV_Verified = a.EV_Verified;
                    model.EV_Verified_by = a.EV_Verified_by;
                    model.EV_Verified_by_dsg_ConNo = a.EV_Verified_by_dsg_ConNo;
                    model.EV_Date_Time_Contacted = a.EV_Date_Time_Contacted;
                    model.EV_OtherDetails6 = a.EV_OtherDetails6;
                    model.EV_OtherDetails7 = a.EV_OtherDetails7;
                    model.EV_OtherDetails8 = a.EV_OtherDetails8;
                    model.EV_OtherDetails9 = a.EV_OtherDetails9;
                    model.EV_OtherDetails10 = a.EV_OtherDetails10;
                    model.EV_OtherDetails11 = a.EV_OtherDetails11;
                    model.EV_OtherDetails12 = a.EV_OtherDetails12;
                    model.EV_OtherDetails13 = a.EV_OtherDetails13;
                    model.EV_OtherDetails14 = a.EV_OtherDetails14;
                    model.EV_OtherDetails15 = a.EV_OtherDetails15;
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
                    model.TypeRevert = a.TypeRevert;
                    model.CheckStatus = a.PQEmployment.CheckStatus;
                    model.Severity = a.Severity;
                    model.ColorName = a.ColorName;
                    model.ReportComments = a.ReportComments;
                    model.Status = a.Status;
                    model.VerifiedBy = a.VerifiedBy;
                    model.VerifierDesignation = a.VerifierDesignation;
                    model.VerifierContactNo = a.VerifierContactNo;
                    model.VerifierMobileNo = a.VerifierMobileNo;
                    model.VerifierEmailId = a.VerifierEmailId;
                    model.VerifiedDate = a.VerifiedDate;


                    #endregion
                }
                else
                {
                    #region  ***** PQEmployments *****

                    var entity = db.PQEmployments.Where(a => a.EmploymentRowId == EmploymentRowId).FirstOrDefault();
                    if (entity != null)
                    {
                        int CompanyId = Convert.ToInt32(entity.EV_OrganisationName);
                        string OrganisationName = string.Empty;
                        if (CompanyId > 0)
                        {
                            OrganisationName = db.MasterEmployers.Find(CompanyId).CompanyName;
                        }
                        model.EmploymentRowID = entity.EmploymentRowId;
                        model.ClientRowID = entity.ClientRowID;
                        model.PersonalRowID = entity.PersonalRowID;
                        model.CheckFamilyRowID = entity.CheckFamilyRowID;
                        model.SubCheckRowID = entity.SubCheckRowID;
                        model.EmploymentVerRowID = 0;
                        model.EV_CandidateName = entity.EV_CandidateName;
                        model.EV_OrganisationName = entity.EV_OrganisationName;
                        model.EV_OrganisationNameId = OrganisationName;
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
                        model.EV_OtherDetails6 = entity.EV_OtherDetails6;
                        model.EV_OtherDetails7 = entity.EV_OtherDetails7;
                        model.EV_OtherDetails8 = entity.EV_OtherDetails8;
                        model.EV_OtherDetails9 = entity.EV_OtherDetails9;
                        model.EV_OtherDetails10 = entity.EV_OtherDetails10;
                        model.ATA_CID_No = entity.ATA_CID_No;
                        model.ATA_Cmpny_Addr = entity.ATA_Cmpny_Addr;
                        model.CheckStatus = entity.CheckStatus;
                    }

                    #endregion
                }

                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public UpdatePQEmploymentVerViewModel GetEmploymentInfoForTMQC(int EmploymentRowId = 0, int CheckstatusId = 0)
        {
            try
            {
                ClientCustomize cc = new ClientCustomize();
                DataTable dt = new DataTable();
                UpdatePQEmploymentVerViewModel model = new UpdatePQEmploymentVerViewModel();
                var EmploymentVer = db.PQEmploymentVers.Where(a => a.EmploymentRowID == EmploymentRowId).FirstOrDefault();
                int EmploymentVerRowId = 0;
                if (EmploymentVer != null && EmploymentVer.EmploymentVerRowID > 0)
                {
                    EmploymentVerRowId = EmploymentVer.EmploymentVerRowID;
                }

                if (EmploymentVerRowId > 0)
                {
                    #region ***** PQEmploymentVers *****

                    var a = db.PQEmploymentVers.Where(e => e.EmploymentVerRowID == EmploymentVerRowId).FirstOrDefault();

                    int CompanyId = Convert.ToInt32(a.EV_OrganisationName);
                    string OrganisationName = string.Empty;
                    if (CompanyId > 0)
                    {
                        OrganisationName = db.MasterEmployers.Find(CompanyId).CompanyName;
                    }
                    model.EmploymentRowID = a.EmploymentRowID;
                    model.EmploymentVerRowID = a.EmploymentVerRowID;
                    model.ClientRowID = a.PQEmployment.ClientRowID;
                    model.PersonalRowID = a.PQEmployment.PersonalRowID;
                    model.CheckFamilyRowID = a.PQEmployment.CheckFamilyRowID;
                    model.SubCheckRowID = a.PQEmployment.SubCheckRowID;
                    // model.EV_CandidateName = a.EV_CandidateName;


                    model.EV_CandidateName = db.PQPersonals.Where(T => T.PersonalRowID == a.PQEmployment.PersonalRowID).Select(x => x.Name + " " + x.MiddleName + x.LastName).FirstOrDefault().ToString();

                    model.EV_OrganisationName = a.EV_OrganisationName;
                    model.EV_OrganisationNameId = OrganisationName;
                    model.EV_CmpLocation = a.EV_CmpLocation;
                    dt = cc.GetComapnyAddress(Convert.ToInt32(model.EV_OrganisationName));
                    if (dt.Rows.Count > 0)
                    {
                        model.EV_CmpNameAddress = dt.Rows[0]["CompanyAddress"].ToString();
                    }
                    //model.EV_CmpNameAddress = a.EV_CmpNameAddress;
                    model.EV_EmployeeName = a.EV_EmployeeName;
                    model.EV_EmployeeCode = a.EV_EmployeeCode;
                    model.EV_EmploymentPeriod = a.EV_EmploymentPeriod;
                    model.EV_EmploymentDual = a.EV_EmploymentDual;
                    model.EV_EmployedSelf = a.EV_EmployedSelf;
                    model.EV_lappingOver = a.EV_lappingOver;
                    model.EV_CandidateTenureWorked = a.EV_CandidateTenureWorked;
                    model.EV_EmployeeTenure = a.EV_EmployeeTenure;
                    model.EV_Designation = a.EV_Designation;
                    model.EV_EmployedEarlierAlso = a.EV_EmployedEarlierAlso;
                    model.EV_nature_emplo = a.EV_nature_emplo;
                    model.EV_typ_employ = a.EV_typ_employ;
                    model.EV_EmpType = a.EV_EmpType;
                    model.EV_Department = a.EV_Department;
                    model.EV_CmpCost = a.EV_CmpCost;
                    model.EV_last_sal = a.EV_last_sal;
                    model.EV_leavingReason = a.EV_leavingReason;
                    model.EV_termina = a.EV_termina;
                    model.EV_DiscontinueServiceReason = a.EV_DiscontinueServiceReason;
                    model.EV_EligibilityforRehire = a.EV_EligibilityforRehire;
                    model.EV_EligibilityOfRehire = a.EV_EligibilityOfRehire;
                    model.EV_EligibileForRehire = a.EV_EligibileForRehire;
                    model.EV_Rhre_Aplcnt_Situt_Ars_Ftre = a.EV_Rhre_Aplcnt_Situt_Ars_Ftre;
                    model.EV_Reported = a.EV_Reported;
                    model.EV_ReportingManagerName = a.EV_ReportingManagerName;
                    model.EV_Rptng_Mgr_Dsg_Eid_Tno = a.EV_Rptng_Mgr_Dsg_Eid_Tno;
                    model.EV_Rptng_Mgr_Name_Ph = a.EV_Rptng_Mgr_Name_Ph;
                    model.EV_Rptng_Mgr_Name_ConDet = a.EV_Rptng_Mgr_Name_ConDet;
                    model.EV_Sub_Rptng_Mgr_Name_Dsg_EidPh = a.EV_Sub_Rptng_Mgr_Name_Dsg_EidPh;
                    model.EV_Supervisor = a.EV_Supervisor;
                    model.EV_Sup_Name_Dsg = a.EV_Sup_Name_Dsg;
                    model.EV_FormalitiesExit = a.EV_FormalitiesExit;
                    model.EV_FullFinalFormalities = a.EV_FullFinalFormalities;
                    model.EV_ExitFormalitiesPending = a.EV_ExitFormalitiesPending;
                    model.EV_HandledResponsibilities = a.EV_HandledResponsibilities;
                    model.EV_ResponsibilitiesHandled = a.EV_ResponsibilitiesHandled;
                    model.EV_OverallJobPerformance = a.EV_OverallJobPerformance;
                    model.EV_PerfromanceLevel = a.EV_PerfromanceLevel;
                    model.EV_KnowledgeProcess = a.EV_KnowledgeProcess;
                    model.EV_ValidationBusinessOperation = a.EV_ValidationBusinessOperation;
                    model.EV_DutiesNature = a.EV_DutiesNature;
                    model.EV_Prof_Ablts_Aplcnt = a.EV_Prof_Ablts_Aplcnt;
                    model.EV_Dsplnry_Perf_Iss_Rptd = a.EV_Dsplnry_Perf_Iss_Rptd;
                    model.EV_DisciplinaryIssue = a.EV_DisciplinaryIssue;
                    model.EV_Cmplnts_Rslt_Dsplnry_Sub = a.EV_Cmplnts_Rslt_Dsplnry_Sub;
                    model.EV_Emp_Invol_Fraud_Actvty = a.EV_Emp_Invol_Fraud_Actvty;
                    model.EV_PerformanceIssues = a.EV_PerformanceIssues;
                    model.EV_TeamConduct = a.EV_TeamConduct;
                    model.EV_TeamPlayerNot = a.EV_TeamPlayerNot;
                    model.EV_TechnologicalKnowledge = a.EV_TechnologicalKnowledge;
                    model.EV_Fback_Accnt_Cduct_Jb = a.EV_Fback_Accnt_Cduct_Jb;
                    model.EV_DisciplinedHowHe = a.EV_DisciplinedHowHe;
                    model.EV_LongKnown = a.EV_LongKnown;
                    model.EV_LongWork = a.EV_LongWork;
                    model.EV_LeavesTook = a.EV_LeavesTook;
                    model.EV_People_Rptd = a.EV_People_Rptd;
                    model.EV_Conduct = a.EV_Conduct;
                    model.EV_Performance = a.EV_Performance;
                    model.EV_Perf_Rate = a.EV_Perf_Rate;
                    model.EV_Mtng_Deadline = a.EV_Mtng_Deadline;
                    model.EV_Comm_Skills = a.EV_Comm_Skills;
                    model.EV_Wrtn_Comm_Skills = a.EV_Wrtn_Comm_Skills;
                    model.EV_Verb_Comm_Skills = a.EV_Verb_Comm_Skills;
                    model.EV_Procs_Knowlg = a.EV_Procs_Knowlg;
                    model.EV_Attdnce_Punct = a.EV_Attdnce_Punct;
                    model.EV_Ldrshp_Qualty = a.EV_Ldrshp_Qualty;
                    model.EV_Mngng_Mtng_Int_Cust_Expect = a.EV_Mngng_Mtng_Int_Cust_Expect;
                    model.EV_OtherIssues = a.EV_OtherIssues;
                    model.EV_AdherenceTAT = a.EV_AdherenceTAT;
                    model.EV_CompletesTATWork = a.EV_CompletesTATWork;
                    model.EV_Atude_Hon_Per_Aplcnt = a.EV_Atude_Hon_Per_Aplcnt;
                    model.EV_WorkAttitude = a.EV_WorkAttitude;
                    model.EV_RatingTeaming = a.EV_RatingTeaming;
                    model.EV_Rate_Tech = a.EV_Rate_Tech;
                    model.EV_AttendancePunctuality = a.EV_AttendancePunctuality;
                    model.EV_Cmnts_Abt_Sub = a.EV_Cmnts_Abt_Sub;
                    model.EV_SubjectComments = a.EV_SubjectComments;
                    model.EV_AdditionalComments = a.EV_AdditionalComments;
                    model.EV_OtherComments = a.EV_OtherComments;
                    model.EV_OtherRemarks = a.EV_OtherRemarks;
                    model.EV_AttachedDocumentGenuine = a.EV_AttachedDocumentGenuine;
                    model.EV_ResponseMode = a.EV_ResponseMode;
                    model.EV_OnlineVerificationCondctedOn = a.EV_OnlineVerificationCondctedOn;
                    model.EV_RefereeResponse = a.EV_RefereeResponse;
                    model.EV_RespondentName = a.EV_RespondentName;
                    model.EV_RefereeDetails = a.EV_RefereeDetails;
                    model.EV_RespondentDesignation = a.EV_RespondentDesignation;
                    model.EV_RefereeNameDetails = a.EV_RefereeNameDetails;
                    model.EV_DetailsReferee = a.EV_DetailsReferee;
                    model.EV_ContactDetail = a.EV_ContactDetail;
                    model.EV_DateTime = a.EV_DateTime;
                    model.EV_Verfson_Poss_Phy_Visit = a.EV_Verfson_Poss_Phy_Visit;
                    model.EV_Details_Verfson_EidPh_Phyvst = a.EV_Details_Verfson_EidPh_Phyvst;
                    model.EV_Nme_Add_Cmpny_Exist_Now = a.EV_Nme_Add_Cmpny_Exist_Now;
                    model.EV_Name_Dsg_Verfyng_Auth = a.EV_Name_Dsg_Verfyng_Auth;
                    model.EV_Verified = a.EV_Verified;
                    model.EV_Verified_by = a.EV_Verified_by;
                    model.EV_Verified_by_dsg_ConNo = a.EV_Verified_by_dsg_ConNo;
                    model.EV_Date_Time_Contacted = a.EV_Date_Time_Contacted;
                    model.EV_OtherDetails6 = a.EV_OtherDetails6;
                    model.EV_OtherDetails7 = a.EV_OtherDetails7;
                    model.EV_OtherDetails8 = a.EV_OtherDetails8;
                    model.EV_OtherDetails9 = a.EV_OtherDetails9;
                    model.EV_OtherDetails10 = a.EV_OtherDetails10;
                    model.EV_OtherDetails11 = a.EV_OtherDetails11;
                    model.EV_OtherDetails12 = a.EV_OtherDetails12;
                    model.EV_OtherDetails13 = a.EV_OtherDetails13;
                    model.EV_OtherDetails14 = a.EV_OtherDetails14;
                    model.EV_OtherDetails15 = a.EV_OtherDetails15;
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
                    model.CheckStatus = a.PQEmployment.CheckStatus;
                    model.CheckStatusId = CheckstatusId;

                    #endregion
                }
                else
                {
                    #region  ***** PQEmployments *****

                    var entity = db.PQEmployments.Where(a => a.EmploymentRowId == EmploymentRowId).FirstOrDefault();
                    if (entity != null)
                    {
                        int CompanyId = Convert.ToInt32(entity.EV_OrganisationName);
                        string OrganisationName = string.Empty;
                        if (CompanyId > 0)
                        {
                            OrganisationName = db.MasterEmployers.Find(CompanyId).CompanyName;
                        }
                        model.EmploymentRowID = entity.EmploymentRowId;
                        model.ClientRowID = entity.ClientRowID;
                        model.PersonalRowID = entity.PersonalRowID;
                        model.CheckFamilyRowID = entity.CheckFamilyRowID;
                        model.SubCheckRowID = entity.SubCheckRowID;
                        model.EmploymentVerRowID = 0;
                        //model.EV_CandidateName = entity.EV_CandidateName;


                        model.EV_CandidateName = db.PQPersonals.Where(T => T.PersonalRowID == entity.PersonalRowID).Select(x => x.Name + " " + x.MiddleName + x.LastName).FirstOrDefault().ToString();

                        model.EV_OrganisationName = entity.EV_OrganisationName;
                        model.EV_OrganisationNameId = OrganisationName;
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
                        model.EV_OtherDetails6 = entity.EV_OtherDetails6;
                        model.EV_OtherDetails7 = entity.EV_OtherDetails7;
                        model.EV_OtherDetails8 = entity.EV_OtherDetails8;
                        model.EV_OtherDetails9 = entity.EV_OtherDetails9;
                        model.EV_OtherDetails10 = entity.EV_OtherDetails10;
                        model.ATA_CID_No = entity.ATA_CID_No;
                        model.ATA_Cmpny_Addr = entity.ATA_Cmpny_Addr;
                        model.CheckStatus = entity.CheckStatus;
                        model.CheckStatusId = CheckstatusId;
                    }

                    #endregion
                }
                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PQPersonalDetailsVerViewModel GetPersonalDetailsForTM(int EmploymentRowId = 0)
        {
            try
            {
                PQPersonalDetailsVerViewModel model = new PQPersonalDetailsVerViewModel();
                model = db.PQEmployments.Where(a => a.EmploymentRowId == EmploymentRowId).Select(a => new PQPersonalDetailsVerViewModel
                {
                    ClientRowID = a.ClientRowID,
                    PersonalRowID = a.PersonalRowID,
                    UniqueComponentID = a.UniqueComponentID,
                    //CandidateName = a.EV_CandidateName,


                    CandidateName = db.PQPersonals.Where(T => T.PersonalRowID == a.PersonalRowID).Select(x => x.Name + " " + x.MiddleName + x.LastName).FirstOrDefault().ToString(),

                    ClientName = a.PQClientMaster.MasterAbbreviation.ClientName,
                    CompanyReferenceNo = a.EV_SecuritasReferenceID,
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

        public void UpdateEmploymentByTeamMember(PQEmploymentVerViewModel model)
        {
            try
            {
                if (model != null)
                {
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_OrganisationName = model.EV_OrganisationName;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_CmpLocation = model.EV_CmpLocation;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_CmpNameAddress = model.EV_CmpNameAddress;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_EmployeeName = model.EV_EmployeeName;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_EmployeeCode = model.EV_EmployeeCode;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_EmploymentPeriod = model.EV_EmploymentPeriod;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_EmploymentDual = model.EV_EmploymentDual;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_EmployedSelf = model.EV_EmployedSelf;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_lappingOver = model.EV_lappingOver;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_CandidateTenureWorked = model.EV_CandidateTenureWorked;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_EmployeeTenure = model.EV_EmployeeTenure;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Designation = model.EV_Designation;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_EmployedEarlierAlso = model.EV_EmployedEarlierAlso;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_nature_emplo = model.EV_nature_emplo;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_typ_employ = model.EV_typ_employ;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_EmpType = model.EV_EmpType;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Department = model.EV_Department;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_CmpCost = model.EV_CmpCost;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_last_sal = model.EV_last_sal;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_leavingReason = model.EV_leavingReason;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_termina = model.EV_termina;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_DiscontinueServiceReason = model.EV_DiscontinueServiceReason;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_EligibilityforRehire = model.EV_EligibilityforRehire;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_EligibilityOfRehire = model.EV_EligibilityOfRehire;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_EligibileForRehire = model.EV_EligibileForRehire;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Rhre_Aplcnt_Situt_Ars_Ftre = model.EV_Rhre_Aplcnt_Situt_Ars_Ftre;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Reported = model.EV_Reported;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_ReportingManagerName = model.EV_ReportingManagerName;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Rptng_Mgr_Dsg_Eid_Tno = model.EV_Rptng_Mgr_Dsg_Eid_Tno;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Rptng_Mgr_Name_Ph = model.EV_Rptng_Mgr_Name_Ph;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Rptng_Mgr_Name_ConDet = model.EV_Rptng_Mgr_Name_ConDet;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Sub_Rptng_Mgr_Name_Dsg_EidPh = model.EV_Sub_Rptng_Mgr_Name_Dsg_EidPh;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Supervisor = model.EV_Supervisor;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Sup_Name_Dsg = model.EV_Sup_Name_Dsg;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_FormalitiesExit = model.EV_FormalitiesExit;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_FullFinalFormalities = model.EV_FullFinalFormalities;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_ExitFormalitiesPending = model.EV_ExitFormalitiesPending;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_HandledResponsibilities = model.EV_HandledResponsibilities;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_ResponsibilitiesHandled = model.EV_ResponsibilitiesHandled;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_OverallJobPerformance = model.EV_OverallJobPerformance;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_PerfromanceLevel = model.EV_PerfromanceLevel;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_KnowledgeProcess = model.EV_KnowledgeProcess;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_ValidationBusinessOperation = model.EV_ValidationBusinessOperation;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_DutiesNature = model.EV_DutiesNature;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Prof_Ablts_Aplcnt = model.EV_Prof_Ablts_Aplcnt;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Dsplnry_Perf_Iss_Rptd = model.EV_Dsplnry_Perf_Iss_Rptd;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_DisciplinaryIssue = model.EV_DisciplinaryIssue;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Cmplnts_Rslt_Dsplnry_Sub = model.EV_Cmplnts_Rslt_Dsplnry_Sub;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Emp_Invol_Fraud_Actvty = model.EV_Emp_Invol_Fraud_Actvty;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_PerformanceIssues = model.EV_PerformanceIssues;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_TeamConduct = model.EV_TeamConduct;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_TeamPlayerNot = model.EV_TeamPlayerNot;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_TechnologicalKnowledge = model.EV_TechnologicalKnowledge;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Fback_Accnt_Cduct_Jb = model.EV_Fback_Accnt_Cduct_Jb;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_DisciplinedHowHe = model.EV_DisciplinedHowHe;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_LongKnown = model.EV_LongKnown;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_LongWork = model.EV_LongWork;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_LeavesTook = model.EV_LeavesTook;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_People_Rptd = model.EV_People_Rptd;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Conduct = model.EV_Conduct;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Performance = model.EV_Performance;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Perf_Rate = model.EV_Perf_Rate;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Mtng_Deadline = model.EV_Mtng_Deadline;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Comm_Skills = model.EV_Comm_Skills;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Wrtn_Comm_Skills = model.EV_Wrtn_Comm_Skills;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Verb_Comm_Skills = model.EV_Verb_Comm_Skills;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Procs_Knowlg = model.EV_Procs_Knowlg;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Attdnce_Punct = model.EV_Attdnce_Punct;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Ldrshp_Qualty = model.EV_Ldrshp_Qualty;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Mngng_Mtng_Int_Cust_Expect = model.EV_Mngng_Mtng_Int_Cust_Expect;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_OtherIssues = model.EV_OtherIssues;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_AdherenceTAT = model.EV_AdherenceTAT;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_CompletesTATWork = model.EV_CompletesTATWork;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Atude_Hon_Per_Aplcnt = model.EV_Atude_Hon_Per_Aplcnt;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_WorkAttitude = model.EV_WorkAttitude;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_RatingTeaming = model.EV_RatingTeaming;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Rate_Tech = model.EV_Rate_Tech;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_AttendancePunctuality = model.EV_AttendancePunctuality;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Cmnts_Abt_Sub = model.EV_Cmnts_Abt_Sub;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_SubjectComments = model.EV_SubjectComments;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_AdditionalComments = model.EV_AdditionalComments;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_OtherComments = model.EV_OtherComments;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_OtherRemarks = model.EV_OtherRemarks;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_AttachedDocumentGenuine = model.EV_AttachedDocumentGenuine;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_ResponseMode = model.EV_ResponseMode;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_OnlineVerificationCondctedOn = model.EV_OnlineVerificationCondctedOn;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_RefereeResponse = model.EV_RefereeResponse;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_RespondentName = model.EV_RespondentName;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_RefereeDetails = model.EV_RefereeDetails;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_RespondentDesignation = model.EV_RespondentDesignation;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_RefereeNameDetails = model.EV_RefereeNameDetails;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_DetailsReferee = model.EV_DetailsReferee;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_ContactDetail = model.EV_ContactDetail;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_DateTime = model.EV_DateTime;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Verfson_Poss_Phy_Visit = model.EV_Verfson_Poss_Phy_Visit;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Details_Verfson_EidPh_Phyvst = model.EV_Details_Verfson_EidPh_Phyvst;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Nme_Add_Cmpny_Exist_Now = model.EV_Nme_Add_Cmpny_Exist_Now;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Name_Dsg_Verfyng_Auth = model.EV_Name_Dsg_Verfyng_Auth;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Verified = model.EV_Verified;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Verified_by = model.EV_Verified_by;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Verified_by_dsg_ConNo = model.EV_Verified_by_dsg_ConNo;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Date_Time_Contacted = model.EV_Date_Time_Contacted;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_OtherDetails6 = model.EV_OtherDetails6;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_OtherDetails7 = model.EV_OtherDetails7;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_OtherDetails8 = model.EV_OtherDetails8;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_OtherDetails9 = model.EV_OtherDetails9;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_OtherDetails10 = model.EV_OtherDetails10;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_OtherDetails11 = model.EV_OtherDetails11;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_OtherDetails12 = model.EV_OtherDetails12;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_OtherDetails13 = model.EV_OtherDetails13;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_OtherDetails14 = model.EV_OtherDetails14;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_OtherDetails15 = model.EV_OtherDetails15;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).ATA_Reverse_Directory = model.ATA_Reverse_Directory;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).ATA_Residcl_Commrcl = model.ATA_Residcl_Commrcl;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).ATA_Stock_Exchnge = model.ATA_Stock_Exchnge;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).ATA_Telphne_Directry_Srch = model.ATA_Telphne_Directry_Srch;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).ATA_Yellow_Pages = model.ATA_Yellow_Pages;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).ATA_Who_Domain = model.ATA_Who_Domain;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).ATA_GoogleSearch = model.ATA_GoogleSearch;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).ATA_InfrastructureOfCmp = model.ATA_InfrastructureOfCmp;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).ATA_Just_Dial = model.ATA_Just_Dial;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).ATA_Neighbor_Check1 = model.ATA_Neighbor_Check1;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).ATA_NASCOM_Empanelment = model.ATA_NASCOM_Empanelment;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).ATA_Photograph = model.ATA_Photograph;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).ATA_Physical_Site = model.ATA_Physical_Site;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).ATA_Reg_Company = model.ATA_Reg_Company;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).ATA_PO_Courier_Check = model.ATA_PO_Courier_Check;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).ATA_Network_Sol = model.ATA_Network_Sol;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).ATA_ApprxEmpWorking = model.ATA_ApprxEmpWorking;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).ATA_CID_No = model.ATA_CID_No;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).ATA_Cmpny_Addr = model.ATA_Cmpny_Addr;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).ATA_Decoy_Call = model.ATA_Decoy_Call;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).ATA_Cmpny_Website = model.ATA_Cmpny_Website;
                    db.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).VerifiedDate = model.VerifiedDate;
                }
                else
                {
                    throw new Exception("Employment could not be blank!");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void UpdateEmploymentByTMQC(UpdatePQEmploymentVerViewModel model, short RptQCManagerId = 0)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            int statusId = context.PQEmploymentStatus.Where(p => p.EmploymentRowId == model.EmploymentRowID).FirstOrDefault().EmploymentStatusRowID;

                            if (model.CheckStatus == "Rejected")
                            {
                                #region Update Check Status

                                string ActualCheckStatus = "WIP-Pending";
                                context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).CheckStatus = ActualCheckStatus;
                                context.SaveChanges();

                                context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == statusId).TMCheckStatus = ActualCheckStatus;
                                context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == statusId).TMQCCheckStatus = "";
                                context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == statusId).Remarks = model.CloserViewModel.ActionHistory;
                                context.SaveChanges();

                                #endregion
                            }
                            else
                            {
                                context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).CheckStatus = model.CheckStatus;
                                context.SaveChanges();

                                if (model.CheckStatus == "Insufficent")
                                {
                                    context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == statusId).TMQCCheckStatus = "Insufficent-QC";
                                    context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == statusId).InfSuffClearBy = 0;
                                    context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == statusId).InfSuffClearComments = "";
                                    context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == statusId).InfSuffClearDate = null;
                                    context.SaveChanges();

                                    context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).CheckStatus = "Insufficent-QC";
                                    context.SaveChanges();

                                    context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).CaseStatus = model.CheckStatus;
                                    context.SaveChanges();
                                }

                                if (model.CheckStatus == "Funds Pending")
                                {
                                    context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == statusId).TMQCCheckStatus = "Funds Pending-QC";
                                    context.SaveChanges();

                                    context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).CheckStatus = "Funds Pending-QC";
                                    context.SaveChanges();

                                    context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).CaseStatus = model.CheckStatus;
                                    context.SaveChanges();
                                }

                                if (model.EmploymentVerRowID > 0 && model.CheckStatus != "Insufficent")
                                {
                                    #region Update Employment Ver Info By TMQC

                                    //context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_CandidateName = model.EV_CandidateName;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_OrganisationName = model.EV_OrganisationName;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_CmpLocation = model.EV_CmpLocation;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_CmpNameAddress = model.EV_CmpNameAddress;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_EmployeeName = model.EV_EmployeeName;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_EmployeeCode = model.EV_EmployeeCode;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_EmploymentPeriod = model.EV_EmploymentPeriod;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_EmploymentDual = model.EV_EmploymentDual;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_EmployedSelf = model.EV_EmployedSelf;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_lappingOver = model.EV_lappingOver;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_CandidateTenureWorked = model.EV_CandidateTenureWorked;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_EmployeeTenure = model.EV_EmployeeTenure;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Designation = model.EV_Designation;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_EmployedEarlierAlso = model.EV_EmployedEarlierAlso;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_nature_emplo = model.EV_nature_emplo;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_typ_employ = model.EV_typ_employ;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_EmpType = model.EV_EmpType;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Department = model.EV_Department;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_CmpCost = model.EV_CmpCost;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_last_sal = model.EV_last_sal;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_leavingReason = model.EV_leavingReason;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_termina = model.EV_termina;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_DiscontinueServiceReason = model.EV_DiscontinueServiceReason;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_EligibilityforRehire = model.EV_EligibilityforRehire;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_EligibilityOfRehire = model.EV_EligibilityOfRehire;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_EligibileForRehire = model.EV_EligibileForRehire;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Rhre_Aplcnt_Situt_Ars_Ftre = model.EV_Rhre_Aplcnt_Situt_Ars_Ftre;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Reported = model.EV_Reported;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_ReportingManagerName = model.EV_ReportingManagerName;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Rptng_Mgr_Dsg_Eid_Tno = model.EV_Rptng_Mgr_Dsg_Eid_Tno;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Rptng_Mgr_Name_Ph = model.EV_Rptng_Mgr_Name_Ph;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Rptng_Mgr_Name_ConDet = model.EV_Rptng_Mgr_Name_ConDet;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Sub_Rptng_Mgr_Name_Dsg_EidPh = model.EV_Sub_Rptng_Mgr_Name_Dsg_EidPh;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Supervisor = model.EV_Supervisor;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Sup_Name_Dsg = model.EV_Sup_Name_Dsg;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_FormalitiesExit = model.EV_FormalitiesExit;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_FullFinalFormalities = model.EV_FullFinalFormalities;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_ExitFormalitiesPending = model.EV_ExitFormalitiesPending;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_HandledResponsibilities = model.EV_HandledResponsibilities;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_ResponsibilitiesHandled = model.EV_ResponsibilitiesHandled;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_OverallJobPerformance = model.EV_OverallJobPerformance;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_PerfromanceLevel = model.EV_PerfromanceLevel;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_KnowledgeProcess = model.EV_KnowledgeProcess;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_ValidationBusinessOperation = model.EV_ValidationBusinessOperation;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_DutiesNature = model.EV_DutiesNature;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Prof_Ablts_Aplcnt = model.EV_Prof_Ablts_Aplcnt;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Dsplnry_Perf_Iss_Rptd = model.EV_Dsplnry_Perf_Iss_Rptd;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_DisciplinaryIssue = model.EV_DisciplinaryIssue;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Cmplnts_Rslt_Dsplnry_Sub = model.EV_Cmplnts_Rslt_Dsplnry_Sub;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Emp_Invol_Fraud_Actvty = model.EV_Emp_Invol_Fraud_Actvty;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_PerformanceIssues = model.EV_PerformanceIssues;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_TeamConduct = model.EV_TeamConduct;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_TeamPlayerNot = model.EV_TeamPlayerNot;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_TechnologicalKnowledge = model.EV_TechnologicalKnowledge;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Fback_Accnt_Cduct_Jb = model.EV_Fback_Accnt_Cduct_Jb;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_DisciplinedHowHe = model.EV_DisciplinedHowHe;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_LongKnown = model.EV_LongKnown;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_LongWork = model.EV_LongWork;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_LeavesTook = model.EV_LeavesTook;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_People_Rptd = model.EV_People_Rptd;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Conduct = model.EV_Conduct;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Performance = model.EV_Performance;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Perf_Rate = model.EV_Perf_Rate;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Mtng_Deadline = model.EV_Mtng_Deadline;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Comm_Skills = model.EV_Comm_Skills;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Wrtn_Comm_Skills = model.EV_Wrtn_Comm_Skills;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Verb_Comm_Skills = model.EV_Verb_Comm_Skills;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Procs_Knowlg = model.EV_Procs_Knowlg;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Attdnce_Punct = model.EV_Attdnce_Punct;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Ldrshp_Qualty = model.EV_Ldrshp_Qualty;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Mngng_Mtng_Int_Cust_Expect = model.EV_Mngng_Mtng_Int_Cust_Expect;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_OtherIssues = model.EV_OtherIssues;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_AdherenceTAT = model.EV_AdherenceTAT;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_CompletesTATWork = model.EV_CompletesTATWork;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Atude_Hon_Per_Aplcnt = model.EV_Atude_Hon_Per_Aplcnt;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_WorkAttitude = model.EV_WorkAttitude;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_RatingTeaming = model.EV_RatingTeaming;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Rate_Tech = model.EV_Rate_Tech;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_AttendancePunctuality = model.EV_AttendancePunctuality;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Cmnts_Abt_Sub = model.EV_Cmnts_Abt_Sub;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_SubjectComments = model.EV_SubjectComments;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_AdditionalComments = model.EV_AdditionalComments;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_OtherComments = model.EV_OtherComments;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_OtherRemarks = model.EV_OtherRemarks;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_AttachedDocumentGenuine = model.EV_AttachedDocumentGenuine;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_ResponseMode = model.EV_ResponseMode;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_OnlineVerificationCondctedOn = model.EV_OnlineVerificationCondctedOn;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_RefereeResponse = model.EV_RefereeResponse;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_RespondentName = model.EV_RespondentName;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_RefereeDetails = model.EV_RefereeDetails;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_RespondentDesignation = model.EV_RespondentDesignation;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_RefereeNameDetails = model.EV_RefereeNameDetails;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_DetailsReferee = model.EV_DetailsReferee;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_ContactDetail = model.EV_ContactDetail;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_DateTime = model.EV_DateTime;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Verfson_Poss_Phy_Visit = model.EV_Verfson_Poss_Phy_Visit;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Details_Verfson_EidPh_Phyvst = model.EV_Details_Verfson_EidPh_Phyvst;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Nme_Add_Cmpny_Exist_Now = model.EV_Nme_Add_Cmpny_Exist_Now;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Name_Dsg_Verfyng_Auth = model.EV_Name_Dsg_Verfyng_Auth;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Verified = model.EV_Verified;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Verified_by = model.EV_Verified_by;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Verified_by_dsg_ConNo = model.EV_Verified_by_dsg_ConNo;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_Date_Time_Contacted = model.EV_Date_Time_Contacted;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_OtherDetails6 = model.EV_OtherDetails6;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_OtherDetails7 = model.EV_OtherDetails7;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_OtherDetails8 = model.EV_OtherDetails8;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_OtherDetails9 = model.EV_OtherDetails9;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_OtherDetails10 = model.EV_OtherDetails10;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_OtherDetails11 = model.EV_OtherDetails11;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_OtherDetails12 = model.EV_OtherDetails12;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_OtherDetails13 = model.EV_OtherDetails13;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_OtherDetails14 = model.EV_OtherDetails14;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).EV_OtherDetails15 = model.EV_OtherDetails15;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).ATA_Reverse_Directory = model.ATA_Reverse_Directory;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).ATA_Residcl_Commrcl = model.ATA_Residcl_Commrcl;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).ATA_Stock_Exchnge = model.ATA_Stock_Exchnge;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).ATA_Telphne_Directry_Srch = model.ATA_Telphne_Directry_Srch;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).ATA_Yellow_Pages = model.ATA_Yellow_Pages;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).ATA_Who_Domain = model.ATA_Who_Domain;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).ATA_GoogleSearch = model.ATA_GoogleSearch;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).ATA_InfrastructureOfCmp = model.ATA_InfrastructureOfCmp;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).ATA_Just_Dial = model.ATA_Just_Dial;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).ATA_Neighbor_Check1 = model.ATA_Neighbor_Check1;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).ATA_NASCOM_Empanelment = model.ATA_NASCOM_Empanelment;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).ATA_Photograph = model.ATA_Photograph;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).ATA_Physical_Site = model.ATA_Physical_Site;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).ATA_Reg_Company = model.ATA_Reg_Company;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).ATA_PO_Courier_Check = model.ATA_PO_Courier_Check;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).ATA_Network_Sol = model.ATA_Network_Sol;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).ATA_ApprxEmpWorking = model.ATA_ApprxEmpWorking;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).ATA_CID_No = model.ATA_CID_No;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).ATA_Cmpny_Addr = model.ATA_Cmpny_Addr;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).ATA_Decoy_Call = model.ATA_Decoy_Call;
                                    context.PQEmploymentVers.Single(a => a.EmploymentRowID == model.EmploymentRowID).ATA_Cmpny_Website = model.ATA_Cmpny_Website;

                                    #endregion

                                    context.SaveChanges();

                                    #region Save Disposition/Severity
                                    if (model.CloserViewModel.DispositionID != null)
                                    {
                                        #region Disposition
                                        string[] DispositionIds = model.CloserViewModel.DispositionID.Split(',');
                                        byte[] SeverityRows = new byte[DispositionIds.Length];
                                        List<PQEmploymentVerDisposition> VerDispositionsList = new List<PQEmploymentVerDisposition>();
                                        var FindVerDisposition = context.PQEmploymentVerDispositions.Where(a => a.EmploymentVerRowID == model.EmploymentVerRowID).ToList();

                                        if (FindVerDisposition != null && FindVerDisposition.Count > 0)
                                        {
                                            context.PQEmploymentVerDispositions.RemoveRange(FindVerDisposition);
                                            context.SaveChanges();
                                        }
                                        foreach (var item in DispositionIds)
                                        {
                                            PQEmploymentVerDisposition VerEntity = new PQEmploymentVerDisposition
                                            {
                                                EmploymentVerRowID = model.EmploymentVerRowID,
                                                ClientDispositionRowId = Convert.ToInt32(item)
                                            };
                                            VerDispositionsList.Add(VerEntity);
                                        }
                                        context.PQEmploymentVerDispositions.AddRange(VerDispositionsList);
                                        context.SaveChanges();
                                        #endregion
                                    }

                                    //Update in EmploymentVer Table            
                                    context.PQEmploymentVers.Single(c => c.EmploymentVerRowID == model.EmploymentVerRowID).ReportComments = model.CloserViewModel.Remarks;
                                    context.PQEmploymentVers.Single(c => c.EmploymentVerRowID == model.EmploymentVerRowID).Severity = model.CloserViewModel.Severity;
                                    context.PQEmploymentVers.Single(c => c.EmploymentVerRowID == model.EmploymentVerRowID).ColorName = model.CloserViewModel.ColorName;
                                    context.PQEmploymentVers.Single(c => c.EmploymentVerRowID == model.EmploymentVerRowID).CheckStatus = model.CheckStatus;
                                    context.SaveChanges();
                                    #endregion

                                    #region ***** Completed-TM & Rejected-RWQC*****

                                    if (model.CheckStatus == "Completed-TM" || model.CheckStatus == "Rejected-RWQC")
                                    {
                                        string allocateMgr = "Allocated-Mgr";
                                        if (model.CloserViewModel.ColorName.ToLower() == "green")
                                        {
                                            context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == statusId).TMQCCheckStatus = "Completed-QC";
                                            context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == statusId).RWQCMgrCheckStatus = allocateMgr;
                                            context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == statusId).RWQCManagerBy = RptQCManagerId;
                                            context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == statusId).RWQCManagerDate = DateTime.Now;
                                            context.SaveChanges();

                                            short TemaMember = context.PQEmployments.Find(model.EmploymentRowID).PQPersonal.RWQCManagerTo;
                                            if (TemaMember == 0)
                                            {
                                                context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).RWQCMgrCaseStatus = allocateMgr;
                                                context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).RWQCManagerTo = RptQCManagerId;
                                                context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).RWQCManagerDate = DateTime.Now;
                                                context.SaveChanges();
                                            }

                                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).CheckStatus = "Allocated-RWQC";
                                            context.SaveChanges();

                                        }
                                        else
                                        {
                                            allocateMgr = "RW-Approval-TL";
                                            context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == statusId).TMQCCheckStatus = "Completed-QC";
                                            context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == statusId).TLCheckStatus = allocateMgr;
                                            context.SaveChanges();

                                            context.PQEmployments.Single(c => c.EmploymentRowId == model.EmploymentRowID).CheckStatus = allocateMgr;
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

        public EmploymentVerCloserViewModel GetEmploymentVerCloserDetails(int EmpVerId = 0)
        {
            EmploymentVerCloserViewModel model = new EmploymentVerCloserViewModel();
            var Disposition = db.PQEmploymentVerDispositions.Where(s => s.EmploymentVerRowID == EmpVerId).ToList();
            List<int> selectList = new List<int>();
            foreach (var item in Disposition)
            {
                selectList.Add(Convert.ToInt32(item.ClientDispositionRowId));
            }
            //string type = string.Join(",", selectList.ToArray());           
            var Dispositions = db.PQEmploymentVerDispositions.Where(d => d.EmploymentVerRowID == EmpVerId).Select(a => new EmploymentVerCloserViewModel
            {
                CDispositionRowId = selectList,
                ColorName = a.PQEmploymentVer.ColorName,
                Severity = a.PQEmploymentVer.Severity,
                Remarks = a.PQEmploymentVer.ReportComments,
                CheckStatus = a.PQEmploymentVer.CheckStatus,
            }).FirstOrDefault();

            var entity = db.PQEmploymentVers.Find(EmpVerId);
            if (entity != null)
            {
                if (Dispositions == null)
                {
                    Dispositions = new EmploymentVerCloserViewModel();
                    Dispositions.EmploymentRowID = entity.EmploymentRowID;
                    Dispositions.EmploymentVerRowID = entity.EmploymentVerRowID;
                    Dispositions.ClientRowID = entity.PQEmployment.ClientRowID;
                    Dispositions.CheckFamilyRowID = entity.PQEmployment.CheckFamilyRowID;
                    Dispositions.SubCheckRowID = entity.PQEmployment.SubCheckRowID;
                    Dispositions.PersonalRowID = entity.PQEmployment.PersonalRowID;
                }
                else
                {
                    Dispositions.EmploymentRowID = entity.EmploymentRowID;
                    Dispositions.EmploymentVerRowID = entity.EmploymentVerRowID;
                    Dispositions.ClientRowID = entity.PQEmployment.ClientRowID;
                    Dispositions.CheckFamilyRowID = entity.PQEmployment.CheckFamilyRowID;
                    Dispositions.SubCheckRowID = entity.PQEmployment.SubCheckRowID;
                    Dispositions.PersonalRowID = entity.PQEmployment.PersonalRowID;
                }

            }
            return Dispositions;
        }

        public void AddEmploymentVerCloserDetails(EmploymentVerCloserViewModel model)
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
                            List<PQEmploymentVerDisposition> VerDispositionsList = new List<PQEmploymentVerDisposition>();
                            var FindEmploymentVerDisposition = context.PQEmploymentVerDispositions.Where(a => a.EmploymentVerRowID == model.EmploymentVerRowID).ToList();

                            if (FindEmploymentVerDisposition != null && FindEmploymentVerDisposition.Count > 0)
                            {
                                context.PQEmploymentVerDispositions.RemoveRange(FindEmploymentVerDisposition);
                                context.SaveChanges();
                            }
                            foreach (var item in DispositionIds)
                            {
                                PQEmploymentVerDisposition VerEntity = new PQEmploymentVerDisposition
                                {
                                    EmploymentVerRowID = model.EmploymentVerRowID,
                                    ClientDispositionRowId = Convert.ToInt32(item)
                                };
                                VerDispositionsList.Add(VerEntity);
                            }
                            context.PQEmploymentVerDispositions.AddRange(VerDispositionsList);
                            context.SaveChanges();
                            #endregion
                        }

                        //Update in AddressVer Table            
                        context.PQEmploymentVers.Single(c => c.EmploymentVerRowID == model.EmploymentVerRowID).ReportComments = model.Remarks;
                        context.PQEmploymentVers.Single(c => c.EmploymentVerRowID == model.EmploymentVerRowID).Severity = model.Severity;
                        context.PQEmploymentVers.Single(c => c.EmploymentVerRowID == model.EmploymentVerRowID).ColorName = model.ColorName;
                        context.PQEmploymentVers.Single(c => c.EmploymentVerRowID == model.EmploymentVerRowID).CheckStatus = model.CheckStatus;
                        context.SaveChanges();

                        int statusId = context.PQEmploymentStatus.Where(p => p.EmploymentRowId == model.EmploymentRowID).FirstOrDefault().EmploymentStatusRowID;

                        if (model.CheckStatus == "Review Requested")
                        {
                            context.PQEmployments.Single(p => p.EmploymentRowId == model.EmploymentRowID).CheckStatus = model.CheckStatus;
                            context.SaveChanges();

                            context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == statusId).TMCheckStatus = model.CheckStatus;
                            context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == statusId).Remarks = model.ActionHistory;
                            context.SaveChanges();
                        }
                        if (model.CheckStatus == "Completed")
                        {
                            ClientCustomize cc = new ClientCustomize();
                            DataSet dd = new DataSet();
                            dd = cc.getInformationFromDepartmentRowID(model.EmploymentRowID, "Employment");
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

                            context.PQEmployments.Single(p => p.EmploymentRowId == model.EmploymentRowID).CheckStatus = "Completed-TM";
                            context.SaveChanges();

                            context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == statusId).TMQCCheckStatus = model.CheckStatus;
                            context.PQEmploymentStatus.Single(c => c.EmploymentStatusRowID == statusId).TMCheckStatus = model.CheckStatus;
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

        public string GetCompanyNameForResearch(int EmploymentRowId = 0)
        {
            try
            {
                var model = db.PQEmployments.Where(a => a.EmploymentRowId == EmploymentRowId).FirstOrDefault();

                if (model != null && !string.IsNullOrEmpty(model.EV_OrganisationName) && Convert.ToInt32(model.EV_OrganisationName) > 0)
                {
                    int CompanyId = Convert.ToInt32(model.EV_OrganisationName);
                    return db.MasterEmployers.Find(CompanyId).CompanyName;
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
