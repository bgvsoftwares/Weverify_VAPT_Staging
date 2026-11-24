using DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.VerificationViewModel;

namespace BAL.VerificationRepository
{
    public class PQReferenceVerRepository : IPQReferenceVerRepository
    {
        DataContext db;
        public PQReferenceVerRepository()
        {
            db = new DataContext();
        }

        public void AddReferenceByTeamMember(PQReferenceVerViewModel model)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            PQReferenceVer entity = new PQReferenceVer();
                            entity.ReferenceCheckRowID = model.ReferenceCheckRowID;
                            entity.RV_Cmplnts_Result_Dsplnry_Sub = model.RV_Cmplnts_Result_Dsplnry_Sub;
                            entity.RV_Reliable_Honest_Person = model.RV_Reliable_Honest_Person;
                            entity.RV_Cand_Fmly_Associat_Politics = model.RV_Cand_Fmly_Associat_Politics;
                            entity.RV_Duration = model.RV_Duration;
                            entity.RV_Relation_Sub = model.RV_Relation_Sub;
                            entity.RV_Response_Frm_Rfre = model.RV_Response_Frm_Rfre;
                            entity.RV_CandidateName = model.RV_CandidateName;
                            entity.RV_OrganisationName = model.RV_OrganisationName;
                            entity.RV_RefereeName = model.RV_RefereeName;
                            entity.RV_RefereeDesignation = model.RV_RefereeDesignation;
                            entity.RV_Refre_Prsnt_Emp = model.RV_Refre_Prsnt_Emp;
                            entity.RV_Refre_ConDet = model.RV_Refre_ConDet;
                            entity.RV_Refere_Desig = model.RV_Refere_Desig;
                            entity.RV_Refere_Add = model.RV_Refere_Add;
                            entity.RV_Refre_MobNo = model.RV_Refre_MobNo;
                            entity.RV_Refre_Eid = model.RV_Refre_Eid;
                            entity.RV_Prof_Relationship_Cand_Dur = model.RV_Prof_Relationship_Cand_Dur;
                            entity.RV_Know_Long = model.RV_Know_Long;
                            entity.RV_Tenure_Emp = model.RV_Tenure_Emp;
                            entity.RV_Know_Subject = model.RV_Know_Subject;
                            entity.RV_Was_Cand_Honest = model.RV_Was_Cand_Honest;
                            entity.RV_Rate_Prof_Ability = model.RV_Rate_Prof_Ability;
                            entity.RV_Commun_Jr_Sr_Colleague = model.RV_Commun_Jr_Sr_Colleague;
                            entity.RV_Relationship_With_Sub = model.RV_Relationship_With_Sub;
                            entity.RV_Overall_Jb_Perf = model.RV_Overall_Jb_Perf;
                            entity.RV_Response_Wrtn_Verb = model.RV_Response_Wrtn_Verb;
                            entity.RV_State_Specify_Medcl_Prblm = model.RV_State_Specify_Medcl_Prblm;
                            entity.RV_Sate_Specify_Per_Prblm_Perf = model.RV_Sate_Specify_Per_Prblm_Perf;
                            entity.RV_Sub_Perf_Intgrty_Wrk = model.RV_Sub_Perf_Intgrty_Wrk;
                            entity.RV_Particular_Gud_Least_Effect = model.RV_Particular_Gud_Least_Effect;
                            entity.RV_Comment_Per_Qualities = model.RV_Comment_Per_Qualities;
                            entity.RV_Aware_Son_Daughtr_Appld_Jb = model.RV_Aware_Son_Daughtr_Appld_Jb;
                            entity.RV_Character = model.RV_Character;
                            entity.RV_Anger_Mngmt = model.RV_Anger_Mngmt;
                            entity.RV_Behaviour = model.RV_Behaviour;
                            entity.RV_Intgrty_Commitmnt = model.RV_Intgrty_Commitmnt;
                            entity.RV_Nature = model.RV_Nature;
                            entity.RV_Perf_level = model.RV_Perf_level;
                            entity.RV_SoftSkill = model.RV_SoftSkill;
                            entity.RV_Action_Orient = model.RV_Action_Orient;
                            entity.RV_Algmnt_Skills = model.RV_Algmnt_Skills;
                            entity.RV_Anger_Mgmt_Skills = model.RV_Anger_Mgmt_Skills;
                            entity.RV_Reliability = model.RV_Reliability;
                            entity.RV_Aptitutde = model.RV_Aptitutde;
                            entity.RV_Influence_Style = model.RV_Influence_Style;
                            entity.RV_Initiative = model.RV_Initiative;
                            entity.RV_Interper_Skills = model.RV_Interper_Skills;
                            entity.RV_Comm_Skills = model.RV_Comm_Skills;
                            entity.RV_Energy = model.RV_Energy;
                            entity.RV_Performance = model.RV_Performance;
                            entity.RV_Team_Mgmt_Skills = model.RV_Team_Mgmt_Skills;
                            entity.RV_Teamng_Skills = model.RV_Teamng_Skills;
                            entity.RV_Tech_Competence = model.RV_Tech_Competence;
                            entity.RV_Time_Mgmt_Org_Skills = model.RV_Time_Mgmt_Org_Skills;
                            entity.RV_Value_System = model.RV_Value_System;
                            entity.RV_Voice_Accent = model.RV_Voice_Accent;
                            entity.RV_Plan_Fr_Higher_Studies = model.RV_Plan_Fr_Higher_Studies;
                            entity.RV_What_kind_of_Jb = model.RV_What_kind_of_Jb;
                            entity.RV_Family_Background = model.RV_Family_Background;
                            entity.RV_His_Behaviour = model.RV_His_Behaviour;
                            entity.RV_His_Conduct = model.RV_His_Conduct;
                            entity.RV_His_Perf = model.RV_His_Perf;
                            entity.RV_Recommd_Work_MNC = model.RV_Recommd_Work_MNC;
                            entity.RV_Manag_Analyz_MIS_Plng = model.RV_Manag_Analyz_MIS_Plng;
                            entity.RV_Mngng_Int_Ext_Cust_Expect = model.RV_Mngng_Int_Ext_Cust_Expect;
                            entity.RV_Additional_Cmmnts_RC = model.RV_Additional_Cmmnts_RC;
                            entity.RV_Cmplnts_Rslt_Dsplnry_Sub = model.RV_Cmplnts_Rslt_Dsplnry_Sub;
                            entity.RV_Achivement_During_Tenure = model.RV_Achivement_During_Tenure;
                            entity.RV_Other_cmmnts_RC = model.RV_Other_cmmnts_RC;
                            entity.RV_Other_Qualitative_Input = model.RV_Other_Qualitative_Input;
                            entity.RV_Qualitative_Input_Plng = model.RV_Qualitative_Input_Plng;
                            entity.RV_Cand_Record_Misreprsnt_Facts = model.RV_Cand_Record_Misreprsnt_Facts;
                            entity.RV_Area_Development = model.RV_Area_Development;
                            entity.RV_Attendance_Punct_RC = model.RV_Attendance_Punct_RC;
                            entity.RV_Cand_Attitude_Task_Hndld = model.RV_Cand_Attitude_Task_Hndld;
                            entity.RV_Comm_Skills_RC = model.RV_Comm_Skills_RC;
                            entity.RV_Contribution_Intro_Concept = model.RV_Contribution_Intro_Concept;
                            entity.RV_Dsg_Refre = model.RV_Dsg_Refre;
                            entity.RV_Dsg_RC = model.RV_Dsg_RC;
                            entity.RV_CTC = model.RV_CTC;
                            entity.RV_Reason_Leav = model.RV_Reason_Leav;
                            entity.RV_Relationship_With_Sub_Dur = model.RV_Relationship_With_Sub_Dur;
                            entity.RV_Sub_Elgblty_Rhre = model.RV_Sub_Elgblty_Rhre;
                            entity.RV_Sub_Meet_Pos_Perf_Req = model.RV_Sub_Meet_Pos_Perf_Req;
                            entity.RV_Recall_Edu_Credential = model.RV_Recall_Edu_Credential;
                            entity.RV_Recall_Emp_Back = model.RV_Recall_Emp_Back;
                            entity.RV_Do_you_trust = model.RV_Do_you_trust;
                            entity.RV_Effect_Settng_Meetng_Goals = model.RV_Effect_Settng_Meetng_Goals;
                            entity.RV_Cand_Interpersonal_Skills = model.RV_Cand_Interpersonal_Skills;
                            entity.RV_Cand_Mngmt_Skills = model.RV_Cand_Mngmt_Skills;
                            entity.RV_Acquainted_Cand_how_long = model.RV_Acquainted_Cand_how_long;
                            entity.RV_Diligent_Twds_Work = model.RV_Diligent_Twds_Work;
                            entity.RV_How_Discipline = model.RV_How_Discipline;
                            entity.RV_How_Ppl_Respond = model.RV_How_Ppl_Respond;
                            entity.RV_Rate_Perf_Ability = model.RV_Rate_Perf_Ability;
                            entity.RV_Respond_Work_Pressure = model.RV_Respond_Work_Pressure;
                            entity.RV_Cand_Mtng_Target_Dline = model.RV_Cand_Mtng_Target_Dline;
                            entity.RV_As_Team_Player = model.RV_As_Team_Player;
                            entity.RV_Atude_Twds_Work = model.RV_Atude_Twds_Work;
                            entity.RV_Was_Cand_Motivated = model.RV_Was_Cand_Motivated;
                            entity.RV_How_Was_Behaviour = model.RV_How_Was_Behaviour;
                            entity.RV_Perform_Task_Time_Cost = model.RV_Perform_Task_Time_Cost;
                            entity.RV_Rate_Drive_Fr_Result = model.RV_Rate_Drive_Fr_Result;
                            entity.RV_Rate_Resilience_Perserve = model.RV_Rate_Resilience_Perserve;
                            entity.RV_Term_Resource_Discipline = model.RV_Term_Resource_Discipline;
                            entity.RV_Rhre_Cmpny_Again = model.RV_Rhre_Cmpny_Again;
                            entity.RV_Is_Reliable_Hon_Person = model.RV_Is_Reliable_Hon_Person;
                            entity.RV_Cand_High_Intgrty = model.RV_Cand_High_Intgrty;
                            entity.RV_Cand_Sincere_Hon = model.RV_Cand_Sincere_Hon;
                            entity.RV_Knowlge_Pos_Responsiblts = model.RV_Knowlge_Pos_Responsiblts;
                            entity.RV_Major_Carrer_Changes_Sub = model.RV_Major_Carrer_Changes_Sub;
                            entity.RV_Mngng_Mtng_Int_Cust_Expect = model.RV_Mngng_Mtng_Int_Cust_Expect;
                            entity.RV_Mgng_Complexity = model.RV_Mgng_Complexity;
                            entity.RV_Meeting_Deadlines = model.RV_Meeting_Deadlines;
                            entity.RV_Mode_Of_Exit = model.RV_Mode_Of_Exit;
                            entity.RV_Moral_Values = model.RV_Moral_Values;
                            entity.RV_Oral_Comm_Skills = model.RV_Oral_Comm_Skills;
                            entity.RV_Commnt_Ldrshp_Abilities = model.RV_Commnt_Ldrshp_Abilities;
                            entity.RV_Specify_Cand_Area_Improve = model.RV_Specify_Cand_Area_Improve;
                            entity.RV_Specify_Cand_Strength = model.RV_Specify_Cand_Strength;
                            entity.RV_Prblm_Solving_Ablts = model.RV_Prblm_Solving_Ablts;
                            entity.RV_Prof_Conduct = model.RV_Prof_Conduct;
                            entity.RV_Prof_Strength = model.RV_Prof_Strength;
                            entity.RV_Prof_Values = model.RV_Prof_Values;
                            entity.RV_Quality_Of_Work = model.RV_Quality_Of_Work;
                            entity.RV_Rating_On_Scale = model.RV_Rating_On_Scale;
                            entity.RV_Rating_Que_1 = model.RV_Rating_Que_1;
                            entity.RV_Rating_Que_2 = model.RV_Rating_Que_2;
                            entity.RV_Rating_Que_3 = model.RV_Rating_Que_3;
                            entity.RV_Rating_Que_4 = model.RV_Rating_Que_4;
                            entity.RV_Recommd_Similar_Pos = model.RV_Recommd_Similar_Pos;
                            entity.RV_Reslt_Orient = model.RV_Reslt_Orient;
                            entity.RV_Role_Responsibilities_Cand = model.RV_Role_Responsibilities_Cand;
                            entity.RV_Stress_Mgmt = model.RV_Stress_Mgmt;
                            entity.RV_Sub_Perf_Intgrty_Wrk_RC = model.RV_Sub_Perf_Intgrty_Wrk_RC;
                            entity.RV_Suff_Notice_Period_Prov = model.RV_Suff_Notice_Period_Prov;
                            entity.RV_Team_Conduct = model.RV_Team_Conduct;
                            entity.RV_Team_Ldrshp = model.RV_Team_Ldrshp;
                            entity.RV_Team_Mgmt = model.RV_Team_Mgmt;
                            entity.RV_Was_Sub_Team_Player = model.RV_Was_Sub_Team_Player;
                            entity.RV_Was_Cand_Involve_Union_Act = model.RV_Was_Cand_Involve_Union_Act;
                            entity.RV_Weakness = model.RV_Weakness;
                            entity.RV_Strength_Area_Developmnt = model.RV_Strength_Area_Developmnt;
                            entity.RV_Cand_Edu_Prof_Qualification = model.RV_Cand_Edu_Prof_Qualification;
                            entity.RV_Sub_Area_Develop_Weak = model.RV_Sub_Area_Develop_Weak;
                            entity.RV_Say_Abt_Enterpren_Skills = model.RV_Say_Abt_Enterpren_Skills;
                            entity.RV_Particiular_Gud_Least_Effect = model.RV_Particiular_Gud_Least_Effect;
                            entity.RV_Pos_Cand_Working = model.RV_Pos_Cand_Working;
                            entity.RV_TimePeriod_Cand_Work_Under = model.RV_TimePeriod_Cand_Work_Under;
                            entity.RV_Work_Ethnics_Moral_Intgrty = model.RV_Work_Ethnics_Moral_Intgrty;
                            entity.RV_Commnt_Per_Abilities = model.RV_Commnt_Per_Abilities;
                            entity.RV_Wrtn_Comm_Skills_RC = model.RV_Wrtn_Comm_Skills_RC;
                            entity.RV_AnyIssues = model.RV_AnyIssues;
                            entity.RV_Sub_Meet_Pos_Perf_Req_RC = model.RV_Sub_Meet_Pos_Perf_Req_RC;
                            entity.RV_Sub_Comm_Interper_Skills = model.RV_Sub_Comm_Interper_Skills;
                            entity.RV_Sub_Effect_Gettng_Rslt = model.RV_Sub_Effect_Gettng_Rslt;
                            entity.RV_Intgrty_Commitmnt_RC = model.RV_Intgrty_Commitmnt_RC;
                            entity.RV_Is_Reliable_Hon_Person_RC = model.RV_Is_Reliable_Hon_Person_RC;
                            entity.RV_Nature_Job = model.RV_Nature_Job;
                            entity.RV_Overall_Jb_Perf_RC = model.RV_Overall_Jb_Perf_RC;
                            entity.RV_Social_Conduct = model.RV_Social_Conduct;
                            entity.RV_Social_Conduct_RC = model.RV_Social_Conduct_RC;
                            entity.RV_Sub_Per_Prof_Weak = model.RV_Sub_Per_Prof_Weak;
                            entity.RV_Work_Ethics_Moral_Intgrty_Rc = model.RV_Work_Ethics_Moral_Intgrty_Rc;
                            entity.RV_Recommnd_Sub_Emp_Top_MNC = model.RV_Recommnd_Sub_Emp_Top_MNC;
                            entity.RV_Cmmnts_Sub_Strngth_Weak = model.RV_Cmmnts_Sub_Strngth_Weak;
                            entity.RV_Cmplnts_Rslt_Dsplnry_Sub_RC = model.RV_Cmplnts_Rslt_Dsplnry_Sub_RC;
                            entity.RV_Contribution_Intro_Concept_RC = model.RV_Contribution_Intro_Concept_RC;
                            entity.RV_Sub_Pos_Perf_Req_RC = model.RV_Sub_Pos_Perf_Req_RC;
                            entity.RV_Confrontation_Tackled = model.RV_Confrontation_Tackled;
                            entity.RV_Pressure_Sit_Hndld = model.RV_Pressure_Sit_Hndld;
                            entity.RV_React_To_Change = model.RV_React_To_Change;
                            entity.RV_Enforcing_Gud_Qualt_Work_Jrs = model.RV_Enforcing_Gud_Qualt_Work_Jrs;
                            entity.RV_Sub_Team_Leader = model.RV_Sub_Team_Leader;
                            entity.RV_Any_Other_Cmmnts_Fdback = model.RV_Any_Other_Cmmnts_Fdback;
                            entity.RV_Exp_During_Financ_Dealing = model.RV_Exp_During_Financ_Dealing;
                            entity.RV_Rel_Busines_Entity = model.RV_Rel_Busines_Entity;
                            entity.RV_Dur_Busines_Rel_Cmpny = model.RV_Dur_Busines_Rel_Cmpny;
                            entity.RV_OtherDetails6 = model.RV_OtherDetails6;
                            entity.RV_OtherDetails7 = model.RV_OtherDetails7;
                            entity.RV_OtherDetails8 = model.RV_OtherDetails8;
                            entity.RV_OtherDetails9 = model.RV_OtherDetails9;
                            entity.RV_OtherDetails10 = model.RV_OtherDetails10;
                            entity.RV_OtherDetails11 = model.RV_OtherDetails11;
                            entity.RV_OtherDetails12 = model.RV_OtherDetails12;
                            entity.RV_OtherDetails13 = model.RV_OtherDetails13;
                            entity.RV_OtherDetails14 = model.RV_OtherDetails14;
                            entity.RV_OtherDetails15 = model.RV_OtherDetails15;
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
                            entity.VerifiedDate =  model.VerifiedDate;

