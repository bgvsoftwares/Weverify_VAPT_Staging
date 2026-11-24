using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class PQEmploymentVer
    {
        public PQEmploymentVer()
        {
            EmploymentVerRowID = 0;
            EmploymentRowID = 0;
            EV_CandidateName                        =string.Empty;
            EV_OrganisationName                     =string.Empty;
             EV_CmpLocation                         =string.Empty;
             EV_CmpNameAddress                      =string.Empty;
             EV_EmployeeName                        =string.Empty;
             EV_EmployeeCode                        =string.Empty;
             EV_EmploymentPeriod                    =string.Empty;
             EV_EmploymentDual                      =string.Empty;
             EV_EmployedSelf                        =string.Empty;
             EV_lappingOver                         =string.Empty;
             EV_CandidateTenureWorked               =string.Empty;
             EV_EmployeeTenure                      =string.Empty;
             EV_Designation                         =string.Empty;
             EV_EmployedEarlierAlso                 =string.Empty;
             EV_nature_emplo                        =string.Empty;
             EV_typ_employ                          =string.Empty;
             EV_EmpType                             =string.Empty;
             EV_Department                          =string.Empty;
             EV_CmpCost                             =string.Empty;
             EV_last_sal                            =string.Empty;
             EV_leavingReason                       =string.Empty;
             EV_termina                             =string.Empty;
             EV_DiscontinueServiceReason            =string.Empty;
             EV_EligibilityforRehire                =string.Empty;
             EV_EligibilityOfRehire                 =string.Empty;
             EV_EligibileForRehire                  =string.Empty;
             EV_Rhre_Aplcnt_Situt_Ars_Ftre          =string.Empty;
             EV_Reported                            =string.Empty;
             EV_ReportingManagerName                =string.Empty;
             EV_Rptng_Mgr_Dsg_Eid_Tno               =string.Empty;
             EV_Rptng_Mgr_Name_Ph                   =string.Empty;
             EV_Rptng_Mgr_Name_ConDet               =string.Empty;
             EV_Sub_Rptng_Mgr_Name_Dsg_EidPh        =string.Empty;
             EV_Supervisor                          =string.Empty;
             EV_Sup_Name_Dsg                        =string.Empty;
             EV_FormalitiesExit                     =string.Empty;
             EV_FullFinalFormalities                =string.Empty;
             EV_ExitFormalitiesPending              =string.Empty;
             EV_HandledResponsibilities             =string.Empty;
             EV_ResponsibilitiesHandled             =string.Empty;
             EV_OverallJobPerformance               =string.Empty;
             EV_PerfromanceLevel                    =string.Empty;
             EV_KnowledgeProcess                    =string.Empty;
             EV_ValidationBusinessOperation          =string.Empty;
             EV_DutiesNature                        =string.Empty;
             EV_Prof_Ablts_Aplcnt                   =string.Empty;
             EV_Dsplnry_Perf_Iss_Rptd               =string.Empty;
             EV_DisciplinaryIssue                   =string.Empty;
             EV_Cmplnts_Rslt_Dsplnry_Sub            =string.Empty;
             EV_Emp_Invol_Fraud_Actvty              =string.Empty;
             EV_PerformanceIssues                   =string.Empty;
             EV_TeamConduct                         =string.Empty;
             EV_TeamPlayerNot                       =string.Empty;
             EV_TechnologicalKnowledge              =string.Empty;
             EV_Fback_Accnt_Cduct_Jb                =string.Empty;
             EV_DisciplinedHowHe                    =string.Empty;
             EV_LongKnown                           =string.Empty;
             EV_LongWork                            =string.Empty;
             EV_LeavesTook                          =string.Empty;
             EV_People_Rptd                         =string.Empty;
             EV_Conduct                             =string.Empty;
             EV_Performance                         =string.Empty;
             EV_Perf_Rate                           =string.Empty;
             EV_Mtng_Deadline                       =string.Empty;
             EV_Comm_Skills                         =string.Empty;
             EV_Wrtn_Comm_Skills                    =string.Empty;
             EV_Verb_Comm_Skills                    =string.Empty;
             EV_Procs_Knowlg                        =string.Empty;
             EV_Attdnce_Punct                       =string.Empty;
             EV_Ldrshp_Qualty                       =string.Empty;
             EV_Mngng_Mtng_Int_Cust_Expect          =string.Empty;
             EV_OtherIssues                         =string.Empty;
             EV_AdherenceTAT                        =string.Empty;
             EV_CompletesTATWork                    =string.Empty;
             EV_Atude_Hon_Per_Aplcnt                =string.Empty;
             EV_WorkAttitude                        =string.Empty;
             EV_RatingTeaming                       =string.Empty;
             EV_Rate_Tech                           =string.Empty;
             EV_AttendancePunctuality               =string.Empty;
             EV_Cmnts_Abt_Sub                       =string.Empty;
             EV_SubjectComments                     =string.Empty;
             EV_AdditionalComments                  =string.Empty;
             EV_OtherComments                       =string.Empty;
             EV_OtherRemarks                        =string.Empty;
             EV_AttachedDocumentGenuine             =string.Empty;
             EV_ResponseMode                        =string.Empty;           
             EV_RefereeResponse                     =string.Empty;
             EV_RespondentName                      =string.Empty;
             EV_RefereeDetails                      =string.Empty;
             EV_RespondentDesignation               =string.Empty;
             EV_RefereeNameDetails                  =string.Empty;
             EV_DetailsReferee                      =string.Empty;
             EV_ContactDetail                       =string.Empty;
             EV_Verfson_Poss_Phy_Visit              =string.Empty;
             EV_Details_Verfson_EidPh_Phyvst        =string.Empty;
             EV_Nme_Add_Cmpny_Exist_Now             =string.Empty;
             EV_Name_Dsg_Verfyng_Auth               =string.Empty;
             EV_Verified                            =string.Empty;
             EV_Verified_by                         =string.Empty;
             EV_Verified_by_dsg_ConNo               =string.Empty;
            EV_OtherDetails6                        =string.Empty;
            EV_OtherDetails7                        =string.Empty;
            EV_OtherDetails8                        =string.Empty;
            EV_OtherDetails9                        =string.Empty;
            EV_OtherDetails10                       =string.Empty;
            EV_OtherDetails11                       =string.Empty;
            EV_OtherDetails12                       =string.Empty;
            EV_OtherDetails13                       =string.Empty;
            EV_OtherDetails14                       =string.Empty;
            EV_OtherDetails15                       =string.Empty;
            ATA_Reverse_Directory                   =string.Empty;
            ATA_Residcl_Commrcl                     =string.Empty;
            ATA_Stock_Exchnge                       =string.Empty;
            ATA_Telphne_Directry_Srch               =string.Empty;
            ATA_Yellow_Pages                        =string.Empty;
            ATA_Who_Domain                          =string.Empty;
            ATA_GoogleSearch                        =string.Empty;
            ATA_InfrastructureOfCmp                 =string.Empty;
            ATA_Just_Dial                           =string.Empty;
            ATA_Neighbor_Check1                     =string.Empty;
            ATA_NASCOM_Empanelment                  =string.Empty;
            ATA_Photograph                          =string.Empty;
            ATA_Physical_Site                       =string.Empty;
            ATA_Reg_Company                         =string.Empty;
            ATA_PO_Courier_Check                    =string.Empty;
            ATA_Network_Sol                         =string.Empty;
            ATA_ApprxEmpWorking                     =string.Empty;
            ATA_CID_No                              =string.Empty;
            ATA_Cmpny_Addr                          =string.Empty;
            ATA_Decoy_Call                          =string.Empty;
            ATA_Cmpny_Website                       =string.Empty;

            TypeRevert = string.Empty;
            CheckStatus = string.Empty;
            Severity = string.Empty;
            ColorName = string.Empty;
            ReportComments = string.Empty;
            Status = 1;

            VerifiedBy = 0;
            VerifierDesignation = string.Empty;
            VerifierContactNo = string.Empty;
            VerifierMobileNo = string.Empty;
            VerifierEmailId = string.Empty;

        }

        public int EmploymentVerRowID  { get; set; }

        public int EmploymentRowID      { get; set; }
        public virtual PQEmployment PQEmployment { get; set; }

        #region Employment Verification Antecedents

        public string EV_CandidateName { get; set; }// Text Field
        public string EV_OrganisationName { get; set; }//Drop Down -Auto populated from employer master 
        public string EV_CmpLocation { get; set; }// drop down -Location filter from employer master 
        public string EV_CmpNameAddress { get; set; }// Text Field -Auto populated from employer master 
        public string EV_EmployeeName { get; set; }// Text Field
        public string EV_EmployeeCode { get; set; }//Text Field
        public string EV_EmploymentPeriod { get; set; } // text filed To & from calender
        public string EV_EmploymentDual { get; set; } // drop down -yes/No
        public string EV_EmployedSelf { get; set; }// drop down -yes/No
        public string EV_lappingOver { get; set; }// drop down -yes/No
        public string EV_CandidateTenureWorked { get; set; } // text filed To & from calender
        public string EV_EmployeeTenure { get; set; } // text filed To & from calender
        public string EV_Designation { get; set; }// text filed 
        public string EV_EmployedEarlierAlso { get; set; } // drop down -yes/No
        public string EV_nature_emplo { get; set; } // drop down -Permanent / Temporary
        public string EV_typ_employ { get; set; }// drop down -Full time / part time
        public string EV_EmpType { get; set; }// drop down -Permanent / Temporary / contractual
        public string EV_Department { get; set; }// text filed 
        public string EV_CmpCost { get; set; }// text filed 
        public string EV_last_sal { get; set; }// text filed 
        public string EV_leavingReason { get; set; }// text Area 
        public string EV_termina { get; set; }// text Area 
        public string EV_DiscontinueServiceReason { get; set; }// text Area 
        public string EV_EligibilityforRehire { get; set; }// text Area 
        public string EV_EligibilityOfRehire { get; set; }// text Area 
        public string EV_EligibileForRehire { get; set; }// text Area 
        public string EV_Rhre_Aplcnt_Situt_Ars_Ftre { get; set; }// text Area 
        public string EV_Reported { get; set; }// text filed 
        public string EV_ReportingManagerName { get; set; }// 
        public string EV_Rptng_Mgr_Dsg_Eid_Tno { get; set; }// Text Area
        public string EV_Rptng_Mgr_Name_Ph { get; set; }// text filed
        public string EV_Rptng_Mgr_Name_ConDet { get; set; }// text filed
        public string EV_Sub_Rptng_Mgr_Name_Dsg_EidPh { get; set; }// text filed
        public string EV_Supervisor { get; set; }// Text filed
        public string EV_Sup_Name_Dsg { get; set; }// text filed
        public string EV_FormalitiesExit { get; set; }//Text Area
        public string EV_FullFinalFormalities { get; set; }//Text Area
        public string EV_ExitFormalitiesPending { get; set; }//Text Area
        public string EV_HandledResponsibilities { get; set; }//Text Area
        public string EV_ResponsibilitiesHandled { get; set; }//Text Area
        public string EV_OverallJobPerformance { get; set; }//Text Area
        public string EV_PerfromanceLevel { get; set; }//Text Area
        public string EV_KnowledgeProcess { get; set; }//Text Area
        public string EV_ValidationBusinessOperation { get; set; }//Text Area
        public string EV_DutiesNature { get; set; }//Text Area
        public string EV_Prof_Ablts_Aplcnt { get; set; }//Text Area
        public string EV_Dsplnry_Perf_Iss_Rptd { get; set; }//Text Area
        public string EV_DisciplinaryIssue { get; set; }//Text Area
        public string EV_Cmplnts_Rslt_Dsplnry_Sub { get; set; }//Text Area
        public string EV_Emp_Invol_Fraud_Actvty { get; set; }//Text Area
        public string EV_PerformanceIssues { get; set; }//Text Area
        public string EV_TeamConduct { get; set; }//Text Area
        public string EV_TeamPlayerNot { get; set; }//Text Area
        public string EV_TechnologicalKnowledge { get; set; }//Text Area
        public string EV_Fback_Accnt_Cduct_Jb { get; set; }//Text Area
        public string EV_DisciplinedHowHe { get; set; }//Text Area
        public string EV_LongKnown { get; set; }//Text Area
        public string EV_LongWork { get; set; }//Text Area
        public string EV_LeavesTook { get; set; }//Text Area
        public string EV_People_Rptd { get; set; }//Text Area
        public string EV_Conduct { get; set; }//Text Area
        public string EV_Performance { get; set; }//Text Area
        public string EV_Perf_Rate { get; set; }// drop down -1,2,3,4,5,6,7,8,9,10
        public string EV_Mtng_Deadline { get; set; }// drop down -1,2,3,4,5,6,7,8,9,10
        public string EV_Comm_Skills { get; set; }// drop down -1,2,3,4,5,6,7,8,9,10
        public string EV_Wrtn_Comm_Skills { get; set; }// drop down -1,2,3,4,5,6,7,8,9,10
        public string EV_Verb_Comm_Skills { get; set; }// drop down -1,2,3,4,5,6,7,8,9,10
        public string EV_Procs_Knowlg { get; set; }// drop down -1,2,3,4,5,6,7,8,9,10
        public string EV_Attdnce_Punct { get; set; }// drop down -1,2,3,4,5,6,7,8,9,10
        public string EV_Ldrshp_Qualty { get; set; }// drop down -1,2,3,4,5,6,7,8,9,10
        public string EV_Mngng_Mtng_Int_Cust_Expect { get; set; }// drop down -1,2,3,4,5,6,7,8,9,10
        public string EV_OtherIssues { get; set; }// text  Area
        public string EV_AdherenceTAT { get; set; }// text  Area
        public string EV_CompletesTATWork { get; set; }// text  Area
        public string EV_Atude_Hon_Per_Aplcnt { get; set; }// text  Area
        public string EV_WorkAttitude { get; set; }// text  Area
        public string EV_RatingTeaming { get; set; }// drop down -1,2,3,4,5,6,7,8,9,10
        public string EV_Rate_Tech { get; set; }// drop down -1,2,3,4,5,6,7,8,9,10
        public string EV_AttendancePunctuality { get; set; }// text filed
        public string EV_Cmnts_Abt_Sub { get; set; }// text Area
        public string EV_SubjectComments { get; set; }// text Area
        public string EV_AdditionalComments { get; set; }// text Area
        public string EV_OtherComments { get; set; }// text Area
        public string EV_OtherRemarks { get; set; }// text Area
        public string EV_AttachedDocumentGenuine { get; set; }// drop down -Yes / No
        public string EV_ResponseMode { get; set; }// Drop Down 
        public DateTime? EV_OnlineVerificationCondctedOn { get; set; }// Date
        public string EV_RefereeResponse { get; set; }// Drop Down -Written / Verbal / Online / Others
        public string EV_RespondentName { get; set; }// text Filed
        public string EV_RefereeDetails { get; set; }// text Filed
        public string EV_RespondentDesignation { get; set; }// text Filed
        public string EV_RefereeNameDetails { get; set; }//Text Area
        public string EV_DetailsReferee { get; set; }//Text Area
        public string EV_ContactDetail { get; set; }//Text Area
        public DateTime? EV_DateTime { get; set; }// date 
        public string EV_Verfson_Poss_Phy_Visit { get; set; }// drop down - Yes /No
        public string EV_Details_Verfson_EidPh_Phyvst { get; set; }// text area 
        public string EV_Nme_Add_Cmpny_Exist_Now { get; set; }// Text Area 
        public string EV_Name_Dsg_Verfyng_Auth { get; set; }// Text Area
        public string EV_Verified { get; set; }// Text Area
        public string EV_Verified_by { get; set; }// Text Area
        public string EV_Verified_by_dsg_ConNo { get; set; }// Text Area
        public DateTime? EV_Date_Time_Contacted { get; set; }// date
        public string EV_OtherDetails6 { get; set; }// text Area
        public string EV_OtherDetails7 { get; set; }// text Area
        public string EV_OtherDetails8 { get; set; }// text Area
        public string EV_OtherDetails9 { get; set; }// text Area
        public string EV_OtherDetails10 { get; set; }// text Area
        public string EV_OtherDetails11 { get; set; }// text Area
        public string EV_OtherDetails12 { get; set; }// text Area
        public string EV_OtherDetails13 { get; set; }// text Area
        public string EV_OtherDetails14 { get; set; }// text Area
        public string EV_OtherDetails15 { get; set; }// text Area
        public string ATA_Reverse_Directory { get; set; }// text Area
        public string ATA_Residcl_Commrcl { get; set; }// text Area
        public string ATA_Stock_Exchnge { get; set; }// text Area
        public string ATA_Telphne_Directry_Srch { get; set; }// text Area
        public string ATA_Yellow_Pages { get; set; }// text Area
        public string ATA_Who_Domain { get; set; }// text Area
        public string ATA_GoogleSearch { get; set; }// text Area
        public string ATA_InfrastructureOfCmp { get; set; }// text Area
        public string ATA_Just_Dial { get; set; }// text Area
        public string ATA_Neighbor_Check1 { get; set; }// text Area
        public string ATA_NASCOM_Empanelment { get; set; }// text Area
        public string ATA_Photograph { get; set; }// text Area
        public string ATA_Physical_Site { get; set; }// text Area
        public string ATA_Reg_Company { get; set; }// text Area
        public string ATA_PO_Courier_Check { get; set; }// text Area
        public string ATA_Network_Sol { get; set; }// text Area
        public string ATA_ApprxEmpWorking { get; set; }// text Area
        public string ATA_CID_No { get; set; }// text filed
        public string ATA_Cmpny_Addr { get; set; }// text filed
        public string ATA_Decoy_Call { get; set; }// text Area
        public string ATA_Cmpny_Website { get; set; }// text Area          

        #endregion

        public string TypeRevert { get; set; }
        public string CheckStatus { get; set; }
        public string Severity { get; set; }
        public string ColorName { get; set; }
        public string ReportComments { get; set; }
        public byte Status { get; set; }

        public short VerifiedBy { get; set; }
        public string VerifierDesignation { get; set; }
        public string VerifierContactNo { get; set; }
        public string VerifierMobileNo { get; set; }
        public string VerifierEmailId { get; set; }
        public DateTime? VerifiedDate { get; set; }
    }
}
