using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.VerificationViewModel;

namespace BAL.VerificationRepository
{
    public interface IDashboardPVRepository
    {
        DashboardEmpEduCriPVManagerViewModel GetPVManagerDashBoardInfo(short TeamMemberRowId, string Designation);
        DashboardEmpEduCriPVTLViewModel GetPVTeamLeaderDashBoardInfo(short TeamMemberRowId, string Designation);
        DashboardEmpEduCriPVTMViewModel GetPVTeamMemberDashBoardInfo(short TeamMemberRowId, string Designation);
        DashboardEmpEduCriPVTMQCViewModel GetPVTeamMemberQCDashBoardInfo(short TeamMemberRowId, string Designation);
    }
}
