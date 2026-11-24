using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.ReportQCViewModel;
using ViewModels;
namespace BAL.ReportQCRepository
{
    public interface IDashboardRWQCRepository
    {
        DashboardRWQCListViewModel GetCandidateListForReportQCTeamMembersinglecheck(string Type , int pageNo, int pageSize, string sort, string sortDir, string Search = "", string CaseStatus = "", string Statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string RecievingDate = "", string CompletedDate = "", byte tatHours = 0);
        //DashboardRWQCViewModel GetRQQCManagerDashBoardInfo(short TeamMemberRowId, string Designation);
        DashboardRWQCManagerViewModel GetRWQCManagerDashBoardInfo(short TeamMemberRowId, string Designation);

        //DashboardRWQCViewModel GetRQQCTeamLeaderDashBoardInfo(short TeamMemberRowId, string Designation);
        DashboardRWQCTeamLeaderViewModel GetRWQCTeamLeaderDashBoardInfo(short TeamMemberRowId, string Designation);
        DashboardRWQCTeamLeaderViewModel GetRWQCTeamLeader2DashBoardInfo(short TeamMemberRowId, string Designation);

        DashboardRWQCViewModel GetRQQCTeamMemberDashBoardInfo(short TeamMemberRowId, string Designation);

        DashboardRWQCListViewModel GetCandidateListForReportQCManager(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string CaseStatus = "", short teamMemerRowId = 0, short ClientRowID = 0, string RecievingDate = "", string CompletedDate = "", byte tatHours = 0);

        DashboardRWQCListViewModel GetCandidateListForReportQCTeamLeader(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string CaseStatus = "", short teamMemerRowId = 0, short ClientRowID = 0, string RecievingDate = "", string CompletedDate = "", byte tatHours = 0);

        DashboardRWQCListViewModel GetCandidateListForReportQCTeamMember(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string CaseStatus = "", short teamMemerRowId = 0, short ClientRowID = 0, string RecievingDate = "", string CompletedDate = "", byte tatHours = 0);

        DashboardRWQCListViewModel GetExportRWQCListByMgr(string Designation = "", short teamMemerRowId = 0, byte tatHours = 0, string CaseStatus = "");

        DashboardRWQCListViewModel GetExportRWQCListByTl(string Designation = "", short teamMemerRowId = 0, byte tatHours = 0, string CaseStatus = "");

        DashboardRWQCListViewModel GetExportRWQCListByTM(string Designation = "", short teamMemerRowId = 0, byte tatHours = 0, string CaseStatus = "");
        DashboardRWQCListViewModel GetCandidateListForReportQCManager1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string Statusid = "", string CaseStatus = "", short teamMemerRowId = 0, short ClientRowID = 0, string RecievingDate = "", string CompletedDate = "", byte tatHours = 0);        
        DashboardRWQCListViewModel GetCandidateListForReportQCManager_InterimAllocated(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string CaseStatus = "", string Statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string RecievingDate = "", string CompletedDate = "", byte tatHours = 0);
        DashboardRWQCListViewModel GetCandidateListForReportQCTeamLeader1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string Statusid = "", string CaseStatus = "", short teamMemerRowId = 0, short ClientRowID = 0, string RecievingDate = "", string CompletedDate = "", byte tatHours = 0);
        DashboardRWQCListViewModel GetCandidateListForReportQCTeamLeader_Interim(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string CaseStatus = "", string Statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string RecievingDate = "", string CompletedDate = "", byte tatHours = 0);
        DashboardRWQCListViewModel GetCandidateListForReportQCTeamLeader1TCS(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string CaseStatus = "", string Statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string RecievingDate = "", string CompletedDate = "", byte tatHours = 0);
        DashboardRWQCListViewModel GetCandidateListForReportQCTeamMember1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string CaseStatus = "", string Statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string RecievingDate = "", string CompletedDate = "", byte tatHours = 0 , bool fetchDataCheckbox=false, bool GeneralCase = false);
        DashboardRWQCListViewModel GetCandidateListForReportQCTeamMember2(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string CaseStatus = "", string Statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string RecievingDate = "", string CompletedDate = "", byte tatHours = 0 , bool fetchDataCheckbox=false, bool GeneralCase = false);
         DashboardRWQCListViewModel GetCandidateListForReportQCTeamMember_Interim(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string CaseStatus = "", string Statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string RecievingDate = "", string CompletedDate = "", byte tatHours = 0);
        DashboardRWQCListViewModel GetCandidateListForReportQCTeamMemberTCSDB(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string CaseStatus = "", string Statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string RecievingDate = "", string CompletedDate = "", byte tatHours = 0);
        DashboardRWQCListViewModel GetCandidateListForReportQCTeamMemberChola(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string CaseStatus = "", string Statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string RecievingDate = "", string CompletedDate = "", byte tatHours = 0);
        DashboardRWQCListViewModel GetCandidateListForReportQCTeamMemberWIPRO(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string CaseStatus = "", string Statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string RecievingDate = "", string CompletedDate = "", byte tatHours = 0);
        DashboardRWQCListViewModel GetCandidateListForReportQCTeamMemberDAIMLER(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string CaseStatus = "", string Statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string RecievingDate = "", string CompletedDate = "", byte tatHours = 0);
        DashboardRWQCListViewModel GetCandidateListForReportQCTeamMemberTitan(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string CaseStatus = "", string Statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string RecievingDate = "", string CompletedDate = "", byte tatHours = 0);
        DashboardRWQCListViewModel GetCandidateListForReportQCTeamMemberGlobal(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string CaseStatus = "", string Statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string RecievingDate = "", string CompletedDate = "", byte tatHours = 0);
        DashboardRWQCListViewModel GetCandidateListForReportQCTeamMemberTCSBPO(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string CaseStatus = "", string Statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string RecievingDate = "", string CompletedDate = "", byte tatHours = 0);

        DashboardRWQCListViewModel ManageInterimRequest(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string CaseStatus = "", string Statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string RecievingDate = "", string CompletedDate = "", byte tatHours = 0);
        //ReportVerificationScopeInfoListPagedModel GetWiproVerificationScope(int pageNo, int pageSize, int personalRowId = 0);
        List<ReportVerificationScope> GetWiproVerificationScopeReport(int personalRowId);
        DashboardRWQCListViewModel GetCandidateListForReportQCTeamMemberWiproLimited(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string CaseStatus = "", string Statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string RecievingDate = "", string CompletedDate = "", byte tatHours = 0);
        PageRaddarSearchListPagedModel GetCaseIdTMSearch(int ClientRowID, string UserID, int pageNo = 1, int pageSize = 1, int page = 1, string sort = "", string sortDir = "asc", string SearchType = "", string SearchValue = "");
        RWRejectionTrackerListPagedModel GetRejectionTracker(int ClientRowID, string UserID, int pageNo = 1, int pageSize = 1, int page = 1, string sort = "", string sortDir = "asc", string SearchValue = "");
        ReportWritingProductivityListPagedModel GetReportWritingProductivity(int ClientRowID, string UserID, int pageNo = 1, int pageSize = 1, int page = 1, string sort = "", string sortDir = "asc", string SearchValue = "");
        DashboardRWQCViewModel GetRQQCTeamMemberQCDashBoardInfo(short TeamMemberRowId, string Designation);
    }
}
