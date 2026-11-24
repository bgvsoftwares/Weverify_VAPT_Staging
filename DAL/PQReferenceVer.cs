using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class PQReferenceVer
    {
        public PQReferenceVer()
        {
            ReferenceCheckVerRowID                      = 0;
            ReferenceCheckRowID                         = 0;

            RV_Cmplnts_Result_Dsplnry_Sub               =string.Empty;
            RV_Reliable_Honest_Person                   =string.Empty;
            RV_Cand_Fmly_Associat_Politics              =string.Empty;
            RV_Duration                                 =string.Empty;
            RV_Relation_Sub                             =string.Empty;
            RV_Response_Frm_Rfre                        =string.Empty;
            RV_CandidateName                            =string.Empty;
            RV_OrganisationName                         =string.Empty;
            RV_RefereeName                              =string.Empty;
            RV_RefereeDesignation                       =string.Empty;
            RV_Refre_Prsnt_Emp                          =string.Empty;
            RV_Refre_ConDet                             =string.Empty;
            RV_Refere_Desig                             =string.Empty;
            RV_Refere_Add                               =string.Empty;
            RV_Refre_MobNo                              =string.Empty;
            RV_Refre_Eid                                =string.Empty;
            RV_Prof_Relationship_Cand_Dur               =string.Empty;
            RV_Know_Long                                =string.Empty;
            RV_Tenure_Emp                               =string.Empty;
            RV_Know_Subject                             =string.Empty;
            RV_Was_Cand_Honest                          =string.Empty;
            RV_Rate_Prof_Ability                        =string.Empty;
            RV_Commun_Jr_Sr_Colleague                   =string.Empty;
            RV_Relationship_With_Sub                    =string.Empty;
            RV_Overall_Jb_Perf                          =string.Empty;
            RV_Response_Wrtn_Verb                       =string.Empty;
            RV_State_Specify_Medcl_Prblm                =string.Empty;
            RV_Sate_Specify_Per_Prblm_Perf              =string.Empty;
            RV_Sub_Perf_Intgrty_Wrk                     =string.Empty;
            RV_Particular_Gud_Least_Effect              =string.Empty;
            RV_Comment_Per_Qualities                    =string.Empty;
            RV_Aware_Son_Daughtr_Appld_Jb               =string.Empty;
            RV_Character                                =string.Empty;
            RV_Anger_Mngmt                              =string.Empty;
            RV_Behaviour                                =string.Empty;
            RV_Intgrty_Commitmnt                        =string.Empty;
            RV_Nature                                   =string.Empty;
            RV_Perf_level                               =string.Empty;
            RV_SoftSkill                                =string.Empty;
            RV_Action_Orient                            =string.Empty;
            RV_Algmnt_Skills                            =string.Empty;
            RV_Anger_Mgmt_Skills                        =string.Empty;
            RV_Reliability                              =string.Empty;
            RV_Aptitutde                                =string.Empty;
            RV_Influence_Style                          =string.Empty;
            RV_Initiative                               =string.Empty;
            RV_Interper_Skills                          =string.Empty;
            RV_Comm_Skills                              =string.Empty;
            RV_Energy                                   =string.Empty;
            RV_Performance                              =string.Empty;
            RV_Team_Mgmt_Skills                         =string.Empty;
            RV_Teamng_Skills                            =string.Empty;
            RV_Tech_Competence                          =string.Empty;
            RV_Time_Mgmt_Org_Skills                     =string.Empty;
            RV_Value_System                             =string.Empty;
            RV_Voice_Accent                             =string.Empty;
            RV_Plan_Fr_Higher_Studies                   =string.Empty;
            RV_What_kind_of_Jb                          =string.Empty;
            RV_Family_Background                        =string.Empty;
            RV_His_Behaviour                            =string.Empty;
            RV_His_Conduct                              =string.Empty;
            RV_His_Perf                                 =string.Empty;
            RV_Recommd_Work_MNC                         =string.Empty;
            RV_Manag_Analyz_MIS_Plng                    =string.Empty;
            RV_Mngng_Int_Ext_Cust_Expect                =string.Empty;
            RV_Additional_Cmmnts_RC                     =string.Empty;
            RV_Cmplnts_Rslt_Dsplnry_Sub                 =string.Empty;
            RV_Achivement_During_Tenure                 =string.Empty;
            RV_Other_cmmnts_RC                          =string.Empty;
            RV_Other_Qualitative_Input                  =string.Empty;
            RV_Qualitative_Input_Plng                   =string.Empty;
            RV_Cand_Record_Misreprsnt_Facts             =string.Empty;
            RV_Area_Development                         =string.Empty;
            RV_Attendance_Punct_RC                      =string.Empty;
            RV_Cand_Attitude_Task_Hndld                 =string.Empty;
            RV_Comm_Skills_RC                           =string.Empty;
            RV_Contribution_Intro_Concept               =string.Empty;
            RV_Dsg_Refre                                =string.Empty;
            RV_Dsg_RC                                   =string.Empty;
            RV_CTC                                      =string.Empty;
            RV_Reason_Leav                              =string.Empty;
            RV_Relationship_With_Sub_Dur                =string.Empty;
            RV_Sub_Elgblty_Rhre                         =string.Empty;
            RV_Sub_Meet_Pos_Perf_Req                    =string.Empty;
            RV_Recall_Edu_Credential                    =string.Empty;
            RV_Recall_Emp_Back                          =string.Empty;
            RV_Do_you_trust                             =string.Empty;
            RV_Effect_Settng_Meetng_Goals               =string.Empty;
            RV_Cand_Interpersonal_Skills                =string.Empty;
            RV_Cand_Mngmt_Skills                        =string.Empty;
            RV_Acquainted_Cand_how_long                 =string.Empty;
            RV_Diligent_Twds_Work                       =string.Empty;
            RV_How_Discipline                           =string.Empty;
            RV_How_Ppl_Respond                          =string.Empty;
            RV_Rate_Perf_Ability                        =string.Empty;
            RV_Respond_Work_Pressure                    =string.Empty;
            RV_Cand_Mtng_Target_Dline                   =string.Empty;
            RV_As_Team_Player                           =string.Empty;
            RV_Atude_Twds_Work                          =string.Empty;
            RV_Was_Cand_Motivated                       =string.Empty;
            RV_How_Was_Behaviour                        =string.Empty;
            RV_Perform_Task_Time_Cost                   =string.Empty;
            RV_Rate_Drive_Fr_Result                     =string.Empty;
            RV_Rate_Resilience_Perserve                 =string.Empty;
            RV_Term_Resource_Discipline                 =string.Empty;
            RV_Rhre_Cmpny_Again                         =string.Empty;
            RV_Is_Reliable_Hon_Person                   =string.Empty;
            RV_Cand_High_Intgrty                        =string.Empty;
            RV_Cand_Sincere_Hon                         =string.Empty;
            RV_Knowlge_Pos_Responsiblts                 =string.Empty;
            RV_Major_Carrer_Changes_Sub                 =string.Empty;
            RV_Mngng_Mtng_Int_Cust_Expect               =string.Empty;
            RV_Mgng_Complexity                          =string.Empty;
            RV_Meeting_Deadlines                        =string.Empty;
            RV_Mode_Of_Exit                             =string.Empty;
            RV_Moral_Values                             =string.Empty;
            RV_Oral_Comm_Skills                         =string.Empty;
            RV_Commnt_Ldrshp_Abilities                  =string.Empty;
            RV_Specify_Cand_Area_Improve                =string.Empty;
            RV_Specify_Cand_Strength                    =string.Empty;
            RV_Prblm_Solving_Ablts                      =string.Empty;
            RV_Prof_Conduct                             =string.Empty;
            RV_Prof_Strength                            =string.Empty;
            RV_Prof_Values                              =string.Empty;
            RV_Quality_Of_Work                          =string.Empty;
            RV_Rating_On_Scale                          =string.Empty;
            RV_Rating_Que_1                             =string.Empty;
            RV_Rating_Que_2                             =string.Empty;
            RV_Rating_Que_3                             =string.Empty;
            RV_Rating_Que_4                             =string.Empty;
            RV_Recommd_Similar_Pos                      =string.Empty;
            RV_Reslt_Orient                             =string.Empty;
            RV_Role_Responsibilities_Cand               =string.Empty;
            RV_Stress_Mgmt                              =string.Empty;
            RV_Sub_Perf_Intgrty_Wrk_RC                  =string.Empty;
            RV_Suff_Notice_Period_Prov                  =string.Empty;
            RV_Team_Conduct                             =string.Empty;
            RV_Team_Ldrshp                              =string.Empty;
            RV_Team_Mgmt                                =string.Empty;
            RV_Was_Sub_Team_Player                      =string.Empty;
            RV_Was_Cand_Involve_Union_Act               =string.Empty;
            RV_Weakness                                 =string.Empty;
            RV_Strength_Area_Developmnt                 =string.Empty;
            RV_Cand_Edu_Prof_Qualification              =string.Empty;
            RV_Sub_Area_Develop_Weak                    =string.Empty;
            RV_Say_Abt_Enterpren_Skills                 =string.Empty;
            RV_Particiular_Gud_Least_Effect             =string.Empty;
            RV_Pos_Cand_Working                         =string.Empty;
            RV_TimePeriod_Cand_Work_Under               =string.Empty;
            RV_Work_Ethnics_Moral_Intgrty               =string.Empty;
            RV_Commnt_Per_Abilities                     =string.Empty;
            RV_Wrtn_Comm_Skills_RC                      =string.Empty;
            RV_AnyIssues                                =string.Empty;
            RV_Sub_Meet_Pos_Perf_Req_RC                 =string.Empty;
            RV_Sub_Comm_Interper_Skills                 =string.Empty;
            RV_Sub_Effect_Gettng_Rslt                   =string.Empty;
            RV_Intgrty_Commitmnt_RC                     =string.Empty;
            RV_Is_Reliable_Hon_Person_RC                =string.Empty;
            RV_Nature_Job                               =string.Empty;
            RV_Overall_Jb_Perf_RC                       =string.Empty;
            RV_Social_Conduct                           = string.Empty;
            RV_Social_Conduct_RC                        =string.Empty;
            RV_Sub_Per_Prof_Weak                        =string.Empty;
            RV_Work_Ethics_Moral_Intgrty_Rc             =string.Empty;
            RV_Recommnd_Sub_Emp_Top_MNC                 =string.Empty;
            RV_Cmmnts_Sub_Strngth_Weak                  =string.Empty;
            RV_Cmplnts_Rslt_Dsplnry_Sub_RC              =string.Empty;
            RV_Contribution_Intro_Concept_RC            =string.Empty;
            RV_Sub_Pos_Perf_Req_RC                      =string.Empty;
            RV_Confrontation_Tackled                    =string.Empty;
            RV_Pressure_Sit_Hndld                       =string.Empty;
            RV_React_To_Change                          =string.Empty;
            RV_Enforcing_Gud_Qualt_Work_Jrs             =string.Empty;
            RV_Sub_Team_Leader                          =string.Empty;
            RV_Any_Other_Cmmnts_Fdback                  =string.Empty;
            RV_Exp_During_Financ_Dealing                =string.Empty;
            RV_Rel_Busines_Entity                       =string.Empty;
            RV_Dur_Busines_Rel_Cmpny                    =string.Empty;
            RV_OtherDetails6                            =string.Empty;
            RV_OtherDetails7                            =string.Empty;
            RV_OtherDetails8                            =string.Empty;
            RV_OtherDetails9                            =string.Empty;
            RV_OtherDetails10                           =string.Empty;
            RV_OtherDetails11                           =string.Empty;
            RV_OtherDetails12                           =string.Empty;
            RV_OtherDetails13                           =string.Empty;
            RV_OtherDetails14                           =string.Empty;
            RV_OtherDetails15                           =string.Empty;
            ATA_Reverse_Directory                       =string.Empty;
            ATA_Residcl_Commrcl                         =string.Empty;
            ATA_Stock_Exchnge                           =string.Empty;
            ATA_Telphne_Directry_Srch                   =string.Empty;
            ATA_Yellow_Pages                            =string.Empty;
            ATA_Who_Domain                              =string.Empty;
            ATA_GoogleSearch                            =string.Empty;
            ATA_InfrastructureOfCmp                     =string.Empty;
            ATA_Just_Dial                               =string.Empty;
            ATA_Neighbor_Check1                         =string.Empty;
            ATA_NASCOM_Empanelment                      =string.Empty;
            ATA_Photograph                              =string.Empty;
            ATA_Physical_Site                           =string.Empty;
            ATA_Reg_Company                             =string.Empty;
            ATA_PO_Courier_Check                        =string.Empty;
            ATA_Network_Sol                             =string.Empty;
            ATA_ApprxEmpWorking                         =string.Empty;
            ATA_CID_No                                  =string.Empty;
            ATA_Cmpny_Addr                              =string.Empty;
            ATA_Decoy_Call                              =string.Empty;
            ATA_Cmpny_Website = string.Empty;

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

        public int      ReferenceCheckVerRowID                      { get; set; }

        public int      ReferenceCheckRowID                         { get; set; }
        public virtual PQReference PQReferenceCheck                 { get; set; }

        public string RV_Cmplnts_Result_Dsplnry_Sub                 {get;set;}              // Text Area
        public string RV_Reliable_Honest_Person                     {get;set;}              // Text Area
        public string RV_Cand_Fmly_Associat_Politics                {get;set;}              // Text Area
        public string RV_Duration                                   {get;set;}              // Text Area
        public string RV_Relation_Sub                               {get;set;}              // Text Area
        public string RV_Response_Frm_Rfre                          {get;set;}              // Text Area
        public string RV_CandidateName                              {get;set;}              // text Filed 
        public string RV_OrganisationName                           {get;set;}              // text Filed 
        public string RV_RefereeName                                {get;set;}              // text Filed 
        public string RV_RefereeDesignation                         {get;set;}              // text Filed 
        public string RV_Refre_Prsnt_Emp                            {get;set;}              // text Filed
        public string RV_Refre_ConDet                               {get;set;}              // text Filed
        public string RV_Refere_Desig                               {get;set;}              // text Filed
        public string RV_Refere_Add                                 {get;set;}              // text Filed
        public string RV_Refre_MobNo                                {get;set;}              // text Filed
        public string RV_Refre_Eid                                  {get;set;}              // text Filed - Email validation 
        public string RV_Prof_Relationship_Cand_Dur                 {get;set;}              //Text Area
        public string RV_Know_Long                                  {get;set;}              // Text Area
        public string RV_Tenure_Emp                                 {get;set;}              //text filed- To & from Calender
        public string RV_Know_Subject                               {get;set;}               //Drop down - Yes/No 
        public string RV_Was_Cand_Honest                            {get;set;}               // Drop Down Yes/No
        public string RV_Rate_Prof_Ability                          {get;set;}              //Drop Down -1,2,3,4,5,6,7,8,9,10
        public string RV_Commun_Jr_Sr_Colleague                     {get;set;}              // Text Area
        public string RV_Relationship_With_Sub                      {get;set;}              //Text Filed
        public string RV_Overall_Jb_Perf                            {get;set;}              //Drop Down -1,2,3,4,5,6,7,8,9,10
        public string RV_Response_Wrtn_Verb                         {get;set;}              //Drop Down -Verbal / Written / Other
        public string RV_State_Specify_Medcl_Prblm                  {get;set;}              //Text Area
        public string RV_Sate_Specify_Per_Prblm_Perf                {get;set;}              //Text Area
        public string RV_Sub_Perf_Intgrty_Wrk                       {get;set;}              //Text Area
        public string RV_Particular_Gud_Least_Effect                {get;set;}              //Text Area
        public string RV_Comment_Per_Qualities                      {get;set;}              //Text Area
        public string RV_Aware_Son_Daughtr_Appld_Jb                 {get;set;}              // Drop down - Yes/No
        public string RV_Character                                  {get;set;}              //Drop down -1,2,3,4,5
        public string RV_Anger_Mngmt                                {get;set;}              //Drop down -1,2,3,4,5
        public string RV_Behaviour                                  {get;set;}              //Drop down -1,2,3,4,5
        public string RV_Intgrty_Commitmnt                          {get;set;}              //Drop down -1,2,3,4,5
        public string RV_Nature                                     {get;set;}              //Drop down -1,2,3,4,5
        public string RV_Perf_level                                 {get;set;}              //Drop down -1,2,3,4,5
        public string RV_SoftSkill                                  {get;set;}              //Drop down -1,2,3,4,5
        public string RV_Action_Orient                              {get;set;}              //Drop down -1,2,3,4,5
        public string RV_Algmnt_Skills                              {get;set;}              //Drop down -1,2,3,4,5
        public string RV_Anger_Mgmt_Skills                          {get;set;}              //Drop down -1,2,3,4,5
        public string RV_Reliability                                {get;set;}              //Drop down -1,2,3,4,5
        public string RV_Aptitutde                                  {get;set;}              //Drop down -1,2,3,4,5
        public string RV_Influence_Style                            {get;set;}              //Drop down -1,2,3,4,5
        public string RV_Initiative                                 {get;set;}              //Drop down -1,2,3,4,5
        public string RV_Interper_Skills                            {get;set;}              //Drop down -1,2,3,4,5
        public string RV_Comm_Skills                                {get;set;}              //Drop down -1,2,3,4,5
        public string RV_Energy                                     {get;set;}              //Drop down -1,2,3,4,5
        public string RV_Performance                                {get;set;}              //Drop down -1,2,3,4,5
        public string RV_Team_Mgmt_Skills                           {get;set;}              //Drop down -1,2,3,4,5
        public string RV_Teamng_Skills                              {get;set;}              //Drop down -1,2,3,4,5
        public string RV_Tech_Competence                            {get;set;}              //Drop down -1,2,3,4,5
        public string RV_Time_Mgmt_Org_Skills                       {get;set;}              //Drop down -1,2,3,4,5
        public string RV_Value_System                               {get;set;}              //Drop down -1,2,3,4,5
        public string RV_Voice_Accent                               {get;set;}              //Drop down -1,2,3,4,5
        public string RV_Plan_Fr_Higher_Studies                     {get;set;}              //Text Area
        public string RV_What_kind_of_Jb                            {get;set;}              //Text Area
        public string RV_Family_Background                          {get;set;}              //drop down -1,2,3,4,5
        public string RV_His_Behaviour                              {get;set;}              //drop down -1,2,3,4,5
        public string RV_His_Conduct                                {get;set;}              //drop down -1,2,3,4,5
        public string RV_His_Perf                                   {get;set;}              //drop down -1,2,3,4,5
        public string RV_Recommd_Work_MNC                           {get;set;}              // drop down Yes / No / Not disclosed 
        public string RV_Manag_Analyz_MIS_Plng                      {get;set;}              // Text Area
        public string RV_Mngng_Int_Ext_Cust_Expect                  {get;set;}              // Text Area
        public string RV_Additional_Cmmnts_RC                       {get;set;}              // Text Area
        public string RV_Cmplnts_Rslt_Dsplnry_Sub                   {get;set;}              // Text Area
        public string RV_Achivement_During_Tenure                   {get;set;}              // Text Area
        public string RV_Other_cmmnts_RC                            {get;set;}              // Text Area
        public string RV_Other_Qualitative_Input                    {get;set;}              // Text Area
        public string RV_Qualitative_Input_Plng                     {get;set;}              // Text Area
        public string RV_Cand_Record_Misreprsnt_Facts               {get;set;}              // Text Area
        public string RV_Area_Development                           {get;set;}              // Text Area
        public string RV_Attendance_Punct_RC                        {get;set;}              // Text Area
        public string RV_Cand_Attitude_Task_Hndld                   {get;set;}              // Text Area
        public string RV_Comm_Skills_RC                             {get;set;}              // Drop down Yes / No / Not disclosed 
        public string RV_Contribution_Intro_Concept                 {get;set;}              //text area
        public string RV_Dsg_Refre                                  {get;set;}              // text
        public string RV_Dsg_RC                                     {get;set;}              // text filed
        public string RV_CTC                                        {get;set;}              //text filed
        public string RV_Reason_Leav                                {get;set;}              // text Area
        public string RV_Relationship_With_Sub_Dur                  {get;set;}              // text Area
        public string RV_Sub_Elgblty_Rhre                           {get;set;}              // text Area
        public string RV_Sub_Meet_Pos_Perf_Req                      {get;set;}              //drop down -Yes / No/ Not disclosed 
        public string RV_Recall_Edu_Credential                      {get;set;}              // text Area
        public string RV_Recall_Emp_Back                            {get;set;}              //TExt Area
        public string RV_Do_you_trust                               {get;set;}              //Drop down -Yes / No/ Not disclosed 
        public string RV_Effect_Settng_Meetng_Goals                 {get;set;}              //Drop down-1,2,3,4,5
        public string RV_Cand_Interpersonal_Skills                  {get;set;}              //text Area
        public string RV_Cand_Mngmt_Skills                          {get;set;}              //text Area
        public string RV_Acquainted_Cand_how_long                   {get;set;}              //text Area
        public string RV_Diligent_Twds_Work                         {get;set;}              //text Area
        public string RV_How_Discipline                             {get;set;}              //text Area
        public string RV_How_Ppl_Respond                            {get;set;}              //text Area
        public string RV_Rate_Perf_Ability                          {get;set;}              //drop down -1,2,3,4,5
        public string RV_Respond_Work_Pressure                      {get;set;}              //text Area
        public string RV_Cand_Mtng_Target_Dline                     {get;set;}              //Text Area
        public string RV_As_Team_Player                             {get;set;}              //Text Area
        public string RV_Atude_Twds_Work                            {get;set;}              //Text Area
        public string RV_Was_Cand_Motivated                         {get;set;}              //Text Area
        public string RV_How_Was_Behaviour                          {get;set;}              //Text Area
        public string RV_Perform_Task_Time_Cost                     {get;set;}              //Text Area
        public string RV_Rate_Drive_Fr_Result                       {get;set;}              //drop down -1,2,3,4,5
        public string RV_Rate_Resilience_Perserve                   {get;set;}              //drop down -1,2,3,4,5
        public string RV_Term_Resource_Discipline                   {get;set;}              //Text filed
        public string RV_Rhre_Cmpny_Again                           {get;set;}              //text Filed
        public string RV_Is_Reliable_Hon_Person                     {get;set;}              //Text Area
        public string RV_Cand_High_Intgrty                          {get;set;}              //Text Area
        public string RV_Cand_Sincere_Hon                           {get;set;}              //Text Filed
        public string RV_Knowlge_Pos_Responsiblts                   {get;set;}              // Drop down -1,2,3,4,5
        public string RV_Major_Carrer_Changes_Sub                   {get;set;}              //Text Area
        public string RV_Mngng_Mtng_Int_Cust_Expect                 {get;set;}              //Text Area
        public string RV_Mgng_Complexity                            {get;set;}              //Text Filed
        public string RV_Meeting_Deadlines                          {get;set;}              // Drop down -1,2,3,4,5
        public string RV_Mode_Of_Exit                               {get;set;}              //Text Area
        public string RV_Moral_Values                               {get;set;}              //Text Area
        public string RV_Oral_Comm_Skills                           {get;set;}              // Drop down -1,2,3,4,5
        public string RV_Commnt_Ldrshp_Abilities                    {get;set;}              //Text Area
        public string RV_Specify_Cand_Area_Improve                  {get;set;}              //Text Area
        public string RV_Specify_Cand_Strength                      {get;set;}              //Text Area
        public string RV_Prblm_Solving_Ablts                        {get;set;}              // Drop down -1,2,3,4,5
        public string RV_Prof_Conduct                               {get;set;}              // Drop down -1,2,3,4,5
        public string RV_Prof_Strength                              {get;set;}              //Text Area
        public string RV_Prof_Values                                {get;set;}              //Text Area
        public string RV_Quality_Of_Work                            {get;set;}              // Drop down -Excellent / Outstanding / Good / Average / Poor
        public string RV_Rating_On_Scale                            {get;set;}              // Drop down-1,2,3,4,5,6,7,8,9,10
        public string RV_Rating_Que_1                               {get;set;}              // Drop down-1,2,3,4,5,6,7,8,9,10
        public string RV_Rating_Que_2                               {get;set;}              // Drop down-1,2,3,4,5
        public string RV_Rating_Que_3                               {get;set;}              // Drop down-1,2,3,4,5,6,7,8,9,10
        public string RV_Rating_Que_4                               {get;set;}              // Drop down-1,2,3,4,5
        public string RV_Recommd_Similar_Pos                        {get;set;}              //Text Area
        public string RV_Reslt_Orient                               {get;set;}              // Text Area
        public string RV_Role_Responsibilities_Cand                 {get;set;}              // Text Area
        public string RV_Stress_Mgmt                                {get;set;}              // Drop down 1,2,3,4,5
        public string RV_Sub_Perf_Intgrty_Wrk_RC                    {get;set;}              // Text Area
        public string RV_Suff_Notice_Period_Prov                    {get;set;}              // Text Area
        public string RV_Team_Conduct                               {get;set;}              //Drop down 1,2,3,4,5
        public string RV_Team_Ldrshp                                {get;set;}              //Drop down 1,2,3,4,5
        public string RV_Team_Mgmt                                  {get;set;}              // text Area
        public string RV_Was_Sub_Team_Player                        {get;set;}              //Drop Down -Yes / No / Not disclosed 
        public string RV_Was_Cand_Involve_Union_Act                 {get;set;}              //text Area
        public string RV_Weakness                                   {get;set;}              //text Area
        public string RV_Strength_Area_Developmnt                   {get;set;}              //text Area
        public string RV_Cand_Edu_Prof_Qualification                {get;set;}              //text Area
        public string RV_Sub_Area_Develop_Weak                      {get;set;}              //text Area
        public string RV_Say_Abt_Enterpren_Skills                   {get;set;}              //text Area
        public string RV_Particiular_Gud_Least_Effect               {get;set;}              //text Area
        public string RV_Pos_Cand_Working                           {get;set;}              //text Area
        public string RV_TimePeriod_Cand_Work_Under                 {get;set;}              //text Area
        public string RV_Work_Ethnics_Moral_Intgrty                 {get;set;}              //text Area
        public string RV_Commnt_Per_Abilities                       {get;set;}              //text Area
        public string RV_Wrtn_Comm_Skills_RC                        {get;set;}              // drop down -Excellent / Outstanding / Good / Average / Poor
        public string RV_AnyIssues                                  {get;set;}              //Text Area
        public string RV_Sub_Meet_Pos_Perf_Req_RC                   {get;set;}              //Text Area
        public string RV_Sub_Comm_Interper_Skills                   {get;set;}              //Text Area
        public string RV_Sub_Effect_Gettng_Rslt                     {get;set;}              //Text Area
        public string RV_Intgrty_Commitmnt_RC                       {get;set;}              //Text Area
        public string RV_Is_Reliable_Hon_Person_RC                  {get;set;}              //Drop Down --Yes / No / Not disclosed 
        public string RV_Nature_Job                                 {get;set;}              // Text Area
        public string RV_Overall_Jb_Perf_RC                         {get;set; }              // Text Area
        public string RV_Social_Conduct                             { get; set; }           // Drop Down - 1,2,3,4,5
        public string RV_Social_Conduct_RC                          {get;set;}              // Text Area
        public string RV_Sub_Per_Prof_Weak                          {get;set;}              // Text Area
        public string RV_Work_Ethics_Moral_Intgrty_Rc               {get;set;}              //Drop Down -Excellent / Outstanding / Good / Average / Poor
        public string RV_Recommnd_Sub_Emp_Top_MNC                   {get;set;}              //Drop Down -Yes / No / Not disclosed 
        public string RV_Cmmnts_Sub_Strngth_Weak                    {get;set;}              //Text Area
        public string RV_Cmplnts_Rslt_Dsplnry_Sub_RC                {get;set;}              //Text Area
        public string RV_Contribution_Intro_Concept_RC              {get;set;}              //Text Area
        public string RV_Sub_Pos_Perf_Req_RC                        {get;set;}              //Text Area
        public string RV_Confrontation_Tackled                      {get;set;}              //Text Area
        public string RV_Pressure_Sit_Hndld                         {get;set;}              //Text Area
        public string RV_React_To_Change                            {get;set;}              //Text Area
        public string RV_Enforcing_Gud_Qualt_Work_Jrs               {get;set;}              //Text Area
        public string RV_Sub_Team_Leader                            {get;set;}              //Text Area
        public string RV_Any_Other_Cmmnts_Fdback                    {get;set;}              //Text Area
        public string RV_Exp_During_Financ_Dealing                  {get;set;}              //Text Area
        public string RV_Rel_Busines_Entity                         {get;set;}              //Text Area
        public string RV_Dur_Busines_Rel_Cmpny                      {get;set;}              // Text Area
        public string RV_OtherDetails6                              {get;set;}              // Text Filed
        public string RV_OtherDetails7                              {get;set;}              // Text Filed
        public string RV_OtherDetails8                              {get;set;}              // Text Filed
        public string RV_OtherDetails9                              {get;set;}              // Text Filed
        public string RV_OtherDetails10                             {get;set;}              // Text Filed
        public string RV_OtherDetails11                             {get;set;}              // Text Filed
        public string RV_OtherDetails12                             {get;set;}              // Text Filed
        public string RV_OtherDetails13                             {get;set;}              // Text Filed
        public string RV_OtherDetails14                             {get;set;}              // Text Filed
        public string RV_OtherDetails15                             {get;set;}              // Text Filed
        public string ATA_Reverse_Directory                         {get;set;}              // Text Area
        public string ATA_Residcl_Commrcl                           {get;set;}              // Text Area
        public string ATA_Stock_Exchnge                             {get;set;}              // Text Area
        public string ATA_Telphne_Directry_Srch                     {get;set;}              // Text Area
        public string ATA_Yellow_Pages                              {get;set;}              // Text Area
        public string ATA_Who_Domain                                {get;set;}              // Text Area
        public string ATA_GoogleSearch                              {get;set;}              // Text Area
        public string ATA_InfrastructureOfCmp                       {get;set;}              // Text Area
        public string ATA_Just_Dial                                 {get;set;}              // Text Area
        public string ATA_Neighbor_Check1                           {get;set;}              // Text Area
        public string ATA_NASCOM_Empanelment                        {get;set;}              // Text Area
        public string ATA_Photograph                                {get;set;}              // Text Area
        public string ATA_Physical_Site                             {get;set;}              // Text Area
        public string ATA_Reg_Company                               {get;set;}              // Text Area
        public string ATA_PO_Courier_Check                          {get;set;}              // Text Area
        public string ATA_Network_Sol                               {get;set;}              // Text Area
        public string ATA_ApprxEmpWorking                           {get;set;}              // Text Area
        public string ATA_CID_No                                    {get;set;}              // Text Filed
        public string ATA_Cmpny_Addr                                {get;set;}              // Text Filed
        public string ATA_Decoy_Call                                {get;set;}              // Text Area
        public string ATA_Cmpny_Website                             { get; set; }           // Text Area 

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
