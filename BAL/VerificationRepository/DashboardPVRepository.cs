using DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.VerificationViewModel;
using System.Data;
namespace BAL.VerificationRepository
{
    public class DashboardPVRepository : IDashboardPVRepository
    {
        DataContext db;

        DateTime todayTATDate;
        DateTime totTodaysDue;
        DateTime totDueinNext2days;
        DateTime totInTAT4days;
        DateTime totInTAT7days;
        DateTime totBT1days;
        DateTime totBT2days;
        DateTime totBT5days;
        DateTime totBT10days;
        ClientCustomize cc;




        public DashboardPVRepository()
        {
            db = new DataContext();
            todayTATDate = DateTime.Now;
            totTodaysDue = todayTATDate;
            totDueinNext2days = todayTATDate.AddDays(2);
            totInTAT4days = todayTATDate.AddDays(4);
            totInTAT7days = todayTATDate.AddDays(7);
            totBT1days = todayTATDate.AddDays(-1);
            totBT2days = todayTATDate.AddDays(-2);
            totBT5days = todayTATDate.AddDays(-5);
            totBT10days = todayTATDate.AddDays(-10);
            cc = new ClientCustomize();
        }

        public DashboardEmpEduCriPVManagerViewModel GetPVManagerDashBoardInfo(short TeamMemberRowId, string Designation)
        {
            try
            {
                DashboardEmpEduCriPVManagerViewModel modelPV = new DashboardEmpEduCriPVManagerViewModel();

                #region ********** For PQEmploymentStatus **********

                IQueryable<PQEmploymentStatus> ChecksListEmp = db.PQEmploymentStatus.Where(a => a.PVMgrAllocatedTo == TeamMemberRowId);
                IQueryable<PQEmploymentStatus> ChecksListEmpDays = ChecksListEmp.Where(a => a.PVTLAllocatedTo == 0);
                DashboardPVManagerViewModel modelEmp = new DashboardPVManagerViewModel();
                DataTable dtEmp = new DataTable();

                dtEmp = cc.getdashboardPVManagerSummary("Manager-PV", TeamMemberRowId, "Employment");


                if (ChecksListEmp != null && ChecksListEmp.Count() > 0)
                {
                    modelEmp.totNonAllocated1 = Convert.ToInt32(dtEmp.Rows[0]["UnAllocated"].ToString());
                    modelEmp.totAllocated1 = Convert.ToInt32(dtEmp.Rows[0]["Allocated"].ToString());
                    modelEmp.totClosed1 = Convert.ToInt32(dtEmp.Rows[0]["Closed"].ToString());

                    modelEmp.totWIP = ChecksListEmp.ToList().Count;
                    modelEmp.totAllocated = ChecksListEmp.Where(t => t.PVTLAllocatedTo > 0).Count();
                    modelEmp.totNonAllocated = ChecksListEmpDays.Where(t => t.PVTLAllocatedTo == 0).Count();
                    modelEmp.totInitiated = 0;//ChecksListEmp.Where(t => t.PTRTLAllocatedTo > 0).Count();       //Parter Mgr Allocated To Mobile App User
                    modelEmp.totNonInitiated = 0;//ChecksListEmp.Where(t => t.PTRMgrAllocatedTo > 0).Count();   // Allocated Partner Manager
                    modelEmp.totRejectionWIP = ChecksListEmp.Where(t => t.PVTMCheckStatus == "WIP-Pending" && string.IsNullOrEmpty(t.PVTMQCCheckStatus)).Count();
                    modelEmp.totReOpen = 0;//ChecksListEmp.Where(t => t.PQEmployment.PQPersonal.CaseStatus == "ReOpen").Count();
                    modelEmp.totPriority = ChecksListEmp.Where(t => t.PQEmployment.PQPersonal.CaseType == "Priority").Count();
                    modelEmp.totInsuffWIP = 0;//ChecksListEmp.Where(t => t.PVTMQCCheckStatus == "Insufficent").Count();

                    modelEmp.totTodaysDue = ChecksListEmpDays.Where(t =>
                        DbFunctions.TruncateTime(t.PQEmployment.InternalOutDate) == DbFunctions.TruncateTime(todayTATDate)).ToList().Count;

                    modelEmp.totDueinNext2days = ChecksListEmpDays.Where(t =>
                    DbFunctions.TruncateTime(t.PQEmployment.InternalOutDate) > DbFunctions.TruncateTime(todayTATDate) &&
                      DbFunctions.TruncateTime(t.PQEmployment.InternalOutDate) <= DbFunctions.TruncateTime(totDueinNext2days)
                        ).ToList().Count;

                    modelEmp.totInTAT4to7days = ChecksListEmpDays.Where(t =>
                      DbFunctions.TruncateTime(t.PQEmployment.InternalOutDate) > DbFunctions.TruncateTime(totDueinNext2days) &&
                      DbFunctions.TruncateTime(t.PQEmployment.InternalOutDate) <= DbFunctions.TruncateTime(totInTAT7days)
                        ).ToList().Count;

                    modelEmp.totInTATGreater7days = ChecksListEmpDays.Where(t =>
                         DbFunctions.TruncateTime(t.PQEmployment.InternalOutDate) > DbFunctions.TruncateTime(totInTAT7days)
                        ).ToList().Count;

                    modelEmp.totBT1to2days = ChecksListEmpDays.Where(t =>
                  DbFunctions.TruncateTime(t.PQEmployment.InternalOutDate) <= DbFunctions.TruncateTime(totBT1days) &&
                    DbFunctions.TruncateTime(t.PQEmployment.InternalOutDate) >= DbFunctions.TruncateTime(totBT2days)
                        ).ToList().Count;

                    modelEmp.totBT3to5days = ChecksListEmpDays.Where(t =>
                DbFunctions.TruncateTime(t.PQEmployment.InternalOutDate) < DbFunctions.TruncateTime(totBT2days) &&
                      DbFunctions.TruncateTime(t.PQEmployment.InternalOutDate) >= DbFunctions.TruncateTime(totBT5days)
                        ).ToList().Count;

                    modelEmp.totBT5to10days = ChecksListEmpDays.Where(t =>
                DbFunctions.TruncateTime(t.PQEmployment.InternalOutDate) < DbFunctions.TruncateTime(totBT5days) &&
                      DbFunctions.TruncateTime(t.PQEmployment.InternalOutDate) >= DbFunctions.TruncateTime(totBT10days)
                        ).ToList().Count;

                    modelEmp.totBT10daysPlus = ChecksListEmpDays.Where(t =>
                    DbFunctions.TruncateTime(t.PQEmployment.InternalOutDate) < DbFunctions.TruncateTime(totBT10days)
                        ).ToList().Count;


                    modelEmp.totInsufficiency = ChecksListEmp.Where(t => t.TMQCCheckStatus == "Insufficent-QC").Count();
                    modelEmp.totPrecallingWIP = 0;
                    modelEmp.totPostcallingWIP = 0;
                    modelEmp.totQCPending = 0;//ChecksListEmp.Where(t => t.PVTMQCCheckStatus == "Allocated-TM").Count();
                    modelEmp.totFundApproval = 0;//ChecksListEmp.Where(t => t.PQEmployment.CheckStatus == "Funds Pending").Count();
                    modelEmp.totApprovalpending = 0;//ChecksListEmp.Where(t => t.MgrCheckStatus == "RW-Approval-Mgr" && t.PQEmployment.CheckStatus == "RW-Approval-Mgr" && t.MgrAllocatedTo > 0).Count();
                    modelEmp.totEscalated = 0;
                    modelEmp.totClosed = 0;//ChecksListEmp.Where(t => t.RWQCManagerBy > 0).Count();

                    modelEmp.totOutTAT = 0;
                }

                modelPV.ManagerDashboardEmp = modelEmp;

                #endregion

                #region ********** For PQEducationStatus **********

                IQueryable<PQEducationStatus> ChecksListEdu = db.PQEducationStatus.Where(a => a.PVMgrAllocatedTo == TeamMemberRowId);
                IQueryable<PQEducationStatus> ChecksListEduDays = ChecksListEdu.Where(a => a.PVTLAllocatedTo == 0);

                DashboardPVManagerViewModel modelEdu = new DashboardPVManagerViewModel();
                DataTable dtEdu = new DataTable();

                dtEdu = cc.getdashboardPVManagerSummaryEcu("Manager-PV", TeamMemberRowId, "Education");

                if (ChecksListEdu != null && ChecksListEdu.Count() > 0)
                {

                    modelEdu.totNonAllocated1 = Convert.ToInt32(dtEdu.Rows[0]["UnAllocated"].ToString());
                    modelEdu.totAllocated1 = Convert.ToInt32(dtEdu.Rows[0]["Allocated"].ToString());
                    modelEdu.totClosed1 = Convert.ToInt32(dtEdu.Rows[0]["Closed"].ToString());
                    modelEdu.totWIP = ChecksListEdu.ToList().Count;
                    modelEdu.totAllocated = ChecksListEdu.Where(t => t.PVTLAllocatedTo > 0).Count();
                    modelEdu.totNonAllocated = ChecksListEduDays.Count();
                    modelEdu.totInitiated = 0;//ChecksListEdu.Where(t => t.PTRTLAllocatedTo > 0).Count();       //Parter Mgr Allocated To Mobile App User
                    modelEdu.totNonInitiated = 0;//ChecksListEdu.Where(t => t.PTRMgrAllocatedTo > 0).Count();   // Allocated Partner Manager
                    modelEdu.totRejectionWIP = ChecksListEdu.Where(t => t.PVTMCheckStatus == "WIP-Pending" && string.IsNullOrEmpty(t.PVTMQCCheckStatus)).Count();
                    modelEdu.totReOpen = 0;//ChecksListEdu.Where(t => t.PQEducation.PQPersonal.CaseStatus == "ReOpen").Count();
                    modelEdu.totPriority = 0;//ChecksListEdu.Where(t => t.PQEducation.PQPersonal.CaseType == "Priority").Count();
                    modelEdu.totInsuffWIP = 0;// ChecksListEdu.Where(t => t.PVTMQCCheckStatus == "Insufficent").Count();

                    modelEdu.totTodaysDue = ChecksListEduDays.Where(t =>
                        DbFunctions.TruncateTime(t.PQEducation.InternalOutDate) == DbFunctions.TruncateTime(todayTATDate)).ToList().Count;

                    modelEdu.totDueinNext2days = ChecksListEduDays.Where(t =>
                    DbFunctions.TruncateTime(t.PQEducation.InternalOutDate) > DbFunctions.TruncateTime(todayTATDate) &&
                      DbFunctions.TruncateTime(t.PQEducation.InternalOutDate) <= DbFunctions.TruncateTime(totDueinNext2days)
                        ).ToList().Count;

                    modelEdu.totInTAT4to7days = ChecksListEduDays.Where(t =>
                      DbFunctions.TruncateTime(t.PQEducation.InternalOutDate) > DbFunctions.TruncateTime(totDueinNext2days) &&
                      DbFunctions.TruncateTime(t.PQEducation.InternalOutDate) <= DbFunctions.TruncateTime(totInTAT7days)
                        ).ToList().Count;

                    modelEdu.totInTATGreater7days = ChecksListEduDays.Where(t =>
                         DbFunctions.TruncateTime(t.PQEducation.InternalOutDate) > DbFunctions.TruncateTime(totInTAT7days)
                        ).ToList().Count;

                    modelEdu.totBT1to2days = ChecksListEduDays.Where(t =>
                  DbFunctions.TruncateTime(t.PQEducation.InternalOutDate) <= DbFunctions.TruncateTime(totBT1days) &&
                    DbFunctions.TruncateTime(t.PQEducation.InternalOutDate) >= DbFunctions.TruncateTime(totBT2days)
                        ).ToList().Count;

                    modelEdu.totBT3to5days = ChecksListEduDays.Where(t =>
                DbFunctions.TruncateTime(t.PQEducation.InternalOutDate) < DbFunctions.TruncateTime(totBT2days) &&
                      DbFunctions.TruncateTime(t.PQEducation.InternalOutDate) >= DbFunctions.TruncateTime(totBT5days)
                        ).ToList().Count;

                    modelEdu.totBT5to10days = ChecksListEduDays.Where(t =>
                DbFunctions.TruncateTime(t.PQEducation.InternalOutDate) < DbFunctions.TruncateTime(totBT5days) &&
                      DbFunctions.TruncateTime(t.PQEducation.InternalOutDate) >= DbFunctions.TruncateTime(totBT10days)
                        ).ToList().Count;

                    modelEdu.totBT10daysPlus = ChecksListEduDays.Where(t =>
                    DbFunctions.TruncateTime(t.PQEducation.InternalOutDate) < DbFunctions.TruncateTime(totBT10days)
                        ).ToList().Count;


                    modelEdu.totInsufficiency = ChecksListEdu.Where(t => t.TMQCCheckStatus == "Insufficent-QC").Count();
                    modelEdu.totPrecallingWIP = 0;
                    modelEdu.totPostcallingWIP = 0;
                    modelEdu.totQCPending = 0;//ChecksListEdu.Where(t => t.PVTMQCCheckStatus == "Allocated-TM").Count();
                    modelEdu.totFundApproval = 0;//ChecksListEdu.Where(t => t.PQEducation.CheckStatus == "Funds Pending").Count();
                    modelEdu.totApprovalpending = 0;// ChecksListEdu.Where(t => t.MgrCheckStatus == "RW-Approval-Mgr" && t.PQEducation.CheckStatus == "RW-Approval-Mgr" && t.MgrAllocatedTo > 0).Count();
                    modelEdu.totEscalated = 0;
                    modelEdu.totClosed = 0;//ChecksListEdu.Where(t => t.RWQCManagerBy > 0).Count();

                    modelEdu.totOutTAT = 0;
                }

                modelPV.ManagerDashboardEdu = modelEdu;

                #endregion

                #region ********** For PQCriminalStatus **********

                IQueryable<PQCriminalStatus> ChecksListCri = db.PQCriminalStatus.Where(a => a.PVMgrAllocatedTo == TeamMemberRowId);
                IQueryable<PQCriminalStatus> ChecksListCriDays = ChecksListCri.Where(a => a.PVTLAllocatedTo == 0);

                DashboardPVManagerViewModel modelCri = new DashboardPVManagerViewModel();
                DataTable dtCri = new DataTable();

                dtCri = cc.getdashboardPVManagerSummaryCri("Manager-PV", TeamMemberRowId, "Criminal");
                if (ChecksListCri != null && ChecksListCri.Count() > 0)
                {
                    modelCri.totNonAllocated1 = Convert.ToInt32(dtCri.Rows[0]["UnAllocated"].ToString());
                    modelCri.totAllocated1 = Convert.ToInt32(dtCri.Rows[0]["Allocated"].ToString());
                    modelCri.totClosed1 = Convert.ToInt32(dtCri.Rows[0]["Closed"].ToString());
                    modelCri.totWIP = ChecksListCri.ToList().Count;
                    modelCri.totAllocated = ChecksListCri.Where(t => t.PVTLAllocatedTo > 0).Count();
                    modelCri.totNonAllocated = ChecksListCriDays.Count();
                    modelCri.totInitiated = 0;// ChecksListCri.Where(t => t.PTRTLAllocatedTo > 0).Count();       //Parter Mgr Allocated To Mobile App User
                    modelCri.totNonInitiated = 0;// ChecksListCri.Where(t => t.PTRMgrAllocatedTo > 0).Count();   // Allocated Partner Manager
                    modelCri.totRejectionWIP = ChecksListCri.Where(t => t.PVTMCheckStatus == "WIP-Pending" && string.IsNullOrEmpty(t.PVTMQCCheckStatus)).Count();
                    modelCri.totReOpen = 0;//ChecksListCri.Where(t => t.PQCriminal.PQPersonal.CaseStatus == "ReOpen").Count();
                    modelCri.totPriority = ChecksListCri.Where(t => t.PQCriminal.PQPersonal.CaseType == "Priority").Count();
                    modelCri.totInsuffWIP = 0;//ChecksListCri.Where(t => t.PVTMQCCheckStatus == "Insufficent").Count();

                    modelCri.totTodaysDue = ChecksListCriDays.Where(t =>
                        DbFunctions.TruncateTime(t.PQCriminal.InternalOutDate) == DbFunctions.TruncateTime(todayTATDate)).ToList().Count;

                    modelCri.totDueinNext2days = ChecksListCriDays.Where(t =>
                    DbFunctions.TruncateTime(t.PQCriminal.InternalOutDate) > DbFunctions.TruncateTime(todayTATDate) &&
                      DbFunctions.TruncateTime(t.PQCriminal.InternalOutDate) <= DbFunctions.TruncateTime(totDueinNext2days)
                        ).ToList().Count;

                    modelCri.totInTAT4to7days = ChecksListCriDays.Where(t =>
                      DbFunctions.TruncateTime(t.PQCriminal.InternalOutDate) > DbFunctions.TruncateTime(totDueinNext2days) &&
                      DbFunctions.TruncateTime(t.PQCriminal.InternalOutDate) <= DbFunctions.TruncateTime(totInTAT7days)
                        ).ToList().Count;

                    modelCri.totInTATGreater7days = ChecksListCriDays.Where(t =>
                         DbFunctions.TruncateTime(t.PQCriminal.InternalOutDate) > DbFunctions.TruncateTime(totInTAT7days)
                        ).ToList().Count;

                    modelCri.totBT1to2days = ChecksListCriDays.Where(t =>
                  DbFunctions.TruncateTime(t.PQCriminal.InternalOutDate) <= DbFunctions.TruncateTime(totBT1days) &&
                    DbFunctions.TruncateTime(t.PQCriminal.InternalOutDate) >= DbFunctions.TruncateTime(totBT2days)
                        ).ToList().Count;

                    modelCri.totBT3to5days = ChecksListCriDays.Where(t =>
                DbFunctions.TruncateTime(t.PQCriminal.InternalOutDate) < DbFunctions.TruncateTime(totBT2days) &&
                      DbFunctions.TruncateTime(t.PQCriminal.InternalOutDate) >= DbFunctions.TruncateTime(totBT5days)
                        ).ToList().Count;

                    modelCri.totBT5to10days = ChecksListCriDays.Where(t =>
                DbFunctions.TruncateTime(t.PQCriminal.InternalOutDate) < DbFunctions.TruncateTime(totBT5days) &&
                      DbFunctions.TruncateTime(t.PQCriminal.InternalOutDate) >= DbFunctions.TruncateTime(totBT10days)
                        ).ToList().Count;

                    modelCri.totBT10daysPlus = ChecksListCriDays.Where(t =>
                    DbFunctions.TruncateTime(t.PQCriminal.InternalOutDate) < DbFunctions.TruncateTime(totBT10days)
                        ).ToList().Count;


                    modelCri.totInsufficiency = ChecksListCri.Where(t => t.TMQCCheckStatus == "Insufficent-QC").Count();
                    modelCri.totPrecallingWIP = 0;
                    modelCri.totPostcallingWIP = 0;
                    modelCri.totQCPending = 0;// ChecksListCri.Where(t => t.PVTMQCCheckStatus == "Allocated-TM").Count();
                    modelCri.totFundApproval = 0;//ChecksListCri.Where(t => t.PQCriminal.CheckStatus == "Funds Pending").Count();
                    modelCri.totApprovalpending = 0;//ChecksListCri.Where(t => t.MgrCheckStatus == "RW-Approval-Mgr" && t.PQCriminal.CheckStatus == "RW-Approval-Mgr" && t.MgrAllocatedTo > 0).Count();
                    modelCri.totEscalated = 0;
                    modelCri.totClosed = 0;//ChecksListCri.Where(t => t.RWQCManagerBy > 0).Count();

                    modelCri.totOutTAT = 0;
                }

                modelPV.ManagerDashboardCri = modelCri;

                #endregion

                return modelPV;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DashboardEmpEduCriPVTLViewModel GetPVTeamLeaderDashBoardInfo(short TeamMemberRowId, string Designation)
        {
            try
            {
                DashboardEmpEduCriPVTLViewModel modelPV = new DashboardEmpEduCriPVTLViewModel();

                #region ********** For PQEmploymentStatus **********
                IQueryable<PQEmploymentStatus> ChecksListEmp = db.PQEmploymentStatus.Where(a => a.PVTLAllocatedTo == TeamMemberRowId);
                IQueryable<PQEmploymentStatus> ChecksListEmpDays = ChecksListEmp.Where(a => a.PVTMAllocatedTo == 0 && a.PVTMQCAllocatedTo == 0);

                DashboardPVTeamLeaderViewModel modelEmp = new DashboardPVTeamLeaderViewModel();

                if (ChecksListEmp != null && ChecksListEmp.Count() > 0)
                {
                    DataTable dt = new DataTable();

                    dt = cc.getdashboardPVManagerSummary("Team Leader-PV", TeamMemberRowId, "Employment");

                    modelEmp.totNonAllocated1 = Convert.ToInt32(dt.Rows[0]["UnAllocated"].ToString());
                    modelEmp.totAllocated1 = Convert.ToInt32(dt.Rows[0]["Allocated"].ToString());
                    modelEmp.totClosed1 = Convert.ToInt32(dt.Rows[0]["Closed"].ToString());
                    modelEmp.totWIP = ChecksListEmp.ToList().Count;
                    modelEmp.totAllocated = ChecksListEmp.Where(t => t.PVTLAllocatedTo > 0).Count();
                    modelEmp.totNonAllocated = ChecksListEmpDays.Count();
                    modelEmp.totInitiated = ChecksListEmp.Where(t => t.PTRTLAllocatedTo > 0).Count();       //Parter Mgr Allocated To Mobile App User
                    modelEmp.totNonInitiated = ChecksListEmp.Where(t => t.PTRMgrAllocatedTo > 0).Count();   // Allocated Partner Manager
                    modelEmp.totRejectionWIP = ChecksListEmp.Where(t => t.PVTMQCCheckStatus == "Rejected").Count();
                    modelEmp.totReOpen = ChecksListEmp.Where(t => t.PQEmployment.PQPersonal.CaseStatus == "ReOpen").Count();
                    modelEmp.totPriority = ChecksListEmp.Where(t => t.PQEmployment.PQPersonal.CaseType == "Priority").Count();
                    modelEmp.totInsuffWIP = ChecksListEmp.Where(t => t.PVTMQCCheckStatus == "Insufficent").Count();

                    modelEmp.totTodaysDue = ChecksListEmpDays.Where(t =>
                        DbFunctions.TruncateTime(t.PQEmployment.InternalOutDate) == DbFunctions.TruncateTime(todayTATDate)).ToList().Count;

                    modelEmp.totDueinNext2days = ChecksListEmpDays.Where(t =>
                    DbFunctions.TruncateTime(t.PQEmployment.InternalOutDate) > DbFunctions.TruncateTime(todayTATDate) &&
                      DbFunctions.TruncateTime(t.PQEmployment.InternalOutDate) <= DbFunctions.TruncateTime(totDueinNext2days)
                        ).ToList().Count;

                    modelEmp.totInTAT4to7days = ChecksListEmpDays.Where(t =>
                      DbFunctions.TruncateTime(t.PQEmployment.InternalOutDate) > DbFunctions.TruncateTime(totDueinNext2days) &&
                      DbFunctions.TruncateTime(t.PQEmployment.InternalOutDate) <= DbFunctions.TruncateTime(totInTAT7days)
                        ).ToList().Count;

                    modelEmp.totInTATGreater7days = ChecksListEmpDays.Where(t =>
                         DbFunctions.TruncateTime(t.PQEmployment.InternalOutDate) > DbFunctions.TruncateTime(totInTAT7days)
                        ).ToList().Count;

                    modelEmp.totBT1to2days = ChecksListEmpDays.Where(t =>
                  DbFunctions.TruncateTime(t.PQEmployment.InternalOutDate) <= DbFunctions.TruncateTime(totBT1days) &&
                    DbFunctions.TruncateTime(t.PQEmployment.InternalOutDate) >= DbFunctions.TruncateTime(totBT2days)
                        ).ToList().Count;

                    modelEmp.totBT3to5days = ChecksListEmpDays.Where(t =>
                DbFunctions.TruncateTime(t.PQEmployment.InternalOutDate) < DbFunctions.TruncateTime(totBT2days) &&
                      DbFunctions.TruncateTime(t.PQEmployment.InternalOutDate) >= DbFunctions.TruncateTime(totBT5days)
                        ).ToList().Count;

                    modelEmp.totBT5to10days = ChecksListEmpDays.Where(t =>
                DbFunctions.TruncateTime(t.PQEmployment.InternalOutDate) < DbFunctions.TruncateTime(totBT5days) &&
                      DbFunctions.TruncateTime(t.PQEmployment.InternalOutDate) >= DbFunctions.TruncateTime(totBT10days)
                        ).ToList().Count;

                    modelEmp.totBT10daysPlus = ChecksListEmpDays.Where(t =>
                    DbFunctions.TruncateTime(t.PQEmployment.InternalOutDate) < DbFunctions.TruncateTime(totBT10days)
                        ).ToList().Count;


                    modelEmp.totInsufficiency = ChecksListEmp.Where(t => t.PVTMQCCheckStatus == "Insufficent-QC").Count();
                    modelEmp.totPrecallingWIP = 0;
                    modelEmp.totPostcallingWIP = 0;
                    modelEmp.totQCPending = ChecksListEmp.Where(t => t.PVTMQCCheckStatus == "Allocated-TM").Count();
                    modelEmp.totFundApproval = ChecksListEmp.Where(t => t.PQEmployment.CheckStatus == "Funds Pending").Count();
                    modelEmp.totApprovalpending = ChecksListEmp.Where(t => t.MgrCheckStatus == "RW-Approval-Mgr" && t.PQEmployment.CheckStatus == "RW-Approval-Mgr" && t.MgrAllocatedTo > 0).Count();
                    modelEmp.totEscalated = 0;
                    modelEmp.totClosed = ChecksListEmp.Where(t => t.RWQCManagerBy > 0).Count();

                    modelEmp.totOutTAT = 0;
                }
                modelPV.TeamLeaderDashboardEmp = modelEmp;

                #endregion

                #region ********** For PQEducationStatus **********

                IQueryable<PQEducationStatus> ChecksListEdu = db.PQEducationStatus.Where(a => a.PVTLAllocatedTo == TeamMemberRowId);
                IQueryable<PQEducationStatus> ChecksListEduDays = ChecksListEdu.Where(a => a.PVTMAllocatedTo == 0 && a.PVTMQCAllocatedTo == 0);

                DashboardPVTeamLeaderViewModel modelEdu = new DashboardPVTeamLeaderViewModel();

                if (ChecksListEdu != null && ChecksListEdu.Count() > 0)
                {
                    DataTable dtEdu = new DataTable();

                    dtEdu = cc.getdashboardPVManagerSummaryEcu("Team Leader-PV", TeamMemberRowId, "Education");

                    modelEdu.totNonAllocated1 = Convert.ToInt32(dtEdu.Rows[0]["UnAllocated"].ToString());
                    modelEdu.totAllocated1 = Convert.ToInt32(dtEdu.Rows[0]["Allocated"].ToString());
                    modelEdu.totClosed1 = Convert.ToInt32(dtEdu.Rows[0]["Closed"].ToString());
                    modelEdu.totWIP = ChecksListEdu.ToList().Count;
                    modelEdu.totAllocated = ChecksListEdu.Where(t => t.PVTLAllocatedTo > 0).Count();
                    modelEdu.totNonAllocated = ChecksListEdu.Where(a => a.PVTMAllocatedTo == 0 && a.PVTMQCAllocatedTo == 0).Count();
                    modelEdu.totInitiated = ChecksListEdu.Where(t => t.PTRTLAllocatedTo > 0).Count();       //Parter Mgr Allocated To Mobile App User
                    modelEdu.totNonInitiated = ChecksListEdu.Where(t => t.PTRMgrAllocatedTo > 0).Count();   // Allocated Partner Manager
                    modelEdu.totRejectionWIP = ChecksListEdu.Where(t => t.PVTMQCCheckStatus == "Rejected").Count();
                    modelEdu.totReOpen = ChecksListEdu.Where(t => t.PQEducation.PQPersonal.CaseStatus == "ReOpen").Count();
                    modelEdu.totPriority = ChecksListEdu.Where(t => t.PQEducation.PQPersonal.CaseType == "Priority").Count();
                    modelEdu.totInsuffWIP = ChecksListEdu.Where(t => t.PVTMQCCheckStatus == "Insufficent").Count();

                    modelEdu.totTodaysDue = ChecksListEduDays.Where(t =>
                        DbFunctions.TruncateTime(t.PQEducation.InternalOutDate) == DbFunctions.TruncateTime(todayTATDate)).ToList().Count;

                    modelEdu.totDueinNext2days = ChecksListEduDays.Where(t =>
                    DbFunctions.TruncateTime(t.PQEducation.InternalOutDate) > DbFunctions.TruncateTime(todayTATDate) &&
                      DbFunctions.TruncateTime(t.PQEducation.InternalOutDate) <= DbFunctions.TruncateTime(totDueinNext2days)
                        ).ToList().Count;

                    modelEdu.totInTAT4to7days = ChecksListEduDays.Where(t =>
                      DbFunctions.TruncateTime(t.PQEducation.InternalOutDate) > DbFunctions.TruncateTime(totDueinNext2days) &&
                      DbFunctions.TruncateTime(t.PQEducation.InternalOutDate) <= DbFunctions.TruncateTime(totInTAT7days)
                        ).ToList().Count;

                    modelEdu.totInTATGreater7days = ChecksListEduDays.Where(t =>
                         DbFunctions.TruncateTime(t.PQEducation.InternalOutDate) > DbFunctions.TruncateTime(totInTAT7days)
                        ).ToList().Count;

                    modelEdu.totBT1to2days = ChecksListEduDays.Where(t =>
                  DbFunctions.TruncateTime(t.PQEducation.InternalOutDate) <= DbFunctions.TruncateTime(totBT1days) &&
                    DbFunctions.TruncateTime(t.PQEducation.InternalOutDate) >= DbFunctions.TruncateTime(totBT2days)
                        ).ToList().Count;

                    modelEdu.totBT3to5days = ChecksListEduDays.Where(t =>
                DbFunctions.TruncateTime(t.PQEducation.InternalOutDate) < DbFunctions.TruncateTime(totBT2days) &&
                      DbFunctions.TruncateTime(t.PQEducation.InternalOutDate) >= DbFunctions.TruncateTime(totBT5days)
                        ).ToList().Count;

                    modelEdu.totBT5to10days = ChecksListEduDays.Where(t =>
                DbFunctions.TruncateTime(t.PQEducation.InternalOutDate) < DbFunctions.TruncateTime(totBT5days) &&
                      DbFunctions.TruncateTime(t.PQEducation.InternalOutDate) >= DbFunctions.TruncateTime(totBT10days)
                        ).ToList().Count;

                    modelEdu.totBT10daysPlus = ChecksListEduDays.Where(t =>
                    DbFunctions.TruncateTime(t.PQEducation.InternalOutDate) < DbFunctions.TruncateTime(totBT10days)
                        ).ToList().Count;


                    modelEdu.totInsufficiency = ChecksListEdu.Where(t => t.PVTMQCCheckStatus == "Insufficent-QC").Count();
                    modelEdu.totPrecallingWIP = 0;
                    modelEdu.totPostcallingWIP = 0;
                    modelEdu.totQCPending = ChecksListEdu.Where(t => t.PVTMQCCheckStatus == "Allocated-TM").Count();
                    modelEdu.totFundApproval = ChecksListEdu.Where(t => t.PQEducation.CheckStatus == "Funds Pending").Count();
                    modelEdu.totApprovalpending = ChecksListEdu.Where(t => t.MgrCheckStatus == "RW-Approval-Mgr" && t.PQEducation.CheckStatus == "RW-Approval-Mgr" && t.MgrAllocatedTo > 0).Count();
                    modelEdu.totEscalated = 0;
                    modelEdu.totClosed = ChecksListEdu.Where(t => t.RWQCManagerBy > 0).Count();

                    modelEdu.totOutTAT = 0;
                }


                modelPV.TeamLeaderDashboardEdu = modelEdu;

                #endregion

                #region ********** For PQCriminalStatus **********
                IQueryable<PQCriminalStatus> ChecksListCri = db.PQCriminalStatus.Where(a => a.PVTLAllocatedTo == TeamMemberRowId);
                IQueryable<PQCriminalStatus> ChecksListCriDays = ChecksListCri.Where(a => a.PVTMAllocatedTo == 0 && a.PVTMQCAllocatedTo == 0);

                DashboardPVTeamLeaderViewModel modelCri = new DashboardPVTeamLeaderViewModel();

                if (ChecksListCri != null && ChecksListCri.Count() > 0)
                {
                    DataTable dtCri = new DataTable();

                    dtCri = cc.getdashboardPVManagerSummaryCri("Team Leader-PV", TeamMemberRowId, "Criminal");

                    modelCri.totNonAllocated1 = Convert.ToInt32(dtCri.Rows[0]["UnAllocated"].ToString());
                    modelCri.totAllocated1 = Convert.ToInt32(dtCri.Rows[0]["Allocated"].ToString());
                    modelCri.totClosed1 = Convert.ToInt32(dtCri.Rows[0]["Closed"].ToString());
                    modelCri.totWIP = ChecksListCri.ToList().Count;
                    modelCri.totAllocated = ChecksListCri.Where(t => t.PVTLAllocatedTo > 0).Count();
                    modelCri.totNonAllocated = ChecksListCriDays.Count();
                    modelCri.totInitiated = ChecksListCri.Where(t => t.PTRTLAllocatedTo > 0).Count();       //Parter Mgr Allocated To Mobile App User
                    modelCri.totNonInitiated = ChecksListCri.Where(t => t.PTRMgrAllocatedTo > 0).Count();   // Allocated Partner Manager
                    modelCri.totRejectionWIP = ChecksListCri.Where(t => t.PVTMQCCheckStatus == "Rejected").Count();
                    modelCri.totReOpen = ChecksListCri.Where(t => t.PQCriminal.PQPersonal.CaseStatus == "ReOpen").Count();
                    modelCri.totPriority = ChecksListCri.Where(t => t.PQCriminal.PQPersonal.CaseType == "Priority").Count();
                    modelCri.totInsuffWIP = ChecksListCri.Where(t => t.PVTMQCCheckStatus == "Insufficent").Count();

                    modelCri.totTodaysDue = ChecksListCriDays.Where(t =>
                        DbFunctions.TruncateTime(t.PQCriminal.InternalOutDate) == DbFunctions.TruncateTime(todayTATDate)).ToList().Count;

                    modelCri.totDueinNext2days = ChecksListCriDays.Where(t =>
                    DbFunctions.TruncateTime(t.PQCriminal.InternalOutDate) > DbFunctions.TruncateTime(todayTATDate) &&
                      DbFunctions.TruncateTime(t.PQCriminal.InternalOutDate) <= DbFunctions.TruncateTime(totDueinNext2days)
                        ).ToList().Count;

                    modelCri.totInTAT4to7days = ChecksListCriDays.Where(t =>
                      DbFunctions.TruncateTime(t.PQCriminal.InternalOutDate) > DbFunctions.TruncateTime(totDueinNext2days) &&
                      DbFunctions.TruncateTime(t.PQCriminal.InternalOutDate) <= DbFunctions.TruncateTime(totInTAT7days)
                        ).ToList().Count;

                    modelCri.totInTATGreater7days = ChecksListCriDays.Where(t =>
                         DbFunctions.TruncateTime(t.PQCriminal.InternalOutDate) > DbFunctions.TruncateTime(totInTAT7days)
                        ).ToList().Count;

                    modelCri.totBT1to2days = ChecksListCriDays.Where(t =>
                  DbFunctions.TruncateTime(t.PQCriminal.InternalOutDate) <= DbFunctions.TruncateTime(totBT1days) &&
                    DbFunctions.TruncateTime(t.PQCriminal.InternalOutDate) >= DbFunctions.TruncateTime(totBT2days)
                        ).ToList().Count;

                    modelCri.totBT3to5days = ChecksListCriDays.Where(t =>
                DbFunctions.TruncateTime(t.PQCriminal.InternalOutDate) < DbFunctions.TruncateTime(totBT2days) &&
                      DbFunctions.TruncateTime(t.PQCriminal.InternalOutDate) >= DbFunctions.TruncateTime(totBT5days)
                        ).ToList().Count;

                    modelCri.totBT5to10days = ChecksListCriDays.Where(t =>
                DbFunctions.TruncateTime(t.PQCriminal.InternalOutDate) < DbFunctions.TruncateTime(totBT5days) &&
                      DbFunctions.TruncateTime(t.PQCriminal.InternalOutDate) >= DbFunctions.TruncateTime(totBT10days)
                        ).ToList().Count;

                    modelCri.totBT10daysPlus = ChecksListCriDays.Where(t =>
                    DbFunctions.TruncateTime(t.PQCriminal.InternalOutDate) < DbFunctions.TruncateTime(totBT10days)
                        ).ToList().Count;


                    modelCri.totInsufficiency = ChecksListCri.Where(t => t.PVTMQCCheckStatus == "Insufficent-QC").Count();
                    modelCri.totPrecallingWIP = 0;
                    modelCri.totPostcallingWIP = 0;
                    modelCri.totQCPending = ChecksListCri.Where(t => t.PVTMQCCheckStatus == "Allocated-TM").Count();
                    modelCri.totFundApproval = ChecksListCri.Where(t => t.PQCriminal.CheckStatus == "Funds Pending").Count();
                    modelCri.totApprovalpending = ChecksListCri.Where(t => t.MgrCheckStatus == "RW-Approval-Mgr" && t.PQCriminal.CheckStatus == "RW-Approval-Mgr" && t.MgrAllocatedTo > 0).Count();
                    modelCri.totEscalated = 0;
                    modelCri.totClosed = ChecksListCri.Where(t => t.RWQCManagerBy > 0).Count();

                    modelCri.totOutTAT = 0;
                }

                modelPV.TeamLeaderDashboardCri = modelCri;

                #endregion

                return modelPV;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DashboardEmpEduCriPVTMViewModel GetPVTeamMemberDashBoardInfo(short TeamMemberRowId, string Designation)
        {
            try
            {
                todayTATDate = DateTime.Now;
                totDueinNext2days = todayTATDate.AddDays(2);
                totInTAT4days = todayTATDate.AddDays(4);
                totInTAT7days = todayTATDate.AddDays(7);
                totBT1days = todayTATDate.AddDays(-1);
                totBT2days = todayTATDate.AddDays(-2);
                totBT5days = todayTATDate.AddDays(-5);
                DashboardEmpEduCriPVTMViewModel modelPV = new DashboardEmpEduCriPVTMViewModel();

                #region ********** For PQEmploymentStatus **********
                IQueryable<PQEmploymentStatus> ChecksListEmp = db.PQEmploymentStatus.Where(a => a.PVTMAllocatedTo == TeamMemberRowId);
                IQueryable<PQEmploymentStatus> ChecksListEmpDays = ChecksListEmp.Where(a => a.PVTMAllocatedTo == TeamMemberRowId && a.PTRMgrAllocatedTo == 0);

                DashboardPVTeamMemberViewModel modelEmp = new DashboardPVTeamMemberViewModel();

                if (ChecksListEmp != null && ChecksListEmp.Count() > 0)
                {
                    DataTable dt = new DataTable();

                    dt = cc.getdashboardPVManagerSummary("Team Member-PV", TeamMemberRowId, "Employment");
                    modelEmp.totWIP1 = Convert.ToInt32(dt.Rows[0]["WIP"].ToString());
                    modelEmp.totRejectionWIP1 = Convert.ToInt32(dt.Rows[0]["RejectedtoPVQC"].ToString());
                    modelEmp.totClosed1 = Convert.ToInt32(dt.Rows[0]["Closed"].ToString());
                    modelEmp.totWIP = ChecksListEmpDays.Where(p => p.PVTMCheckStatus == "WIP-Pending" || p.PVTMCheckStatus == "Allocated-TM").Count();
                    modelEmp.totInitiated = 0; //ChecksListEmpDays.Where(t => t.PTRMgrAllocatedTo > 0).Count();
                    modelEmp.totNonInitiated = 0; //ChecksListEmpDays.Where(t => t.PTRMgrAllocatedTo == 0).Count();
                    modelEmp.totRejectionWIP = ChecksListEmpDays.Where(t => t.PVTMCheckStatus == "WIP-Pending" && string.IsNullOrEmpty(t.PVTMQCCheckStatus)).Count();
                    modelEmp.totReOpen = 0; //ChecksListEmpDays.Where(t => t.PQEmployment.PQPersonal.CaseStatus == "ReOpen").Count();
                    modelEmp.totPriority = ChecksListEmpDays.Where(t => t.PQEmployment.PQPersonal.CaseType == "Priority").Count();
                    modelEmp.totInsuffWIP = 0; //ChecksListEmpDays.Where(t => t.PVTMQCCheckStatus == "Insufficent-QC").Count();

                    modelEmp.totTodaysDue = ChecksListEmpDays.Where(t =>
                 DbFunctions.TruncateTime(t.PQEmployment.InternalOutDate) == DbFunctions.TruncateTime(todayTATDate)
                        ).ToList().Count;

                    modelEmp.totDueinNext2days = ChecksListEmpDays.Where(t =>
                    DbFunctions.TruncateTime(t.PQEmployment.InternalOutDate) > DbFunctions.TruncateTime(todayTATDate) &&
                      DbFunctions.TruncateTime(t.PQEmployment.InternalOutDate) <= DbFunctions.TruncateTime(totDueinNext2days)
                        ).ToList().Count;

                    modelEmp.totInTAT4to7days = ChecksListEmpDays.Where(t =>
                       DbFunctions.TruncateTime(t.PQEmployment.InternalOutDate) > DbFunctions.TruncateTime(totDueinNext2days) &&
                       DbFunctions.TruncateTime(t.PQEmployment.InternalOutDate) <= DbFunctions.TruncateTime(totInTAT7days)
                         ).ToList().Count;

                    modelEmp.totInTATGreater7days = ChecksListEmpDays.Where(t =>
                         DbFunctions.TruncateTime(t.PQEmployment.InternalOutDate) > DbFunctions.TruncateTime(totInTAT7days)
                        ).ToList().Count;

                    modelEmp.totBT1to5days = ChecksListEmpDays.Where(t =>
                  DbFunctions.TruncateTime(t.PQEmployment.InternalOutDate) <= DbFunctions.TruncateTime(totBT1days) &&
                    DbFunctions.TruncateTime(t.PQEmployment.InternalOutDate) >= DbFunctions.TruncateTime(totBT5days)
                        ).ToList().Count;

                    modelEmp.totBT5daysPlus = ChecksListEmpDays.Where(t =>
                    DbFunctions.TruncateTime(t.PQEmployment.InternalOutDate) < DbFunctions.TruncateTime(totBT5days)
                        ).ToList().Count;

                    modelEmp.tot1checkPending = 0;
                    modelEmp.totFundApproval = 0; //ChecksListEmpDays.Where(t => t.PVTLCheckStatus == "Funds Pending").Count();
                    modelEmp.totReviewRaised = 0; //ChecksListEmpDays.Where(t => t.PQEmployment.CheckStatus == "Review Requested").Count();
                    modelEmp.totReviewResolved = 0;
                    modelEmp.totInsufficiencyRaised = 0; // ChecksListEmpDays.Where(t => t.PVTMQCCheckStatus == "Insufficent-QC").Count();
                    modelEmp.totOutTAT = ChecksListEmpDays.Where(t =>
                    DbFunctions.TruncateTime(t.PQEmployment.InternalOutDate) < DbFunctions.TruncateTime(todayTATDate)).Count();
                    modelEmp.totOrange = 0;
                    modelEmp.totClosed = ChecksListEmpDays.Where(t => t.RWQCManagerBy > 0).Count();
                }

                modelPV.TeamMemberDashboardEmp = modelEmp;

                #endregion

                #region ********** For PQEducationStatus **********


                IQueryable<PQEducationStatus> ChecksListEdu = db.PQEducationStatus.Where(a => a.PVTMAllocatedTo == TeamMemberRowId);
                IQueryable<PQEducationStatus> ChecksListEduDays = ChecksListEdu.Where(a => a.PVTMAllocatedTo == TeamMemberRowId && a.PTRMgrAllocatedTo == 0);
                DashboardPVTeamMemberViewModel modelEdu = new DashboardPVTeamMemberViewModel();

                if (ChecksListEdu != null && ChecksListEdu.Count() > 0)
                {
                    DataTable dtEdu = new DataTable();

                    dtEdu = cc.getdashboardPVManagerSummaryEcu("Team Member-PV", TeamMemberRowId, "Education");
                    modelEdu.totWIP1 = Convert.ToInt32(dtEdu.Rows[0]["WIP"].ToString());
                    modelEdu.totRejectionWIP1 = Convert.ToInt32(dtEdu.Rows[0]["RejectedtoPVQC"].ToString());
                    modelEdu.totWIP = ChecksListEduDays.Where(p => p.PVTMCheckStatus == "WIP-Pending" || p.PVTMCheckStatus == "Allocated-TM").Count();
                    modelEdu.totInitiated = 0; //ChecksListEduDays.Where(t => t.PTRMgrAllocatedTo > 0).Count();
                    modelEdu.totNonInitiated = 0; //ChecksListEduDays.Where(t => t.PTRMgrAllocatedTo == 0).Count();
                    modelEdu.totRejectionWIP = ChecksListEduDays.Where(t => t.PVTMCheckStatus == "WIP-Pending" && string.IsNullOrEmpty(t.PVTMCheckStatus)).Count();
                    modelEdu.totReOpen = 0; //ChecksListEduDays.Where(t => t.PQEducation.PQPersonal.CaseStatus == "ReOpen").Count();
                    modelEdu.totPriority = ChecksListEduDays.Where(t => t.PQEducation.PQPersonal.CaseType == "Priority").Count();
                    modelEdu.totInsuffWIP = 0; //ChecksListEduDays.Where(t => t.PVTMQCCheckStatus == "Insufficent-QC").Count();

                    modelEdu.totTodaysDue = ChecksListEduDays.Where(t =>
                 DbFunctions.TruncateTime(t.PQEducation.InternalOutDate) == DbFunctions.TruncateTime(todayTATDate)
                        ).ToList().Count;

                    modelEdu.totDueinNext2days = ChecksListEduDays.Where(t =>
                    DbFunctions.TruncateTime(t.PQEducation.InternalOutDate) > DbFunctions.TruncateTime(todayTATDate) &&
                      DbFunctions.TruncateTime(t.PQEducation.InternalOutDate) <= DbFunctions.TruncateTime(totDueinNext2days)
                        ).ToList().Count;

                    modelEdu.totInTAT4to7days = ChecksListEduDays.Where(t =>
                       DbFunctions.TruncateTime(t.PQEducation.InternalOutDate) > DbFunctions.TruncateTime(totDueinNext2days) &&
                       DbFunctions.TruncateTime(t.PQEducation.InternalOutDate) <= DbFunctions.TruncateTime(totInTAT7days)
                         ).ToList().Count;

                    modelEdu.totInTATGreater7days = ChecksListEduDays.Where(t =>
                         DbFunctions.TruncateTime(t.PQEducation.InternalOutDate) > DbFunctions.TruncateTime(totInTAT7days)
                        ).ToList().Count;

                    modelEdu.totBT1to5days = ChecksListEduDays.Where(t =>
                  DbFunctions.TruncateTime(t.PQEducation.InternalOutDate) <= DbFunctions.TruncateTime(totBT1days) &&
                    DbFunctions.TruncateTime(t.PQEducation.InternalOutDate) >= DbFunctions.TruncateTime(totBT5days)
                        ).ToList().Count;

                    modelEdu.totBT5daysPlus = ChecksListEduDays.Where(t =>
                    DbFunctions.TruncateTime(t.PQEducation.InternalOutDate) < DbFunctions.TruncateTime(totBT5days)
                        ).ToList().Count;

                    modelEdu.tot1checkPending = 0;
                    modelEdu.totFundApproval = 0; //ChecksListEduDays.Where(t => t.PVTLCheckStatus == "Funds Pending").Count();
                    modelEdu.totReviewRaised = 0; //ChecksListEduDays.Where(t => t.PQEducation.CheckStatus == "Review Requested").Count();
                    modelEdu.totReviewResolved = 0;
                    modelEdu.totInsufficiencyRaised = 0; //ChecksListEduDays.Where(t => t.PVTMQCCheckStatus == "Insufficent-QC").Count();
                    modelEdu.totOutTAT = ChecksListEduDays.Where(t =>
                    DbFunctions.TruncateTime(t.PQEducation.InternalOutDate) < DbFunctions.TruncateTime(todayTATDate)).Count();
                    modelEdu.totOrange = 0;
                    modelEdu.totClosed = 0; //ChecksListEduDays.Where(t => t.RWQCManagerBy > 0).Count();
                }

                modelPV.TeamMemberDashboardEdu = modelEdu;

                #endregion

                #region ********** For PQCriminalStatus **********

                IQueryable<PQCriminalStatus> ChecksListCri = db.PQCriminalStatus.Where(a => a.PVTMAllocatedTo == TeamMemberRowId);
                IQueryable<PQCriminalStatus> ChecksListCriDays = ChecksListCri.Where(a => a.PVTMAllocatedTo == TeamMemberRowId && a.PTRMgrAllocatedTo == 0);
                DashboardPVTeamMemberViewModel modelCri = new DashboardPVTeamMemberViewModel();

                if (ChecksListCri != null && ChecksListCri.Count() > 0)
                {
                    DataTable dtCri = new DataTable();

                    dtCri = cc.getdashboardPVManagerSummaryCri("Team Member-PV", TeamMemberRowId, "Criminal");
                    modelCri.totWIP1 = Convert.ToInt32(dtCri.Rows[0]["WIP"].ToString());
                    modelCri.totRejectionWIP1 = Convert.ToInt32(dtCri.Rows[0]["RejectedtoPVQC"].ToString());
                    modelCri.totWIP = ChecksListCriDays.Where(p => p.PVTMCheckStatus == "WIP-Pending" || p.PVTMCheckStatus == "Allocated-TM").Count();
                    modelCri.totInitiated = 0; //ChecksListCriDays.Where(t => t.PTRMgrAllocatedTo > 0).Count();
                    modelCri.totNonInitiated = 0; //ChecksListCriDays.Where(t => t.PTRMgrAllocatedTo == 0).Count();
                    modelCri.totRejectionWIP = ChecksListCriDays.Where(t => t.PVTMCheckStatus == "WIP-Pending" && string.IsNullOrEmpty(t.PVTMCheckStatus)).Count();
                    modelCri.totReOpen = 0; //ChecksListCriDays.Where(t => t.PQCriminal.PQPersonal.CaseStatus == "ReOpen").Count();
                    modelCri.totPriority = ChecksListCriDays.Where(t => t.PQCriminal.PQPersonal.CaseType == "Priority").Count();
                    modelCri.totInsuffWIP = 0; //ChecksListCriDays.Where(t => t.PVTMQCCheckStatus == "Insufficent-QC").Count();

                    modelCri.totTodaysDue = ChecksListCriDays.Where(t =>
                 DbFunctions.TruncateTime(t.PQCriminal.InternalOutDate) == DbFunctions.TruncateTime(todayTATDate)
                        ).ToList().Count;

                    modelCri.totDueinNext2days = ChecksListCriDays.Where(t =>
                    DbFunctions.TruncateTime(t.PQCriminal.InternalOutDate) > DbFunctions.TruncateTime(todayTATDate) &&
                      DbFunctions.TruncateTime(t.PQCriminal.InternalOutDate) <= DbFunctions.TruncateTime(totDueinNext2days)
                        ).ToList().Count;

                    modelCri.totInTAT4to7days = ChecksListCriDays.Where(t =>
                       DbFunctions.TruncateTime(t.PQCriminal.InternalOutDate) > DbFunctions.TruncateTime(totDueinNext2days) &&
                       DbFunctions.TruncateTime(t.PQCriminal.InternalOutDate) <= DbFunctions.TruncateTime(totInTAT7days)
                         ).ToList().Count;

                    modelCri.totInTATGreater7days = ChecksListCriDays.Where(t =>
                         DbFunctions.TruncateTime(t.PQCriminal.InternalOutDate) > DbFunctions.TruncateTime(totInTAT7days)
                        ).ToList().Count;

                    modelCri.totBT1to5days = ChecksListCriDays.Where(t =>
                  DbFunctions.TruncateTime(t.PQCriminal.InternalOutDate) <= DbFunctions.TruncateTime(totBT1days) &&
                    DbFunctions.TruncateTime(t.PQCriminal.InternalOutDate) >= DbFunctions.TruncateTime(totBT5days)
                        ).ToList().Count;

                    modelCri.totBT5daysPlus = ChecksListCriDays.Where(t =>
                    DbFunctions.TruncateTime(t.PQCriminal.InternalOutDate) < DbFunctions.TruncateTime(totBT5days)
                        ).ToList().Count;

                    modelCri.tot1checkPending = 0;
                    modelCri.totFundApproval = 0; //ChecksListCriDays.Where(t => t.PVTLCheckStatus == "Funds Pending").Count();
                    modelCri.totReviewRaised = 0; //ChecksListCriDays.Where(t => t.PQCriminal.CheckStatus == "Review Requested").Count();
                    modelCri.totReviewResolved = 0;
                    modelCri.totInsufficiencyRaised = 0; // ChecksListCriDays.Where(t => t.PVTMQCCheckStatus == "Insufficent-QC").Count();
                    modelCri.totOutTAT = ChecksListCriDays.Where(t =>
                    DbFunctions.TruncateTime(t.PQCriminal.InternalOutDate) < DbFunctions.TruncateTime(todayTATDate)).Count();
                    modelCri.totOrange = 0;
                    modelCri.totClosed = 0; //ChecksListCriDays.Where(t => t.RWQCManagerBy > 0).Count();
                }

                modelPV.TeamMemberDashboardCri = modelCri;

                #endregion

                return modelPV;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DashboardEmpEduCriPVTMQCViewModel GetPVTeamMemberQCDashBoardInfo(short TeamMemberRowId, string Designation)
        {
            try
            {
                DashboardEmpEduCriPVTMQCViewModel modelPV = new DashboardEmpEduCriPVTMQCViewModel();

                #region ********** For PQEmploymentStatus **********
                DateTime todayTATDate = DateTime.Now;
                DateTime totTodaysDue = todayTATDate;
                TimeSpan totWIP2Hrs = todayTATDate.AddDays(2).TimeOfDay;
                TimeSpan totWIP6Hrs = todayTATDate.AddDays(6).TimeOfDay;
                TimeSpan totWIP12Hrs = todayTATDate.AddDays(12).TimeOfDay;
                TimeSpan totWIP24Hrs = todayTATDate.AddDays(24).TimeOfDay;
                TimeSpan totWIP24HrsPlus = todayTATDate.AddDays(24).TimeOfDay;
                TimeSpan currentTime = DateTime.Now.TimeOfDay;

                IQueryable<PQEmploymentStatus> ChecksListEmp = db.PQEmploymentStatus.Where(a => a.PVTMQCAllocatedTo == TeamMemberRowId);
                IQueryable<PQEmploymentStatus> ChecksListEmpDays = ChecksListEmp.Where(a => a.PVTMQCAllocatedTo == TeamMemberRowId && a.PVTMCheckStatus == "Completed" && a.PVTMQCCheckStatus == "Completed");
                DashboardPVTeamMemberQCViewModel modelEmp = new DashboardPVTeamMemberQCViewModel();

                if (ChecksListEmp != null && ChecksListEmp.Count() > 0)
                {
                    DataTable dt = new DataTable();

                    dt = cc.getdashboardPVManagerSummary("Team Member-PV-QC", TeamMemberRowId, "Employment");

                    modelEmp.totNew1 = Convert.ToInt32(dt.Rows[0]["New"].ToString());
                    modelEmp.totRejectionWIP1 = Convert.ToInt32(dt.Rows[0]["RejectedtoPVQC"].ToString());
                    //modelEmp.totClosed1 = Convert.ToInt32(dt.Rows[0]["Closed"].ToString());
                    modelEmp.totNew = ChecksListEmpDays.Count();
                    modelEmp.totWIP = ChecksListEmpDays.Where(p => p.PVTMCheckStatus == "Allocated-TM" && p.PVTMQCCheckStatus == "Allocated-TM").Count();
                    modelEmp.totRejectionWIP = ChecksListEmp.Where(t => t.PVTMCheckStatus == "WIP-Pending" && string.IsNullOrEmpty(t.PVTMCheckStatus)).Count();
                    modelEmp.totReOpen = 0;// ChecksListEmpDays.Where(t => t.PQEmployment.PQPersonal.CaseStatus == "ReOpen").Count();
                    modelEmp.totPriority = ChecksListEmpDays.Where(t => t.PQEmployment.PQPersonal.CaseType == "Priority").Count();
                    modelEmp.totInsuffWIP = 0;//ChecksListEmpDays.Where(t => t.PVTMQCCheckStatus == "Insufficent").Count();

                    modelEmp.totTodaysDue = ChecksListEmpDays.Where(t =>
                            DbFunctions.TruncateTime(t.PQEmployment.InternalOutDate) == DbFunctions.TruncateTime(todayTATDate)
                        ).ToList().Count;

                    modelEmp.totWIP2Hrs = ChecksListEmpDays.Where(t =>
                             DbFunctions.CreateTime(t.PVTMQCAllocatedDate.Value.Hour, t.PVTMQCAllocatedDate.Value.Minute, 0) <= currentTime &&
                             DbFunctions.CreateTime(t.PVTMQCAllocatedDate.Value.Hour, t.PVTMQCAllocatedDate.Value.Minute, 0) < totWIP2Hrs).ToList().Count;

                    modelEmp.totWIP3to6Hrs = ChecksListEmpDays.Where(t =>
                             DbFunctions.CreateTime(t.PVTMQCAllocatedDate.Value.Hour, t.PVTMQCAllocatedDate.Value.Minute, 0) > totWIP2Hrs &&
                             DbFunctions.CreateTime(t.PVTMQCAllocatedDate.Value.Hour, t.PVTMQCAllocatedDate.Value.Minute, 0) < totWIP6Hrs).ToList().Count;

                    modelEmp.totWIP6to12Hrs = ChecksListEmpDays.Where(t =>
                             DbFunctions.CreateTime(t.PVTMQCAllocatedDate.Value.Hour, t.PVTMQCAllocatedDate.Value.Minute, 0) > totWIP6Hrs &&
                             DbFunctions.CreateTime(t.PVTMQCAllocatedDate.Value.Hour, t.PVTMQCAllocatedDate.Value.Minute, 0) < totWIP12Hrs).ToList().Count;

                    modelEmp.totWIP12to24Hrs = ChecksListEmpDays.Where(t =>
                            DbFunctions.CreateTime(t.PVTMQCAllocatedDate.Value.Hour, t.PVTMQCAllocatedDate.Value.Minute, 0) > totWIP12Hrs &&
                             DbFunctions.CreateTime(t.PVTMQCAllocatedDate.Value.Hour, t.PVTMQCAllocatedDate.Value.Minute, 0) < totWIP24Hrs).ToList().Count;

                    modelEmp.totWIP24HrsPlus = ChecksListEmpDays.Where(t =>
                          DbFunctions.CreateTime(t.PVTMQCAllocatedDate.Value.Hour, t.PVTMQCAllocatedDate.Value.Minute, 0) > totWIP24Hrs).ToList().Count;


                    modelEmp.totFundApproval = 0;//ChecksListEmpDays.Where(t => t.PVTLCheckStatus == "Funds Pending").Count();
                    modelEmp.totReviewResolved = 0;
                    modelEmp.totApprovalPending = 0;//ChecksListEmpDays.Where(p => p.PVTMQCCheckStatus == "Completed" || p.PVTMQCCheckStatus == "Insufficent" || p.PVTMQCCheckStatus == "Funds Pending").Count();
                    modelEmp.totInsufficiencyRaised = 0;//ChecksListEmpDays.Where(t => t.PVTMQCCheckStatus == "Insufficent-QC").Count();
                    modelEmp.totQCApproval = 0;// ChecksListEmpDays.Where(p => p.PVTMQCCheckStatus == "Completed-QC" || p.PVTMQCCheckStatus == "Insufficent-QC" || p.PVTMQCCheckStatus == "Funds Pending-QC").Count();
                    modelEmp.totOutTAT = ChecksListEmpDays.Where(t =>
                    DbFunctions.TruncateTime(t.PQEmployment.InternalOutDate) < DbFunctions.TruncateTime(todayTATDate)).Count();

                }
                modelPV.TeamMemberQCDashboardEmp = modelEmp;

                #endregion

                #region ********** For PQEducationStatus **********

                IQueryable<PQEducationStatus> ChecksListEdu = db.PQEducationStatus.Where(a => a.PVTMQCAllocatedTo == TeamMemberRowId);
                IQueryable<PQEducationStatus> ChecksListEduDays = ChecksListEdu.Where(a => a.PVTMQCAllocatedTo == TeamMemberRowId && a.PVTMCheckStatus == "Completed" && a.PVTMQCCheckStatus == "Completed");
                DashboardPVTeamMemberQCViewModel modelEdu = new DashboardPVTeamMemberQCViewModel();
                if (ChecksListEdu != null && ChecksListEdu.Count() > 0)
                {
                    DataTable dtEdu = new DataTable();

                    dtEdu = cc.getdashboardPVManagerSummaryEcu("Team Member-PV-QC", TeamMemberRowId, "Education");

                    modelEdu.totNew1 = Convert.ToInt32(dtEdu.Rows[0]["New"].ToString());
                    modelEdu.totRejectionWIP1 = Convert.ToInt32(dtEdu.Rows[0]["RejectedtoPVQC"].ToString());
                    modelEdu.totNew = ChecksListEduDays.Count();
                    modelEdu.totWIP = ChecksListEduDays.Where(p => p.PVTMCheckStatus == "Allocated-TM" && p.PVTMQCCheckStatus == "Allocated-TM").Count();
                    modelEdu.totRejectionWIP = ChecksListEduDays.Where(t => t.PVTMCheckStatus == "WIP-Pending" && string.IsNullOrEmpty(t.PVTMCheckStatus)).Count();
                    modelEdu.totReOpen = 0;// ChecksListEduDays.Where(t => t.PQEmployment.PQPersonal.CaseStatus == "ReOpen").Count();
                    modelEdu.totPriority = ChecksListEduDays.Where(t => t.PQEducation.PQPersonal.CaseType == "Priority").Count();
                    modelEdu.totInsuffWIP = 0;//ChecksListEduDays.Where(t => t.PVTMQCCheckStatus == "Insufficent").Count();

                    modelEdu.totTodaysDue = ChecksListEduDays.Where(t =>
                            DbFunctions.TruncateTime(t.PQEducation.InternalOutDate) == DbFunctions.TruncateTime(todayTATDate)
                        ).ToList().Count;

                    modelEdu.totWIP2Hrs = ChecksListEduDays.Where(t =>
                             DbFunctions.CreateTime(t.PVTMQCAllocatedDate.Value.Hour, t.PVTMQCAllocatedDate.Value.Minute, 0) <= currentTime &&
                             DbFunctions.CreateTime(t.PVTMQCAllocatedDate.Value.Hour, t.PVTMQCAllocatedDate.Value.Minute, 0) < totWIP2Hrs).ToList().Count;

                    modelEdu.totWIP3to6Hrs = ChecksListEduDays.Where(t =>
                             DbFunctions.CreateTime(t.PVTMQCAllocatedDate.Value.Hour, t.PVTMQCAllocatedDate.Value.Minute, 0) > totWIP2Hrs &&
                             DbFunctions.CreateTime(t.PVTMQCAllocatedDate.Value.Hour, t.PVTMQCAllocatedDate.Value.Minute, 0) < totWIP6Hrs).ToList().Count;

                    modelEdu.totWIP6to12Hrs = ChecksListEduDays.Where(t =>
                             DbFunctions.CreateTime(t.PVTMQCAllocatedDate.Value.Hour, t.PVTMQCAllocatedDate.Value.Minute, 0) > totWIP6Hrs &&
                             DbFunctions.CreateTime(t.PVTMQCAllocatedDate.Value.Hour, t.PVTMQCAllocatedDate.Value.Minute, 0) < totWIP12Hrs).ToList().Count;

                    modelEdu.totWIP12to24Hrs = ChecksListEduDays.Where(t =>
                            DbFunctions.CreateTime(t.PVTMQCAllocatedDate.Value.Hour, t.PVTMQCAllocatedDate.Value.Minute, 0) > totWIP12Hrs &&
                             DbFunctions.CreateTime(t.PVTMQCAllocatedDate.Value.Hour, t.PVTMQCAllocatedDate.Value.Minute, 0) < totWIP24Hrs).ToList().Count;

                    modelEdu.totWIP24HrsPlus = ChecksListEduDays.Where(t =>
                          DbFunctions.CreateTime(t.PVTMQCAllocatedDate.Value.Hour, t.PVTMQCAllocatedDate.Value.Minute, 0) > totWIP24Hrs).ToList().Count;


                    modelEdu.totFundApproval = 0;// ChecksListEduDays.Where(t => t.PVTLCheckStatus == "Funds Pending").Count();
                    modelEdu.totReviewResolved = 0;
                    modelEdu.totApprovalPending = 0;//ChecksListEduDays.Where(p => p.PVTMQCCheckStatus == "Completed" || p.PVTMQCCheckStatus == "Insufficent" || p.PVTMQCCheckStatus == "Funds Pending").Count();
                    modelEdu.totInsufficiencyRaised = 0;//ChecksListEduDays.Where(t => t.PVTMQCCheckStatus == "Insufficent-QC").Count();
                    modelEdu.totQCApproval = 0;//ChecksListEduDays.Where(p => p.PVTMQCCheckStatus == "Completed-QC" || p.PVTMQCCheckStatus == "Insufficent-QC" || p.PVTMQCCheckStatus == "Funds Pending-QC").Count();
                    modelEdu.totOutTAT = ChecksListEduDays.Where(t =>
                    DbFunctions.TruncateTime(t.PQEducation.InternalOutDate) < DbFunctions.TruncateTime(todayTATDate)).Count();

                }
                modelPV.TeamMemberQCDashboardEdu = modelEdu;

                #endregion

                #region ********** For PQCriminalStatus **********

                IQueryable<PQCriminalStatus> ChecksListCri = db.PQCriminalStatus.Where(a => a.PVTMQCAllocatedTo == TeamMemberRowId);
                IQueryable<PQCriminalStatus> ChecksListCriDays = ChecksListCri.Where(a => a.PVTMQCAllocatedTo == TeamMemberRowId && a.PVTMCheckStatus == "Completed" && a.PVTMQCCheckStatus == "Completed");

                DashboardPVTeamMemberQCViewModel modelCri = new DashboardPVTeamMemberQCViewModel();
                if (ChecksListCri != null && ChecksListCri.Count() > 0)
                {
                    DataTable dtCri = new DataTable();

                    dtCri = cc.getdashboardPVManagerSummaryCri("Team Member-PV-QC", TeamMemberRowId, "Criminal");

                    modelCri.totNew1 = Convert.ToInt32(dtCri.Rows[0]["New"].ToString());
                    modelCri.totRejectionWIP1 = Convert.ToInt32(dtCri.Rows[0]["RejectedtoPVQC"].ToString());
                    modelCri.totNew = ChecksListCriDays.Count();
                    modelCri.totWIP = ChecksListCriDays.Where(p => p.PVTMCheckStatus == "Allocated-TM" && p.PVTMQCCheckStatus == "Allocated-TM").Count();
                    modelCri.totRejectionWIP = ChecksListCriDays.Where(t => t.PVTMCheckStatus == "WIP-Pending" && string.IsNullOrEmpty(t.PVTMCheckStatus)).Count();
                    modelCri.totReOpen = 0;// ChecksListCriDays.Where(t => t.PQEmployment.PQPersonal.CaseStatus == "ReOpen").Count();
                    modelCri.totPriority = ChecksListCriDays.Where(t => t.PQCriminal.PQPersonal.CaseType == "Priority").Count();
                    modelCri.totInsuffWIP = 0;//ChecksListCriDays.Where(t => t.PVTMQCCheckStatus == "Insufficent").Count();

                    modelCri.totTodaysDue = ChecksListCriDays.Where(t =>
                            DbFunctions.TruncateTime(t.PQCriminal.InternalOutDate) == DbFunctions.TruncateTime(todayTATDate)
                        ).ToList().Count;

                    modelCri.totWIP2Hrs = ChecksListCriDays.Where(t =>
                             DbFunctions.CreateTime(t.PVTMQCAllocatedDate.Value.Hour, t.PVTMQCAllocatedDate.Value.Minute, 0) <= currentTime &&
                             DbFunctions.CreateTime(t.PVTMQCAllocatedDate.Value.Hour, t.PVTMQCAllocatedDate.Value.Minute, 0) < totWIP2Hrs).ToList().Count;

                    modelCri.totWIP3to6Hrs = ChecksListCriDays.Where(t =>
                             DbFunctions.CreateTime(t.PVTMQCAllocatedDate.Value.Hour, t.PVTMQCAllocatedDate.Value.Minute, 0) > totWIP2Hrs &&
                             DbFunctions.CreateTime(t.PVTMQCAllocatedDate.Value.Hour, t.PVTMQCAllocatedDate.Value.Minute, 0) < totWIP6Hrs).ToList().Count;

                    modelCri.totWIP6to12Hrs = ChecksListCriDays.Where(t =>
                             DbFunctions.CreateTime(t.PVTMQCAllocatedDate.Value.Hour, t.PVTMQCAllocatedDate.Value.Minute, 0) > totWIP6Hrs &&
                             DbFunctions.CreateTime(t.PVTMQCAllocatedDate.Value.Hour, t.PVTMQCAllocatedDate.Value.Minute, 0) < totWIP12Hrs).ToList().Count;

                    modelCri.totWIP12to24Hrs = ChecksListCriDays.Where(t =>
                            DbFunctions.CreateTime(t.PVTMQCAllocatedDate.Value.Hour, t.PVTMQCAllocatedDate.Value.Minute, 0) > totWIP12Hrs &&
                             DbFunctions.CreateTime(t.PVTMQCAllocatedDate.Value.Hour, t.PVTMQCAllocatedDate.Value.Minute, 0) < totWIP24Hrs).ToList().Count;

                    modelCri.totWIP24HrsPlus = ChecksListCriDays.Where(t =>
                          DbFunctions.CreateTime(t.PVTMQCAllocatedDate.Value.Hour, t.PVTMQCAllocatedDate.Value.Minute, 0) > totWIP24Hrs).ToList().Count;


                    modelCri.totFundApproval = 0;// ChecksListCriDays.Where(t => t.PVTLCheckStatus == "Funds Pending").Count();
                    modelCri.totReviewResolved = 0;
                    modelCri.totApprovalPending = 0;//ChecksListCriDays.Where(p => p.PVTMQCCheckStatus == "Completed" || p.PVTMQCCheckStatus == "Insufficent" || p.PVTMQCCheckStatus == "Funds Pending").Count();
                    modelCri.totInsufficiencyRaised = 0;//ChecksListCriDays.Where(t => t.PVTMQCCheckStatus == "Insufficent-QC").Count();
                    modelCri.totQCApproval = 0;//ChecksListCriDays.Where(p => p.PVTMQCCheckStatus == "Completed-QC" || p.PVTMQCCheckStatus == "Insufficent-QC" || p.PVTMQCCheckStatus == "Funds Pending-QC").Count();
                    modelCri.totOutTAT = ChecksListCriDays.Where(t =>
                    DbFunctions.TruncateTime(t.PQCriminal.InternalOutDate) < DbFunctions.TruncateTime(todayTATDate)).Count();

                }
                modelPV.TeamMemberQCDashboardCri = modelCri;

                #endregion

                return modelPV;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
