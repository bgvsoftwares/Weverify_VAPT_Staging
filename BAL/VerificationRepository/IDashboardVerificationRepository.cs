using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.ClientViewModel;
using ViewModels.VerificationViewModel;

namespace BAL.VerificationRepository
{
    public interface IDashboardVerificationRepository
    {
        DashboardManagerViewModel GetManagerDashBoardInfo(short TeamMemberRowId, string Designation);
        DashboardTeamLeaderViewModel GetTeamLeaderDashBoardInfo(short TeamMemberRowId, string Designation, int department_id = 0);
        DashboardTeamMemberViewModel GetTeamMemberDashBoardInfo(short TeamMemberRowId, string Designation, int department_id = 0);
        DashboardTeamMemberQCViewModel GetTeamMemberQCDashBoardInfo(short TeamMemberRowId, string Designation, int department_id = 0);
        Insuff_FundsAllRemarks getAllProcess_Remark(string processname, int personalRowID, int subCheckRowID);
        ReminderMail get_AutoFindReminderEmailer(int personalRowID, int subCheckRowID, string Rem_Status);
        ClientServiceListViewModel GetFakeUniversityList(int page, int pageSize);
    }
}
