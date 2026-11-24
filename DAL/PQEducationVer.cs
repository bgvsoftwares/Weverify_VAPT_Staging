using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class PQEducationVer
    {
        public PQEducationVer()
        {
           
            EducationVerRowID = 0;
            EV_Cand_Name                        = String.Empty;
            EV_Cand_Name_Supp_Edu_Doc           =String.Empty;
            EV_Colg_Schl_Inst_Name              =String.Empty;
            EV_Colg_Schl_Inst_Add_ConDet        =String.Empty;
            EV_Affl_University_Name             =String.Empty;
            EV_Degree_Name                      =String.Empty;
            EV_Mode_Of_Edu                      =String.Empty;
            EV_Enroll_RollNo                    =String.Empty;
            EV_RegNo_RegCode                    =String.Empty;
            EV_Dur_Course                       =String.Empty;
            EV_Course_Status                    =String.Empty;
            EV_Specialization                   =String.Empty;
            EV_Period_Course                    =String.Empty;
            EV_Yr_Passing                       =String.Empty;
            EV_Rslt_Obtain                      =String.Empty;
            EV_Mode_of_Verf                     =String.Empty;
            EV_Refre_Name_Dsg                   =String.Empty;
            EV_Refre_Eid                        =String.Empty;
            EV_Refre_ConNo                      =String.Empty;
            EV_Attch_Doc_Genuine                =String.Empty;
            EV_Any_Other_Cmmnts_EduV            =String.Empty;          
            EV_Other_Remarks                    =String.Empty;
            EV_Division_CGPA                    =String.Empty;
            EV_Official_Fee_Proof               =String.Empty;
            EV_Colg_Unvrsty_Affl                =String.Empty;
            EV_Others                           =String.Empty;
            EV_Others2                          =String.Empty;
            EV_Others3                          =String.Empty;
            EV_Others4                          =String.Empty;
            EV_Others5                          =String.Empty;
            EV_Others6                          =String.Empty;
            EV_Others7                          =String.Empty;
            EV_Others8                          =String.Empty;
            EV_Others9                          =String.Empty;
            EV_Others10                         =String.Empty;
            EV_Others11                         =String.Empty;
            EV_Others12                         =String.Empty;
            EV_Others13                         =String.Empty;
            EV_Others14                         =String.Empty;
            EV_Others15                         =String.Empty;
            ATA_Reverse_Directory               =String.Empty;
            ATA_Residcl_Commrcl                 =String.Empty;
            ATA_Stock_Exchnge                   =String.Empty;
            ATA_Telphne_Directry_Srch           =String.Empty;
            ATA_Yellow_Pages                    =String.Empty;
            ATA_Who_Domain                      =String.Empty;
            ATA_GoogleSearch                    =String.Empty;
            ATA_InfrastructureOfCmp             =String.Empty;
            ATA_Just_Dial                       =String.Empty;
            ATA_Neighbor_Check1                 =String.Empty;
            ATA_NASCOM_Empanelment              =String.Empty;
            ATA_Photograph                      =String.Empty;
            ATA_Physical_Site                   =String.Empty;
            ATA_Reg_Company                     =String.Empty;
            ATA_PO_Courier_Check                =String.Empty;
            ATA_Network_Sol                     =String.Empty;
            ATA_ApprxEmpWorking                 =String.Empty;
            ATA_CID_No                          =String.Empty;
            ATA_Cmpny_Addr                      =String.Empty;
            ATA_Decoy_Call                      =String.Empty;
            ATA_Cmpny_Website                  = String.Empty;

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

        public int            EducationVerRowID       { get; set; }

        public int EducationRowID                     { get; set; }
        public virtual PQEducation PQEducation        { get; set; }

        #region Education Verification Antecedents

        public  string   EV_Cand_Name                 { get; set; } //   Text Field
        public  string   EV_Cand_Name_Supp_Edu_Doc    { get; set; } //   Text Field
        public  string   EV_Colg_Schl_Inst_Name       { get; set; }  //    drop down -Auto populated from education master 
        public  string   EV_Colg_Schl_Inst_Add_ConDet { get; set; }  //    drop down -Auto populated from education master 
        public  string   EV_Affl_University_Name      { get; set; } //   drop down -University filter from education master 
        public  string   EV_Degree_Name               { get; set; } //   drop down
        public  string   EV_Mode_Of_Edu               { get; set; } //   drop down -Full Time / Part Time / Distance
        public  string   EV_Enroll_RollNo             { get; set; }  //    text field
        public  string   EV_RegNo_RegCode             { get; set; }  //    text filed
        public  string   EV_Dur_Course                { get; set; }  //    text file 
        public  string   EV_Course_Status             { get; set; } //   Drop Down -Completed / Pursuing / Result Awaited / Dropout / Back logs / Others
        public  string   EV_Specialization            { get; set; } //   Text Area
        public  string   EV_Period_Course             { get; set; } //   Date Range To & from calender  
        public  string   EV_Yr_Passing                { get; set; } //   Calender
        public  string   EV_Rslt_Obtain               { get; set; } //   Text Field
        public  string   EV_Mode_of_Verf              { get; set; } //   Drop Down -Verbal / Written / Other 
        public  string   EV_Refre_Name_Dsg            { get; set; } //   Text Area
        public  string   EV_Refre_Eid                 { get; set; } //   Text Field
        public  string   EV_Refre_ConNo               { get; set; } //   Text Field- Email id validation 
        public  string   EV_Attch_Doc_Genuine         { get; set; }  //    Text Filed
        public  string   EV_Any_Other_Cmmnts_EduV     { get; set; } //   Text Area
        public  DateTime? EV_Date_Time_Verf           { get; set; } //   Date & Time 
        public  DateTime? EV_Verb_Verf_Date           { get; set; } //   Date Calender
        public  DateTime? EV_Wrtn_Verf_Date           { get; set; } //   Date Calender
        public  string   EV_Other_Remarks             { get; set; } //   Text
        public  string   EV_Division_CGPA             { get; set; } //   Text Field
        public  string   EV_Official_Fee_Proof        { get; set; } //   Drop Down
        public  string   EV_Colg_Unvrsty_Affl         { get; set; } //   List Box
        public  string   EV_Others                    { get; set; }  //    text 
        public  string   EV_Others2                   { get; set; } //   text 
        public  string   EV_Others3                   { get; set; } //   text 
        public  string   EV_Others4                   { get; set; } //   text 
        public  string   EV_Others5                   { get; set; } //   text 
        public  string   EV_Others6                   { get; set; }  //    drop down 
        public  string   EV_Others7                   { get; set; } //   text 
        public  string   EV_Others8                   { get; set; } //   text 
        public  string   EV_Others9                   { get; set; }  //    drop down
        public  string   EV_Others10                  { get; set; } //   text 
        public  string   EV_Others11                  { get; set; } //   text 
        public  string   EV_Others12                  { get; set; } //   text 
        public  string   EV_Others13                  { get; set; } //   text 
        public  string   EV_Others14                  { get; set; } //   text 
        public  string   EV_Others15                  { get; set; }  //    drop down 
        public  string   ATA_Reverse_Directory        { get; set; } //   Text Area
        public  string   ATA_Residcl_Commrcl          { get; set; } //   Text Area
        public  string   ATA_Stock_Exchnge            { get; set; } //   Text Area
        public  string   ATA_Telphne_Directry_Srch    { get; set; } //   Text Area
        public  string   ATA_Yellow_Pages             { get; set; } //   Text Area
        public  string   ATA_Who_Domain               { get; set; } //   Text Area
        public  string   ATA_GoogleSearch             { get; set; } //   Text Area
        public  string   ATA_InfrastructureOfCmp      { get; set; } //   Text Area
        public  string   ATA_Just_Dial                { get; set; } //   Text Area
        public  string   ATA_Neighbor_Check1          { get; set; } //   Text Area
        public  string   ATA_NASCOM_Empanelment       { get; set; } //   Text Area
        public  string   ATA_Photograph               { get; set; } //   Text Area
        public  string   ATA_Physical_Site            { get; set; } //   Text Area
        public  string   ATA_Reg_Company              { get; set; } //   Text Area
        public  string   ATA_PO_Courier_Check         { get; set; } //   Text Area
        public  string   ATA_Network_Sol              { get; set; } //   Text Area
        public  string   ATA_ApprxEmpWorking          { get; set; } //   Text Area
        public  string   ATA_CID_No                   { get; set; } //   Text Area
        public  string   ATA_Cmpny_Addr               { get; set; } //   Text Area
        public  string   ATA_Decoy_Call               { get; set; } //   Text Filed
        public  string   ATA_Cmpny_Website            { get; set; }// Text Filed

        #endregion

        public  string   TypeRevert                   { get; set; }
        public  string   CheckStatus                  { get; set; }
        public  string   Severity                     { get; set; }
        public  string   ColorName                    { get; set; }
        public  string   ReportComments               { get; set; }
        public  byte     Status                       { get; set; }
                         
        public  short    VerifiedBy                   { get; set; }
        public  string   VerifierDesignation          { get; set; }
        public  string   VerifierContactNo            { get; set; }
        public  string   VerifierMobileNo             { get; set; }
        public  string   VerifierEmailId              { get; set; }
        public  DateTime? VerifiedDate                { get; set; }

    }
}
