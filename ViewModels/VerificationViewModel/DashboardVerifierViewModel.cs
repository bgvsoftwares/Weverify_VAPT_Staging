using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.VerificationViewModel
{
    public class DashboardManagerViewModel
    {
        public int totWIP { get; set; }

        public int ResultAwaitedRaised { get; set; }
        public int totAllocated { get; set; }
        public int totNonAllocated { get; set; }
        public int totInitiated { get; set; }
        public int totNonInitiated { get; set; }
        public int totRejectionWIP { get; set; }
        public int totReOpen { get; set; }
        public int totPriority { get; set; }
        public int totInsuffWIP { get; set; }

        public int totTodaysDue { get; set; }
        public int totDueinNext2days { get; set; }
        public int totInTAT4to7days { get; set; }
        public int totInTATGreater7days { get; set; }
        public int totBT1to2days { get; set; }
        public int totBT3to5days { get; set; }
        public int totBT5to10days { get; set; }
        public int totBT10daysPlus { get; set; }
        public int tot1checkPending { get; set; }

        public int totInsufficiency { get; set; }
        public int totPrecallingWIP { get; set; }
        public int totPostcallingWIP { get; set; }
        public int totQCPending { get; set; }
        public int totFundApproval { get; set; }
        public int totApprovalpending { get; set; }
        public int totEscalated { get; set; }
        public int totClosed { get; set; }
        public int totOutTAT { get; set; }

        public int totPVWIP { get; set; }
        public int totPVClosed { get; set; }

        public int Pending1 { get; set; }
        public int WIP1 { get; set; }
        public int YTR1 { get; set; }
        public int Closed1 { get; set; }

        public int Allocated1 { get; set; }
        public int UnAllocated1 { get; set; }
        public int PVUnallocated { get; set; }

        public int totApprovalpending1 { get; set; }

        
        public int Reopen1 { get; set; }
        public int Insufficency1 { get; set; }
        public int Review { get; set; }
        public int ResearchRaised { get; set; }
        public int InsufficentResolved { get; set; }
        public int FundsResolved { get; set; }
        public int Reverification { get; set; }
        public int WIPALL { get; set; }
        public int YTR { get; set; }

        public int PendingQC { get; set; }
        public int ReviewTL { get; set; }
        public int SpocReviewPending { get; set; }

        public int ReviewForApproval { get; set; }
        public int ReviewResolved { get; set; }

        public int AutoEmailer { get; set; }
        public int Reminder1 { get; set; }
        public int Reminder2 { get; set; }
        public int Reminder3 { get; set; }
        public int Reminder4 { get; set; }
        public int Reminder5 { get; set; }
        public int GreaterThan5 { get; set; }
        public int ErrorInMail { get; set; }
        public int RevNotRec { get; set; }
        public int initialMailSent { get; set; }
        public int wipAll { get; set; }
        public int Bounce { get; set; }
        public int WIPAllEdu { get; set; }

        public DashboardsAging  _dashboardsAging = new DashboardsAging();
    }

    public class DashboardTeamLeaderViewModel
    {
        public int totWIP { get; set; }

        public int PVUnallocated1 { get; set; }
        public int PVUnAllocated { get; set; }
        public int CVUnAllocated { get; set; }
        public int totAllocated { get; set; }
        public int totNonAllocated { get; set; }
        public int totInitiated { get; set; }
        public int totNonInitiated { get; set; }
        public int totRejectionWIP { get; set; }
        public int totReOpen { get; set; }
        public int totPriority { get; set; }
        public int totInsuffWIP { get; set; }

        public int totTodaysDue { get; set; }
        public int totDueinNext2days { get; set; }
        public int totInTAT4to7days { get; set; }
        public int totInTATGreater7days { get; set; }
        public int totBT1to2days { get; set; }
        public int totBT3to5days { get; set; }
        public int totBT5to10days { get; set; }
        public int totBT10daysPlus { get; set; }
        public int tot1checkPending { get; set; }

        public int totInsufficiency { get; set; }
        public int totPrecallingWIP { get; set; }
        public int totPostcallingWIP { get; set; }
        public int totQCPending { get; set; }
        public int totFundApproval { get; set; }
        public int totFundPending { get; set; } 
        public int totApprovalpending { get; set; }
        public int totEscalated { get; set; }
        public int totClosed { get; set; }
        public int totOutTAT { get; set; }

        public int totPVWIP { get; set; }
        public int totPVClosed { get; set; }

        public int Pending1 { get; set; }
        public int WIP1 { get; set; }
        public int YTR1 { get; set; }
        public int Closed1 { get; set; }

        public int Allocated1 { get; set; }
        public int UnAllocated1 { get; set; }
        public int Review1 { get; set; }
        public int InsufficentResolved { get; set; }
        public int FundsResolved { get; set; }
        public int AInitiated { get; set; }
        public int ARevertRec { get; set; }
        public int ARevertPending { get; set; }
        public int ARevertProcessed { get; set; }
        public int SubNotMatch { get; set; }
        public int Discarted { get; set; }
        public int ColorApprovalRequestToQC { get; set; }
        public int ColorApproved { get; set; }

        public int MInitiated { get; set; }
        public int MRevertRec { get; set; }
        public int MRevertPending { get; set; }
        public int MRevertProcessed { get; set; }
        public int Reverification { get; set; }
        public int PartnerClosed1 { get; set; }

        public int AutoEmailer { get; set; }
        public int Reminder1 { get; set; }
        public int Reminder2 { get; set; }
        public int Reminder3 { get; set; }
        public int Reminder4 { get; set; }
        public int Reminder5 { get; set; }
        public int GreaterThan5 { get; set; }
        public int ErrorInMail { get; set; }
        public int RevNotRec { get; set; }
        public int initialMailSent { get; set; }
        public int wipAll { get; set; }
        public int Bounce { get; set; }
        public int totL2InsuffApproved { get; set; }
        public int WIPAllEdu { get; set; }
        public DashboardsAging _dashboardsAging = new DashboardsAging();

    }

    public class DashboardTeamMemberViewModel
    {
        public int totWIP { get; set; }
        public int PVUnallocated { get; set; }
        public int totInitiated { get; set; }
        public int totNonInitiated { get; set; }
        public int totRejectionWIP { get; set; }
        public int totReOpen { get; set; }
        public int totPriority { get; set; }
        public int totInsuffWIP { get; set; }
        public int totPVWIP { get; set; }

        public int totTodaysDue { get; set; }
        public int totDueinNext2days { get; set; }
        public int totInTAT4to7days { get; set; }
        public int totInTATGreater7days { get; set; }
        //public int totBT1to2days { get; set; }
        public int totBT1to5days { get; set; }
        public int totBT5daysPlus { get; set; }
        public int tot1checkPending { get; set; }
        public int totPVClosed { get; set; }

        public int totQCRejectionBT { get; set; }
        public int totFundApproval { get; set; }
        public int totReviewRaised { get; set; }
        public int totReviewResolved { get; set; }
        public int totInsufficiencyRaised { get; set; }
        public int totClosed { get; set; }
        public int totOutTAT { get; set; }
        public int totOrange { get; set; }
        public int Pending1 { get; set; }
        public int PartnerClosed1 { get; set; }
        public int WIP1 { get; set; }
        public int Closed1 { get; set; }
        public int InsufficentRaised1 { get; set; }
        public int FundsPending1 { get; set; }
        public int Research1 { get; set; }
        public int InsufficencyResolved1 { get; set; }
        public int FundsResolved1 { get; set; }
        public int totRejectionWIP1 { get; set; }
        public int ReviewResolved1 { get; set; }
        public int ReviewRaised1 { get; set; }
        public int totPVClosed1 { get; set; }
        public int NotDurable { get; set; }
        public int AutoEmailer { get; set; }        
        public int Reminder1 { get; set; }
        public int Reminder2 { get; set; }
        public int Reminder3 { get; set; }
        public int Reminder4 { get; set; }
        public int Reminder5 { get; set; }
        public int GreaterThan5 { get; set; }
        public int ErrorInMail { get; set; }
        public int RevNotRec { get; set; }
        public int initialMailSent { get; set; }
        public int wipAll { get; set; }
        public int Discarted { get; set; }
        public int AutoRevertRejected { get; set; }
        public int Groupcreation { get; set; }
        public int G_initialMailSent { get; set; }
        public int G_Reminder1 { get; set; }
        public int G_Reminder2 { get; set; }
        public int G_Reminder3 { get; set; }
        public int G_Reminder4 { get; set; }
        public int G_Reminder5 { get; set; }
        public int G_GreaterThan5 { get; set; }
        public int Bounce { get; set; }
        public int FieldPartnerClosed { get; set; }
        public int PartnerRejected { get; set; }
        public int Reverification { get; set; }
        public int WIPAllEdu { get; set; }
        public int APIChecks { get; set; }
        public int SpocReviewResolved { get; set; }
        public int TotTMReview { get; set; }
        public int DBCheckAPIFailed { get; set; }
        public int RevertRec { get; set; }
        public DashboardsAging _dashboardsAging = new DashboardsAging();

    }

    public class DashboardTeamMemberQCViewModel
    {
        public int totWIP { get; set; }

        public int ResultAwaitedRaised { get; set; }
        public int ResultAwaitedResloved { get; set; }
        public int totNew { get; set; }
        public int totRejectionWIP { get; set; }
        public int totReOpen { get; set; }
        public int totPriority { get; set; }
        public int totInsuffWIP { get; set; }

        public int totTodaysDue { get; set; }
        public int totWIP2Hrs { get; set; }
        public int totWIP3to6Hrs { get; set; }
        public int totWIP6to12Hrs { get; set; }
        public int totWIP12to24Hrs { get; set; }
        public int totWIP24HrsPlus { get; set; }

        public int totFundApproval { get; set; }
        public int totQCRejectionBT { get; set; }
        public int totReviewResolved { get; set; }
        public int totApprovalPending { get; set; }
        public int totInsufficiencyRaised { get; set; }
        public int totQCApproval { get; set; }
        public int totOutTAT { get; set; }
        public int Pending1 { get; set; }
        public int Closed1 { get; set; }
        public int FundsPending1 { get; set; }
        public int InsuffPending1 { get; set; }

        public int InsufficentRaised1 { get; set; }       
        public int InsufficencyResolved1 { get; set; }
        public int totRejectionWIP1 { get; set; }
        public int Review1 { get; set; }
        public int ReviewResolved1 { get; set; }
        public int EducationReviewResolved { get; set; }
        public int AutoEmailer { get; set; }
        public int wipAll { get; set; }
        public int Discarted { get; set; }
        public int RevNotRec { get; set; }
        public int SubNotMatch { get; set; }
        public int APIChecks { get; set; }
        public DashboardsAging _dashboardsAging = new DashboardsAging();
    }

    #region ***** Physical Manager Dashboard View Model *****

    public class DashboardPVManagerViewModel
    {
        public DashboardPVManagerViewModel()
        {
            totWIP = 0;
            totAllocated = 0;
            totNonAllocated = 0;
            totInitiated = 0;
            totNonInitiated = 0;
            totRejectionWIP = 0;
            totReOpen = 0;
            totPriority = 0;
            totInsuffWIP = 0;
            totTodaysDue = 0;
            totDueinNext2days = 0;
            totInTAT4to7days = 0;
            totInTATGreater7days = 0;
            totBT1to2days = 0;
            totBT3to5days = 0;
            totBT5to10days = 0;
            totBT10daysPlus = 0;
            tot1checkPending = 0;
            totInsufficiency = 0;
            totPrecallingWIP = 0;
            totPostcallingWIP = 0;
            totQCPending = 0;
            totFundApproval = 0;
            totApprovalpending = 0;
            totEscalated = 0;
            totClosed = 0;
            totOutTAT = 0;
            totAllocated1 = 0;
            totNonAllocated1 = 0;
            totClosed1 = 0;

        }

        public int totWIP { get; set; }
        public int totAllocated { get; set; }
        public int totNonAllocated { get; set; }
        public int totInitiated { get; set; }
        public int totNonInitiated { get; set; }
        public int totRejectionWIP { get; set; }
        public int totReOpen { get; set; }
        public int totPriority { get; set; }
        public int totInsuffWIP { get; set; }

        public int totTodaysDue { get; set; }
        public int totDueinNext2days { get; set; }
        public int totInTAT4to7days { get; set; }
        public int totInTATGreater7days { get; set; }
        public int totBT1to2days { get; set; }
        public int totBT3to5days { get; set; }
        public int totBT5to10days { get; set; }
        public int totBT10daysPlus { get; set; }
        public int tot1checkPending { get; set; }

        public int totInsufficiency { get; set; }
        public int totPrecallingWIP { get; set; }
        public int totPostcallingWIP { get; set; }
        public int totQCPending { get; set; }
        public int totFundApproval { get; set; }
        public int totApprovalpending { get; set; }
        public int totEscalated { get; set; }
        public int totClosed { get; set; }
        public int totOutTAT { get; set; }
        public int totAllocated1 { get; set; }
        public int totNonAllocated1 { get; set; }
        public int totClosed1 { get; set; }
    }

    public class DashboardEmpEduCriPVManagerViewModel
    {
        public DashboardPVManagerViewModel ManagerDashboardEmp { get; set; }
        public DashboardPVManagerViewModel ManagerDashboardEdu { get; set; }
        public DashboardPVManagerViewModel ManagerDashboardCri { get; set; }
    }

    public class DashboardPVTeamLeaderViewModel
    {
        public DashboardPVTeamLeaderViewModel()
        {
            totWIP = 0;
            totAllocated = 0;
            totNonAllocated = 0;
            totInitiated = 0;
            totNonInitiated = 0;
            totRejectionWIP = 0;
            totReOpen = 0;
            totPriority = 0;
            totInsuffWIP = 0;
            totTodaysDue = 0;
            totDueinNext2days = 0;
            totInTAT4to7days = 0;
            totInTATGreater7days = 0;
            totBT1to2days = 0;
            totBT3to5days = 0;
            totBT5to10days = 0;
            totBT10daysPlus = 0;
            tot1checkPending = 0;
            totInsufficiency = 0;
            totPrecallingWIP = 0;
            totPostcallingWIP = 0;
            totQCPending = 0;
            totFundApproval = 0;
            totApprovalpending = 0;
            totEscalated = 0;
            totClosed = 0;
            totOutTAT = 0;
            totAllocated1 = 0;
            totNonAllocated1 = 0;
            totClosed1 = 0;
        }

        public int totWIP { get; set; }
        public int totAllocated { get; set; }
        public int totNonAllocated { get; set; }
        public int totInitiated { get; set; }
        public int totNonInitiated { get; set; }
        public int totRejectionWIP { get; set; }
        public int totReOpen { get; set; }
        public int totPriority { get; set; }
        public int totInsuffWIP { get; set; }

        public int totTodaysDue { get; set; }
        public int totDueinNext2days { get; set; }
        public int totInTAT4to7days { get; set; }
        public int totInTATGreater7days { get; set; }
        public int totBT1to2days { get; set; }
        public int totBT3to5days { get; set; }
        public int totBT5to10days { get; set; }
        public int totBT10daysPlus { get; set; }
        public int tot1checkPending { get; set; }

        public int totInsufficiency { get; set; }
        public int totPrecallingWIP { get; set; }
        public int totPostcallingWIP { get; set; }
        public int totQCPending { get; set; }
        public int totFundApproval { get; set; }
        public int totApprovalpending { get; set; }
        public int totEscalated { get; set; }
        public int totClosed { get; set; }
        public int totOutTAT { get; set; }
        public int totAllocated1 { get; set; }
        public int totNonAllocated1 { get; set; }
        public int totClosed1 { get; set; }
    }

    public class DashboardEmpEduCriPVTLViewModel
    {
        public DashboardPVTeamLeaderViewModel TeamLeaderDashboardEmp { get; set; }
        public DashboardPVTeamLeaderViewModel TeamLeaderDashboardEdu { get; set; }
        public DashboardPVTeamLeaderViewModel TeamLeaderDashboardCri { get; set; }
    }

    public class DashboardPVTeamMemberViewModel
    {
        public DashboardPVTeamMemberViewModel()
        {
            totWIP = 0;
            totInitiated = 0;
            totNonInitiated = 0;
            totRejectionWIP = 0;
            totReOpen = 0;
            totPriority = 0;
            totInsuffWIP = 0;
            totPVWIP = 0;
            totTodaysDue = 0;
            totDueinNext2days = 0;
            totInTAT4to7days = 0;
            totInTATGreater7days = 0;
            totBT1to2days = 0;
            totBT1to5days = 0;
            totBT5daysPlus = 0;
            tot1checkPending = 0;
            totPVClosed = 0;
            totQCRejectionBT = 0;
            totFundApproval = 0;
            totReviewRaised = 0;
            totReviewResolved = 0;
            totInsufficiencyRaised = 0;
            totClosed = 0;
            totOutTAT = 0;
            totOrange = 0;
            totWIP1 = 0;
            totRejectionWIP1 = 0;
            totClosed1 = 0;
        }
        public int totWIP { get; set; }
        public int totInitiated { get; set; }
        public int totNonInitiated { get; set; }
        public int totRejectionWIP { get; set; }
        public int totReOpen { get; set; }
        public int totPriority { get; set; }
        public int totInsuffWIP { get; set; }
        public int totPVWIP { get; set; }

        public int totTodaysDue { get; set; }
        public int totDueinNext2days { get; set; }
        public int totInTAT4to7days { get; set; }
        public int totInTATGreater7days { get; set; }
        public int totBT1to2days { get; set; }
        public int totBT1to5days { get; set; }
        public int totBT5daysPlus { get; set; }
        public int tot1checkPending { get; set; }
        public int totPVClosed { get; set; }

        public int totQCRejectionBT { get; set; }
        public int totFundApproval { get; set; }
        public int totReviewRaised { get; set; }
        public int totReviewResolved { get; set; }
        public int totInsufficiencyRaised { get; set; }
        public int totClosed { get; set; }
        public int totOutTAT { get; set; }
        public int totOrange { get; set; }
        public int totWIP1 { get; set; }
        public int totRejectionWIP1 { get; set; }
        public int totClosed1 { get; set; }
    }

    public class DashboardEmpEduCriPVTMViewModel
    {
        public DashboardPVTeamMemberViewModel TeamMemberDashboardEmp { get; set; }
        public DashboardPVTeamMemberViewModel TeamMemberDashboardEdu { get; set; }
        public DashboardPVTeamMemberViewModel TeamMemberDashboardCri { get; set; }
    }

    public class DashboardPVTeamMemberQCViewModel
    {
        public DashboardPVTeamMemberQCViewModel()
        {
            totWIP = 0;
            totNew = 0;
            totRejectionWIP = 0;
            totReOpen = 0;
            totPriority = 0;
            totInsuffWIP = 0;

            totTodaysDue = 0;
            totWIP2Hrs = 0;
            totWIP3to6Hrs = 0;
            totWIP6to12Hrs = 0;
            totWIP12to24Hrs = 0;
            totWIP24HrsPlus = 0;

            totFundApproval = 0;
            totQCRejectionBT = 0;
            totReviewResolved = 0;
            totApprovalPending = 0;
            totInsufficiencyRaised = 0;
            totQCApproval = 0;
            totOutTAT = 0;
            totNew1 = 0;
            totRejectionWIP1 = 0;


        }

        public int totWIP { get; set; }
        public int totNew { get; set; }
        public int totRejectionWIP { get; set; }
        public int totReOpen { get; set; }
        public int totPriority { get; set; }
        public int totInsuffWIP { get; set; }

        public int totTodaysDue { get; set; }
        public int totWIP2Hrs { get; set; }
        public int totWIP3to6Hrs { get; set; }
        public int totWIP6to12Hrs { get; set; }
        public int totWIP12to24Hrs { get; set; }
        public int totWIP24HrsPlus { get; set; }

        public int totFundApproval { get; set; }
        public int totQCRejectionBT { get; set; }
        public int totReviewResolved { get; set; }
        public int totApprovalPending { get; set; }
        public int totInsufficiencyRaised { get; set; }
        public int totQCApproval { get; set; }
        public int totOutTAT { get; set; }
        public int totNew1 { get; set; }
        public int totRejectionWIP1 { get; set; }
    }

    public class DashboardEmpEduCriPVTMQCViewModel
    {
        public DashboardPVTeamMemberQCViewModel TeamMemberQCDashboardEmp { get; set; }
        public DashboardPVTeamMemberQCViewModel TeamMemberQCDashboardEdu { get; set; }
        public DashboardPVTeamMemberQCViewModel TeamMemberQCDashboardCri { get; set; }
    }

    #endregion

    public class DashboardsAging
    {
        public string totalWIP { get; set; }
        public string todayPending { get; set; }
        public string NextTwoDays { get; set; }
        public string WithinTAT { get; set; }
        public string daysBT1_2 { get; set; }
        public string daysBT3_5 { get; set; }
        public string above5daysBT { get; set;}       

    }

}
