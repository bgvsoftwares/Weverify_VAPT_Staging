using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class PQEducation
    {
        public PQEducation()
        {
            EducationRowId = 0;
            ClientRowID = 0;
            PersonalRowID = 0;
            ClientPackageRowID = 0;
            CheckFamilyRowID = 0;
            SubCheckRowID = 0;
            UniqueComponentID = string.Empty;
            EV_Cand_Name = string.Empty;
            EV_Sec_Ref_Id = string.Empty;
            EV_Cand_Name_Supp_Edu_Doc = string.Empty;
            EV_Colg_Schl_Inst_Name = 0;
            EV_Colg_Schl_Inst_Add_ConDet = string.Empty;
            EV_Affl_University_Name = 0;
            EV_Degree_Name = 0;
            EV_Mode_Of_Edu = string.Empty;
            EV_Enroll_RollNo = string.Empty;
            EV_RegNo_RegCode = string.Empty;
            EV_Dur_Course = string.Empty;
            EV_Course_Status = string.Empty;
            EV_Specialization = string.Empty;
            EV_Period_Course = string.Empty;
            EV_Yr_Passing = string.Empty;
            EV_Division_CGPA = string.Empty;
            EV_DegreeCertfict = 0;
            EV_ProvisionalCertfict = 0;
            EV_GradeSheet = 0;
            EV_AllMarksCards = 0;
            EV_DocStatColgNameNo = 0;
            EV_OtherProof = 0;
            EV_Others = string.Empty;
            EV_Others2 = string.Empty;
            EV_Others3 = string.Empty;
            EV_Others4 = string.Empty;
            EV_Others5 = string.Empty;
            EV_Others6 = string.Empty;
            EV_Others7 = string.Empty;
            EV_Others8 = string.Empty;
            EV_Others9 = string.Empty;
            EV_Others10 = string.Empty;
            ATA_CID_No = string.Empty;
            ATA_Cmpny_Addr = string.Empty;

            CreatedBy = 0;
            ModifiedBy = 0;
            Status = 1;

            CheckStatus = string.Empty;
            ReWorkCheckStatus = string.Empty;
            Remarks = string.Empty;
        }

        public int EducationRowId { get; set; }

        #region Common for all Checks 

        public short ClientRowID { get; set; }
        public virtual PQClientMaster PQClientMaster { get; set; }

        public int PersonalRowID { get; set; }
        public virtual PQPersonal PQPersonal { get; set; }

        public int ClientPackageRowID { get; set; }

        public short CheckFamilyRowID { get; set; }
        public virtual MasterCheckFamily MasterCheckFamily { get; set; }

        public short SubCheckRowID { get; set; }
        public virtual MasterSubCheckFamily MasterSubCheckFamily { get; set; }

        public string UniqueComponentID { get; set; }

        #endregion

        public string EV_Cand_Name { get; set; }        // Text Field
        public string EV_Sec_Ref_Id { get; set; }        // Text Field
        public string EV_Cand_Name_Supp_Edu_Doc { get; set; }        // Text Field
        public int EV_Colg_Schl_Inst_Name { get; set; }           //  drop down -Auto populated from Colleges master 
        public string EV_Colg_Schl_Inst_Add_ConDet { get; set; }         //  Text Area -Auto populated from Colleges master 
        public short EV_Affl_University_Name { get; set; }         //  drop down -University filter from Colleges master 
        public short EV_Degree_Name { get; set; }        //  drop down -Auto populated from DegreeTypes master 
        public string EV_Mode_Of_Edu { get; set; }        // drop down -Full Time / Part Time / Distance
        public string EV_Enroll_RollNo { get; set; }        // Text Filed 
        public string EV_RegNo_RegCode { get; set; }        // Text Filed 
        public string EV_Dur_Course { get; set; }        // Text Filed 
        public string EV_Course_Status { get; set; }        // Drop down -Completed / Pursuing / Result Awaited / Dropout / Back logs / Others
        public string EV_Specialization { get; set; }        // Text Area
        public string EV_Period_Course { get; set; }        // Date To & From Calender 
        public string EV_Yr_Passing { get; set; }           // Text Box
        public string EV_Division_CGPA { get; set; }        // Text Field
        public byte EV_DegreeCertfict { get; set; }   // CheckBox     Default - 0 otherwise 1
        public byte EV_ProvisionalCertfict { get; set; }   // CheckBox     Default - 0 otherwise 1
        public byte EV_GradeSheet { get; set; }   // CheckBox     Default - 0 otherwise 1
        public byte EV_AllMarksCards { get; set; }   // CheckBox     Default - 0 otherwise 1
        public byte EV_DocStatColgNameNo { get; set; }   // CheckBox     Default - 0 otherwise 1
        public byte EV_OtherProof { get; set; }                         // CheckBox     Default - 0 otherwise 1
        public string EV_Others { get; set; }        // Text Area
        public string EV_Others2 { get; set; }        // Text Area
        public string EV_Others3 { get; set; }        // Text Area
        public string EV_Others4 { get; set; }        // Text Area
        public string EV_Others5 { get; set; }        // Text Area
        public string EV_Others6 { get; set; }        // Text Area
        public string EV_Others7 { get; set; }        // Text Area
        public string EV_Others8 { get; set; }        // Text Area
        public string EV_Others9 { get; set; }        // Text Area
        public string EV_Others10 { get; set; }        // Text Area
        public string ATA_CID_No { get; set; }                          // Text Field
        public string ATA_Cmpny_Addr { get; set; }                      // Text Area

        public int CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? OutDate { get; set; }
        public DateTime? InternalOutDate { get; set; }
        public short ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public string CheckStatus { get; set; }
        public string ReWorkCheckStatus { get; set; }
        public string Remarks { get; set; }
        public byte Status { get; set; }

    }
}
