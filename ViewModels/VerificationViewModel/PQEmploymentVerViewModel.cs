using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ViewModels.VerificationViewModel
{
    public class PQEmploymentVerViewModel
    {

        [ScaffoldColumn(false)]
        public int EmploymentVerRowID { get; set; }

        [ScaffoldColumn(false)]
        public int EmploymentRowID { get; set; }

        [ScaffoldColumn(false)]
        public short ClientRowID { get; set; }
        public string ClientName { get; set; }

        [ScaffoldColumn(false)]
        public short SubCheckRowID { get; set; }
        public int CheckStatusID { get; set; }

        [ScaffoldColumn(false)]
        public int PersonalRowID { get; set; }
        public string PersonalName { get; set; }

        [ScaffoldColumn(false)]
        public string UniqueComponentID { get; set; }
        public string CandidateName { get; set; }
        public string CompanyReferenceNo { get; set; }

        [ScaffoldColumn(false)]
        public string SubReferenceID { get; set; }
        public string MobileNo { get; set; }
        public string AllocatedTMBy { get; set; }
        [AllowHtml]
        public string SpecialInstructions { get; set; }

        [ScaffoldColumn(false)]
        public short CheckFamilyRowID { get; set; }

        public bool Aging { get; set; }

        #region Employment Verification Antecedents
        [MaxLength(100)]
        public string EV_CandidateName { get; set; }// Text Field

        [MaxLength(500)]
        public string EV_OrganisationName { get; set; }//Drop Down -Auto populated from employer master 

        public string EV_OrganisationNameId { get; set; }

        [MaxLength(100)]
        public string EV_CmpLocation { get; set; }// drop down -Location filter from employer master 

        [MaxLength(700)]
        public string EV_CmpNameAddress { get; set; }// Text Field -Auto populated from employer master 

        [MaxLength(100)]
        public string EV_EmployeeName { get; set; }// Text Field

        [MaxLength(100)]
        public string EV_EmployeeCode { get; set; }//Text Field

        [MaxLength(100)]
        public string EV_EmploymentPeriod { get; set; } // text filed To & from calender

        [MaxLength(100)]
        public string EV_EmploymentDual { get; set; } // drop down -yes/No

        [MaxLength(100)]
        public string EV_EmployedSelf { get; set; }// drop down -yes/No

        [MaxLength(100)]
        public string EV_lappingOver { get; set; }// drop down -yes/No

        [MaxLength(100)]
        public string EV_CandidateTenureWorked { get; set; } // text filed To & from calender

        [MaxLength(100)]
        public string EV_EmployeeTenure { get; set; } // text filed To & from calender

        [MaxLength(100)]
        public string EV_Designation { get; set; }// text filed 

        [MaxLength(100)]
        public string EV_EmployedEarlierAlso { get; set; } // drop down -yes/No

        [MaxLength(100)]
        public string EV_nature_emplo { get; set; } // drop down -Permanent / Temporary

        [MaxLength(100)]
        public string EV_typ_employ { get; set; }// drop down -Full time / Part time

        [MaxLength(100)]
        public string EV_EmpType { get; set; }// drop down -Permanent / Temporary / contractual

        [MaxLength(100)]
        public string EV_Department { get; set; }// text filed 

        [MaxLength(100)]
        public string EV_CmpCost { get; set; }// text filed 

        [MaxLength(100)]
        public string EV_last_sal { get; set; }// text filed 

        [MaxLength(200)]
        public string EV_leavingReason { get; set; }// text Area 

        [MaxLength(200)]
        public string EV_termina { get; set; }// text Area 

        [MaxLength(200)]
        public string EV_DiscontinueServiceReason { get; set; }// text Area 

        [MaxLength(200)]
        public string EV_EligibilityforRehire { get; set; }// text Area 

        [MaxLength(200)]
        public string EV_EligibilityOfRehire { get; set; }// text Area 

        [MaxLength(200)]
        public string EV_EligibileForRehire { get; set; }// text Area 


        [MaxLength(200)]
        public string EV_Rhre_Aplcnt_Situt_Ars_Ftre { get; set; }// text Area 

        [MaxLength(100)]
        public string EV_Reported { get; set; }// text filed 

        [MaxLength(100)]
        public string EV_ReportingManagerName { get; set; }// text filed 

        [MaxLength(100)]
        public string EV_Rptng_Mgr_Dsg_Eid_Tno { get; set; }// Text Area


        [MaxLength(100)]
        public string EV_Rptng_Mgr_Name_Ph { get; set; }// text filed

        [MaxLength(100)]
        public string EV_Rptng_Mgr_Name_ConDet { get; set; }// text filed

        [MaxLength(100)]
        public string EV_Sub_Rptng_Mgr_Name_Dsg_EidPh { get; set; }// text filed

        [MaxLength(100)]
        public string EV_Supervisor { get; set; }// Text filed

        [MaxLength(100)]
        public string EV_Sup_Name_Dsg { get; set; }// text filed

        [MaxLength(200)]
        public string EV_FormalitiesExit { get; set; }//Text Area

        [MaxLength(200)]
        public string EV_FullFinalFormalities { get; set; }//Text Area

        [MaxLength(200)]
        public string EV_ExitFormalitiesPending { get; set; }//Text Area


        [MaxLength(200)]
        public string EV_HandledResponsibilities { get; set; }//Text Area

        [MaxLength(200)]
        public string EV_ResponsibilitiesHandled { get; set; }//Text Area

        [MaxLength(200)]
        public string EV_OverallJobPerformance { get; set; }//Text Area

        [MaxLength(200)]
        public string EV_PerfromanceLevel { get; set; }//Text Area

        [MaxLength(200)]
        public string EV_KnowledgeProcess { get; set; }//Text Area

        [MaxLength(200)]
        public string EV_ValidationBusinessOperation { get; set; }//Text Area

        [MaxLength(200)]
        public string EV_DutiesNature { get; set; }//Text Area

        [MaxLength(200)]
        public string EV_Prof_Ablts_Aplcnt { get; set; }//Text Area

        [MaxLength(200)]
        public string EV_Dsplnry_Perf_Iss_Rptd { get; set; }//Text Area

        [MaxLength(200)]
        public string EV_DisciplinaryIssue { get; set; }//Text Area

        [MaxLength(200)]
        public string EV_Cmplnts_Rslt_Dsplnry_Sub { get; set; }//Text Area

        [MaxLength(200)]
        public string EV_Emp_Invol_Fraud_Actvty { get; set; }//Text Area

        [MaxLength(200)]
        public string EV_PerformanceIssues { get; set; }//Text Area

        [MaxLength(200)]
        public string EV_TeamConduct { get; set; }//Text Area

        [MaxLength(200)]
        public string EV_TeamPlayerNot { get; set; }//Text Area

        [MaxLength(200)]
        public string EV_TechnologicalKnowledge { get; set; }//Text Area

        [MaxLength(200)]
        public string EV_Fback_Accnt_Cduct_Jb { get; set; }//Text Area

        [MaxLength(200)]
        public string EV_DisciplinedHowHe { get; set; }//Text Area

        [MaxLength(200)]
        public string EV_LongKnown { get; set; }//Text Area

        [MaxLength(200)]
        public string EV_LongWork { get; set; }//Text Area

        [MaxLength(200)]
        public string EV_LeavesTook { get; set; }//Text Area

        [MaxLength(200)]
        public string EV_People_Rptd { get; set; }//Text Area

        [MaxLength(200)]
        public string EV_Conduct { get; set; }//Text Area

        [MaxLength(200)]
        public string EV_Performance { get; set; }//Text Area

        [MaxLength(100)]
        public string EV_Perf_Rate { get; set; }// drop down -1,2,3,4,5,6,7,8,9,10

        [MaxLength(100)]
        public string EV_Mtng_Deadline { get; set; }// drop down -1,2,3,4,5,6,7,8,9,10

        [MaxLength(100)]
        public string EV_Comm_Skills { get; set; }// drop down -1,2,3,4,5,6,7,8,9,10

        [MaxLength(100)]
        public string EV_Wrtn_Comm_Skills { get; set; }// drop down -1,2,3,4,5,6,7,8,9,10

        [MaxLength(100)]
        public string EV_Verb_Comm_Skills { get; set; }// drop down -1,2,3,4,5,6,7,8,9,10

        [MaxLength(100)]
        public string EV_Procs_Knowlg { get; set; }// drop down -1,2,3,4,5,6,7,8,9,10

        [MaxLength(100)]
        public string EV_Attdnce_Punct { get; set; }// drop down -1,2,3,4,5,6,7,8,9,10

        [MaxLength(100)]
        public string EV_Ldrshp_Qualty { get; set; }// drop down -1,2,3,4,5,6,7,8,9,10

        [MaxLength(100)]
        public string EV_Mngng_Mtng_Int_Cust_Expect { get; set; }// drop down -1,2,3,4,5,6,7,8,9,10

        [MaxLength(200)]
        public string EV_OtherIssues { get; set; }// text  Area


        [MaxLength(200)]
        public string EV_AdherenceTAT { get; set; }// text  Area

        [MaxLength(200)]
        public string EV_CompletesTATWork { get; set; }// text  Area

        [MaxLength(200)]
        public string EV_Atude_Hon_Per_Aplcnt { get; set; }// text  Area

        [MaxLength(100)]
        public string EV_WorkAttitude { get; set; }// text  Area

        [MaxLength(100)]
        public string EV_RatingTeaming { get; set; }// drop down -1,2,3,4,5,6,7,8,9,10

        [MaxLength(100)]
        public string EV_Rate_Tech { get; set; }// drop down -1,2,3,4,5,6,7,8,9,10

        [MaxLength(100)]
        public string EV_AttendancePunctuality { get; set; }// text filed

        [MaxLength(100)]
        public string EV_Cmnts_Abt_Sub { get; set; }// text Area

        [MaxLength(100)]
        public string EV_SubjectComments { get; set; }// text Area

        [MaxLength(100)]
        public string EV_AdditionalComments { get; set; }// text Area

        [MaxLength(4000)]
        public string EV_OtherComments { get; set; }// text Area

        [MaxLength(100)]
        public string EV_OtherRemarks { get; set; }// text Area

        [MaxLength(100)]
        public string EV_AttachedDocumentGenuine { get; set; }// drop down -Yes / No

        [MaxLength(100)]
        public string EV_ResponseMode { get; set; }// Drop Down 

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? EV_OnlineVerificationCondctedOn { get; set; }// Date

        [MaxLength(100)]
        public string EV_RefereeResponse { get; set; }// Drop Down -Written / Verbal / Online / Others

        [MaxLength(100)]
        public string EV_RespondentName { get; set; }// text Filed

        [MaxLength(100)]
        public string EV_RefereeDetails { get; set; }// text Filed

        [MaxLength(100)]
        public string EV_RespondentDesignation { get; set; }// text Filed

        [MaxLength(200)]
        public string EV_RefereeNameDetails { get; set; }//Text Area

        [MaxLength(200)]
        public string EV_DetailsReferee { get; set; }//Text Area

        [MaxLength(200)]
        public string EV_ContactDetail { get; set; }//Text Area

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? EV_DateTime { get; set; }// date 

        [MaxLength(100)]
        public string EV_Verfson_Poss_Phy_Visit { get; set; }// drop down - Yes /No

        [MaxLength(200)]
        public string EV_Details_Verfson_EidPh_Phyvst { get; set; }// text area 

        [MaxLength(200)]
        public string EV_Nme_Add_Cmpny_Exist_Now { get; set; }// Text Area 

        [MaxLength(200)]
        public string EV_Name_Dsg_Verfyng_Auth { get; set; }// Text Area

        [MaxLength(200)]
        public string EV_Verified { get; set; }// Text Area

        [MaxLength(200)]
        public string EV_Verified_by { get; set; }// Text Area

        [MaxLength(200)]
        public string EV_Verified_by_dsg_ConNo { get; set; }// Text Area

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? EV_Date_Time_Contacted { get; set; }// date

        [MaxLength(200)]
        public string EV_OtherDetails6 { get; set; }// text Area

        [MaxLength(200)]
        public string EV_OtherDetails7 { get; set; }// text Area

        [MaxLength(200)]
        public string EV_OtherDetails8 { get; set; }// text Area

        [MaxLength(200)]
        public string EV_OtherDetails9 { get; set; }// text Area

        [MaxLength(200)]
        public string EV_OtherDetails10 { get; set; }// text Area

        [MaxLength(200)]
        public string EV_OtherDetails11 { get; set; }// text Area

        [MaxLength(200)]
        public string EV_OtherDetails12 { get; set; }// text Area

        [MaxLength(200)]
        public string EV_OtherDetails13 { get; set; }// text Area

        [MaxLength(200)]
        public string EV_OtherDetails14 { get; set; }// text Area

        [MaxLength(200)]
        public string EV_OtherDetails15 { get; set; }// text Area

        [MaxLength(200)]
        public string ATA_Reverse_Directory { get; set; }// text Area

        [MaxLength(200)]
        public string ATA_Residcl_Commrcl { get; set; }// text Area

        [MaxLength(200)]
        public string ATA_Stock_Exchnge { get; set; }// text Area

        [MaxLength(200)]
        public string ATA_Telphne_Directry_Srch { get; set; }// text Area

        [MaxLength(200)]
        public string ATA_Yellow_Pages { get; set; }// text Area

        [MaxLength(200)]
        public string ATA_Who_Domain { get; set; }// text Area

        [MaxLength(200)]
        public string ATA_GoogleSearch { get; set; }// text Area

        [MaxLength(200)]
        public string ATA_InfrastructureOfCmp { get; set; }// text Area

        [MaxLength(200)]
        public string ATA_Just_Dial { get; set; }// text Area

        [MaxLength(200)]
        public string ATA_Neighbor_Check1 { get; set; }// text Area

        [MaxLength(200)]
        public string ATA_NASCOM_Empanelment { get; set; }// text Area

        [MaxLength(200)]
        public string ATA_Photograph { get; set; }// text Area

        [MaxLength(200)]
        public string ATA_Physical_Site { get; set; }// text Area

        [MaxLength(200)]
        public string ATA_Reg_Company { get; set; }// text Area

        [MaxLength(200)]
        public string ATA_PO_Courier_Check { get; set; }// text Area

        [MaxLength(200)]
        public string ATA_Network_Sol { get; set; }// text Area

        [MaxLength(200)]
        public string ATA_ApprxEmpWorking { get; set; }// text Area

        [MaxLength(100)]
        public string ATA_CID_No { get; set; }// text filed

        [MaxLength(100)]
        public string ATA_Cmpny_Addr { get; set; }// text filed

        [MaxLength(200)]
        public string ATA_Decoy_Call { get; set; }// text Area

        [MaxLength(200)]
        public string ATA_Cmpny_Website { get; set; }// text Area          

        #endregion



        [MaxLength(50)]
        public string TypeRevert { get; set; }

        [MaxLength(50)]
        public string CheckStatus { get; set; }

        [MaxLength(50)]
        public string Severity { get; set; }

        [MaxLength(50)]
        public string ColorName { get; set; }

        [MaxLength(3000)]
        public string ReportComments { get; set; }

        public byte Status { get; set; }

        public short VerifiedBy { get; set; }

        [MaxLength(100)]
        public string VerifierDesignation { get; set; }

        [MaxLength(20)]
        public string VerifierContactNo { get; set; }

        [MaxLength(20)]
        public string VerifierMobileNo { get; set; }

        [MaxLength(100)]
        public string VerifierEmailId { get; set; }

        public DateTime? VerifiedDate { get; set; }
        public string ApproveStatus { get; set; }
    }

    public class UpdatePQEmploymentVerViewModel
    {

        [ScaffoldColumn(false)]
        public int EmploymentVerRowID { get; set; }

        [ScaffoldColumn(false)]
        public int EmploymentRowID { get; set; }

        [ScaffoldColumn(false)]
        public short ClientRowID { get; set; }
       
        [ScaffoldColumn(false)]
        public short SubCheckRowID { get; set; }

        [ScaffoldColumn(false)]
        public int PersonalRowID { get; set; }
             
        [ScaffoldColumn(false)]
        public short CheckFamilyRowID { get; set; }

        #region Employment Verification Antecedents
        [MaxLength(100)]
        public string EV_CandidateName { get; set; }// Text Field

        [MaxLength(500)]
        public string EV_OrganisationName { get; set; }//Drop Down -Auto populated from employer master 

        public string EV_OrganisationNameId { get; set; }

        [MaxLength(100)]
        public string EV_CmpLocation { get; set; }// drop down -Location filter from employer master 

        [MaxLength(700)]
        public string EV_CmpNameAddress { get; set; }// Text Field -Auto populated from employer master 

        [MaxLength(100)]
        public string EV_EmployeeName { get; set; }// Text Field

        [MaxLength(100)]
        public string EV_EmployeeCode { get; set; }//Text Field

        [MaxLength(100)]
        public string EV_EmploymentPeriod { get; set; } // text filed To & from calender

        [MaxLength(100)]
        public string EV_EmploymentDual { get; set; } // drop down -yes/No

        [MaxLength(100)]
        public string EV_EmployedSelf { get; set; }// drop down -yes/No

        [MaxLength(100)]
        public string EV_lappingOver { get; set; }// drop down -yes/No

        [MaxLength(100)]
        public string EV_CandidateTenureWorked { get; set; } // text filed To & from calender

        [MaxLength(100)]
        public string EV_EmployeeTenure { get; set; } // text filed To & from calender

        [MaxLength(100)]
        public string EV_Designation { get; set; }// text filed 

        [MaxLength(100)]
        public string EV_EmployedEarlierAlso { get; set; } // drop down -yes/No

        [MaxLength(100)]
        public string EV_nature_emplo { get; set; } // drop down -Permanent / Temporary

        [MaxLength(100)]
        public string EV_typ_employ { get; set; }// drop down -Full time / Part time

        [MaxLength(100)]
        public string EV_EmpType { get; set; }// drop down -Permanent / Temporary / contractual

        [MaxLength(100)]
        public string EV_Department { get; set; }// text filed 

        [MaxLength(100)]
        public string EV_CmpCost { get; set; }// text filed 

        [MaxLength(100)]
        public string EV_last_sal { get; set; }// text filed 

        [MaxLength(200)]
        public string EV_leavingReason { get; set; }// text Area 

        [MaxLength(200)]
        public string EV_termina { get; set; }// text Area 

        [MaxLength(200)]
        public string EV_DiscontinueServiceReason { get; set; }// text Area 

        [MaxLength(200)]
        public string EV_EligibilityforRehire { get; set; }// text Area 

        [MaxLength(200)]
        public string EV_EligibilityOfRehire { get; set; }// text Area 

        [MaxLength(200)]
        public string EV_EligibileForRehire { get; set; }// text Area 


        [MaxLength(200)]
        public string EV_Rhre_Aplcnt_Situt_Ars_Ftre { get; set; }// text Area 

        [MaxLength(100)]
        public string EV_Reported { get; set; }// text filed 

        [MaxLength(100)]
        public string EV_ReportingManagerName { get; set; }// text filed 

        [MaxLength(100)]
        public string EV_Rptng_Mgr_Dsg_Eid_Tno { get; set; }// Text Area


        [MaxLength(100)]
        public string EV_Rptng_Mgr_Name_Ph { get; set; }// text filed

        [MaxLength(100)]
        public string EV_Rptng_Mgr_Name_ConDet { get; set; }// text filed

        [MaxLength(100)]
        public string EV_Sub_Rptng_Mgr_Name_Dsg_EidPh { get; set; }// text filed

        [MaxLength(100)]
        public string EV_Supervisor { get; set; }// Text filed

        [MaxLength(100)]
        public string EV_Sup_Name_Dsg { get; set; }// text filed

        [MaxLength(200)]
        public string EV_FormalitiesExit { get; set; }//Text Area

        [MaxLength(200)]
        public string EV_FullFinalFormalities { get; set; }//Text Area

        [MaxLength(200)]
        public string EV_ExitFormalitiesPending { get; set; }//Text Area


        [MaxLength(200)]
        public string EV_HandledResponsibilities { get; set; }//Text Area

        [MaxLength(200)]
        public string EV_ResponsibilitiesHandled { get; set; }//Text Area

        [MaxLength(200)]
        public string EV_OverallJobPerformance { get; set; }//Text Area

        [MaxLength(200)]
        public string EV_PerfromanceLevel { get; set; }//Text Area

        [MaxLength(200)]
        public string EV_KnowledgeProcess { get; set; }//Text Area

        [MaxLength(200)]
        public string EV_ValidationBusinessOperation { get; set; }//Text Area

        [MaxLength(200)]
        public string EV_DutiesNature { get; set; }//Text Area

        [MaxLength(200)]
        public string EV_Prof_Ablts_Aplcnt { get; set; }//Text Area

        [MaxLength(200)]
        public string EV_Dsplnry_Perf_Iss_Rptd { get; set; }//Text Area

        [MaxLength(200)]
        public string EV_DisciplinaryIssue { get; set; }//Text Area

        [MaxLength(200)]
        public string EV_Cmplnts_Rslt_Dsplnry_Sub { get; set; }//Text Area

        [MaxLength(200)]
        public string EV_Emp_Invol_Fraud_Actvty { get; set; }//Text Area

        [MaxLength(200)]
        public string EV_PerformanceIssues { get; set; }//Text Area

        [MaxLength(200)]
        public string EV_TeamConduct { get; set; }//Text Area

        [MaxLength(200)]
        public string EV_TeamPlayerNot { get; set; }//Text Area

        [MaxLength(200)]
        public string EV_TechnologicalKnowledge { get; set; }//Text Area

        [MaxLength(200)]
        public string EV_Fback_Accnt_Cduct_Jb { get; set; }//Text Area

        [MaxLength(200)]
        public string EV_DisciplinedHowHe { get; set; }//Text Area

        [MaxLength(200)]
        public string EV_LongKnown { get; set; }//Text Area

        [MaxLength(200)]
        public string EV_LongWork { get; set; }//Text Area

        [MaxLength(200)]
        public string EV_LeavesTook { get; set; }//Text Area

        [MaxLength(200)]
        public string EV_People_Rptd { get; set; }//Text Area

        [MaxLength(200)]
        public string EV_Conduct { get; set; }//Text Area

        [MaxLength(200)]
        public string EV_Performance { get; set; }//Text Area

        [MaxLength(100)]
        public string EV_Perf_Rate { get; set; }// drop down -1,2,3,4,5,6,7,8,9,10

        [MaxLength(100)]
        public string EV_Mtng_Deadline { get; set; }// drop down -1,2,3,4,5,6,7,8,9,10

        [MaxLength(100)]
        public string EV_Comm_Skills { get; set; }// drop down -1,2,3,4,5,6,7,8,9,10

        [MaxLength(100)]
        public string EV_Wrtn_Comm_Skills { get; set; }// drop down -1,2,3,4,5,6,7,8,9,10

        [MaxLength(100)]
        public string EV_Verb_Comm_Skills { get; set; }// drop down -1,2,3,4,5,6,7,8,9,10

        [MaxLength(100)]
        public string EV_Procs_Knowlg { get; set; }// drop down -1,2,3,4,5,6,7,8,9,10

        [MaxLength(100)]
        public string EV_Attdnce_Punct { get; set; }// drop down -1,2,3,4,5,6,7,8,9,10

        [MaxLength(100)]
        public string EV_Ldrshp_Qualty { get; set; }// drop down -1,2,3,4,5,6,7,8,9,10

        [MaxLength(100)]
        public string EV_Mngng_Mtng_Int_Cust_Expect { get; set; }// drop down -1,2,3,4,5,6,7,8,9,10

        [MaxLength(200)]
        public string EV_OtherIssues { get; set; }// text  Area


        [MaxLength(200)]
        public string EV_AdherenceTAT { get; set; }// text  Area

        [MaxLength(200)]
        public string EV_CompletesTATWork { get; set; }// text  Area

        [MaxLength(200)]
        public string EV_Atude_Hon_Per_Aplcnt { get; set; }// text  Area

        [MaxLength(100)]
        public string EV_WorkAttitude { get; set; }// text  Area

        [MaxLength(100)]
        public string EV_RatingTeaming { get; set; }// drop down -1,2,3,4,5,6,7,8,9,10

        [MaxLength(100)]
        public string EV_Rate_Tech { get; set; }// drop down -1,2,3,4,5,6,7,8,9,10

        [MaxLength(100)]
        public string EV_AttendancePunctuality { get; set; }// text filed

        [MaxLength(100)]
        public string EV_Cmnts_Abt_Sub { get; set; }// text Area

        [MaxLength(100)]
        public string EV_SubjectComments { get; set; }// text Area

        [MaxLength(100)]
        public string EV_AdditionalComments { get; set; }// text Area

        [MaxLength(4000)]
        public string EV_OtherComments { get; set; }// text Area

        [MaxLength(100)]
        public string EV_OtherRemarks { get; set; }// text Area

        [MaxLength(100)]
        public string EV_AttachedDocumentGenuine { get; set; }// drop down -Yes / No

        [MaxLength(100)]
        public string EV_ResponseMode { get; set; }// Drop Down 

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? EV_OnlineVerificationCondctedOn { get; set; }// Date

        [MaxLength(100)]
        public string EV_RefereeResponse { get; set; }// Drop Down -Written / Verbal / Online / Others

        [MaxLength(100)]
        public string EV_RespondentName { get; set; }// text Filed

        [MaxLength(100)]
        public string EV_RefereeDetails { get; set; }// text Filed

        [MaxLength(100)]
        public string EV_RespondentDesignation { get; set; }// text Filed

        [MaxLength(200)]
        public string EV_RefereeNameDetails { get; set; }//Text Area

        [MaxLength(200)]
        public string EV_DetailsReferee { get; set; }//Text Area

        [MaxLength(200)]
        public string EV_ContactDetail { get; set; }//Text Area

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? EV_DateTime { get; set; }// date 

        [MaxLength(100)]
        public string EV_Verfson_Poss_Phy_Visit { get; set; }// drop down - Yes /No

        [MaxLength(200)]
        public string EV_Details_Verfson_EidPh_Phyvst { get; set; }// text area 

        [MaxLength(200)]
        public string EV_Nme_Add_Cmpny_Exist_Now { get; set; }// Text Area 

        [MaxLength(200)]
        public string EV_Name_Dsg_Verfyng_Auth { get; set; }// Text Area

        [MaxLength(200)]
        public string EV_Verified { get; set; }// Text Area

        [MaxLength(200)]
        public string EV_Verified_by { get; set; }// Text Area

        [MaxLength(200)]
        public string EV_Verified_by_dsg_ConNo { get; set; }// Text Area

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? EV_Date_Time_Contacted { get; set; }// date

        [MaxLength(200)]
        public string EV_OtherDetails6 { get; set; }// text Area

        [MaxLength(200)]
        public string EV_OtherDetails7 { get; set; }// text Area

        [MaxLength(200)]
        public string EV_OtherDetails8 { get; set; }// text Area

        [MaxLength(200)]
        public string EV_OtherDetails9 { get; set; }// text Area

        [MaxLength(200)]
        public string EV_OtherDetails10 { get; set; }// text Area

        [MaxLength(200)]
        public string EV_OtherDetails11 { get; set; }// text Area

        [MaxLength(200)]
        public string EV_OtherDetails12 { get; set; }// text Area

        [MaxLength(200)]
        public string EV_OtherDetails13 { get; set; }// text Area

        [MaxLength(200)]
        public string EV_OtherDetails14 { get; set; }// text Area

        [MaxLength(200)]
        public string EV_OtherDetails15 { get; set; }// text Area

        [MaxLength(200)]
        public string ATA_Reverse_Directory { get; set; }// text Area

        [MaxLength(200)]
        public string ATA_Residcl_Commrcl { get; set; }// text Area

        [MaxLength(200)]
        public string ATA_Stock_Exchnge { get; set; }// text Area

        [MaxLength(200)]
        public string ATA_Telphne_Directry_Srch { get; set; }// text Area

        [MaxLength(200)]
        public string ATA_Yellow_Pages { get; set; }// text Area

        [MaxLength(200)]
        public string ATA_Who_Domain { get; set; }// text Area

        [MaxLength(200)]
        public string ATA_GoogleSearch { get; set; }// text Area

        [MaxLength(200)]
        public string ATA_InfrastructureOfCmp { get; set; }// text Area

        [MaxLength(200)]
        public string ATA_Just_Dial { get; set; }// text Area

        [MaxLength(200)]
        public string ATA_Neighbor_Check1 { get; set; }// text Area

        [MaxLength(200)]
        public string ATA_NASCOM_Empanelment { get; set; }// text Area

        [MaxLength(200)]
        public string ATA_Photograph { get; set; }// text Area

        [MaxLength(200)]
        public string ATA_Physical_Site { get; set; }// text Area

        [MaxLength(200)]
        public string ATA_Reg_Company { get; set; }// text Area

        [MaxLength(200)]
        public string ATA_PO_Courier_Check { get; set; }// text Area

        [MaxLength(200)]
        public string ATA_Network_Sol { get; set; }// text Area

        [MaxLength(200)]
        public string ATA_ApprxEmpWorking { get; set; }// text Area

        [MaxLength(100)]
        public string ATA_CID_No { get; set; }// text filed

        [MaxLength(100)]
        public string ATA_Cmpny_Addr { get; set; }// text filed

        [MaxLength(200)]
        public string ATA_Decoy_Call { get; set; }// text Area

        [MaxLength(200)]
        public string ATA_Cmpny_Website { get; set; }// text Area   

        [MaxLength(50)]
        public string CheckStatus { get; set; }
        public int CheckStatusId { get; set; }

        #endregion

        #region Employment Closer fields

        public EmploymentVerCloserViewModel CloserViewModel { get; set; }
        public bool Aging { get; set; }

        #endregion
        public decimal Amount { get; set; }
    }

    public class EmploymentVerCloserViewModel
    {

        [ScaffoldColumn(false)]
        public int EmploymentVerRowID { get; set; }

        [ScaffoldColumn(false)]
        public int EmploymentRowID { get; set; }

        [ScaffoldColumn(false)]
        public short ClientRowID { get; set; }

        [ScaffoldColumn(false)]
        public short CheckFamilyRowID { get; set; }

        [ScaffoldColumn(false)]
        public short SubCheckRowID { get; set; }

        [ScaffoldColumn(false)]
        public int PersonalRowID { get; set; }
        public string Disposition { get; set; }
        public string DispositionID { get; set; }
        //public int ClientDispositionRowId { get; set; }
        public string ColorName { get; set; }
        public string Severity { get; set; }
        public int SeverityId { get; set; }
        public string Remarks { get; set; }
        public string CheckStatus { get; set; }// check action and pqaddress update   
        public string ActionHistory { get; set; }
        public string ShortRemarks { get; set; }
        public bool Aging { get; set; }


        public List<int> CDispositionRowId { get; set; }
    }

    public class EmploymentStatusPVTMVeiwModel
    {
        public int EmploymentRowId { get; set; }
        public string CheckStatus { get; set; }

        public string PVTMCheckStatus { get; set; }
        public string PVTMQCCheckStatus { get; set; }

        public DateTime? UpdatedDate { get; set; }
    }
}
