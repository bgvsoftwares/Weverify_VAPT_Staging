using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.ClientViewModel;
using ViewModels.VerificationViewModel;


namespace BAL.VerificationRepository
{
    public class DashboardVerificationRepository : IDashboardVerificationRepository
    {
        DataContext db;
        ClientCustomize cc;

        public DashboardVerificationRepository()
        {
            db = new DataContext();
            cc = new ClientCustomize();
        }

        public DashboardManagerViewModel GetManagerDashBoardInfo(short TeamMemberRowId, string Designation)
        {
            try
            {
                DateTime todayTATDate = DateTime.Now;
                DateTime totTodaysDue = todayTATDate;
                DateTime totDueinNext2days = todayTATDate.AddDays(2);
                DateTime totInTAT4days = todayTATDate.AddDays(4);
                DateTime totInTAT7days = todayTATDate.AddDays(7);
                DateTime totBT1days = todayTATDate.AddDays(-1);
                DateTime totBT2days = todayTATDate.AddDays(-2);
                DateTime totBT5days = todayTATDate.AddDays(-5);
                DateTime totBT10days = todayTATDate.AddDays(-10);

                if (Designation == "Manager-Add")
                {
                   DashboardManagerViewModel model = new DashboardManagerViewModel();
                    DataTable dt = new DataTable();
                    dt = cc.getdashboardOperationAddressManagerSummary("Address", TeamMemberRowId, "OP-Manager");
                    DataTable dt1 = new DataTable();
                    dt1 = cc.getdashboardAging(2);
                    model._dashboardsAging.todayPending = dt1.Rows[0]["todayPending"].ToString();
                    model._dashboardsAging.NextTwoDays = dt1.Rows[0]["NextTwoDays"].ToString();
                    model._dashboardsAging.WithinTAT = dt1.Rows[0]["Within TAT"].ToString();
                    model._dashboardsAging.daysBT1_2 = dt1.Rows[0]["1-2 days BT"].ToString();
                    model._dashboardsAging.daysBT3_5 = dt1.Rows[0]["3-5 days BT"].ToString();
                    model._dashboardsAging.above5daysBT = dt1.Rows[0]["above 5 days BT"].ToString();                   
                    model.Allocated1 = Convert.ToInt32(dt.Rows[0]["Allocated"].ToString());
                    model.UnAllocated1 = Convert.ToInt32(dt.Rows[0]["UnAllocated"].ToString());
                    model.Closed1 = Convert.ToInt32(dt.Rows[0]["Closed"].ToString());
                    model.Reopen1 = Convert.ToInt32(dt.Rows[0]["Reopen"].ToString());
                    model.Insufficency1 = Convert.ToInt32(dt.Rows[0]["Insufficency"].ToString());
                    model.WIP1 = Convert.ToInt32(dt.Rows[0]["WIP"].ToString());
                    model.Review = Convert.ToInt32(dt.Rows[0]["Review"].ToString());
                    model.InsufficentResolved = Convert.ToInt32(dt.Rows[0]["InsuffResolved"].ToString());

                    model.FundsResolved = Convert.ToInt32(dt.Rows[0]["FundsResolved"].ToString());
                    model.totRejectionWIP = Convert.ToInt32(dt.Rows[0]["Rejected"].ToString());
                    model.totFundApproval = Convert.ToInt32(dt.Rows[0]["FundsPending"].ToString());
                    model.totInsufficiency = Convert.ToInt32(dt.Rows[0]["InsufficentRaised"].ToString());
                    model.ReviewTL = Convert.ToInt32(dt.Rows[0]["ReviewTL"].ToString());
                    model.PendingQC = Convert.ToInt32(dt.Rows[0]["PendingOPQC"].ToString());
                    model.ReviewForApproval = Convert.ToInt32(dt.Rows[0]["ReviewForApproval"].ToString());
                    model.ReviewResolved = Convert.ToInt32(dt.Rows[0]["ReviewResolved"].ToString());
                    return model;
                    
                }
                if (Designation == "Manager-Emp")
                {
                   DashboardManagerViewModel model = new DashboardManagerViewModel();
                    DataTable dt = new DataTable();
                    dt = cc.getdashboardOperationTLSummary("Employment", TeamMemberRowId, "OP-Manager");
                    DataTable dt1 = new DataTable();
                    dt1 = cc.getdashboardAging(12);
                    model._dashboardsAging.todayPending = dt1.Rows[0]["todayPending"].ToString();
                    model._dashboardsAging.NextTwoDays = dt1.Rows[0]["NextTwoDays"].ToString();
                    model._dashboardsAging.WithinTAT = dt1.Rows[0]["Within TAT"].ToString();
                    model._dashboardsAging.daysBT1_2 = dt1.Rows[0]["1-2 days BT"].ToString();
                    model._dashboardsAging.daysBT3_5 = dt1.Rows[0]["3-5 days BT"].ToString();
                    model._dashboardsAging.above5daysBT = dt1.Rows[0]["above 5 days BT"].ToString();
                                       model.Allocated1 = Convert.ToInt32(dt.Rows[0]["Allocated"].ToString());
                    model.UnAllocated1 = Convert.ToInt32(dt.Rows[0]["UnAllocated"].ToString());
                    model.PVUnallocated = Convert.ToInt32(dt.Rows[0]["PVUnallocated"].ToString());
                    model.Closed1 = Convert.ToInt32(dt.Rows[0]["Closed"].ToString());
                    model.Reopen1 = Convert.ToInt32(dt.Rows[0]["Reopen"].ToString());
                    model.Insufficency1 = Convert.ToInt32(dt.Rows[0]["Insufficency"].ToString());
                    model.WIP1 = Convert.ToInt32(dt.Rows[0]["WIP"].ToString());
                    model.Review = Convert.ToInt32(dt.Rows[0]["Review"].ToString());
                    model.Reverification = Convert.ToInt32(dt.Rows[0]["Reverification"].ToString());
                    model.WIPALL = Convert.ToInt32(dt.Rows[0]["WIPALL"].ToString());
                    model.YTR = Convert.ToInt32(dt.Rows[0]["YTR"].ToString());
                    model.ResearchRaised = Convert.ToInt32(dt.Rows[0]["Research"].ToString());


                    model.InsufficentResolved = Convert.ToInt32(dt.Rows[0]["InsuffResolved"].ToString());

                    model.FundsResolved = Convert.ToInt32(dt.Rows[0]["FundsResolved"].ToString());
                    model.totRejectionWIP = Convert.ToInt32(dt.Rows[0]["Rejected"].ToString());
                    model.totFundApproval = Convert.ToInt32(dt.Rows[0]["FundsPending"].ToString());
                    model.totInsufficiency = Convert.ToInt32(dt.Rows[0]["InsufficentRaised"].ToString());
                    model.ReviewTL = Convert.ToInt32(dt.Rows[0]["ReviewTL"].ToString());
                    model.PendingQC = Convert.ToInt32(dt.Rows[0]["PendingOPQC"].ToString());
                    model.ReviewForApproval = Convert.ToInt32(dt.Rows[0]["ReviewForApproval"].ToString());
                    model.ReviewResolved = Convert.ToInt32(dt.Rows[0]["ReviewResolved"].ToString());

                    return model;                   
                }
                if (Designation == "Manager-Edu")
                {
                    DashboardManagerViewModel model = new DashboardManagerViewModel();
                    DataTable dt = new DataTable();
                    dt = cc.getdashboardOperationTLSummary("Education", TeamMemberRowId, "OP-Manager");
                    DataTable dt1 = new DataTable();
                    dt1 = cc.getdashboardAging(11);
                    model._dashboardsAging.totalWIP = dt1.Rows[0]["TotalWIP"].ToString();
                    model._dashboardsAging.todayPending = dt1.Rows[0]["todayPending"].ToString();
                    model._dashboardsAging.NextTwoDays = dt1.Rows[0]["NextTwoDays"].ToString();
                    model._dashboardsAging.WithinTAT = dt1.Rows[0]["Within TAT"].ToString();
                    model._dashboardsAging.daysBT1_2 = dt1.Rows[0]["1-2 days BT"].ToString();
                    model._dashboardsAging.daysBT3_5 = dt1.Rows[0]["3-5 days BT"].ToString();
                    model._dashboardsAging.above5daysBT = dt1.Rows[0]["above 5 days BT"].ToString();
                    model.Allocated1 = Convert.ToInt32(dt.Rows[0]["Allocated"].ToString());
                    model.UnAllocated1 = Convert.ToInt32(dt.Rows[0]["UnAllocated"].ToString());
                    model.Closed1 = Convert.ToInt32(dt.Rows[0]["Closed"].ToString());
                    model.Reopen1 = Convert.ToInt32(dt.Rows[0]["Reopen"].ToString());
                    model.Insufficency1 = Convert.ToInt32(dt.Rows[0]["Insufficency"].ToString());
                    model.WIP1 = Convert.ToInt32(dt.Rows[0]["WIP"].ToString());
                    model.totApprovalpending1= Convert.ToInt32(dt.Rows[0]["totApprovalpending"].ToString());
                    model.Review = Convert.ToInt32(dt.Rows[0]["Review"].ToString());
                    model.InsufficentResolved = Convert.ToInt32(dt.Rows[0]["InsuffResolved"].ToString());
                    model.Reverification = Convert.ToInt32(dt.Rows[0]["Reverification"].ToString());
                    model.FundsResolved = Convert.ToInt32(dt.Rows[0]["FundsResolved"].ToString());
                    model.totRejectionWIP = Convert.ToInt32(dt.Rows[0]["Rejected"].ToString());
                    model.totFundApproval = Convert.ToInt32(dt.Rows[0]["FundsPending"].ToString());
                    model.totInsufficiency = Convert.ToInt32(dt.Rows[0]["InsufficentRaised"].ToString());
                    model.ReviewTL = Convert.ToInt32(dt.Rows[0]["ReviewTL"].ToString());
                    model.PendingQC = Convert.ToInt32(dt.Rows[0]["PendingOPQC"].ToString());
                    model.ReviewForApproval = Convert.ToInt32(dt.Rows[0]["ReviewForApproval"].ToString());
                    model.ReviewResolved = Convert.ToInt32(dt.Rows[0]["ReviewResolved"].ToString());
                    model.ResearchRaised = Convert.ToInt32(dt.Rows[0]["Research"].ToString());
                    model.ResultAwaitedRaised = Convert.ToInt32(dt.Rows[0]["ResultAwaitedRaised"].ToString());
                    DataTable DTAuto = new DataTable();
                    DTAuto = cc.GetEducationAutoWIPManager(TeamMemberRowId);

                    model.initialMailSent = Convert.ToInt32(DTAuto.Rows[0]["InitialMailsend"].ToString());
                    model.Reminder1 = Convert.ToInt32(DTAuto.Rows[0]["SentReminder1"].ToString());
                    model.Reminder2 = Convert.ToInt32(DTAuto.Rows[0]["SentReminder2"].ToString());
                    model.Reminder3 = Convert.ToInt32(DTAuto.Rows[0]["SentReminder3"].ToString());
                    model.Reminder4 = Convert.ToInt32(DTAuto.Rows[0]["SentReminder4"].ToString());
                    model.Reminder5 = Convert.ToInt32(DTAuto.Rows[0]["SentReminder5"].ToString());
                    model.GreaterThan5 = Convert.ToInt32(DTAuto.Rows[0]["GreaterThan5"].ToString());
                    model.ErrorInMail = Convert.ToInt32(DTAuto.Rows[0]["ErrorInMail"].ToString());
                    model.wipAll = Convert.ToInt32(DTAuto.Rows[0]["wipAll"].ToString());
                    model.Bounce = Convert.ToInt32(DTAuto.Rows[0]["Bounce"].ToString());
                    model.WIPAllEdu = Convert.ToInt32(DTAuto.Rows[0]["WIPAllEdu"].ToString());

                    return model;                    
                }
                if (Designation == "Manager-Cri")
                {                    
                    DashboardManagerViewModel model = new DashboardManagerViewModel();
                    DataTable dt = new DataTable();
                    dt = cc.getdashboardOperationTLSummary("Criminal", TeamMemberRowId, "OP-Manager");
                    DataTable dt1 = new DataTable();
                    dt1 = cc.getdashboardAging(5);
                    model._dashboardsAging.todayPending = dt1.Rows[0]["todayPending"].ToString();
                    model._dashboardsAging.NextTwoDays = dt1.Rows[0]["NextTwoDays"].ToString();
                    model._dashboardsAging.WithinTAT = dt1.Rows[0]["Within TAT"].ToString();
                    model._dashboardsAging.daysBT1_2 = dt1.Rows[0]["1-2 days BT"].ToString();
                    model._dashboardsAging.daysBT3_5 = dt1.Rows[0]["3-5 days BT"].ToString();
                    model._dashboardsAging.above5daysBT = dt1.Rows[0]["above 5 days BT"].ToString();
                    
                    model.Allocated1 = Convert.ToInt32(dt.Rows[0]["Allocated"].ToString());
                    model.UnAllocated1 = Convert.ToInt32(dt.Rows[0]["UnAllocated"].ToString());
                    model.Closed1 = Convert.ToInt32(dt.Rows[0]["Closed"].ToString());
                    model.Reopen1 = Convert.ToInt32(dt.Rows[0]["Reopen"].ToString());
                    model.Insufficency1 = Convert.ToInt32(dt.Rows[0]["Insufficency"].ToString());
                    model.WIP1 = Convert.ToInt32(dt.Rows[0]["WIP"].ToString());
                    model.Review = Convert.ToInt32(dt.Rows[0]["Review"].ToString());

                    model.Reverification = Convert.ToInt32(dt.Rows[0]["Reverification"].ToString());
                    model.FundsResolved = Convert.ToInt32(dt.Rows[0]["FundsResolved"].ToString());
                    model.InsufficentResolved = Convert.ToInt32(dt.Rows[0]["InsuffResolved"].ToString());
                    model.FundsResolved = Convert.ToInt32(dt.Rows[0]["FundsRequestClosed"].ToString());
                    model.PendingQC = Convert.ToInt32(dt.Rows[0]["PendingOPQC"].ToString());
                    model.ReviewTL = Convert.ToInt32(dt.Rows[0]["ReviewTL"].ToString());
                    model.ReviewForApproval = Convert.ToInt32(dt.Rows[0]["ReviewForApproval"].ToString());
                    model.ReviewResolved = Convert.ToInt32(dt.Rows[0]["ReviewResolved"].ToString());
                    model.SpocReviewPending = Convert.ToInt32(dt.Rows[0]["SpocReviewPending"]);
                    return model;                   
                }
                if (Designation == "Manager-Ref")
                {
                    DashboardManagerViewModel model = new DashboardManagerViewModel();
                    DataTable dt = new DataTable();
                    dt = cc.getdashboardOperationTLSummary("Reference", TeamMemberRowId, "OP-Manager");
                    DataTable dt1 = new DataTable();
                    dt1 = cc.getdashboardAging(30);
                    model._dashboardsAging.todayPending = dt1.Rows[0]["todayPending"].ToString();
                    model._dashboardsAging.NextTwoDays = dt1.Rows[0]["NextTwoDays"].ToString();
                    model._dashboardsAging.WithinTAT = dt1.Rows[0]["Within TAT"].ToString();
                    model._dashboardsAging.daysBT1_2 = dt1.Rows[0]["1-2 days BT"].ToString();
                    model._dashboardsAging.daysBT3_5 = dt1.Rows[0]["3-5 days BT"].ToString();
                    model._dashboardsAging.above5daysBT = dt1.Rows[0]["above 5 days BT"].ToString();
                                       
                    model.Allocated1 = Convert.ToInt32(dt.Rows[0]["Allocated"].ToString());
                    model.UnAllocated1 = Convert.ToInt32(dt.Rows[0]["UnAllocated"].ToString());
                    model.Closed1 = Convert.ToInt32(dt.Rows[0]["Closed"].ToString());
                    model.Reopen1 = Convert.ToInt32(dt.Rows[0]["Reopen"].ToString());
                    model.Insufficency1 = Convert.ToInt32(dt.Rows[0]["Insufficency"].ToString());
                    model.WIP1 = Convert.ToInt32(dt.Rows[0]["WIP"].ToString());
                    model.WIPALL = Convert.ToInt32(dt.Rows[0]["WIPALL"].ToString());
                    model.Review = Convert.ToInt32(dt.Rows[0]["Review"].ToString());
                     return model;
                    
                   // };
                }
                if (Designation == "Manager-Ide")
                {
                   DashboardManagerViewModel model = new DashboardManagerViewModel();
                    DataTable dt = new DataTable();
                    dt = cc.getdashboardOperationTLSummary("National", TeamMemberRowId, "OP-Manager");
                    DataTable dt1 = new DataTable();
                    dt1 = cc.getdashboardAging(26);
                    model._dashboardsAging.todayPending = dt1.Rows[0]["todayPending"].ToString();
                    model._dashboardsAging.NextTwoDays = dt1.Rows[0]["NextTwoDays"].ToString();
                    model._dashboardsAging.WithinTAT = dt1.Rows[0]["Within TAT"].ToString();
                    model._dashboardsAging.daysBT1_2 = dt1.Rows[0]["1-2 days BT"].ToString();
                    model._dashboardsAging.daysBT3_5 = dt1.Rows[0]["3-5 days BT"].ToString();
                    model._dashboardsAging.above5daysBT = dt1.Rows[0]["above 5 days BT"].ToString();
                   
                    model.Allocated1 = Convert.ToInt32(dt.Rows[0]["Allocated"].ToString());
                    model.UnAllocated1 = Convert.ToInt32(dt.Rows[0]["UnAllocated"].ToString());
                    model.Closed1 = Convert.ToInt32(dt.Rows[0]["Closed"].ToString());
                    model.Reopen1 = Convert.ToInt32(dt.Rows[0]["Reopen"].ToString());
                    model.Insufficency1 = Convert.ToInt32(dt.Rows[0]["Insufficency"].ToString());
                    model.WIP1 = Convert.ToInt32(dt.Rows[0]["WIP"].ToString());
                    model.Review = Convert.ToInt32(dt.Rows[0]["Review"].ToString());
                    model.SpocReviewPending = Convert.ToInt32(dt.Rows[0]["SpocReviewPending"]);
                    return model;                    
                }
                if (Designation == "Manager-SPL")
                {
                   DashboardManagerViewModel model = new DashboardManagerViewModel();
                    DataTable dt = new DataTable();
                    dt = cc.getdashboardOperationTLSummary("Special", TeamMemberRowId, "OP-Manager");
                    DataTable dt1 = new DataTable();
                    dt1 = cc.getdashboardAging(40);
                    model._dashboardsAging.todayPending = dt1.Rows[0]["todayPending"].ToString();
                    model._dashboardsAging.NextTwoDays = dt1.Rows[0]["NextTwoDays"].ToString();
                    model._dashboardsAging.WithinTAT = dt1.Rows[0]["Within TAT"].ToString();
                    model._dashboardsAging.daysBT1_2 = dt1.Rows[0]["1-2 days BT"].ToString();
                    model._dashboardsAging.daysBT3_5 = dt1.Rows[0]["3-5 days BT"].ToString();
                    model._dashboardsAging.above5daysBT = dt1.Rows[0]["above 5 days BT"].ToString();
                                        
                    model.Allocated1 = Convert.ToInt32(dt.Rows[0]["Allocated"].ToString());
                    model.UnAllocated1 = Convert.ToInt32(dt.Rows[0]["UnAllocated"].ToString());
                    model.Closed1 = Convert.ToInt32(dt.Rows[0]["Closed"].ToString());
                    model.Reopen1 = Convert.ToInt32(dt.Rows[0]["Reopen"].ToString());
                    model.Insufficency1 = Convert.ToInt32(dt.Rows[0]["Insufficency"].ToString());
                    model.WIP1 = Convert.ToInt32(dt.Rows[0]["WIP"].ToString());
                    model.Review = Convert.ToInt32(dt.Rows[0]["Review"].ToString());
                    model.SpocReviewPending = Convert.ToInt32(dt.Rows[0]["SpocReviewPending"]);
                    return model;                   
                }
                if (Designation == "Manager-PV")
                {
                    DashboardManagerViewModel model = new DashboardManagerViewModel();
                    DataTable dt = new DataTable();
                    dt = cc.getdashboardOperationTLSummary("Physical Verification", TeamMemberRowId, "PV-Manager");
                    DataTable dt1 = new DataTable();                   
                    model.Allocated1 = Convert.ToInt32(dt.Rows[0]["Allocated"].ToString());
                    model.UnAllocated1 = Convert.ToInt32(dt.Rows[0]["UnAllocated"].ToString());
                    model.Closed1 = Convert.ToInt32(dt.Rows[0]["Closed"].ToString());
                    model.Review = Convert.ToInt32(dt.Rows[0]["PVManagerReview"].ToString()); 

                    return model;
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DashboardTeamLeaderViewModel GetTeamLeaderDashBoardInfo(short TeamMemberRowId, string Designation,int department_id=0)
        {
            try
            {
                DateTime todayTATDate = DateTime.Now;
                DateTime totTodaysDue = todayTATDate;
                DateTime totDueinNext2days = todayTATDate.AddDays(2);
                DateTime totInTAT4days = todayTATDate.AddDays(4);
                DateTime totInTAT7days = todayTATDate.AddDays(7);
                DateTime totBT1days = todayTATDate.AddDays(-1);
                DateTime totBT2days = todayTATDate.AddDays(-2);
                DateTime totBT5days = todayTATDate.AddDays(-5);
                DateTime totBT10days = todayTATDate.AddDays(-10);
                
                if (Designation == "Team Leader-Add")
                {
                   DataTable dt = new DataTable();
                    dt = cc.getdashboardOperationAddressManagerSummary("Address", TeamMemberRowId,"OPTL");
                    DashboardTeamLeaderViewModel model = new DashboardTeamLeaderViewModel();
                    DataTable dt1 = new DataTable();
                    dt1 = cc.getdashboardAging(2);
                    model._dashboardsAging.todayPending = dt1.Rows[0]["todayPending"].ToString();
                    model._dashboardsAging.NextTwoDays = dt1.Rows[0]["NextTwoDays"].ToString();
                    model._dashboardsAging.WithinTAT = dt1.Rows[0]["Within TAT"].ToString();
                    model._dashboardsAging.daysBT1_2 = dt1.Rows[0]["1-2 days BT"].ToString();
                    model._dashboardsAging.daysBT3_5 = dt1.Rows[0]["3-5 days BT"].ToString();
                    model._dashboardsAging.above5daysBT = dt1.Rows[0]["above 5 days BT"].ToString();
                                      model.Allocated1 = Convert.ToInt32(dt.Rows[0]["Allocated"].ToString());
                    model.UnAllocated1 = Convert.ToInt32(dt.Rows[0]["UnAllocated"].ToString());
                    //model.YTR1 = Convert.ToInt32(dt.Rows[0]["YTR"].ToString());
                    model.Closed1 = Convert.ToInt32(dt.Rows[0]["Closed"].ToString());
                    model.Review1 = Convert.ToInt32(dt.Rows[0]["Review"].ToString());
                    model.PartnerClosed1 = Convert.ToInt32(dt.Rows[0]["PartnerClosed"].ToString());
                    return model;
                    //};
                }
                if (Designation == "Team Leader-Emp")
                {
                     DataTable dt = new DataTable();                   
                    dt = cc.getdashboardOperationTLSummary("Employment", TeamMemberRowId, "OPTL");
                    DashboardTeamLeaderViewModel model = new DashboardTeamLeaderViewModel();
                    DataTable dt1 = new DataTable();
                    dt1 = cc.getdashboardAging(12);
                    model._dashboardsAging.todayPending = dt1.Rows[0]["todayPending"].ToString();
                    model._dashboardsAging.NextTwoDays = dt1.Rows[0]["NextTwoDays"].ToString();
                    model._dashboardsAging.WithinTAT = dt1.Rows[0]["Within TAT"].ToString();
                    model._dashboardsAging.daysBT1_2 = dt1.Rows[0]["1-2 days BT"].ToString();
                    model._dashboardsAging.daysBT3_5 = dt1.Rows[0]["3-5 days BT"].ToString();
                    model._dashboardsAging.above5daysBT = dt1.Rows[0]["above 5 days BT"].ToString();

                    model.PVUnallocated1 = Convert.ToInt32(dt.Rows[0]["PVUnallocated"].ToString());
                    model.Allocated1 = Convert.ToInt32(dt.Rows[0]["Allocated"].ToString());
                    model.Review1 = Convert.ToInt32(dt.Rows[0]["Review"].ToString());
                    model.UnAllocated1 = Convert.ToInt32(dt.Rows[0]["UnAllocated"].ToString());
                    model.Reverification = Convert.ToInt32(dt.Rows[0]["Reverification"].ToString());
                    //model.YTR1 = Convert.ToInt32(dt.Rows[0]["YTR"].ToString());
                    model.Closed1 = Convert.ToInt32(dt.Rows[0]["Closed"].ToString());
                    model.totL2InsuffApproved= Convert.ToInt32(dt.Rows[0]["totL2InsuffApproved"].ToString());


                    model.InsufficentResolved = Convert.ToInt32(dt.Rows[0]["InsuffResolved"].ToString());

                    model.FundsResolved = Convert.ToInt32(dt.Rows[0]["FundsResolved"].ToString());
                    model.totRejectionWIP = Convert.ToInt32(dt.Rows[0]["Rejected"].ToString());
                    model.wipAll = Convert.ToInt32(dt.Rows[0]["WIPALL"].ToString());
                    model.SubNotMatch = Convert.ToInt32(dt.Rows[0]["SubjectMatch"].ToString());
                    DataTable dtAuto = new DataTable();
                    dtAuto = cc.getAuto_RevertSummaryTL();
                    model.Discarted = Convert.ToInt32(dtAuto.Rows[0]["discarted"].ToString());

                    return model;                    
                }
                if (Designation == "Team Leader-Edu")
                {
                   DataTable dt = new DataTable();
                    dt = cc.getdashboardOperationTLSummary("Education", TeamMemberRowId, "OPTL");
                    DashboardTeamLeaderViewModel model = new DashboardTeamLeaderViewModel();
                    DataTable dt1 = new DataTable();
                    dt1 = cc.getdashboardAging(11);
                    model._dashboardsAging.totalWIP = dt1.Rows[0]["totalWIP"].ToString();
                    model._dashboardsAging.todayPending = dt1.Rows[0]["todayPending"].ToString();
                    model._dashboardsAging.NextTwoDays = dt1.Rows[0]["NextTwoDays"].ToString();
                    model._dashboardsAging.WithinTAT = dt1.Rows[0]["Within TAT"].ToString();
                    model._dashboardsAging.daysBT1_2 = dt1.Rows[0]["1-2 days BT"].ToString();
                    model._dashboardsAging.daysBT3_5 = dt1.Rows[0]["3-5 days BT"].ToString();
                    model._dashboardsAging.above5daysBT = dt1.Rows[0]["above 5 days BT"].ToString();
                                       
                    model.Allocated1 = Convert.ToInt32(dt.Rows[0]["Allocated"].ToString());
                    model.UnAllocated1 = Convert.ToInt32(dt.Rows[0]["UnAllocated"].ToString());
                    //model.YTR1 = Convert.ToInt32(dt.Rows[0]["YTR"].ToString());
                    model.Closed1 = Convert.ToInt32(dt.Rows[0]["Closed"].ToString());
                    model.Review1 = Convert.ToInt32(dt.Rows[0]["Review"].ToString());
                    model.InsufficentResolved = Convert.ToInt32(dt.Rows[0]["InsuffResolved"].ToString());
                    model.Reverification = Convert.ToInt32(dt.Rows[0]["Reverification"].ToString());
                    model.FundsResolved = Convert.ToInt32(dt.Rows[0]["FundsResolved"].ToString());
                    model.totRejectionWIP = Convert.ToInt32(dt.Rows[0]["Rejected"].ToString());
                    model.totInsufficiency = Convert.ToInt32(dt.Rows[0]["totInsufficiency"].ToString());
                    model.totFundPending = Convert.ToInt32(dt.Rows[0]["totFundPending"].ToString());
                    model.totL2InsuffApproved = Convert.ToInt32(dt.Rows[0]["totL2InsuffApproved"].ToString());
                    model.PVUnAllocated = dt.Columns.Contains("PVUnAllocated") ?  Convert.ToInt32(dt.Rows[0]["PVUnAllocated"].ToString()) : 0;
                    model.CVUnAllocated = dt.Columns.Contains("CVUnAllocated") ?  Convert.ToInt32(dt.Rows[0]["CVUnAllocated"].ToString()) : 0;

                    //---------------------------------------------------------------
                    DataTable DTAuto = new DataTable();
                    DTAuto = cc.GetEducationAutoWIPAllocatetoTL(TeamMemberRowId);      
                    
                    model.initialMailSent = Convert.ToInt32(DTAuto.Rows[0]["InitialMailsend"].ToString());
                    model.Reminder1 = Convert.ToInt32(DTAuto.Rows[0]["SentReminder1"].ToString());
                    model.Reminder2 = Convert.ToInt32(DTAuto.Rows[0]["SentReminder2"].ToString());
                    model.Reminder3 = Convert.ToInt32(DTAuto.Rows[0]["SentReminder3"].ToString());
                    model.Reminder4 = Convert.ToInt32(DTAuto.Rows[0]["SentReminder4"].ToString());
                    model.Reminder5 = Convert.ToInt32(DTAuto.Rows[0]["SentReminder5"].ToString());
                    model.GreaterThan5 = Convert.ToInt32(DTAuto.Rows[0]["GreaterThan5"].ToString());
                    model.ErrorInMail = Convert.ToInt32(DTAuto.Rows[0]["ErrorInMail"].ToString());
                    model.wipAll = Convert.ToInt32(DTAuto.Rows[0]["wipAll"].ToString());
                    model.Bounce = Convert.ToInt32(DTAuto.Rows[0]["Bounce"].ToString());
                    model.WIPAllEdu = Convert.ToInt32(DTAuto.Rows[0]["WIPAllEdu"].ToString());
                                       

                    return model;                   
                }
                if (Designation == "Team Leader-Cri")
                {
                    DashboardTeamLeaderViewModel model = new DashboardTeamLeaderViewModel();
                    DataTable dt = new DataTable();
                    dt = cc.getdashboardOperationTLSummary("Criminal", TeamMemberRowId, "OPTL");
                    DataTable dt1 = new DataTable();
                    dt1 = cc.getdashboardAging(5);
                    model._dashboardsAging.todayPending = dt1.Rows[0]["todayPending"].ToString();
                    model._dashboardsAging.NextTwoDays = dt1.Rows[0]["NextTwoDays"].ToString();
                    model._dashboardsAging.WithinTAT = dt1.Rows[0]["Within TAT"].ToString();
                    model._dashboardsAging.daysBT1_2 = dt1.Rows[0]["1-2 days BT"].ToString();
                    model._dashboardsAging.daysBT3_5 = dt1.Rows[0]["3-5 days BT"].ToString();
                    model._dashboardsAging.above5daysBT = dt1.Rows[0]["above 5 days BT"].ToString();                   
                                        model.Allocated1 = Convert.ToInt32(dt.Rows[0]["Allocated"].ToString());
                    model.UnAllocated1 = Convert.ToInt32(dt.Rows[0]["UnAllocated"].ToString());
                    model.totL2InsuffApproved = Convert.ToInt32(dt.Rows[0]["totL2InsuffApproved"].ToString());
                    //model.YTR1 = Convert.ToInt32(dt.Rows[0]["YTR"].ToString());
                    model.Closed1 = Convert.ToInt32(dt.Rows[0]["Closed"].ToString());
                    model.Review1 = Convert.ToInt32(dt.Rows[0]["Review"].ToString());
                     return model;                   
                }
                if (Designation == "Team Leader-Ref")
                {
                    DataTable dt = new DataTable();
                    dt = cc.getdashboardOperationTLSummary("Reference", TeamMemberRowId, "OPTL");
                    DashboardTeamLeaderViewModel model = new DashboardTeamLeaderViewModel();
                    DataTable dt1 = new DataTable();
                    dt1 = cc.getdashboardAging(30);
                    model._dashboardsAging.todayPending = dt1.Rows[0]["todayPending"].ToString();
                    model._dashboardsAging.NextTwoDays = dt1.Rows[0]["NextTwoDays"].ToString();
                    model._dashboardsAging.WithinTAT = dt1.Rows[0]["Within TAT"].ToString();
                    model._dashboardsAging.daysBT1_2 = dt1.Rows[0]["1-2 days BT"].ToString();
                    model._dashboardsAging.daysBT3_5 = dt1.Rows[0]["3-5 days BT"].ToString();
                    model._dashboardsAging.above5daysBT = dt1.Rows[0]["above 5 days BT"].ToString();
                    
                    model.Allocated1 = Convert.ToInt32(dt.Rows[0]["Allocated"].ToString());
                    model.UnAllocated1 = Convert.ToInt32(dt.Rows[0]["UnAllocated"].ToString());
                    //model.YTR1 = Convert.ToInt32(dt.Rows[0]["YTR"].ToString());
                    model.Closed1 = Convert.ToInt32(dt.Rows[0]["Closed"].ToString());
                    model.Review1 = Convert.ToInt32(dt.Rows[0]["Review"].ToString());
                    model.totL2InsuffApproved = Convert.ToInt32(dt.Rows[0]["totL2InsuffApproved"].ToString());
                    return model;                   
                }
                if (Designation == "Team Leader-Ide")
                {
                    DataTable dt = new DataTable();
                    dt = cc.getdashboardOperationTLSummary("National", TeamMemberRowId, "OPTL");
                    DashboardTeamLeaderViewModel model = new DashboardTeamLeaderViewModel();
                    DataTable dt1 = new DataTable();
                    dt1 = cc.getdashboardAging(26);
                    model._dashboardsAging.todayPending = dt1.Rows[0]["todayPending"].ToString();
                    model._dashboardsAging.NextTwoDays = dt1.Rows[0]["NextTwoDays"].ToString();
                    model._dashboardsAging.WithinTAT = dt1.Rows[0]["Within TAT"].ToString();
                    model._dashboardsAging.daysBT1_2 = dt1.Rows[0]["1-2 days BT"].ToString();
                    model._dashboardsAging.daysBT3_5 = dt1.Rows[0]["3-5 days BT"].ToString();
                    model._dashboardsAging.above5daysBT = dt1.Rows[0]["above 5 days BT"].ToString();

                    model.Allocated1 = Convert.ToInt32(dt.Rows[0]["Allocated"].ToString());
                    model.UnAllocated1 = Convert.ToInt32(dt.Rows[0]["UnAllocated"].ToString());
                    model.Review1 = Convert.ToInt32(dt.Rows[0]["Review"].ToString());
                    //model.YTR1 = Convert.ToInt32(dt.Rows[0]["YTR"].ToString());
                    model.Closed1 = Convert.ToInt32(dt.Rows[0]["Closed"].ToString());
                    model.totL2InsuffApproved = Convert.ToInt32(dt.Rows[0]["totL2InsuffApproved"].ToString());
                    return model;                    
                }
                if (Designation == "Team Leader-SPL")
                {
                    DataTable dt = new DataTable();
                    dt = cc.getdashboardOperationTLSummary("Special", TeamMemberRowId, "OPTL");
                    DashboardTeamLeaderViewModel model = new DashboardTeamLeaderViewModel();
                    DataTable dt1 = new DataTable();
                    dt1 = cc.getdashboardAging(40);
                    model._dashboardsAging.todayPending = dt1.Rows[0]["todayPending"].ToString();
                    model._dashboardsAging.NextTwoDays = dt1.Rows[0]["NextTwoDays"].ToString();
                    model._dashboardsAging.WithinTAT = dt1.Rows[0]["Within TAT"].ToString();
                    model._dashboardsAging.daysBT1_2 = dt1.Rows[0]["1-2 days BT"].ToString();
                    model._dashboardsAging.daysBT3_5 = dt1.Rows[0]["3-5 days BT"].ToString();
                    model._dashboardsAging.above5daysBT = dt1.Rows[0]["above 5 days BT"].ToString();
                                        
                    model.Allocated1 = Convert.ToInt32(dt.Rows[0]["Allocated"].ToString());
                    model.UnAllocated1 = Convert.ToInt32(dt.Rows[0]["UnAllocated"].ToString());
                    //model.YTR1 = Convert.ToInt32(dt.Rows[0]["YTR"].ToString());
                    model.Closed1 = Convert.ToInt32(dt.Rows[0]["Closed"].ToString());
                    model.Review1 = Convert.ToInt32(dt.Rows[0]["Review"].ToString());
                    model.totL2InsuffApproved = Convert.ToInt32(dt.Rows[0]["totL2InsuffApproved"].ToString());
                    return model;                    
                }
                if (Designation == "Team Leader-PV")
                {
                    DataTable dt = new DataTable();
                    dt = cc.getdashboardOperationTLSummary("Physical Verification", TeamMemberRowId, "PVTL");
                    DashboardTeamLeaderViewModel model = new DashboardTeamLeaderViewModel();                  
                    model.Allocated1 = Convert.ToInt32(dt.Rows[0]["Allocated"].ToString());
                    model.UnAllocated1 = Convert.ToInt32(dt.Rows[0]["UnAllocated"].ToString());                  
                    model.Closed1 = Convert.ToInt32(dt.Rows[0]["Closed"].ToString());
                    model.Review1 = Convert.ToInt32(dt.Rows[0]["PVTLReview"].ToString());
                    return model;
                }

                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        


        public DashboardTeamMemberViewModel GetTeamMemberDashBoardInfo(short TeamMemberRowId, string Designation, int department_id = 0)
        {
            try
            {
                DateTime todayTATDate = DateTime.Now;
                DateTime totDueinNext2days = todayTATDate.AddDays(2);
                DateTime totInTAT4days = todayTATDate.AddDays(4);
                DateTime totInTAT7days = todayTATDate.AddDays(7);
                DateTime totBT1days = todayTATDate.AddDays(-1);
                //DateTime totBT2days = todayTATDate.AddDays(-2);
                DateTime totBT5days = todayTATDate.AddDays(-5);

                if (Designation == "Team Member-Add")
                {
                    DataTable dt = new DataTable();
                    dt = cc.getdashboardOperationAddressManagerSummary("Address", TeamMemberRowId,"OPTM");
                    DashboardTeamMemberViewModel model = new DashboardTeamMemberViewModel();
                    DataTable dt1 = new DataTable();
                    dt1 = cc.getdashboardAging(2);
                    model._dashboardsAging.todayPending = dt1.Rows[0]["todayPending"].ToString();
                    model._dashboardsAging.NextTwoDays = dt1.Rows[0]["NextTwoDays"].ToString();
                    model._dashboardsAging.WithinTAT = dt1.Rows[0]["Within TAT"].ToString();
                    model._dashboardsAging.daysBT1_2 = dt1.Rows[0]["1-2 days BT"].ToString();
                    model._dashboardsAging.daysBT3_5 = dt1.Rows[0]["3-5 days BT"].ToString();
                    model._dashboardsAging.above5daysBT = dt1.Rows[0]["above 5 days BT"].ToString();
                                       
                    model.Pending1 = Convert.ToInt32(dt.Rows[0]["Pending"].ToString());
                    model.WIP1 = Convert.ToInt32(dt.Rows[0]["WIP"].ToString());
                    model.PartnerClosed1 = Convert.ToInt32(dt.Rows[0]["PartnerClosed"].ToString());
                    model.InsufficentRaised1 = Convert.ToInt32(dt.Rows[0]["InsufficentRaised"].ToString());
                    model.InsufficencyResolved1 = Convert.ToInt32(dt.Rows[0]["InsufficentResolved"].ToString());
                    model.Closed1 = Convert.ToInt32(dt.Rows[0]["Closed"].ToString());
                    model.totRejectionWIP1 = Convert.ToInt32(dt.Rows[0]["Rejected"].ToString());
                    model.FieldPartnerClosed= Convert.ToInt32(dt.Rows[0]["FieldPartnerClosed"].ToString());
                    model.SpocReviewResolved= Convert.ToInt32(dt.Rows[0]["SpocReview"].ToString());
                    model.NotDurable = Convert.ToInt32(dt.Rows[0]["NotDurable"].ToString());
                    model.PartnerRejected = Convert.ToInt32(dt.Rows[0]["PartnerRejected"].ToString());
                    model.Reverification=Convert.ToInt32(dt.Rows[0]["Reverification"].ToString());
                    
                    return model;                   
                }
                else if (Designation == "Team Member-Emp")
                {
                   DataTable dt = new DataTable();
                    dt = cc.getdashboardOperationTLSummary("Employment", TeamMemberRowId, "OPTM");
                    DashboardTeamMemberViewModel model = new DashboardTeamMemberViewModel();
                    DataTable DTAuto = new DataTable();
                    DataTable dt1 = new DataTable();
                    dt1 = cc.getdashboardAging(12,20);
                    DTAuto= cc.GetAutoWIPAllocatetoTM(TeamMemberRowId);
                    model._dashboardsAging.todayPending = dt1.Rows[0]["todayPending"].ToString();
                    model._dashboardsAging.NextTwoDays = dt1.Rows[0]["NextTwoDays"].ToString();
                    model._dashboardsAging.WithinTAT = dt1.Rows[0]["Within TAT"].ToString();
                    model._dashboardsAging.daysBT1_2 = dt1.Rows[0]["1-2 days BT"].ToString();
                    model._dashboardsAging.daysBT3_5 = dt1.Rows[0]["3-5 days BT"].ToString();
                    model._dashboardsAging.above5daysBT = dt1.Rows[0]["above 5 days BT"].ToString();
                                     
                    model.Pending1 = Convert.ToInt32(dt.Rows[0]["Pending"].ToString());
                    model.WIP1 = Convert.ToInt32(dt.Rows[0]["WIP"].ToString());

                    model.PVUnallocated =  Convert.ToInt32(dt.Rows[0]["PVUnallocated"].ToString());
                    model.PartnerClosed1 = Convert.ToInt32(dt.Rows[0]["PartnerClosedNew"].ToString());
                    model.PartnerRejected = Convert.ToInt32(dt.Rows[0]["PartnerRejectedNew"].ToString());
                    model.FieldPartnerClosed = Convert.ToInt32(dt.Rows[0]["FieldPartnerClosedNew"].ToString());

                    model.InsufficentRaised1 = Convert.ToInt32(dt.Rows[0]["InsufficentRaised"].ToString());
                    model.InsufficencyResolved1 = Convert.ToInt32(dt.Rows[0]["InsufficentResolved"].ToString());
                    model.Closed1 = Convert.ToInt32(dt.Rows[0]["Closed"].ToString());
                    model.FundsResolved1 = Convert.ToInt32(dt.Rows[0]["FundsResolved"].ToString());
                    model.FundsPending1 = Convert.ToInt32(dt.Rows[0]["FundsPending"].ToString());
                    model.totRejectionWIP1 = Convert.ToInt32(dt.Rows[0]["Rejected"].ToString());
                    model.totPVClosed1= Convert.ToInt32(dt.Rows[0]["PVClose"].ToString());
                    model.AutoEmailer = Convert.ToInt32(dt.Rows[0]["AutoEmailer"].ToString());
                    model.RevNotRec = Convert.ToInt32(dt.Rows[0]["RevNotRec"].ToString());
                    model.Reverification = Convert.ToInt32(dt.Rows[0]["Reverification"].ToString());
                    model.totPVClosed= Convert.ToInt32(dt.Rows[0]["PVComplete"].ToString());
                    model.totPVWIP= Convert.ToInt32(dt.Rows[0]["PVIssue"].ToString());
                    model.SpocReviewResolved = Convert.ToInt32(dt.Rows[0]["SpocReview"].ToString());

                    model.AutoRevertRejected = Convert.ToInt32(DTAuto.Rows[0]["RevertRejected"].ToString());

                    model.initialMailSent = Convert.ToInt32(DTAuto.Rows[0]["InitialMailsend"].ToString());
                    model.Reminder1 = Convert.ToInt32(DTAuto.Rows[0]["SentReminder1"].ToString());
                    model.Reminder2 = Convert.ToInt32(DTAuto.Rows[0]["SentReminder2"].ToString());
                    model.Reminder3 = Convert.ToInt32(DTAuto.Rows[0]["SentReminder3"].ToString());
                    model.Reminder4 = Convert.ToInt32(DTAuto.Rows[0]["SentReminder4"].ToString());
                    model.Reminder5 = Convert.ToInt32(DTAuto.Rows[0]["SentReminder5"].ToString());
                    model.GreaterThan5 = Convert.ToInt32(DTAuto.Rows[0]["GreaterThan5"].ToString());
                    model.ErrorInMail = Convert.ToInt32(DTAuto.Rows[0]["ErrorInMail"].ToString());
                    model.wipAll = Convert.ToInt32(DTAuto.Rows[0]["wipAll"].ToString());
                    model.Bounce = Convert.ToInt32(DTAuto.Rows[0]["Bounce"].ToString());
                 return model;                    
                }

                else if (Designation == "Team Member-Edu")
                {
                   DataTable dt = new DataTable();
                    dt = cc.getdashboardOperationTLSummary("Education", TeamMemberRowId, "OPTM");
                    DashboardTeamMemberViewModel model = new DashboardTeamMemberViewModel();
                    DataTable dt1 = new DataTable();
                    dt1 = cc.getdashboardAging(11,20);
                    model._dashboardsAging.todayPending = dt1.Rows[0]["todayPending"].ToString();
                    model._dashboardsAging.NextTwoDays = dt1.Rows[0]["NextTwoDays"].ToString();
                    model._dashboardsAging.WithinTAT = dt1.Rows[0]["Within TAT"].ToString();
                    model._dashboardsAging.daysBT1_2 = dt1.Rows[0]["1-2 days BT"].ToString();
                    model._dashboardsAging.daysBT3_5 = dt1.Rows[0]["3-5 days BT"].ToString();
                    model._dashboardsAging.above5daysBT = dt1.Rows[0]["above 5 days BT"].ToString();
                                       
                    model.Pending1 = Convert.ToInt32(dt.Rows[0]["WIP"].ToString());
                    model.InsufficentRaised1 = Convert.ToInt32(dt.Rows[0]["InsufficentRaised"].ToString());
                    model.InsufficencyResolved1 = Convert.ToInt32(dt.Rows[0]["InsufficentResolved"].ToString());
                    model.FundsPending1 = Convert.ToInt32(dt.Rows[0]["FundsPending"].ToString());
                    model.Research1 = Convert.ToInt32(dt.Rows[0]["Research"].ToString());
                    //model.YTR1 = Convert.ToInt32(dt.Rows[0]["YTR"].ToString());
                    model.Closed1 = Convert.ToInt32(dt.Rows[0]["Closed"].ToString());
                    model.FundsResolved1= Convert.ToInt32(dt.Rows[0]["Fundsresolved"].ToString());
                    model.totRejectionWIP1 = Convert.ToInt32(dt.Rows[0]["Rejected"].ToString());
                    model.totPVClosed1 = Convert.ToInt32(dt.Rows[0]["PVClose"].ToString());
                    model.totPVWIP=Convert.ToInt32(dt.Rows[0]["PVAllocated"].ToString());
                    model.SpocReviewResolved = Convert.ToInt32(dt.Rows[0]["SpocReview"].ToString());
                    model.RevertRec = Convert.ToInt32(dt.Rows[0]["RevertRec"].ToString());
                    if (Convert.ToString(dt.Rows[0]["TOTPVClose"])!="")
                    {
                        model.totPVClosed = Convert.ToInt32(dt.Rows[0]["TOTPVClose"]);
                    }
                    else
                    {
                        model.totPVClosed = 0;
                    }
                    
                    DataTable DTAuto = new DataTable();

                    DTAuto = cc.GetEducationAutoWIPAllocatetoTM(TeamMemberRowId);
                    model.AutoRevertRejected = Convert.ToInt32(DTAuto.Rows[0]["RevertRejected"].ToString());

                    model.initialMailSent = Convert.ToInt32(DTAuto.Rows[0]["InitialMailsend"].ToString());
                    model.Reminder1 = Convert.ToInt32(DTAuto.Rows[0]["SentReminder1"].ToString());
                    model.Reminder2 = Convert.ToInt32(DTAuto.Rows[0]["SentReminder2"].ToString());
                    model.Reminder3 = Convert.ToInt32(DTAuto.Rows[0]["SentReminder3"].ToString());
                    model.Reminder4 = Convert.ToInt32(DTAuto.Rows[0]["SentReminder4"].ToString());
                    model.Reminder5 = Convert.ToInt32(DTAuto.Rows[0]["SentReminder5"].ToString());
                    model.GreaterThan5 = Convert.ToInt32(DTAuto.Rows[0]["GreaterThan5"].ToString());
                    model.ErrorInMail = Convert.ToInt32(DTAuto.Rows[0]["ErrorInMail"].ToString());
                    model.wipAll = Convert.ToInt32(DTAuto.Rows[0]["wipAll"].ToString());
                    model.Bounce = Convert.ToInt32(DTAuto.Rows[0]["Bounce"].ToString());
                    model.WIPAllEdu = Convert.ToInt32(DTAuto.Rows[0]["WIPAllEdu"].ToString());
                     return model;                   
                }
                else if (Designation == "Team Member-Cri")
                {
                   DataTable dt = new DataTable();
                    dt = cc.getdashboardOperationTLSummary("Criminal", TeamMemberRowId, "OPTM");
                    DashboardTeamMemberViewModel model = new DashboardTeamMemberViewModel();
                    DataTable dt1 = new DataTable();
                    dt1 = cc.getdashboardAging(5,20);
                    model._dashboardsAging.todayPending = dt1.Rows[0]["todayPending"].ToString();
                    model._dashboardsAging.NextTwoDays = dt1.Rows[0]["NextTwoDays"].ToString();
                    model._dashboardsAging.WithinTAT = dt1.Rows[0]["Within TAT"].ToString();
                    model._dashboardsAging.daysBT1_2 = dt1.Rows[0]["1-2 days BT"].ToString();
                    model._dashboardsAging.daysBT3_5 = dt1.Rows[0]["3-5 days BT"].ToString();
                    model._dashboardsAging.above5daysBT = dt1.Rows[0]["above 5 days BT"].ToString();
                                       
                    model.Pending1 = Convert.ToInt32(dt.Rows[0]["Pending"].ToString());
                    model.WIP1 = Convert.ToInt32(dt.Rows[0]["WIP"].ToString());
                    model.FundsPending1 = Convert.ToInt32(dt.Rows[0]["FundsPending"].ToString());
                    model.Research1 = Convert.ToInt32(dt.Rows[0]["Research"].ToString());
                    model.Closed1 = Convert.ToInt32(dt.Rows[0]["Closed"].ToString());
                    model.InsufficentRaised1 = Convert.ToInt32(dt.Rows[0]["InsufficentRaised"].ToString());
                    model.InsufficencyResolved1 = Convert.ToInt32(dt.Rows[0]["InsufficentResolved"].ToString());
                    model.totRejectionWIP1 = Convert.ToInt32(dt.Rows[0]["Rejected"].ToString());
                    model.totPVClosed1 = Convert.ToInt32(dt.Rows[0]["PVClose"].ToString());
                    model.Reverification= Convert.ToInt32(dt.Rows[0]["Reverification"].ToString());
                    model.FundsResolved1 = Convert.ToInt32(dt.Rows[0]["FundsResolved"].ToString());
                    model.SpocReviewResolved = Convert.ToInt32(dt.Rows[0]["SpocReview"].ToString());
                    model.TotTMReview = Convert.ToInt32(dt.Rows[0]["OPTMReview"].ToString());
                    model.DBCheckAPIFailed = Convert.ToInt32(dt.Rows[0]["DBCheckAPIFailed"].ToString());
                    return model;                    
                }
                else if (Designation == "Team Member-Ref")
                {
                   DataTable dt = new DataTable();
                    dt = cc.getdashboardOperationTLSummary("Reference", TeamMemberRowId, "OPTM");
                    DashboardTeamMemberViewModel model = new DashboardTeamMemberViewModel();
                    DataTable dt1 = new DataTable();
                    dt1 = cc.getdashboardAging(30,20);
                    model._dashboardsAging.todayPending = dt1.Rows[0]["todayPending"].ToString();
                    model._dashboardsAging.NextTwoDays = dt1.Rows[0]["NextTwoDays"].ToString();
                    model._dashboardsAging.WithinTAT = dt1.Rows[0]["Within TAT"].ToString();
                    model._dashboardsAging.daysBT1_2 = dt1.Rows[0]["1-2 days BT"].ToString();
                    model._dashboardsAging.daysBT3_5 = dt1.Rows[0]["3-5 days BT"].ToString();
                    model._dashboardsAging.above5daysBT = dt1.Rows[0]["above 5 days BT"].ToString();
                                       
                    model.InsufficentRaised1 = Convert.ToInt32(dt.Rows[0]["InsufficentRaised"].ToString());
                    model.InsufficencyResolved1 = Convert.ToInt32(dt.Rows[0]["InsufficentResolved"].ToString());
                    model.WIP1 = Convert.ToInt32(dt.Rows[0]["WIP"].ToString());
                    model.Closed1 = Convert.ToInt32(dt.Rows[0]["Closed"].ToString());
                    model.totRejectionWIP1 = Convert.ToInt32(dt.Rows[0]["Rejected"].ToString());
                    model.SpocReviewResolved = Convert.ToInt32(dt.Rows[0]["SpocReview"].ToString());
                    model.Reverification = Convert.ToInt32(dt.Rows[0]["Reverification"].ToString());                   


                    model.PartnerClosed1 = Convert.ToInt32(dt.Rows[0]["PartnerClosed"].ToString());                   
                    model.FundsResolved1 = Convert.ToInt32(dt.Rows[0]["FundsResolved"].ToString());
                    model.FundsPending1 = Convert.ToInt32(dt.Rows[0]["FundsPending"].ToString());                   
                    model.totPVClosed1 = Convert.ToInt32(dt.Rows[0]["PVClose"].ToString());
                    model.AutoEmailer = Convert.ToInt32(dt.Rows[0]["AutoEmailer"].ToString());
                    model.RevNotRec = Convert.ToInt32(dt.Rows[0]["RevNotRec"].ToString());                 
                    model.totPVClosed = Convert.ToInt32(dt.Rows[0]["PVComplete"].ToString());
                    model.totPVWIP = Convert.ToInt32(dt.Rows[0]["PVIssue"].ToString());
                    model.Pending1 = Convert.ToInt32(dt.Rows[0]["Pending"].ToString());


                    DataTable DTAuto = new DataTable();


                    DTAuto = cc.GetReferenceAutoWIPAllocatetoTM(TeamMemberRowId);
                    model.initialMailSent = Convert.ToInt32(DTAuto.Rows[0]["InitialMailsend"].ToString());
                    model.Reminder1 = Convert.ToInt32(DTAuto.Rows[0]["SentReminder1"].ToString());
                    model.Reminder2 = Convert.ToInt32(DTAuto.Rows[0]["SentReminder2"].ToString());
                    model.Reminder3 = Convert.ToInt32(DTAuto.Rows[0]["SentReminder3"].ToString());
                    model.Reminder4 = Convert.ToInt32(DTAuto.Rows[0]["SentReminder4"].ToString());
                    model.Reminder5 = Convert.ToInt32(DTAuto.Rows[0]["SentReminder5"].ToString());
                    model.GreaterThan5 = Convert.ToInt32(DTAuto.Rows[0]["GreaterThan5"].ToString());
                    model.ErrorInMail = Convert.ToInt32(DTAuto.Rows[0]["ErrorInMail"].ToString());
                    model.wipAll = Convert.ToInt32(DTAuto.Rows[0]["wipAll"].ToString());
                    model.Bounce = Convert.ToInt32(DTAuto.Rows[0]["Bounce"].ToString());
                    return model;                   
                }
                else if (Designation == "Team Member-Ide")
                {
                    DataTable dt = new DataTable();
                    dt = cc.getdashboardOperationTLSummary("National", TeamMemberRowId, "OPTM");
                    DashboardTeamMemberViewModel model = new DashboardTeamMemberViewModel();

                    DataTable dt1 = new DataTable();
                    dt1 = cc.getdashboardAging(26,20);
                    model._dashboardsAging.todayPending = dt1.Rows[0]["todayPending"].ToString();
                    model._dashboardsAging.NextTwoDays = dt1.Rows[0]["NextTwoDays"].ToString();
                    model._dashboardsAging.WithinTAT = dt1.Rows[0]["Within TAT"].ToString();
                    model._dashboardsAging.daysBT1_2 = dt1.Rows[0]["1-2 days BT"].ToString();
                    model._dashboardsAging.daysBT3_5 = dt1.Rows[0]["3-5 days BT"].ToString();
                    model._dashboardsAging.above5daysBT = dt1.Rows[0]["above 5 days BT"].ToString();

                    model.Pending1 = Convert.ToInt32(dt.Rows[0]["Pending"].ToString());
                    model.WIP1 = Convert.ToInt32(dt.Rows[0]["WIP"].ToString());
                    model.PartnerClosed1 = Convert.ToInt32(dt.Rows[0]["PartnerClosed"].ToString());
                    model.InsufficentRaised1 = Convert.ToInt32(dt.Rows[0]["InsufficentRaised"].ToString());
                    model.InsufficencyResolved1 = Convert.ToInt32(dt.Rows[0]["InsufficentResolved"].ToString());
                    model.Closed1 = Convert.ToInt32(dt.Rows[0]["Closed"].ToString());
                   // model.FundsResolved1 = Convert.ToInt32(dt.Rows[0]["FundsResolved"].ToString());
                    model.FundsPending1 = Convert.ToInt32(dt.Rows[0]["FundsPending"].ToString());
                    model.totRejectionWIP1 = Convert.ToInt32(dt.Rows[0]["Rejected"].ToString());
                    model.Reverification = Convert.ToInt32(dt.Rows[0]["Reverification"].ToString());
                    model.APIChecks = Convert.ToInt32(dt.Rows[0]["APIChecks"].ToString());
                    model.SpocReviewResolved = Convert.ToInt32(dt.Rows[0]["SpocReview"].ToString());
                    return model;                    
                }
                else if (Designation == "Team Member-SPL")
                {
                    DataTable dt = new DataTable();
                    dt = cc.getdashboardOperationTLSummary("Special", TeamMemberRowId, "OPTM");
                    DashboardTeamMemberViewModel model = new DashboardTeamMemberViewModel();
                    DataTable dt1 = new DataTable();
                    dt1 = cc.getdashboardAging(40,20);
                    model._dashboardsAging.todayPending = dt1.Rows[0]["todayPending"].ToString();
                    model._dashboardsAging.NextTwoDays = dt1.Rows[0]["NextTwoDays"].ToString();
                    model._dashboardsAging.WithinTAT = dt1.Rows[0]["Within TAT"].ToString();
                    model._dashboardsAging.daysBT1_2 = dt1.Rows[0]["1-2 days BT"].ToString();
                    model._dashboardsAging.daysBT3_5 = dt1.Rows[0]["3-5 days BT"].ToString();
                    model._dashboardsAging.above5daysBT = dt1.Rows[0]["above 5 days BT"].ToString();
                                        

                    model.Pending1 = Convert.ToInt32(dt.Rows[0]["Pending"].ToString());
                    model.WIP1 = Convert.ToInt32(dt.Rows[0]["WIP"].ToString());
                    model.PartnerClosed1 = Convert.ToInt32(dt.Rows[0]["PartnerClosed"].ToString());
                    model.InsufficentRaised1 = Convert.ToInt32(dt.Rows[0]["InsufficentRaised"].ToString());
                    model.InsufficencyResolved1 = Convert.ToInt32(dt.Rows[0]["InsufficentResolved"].ToString());
                    model.Closed1 = Convert.ToInt32(dt.Rows[0]["Closed"].ToString());
                    model.totRejectionWIP1 = Convert.ToInt32(dt.Rows[0]["Rejected"].ToString());
                    model.Reverification = Convert.ToInt32(dt.Rows[0]["Reverification"].ToString());
                    model.APIChecks = Convert.ToInt32(dt.Rows[0]["APIChecks"].ToString());
                    model.SpocReviewResolved = Convert.ToInt32(dt.Rows[0]["SpocReview"].ToString());
                    model.TotTMReview = Convert.ToInt32(dt.Rows[0]["OPTMReview"].ToString());
                    model.DBCheckAPIFailed = Convert.ToInt32(dt.Rows[0]["DBCheckAPIFailed"].ToString());
                    model.FundsPending1 = Convert.ToInt32(dt.Rows[0]["FundsPending"].ToString());
                    model.FundsResolved1 = Convert.ToInt32(dt.Rows[0]["FundsResolved"].ToString());
                    return model;                    
                }
                else if(Designation == "Auto Revert-QC")
                {
                    DataTable dt = new DataTable();
                    dt = cc.getAuto_RevertSummary();
                    DashboardTeamMemberViewModel model = new DashboardTeamMemberViewModel();
                    model.AutoEmailer = Convert.ToInt32(dt.Rows[0]["RevertRec"].ToString());
                    model.RevNotRec = Convert.ToInt32(dt.Rows[0]["SubNotMatch"].ToString());
                    model.wipAll = Convert.ToInt32(dt.Rows[0]["WIPAll"].ToString());
                    model.Discarted = Convert.ToInt32(dt.Rows[0]["discarted"].ToString());

                }
                else if (Designation == "Team Member-PV")
                {
                    DataTable dt = new DataTable();
                    dt = cc.getdashboardOperationTLSummary("Physical Verification", TeamMemberRowId, "PVTM");
                    DashboardTeamMemberViewModel model = new DashboardTeamMemberViewModel();

                    model.Pending1 = Convert.ToInt32(dt.Rows[0]["Pending"].ToString());
                    model.WIP1 = Convert.ToInt32(dt.Rows[0]["WIP"].ToString());
                    model.PartnerClosed1 = Convert.ToInt32(dt.Rows[0]["PartnerClosed"].ToString());
                    model.PartnerRejected = Convert.ToInt32(dt.Rows[0]["PartnerRejected"].ToString());
                    model.Closed1= Convert.ToInt32(dt.Rows[0]["PVTMClosed"].ToString());
                    return model;
                }
                //
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public DashboardTeamMemberQCViewModel GetTeamMemberQCDashBoardInfo(short TeamMemberRowId, string Designation,int department_id=0)
        {
            try
            {
                DateTime todayTATDate = DateTime.Now;
                DateTime totTodaysDue = todayTATDate;
                TimeSpan totWIP2Hrs = todayTATDate.AddDays(2).TimeOfDay;
                TimeSpan totWIP6Hrs = todayTATDate.AddDays(6).TimeOfDay;
                TimeSpan totWIP12Hrs = todayTATDate.AddDays(12).TimeOfDay;
                TimeSpan totWIP24Hrs = todayTATDate.AddDays(24).TimeOfDay;
                TimeSpan totWIP24HrsPlus = todayTATDate.AddDays(24).TimeOfDay;
                TimeSpan currentTime = DateTime.Now.TimeOfDay;

                if (Designation == "Team Member-Add-QC")
                {
                    
                    DataTable dt = new DataTable();
                    dt = cc.getdashboardOperationTLSummary("Address", TeamMemberRowId, "OPQC");
                    DashboardTeamMemberQCViewModel model = new DashboardTeamMemberQCViewModel();
                    DataTable dt1 = new DataTable();
                    dt1 = cc.getdashboardAging(2,24);
                    model._dashboardsAging.todayPending = dt1.Rows[0]["todayPending"].ToString();
                    model._dashboardsAging.NextTwoDays = dt1.Rows[0]["NextTwoDays"].ToString();
                    model._dashboardsAging.WithinTAT = dt1.Rows[0]["Within TAT"].ToString();
                    model._dashboardsAging.daysBT1_2 = dt1.Rows[0]["1-2 days BT"].ToString();
                    model._dashboardsAging.daysBT3_5 = dt1.Rows[0]["3-5 days BT"].ToString();
                    model._dashboardsAging.above5daysBT = dt1.Rows[0]["above 5 days BT"].ToString();
                    model.Pending1 = Convert.ToInt32(dt.Rows[0]["Pending"].ToString());                    
                    model.Closed1 = Convert.ToInt32(dt.Rows[0]["Closed"].ToString());
                    model.InsuffPending1 = Convert.ToInt32(dt.Rows[0]["InsuffPending"].ToString()); 
                        model.FundsPending1 = Convert.ToInt32(dt.Rows[0]["FundsPending"].ToString());
                    model.totRejectionWIP1 = Convert.ToInt32(dt.Rows[0]["Rejected"].ToString());

                    model.Review1 = Convert.ToInt32(dt.Rows[0]["Review"].ToString());
                    model.ReviewResolved1 = Convert.ToInt32(dt.Rows[0]["ReviewResolved"].ToString());

                    return model;                   
                }
                if (Designation == "Team Member-Emp-QC")
                {
                     DataTable dt = new DataTable();
                    dt = cc.getdashboardOperationTLSummary("Employment", TeamMemberRowId, "OPQC");
                    DashboardTeamMemberQCViewModel model = new DashboardTeamMemberQCViewModel();
                    DataTable dt1 = new DataTable();
                    dt1 = cc.getdashboardAging(12,24);
                    model._dashboardsAging.todayPending = dt1.Rows[0]["todayPending"].ToString();
                    model._dashboardsAging.NextTwoDays = dt1.Rows[0]["NextTwoDays"].ToString();
                    model._dashboardsAging.WithinTAT = dt1.Rows[0]["Within TAT"].ToString();
                    model._dashboardsAging.daysBT1_2 = dt1.Rows[0]["1-2 days BT"].ToString();
                    model._dashboardsAging.daysBT3_5 = dt1.Rows[0]["3-5 days BT"].ToString();
                    model._dashboardsAging.above5daysBT = dt1.Rows[0]["above 5 days BT"].ToString();
                                        model.Pending1 = Convert.ToInt32(dt.Rows[0]["Pending"].ToString());
                    model.FundsPending1 = Convert.ToInt32(dt.Rows[0]["FundsPending"].ToString());
                    model.InsuffPending1 = Convert.ToInt32(dt.Rows[0]["InsuffPending"].ToString());
                    model.Closed1 = Convert.ToInt32(dt.Rows[0]["Closed"].ToString());
                    model.totRejectionWIP1 = Convert.ToInt32(dt.Rows[0]["Rejected"].ToString());
                    model.Review1 = Convert.ToInt32(dt.Rows[0]["Review"].ToString());
                    model.ReviewResolved1 = Convert.ToInt32(dt.Rows[0]["ReviewResolved"].ToString());
                    DataTable dtAuto = new DataTable();
                    dtAuto = cc.getAuto_RevertSummary();                    
                    model.AutoEmailer = Convert.ToInt32(dtAuto.Rows[0]["RevertRec"].ToString());
                    model.SubNotMatch = Convert.ToInt32(dtAuto.Rows[0]["SubNotMatch"].ToString());
                    model.wipAll = Convert.ToInt32(dtAuto.Rows[0]["WIPAll"].ToString());
                    model.Discarted = Convert.ToInt32(dtAuto.Rows[0]["discarted"].ToString());

                    return model;                    
                }
                if (Designation == "Team Member-Edu-QC")
                {

                     DataTable dt = new DataTable();
                    dt = cc.getdashboardOperationTLSummary("Education", TeamMemberRowId, "OPQC");
                    DashboardTeamMemberQCViewModel model = new DashboardTeamMemberQCViewModel();
                    DataTable dt1 = new DataTable();
                    dt1 = cc.getdashboardAging(11,24);
                    model._dashboardsAging.todayPending = dt1.Rows[0]["todayPending"].ToString();
                    model._dashboardsAging.NextTwoDays = dt1.Rows[0]["NextTwoDays"].ToString();
                    model._dashboardsAging.WithinTAT = dt1.Rows[0]["Within TAT"].ToString();
                    model._dashboardsAging.daysBT1_2 = dt1.Rows[0]["1-2 days BT"].ToString();
                    model._dashboardsAging.daysBT3_5 = dt1.Rows[0]["3-5 days BT"].ToString();
                    model._dashboardsAging.above5daysBT = dt1.Rows[0]["above 5 days BT"].ToString();
                                        model.Pending1 = Convert.ToInt32(dt.Rows[0]["Pending"].ToString());
                    model.FundsPending1 = Convert.ToInt32(dt.Rows[0]["FundsPending"].ToString());
                    model.InsuffPending1 = Convert.ToInt32(dt.Rows[0]["InsuffPending"].ToString());
                    model.Closed1 = Convert.ToInt32(dt.Rows[0]["Closed"].ToString());
                    model.totRejectionWIP1 = Convert.ToInt32(dt.Rows[0]["Rejected"].ToString());
                    model.Review1 = Convert.ToInt32(dt.Rows[0]["Review"].ToString()); 
                    model.ReviewResolved1 = Convert.ToInt32(dt.Rows[0]["ReviewResolved"].ToString());
                    model.EducationReviewResolved = Convert.ToInt32(dt.Rows[0]["EducationReviewResolved"].ToString());
                    DataTable dtAuto = new DataTable();
                    dtAuto = cc.getAuto_Education_RevertSummary();
                    model.AutoEmailer = Convert.ToInt32(dtAuto.Rows[0]["RevertRec"].ToString());
                    model.SubNotMatch = Convert.ToInt32(dtAuto.Rows[0]["SubNotMatch"].ToString());
                    model.wipAll = Convert.ToInt32(dtAuto.Rows[0]["WIPAll"].ToString());
                    model.Discarted = Convert.ToInt32(dtAuto.Rows[0]["discarted"].ToString());
                    model.ResultAwaitedRaised = Convert.ToInt32(dt.Rows[0]["ResultAwaitedRaised"].ToString());
                    model.ResultAwaitedResloved = Convert.ToInt32(dt.Rows[0]["ResultAwaitedResloved"].ToString());

                    return model;                   
                }
                if (Designation == "Team Member-Cri-QC")
                {
                    DataTable dt = new DataTable();
                    dt = cc.getdashboardOperationTLSummary("Criminal", TeamMemberRowId, "OPQC");
                    DashboardTeamMemberQCViewModel model = new DashboardTeamMemberQCViewModel();
                    DataTable dt1 = new DataTable();
                    dt1 = cc.getdashboardAging(5,24);
                    model._dashboardsAging.todayPending = dt1.Rows[0]["todayPending"].ToString();
                    model._dashboardsAging.NextTwoDays = dt1.Rows[0]["NextTwoDays"].ToString();
                    model._dashboardsAging.WithinTAT = dt1.Rows[0]["Within TAT"].ToString();
                    model._dashboardsAging.daysBT1_2 = dt1.Rows[0]["1-2 days BT"].ToString();
                    model._dashboardsAging.daysBT3_5 = dt1.Rows[0]["3-5 days BT"].ToString();
                    model._dashboardsAging.above5daysBT = dt1.Rows[0]["above 5 days BT"].ToString();
                                       model.Pending1 = Convert.ToInt32(dt.Rows[0]["Pending"].ToString());
                    model.FundsPending1 = Convert.ToInt32(dt.Rows[0]["FundsPending"].ToString());
                    model.InsuffPending1 = Convert.ToInt32(dt.Rows[0]["InsuffPending"].ToString());
                    model.Closed1 = Convert.ToInt32(dt.Rows[0]["Closed"].ToString());
                    model.totRejectionWIP1 = Convert.ToInt32(dt.Rows[0]["Rejected"].ToString());
                    model.Review1 = Convert.ToInt32(dt.Rows[0]["Review"].ToString());
                    model.ReviewResolved1 = Convert.ToInt32(dt.Rows[0]["ReviewResolved"].ToString());

                    return model;                  
                }
                if (Designation == "Team Member-Ref-QC")
                {
                     DataTable dt = new DataTable();
                    dt = cc.getdashboardOperationTLSummary("Reference", TeamMemberRowId, "OPQC");
                    DashboardTeamMemberQCViewModel model = new DashboardTeamMemberQCViewModel();
                    DataTable dt1 = new DataTable();
                    dt1 = cc.getdashboardAging(30,24);
                    model._dashboardsAging.todayPending = dt1.Rows[0]["todayPending"].ToString();
                    model._dashboardsAging.NextTwoDays = dt1.Rows[0]["NextTwoDays"].ToString();
                    model._dashboardsAging.WithinTAT = dt1.Rows[0]["Within TAT"].ToString();
                    model._dashboardsAging.daysBT1_2 = dt1.Rows[0]["1-2 days BT"].ToString();
                    model._dashboardsAging.daysBT3_5 = dt1.Rows[0]["3-5 days BT"].ToString();
                    model._dashboardsAging.above5daysBT = dt1.Rows[0]["above 5 days BT"].ToString();
                   
                    model.Pending1 = Convert.ToInt32(dt.Rows[0]["Pending"].ToString());
                    model.FundsPending1 = Convert.ToInt32(dt.Rows[0]["FundsPending"].ToString());
                    model.InsuffPending1 = Convert.ToInt32(dt.Rows[0]["InsuffPending"].ToString());
                    model.Closed1 = Convert.ToInt32(dt.Rows[0]["Closed"].ToString());
                    model.totRejectionWIP1 = Convert.ToInt32(dt.Rows[0]["Rejected"].ToString());
                    model.Review1 = Convert.ToInt32(dt.Rows[0]["Review"].ToString());
                    model.ReviewResolved1 = Convert.ToInt32(dt.Rows[0]["ReviewResolved"].ToString());

                    DataTable dtAuto = new DataTable();
                    dtAuto = cc.getRefenceAuto_RevertSummary();
                    model.AutoEmailer = Convert.ToInt32(dtAuto.Rows[0]["RevertRec"].ToString());
                    model.SubNotMatch = Convert.ToInt32(dtAuto.Rows[0]["SubNotMatch"].ToString());
                    model.wipAll = Convert.ToInt32(dtAuto.Rows[0]["WIPAll"].ToString());
                    model.Discarted = Convert.ToInt32(dtAuto.Rows[0]["discarted"].ToString());




                    return model;                   
                }
                if (Designation == "Team Member-Ide-QC")
                {
                    DataTable dt = new DataTable();
                    dt = cc.getdashboardOperationTLSummary("National", TeamMemberRowId, "OPQC");
                    DashboardTeamMemberQCViewModel model = new DashboardTeamMemberQCViewModel();
                    DataTable dt1 = new DataTable();
                    dt1 = cc.getdashboardAging(26,24);
                    model._dashboardsAging.todayPending = dt1.Rows[0]["todayPending"].ToString();
                    model._dashboardsAging.NextTwoDays = dt1.Rows[0]["NextTwoDays"].ToString();
                    model._dashboardsAging.WithinTAT = dt1.Rows[0]["Within TAT"].ToString();
                    model._dashboardsAging.daysBT1_2 = dt1.Rows[0]["1-2 days BT"].ToString();
                    model._dashboardsAging.daysBT3_5 = dt1.Rows[0]["3-5 days BT"].ToString();
                    model._dashboardsAging.above5daysBT = dt1.Rows[0]["above 5 days BT"].ToString();
                                        model.Pending1 = Convert.ToInt32(dt.Rows[0]["Pending"].ToString());
                    model.FundsPending1 = Convert.ToInt32(dt.Rows[0]["FundsPending"].ToString());
                    model.InsuffPending1 = Convert.ToInt32(dt.Rows[0]["InsuffPending"].ToString());
                    model.Closed1 = Convert.ToInt32(dt.Rows[0]["Closed"].ToString());
                    model.totRejectionWIP1 = Convert.ToInt32(dt.Rows[0]["Rejected"].ToString());
                    model.Review1 = Convert.ToInt32(dt.Rows[0]["Review"].ToString());
                    model.ReviewResolved1 = Convert.ToInt32(dt.Rows[0]["ReviewResolved"].ToString());
                    model.APIChecks = Convert.ToInt32(dt.Rows[0]["APIChecks"].ToString());

                    return model;                    
                }
                if (Designation == "Team Member-SPL-QC")
                {
                     DataTable dt = new DataTable();
                    dt = cc.getdashboardOperationTLSummary("Special", TeamMemberRowId, "OPQC");
                    DashboardTeamMemberQCViewModel model = new DashboardTeamMemberQCViewModel();
                    DataTable dt1 = new DataTable();
                    dt1 = cc.getdashboardAging(40,24);
                    model._dashboardsAging.todayPending = dt1.Rows[0]["todayPending"].ToString();
                    model._dashboardsAging.NextTwoDays = dt1.Rows[0]["NextTwoDays"].ToString();
                    model._dashboardsAging.WithinTAT = dt1.Rows[0]["Within TAT"].ToString();
                    model._dashboardsAging.daysBT1_2 = dt1.Rows[0]["1-2 days BT"].ToString();
                    model._dashboardsAging.daysBT3_5 = dt1.Rows[0]["3-5 days BT"].ToString();
                    model._dashboardsAging.above5daysBT = dt1.Rows[0]["above 5 days BT"].ToString();
                                       model.Pending1 = Convert.ToInt32(dt.Rows[0]["Pending"].ToString());
                    model.FundsPending1 = Convert.ToInt32(dt.Rows[0]["FundsPending"].ToString());
                    model.InsuffPending1 = Convert.ToInt32(dt.Rows[0]["InsuffPending"].ToString());
                    model.Closed1 = Convert.ToInt32(dt.Rows[0]["Closed"].ToString());
                    model.totRejectionWIP1 = Convert.ToInt32(dt.Rows[0]["Rejected"].ToString());
                    model.Review1 = Convert.ToInt32(dt.Rows[0]["Review"].ToString());
                    model.ReviewResolved1 = Convert.ToInt32(dt.Rows[0]["ReviewResolved"].ToString());
                    model.APIChecks = Convert.ToInt32(dt.Rows[0]["APIChecks"].ToString());
                    return model;                   
                }
                if (Designation == "Team Member-PV-QC")
                {
                    DataTable dt = new DataTable();
                    dt = cc.getdashboardOperationTLSummary("Physical Verification", TeamMemberRowId, "PVQC");
                    DashboardTeamMemberQCViewModel model = new DashboardTeamMemberQCViewModel();                   
                 
                    model.Pending1 = Convert.ToInt32(dt.Rows[0]["WIP"].ToString());
                    model.Closed1 = Convert.ToInt32(dt.Rows[0]["Closed"].ToString());
                    model.totQCRejectionBT = Convert.ToInt32(dt.Rows[0]["RejectedtoPVQC"].ToString());
                    return model;
                }

                return null;

            }
            catch (Exception)
            {
                throw;
            }
        }




        public Insuff_FundsAllRemarks getAllProcess_Remark(string  processname, int personalRowID, int subCheckRowID)
        {
            try
            {
                Insuff_FundsAllRemarks model = new Insuff_FundsAllRemarks();
                    DataTable dt = new DataTable();
                    dt = cc.getAllRemark(processname, personalRowID, subCheckRowID);
                                        if(dt.Rows.Count>0)
                {
                    model.Remarks = (dt.Rows[0]["Remarks"].ToString());
                    model.RemarksID = Convert.ToInt32(dt.Rows[0]["RemarksID"].ToString());
                    model.CreatedBy = dt.Rows[0]["CreatedBy"].ToString();
                    model.CreatedByID = Convert.ToInt32(dt.Rows[0]["CreatedByID"].ToString());

                    model.PersonalRowID = Convert.ToInt32(dt.Rows[0]["PersonalRowID"].ToString());
                    model.SubCheckRowID = Convert.ToInt16(dt.Rows[0]["SubCheckRowID"].ToString());
                    model.CheckstatusId = Convert.ToInt32(dt.Rows[0]["CheckStatusID"].ToString());
                }                     
                   
                    return model;     
                

                
            }
            catch (Exception)
            {
                throw;
            }
        }


        public ReminderMail get_AutoFindReminderEmailer(int personalRowID, int subCheckRowID, string Rem_Status)
        {
            try
            {

                ReminderMail model = new ReminderMail();
                DataTable dt = new DataTable();
                dt = cc.get_AutoFindEmailer( personalRowID, subCheckRowID,Rem_Status);                
                if (dt.Rows.Count > 0)
                {
                    model.PersonalRowID = Convert.ToInt32(dt.Rows[0]["PersonalRowID"].ToString());
                    model.SubCheckRowID = Convert.ToInt32(dt.Rows[0]["SubCheckRowID"].ToString());
                    model.Tomail = dt.Rows[0]["ToEmail"].ToString();
                    model.subject = dt.Rows[0]["subject"].ToString();
                    model.Body = dt.Rows[0]["Body"].ToString();
                    model.Status = Convert.ToInt32(dt.Rows[0]["Status"].ToString());
                    model.Creation_Date= Convert.ToDateTime(dt.Rows[0]["CreationDate"].ToString());
                    model.CreatedBy=dt.Rows[0]["CreatedBy"].ToString();


                }

                return model;

            }
            catch (Exception)
            {
                throw;
            }
        }


               
        public ClientServiceListViewModel GetFakeUniversityList(int pageNo, int pageSize)
        {


            try
            {
                if (pageNo < 0)
                {
                    pageNo = 1;
                }
                ClientServiceListViewModel model = new ClientServiceListViewModel();
                // ClientDashboardCheckSummaryDetailViewModel model = new ClientDashboardCheckSummaryDetailViewModel();
                DataTable dt = new DataTable();
                ClientCustomize cc = new ClientCustomize();
                dt = cc.GetFakeUniversity();
                List<FakeUniversity> lmd = new List<FakeUniversity>();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt.Rows) // loop for adding add from dataset to list<modeldata>
                    {
                        lmd.Add(new FakeUniversity
                        {
                            SrNo = Convert.ToInt32(dr["SrNo"].ToString()),
                            Name = (dr["Name"].ToString()),
                            Location = dr["Location"].ToString(),
                            CurrentStatus = (dr["CurrentStatus"].ToString()),


                        });

                    }
                    model.TotalRecords = dt.Rows.Count;
                }
                else
                {
                    model.TotalRecords = 0;
                }
                model.PageSize = pageSize;
                model.FakeUniversity = lmd.Skip((pageNo - 1) * pageSize).Take(pageSize);
                return model;

            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
