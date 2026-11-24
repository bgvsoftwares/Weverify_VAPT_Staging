using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.ProvidedInfoViewModel;

namespace BAL.ProvidedInfoRepository
{
    public interface IDashboardDataEntryRepository
    {
        DashboardDataEntryTLViewModel GetDEManagerDashBoardInfo(short TeamMemberRowId, string rolename);

        DashboardDataEntryTLViewModel GetDETeamLeaderDashBoardInfo(short TeamMemberRowId, string rolename);
        DashboardDETeamMemberDEScanViewModel GetDETeamMemberDEScanDashBoardInfo(short TeamMemberRowId, string rolename);
        DashboardDETeamMemberDEViewModel GetDETeamMemberDEDashBoardInfo(short TeamMemberRowId, string rolename);
        DashboardDETeamMemberDEQCViewModel GetDETeamMemberDEQCDashBoardInfo(short TeamMemberRowId, string rolename);

        DashboardDEListViewModel GetDEManagerDashBoardInfo(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string CaseStatus = "", short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", byte tatHours = 0);

        DashboardDEListViewModel GetDETeamLeaderDashBoardInfo(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string CaseStatus = "", short teamMemerRowId = 0, short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", byte tatHours = 0);

        PQPersonalInfoListPagedModel GetDEEmportExcel(int teamMemerRowId = 0, string Designation = "", string CaseStatus = "", short ClientRowID = 0, byte tatHours = 0);
        DashboardExportDEListViewModel GetDEYTREmportExcel(int teamMemerRowId = 0, string Designation = "", string CheckStatus = "", short ClientRowID = 0, byte tatHours = 0);
        PQPersonalInfoListPagedModel GetREAllocationByDETLmportExcel(int teamMemerRowId = 0, string Designation = "", string CheckStatus = "", short ClientRowID = 0, byte tatHours = 0);
        PQPersonalInfoListPagedModel GetDEScannermportExcel(int teamMemerRowId = 0, string Designation = "", string CaseStatus = "", short ClientRowID = 0, byte tatHours = 0);
        PQPersonalInfoListPagedModel GetDEQCEmportExcel(int teamMemerRowId = 0, string Designation = "", string CaseStatus = "", short ClientRowID = 0, byte tatHours = 0);
        DashboardDEListViewModel GetDETeamLeaderDashBoardInfo1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string CaseStatus = "", string rolename = "", short teamMemerRowId = 0, short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", byte tatHours = 0);
        DashboardDEListViewModel GetDEManagerDashBoardInfo1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string CaseStatus = "", string rolename = "", short teamMemerRowId = 0, short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", byte tatHours = 0);
    }
}