                            context.PQReferenceVers.Add(entity);
                            context.SaveChanges();

                            context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceCheckRowID).CheckStatus = "WIP-Pending";
                            context.SaveChanges();

                            int statusId = context.PQReferenceStatus.Where(p => p.ReferenceRowID == model.ReferenceCheckRowID).FirstOrDefault().ReferenceStatusRowID;
                            context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == statusId).TMCheckStatus = "WIP-Pending";
                            context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == statusId).TMQCCheckStatus = "";
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

        public PQPersonalDetailsVerViewModel GetPersonalDetailsForTM(int ReferenceRowId = 0)
        {
            try
            {
                PQPersonalDetailsVerViewModel model = new PQPersonalDetailsVerViewModel();
                model = db.PQReferences.Where(a => a.ReferenceRowID == ReferenceRowId).Select(a => new PQPersonalDetailsVerViewModel
                {
                    ClientRowID = a.ClientRowID,
                    PersonalRowID = a.PersonalRowID,
                    UniqueComponentID = a.UniqueComponentID,
                    //CandidateName = a.RV_CandidateName,


                    CandidateName = db.PQPersonals.Where(T => T.PersonalRowID == a.PersonalRowID).Select(x => x.Name + " " + x.MiddleName + x.LastName).FirstOrDefault().ToString(),

                    ClientName = a.PQClientMaster.MasterAbbreviation.ClientName,
                    CompanyReferenceNo = a.RV_Sec_Ref_Id,
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

        public PQReferenceVerViewModel GetReferenceForTM(int ReferenceRowId = 0)
        {
            try
            {
                PQReferenceVerViewModel model = new PQReferenceVerViewModel();
                model = db.PQReferences.Where(a => a.ReferenceRowID == ReferenceRowId).Select(a => new PQReferenceVerViewModel
                {

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
                    RV_OtherDetails6 = a.RV_OtherDetails6,
                    RV_OtherDetails7 = a.RV_OtherDetails7,
                    RV_OtherDetails8 = a.RV_OtherDetails8,
                    RV_OtherDetails9 = a.RV_OtherDetails9,
                    RV_OtherDetails10 = a.RV_OtherDetails10,
                    ATA_CID_No = a.ATA_CID_No,
                    ATA_Cmpny_Addr = a.ATA_Cmpny_Addr,
                    CheckStatus = a.CheckStatus,
                }).FirstOrDefault();


                return model;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public PQReferenceVerViewModel GetReferenceInfoForDataEntry(int ReferenceRowId = 0)
        {
            try
            {
                PQReferenceVerViewModel model = new PQReferenceVerViewModel();
                var ReferenceVer = db.PQReferenceVers.Where(a => a.ReferenceCheckRowID == ReferenceRowId).FirstOrDefault();
                int ReferenceVerRowId = 0;
                if (ReferenceVer != null && ReferenceVer.ReferenceCheckVerRowID > 0)
                {
                    ReferenceVerRowId = ReferenceVer.ReferenceCheckVerRowID;
                }

                if (ReferenceVerRowId > 0)
                {
                    #region Reference
                    model = db.PQReferenceVers.Where(a => a.ReferenceCheckVerRowID == ReferenceVerRowId).Select(a => new PQReferenceVerViewModel
                    {
                        ReferenceCheckRowID = a.ReferenceCheckRowID,
                        ReferenceCheckVerRowID = a.ReferenceCheckVerRowID,
                        ClientRowID = a.PQReferenceCheck.ClientRowID,
                        PersonalRowID = a.PQReferenceCheck.PersonalRowID,
                        CheckFamilyRowID = a.PQReferenceCheck.CheckFamilyRowID,
                        SubCheckRowID = a.PQReferenceCheck.SubCheckRowID,
                        RV_Cmplnts_Result_Dsplnry_Sub = a.RV_Cmplnts_Result_Dsplnry_Sub,
                        RV_Reliable_Honest_Person = a.RV_Reliable_Honest_Person,
                        RV_Cand_Fmly_Associat_Politics = a.RV_Cand_Fmly_Associat_Politics,
                        RV_Duration = a.RV_Duration,
                        RV_Relation_Sub = a.RV_Relation_Sub,
                        RV_Response_Frm_Rfre = a.RV_Response_Frm_Rfre,
                        RV_CandidateName = a.RV_CandidateName,
                        RV_OrganisationName = a.RV_OrganisationName,
                        RV_RefereeName = a.RV_RefereeName,
                        RV_RefereeDesignation = a.RV_RefereeDesignation,
                        RV_Refre_Prsnt_Emp = a.RV_Refre_Prsnt_Emp,
                        RV_Refre_ConDet = a.RV_Refre_ConDet,
                        RV_Refere_Desig = a.RV_Refere_Desig,
                        RV_Refere_Add = a.RV_Refere_Add,
                        RV_Refre_MobNo = a.RV_Refre_MobNo,
                        RV_Refre_Eid = a.RV_Refre_Eid,
                        RV_Prof_Relationship_Cand_Dur = a.RV_Prof_Relationship_Cand_Dur,
                        RV_Know_Long = a.RV_Know_Long,
                        RV_Tenure_Emp = a.RV_Tenure_Emp,
                        RV_Know_Subject = a.RV_Know_Subject,
                        RV_Was_Cand_Honest = a.RV_Was_Cand_Honest,
                        RV_Rate_Prof_Ability = a.RV_Rate_Prof_Ability,
                        RV_Commun_Jr_Sr_Colleague = a.RV_Commun_Jr_Sr_Colleague,
                        RV_Relationship_With_Sub = a.RV_Relationship_With_Sub,
                        RV_Overall_Jb_Perf = a.RV_Overall_Jb_Perf,
                        RV_Response_Wrtn_Verb = a.RV_Response_Wrtn_Verb,
                        RV_State_Specify_Medcl_Prblm = a.RV_State_Specify_Medcl_Prblm,
                        RV_Sate_Specify_Per_Prblm_Perf = a.RV_Sate_Specify_Per_Prblm_Perf,
                        RV_Sub_Perf_Intgrty_Wrk = a.RV_Sub_Perf_Intgrty_Wrk,
                        RV_Particular_Gud_Least_Effect = a.RV_Particular_Gud_Least_Effect,
                        RV_Comment_Per_Qualities = a.RV_Comment_Per_Qualities,
                        RV_Aware_Son_Daughtr_Appld_Jb = a.RV_Aware_Son_Daughtr_Appld_Jb,
                        RV_Character = a.RV_Character,
                        RV_Anger_Mngmt = a.RV_Anger_Mngmt,
                        RV_Behaviour = a.RV_Behaviour,
                        RV_Intgrty_Commitmnt = a.RV_Intgrty_Commitmnt,
                        RV_Nature = a.RV_Nature,
                        RV_Perf_level = a.RV_Perf_level,
                        RV_SoftSkill = a.RV_SoftSkill,
                        RV_Action_Orient = a.RV_Action_Orient,
                        RV_Algmnt_Skills = a.RV_Algmnt_Skills,
                        RV_Anger_Mgmt_Skills = a.RV_Anger_Mgmt_Skills,
                        RV_Reliability = a.RV_Reliability,
                        RV_Aptitutde = a.RV_Aptitutde,
                        RV_Influence_Style = a.RV_Influence_Style,
                        RV_Initiative = a.RV_Initiative,
                        RV_Interper_Skills = a.RV_Interper_Skills,
                        RV_Comm_Skills = a.RV_Comm_Skills,
                        RV_Energy = a.RV_Energy,
                        RV_Performance = a.RV_Performance,
                        RV_Team_Mgmt_Skills = a.RV_Team_Mgmt_Skills,
                        RV_Teamng_Skills = a.RV_Teamng_Skills,
                        RV_Tech_Competence = a.RV_Tech_Competence,
                        RV_Time_Mgmt_Org_Skills = a.RV_Time_Mgmt_Org_Skills,
                        RV_Value_System = a.RV_Value_System,
                        RV_Voice_Accent = a.RV_Voice_Accent,
                        RV_Plan_Fr_Higher_Studies = a.RV_Plan_Fr_Higher_Studies,
                        RV_What_kind_of_Jb = a.RV_What_kind_of_Jb,
                        RV_Family_Background = a.RV_Family_Background,
                        RV_His_Behaviour = a.RV_His_Behaviour,
                        RV_His_Conduct = a.RV_His_Conduct,
                        RV_His_Perf = a.RV_His_Perf,
                        RV_Recommd_Work_MNC = a.RV_Recommd_Work_MNC,
                        RV_Manag_Analyz_MIS_Plng = a.RV_Manag_Analyz_MIS_Plng,
                        RV_Mngng_Int_Ext_Cust_Expect = a.RV_Mngng_Int_Ext_Cust_Expect,
                        RV_Additional_Cmmnts_RC = a.RV_Additional_Cmmnts_RC,
                        RV_Cmplnts_Rslt_Dsplnry_Sub = a.RV_Cmplnts_Rslt_Dsplnry_Sub,
                        RV_Achivement_During_Tenure = a.RV_Achivement_During_Tenure,
                        RV_Other_cmmnts_RC = a.RV_Other_cmmnts_RC,
                        RV_Other_Qualitative_Input = a.RV_Other_Qualitative_Input,
                        RV_Qualitative_Input_Plng = a.RV_Qualitative_Input_Plng,
                        RV_Cand_Record_Misreprsnt_Facts = a.RV_Cand_Record_Misreprsnt_Facts,
                        RV_Area_Development = a.RV_Area_Development,
                        RV_Attendance_Punct_RC = a.RV_Attendance_Punct_RC,
                        RV_Cand_Attitude_Task_Hndld = a.RV_Cand_Attitude_Task_Hndld,
                        RV_Comm_Skills_RC = a.RV_Comm_Skills_RC,
                        RV_Contribution_Intro_Concept = a.RV_Contribution_Intro_Concept,
                        RV_Dsg_Refre = a.RV_Dsg_Refre,
                        RV_Dsg_RC = a.RV_Dsg_RC,
                        RV_CTC = a.RV_CTC,
                        RV_Reason_Leav = a.RV_Reason_Leav,
                        RV_Relationship_With_Sub_Dur = a.RV_Relationship_With_Sub_Dur,
                        RV_Sub_Elgblty_Rhre = a.RV_Sub_Elgblty_Rhre,
                        RV_Sub_Meet_Pos_Perf_Req = a.RV_Sub_Meet_Pos_Perf_Req,
                        RV_Recall_Edu_Credential = a.RV_Recall_Edu_Credential,
                        RV_Recall_Emp_Back = a.RV_Recall_Emp_Back,
                        RV_Do_you_trust = a.RV_Do_you_trust,
                        RV_Effect_Settng_Meetng_Goals = a.RV_Effect_Settng_Meetng_Goals,
                        RV_Cand_Interpersonal_Skills = a.RV_Cand_Interpersonal_Skills,
                        RV_Cand_Mngmt_Skills = a.RV_Cand_Mngmt_Skills,
                        RV_Acquainted_Cand_how_long = a.RV_Acquainted_Cand_how_long,
                        RV_Diligent_Twds_Work = a.RV_Diligent_Twds_Work,
                        RV_How_Discipline = a.RV_How_Discipline,
                        RV_How_Ppl_Respond = a.RV_How_Ppl_Respond,
                        RV_Rate_Perf_Ability = a.RV_Rate_Perf_Ability,
                        RV_Respond_Work_Pressure = a.RV_Respond_Work_Pressure,
                        RV_Cand_Mtng_Target_Dline = a.RV_Cand_Mtng_Target_Dline,
                        RV_As_Team_Player = a.RV_As_Team_Player,
                        RV_Atude_Twds_Work = a.RV_Atude_Twds_Work,
                        RV_Was_Cand_Motivated = a.RV_Was_Cand_Motivated,
                        RV_How_Was_Behaviour = a.RV_How_Was_Behaviour,
                        RV_Perform_Task_Time_Cost = a.RV_Perform_Task_Time_Cost,
                        RV_Rate_Drive_Fr_Result = a.RV_Rate_Drive_Fr_Result,
                        RV_Rate_Resilience_Perserve = a.RV_Rate_Resilience_Perserve,
                        RV_Term_Resource_Discipline = a.RV_Term_Resource_Discipline,
                        RV_Rhre_Cmpny_Again = a.RV_Rhre_Cmpny_Again,
                        RV_Is_Reliable_Hon_Person = a.RV_Is_Reliable_Hon_Person,
                        RV_Cand_High_Intgrty = a.RV_Cand_High_Intgrty,
                        RV_Cand_Sincere_Hon = a.RV_Cand_Sincere_Hon,
                        RV_Knowlge_Pos_Responsiblts = a.RV_Knowlge_Pos_Responsiblts,
                        RV_Major_Carrer_Changes_Sub = a.RV_Major_Carrer_Changes_Sub,
                        RV_Mngng_Mtng_Int_Cust_Expect = a.RV_Mngng_Mtng_Int_Cust_Expect,
                        RV_Mgng_Complexity = a.RV_Mgng_Complexity,
                        RV_Meeting_Deadlines = a.RV_Meeting_Deadlines,
                        RV_Mode_Of_Exit = a.RV_Mode_Of_Exit,
                        RV_Moral_Values = a.RV_Moral_Values,
                        RV_Oral_Comm_Skills = a.RV_Oral_Comm_Skills,
                        RV_Commnt_Ldrshp_Abilities = a.RV_Commnt_Ldrshp_Abilities,
                        RV_Specify_Cand_Area_Improve = a.RV_Specify_Cand_Area_Improve,
                        RV_Specify_Cand_Strength = a.RV_Specify_Cand_Strength,
                        RV_Prblm_Solving_Ablts = a.RV_Prblm_Solving_Ablts,
                        RV_Prof_Conduct = a.RV_Prof_Conduct,
                        RV_Prof_Strength = a.RV_Prof_Strength,
                        RV_Prof_Values = a.RV_Prof_Values,
                        RV_Quality_Of_Work = a.RV_Quality_Of_Work,
                        RV_Rating_On_Scale = a.RV_Rating_On_Scale,
                        RV_Rating_Que_1 = a.RV_Rating_Que_1,
                        RV_Rating_Que_2 = a.RV_Rating_Que_2,
                        RV_Rating_Que_3 = a.RV_Rating_Que_3,
                        RV_Rating_Que_4 = a.RV_Rating_Que_4,
                        RV_Recommd_Similar_Pos = a.RV_Recommd_Similar_Pos,
                        RV_Reslt_Orient = a.RV_Reslt_Orient,
                        RV_Role_Responsibilities_Cand = a.RV_Role_Responsibilities_Cand,
                        RV_Stress_Mgmt = a.RV_Stress_Mgmt,
                        RV_Sub_Perf_Intgrty_Wrk_RC = a.RV_Sub_Perf_Intgrty_Wrk_RC,
                        RV_Suff_Notice_Period_Prov = a.RV_Suff_Notice_Period_Prov,
                        RV_Team_Conduct = a.RV_Team_Conduct,
                        RV_Team_Ldrshp = a.RV_Team_Ldrshp,
                        RV_Team_Mgmt = a.RV_Team_Mgmt,
                        RV_Was_Sub_Team_Player = a.RV_Was_Sub_Team_Player,
                        RV_Was_Cand_Involve_Union_Act = a.RV_Was_Cand_Involve_Union_Act,
                        RV_Weakness = a.RV_Weakness,
                        RV_Strength_Area_Developmnt = a.RV_Strength_Area_Developmnt,
                        RV_Cand_Edu_Prof_Qualification = a.RV_Cand_Edu_Prof_Qualification,
                        RV_Sub_Area_Develop_Weak = a.RV_Sub_Area_Develop_Weak,
                        RV_Say_Abt_Enterpren_Skills = a.RV_Say_Abt_Enterpren_Skills,
                        RV_Particiular_Gud_Least_Effect = a.RV_Particiular_Gud_Least_Effect,
                        RV_Pos_Cand_Working = a.RV_Pos_Cand_Working,
                        RV_TimePeriod_Cand_Work_Under = a.RV_TimePeriod_Cand_Work_Under,
                        RV_Work_Ethnics_Moral_Intgrty = a.RV_Work_Ethnics_Moral_Intgrty,
                        RV_Commnt_Per_Abilities = a.RV_Commnt_Per_Abilities,
                        RV_Wrtn_Comm_Skills_RC = a.RV_Wrtn_Comm_Skills_RC,
                        RV_AnyIssues = a.RV_AnyIssues,
                        RV_Sub_Meet_Pos_Perf_Req_RC = a.RV_Sub_Meet_Pos_Perf_Req_RC,
                        RV_Sub_Comm_Interper_Skills = a.RV_Sub_Comm_Interper_Skills,
                        RV_Sub_Effect_Gettng_Rslt = a.RV_Sub_Effect_Gettng_Rslt,
                        RV_Intgrty_Commitmnt_RC = a.RV_Intgrty_Commitmnt_RC,
                        RV_Is_Reliable_Hon_Person_RC = a.RV_Is_Reliable_Hon_Person_RC,
                        RV_Nature_Job = a.RV_Nature_Job,
                        RV_Overall_Jb_Perf_RC = a.RV_Overall_Jb_Perf_RC,
                        RV_Social_Conduct = a.RV_Social_Conduct,
                        RV_Social_Conduct_RC = a.RV_Social_Conduct_RC,
                        RV_Sub_Per_Prof_Weak = a.RV_Sub_Per_Prof_Weak,
                        RV_Work_Ethics_Moral_Intgrty_Rc = a.RV_Work_Ethics_Moral_Intgrty_Rc,
                        RV_Recommnd_Sub_Emp_Top_MNC = a.RV_Recommnd_Sub_Emp_Top_MNC,
                        RV_Cmmnts_Sub_Strngth_Weak = a.RV_Cmmnts_Sub_Strngth_Weak,
                        RV_Cmplnts_Rslt_Dsplnry_Sub_RC = a.RV_Cmplnts_Rslt_Dsplnry_Sub_RC,
                        RV_Contribution_Intro_Concept_RC = a.RV_Contribution_Intro_Concept_RC,
                        RV_Sub_Pos_Perf_Req_RC = a.RV_Sub_Pos_Perf_Req_RC,
                        RV_Confrontation_Tackled = a.RV_Confrontation_Tackled,
                        RV_Pressure_Sit_Hndld = a.RV_Pressure_Sit_Hndld,
                        RV_React_To_Change = a.RV_React_To_Change,
                        RV_Enforcing_Gud_Qualt_Work_Jrs = a.RV_Enforcing_Gud_Qualt_Work_Jrs,
                        RV_Sub_Team_Leader = a.RV_Sub_Team_Leader,
                        RV_Any_Other_Cmmnts_Fdback = a.RV_Any_Other_Cmmnts_Fdback,
                        RV_Exp_During_Financ_Dealing = a.RV_Exp_During_Financ_Dealing,
                        RV_Rel_Busines_Entity = a.RV_Rel_Busines_Entity,
                        RV_Dur_Busines_Rel_Cmpny = a.RV_Dur_Busines_Rel_Cmpny,
                        RV_OtherDetails6 = a.RV_OtherDetails6,
                        RV_OtherDetails7 = a.RV_OtherDetails7,
                        RV_OtherDetails8 = a.RV_OtherDetails8,
                        RV_OtherDetails9 = a.RV_OtherDetails9,
                        RV_OtherDetails10 = a.RV_OtherDetails10,
                        RV_OtherDetails11 = a.RV_OtherDetails11,
                        RV_OtherDetails12 = a.RV_OtherDetails12,
                        RV_OtherDetails13 = a.RV_OtherDetails13,
                        RV_OtherDetails14 = a.RV_OtherDetails14,
                        RV_OtherDetails15 = a.RV_OtherDetails15,
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
                        CheckStatus = a.PQReferenceCheck.CheckStatus,
                    }).FirstOrDefault();
                    #endregion
                }
                else
                {
                    #region Reference
                    model = db.PQReferences.Where(a => a.ReferenceRowID == ReferenceRowId).Select(a => new PQReferenceVerViewModel
                    {
                        ReferenceCheckRowID = a.ReferenceRowID,
                        ReferenceCheckVerRowID = 0,
                        ClientRowID = a.ClientRowID,
                        PersonalRowID = a.PersonalRowID,
                        CheckFamilyRowID = a.CheckFamilyRowID,
                        SubCheckRowID = a.SubCheckRowID,
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
                        RV_OtherDetails6 = a.RV_OtherDetails6,
                        RV_OtherDetails7 = a.RV_OtherDetails7,
                        RV_OtherDetails8 = a.RV_OtherDetails8,
                        RV_OtherDetails9 = a.RV_OtherDetails9,
                        RV_OtherDetails10 = a.RV_OtherDetails10,
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

        public ReferenceVerCloserViewModel GetReferenceVerCloserDetails(int RefVerId = 0)
        {
            ReferenceVerCloserViewModel model = new ReferenceVerCloserViewModel();
            var Disposition = db.PQReferenceVerDispositions.Where(s => s.ReferenceCheckVerRowID == RefVerId).ToList();
            List<int> selectList = new List<int>();
            foreach (var item in Disposition)
            {
                selectList.Add(Convert.ToInt32(item.ClientDispositionRowId));
            }
            //string type = string.Join(",", selectList.ToArray());           
            var Dispositions = db.PQReferenceVerDispositions.Where(d => d.ReferenceCheckVerRowID == RefVerId).Select(a => new ReferenceVerCloserViewModel
            {
                CDispositionRowId = selectList,
                ColorName = a.PQReferenceVer.ColorName,
                Severity = a.PQReferenceVer.Severity,
                Remarks = a.PQReferenceVer.ReportComments,
                CheckStatus = a.PQReferenceVer.CheckStatus,
            }).FirstOrDefault();

            var entity = db.PQReferenceVers.Find(RefVerId);
            if (entity != null)
            {
                if (Dispositions == null)
                {
                    Dispositions = new ReferenceVerCloserViewModel();
                    Dispositions.ReferenceCheckRowID = entity.ReferenceCheckRowID;
                    Dispositions.ReferenceVerRowID = entity.ReferenceCheckVerRowID;
                    Dispositions.ClientRowID = entity.PQReferenceCheck.ClientRowID;
                    Dispositions.CheckFamilyRowID = entity.PQReferenceCheck.CheckFamilyRowID;
                    Dispositions.SubCheckRowID = entity.PQReferenceCheck.SubCheckRowID;
                    Dispositions.PersonalRowID = entity.PQReferenceCheck.PersonalRowID;
                }
                else
                {
                    Dispositions.ReferenceCheckRowID = entity.ReferenceCheckRowID;
                    Dispositions.ReferenceVerRowID = entity.ReferenceCheckVerRowID;
                    Dispositions.ClientRowID = entity.PQReferenceCheck.ClientRowID;
                    Dispositions.CheckFamilyRowID = entity.PQReferenceCheck.CheckFamilyRowID;
                    Dispositions.SubCheckRowID = entity.PQReferenceCheck.SubCheckRowID;
                    Dispositions.PersonalRowID = entity.PQReferenceCheck.PersonalRowID;
                }
            }
            return Dispositions;
        }

        public void AddReferenceVerCloserDetails(ReferenceVerCloserViewModel model)
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
                            List<PQReferenceVerDisposition> VerDispositionsList = new List<PQReferenceVerDisposition>();
                            var FindReferenceVerDisposition = context.PQReferenceVerDispositions.Where(a => a.ReferenceCheckVerRowID == model.ReferenceVerRowID).ToList();

                            if (FindReferenceVerDisposition != null && FindReferenceVerDisposition.Count > 0)
                            {
                                context.PQReferenceVerDispositions.RemoveRange(FindReferenceVerDisposition);
                                context.SaveChanges();
                            }
                            foreach (var item in DispositionIds)
                            {
                                PQReferenceVerDisposition VerEntity = new PQReferenceVerDisposition
                                {
                                    ReferenceCheckVerRowID = model.ReferenceVerRowID,
                                    ClientDispositionRowId = Convert.ToInt32(item)
                                };
                                VerDispositionsList.Add(VerEntity);
                            }
                            context.PQReferenceVerDispositions.AddRange(VerDispositionsList);
                            context.SaveChanges();
                            #endregion
                        }

                        //Update in ReferenceVer Table            
                        context.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceVerRowID).ReportComments = model.Remarks;
                        context.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceVerRowID).Severity = model.Severity;
                        context.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceVerRowID).ColorName = model.ColorName;
                        context.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceVerRowID).CheckStatus = model.CheckStatus;
                        context.SaveChanges();
                        int statusId = context.PQReferenceStatus.Where(p => p.ReferenceRowID == model.ReferenceCheckRowID).FirstOrDefault().ReferenceStatusRowID;

                        if (model.CheckStatus == "Review Requested")
                        {
                            context.PQReferences.Single(p => p.ReferenceRowID == model.ReferenceCheckRowID).CheckStatus = model.CheckStatus;
                            context.SaveChanges();

                            context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == statusId).TMCheckStatus = model.CheckStatus;
                            context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == statusId).Remarks = model.ActionHistory;
                            context.SaveChanges();
                        }
                        if (model.CheckStatus == "Completed")
                        {
                            context.PQReferences.Single(p => p.ReferenceRowID == model.ReferenceCheckRowID).CheckStatus = "Completed-TM";
                            context.SaveChanges();

                            context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == statusId).TMQCCheckStatus = model.CheckStatus;
                            context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == statusId).TMCheckStatus = model.CheckStatus;
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

        public void UpdateReferenceByTeamMember(PQReferenceVerViewModel model)
        {
            try
            {
                if (model != null)
                {
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).ReferenceCheckRowID = model.ReferenceCheckRowID;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Cmplnts_Result_Dsplnry_Sub = model.RV_Cmplnts_Result_Dsplnry_Sub;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Reliable_Honest_Person = model.RV_Reliable_Honest_Person;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Cand_Fmly_Associat_Politics = model.RV_Cand_Fmly_Associat_Politics;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Duration = model.RV_Duration;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Relation_Sub = model.RV_Relation_Sub;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Response_Frm_Rfre = model.RV_Response_Frm_Rfre;
                    //db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_CandidateName = model.RV_CandidateName;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_OrganisationName = model.RV_OrganisationName;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_RefereeName = model.RV_RefereeName;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_RefereeDesignation = model.RV_RefereeDesignation;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Refre_Prsnt_Emp = model.RV_Refre_Prsnt_Emp;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Refre_ConDet = model.RV_Refre_ConDet;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Refere_Desig = model.RV_Refere_Desig;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Refere_Add = model.RV_Refere_Add;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Refre_MobNo = model.RV_Refre_MobNo;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Refre_Eid = model.RV_Refre_Eid;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Prof_Relationship_Cand_Dur = model.RV_Prof_Relationship_Cand_Dur;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Know_Long = model.RV_Know_Long;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Tenure_Emp = model.RV_Tenure_Emp;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Know_Subject = model.RV_Know_Subject;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Was_Cand_Honest = model.RV_Was_Cand_Honest;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Rate_Prof_Ability = model.RV_Rate_Prof_Ability;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Commun_Jr_Sr_Colleague = model.RV_Commun_Jr_Sr_Colleague;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Relationship_With_Sub = model.RV_Relationship_With_Sub;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Overall_Jb_Perf = model.RV_Overall_Jb_Perf;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Response_Wrtn_Verb = model.RV_Response_Wrtn_Verb;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_State_Specify_Medcl_Prblm = model.RV_State_Specify_Medcl_Prblm;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Sate_Specify_Per_Prblm_Perf = model.RV_Sate_Specify_Per_Prblm_Perf;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Sub_Perf_Intgrty_Wrk = model.RV_Sub_Perf_Intgrty_Wrk;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Particular_Gud_Least_Effect = model.RV_Particular_Gud_Least_Effect;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Comment_Per_Qualities = model.RV_Comment_Per_Qualities;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Aware_Son_Daughtr_Appld_Jb = model.RV_Aware_Son_Daughtr_Appld_Jb;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Character = model.RV_Character;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Anger_Mngmt = model.RV_Anger_Mngmt;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Behaviour = model.RV_Behaviour;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Intgrty_Commitmnt = model.RV_Intgrty_Commitmnt;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Nature = model.RV_Nature;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Perf_level = model.RV_Perf_level;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_SoftSkill = model.RV_SoftSkill;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Action_Orient = model.RV_Action_Orient;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Algmnt_Skills = model.RV_Algmnt_Skills;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Anger_Mgmt_Skills = model.RV_Anger_Mgmt_Skills;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Reliability = model.RV_Reliability;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Aptitutde = model.RV_Aptitutde;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Influence_Style = model.RV_Influence_Style;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Initiative = model.RV_Initiative;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Interper_Skills = model.RV_Interper_Skills;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Comm_Skills = model.RV_Comm_Skills;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Energy = model.RV_Energy;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Performance = model.RV_Performance;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Team_Mgmt_Skills = model.RV_Team_Mgmt_Skills;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Teamng_Skills = model.RV_Teamng_Skills;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Tech_Competence = model.RV_Tech_Competence;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Time_Mgmt_Org_Skills = model.RV_Time_Mgmt_Org_Skills;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Value_System = model.RV_Value_System;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Voice_Accent = model.RV_Voice_Accent;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Plan_Fr_Higher_Studies = model.RV_Plan_Fr_Higher_Studies;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_What_kind_of_Jb = model.RV_What_kind_of_Jb;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Family_Background = model.RV_Family_Background;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_His_Behaviour = model.RV_His_Behaviour;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_His_Conduct = model.RV_His_Conduct;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_His_Perf = model.RV_His_Perf;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Recommd_Work_MNC = model.RV_Recommd_Work_MNC;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Manag_Analyz_MIS_Plng = model.RV_Manag_Analyz_MIS_Plng;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Mngng_Int_Ext_Cust_Expect = model.RV_Mngng_Int_Ext_Cust_Expect;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Additional_Cmmnts_RC = model.RV_Additional_Cmmnts_RC;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Cmplnts_Rslt_Dsplnry_Sub = model.RV_Cmplnts_Rslt_Dsplnry_Sub;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Achivement_During_Tenure = model.RV_Achivement_During_Tenure;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Other_cmmnts_RC = model.RV_Other_cmmnts_RC;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Other_Qualitative_Input = model.RV_Other_Qualitative_Input;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Qualitative_Input_Plng = model.RV_Qualitative_Input_Plng;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Cand_Record_Misreprsnt_Facts = model.RV_Cand_Record_Misreprsnt_Facts;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Area_Development = model.RV_Area_Development;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Attendance_Punct_RC = model.RV_Attendance_Punct_RC;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Cand_Attitude_Task_Hndld = model.RV_Cand_Attitude_Task_Hndld;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Comm_Skills_RC = model.RV_Comm_Skills_RC;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Contribution_Intro_Concept = model.RV_Contribution_Intro_Concept;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Dsg_Refre = model.RV_Dsg_Refre;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Dsg_RC = model.RV_Dsg_RC;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_CTC = model.RV_CTC;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Reason_Leav = model.RV_Reason_Leav;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Relationship_With_Sub_Dur = model.RV_Relationship_With_Sub_Dur;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Sub_Elgblty_Rhre = model.RV_Sub_Elgblty_Rhre;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Sub_Meet_Pos_Perf_Req = model.RV_Sub_Meet_Pos_Perf_Req;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Recall_Edu_Credential = model.RV_Recall_Edu_Credential;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Recall_Emp_Back = model.RV_Recall_Emp_Back;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Do_you_trust = model.RV_Do_you_trust;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Effect_Settng_Meetng_Goals = model.RV_Effect_Settng_Meetng_Goals;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Cand_Interpersonal_Skills = model.RV_Cand_Interpersonal_Skills;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Cand_Mngmt_Skills = model.RV_Cand_Mngmt_Skills;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Acquainted_Cand_how_long = model.RV_Acquainted_Cand_how_long;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Diligent_Twds_Work = model.RV_Diligent_Twds_Work;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_How_Discipline = model.RV_How_Discipline;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_How_Ppl_Respond = model.RV_How_Ppl_Respond;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Rate_Perf_Ability = model.RV_Rate_Perf_Ability;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Respond_Work_Pressure = model.RV_Respond_Work_Pressure;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Cand_Mtng_Target_Dline = model.RV_Cand_Mtng_Target_Dline;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_As_Team_Player = model.RV_As_Team_Player;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Atude_Twds_Work = model.RV_Atude_Twds_Work;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Was_Cand_Motivated = model.RV_Was_Cand_Motivated;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_How_Was_Behaviour = model.RV_How_Was_Behaviour;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Perform_Task_Time_Cost = model.RV_Perform_Task_Time_Cost;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Rate_Drive_Fr_Result = model.RV_Rate_Drive_Fr_Result;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Rate_Resilience_Perserve = model.RV_Rate_Resilience_Perserve;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Term_Resource_Discipline = model.RV_Term_Resource_Discipline;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Rhre_Cmpny_Again = model.RV_Rhre_Cmpny_Again;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Is_Reliable_Hon_Person = model.RV_Is_Reliable_Hon_Person;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Cand_High_Intgrty = model.RV_Cand_High_Intgrty;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Cand_Sincere_Hon = model.RV_Cand_Sincere_Hon;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Knowlge_Pos_Responsiblts = model.RV_Knowlge_Pos_Responsiblts;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Major_Carrer_Changes_Sub = model.RV_Major_Carrer_Changes_Sub;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Mngng_Mtng_Int_Cust_Expect = model.RV_Mngng_Mtng_Int_Cust_Expect;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Mgng_Complexity = model.RV_Mgng_Complexity;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Meeting_Deadlines = model.RV_Meeting_Deadlines;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Mode_Of_Exit = model.RV_Mode_Of_Exit;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Moral_Values = model.RV_Moral_Values;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Oral_Comm_Skills = model.RV_Oral_Comm_Skills;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Commnt_Ldrshp_Abilities = model.RV_Commnt_Ldrshp_Abilities;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Specify_Cand_Area_Improve = model.RV_Specify_Cand_Area_Improve;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Specify_Cand_Strength = model.RV_Specify_Cand_Strength;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Prblm_Solving_Ablts = model.RV_Prblm_Solving_Ablts;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Prof_Conduct = model.RV_Prof_Conduct;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Prof_Strength = model.RV_Prof_Strength;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Prof_Values = model.RV_Prof_Values;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Quality_Of_Work = model.RV_Quality_Of_Work;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Rating_On_Scale = model.RV_Rating_On_Scale;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Rating_Que_1 = model.RV_Rating_Que_1;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Rating_Que_2 = model.RV_Rating_Que_2;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Rating_Que_3 = model.RV_Rating_Que_3;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Rating_Que_4 = model.RV_Rating_Que_4;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Recommd_Similar_Pos = model.RV_Recommd_Similar_Pos;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Reslt_Orient = model.RV_Reslt_Orient;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Role_Responsibilities_Cand = model.RV_Role_Responsibilities_Cand;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Stress_Mgmt = model.RV_Stress_Mgmt;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Sub_Perf_Intgrty_Wrk_RC = model.RV_Sub_Perf_Intgrty_Wrk_RC;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Suff_Notice_Period_Prov = model.RV_Suff_Notice_Period_Prov;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Team_Conduct = model.RV_Team_Conduct;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Team_Ldrshp = model.RV_Team_Ldrshp;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Team_Mgmt = model.RV_Team_Mgmt;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Was_Sub_Team_Player = model.RV_Was_Sub_Team_Player;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Was_Cand_Involve_Union_Act = model.RV_Was_Cand_Involve_Union_Act;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Weakness = model.RV_Weakness;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Strength_Area_Developmnt = model.RV_Strength_Area_Developmnt;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Cand_Edu_Prof_Qualification = model.RV_Cand_Edu_Prof_Qualification;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Sub_Area_Develop_Weak = model.RV_Sub_Area_Develop_Weak;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Say_Abt_Enterpren_Skills = model.RV_Say_Abt_Enterpren_Skills;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Particiular_Gud_Least_Effect = model.RV_Particiular_Gud_Least_Effect;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Pos_Cand_Working = model.RV_Pos_Cand_Working;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_TimePeriod_Cand_Work_Under = model.RV_TimePeriod_Cand_Work_Under;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Work_Ethnics_Moral_Intgrty = model.RV_Work_Ethnics_Moral_Intgrty;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Commnt_Per_Abilities = model.RV_Commnt_Per_Abilities;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Wrtn_Comm_Skills_RC = model.RV_Wrtn_Comm_Skills_RC;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_AnyIssues = model.RV_AnyIssues;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Sub_Meet_Pos_Perf_Req_RC = model.RV_Sub_Meet_Pos_Perf_Req_RC;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Sub_Comm_Interper_Skills = model.RV_Sub_Comm_Interper_Skills;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Sub_Effect_Gettng_Rslt = model.RV_Sub_Effect_Gettng_Rslt;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Intgrty_Commitmnt_RC = model.RV_Intgrty_Commitmnt_RC;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Is_Reliable_Hon_Person_RC = model.RV_Is_Reliable_Hon_Person_RC;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Nature_Job = model.RV_Nature_Job;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Overall_Jb_Perf_RC = model.RV_Overall_Jb_Perf_RC;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Social_Conduct = model.RV_Social_Conduct;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Social_Conduct_RC = model.RV_Social_Conduct_RC;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Sub_Per_Prof_Weak = model.RV_Sub_Per_Prof_Weak;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Work_Ethics_Moral_Intgrty_Rc = model.RV_Work_Ethics_Moral_Intgrty_Rc;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Recommnd_Sub_Emp_Top_MNC = model.RV_Recommnd_Sub_Emp_Top_MNC;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Cmmnts_Sub_Strngth_Weak = model.RV_Cmmnts_Sub_Strngth_Weak;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Cmplnts_Rslt_Dsplnry_Sub_RC = model.RV_Cmplnts_Rslt_Dsplnry_Sub_RC;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Contribution_Intro_Concept_RC = model.RV_Contribution_Intro_Concept_RC;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Sub_Pos_Perf_Req_RC = model.RV_Sub_Pos_Perf_Req_RC;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Confrontation_Tackled = model.RV_Confrontation_Tackled;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Pressure_Sit_Hndld = model.RV_Pressure_Sit_Hndld;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_React_To_Change = model.RV_React_To_Change;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Enforcing_Gud_Qualt_Work_Jrs = model.RV_Enforcing_Gud_Qualt_Work_Jrs;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Sub_Team_Leader = model.RV_Sub_Team_Leader;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Any_Other_Cmmnts_Fdback = model.RV_Any_Other_Cmmnts_Fdback;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Exp_During_Financ_Dealing = model.RV_Exp_During_Financ_Dealing;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Rel_Busines_Entity = model.RV_Rel_Busines_Entity;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Dur_Busines_Rel_Cmpny = model.RV_Dur_Busines_Rel_Cmpny;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_OtherDetails6 = model.RV_OtherDetails6;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_OtherDetails7 = model.RV_OtherDetails7;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_OtherDetails8 = model.RV_OtherDetails8;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_OtherDetails9 = model.RV_OtherDetails9;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_OtherDetails10 = model.RV_OtherDetails10;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_OtherDetails11 = model.RV_OtherDetails11;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_OtherDetails12 = model.RV_OtherDetails12;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_OtherDetails13 = model.RV_OtherDetails13;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_OtherDetails14 = model.RV_OtherDetails14;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_OtherDetails15 = model.RV_OtherDetails15;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).ATA_Reverse_Directory = model.ATA_Reverse_Directory;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).ATA_Residcl_Commrcl = model.ATA_Residcl_Commrcl;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).ATA_Stock_Exchnge = model.ATA_Stock_Exchnge;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).ATA_Telphne_Directry_Srch = model.ATA_Telphne_Directry_Srch;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).ATA_Yellow_Pages = model.ATA_Yellow_Pages;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).ATA_Who_Domain = model.ATA_Who_Domain;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).ATA_GoogleSearch = model.ATA_GoogleSearch;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).ATA_InfrastructureOfCmp = model.ATA_InfrastructureOfCmp;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).ATA_Just_Dial = model.ATA_Just_Dial;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).ATA_Neighbor_Check1 = model.ATA_Neighbor_Check1;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).ATA_NASCOM_Empanelment = model.ATA_NASCOM_Empanelment;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).ATA_Photograph = model.ATA_Photograph;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).ATA_Physical_Site = model.ATA_Physical_Site;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).ATA_Reg_Company = model.ATA_Reg_Company;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).ATA_PO_Courier_Check = model.ATA_PO_Courier_Check;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).ATA_Network_Sol = model.ATA_Network_Sol;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).ATA_ApprxEmpWorking = model.ATA_ApprxEmpWorking;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).ATA_CID_No = model.ATA_CID_No;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).ATA_Cmpny_Addr = model.ATA_Cmpny_Addr;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).ATA_Decoy_Call = model.ATA_Decoy_Call;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).ATA_Cmpny_Website = model.ATA_Cmpny_Website;
                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).VerifiedDate = model.VerifiedDate;
                }
                else
                {
                    throw new Exception("All * fields are mandatory!");
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void UpdateReferenceByTMQC(UpdatePQReferenceVerViewModel model, short RptQCManagerId = 0)
        {
            using (DataContext context = new DataContext())
            {
                using (DbContextTransaction transaction = context.Database.BeginTransaction())
                {
                    try
                    {
                        if (model != null)
                        {
                            int statusId = context.PQReferenceStatus.Where(p => p.ReferenceRowID == model.ReferenceCheckRowID).FirstOrDefault().ReferenceStatusRowID;

                            if (model.CheckStatus == "Rejected")
                            {
                                #region Update Check Status

                                string ActualCheckStatus = "WIP-Pending";
                                context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceCheckRowID).CheckStatus = ActualCheckStatus;
                                context.SaveChanges();

                                context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == statusId).TMCheckStatus = ActualCheckStatus;
                                context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == statusId).TMQCCheckStatus = "";
                                context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == statusId).Remarks = model.CloserViewModel.ActionHistory;
                                context.SaveChanges();

                                #endregion
                            }
                            else
                            {
                                context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceCheckRowID).CheckStatus = model.CheckStatus;
                                context.SaveChanges();

                                if (model.CheckStatus == "Insufficent")
                                {
                                    context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == statusId).TMQCCheckStatus = "Insufficent-QC";
                                    context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == statusId).InfSuffClearBy = 0;
                                    context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == statusId).InfSuffClearComments = "";
                                    context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == statusId).InfSuffClearDate = null;
                                    context.SaveChanges();

                                    context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceCheckRowID).CheckStatus = "Insufficent-QC";
                                    context.SaveChanges();

                                    context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).CaseStatus = model.CheckStatus;
                                    context.SaveChanges();
                                }

                                if (model.CheckStatus == "Funds Pending")
                                {
                                    context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == statusId).TMQCCheckStatus = "Funds Pending-QC";
                                    context.SaveChanges();

                                    context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceCheckRowID).CheckStatus = "Funds Pending-QC";
                                    context.SaveChanges();

                                    context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).CaseStatus = model.CheckStatus;
                                    context.SaveChanges();
                                }

                                if (model.ReferenceCheckVerRowID > 0 && model.CheckStatus != "Insufficent")
                                {
                                    #region Update Reference Ver Info By TMQC

                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).ReferenceCheckRowID = model.ReferenceCheckRowID;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Cmplnts_Result_Dsplnry_Sub = model.RV_Cmplnts_Result_Dsplnry_Sub;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Reliable_Honest_Person = model.RV_Reliable_Honest_Person;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Cand_Fmly_Associat_Politics = model.RV_Cand_Fmly_Associat_Politics;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Duration = model.RV_Duration;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Relation_Sub = model.RV_Relation_Sub;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Response_Frm_Rfre = model.RV_Response_Frm_Rfre;
                                    //db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_CandidateName = model.RV_CandidateName;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_OrganisationName = model.RV_OrganisationName;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_RefereeName = model.RV_RefereeName;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_RefereeDesignation = model.RV_RefereeDesignation;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Refre_Prsnt_Emp = model.RV_Refre_Prsnt_Emp;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Refre_ConDet = model.RV_Refre_ConDet;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Refere_Desig = model.RV_Refere_Desig;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Refere_Add = model.RV_Refere_Add;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Refre_MobNo = model.RV_Refre_MobNo;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Refre_Eid = model.RV_Refre_Eid;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Prof_Relationship_Cand_Dur = model.RV_Prof_Relationship_Cand_Dur;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Know_Long = model.RV_Know_Long;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Tenure_Emp = model.RV_Tenure_Emp;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Know_Subject = model.RV_Know_Subject;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Was_Cand_Honest = model.RV_Was_Cand_Honest;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Rate_Prof_Ability = model.RV_Rate_Prof_Ability;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Commun_Jr_Sr_Colleague = model.RV_Commun_Jr_Sr_Colleague;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Relationship_With_Sub = model.RV_Relationship_With_Sub;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Overall_Jb_Perf = model.RV_Overall_Jb_Perf;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Response_Wrtn_Verb = model.RV_Response_Wrtn_Verb;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_State_Specify_Medcl_Prblm = model.RV_State_Specify_Medcl_Prblm;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Sate_Specify_Per_Prblm_Perf = model.RV_Sate_Specify_Per_Prblm_Perf;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Sub_Perf_Intgrty_Wrk = model.RV_Sub_Perf_Intgrty_Wrk;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Particular_Gud_Least_Effect = model.RV_Particular_Gud_Least_Effect;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Comment_Per_Qualities = model.RV_Comment_Per_Qualities;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Aware_Son_Daughtr_Appld_Jb = model.RV_Aware_Son_Daughtr_Appld_Jb;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Character = model.RV_Character;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Anger_Mngmt = model.RV_Anger_Mngmt;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Behaviour = model.RV_Behaviour;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Intgrty_Commitmnt = model.RV_Intgrty_Commitmnt;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Nature = model.RV_Nature;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Perf_level = model.RV_Perf_level;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_SoftSkill = model.RV_SoftSkill;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Action_Orient = model.RV_Action_Orient;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Algmnt_Skills = model.RV_Algmnt_Skills;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Anger_Mgmt_Skills = model.RV_Anger_Mgmt_Skills;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Reliability = model.RV_Reliability;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Aptitutde = model.RV_Aptitutde;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Influence_Style = model.RV_Influence_Style;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Initiative = model.RV_Initiative;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Interper_Skills = model.RV_Interper_Skills;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Comm_Skills = model.RV_Comm_Skills;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Energy = model.RV_Energy;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Performance = model.RV_Performance;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Team_Mgmt_Skills = model.RV_Team_Mgmt_Skills;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Teamng_Skills = model.RV_Teamng_Skills;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Tech_Competence = model.RV_Tech_Competence;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Time_Mgmt_Org_Skills = model.RV_Time_Mgmt_Org_Skills;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Value_System = model.RV_Value_System;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Voice_Accent = model.RV_Voice_Accent;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Plan_Fr_Higher_Studies = model.RV_Plan_Fr_Higher_Studies;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_What_kind_of_Jb = model.RV_What_kind_of_Jb;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Family_Background = model.RV_Family_Background;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_His_Behaviour = model.RV_His_Behaviour;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_His_Conduct = model.RV_His_Conduct;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_His_Perf = model.RV_His_Perf;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Recommd_Work_MNC = model.RV_Recommd_Work_MNC;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Manag_Analyz_MIS_Plng = model.RV_Manag_Analyz_MIS_Plng;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Mngng_Int_Ext_Cust_Expect = model.RV_Mngng_Int_Ext_Cust_Expect;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Additional_Cmmnts_RC = model.RV_Additional_Cmmnts_RC;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Cmplnts_Rslt_Dsplnry_Sub = model.RV_Cmplnts_Rslt_Dsplnry_Sub;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Achivement_During_Tenure = model.RV_Achivement_During_Tenure;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Other_cmmnts_RC = model.RV_Other_cmmnts_RC;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Other_Qualitative_Input = model.RV_Other_Qualitative_Input;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Qualitative_Input_Plng = model.RV_Qualitative_Input_Plng;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Cand_Record_Misreprsnt_Facts = model.RV_Cand_Record_Misreprsnt_Facts;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Area_Development = model.RV_Area_Development;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Attendance_Punct_RC = model.RV_Attendance_Punct_RC;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Cand_Attitude_Task_Hndld = model.RV_Cand_Attitude_Task_Hndld;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Comm_Skills_RC = model.RV_Comm_Skills_RC;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Contribution_Intro_Concept = model.RV_Contribution_Intro_Concept;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Dsg_Refre = model.RV_Dsg_Refre;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Dsg_RC = model.RV_Dsg_RC;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_CTC = model.RV_CTC;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Reason_Leav = model.RV_Reason_Leav;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Relationship_With_Sub_Dur = model.RV_Relationship_With_Sub_Dur;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Sub_Elgblty_Rhre = model.RV_Sub_Elgblty_Rhre;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Sub_Meet_Pos_Perf_Req = model.RV_Sub_Meet_Pos_Perf_Req;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Recall_Edu_Credential = model.RV_Recall_Edu_Credential;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Recall_Emp_Back = model.RV_Recall_Emp_Back;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Do_you_trust = model.RV_Do_you_trust;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Effect_Settng_Meetng_Goals = model.RV_Effect_Settng_Meetng_Goals;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Cand_Interpersonal_Skills = model.RV_Cand_Interpersonal_Skills;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Cand_Mngmt_Skills = model.RV_Cand_Mngmt_Skills;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Acquainted_Cand_how_long = model.RV_Acquainted_Cand_how_long;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Diligent_Twds_Work = model.RV_Diligent_Twds_Work;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_How_Discipline = model.RV_How_Discipline;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_How_Ppl_Respond = model.RV_How_Ppl_Respond;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Rate_Perf_Ability = model.RV_Rate_Perf_Ability;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Respond_Work_Pressure = model.RV_Respond_Work_Pressure;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Cand_Mtng_Target_Dline = model.RV_Cand_Mtng_Target_Dline;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_As_Team_Player = model.RV_As_Team_Player;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Atude_Twds_Work = model.RV_Atude_Twds_Work;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Was_Cand_Motivated = model.RV_Was_Cand_Motivated;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_How_Was_Behaviour = model.RV_How_Was_Behaviour;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Perform_Task_Time_Cost = model.RV_Perform_Task_Time_Cost;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Rate_Drive_Fr_Result = model.RV_Rate_Drive_Fr_Result;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Rate_Resilience_Perserve = model.RV_Rate_Resilience_Perserve;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Term_Resource_Discipline = model.RV_Term_Resource_Discipline;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Rhre_Cmpny_Again = model.RV_Rhre_Cmpny_Again;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Is_Reliable_Hon_Person = model.RV_Is_Reliable_Hon_Person;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Cand_High_Intgrty = model.RV_Cand_High_Intgrty;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Cand_Sincere_Hon = model.RV_Cand_Sincere_Hon;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Knowlge_Pos_Responsiblts = model.RV_Knowlge_Pos_Responsiblts;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Major_Carrer_Changes_Sub = model.RV_Major_Carrer_Changes_Sub;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Mngng_Mtng_Int_Cust_Expect = model.RV_Mngng_Mtng_Int_Cust_Expect;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Mgng_Complexity = model.RV_Mgng_Complexity;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Meeting_Deadlines = model.RV_Meeting_Deadlines;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Mode_Of_Exit = model.RV_Mode_Of_Exit;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Moral_Values = model.RV_Moral_Values;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Oral_Comm_Skills = model.RV_Oral_Comm_Skills;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Commnt_Ldrshp_Abilities = model.RV_Commnt_Ldrshp_Abilities;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Specify_Cand_Area_Improve = model.RV_Specify_Cand_Area_Improve;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Specify_Cand_Strength = model.RV_Specify_Cand_Strength;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Prblm_Solving_Ablts = model.RV_Prblm_Solving_Ablts;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Prof_Conduct = model.RV_Prof_Conduct;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Prof_Strength = model.RV_Prof_Strength;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Prof_Values = model.RV_Prof_Values;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Quality_Of_Work = model.RV_Quality_Of_Work;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Rating_On_Scale = model.RV_Rating_On_Scale;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Rating_Que_1 = model.RV_Rating_Que_1;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Rating_Que_2 = model.RV_Rating_Que_2;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Rating_Que_3 = model.RV_Rating_Que_3;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Rating_Que_4 = model.RV_Rating_Que_4;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Recommd_Similar_Pos = model.RV_Recommd_Similar_Pos;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Reslt_Orient = model.RV_Reslt_Orient;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Role_Responsibilities_Cand = model.RV_Role_Responsibilities_Cand;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Stress_Mgmt = model.RV_Stress_Mgmt;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Sub_Perf_Intgrty_Wrk_RC = model.RV_Sub_Perf_Intgrty_Wrk_RC;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Suff_Notice_Period_Prov = model.RV_Suff_Notice_Period_Prov;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Team_Conduct = model.RV_Team_Conduct;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Team_Ldrshp = model.RV_Team_Ldrshp;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Team_Mgmt = model.RV_Team_Mgmt;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Was_Sub_Team_Player = model.RV_Was_Sub_Team_Player;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Was_Cand_Involve_Union_Act = model.RV_Was_Cand_Involve_Union_Act;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Weakness = model.RV_Weakness;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Strength_Area_Developmnt = model.RV_Strength_Area_Developmnt;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Cand_Edu_Prof_Qualification = model.RV_Cand_Edu_Prof_Qualification;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Sub_Area_Develop_Weak = model.RV_Sub_Area_Develop_Weak;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Say_Abt_Enterpren_Skills = model.RV_Say_Abt_Enterpren_Skills;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Particiular_Gud_Least_Effect = model.RV_Particiular_Gud_Least_Effect;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Pos_Cand_Working = model.RV_Pos_Cand_Working;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_TimePeriod_Cand_Work_Under = model.RV_TimePeriod_Cand_Work_Under;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Work_Ethnics_Moral_Intgrty = model.RV_Work_Ethnics_Moral_Intgrty;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Commnt_Per_Abilities = model.RV_Commnt_Per_Abilities;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Wrtn_Comm_Skills_RC = model.RV_Wrtn_Comm_Skills_RC;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_AnyIssues = model.RV_AnyIssues;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Sub_Meet_Pos_Perf_Req_RC = model.RV_Sub_Meet_Pos_Perf_Req_RC;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Sub_Comm_Interper_Skills = model.RV_Sub_Comm_Interper_Skills;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Sub_Effect_Gettng_Rslt = model.RV_Sub_Effect_Gettng_Rslt;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Intgrty_Commitmnt_RC = model.RV_Intgrty_Commitmnt_RC;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Is_Reliable_Hon_Person_RC = model.RV_Is_Reliable_Hon_Person_RC;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Nature_Job = model.RV_Nature_Job;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Overall_Jb_Perf_RC = model.RV_Overall_Jb_Perf_RC;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Social_Conduct = model.RV_Social_Conduct;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Social_Conduct_RC = model.RV_Social_Conduct_RC;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Sub_Per_Prof_Weak = model.RV_Sub_Per_Prof_Weak;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Work_Ethics_Moral_Intgrty_Rc = model.RV_Work_Ethics_Moral_Intgrty_Rc;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Recommnd_Sub_Emp_Top_MNC = model.RV_Recommnd_Sub_Emp_Top_MNC;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Cmmnts_Sub_Strngth_Weak = model.RV_Cmmnts_Sub_Strngth_Weak;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Cmplnts_Rslt_Dsplnry_Sub_RC = model.RV_Cmplnts_Rslt_Dsplnry_Sub_RC;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Contribution_Intro_Concept_RC = model.RV_Contribution_Intro_Concept_RC;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Sub_Pos_Perf_Req_RC = model.RV_Sub_Pos_Perf_Req_RC;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Confrontation_Tackled = model.RV_Confrontation_Tackled;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Pressure_Sit_Hndld = model.RV_Pressure_Sit_Hndld;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_React_To_Change = model.RV_React_To_Change;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Enforcing_Gud_Qualt_Work_Jrs = model.RV_Enforcing_Gud_Qualt_Work_Jrs;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Sub_Team_Leader = model.RV_Sub_Team_Leader;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Any_Other_Cmmnts_Fdback = model.RV_Any_Other_Cmmnts_Fdback;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Exp_During_Financ_Dealing = model.RV_Exp_During_Financ_Dealing;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Rel_Busines_Entity = model.RV_Rel_Busines_Entity;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_Dur_Busines_Rel_Cmpny = model.RV_Dur_Busines_Rel_Cmpny;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_OtherDetails6 = model.RV_OtherDetails6;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_OtherDetails7 = model.RV_OtherDetails7;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_OtherDetails8 = model.RV_OtherDetails8;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_OtherDetails9 = model.RV_OtherDetails9;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_OtherDetails10 = model.RV_OtherDetails10;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_OtherDetails11 = model.RV_OtherDetails11;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_OtherDetails12 = model.RV_OtherDetails12;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_OtherDetails13 = model.RV_OtherDetails13;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_OtherDetails14 = model.RV_OtherDetails14;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).RV_OtherDetails15 = model.RV_OtherDetails15;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).ATA_Reverse_Directory = model.ATA_Reverse_Directory;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).ATA_Residcl_Commrcl = model.ATA_Residcl_Commrcl;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).ATA_Stock_Exchnge = model.ATA_Stock_Exchnge;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).ATA_Telphne_Directry_Srch = model.ATA_Telphne_Directry_Srch;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).ATA_Yellow_Pages = model.ATA_Yellow_Pages;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).ATA_Who_Domain = model.ATA_Who_Domain;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).ATA_GoogleSearch = model.ATA_GoogleSearch;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).ATA_InfrastructureOfCmp = model.ATA_InfrastructureOfCmp;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).ATA_Just_Dial = model.ATA_Just_Dial;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).ATA_Neighbor_Check1 = model.ATA_Neighbor_Check1;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).ATA_NASCOM_Empanelment = model.ATA_NASCOM_Empanelment;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).ATA_Photograph = model.ATA_Photograph;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).ATA_Physical_Site = model.ATA_Physical_Site;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).ATA_Reg_Company = model.ATA_Reg_Company;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).ATA_PO_Courier_Check = model.ATA_PO_Courier_Check;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).ATA_Network_Sol = model.ATA_Network_Sol;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).ATA_ApprxEmpWorking = model.ATA_ApprxEmpWorking;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).ATA_CID_No = model.ATA_CID_No;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).ATA_Cmpny_Addr = model.ATA_Cmpny_Addr;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).ATA_Decoy_Call = model.ATA_Decoy_Call;
                                    db.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).ATA_Cmpny_Website = model.ATA_Cmpny_Website;
                                    #endregion

                                    context.SaveChanges();

                                    #region Save Disposition/Severity
                                    if (model.CloserViewModel.DispositionID != null)
                                    {
                                        #region Disposition
                                        string[] DispositionIds = model.CloserViewModel.DispositionID.Split(',');
                                        byte[] SeverityRows = new byte[DispositionIds.Length];
                                        List<PQReferenceVerDisposition> VerDispositionsList = new List<PQReferenceVerDisposition>();
                                        var FindVerDisposition = context.PQReferenceVerDispositions.Where(a => a.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).ToList();

                                        if (FindVerDisposition != null && FindVerDisposition.Count > 0)
                                        {
                                            context.PQReferenceVerDispositions.RemoveRange(FindVerDisposition);
                                            context.SaveChanges();
                                        }
                                        foreach (var item in DispositionIds)
                                        {
                                            PQReferenceVerDisposition VerEntity = new PQReferenceVerDisposition
                                            {
                                                ReferenceCheckVerRowID = model.ReferenceCheckVerRowID,
                                                ClientDispositionRowId = Convert.ToInt32(item)
                                            };
                                            VerDispositionsList.Add(VerEntity);
                                        }
                                        context.PQReferenceVerDispositions.AddRange(VerDispositionsList);
                                        context.SaveChanges();
                                        #endregion
                                    }

                                    //Update in ReferenceVer Table            
                                    context.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).ReportComments = model.CloserViewModel.Remarks;
                                    context.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).Severity = model.CloserViewModel.Severity;
                                    context.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).ColorName = model.CloserViewModel.ColorName;
                                    context.PQReferenceVers.Single(c => c.ReferenceCheckVerRowID == model.ReferenceCheckVerRowID).CheckStatus = model.CheckStatus;
                                    context.SaveChanges();
                                    #endregion

                                    #region ***** Completed-TM & Rejected-RWQC*****

                                    if (model.CheckStatus == "Completed-TM" || model.CheckStatus == "Rejected-RWQC")
                                    {
                                        string allocateMgr = "Allocated-Mgr";
                                        if (model.CloserViewModel.ColorName.ToLower() == "green")
                                        {
                                            context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == statusId).TMQCCheckStatus = "Completed-QC";
                                            context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == statusId).RWQCMgrCheckStatus = allocateMgr;
                                            context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == statusId).RWQCManagerBy = RptQCManagerId;
                                            context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == statusId).RWQCManagerDate = DateTime.Now;
                                            context.SaveChanges();

                                            short TemaMember = context.PQReferences.Find(model.ReferenceCheckRowID).PQPersonal.RWQCManagerTo;
                                            if (TemaMember == 0)
                                            {
                                                context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).RWQCMgrCaseStatus = allocateMgr;
                                                context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).RWQCManagerTo = RptQCManagerId;
                                                context.PQPersonals.Single(c => c.PersonalRowID == model.PersonalRowID).RWQCManagerDate = DateTime.Now;
                                                context.SaveChanges();
                                            }

                                            context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceCheckRowID).CheckStatus = "Allocated-RWQC";
                                            context.SaveChanges();
                                        }
                                        else
                                        {
                                            allocateMgr = "RW-Approval-TL";
                                            context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == statusId).TMQCCheckStatus = "Completed-QC";
                                            context.PQReferenceStatus.Single(c => c.ReferenceStatusRowID == statusId).TLCheckStatus = allocateMgr;
                                            context.SaveChanges();

                                            context.PQReferences.Single(c => c.ReferenceRowID == model.ReferenceCheckRowID).CheckStatus = allocateMgr;
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

        public UpdatePQReferenceVerViewModel GetReferenceInfoForTMQC(int ReferenceRowId = 0,int CheckStatusId=0)
        {
            try
            {
                UpdatePQReferenceVerViewModel model = new UpdatePQReferenceVerViewModel();
                var ReferenceVer = db.PQReferenceVers.Where(a => a.ReferenceCheckRowID == ReferenceRowId).FirstOrDefault();
                int ReferenceVerRowId = 0;
                if (ReferenceVer != null && ReferenceVer.ReferenceCheckVerRowID > 0)
                {
                    ReferenceVerRowId = ReferenceVer.ReferenceCheckVerRowID;
                }

                if (ReferenceVerRowId > 0)
                {
                    #region Reference
                    model = db.PQReferenceVers.Where(a => a.ReferenceCheckVerRowID == ReferenceVerRowId).Select(a => new UpdatePQReferenceVerViewModel
                    {
                        ReferenceCheckRowID = a.ReferenceCheckRowID,
                        ReferenceCheckVerRowID = a.ReferenceCheckVerRowID,
                        ClientRowID = a.PQReferenceCheck.ClientRowID,
                        PersonalRowID = a.PQReferenceCheck.PersonalRowID,
                        CheckFamilyRowID = a.PQReferenceCheck.CheckFamilyRowID,
                        SubCheckRowID = a.PQReferenceCheck.SubCheckRowID,
                        RV_Cmplnts_Result_Dsplnry_Sub = a.RV_Cmplnts_Result_Dsplnry_Sub,
                        RV_Reliable_Honest_Person = a.RV_Reliable_Honest_Person,
                        RV_Cand_Fmly_Associat_Politics = a.RV_Cand_Fmly_Associat_Politics,
                        RV_Duration = a.RV_Duration,
                        RV_Relation_Sub = a.RV_Relation_Sub,
                        RV_Response_Frm_Rfre = a.RV_Response_Frm_Rfre,
                        // RV_CandidateName = a.RV_CandidateName,


                        RV_CandidateName = db.PQPersonals.Where(T => T.PersonalRowID == a.PQReferenceCheck.PersonalRowID).Select(x => x.Name + " " + x.MiddleName + x.LastName).FirstOrDefault().ToString(),

                        RV_OrganisationName = a.RV_OrganisationName,
                        RV_RefereeName = a.RV_RefereeName,
                        RV_RefereeDesignation = a.RV_RefereeDesignation,
                        RV_Refre_Prsnt_Emp = a.RV_Refre_Prsnt_Emp,
                        RV_Refre_ConDet = a.RV_Refre_ConDet,
                        RV_Refere_Desig = a.RV_Refere_Desig,
                        RV_Refere_Add = a.RV_Refere_Add,
                        RV_Refre_MobNo = a.RV_Refre_MobNo,
                        RV_Refre_Eid = a.RV_Refre_Eid,
                        RV_Prof_Relationship_Cand_Dur = a.RV_Prof_Relationship_Cand_Dur,
                        RV_Know_Long = a.RV_Know_Long,
                        RV_Tenure_Emp = a.RV_Tenure_Emp,
                        RV_Know_Subject = a.RV_Know_Subject,
                        RV_Was_Cand_Honest = a.RV_Was_Cand_Honest,
                        RV_Rate_Prof_Ability = a.RV_Rate_Prof_Ability,
                        RV_Commun_Jr_Sr_Colleague = a.RV_Commun_Jr_Sr_Colleague,
                        RV_Relationship_With_Sub = a.RV_Relationship_With_Sub,
                        RV_Overall_Jb_Perf = a.RV_Overall_Jb_Perf,
                        RV_Response_Wrtn_Verb = a.RV_Response_Wrtn_Verb,
                        RV_State_Specify_Medcl_Prblm = a.RV_State_Specify_Medcl_Prblm,
                        RV_Sate_Specify_Per_Prblm_Perf = a.RV_Sate_Specify_Per_Prblm_Perf,
                        RV_Sub_Perf_Intgrty_Wrk = a.RV_Sub_Perf_Intgrty_Wrk,
                        RV_Particular_Gud_Least_Effect = a.RV_Particular_Gud_Least_Effect,
                        RV_Comment_Per_Qualities = a.RV_Comment_Per_Qualities,
                        RV_Aware_Son_Daughtr_Appld_Jb = a.RV_Aware_Son_Daughtr_Appld_Jb,
                        RV_Character = a.RV_Character,
                        RV_Anger_Mngmt = a.RV_Anger_Mngmt,
                        RV_Behaviour = a.RV_Behaviour,
                        RV_Intgrty_Commitmnt = a.RV_Intgrty_Commitmnt,
                        RV_Nature = a.RV_Nature,
                        RV_Perf_level = a.RV_Perf_level,
                        RV_SoftSkill = a.RV_SoftSkill,
                        RV_Action_Orient = a.RV_Action_Orient,
                        RV_Algmnt_Skills = a.RV_Algmnt_Skills,
                        RV_Anger_Mgmt_Skills = a.RV_Anger_Mgmt_Skills,
                        RV_Reliability = a.RV_Reliability,
                        RV_Aptitutde = a.RV_Aptitutde,
                        RV_Influence_Style = a.RV_Influence_Style,
                        RV_Initiative = a.RV_Initiative,
                        RV_Interper_Skills = a.RV_Interper_Skills,
                        RV_Comm_Skills = a.RV_Comm_Skills,
                        RV_Energy = a.RV_Energy,
                        RV_Performance = a.RV_Performance,
                        RV_Team_Mgmt_Skills = a.RV_Team_Mgmt_Skills,
                        RV_Teamng_Skills = a.RV_Teamng_Skills,
                        RV_Tech_Competence = a.RV_Tech_Competence,
                        RV_Time_Mgmt_Org_Skills = a.RV_Time_Mgmt_Org_Skills,
                        RV_Value_System = a.RV_Value_System,
                        RV_Voice_Accent = a.RV_Voice_Accent,
                        RV_Plan_Fr_Higher_Studies = a.RV_Plan_Fr_Higher_Studies,
                        RV_What_kind_of_Jb = a.RV_What_kind_of_Jb,
                        RV_Family_Background = a.RV_Family_Background,
                        RV_His_Behaviour = a.RV_His_Behaviour,
                        RV_His_Conduct = a.RV_His_Conduct,
                        RV_His_Perf = a.RV_His_Perf,
                        RV_Recommd_Work_MNC = a.RV_Recommd_Work_MNC,
                        RV_Manag_Analyz_MIS_Plng = a.RV_Manag_Analyz_MIS_Plng,
                        RV_Mngng_Int_Ext_Cust_Expect = a.RV_Mngng_Int_Ext_Cust_Expect,
                        RV_Additional_Cmmnts_RC = a.RV_Additional_Cmmnts_RC,
                        RV_Cmplnts_Rslt_Dsplnry_Sub = a.RV_Cmplnts_Rslt_Dsplnry_Sub,
                        RV_Achivement_During_Tenure = a.RV_Achivement_During_Tenure,
                        RV_Other_cmmnts_RC = a.RV_Other_cmmnts_RC,
                        RV_Other_Qualitative_Input = a.RV_Other_Qualitative_Input,
                        RV_Qualitative_Input_Plng = a.RV_Qualitative_Input_Plng,
                        RV_Cand_Record_Misreprsnt_Facts = a.RV_Cand_Record_Misreprsnt_Facts,
                        RV_Area_Development = a.RV_Area_Development,
                        RV_Attendance_Punct_RC = a.RV_Attendance_Punct_RC,
                        RV_Cand_Attitude_Task_Hndld = a.RV_Cand_Attitude_Task_Hndld,
                        RV_Comm_Skills_RC = a.RV_Comm_Skills_RC,
                        RV_Contribution_Intro_Concept = a.RV_Contribution_Intro_Concept,
                        RV_Dsg_Refre = a.RV_Dsg_Refre,
                        RV_Dsg_RC = a.RV_Dsg_RC,
                        RV_CTC = a.RV_CTC,
                        RV_Reason_Leav = a.RV_Reason_Leav,
                        RV_Relationship_With_Sub_Dur = a.RV_Relationship_With_Sub_Dur,
                        RV_Sub_Elgblty_Rhre = a.RV_Sub_Elgblty_Rhre,
                        RV_Sub_Meet_Pos_Perf_Req = a.RV_Sub_Meet_Pos_Perf_Req,
                        RV_Recall_Edu_Credential = a.RV_Recall_Edu_Credential,
                        RV_Recall_Emp_Back = a.RV_Recall_Emp_Back,
                        RV_Do_you_trust = a.RV_Do_you_trust,
                        RV_Effect_Settng_Meetng_Goals = a.RV_Effect_Settng_Meetng_Goals,
                        RV_Cand_Interpersonal_Skills = a.RV_Cand_Interpersonal_Skills,
                        RV_Cand_Mngmt_Skills = a.RV_Cand_Mngmt_Skills,
                        RV_Acquainted_Cand_how_long = a.RV_Acquainted_Cand_how_long,
                        RV_Diligent_Twds_Work = a.RV_Diligent_Twds_Work,
                        RV_How_Discipline = a.RV_How_Discipline,
                        RV_How_Ppl_Respond = a.RV_How_Ppl_Respond,
                        RV_Rate_Perf_Ability = a.RV_Rate_Perf_Ability,
                        RV_Respond_Work_Pressure = a.RV_Respond_Work_Pressure,
                        RV_Cand_Mtng_Target_Dline = a.RV_Cand_Mtng_Target_Dline,
                        RV_As_Team_Player = a.RV_As_Team_Player,
                        RV_Atude_Twds_Work = a.RV_Atude_Twds_Work,
                        RV_Was_Cand_Motivated = a.RV_Was_Cand_Motivated,
                        RV_How_Was_Behaviour = a.RV_How_Was_Behaviour,
                        RV_Perform_Task_Time_Cost = a.RV_Perform_Task_Time_Cost,
                        RV_Rate_Drive_Fr_Result = a.RV_Rate_Drive_Fr_Result,
                        RV_Rate_Resilience_Perserve = a.RV_Rate_Resilience_Perserve,
                        RV_Term_Resource_Discipline = a.RV_Term_Resource_Discipline,
                        RV_Rhre_Cmpny_Again = a.RV_Rhre_Cmpny_Again,
                        RV_Is_Reliable_Hon_Person = a.RV_Is_Reliable_Hon_Person,
                        RV_Cand_High_Intgrty = a.RV_Cand_High_Intgrty,
                        RV_Cand_Sincere_Hon = a.RV_Cand_Sincere_Hon,
                        RV_Knowlge_Pos_Responsiblts = a.RV_Knowlge_Pos_Responsiblts,
                        RV_Major_Carrer_Changes_Sub = a.RV_Major_Carrer_Changes_Sub,
                        RV_Mngng_Mtng_Int_Cust_Expect = a.RV_Mngng_Mtng_Int_Cust_Expect,
                        RV_Mgng_Complexity = a.RV_Mgng_Complexity,
                        RV_Meeting_Deadlines = a.RV_Meeting_Deadlines,
                        RV_Mode_Of_Exit = a.RV_Mode_Of_Exit,
                        RV_Moral_Values = a.RV_Moral_Values,
                        RV_Oral_Comm_Skills = a.RV_Oral_Comm_Skills,
                        RV_Commnt_Ldrshp_Abilities = a.RV_Commnt_Ldrshp_Abilities,
                        RV_Specify_Cand_Area_Improve = a.RV_Specify_Cand_Area_Improve,
                        RV_Specify_Cand_Strength = a.RV_Specify_Cand_Strength,
                        RV_Prblm_Solving_Ablts = a.RV_Prblm_Solving_Ablts,
                        RV_Prof_Conduct = a.RV_Prof_Conduct,
                        RV_Prof_Strength = a.RV_Prof_Strength,
                        RV_Prof_Values = a.RV_Prof_Values,
                        RV_Quality_Of_Work = a.RV_Quality_Of_Work,
                        RV_Rating_On_Scale = a.RV_Rating_On_Scale,
                        RV_Rating_Que_1 = a.RV_Rating_Que_1,
                        RV_Rating_Que_2 = a.RV_Rating_Que_2,
                        RV_Rating_Que_3 = a.RV_Rating_Que_3,
                        RV_Rating_Que_4 = a.RV_Rating_Que_4,
                        RV_Recommd_Similar_Pos = a.RV_Recommd_Similar_Pos,
                        RV_Reslt_Orient = a.RV_Reslt_Orient,
                        RV_Role_Responsibilities_Cand = a.RV_Role_Responsibilities_Cand,
                        RV_Stress_Mgmt = a.RV_Stress_Mgmt,
                        RV_Sub_Perf_Intgrty_Wrk_RC = a.RV_Sub_Perf_Intgrty_Wrk_RC,
                        RV_Suff_Notice_Period_Prov = a.RV_Suff_Notice_Period_Prov,
                        RV_Team_Conduct = a.RV_Team_Conduct,
                        RV_Team_Ldrshp = a.RV_Team_Ldrshp,
                        RV_Team_Mgmt = a.RV_Team_Mgmt,
                        RV_Was_Sub_Team_Player = a.RV_Was_Sub_Team_Player,
                        RV_Was_Cand_Involve_Union_Act = a.RV_Was_Cand_Involve_Union_Act,
                        RV_Weakness = a.RV_Weakness,
                        RV_Strength_Area_Developmnt = a.RV_Strength_Area_Developmnt,
                        RV_Cand_Edu_Prof_Qualification = a.RV_Cand_Edu_Prof_Qualification,
                        RV_Sub_Area_Develop_Weak = a.RV_Sub_Area_Develop_Weak,
                        RV_Say_Abt_Enterpren_Skills = a.RV_Say_Abt_Enterpren_Skills,
                        RV_Particiular_Gud_Least_Effect = a.RV_Particiular_Gud_Least_Effect,
                        RV_Pos_Cand_Working = a.RV_Pos_Cand_Working,
                        RV_TimePeriod_Cand_Work_Under = a.RV_TimePeriod_Cand_Work_Under,
                        RV_Work_Ethnics_Moral_Intgrty = a.RV_Work_Ethnics_Moral_Intgrty,
                        RV_Commnt_Per_Abilities = a.RV_Commnt_Per_Abilities,
                        RV_Wrtn_Comm_Skills_RC = a.RV_Wrtn_Comm_Skills_RC,
                        RV_AnyIssues = a.RV_AnyIssues,
                        RV_Sub_Meet_Pos_Perf_Req_RC = a.RV_Sub_Meet_Pos_Perf_Req_RC,
                        RV_Sub_Comm_Interper_Skills = a.RV_Sub_Comm_Interper_Skills,
                        RV_Sub_Effect_Gettng_Rslt = a.RV_Sub_Effect_Gettng_Rslt,
                        RV_Intgrty_Commitmnt_RC = a.RV_Intgrty_Commitmnt_RC,
                        RV_Is_Reliable_Hon_Person_RC = a.RV_Is_Reliable_Hon_Person_RC,
                        RV_Nature_Job = a.RV_Nature_Job,
                        RV_Overall_Jb_Perf_RC = a.RV_Overall_Jb_Perf_RC,
                        RV_Social_Conduct = a.RV_Social_Conduct,
                        RV_Social_Conduct_RC = a.RV_Social_Conduct_RC,
                        RV_Sub_Per_Prof_Weak = a.RV_Sub_Per_Prof_Weak,
                        RV_Work_Ethics_Moral_Intgrty_Rc = a.RV_Work_Ethics_Moral_Intgrty_Rc,
                        RV_Recommnd_Sub_Emp_Top_MNC = a.RV_Recommnd_Sub_Emp_Top_MNC,
                        RV_Cmmnts_Sub_Strngth_Weak = a.RV_Cmmnts_Sub_Strngth_Weak,
                        RV_Cmplnts_Rslt_Dsplnry_Sub_RC = a.RV_Cmplnts_Rslt_Dsplnry_Sub_RC,
                        RV_Contribution_Intro_Concept_RC = a.RV_Contribution_Intro_Concept_RC,
                        RV_Sub_Pos_Perf_Req_RC = a.RV_Sub_Pos_Perf_Req_RC,
                        RV_Confrontation_Tackled = a.RV_Confrontation_Tackled,
                        RV_Pressure_Sit_Hndld = a.RV_Pressure_Sit_Hndld,
                        RV_React_To_Change = a.RV_React_To_Change,
                        RV_Enforcing_Gud_Qualt_Work_Jrs = a.RV_Enforcing_Gud_Qualt_Work_Jrs,
                        RV_Sub_Team_Leader = a.RV_Sub_Team_Leader,
                        RV_Any_Other_Cmmnts_Fdback = a.RV_Any_Other_Cmmnts_Fdback,
                        RV_Exp_During_Financ_Dealing = a.RV_Exp_During_Financ_Dealing,
                        RV_Rel_Busines_Entity = a.RV_Rel_Busines_Entity,
                        RV_Dur_Busines_Rel_Cmpny = a.RV_Dur_Busines_Rel_Cmpny,
                        RV_OtherDetails6 = a.RV_OtherDetails6,
                        RV_OtherDetails7 = a.RV_OtherDetails7,
                        RV_OtherDetails8 = a.RV_OtherDetails8,
                        RV_OtherDetails9 = a.RV_OtherDetails9,
                        RV_OtherDetails10 = a.RV_OtherDetails10,
                        RV_OtherDetails11 = a.RV_OtherDetails11,
                        RV_OtherDetails12 = a.RV_OtherDetails12,
                        RV_OtherDetails13 = a.RV_OtherDetails13,
                        RV_OtherDetails14 = a.RV_OtherDetails14,
                        RV_OtherDetails15 = a.RV_OtherDetails15,
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
                        CheckStatus = a.PQReferenceCheck.CheckStatus,
                        CheckStatusId=CheckStatusId
                    }).FirstOrDefault();
                    #endregion
                }
                else
                {
                    #region Reference
                    model = db.PQReferences.Where(a => a.ReferenceRowID == ReferenceRowId).Select(a => new UpdatePQReferenceVerViewModel
                    {
                        ReferenceCheckRowID = a.ReferenceRowID,
                        ReferenceCheckVerRowID = 0,
                        ClientRowID = a.ClientRowID,
                        PersonalRowID = a.PersonalRowID,
                        CheckFamilyRowID = a.CheckFamilyRowID,
                        SubCheckRowID = a.SubCheckRowID,
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
                        RV_OtherDetails6 = a.RV_OtherDetails6,
                        RV_OtherDetails7 = a.RV_OtherDetails7,
                        RV_OtherDetails8 = a.RV_OtherDetails8,
                        RV_OtherDetails9 = a.RV_OtherDetails9,
                        RV_OtherDetails10 = a.RV_OtherDetails10,
                        ATA_CID_No = a.ATA_CID_No,
                        ATA_Cmpny_Addr = a.ATA_Cmpny_Addr,
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
    }
}
