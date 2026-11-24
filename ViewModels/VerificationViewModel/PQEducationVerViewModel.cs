using DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace ViewModels.VerificationViewModel
{
    public class PQEducationVerViewModel
    {
        [ScaffoldColumn(false)]
        public int EducationVerRowID { get; set; }

        [ScaffoldColumn(false)]
        public int EducationRowID { get; set; }

        [ScaffoldColumn(false)]
        public short ClientRowID { get; set; }

        public string ClientName { get; set; }

        [ScaffoldColumn(false)]
        public short SubCheckRowID { get; set; }

        public int CheckStatusId { get; set; }
        public bool Aging { get; set; }

        [ScaffoldColumn(false)]
        public int PersonalRowID { get; set; }
        public string PersonalName { get; set; }

        [ScaffoldColumn(false)]
        public string UniqueComponentID { get; set; }
        public string CandidateName { get; set; }
        public string CompanyReferenceNo { get; set; }
        public string SubReferenceID { get; set; }
        public string MobileNo { get; set; }
        public string AllocatedTMBy { get; set; }
        [AllowHtml]
        public string SpecialInstructions { get; set; }

        [ScaffoldColumn(false)]
        public short CheckFamilyRowID { get; set; }

        [ScaffoldColumn(false)]
        public int CollegeRowID { get; set; }

        [ScaffoldColumn(false)]
        public string CollegeName { get; set; }

        [ScaffoldColumn(false)]
        public short DegreeRowID { get; set; }
        public string DegreeName { get; set; }

        #region Education Verification Antecedents

        [MaxLength(100)]
        public string EV_Cand_Name { get; set; } //   Text Field

        [MaxLength(100)]
        public string EV_Cand_Name_Supp_Edu_Doc { get; set; } //   Text Field

        [MaxLength(100)]
        public string EV_Colg_Schl_Inst_Name { get; set; }  //    drop down -Auto populated from education master 
        public string ColgNameLabel { get; set; }
        public int EV_Colg_Schl_Inst_NameId { get; set; }
        public string UniversityNameLabel { get; set; }

        [MaxLength(100)]
        public string EV_Colg_Schl_Inst_Add_ConDet { get; set; }  //    drop down -Auto populated from education master 

        [MaxLength(100)]
        public string EV_Affl_University_Name { get; set; } //   drop down -University filter from education master 

        [MaxLength(100)]
        public string EV_Degree_Name { get; set; } //   drop down

        [MaxLength(100)]
        public string EV_Mode_Of_Edu { get; set; } //   drop down -Full Time / Part Time / Distance

        [MaxLength(100)]
        public string EV_Enroll_RollNo { get; set; }  //    text field

        [MaxLength(100)]
        public string EV_RegNo_RegCode { get; set; }  //    text filed

        [MaxLength(100)]
        public string EV_Dur_Course { get; set; }  //    text file 

        [MaxLength(100)]
        public string EV_Course_Status { get; set; } //   Drop Down -Completed / Pursuing / Result Awaited / Dropout / Back logs / Others

        [MaxLength(200)]
        public string EV_Specialization { get; set; } //   Text Area

        [MaxLength(100)]
        public string EV_Period_Course { get; set; } //   Date Range To & from calender  

        [MaxLength(100)]
        public string EV_Yr_Passing { get; set; } //   Calender

        [MaxLength(100)]
        public string EV_Rslt_Obtain { get; set; } //   Text Field

        [MaxLength(100)]
        public string EV_Mode_of_Verf { get; set; } //   Drop Down -Verbal / Written / Other 

        [MaxLength(100)]
        public string EV_Refre_Name_Dsg { get; set; } //   Text Area

        [MaxLength(100)]
        public string EV_Refre_Eid { get; set; } //   Text Field

        [MaxLength(100)]
        public string EV_Refre_ConNo { get; set; } //   Text Field- Email id validation 
        public string EV_Attch_Doc_Genuine { get; set; }  //    Text Filed

        [MaxLength(100)]
        public string EV_Any_Other_Cmmnts_EduV { get; set; } //   Text Area

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? EV_Date_Time_Verf { get; set; } //   Date & Time 

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? EV_Verb_Verf_Date { get; set; } //   Date Calender

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? EV_Wrtn_Verf_Date { get; set; } //   Date Calender

        [MaxLength(100)]
        public string EV_Other_Remarks { get; set; } //   Text

        [MaxLength(100)]
        public string EV_Division_CGPA { get; set; } //   Text Field
        public string EV_Official_Fee_Proof { get; set; } //   Drop Down

        [MaxLength(100)]
        public string EV_Colg_Unvrsty_Affl { get; set; } //   List Box

        [MaxLength(200)]
        public string EV_Others { get; set; }  //    text 

        [MaxLength(200)]
        public string EV_Others2 { get; set; } //   text 

        [MaxLength(1000)]
        public string EV_Others3 { get; set; } //   text 

        [MaxLength(200)]
        public string EV_Others4 { get; set; } //   text 

        [MaxLength(2000)]
        public string EV_Others5 { get; set; } //   text 

        [MaxLength(200)]
        public string EV_Others6 { get; set; }  //    drop down 

        [MaxLength(200)]
        public string EV_Others7 { get; set; } //   text 

        [MaxLength(200)]
        public string EV_Others8 { get; set; } //   text 

        [MaxLength(200)]
        public string EV_Others9 { get; set; }  //  drop down

        [MaxLength(200)]
        public string EV_Others10 { get; set; } //   text 

        [MaxLength(200)]
        public string EV_Others11 { get; set; } //   text 

        [MaxLength(200)]
        public string EV_Others12 { get; set; } //   text 

        [MaxLength(200)]
        public string EV_Others13 { get; set; } //   text 

        [MaxLength(200)]
        public string EV_Others14 { get; set; } //   text 

        [MaxLength()]
        public string EV_Others15 { get; set; }  //    drop down 

        [MaxLength(200)]
        public string ATA_Reverse_Directory { get; set; } //   Text Area

        [MaxLength(200)]
        public string ATA_Residcl_Commrcl { get; set; } //   Text Area

        [MaxLength(200)]
        public string ATA_Stock_Exchnge { get; set; } //   Text Area

        [MaxLength(200)]
        public string ATA_Telphne_Directry_Srch { get; set; } //   Text Area

        [MaxLength(200)]
        public string ATA_Yellow_Pages { get; set; } //   Text Area

        [MaxLength(200)]
        public string ATA_Who_Domain { get; set; } //   Text Area

        [MaxLength(200)]
        public string ATA_GoogleSearch { get; set; } //   Text Area

        [MaxLength(200)]
        public string ATA_InfrastructureOfCmp { get; set; } //   Text Area

        [MaxLength(200)]
        public string ATA_Just_Dial { get; set; } //   Text Area

        [MaxLength(200)]
        public string ATA_Neighbor_Check1 { get; set; } //   Text Area

        [MaxLength(200)]
        public string ATA_NASCOM_Empanelment { get; set; } //   Text Area

        [MaxLength(200)]
        public string ATA_Photograph { get; set; } //   Text Area

        [MaxLength(200)]
        public string ATA_Physical_Site { get; set; } //   Text Area

        [MaxLength(200)]
        public string ATA_Reg_Company { get; set; } //   Text Area

        [MaxLength(200)]
        public string ATA_PO_Courier_Check { get; set; } //   Text Area

        [MaxLength(200)]
        public string ATA_Network_Sol { get; set; } //   Text Area

        [MaxLength(200)]
        public string ATA_ApprxEmpWorking { get; set; } //   Text Area

        [MaxLength(200)]
        public string ATA_CID_No { get; set; } //   Text Area

        [MaxLength(200)]
        public string ATA_Cmpny_Addr { get; set; } //   Text Area

        [MaxLength(200)]
        public string ATA_Decoy_Call { get; set; } //   Text Filed

        [MaxLength(100)]
        public string ATA_Cmpny_Website { get; set; }// Text Filed

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
        public string VerifierDesignation { get; set; }
        public string VerifierContactNo { get; set; }
        public string VerifierMobileNo { get; set; }
        public string VerifierEmailId { get; set; }

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? VerifiedDate { get; set; }
        public string ApproveStatus { get; set; }

    }

    public class UpdatePQEducationVerViewModel
    {
        [ScaffoldColumn(false)]
        public int EducationVerRowID { get; set; }

        [ScaffoldColumn(false)]
        public int EducationRowID { get; set; }

        [ScaffoldColumn(false)]
        public short ClientRowID { get; set; }

        public int CheckStatusId { get; set; }

        public string ClientName { get; set; }

        [ScaffoldColumn(false)]
        public short SubCheckRowID { get; set; }

        [ScaffoldColumn(false)]
        public int PersonalRowID { get; set; }

        [ScaffoldColumn(false)]
        public short CheckFamilyRowID { get; set; }

        #region Education Verification Antecedents
        public int CollegeRowID { get; set; } //   Text Field
        public int DegreeRowID { get; set; } //   Text Field

        [MaxLength(100)]
        public string EV_Cand_Name { get; set; } //   Text Field

        [MaxLength(100)]
        public string EV_Cand_Name_Supp_Edu_Doc { get; set; } //   Text Field

        [MaxLength(100)]
        public string EV_Colg_Schl_Inst_Name { get; set; }  //    drop down -Auto populated from education master 

        public int EV_Colg_Schl_Inst_NameId { get; set; }

        [MaxLength(100)]
        public string EV_Colg_Schl_Inst_Add_ConDet { get; set; }  //    drop down -Auto populated from education master 

        [MaxLength(100)]
        public string EV_Affl_University_Name { get; set; } //   drop down -University filter from education master 

        [MaxLength(100)]
        public string EV_Degree_Name { get; set; } //   drop down

        [MaxLength(100)]
        public string EV_Mode_Of_Edu { get; set; } //   drop down -Full Time / Part Time / Distance

        [MaxLength(100)]
        public string EV_Enroll_RollNo { get; set; }  //    text field

        [MaxLength(100)]
        public string EV_RegNo_RegCode { get; set; }  //    text filed

        [MaxLength(100)]
        public string EV_Dur_Course { get; set; }  //    text file 

        [MaxLength(100)]
        public string EV_Course_Status { get; set; } //   Drop Down -Completed / Pursuing / Result Awaited / Dropout / Back logs / Others

        [MaxLength(200)]
        public string EV_Specialization { get; set; } //   Text Area

        [MaxLength(100)]
        public string EV_Period_Course { get; set; } //   Date Range To & from calender  

        [MaxLength(100)]
        public string EV_Yr_Passing { get; set; } //   Calender

        [MaxLength(100)]
        public string EV_Rslt_Obtain { get; set; } //   Text Field

        [MaxLength(100)]
        public string EV_Mode_of_Verf { get; set; } //   Drop Down -Verbal / Written / Other 

        [MaxLength(100)]
        public string EV_Refre_Name_Dsg { get; set; } //   Text Area

        [MaxLength(100)]
        public string EV_Refre_Eid { get; set; } //   Text Field

        [MaxLength(100)]
        public string EV_Refre_ConNo { get; set; } //   Text Field- Email id validation 
        public string EV_Attch_Doc_Genuine { get; set; }  //    Text Filed

        [MaxLength(100)]
        public string EV_Any_Other_Cmmnts_EduV { get; set; } //   Text Area

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? EV_Date_Time_Verf { get; set; } //   Date & Time 

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? EV_Verb_Verf_Date { get; set; } //   Date Calender

        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? EV_Wrtn_Verf_Date { get; set; } //   Date Calender

        [MaxLength(100)]
        public string EV_Other_Remarks { get; set; } //   Text

        [MaxLength(100)]
        public string EV_Division_CGPA { get; set; } //   Text Field
        public string EV_Official_Fee_Proof { get; set; } //   Drop Down

        [MaxLength(100)]
        public string EV_Colg_Unvrsty_Affl { get; set; } //   List Box

        [MaxLength(200)]
        public string EV_Others { get; set; }  //    text 

        [MaxLength(200)]
        public string EV_Others2 { get; set; } //   text 

        [MaxLength(1000)]
        public string EV_Others3 { get; set; } //   text 

        [MaxLength(200)]
        public string EV_Others4 { get; set; } //   text 

        [MaxLength(2000)]
        public string EV_Others5 { get; set; } //   text 

        [MaxLength(200)]
        public string EV_Others6 { get; set; }  //    drop down 

        [MaxLength(200)]
        public string EV_Others7 { get; set; } //   text 

        [MaxLength(200)]
        public string EV_Others8 { get; set; } //   text 

        [MaxLength(200)]
        public string EV_Others9 { get; set; }  //  drop down

        [MaxLength(200)]
        public string EV_Others10 { get; set; } //   text 

        [MaxLength(200)]
        public string EV_Others11 { get; set; } //   text 

        [MaxLength(200)]
        public string EV_Others12 { get; set; } //   text 

        [MaxLength(200)]
        public string EV_Others13 { get; set; } //   text 

        [MaxLength(200)]
        public string EV_Others14 { get; set; } //   text 

        [MaxLength()]
        public string EV_Others15 { get; set; }  //    drop down 

        [MaxLength(200)]
        public string ATA_Reverse_Directory { get; set; } //   Text Area

        [MaxLength(200)]
        public string ATA_Residcl_Commrcl { get; set; } //   Text Area

        [MaxLength(200)]
        public string ATA_Stock_Exchnge { get; set; } //   Text Area

        [MaxLength(200)]
        public string ATA_Telphne_Directry_Srch { get; set; } //   Text Area

        [MaxLength(200)]
        public string ATA_Yellow_Pages { get; set; } //   Text Area

        [MaxLength(200)]
        public string ATA_Who_Domain { get; set; } //   Text Area

        [MaxLength(200)]
        public string ATA_GoogleSearch { get; set; } //   Text Area

        [MaxLength(200)]
        public string ATA_InfrastructureOfCmp { get; set; } //   Text Area

        [MaxLength(200)]
        public string ATA_Just_Dial { get; set; } //   Text Area

        [MaxLength(200)]
        public string ATA_Neighbor_Check1 { get; set; } //   Text Area

        [MaxLength(200)]
        public string ATA_NASCOM_Empanelment { get; set; } //   Text Area

        [MaxLength(200)]
        public string ATA_Photograph { get; set; } //   Text Area

        [MaxLength(200)]
        public string ATA_Physical_Site { get; set; } //   Text Area

        [MaxLength(200)]
        public string ATA_Reg_Company { get; set; } //   Text Area

        [MaxLength(200)]
        public string ATA_PO_Courier_Check { get; set; } //   Text Area

        [MaxLength(200)]
        public string ATA_Network_Sol { get; set; } //   Text Area

        [MaxLength(200)]
        public string ATA_ApprxEmpWorking { get; set; } //   Text Area

        [MaxLength(200)]
        public string ATA_CID_No { get; set; } //   Text Area

        [MaxLength(200)]
        public string ATA_Cmpny_Addr { get; set; } //   Text Area

        [MaxLength(200)]
        public string ATA_Decoy_Call { get; set; } //   Text Filed

        [MaxLength(100)]
        public string ATA_Cmpny_Website { get; set; }// Text Filed

        #endregion
        
        [MaxLength(50)]
        public string CheckStatus { get; set; }

        #region Education Closer fields

        public EducationVerCloserViewModel CloserViewModel { get; set; }

        #endregion

        public bool Aging { get; set; }
        public decimal Amount { get; set; }

    }

    public class getLHSRHSdata
    {
        public int AntecedentRowID { get; set; }
        public string DateEntryQCValue { get; set; }
        public string OpsValue { get; set; }
    }
    public class EducationVerCloserViewModel
    {
        [ScaffoldColumn(false)]
        public int EducationVerRowID { get; set; }

        [ScaffoldColumn(false)]
        public int EducationRowID { get; set; }

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
        public string Remarks { get; set; }
        public string CheckStatus { get; set; }// check action and pqaddress update
        public string ActionHistory { get; set; }
        public string ShortRemarks { get; set; }


        public List<int> CDispositionRowId { get; set; }
    }
}
