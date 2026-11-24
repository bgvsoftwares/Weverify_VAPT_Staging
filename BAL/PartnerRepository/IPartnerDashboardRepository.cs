using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.PartnerViewModels;

namespace BAL.PartnerRepository
{
    public interface IPartnerDashboardRepository
    {
        PtrDashboardViewModel GetPtrManagerDashBoardInfo(short PartnerRowId);
        DashboardPartnerCandidateListViewModel GetCandidateListForReportQCManager(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string CaseStatus = "", short PartnerRowId = 0, short ClientRowID = 0, string RecievingDate = "", string CompletedDate = "", byte tatHours = 0);
        DashboardPartnerCandidateListViewModel GetCandidateListForReportQCManager1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string statusid = "", string CaseStatus = "", short PartnerRowId = 0, short ClientRowID = 0, string RecievingDate = "", string CompletedDate = "", byte tatHours = 0);
    }
}
