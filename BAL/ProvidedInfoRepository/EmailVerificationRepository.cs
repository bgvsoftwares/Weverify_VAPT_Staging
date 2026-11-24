using DAL;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.ProvidedInfoViewModel;

namespace BAL.ProvidedInfoRepository
{
    public class EmailVerificationRepository : IEmailVerificationRepository
    {
        DataContext db;
        public EmailVerificationRepository()
        {
            db = new DataContext();
        }

        //public EducationEmailVerificationViewModel GetEducationEmailVerification(int EducationRowId = 0)
        //{
        //    EducationEmailVerificationViewModel model = new EducationEmailVerificationViewModel();
        //    if (EducationRowId > 0)
        //    {
        //        var Education = db.PQEducations.Find(EducationRowId);

        //        if (Education != null)
        //        {
        //            model.CandidateName = Education.EV_Cand_Name;
        //            if (Education.EV_Affl_University_Name>0)
        //            {
        //                model.UniversityName = db.MasterUniversities.Where(a => a.UniversityRowID == Education.EV_Affl_University_Name).FirstOrDefault().UniversityName;
        //            }
        //            if (Education.EV_Colg_Schl_Inst_Name > 0)
        //            {
        //                model.InstituteCollegeSchoolName = db.MasterColleges.Where(a => a.CollegeRowID == Education.EV_Colg_Schl_Inst_Name).FirstOrDefault().CollegeName;
        //            }
                        
        //            model.CourseName = Education.EV_Dur_Course;
        //            model.RollNoRegistrationNoSeatNo = Education.EV_Enroll_RollNo;
        //            model.YearOfPassing = Education.EV_Yr_Passing;
        //            model.Comments = Education.Remarks;
        //        }
        //    }
        //    return model;
        //}

        public EducationEmailVerificationViewModel GetEducationEmailVerification(int personalrowid = 0, int subcheckRowid = 0)
        {
            EducationEmailVerificationViewModel model = new EducationEmailVerificationViewModel();
            ClientCustomize cc = new ClientCustomize();
            if (personalrowid > 0)
            {


                DataTable DT = cc.getAutoEducationDetails(personalrowid, subcheckRowid);
                if (DT.Rows.Count > 0)
                {
                    model.UniversityName = DT.Rows[0]["UniversityName"].ToString();
                    model.InstituteCollegeSchoolName = DT.Rows[0]["CollegeName"].ToString();
                    model.CourseName = DT.Rows[0]["DegreeType"].ToString();
                    model.RollNoRegistrationNoSeatNo = DT.Rows[0]["RollNo"].ToString();
                    model.YearOfPassing = DT.Rows[0]["YearOfPassing"].ToString();
                    model.Comments= DT.Rows[0]["Comments"].ToString();                  

                }



            }
            return model;
        }

        //public EmploymentEmailVerificationViewModel GetEmploymentEmailVerification(int EmploymentRowId = 0)
        //{
        //    EmploymentEmailVerificationViewModel model = new EmploymentEmailVerificationViewModel();
        //    if (EmploymentRowId > 0)
        //    {
        //        var Employment = db.PQEmployments.Find(EmploymentRowId);
        //        int empId = Convert.ToInt32(Employment.EV_OrganisationName);
        //        if (Employment != null)
        //        {
        //            model.CompanyName = db.MasterEmployers.Where(a => a.EmployerRowID == empId).FirstOrDefault().CompanyName;
        //            model.CandidateName = Employment.EV_CandidateName;
        //            model.CompanyNameAddress = Employment.EV_CmpNameAddress;
        //            model.EmployeeCode = Employment.EV_EmployeeCode;
        //            model.PeriodOfEmployment = Employment.EV_EmploymentPeriod;
        //            model.Designation = Employment.EV_Designation;
        //            model.Remuneration = Employment.EV_last_sal;
        //            model.SupervisorNameDesignation = Employment.EV_Sup_Name_Dsg;
        //            model.Reasonforleaving = Employment.EV_leavingReason;
        //            model.IsExitFormalitiesCompleted = Employment.EV_FullFinalFormalities;
        //            model.DutiesResponsibilitiesHandled = Employment.EV_ResponsibilitiesHandled;
        //            model.AnyIntegrityDisciplinaryIssue = "";

        //            model.AdditionalComments = Employment.Remarks;
        //        }

        //    }
        //    return model;
        //}

        public EmploymentEmailVerificationViewModel GetEmploymentEmailVerification(int personalrowid = 0, int subcheckRowid = 0)
        {
            EmploymentEmailVerificationViewModel model = new EmploymentEmailVerificationViewModel();
            ClientCustomize cc = new ClientCustomize();
            if (personalrowid > 0)
            {
                DataTable DT=  cc.getAutoEmpDetail(personalrowid, subcheckRowid);
                if (DT.Rows.Count>0)
                {
                    model.CLientRowID = Convert.ToInt32(DT.Rows[0]["ClientRowID"].ToString());
                    model.CompanyName = DT.Rows[0]["Company Name"].ToString();
                    model.CandidateName = DT.Rows[0]["CandidateFullName"].ToString();
                    model.CompanyNameAddress = DT.Rows[0]["CompanyNameAddress"].ToString();
                    model.EmployeeCode = DT.Rows[0]["Employee Code"].ToString();
                    model.PeriodOfEmployment = DT.Rows[0]["Tenure"].ToString();
                    model.Designation = DT.Rows[0]["Designation"].ToString();
                    model.Remuneration = DT.Rows[0]["EV_CmpCost"].ToString();
                    model.SupervisorNameDesignation = DT.Rows[0]["Reporting Manager's Name & Designation"].ToString();
                    model.Reasonforleaving = DT.Rows[0]["Reason for leaving"].ToString();
                    model.IsExitFormalitiesCompleted = DT.Rows[0]["Full & Final Formalities"].ToString();
                    model.DutiesResponsibilitiesHandled = DT.Rows[0]["EV_HandledResponsibilities"].ToString();
                    model.AnyIntegrityDisciplinaryIssue = "";
                    model.AdditionalComments = DT.Rows[0]["Any Other Comments"].ToString();
                    model.EligibleForRehire = DT.Rows[0]["Eligibility for rehire"].ToString();
                }
            }
            return model;
        }
        public EducationEmailVerificationViewModel StopVendor(int PersonalRowId, int SubCheckRowID, int CheckStatusID, string AuditType)
        {
            EducationEmailVerificationViewModel model = new EducationEmailVerificationViewModel();
            ClientCustomize cc = new ClientCustomize();
            if (PersonalRowId > 0)
            {
                DataTable DT = cc.StopVendor(PersonalRowId, SubCheckRowID, CheckStatusID, AuditType);
                if (DT.Rows.Count > 0)
                {
                    model.CandidateName = DT.Rows[0]["CandidateFullName"].ToString();
                    model.CompanyRefNo = DT.Rows[0]["CompanyRefNo"].ToString();
                    model.EmailID = DT.Rows[0]["EmailID"].ToString();
                    model.To = DT.Rows[0]["To"].ToString();
                    model.From = DT.Rows[0]["From"].ToString();
                    model.Subject = DT.Rows[0]["Subject"].ToString();
                    model.Body = DT.Rows[0]["Body"].ToString();
                    model.cc = DT.Rows[0]["CC"].ToString();
                }
            }
            return model;
        }
    }
}
