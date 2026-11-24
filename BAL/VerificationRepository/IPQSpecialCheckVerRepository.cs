using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.ProvidedInfoViewModel;
using ViewModels.VerificationViewModel;

namespace BAL.VerificationRepository
{
    public interface ISpecialVerificationRepository
    {
        CheckCommonInfoForAllocationViewModel GetCheckCommonInfoForAllocation(int SpecialCheckRowId = 0);
        void ManagerAllocateCheckToTeamLeader(ManagerAllocateCheckToTeamLeaderViewModel model);
        void TeamLeaderAllocateCheckToTeamMember(TeamLeaderAllocateCheckToTeamMemberViewModel model);
        PQSpecialCheckVerViewModel GetSpecialCheckInfoForDataEntry(int SpecialChecksRowId = 0);
        PQPersonalDetailsVerViewModel GetPersonalDetailsForTM(int SpecialCheckRowId = 0);
        PQSpecialCheckVerViewModel GetSpecialCheckForTM(int SpecialCheckRowId = 0);
        void UpdateSpecialCheckInfoByDataEntry(PQSpecialCheckVerViewModel model);
        void AddSpacelCheckInfoByDataEntryn(PQSpecialCheckVerViewModel model);
        UpdatePQSpecialCheckVerViewModel GetSpecialCheckInfoForTMQC(int SpecialCheckRowId = 0,int CheckStatusId=0);
        void ViewSpecialCheckInfoByVrifierTL(ViewSpecialCheckViewModel model);
        void UpdateSpecialCheckByTMQC(UpdatePQSpecialCheckVerViewModel model, short RptQCManagerId = 0);
        UpdateSpecialCheckInsuffClearViewModel GetPQSpecialCheckDetailsById(int SpecialCheckRowID = 0);
        void UpdateSpecialCheckInfoByTeamMember(PQUpdateSpecialCheckTeamMemberViewModel model);
        void UpdateSpecialCheckInfoByTeamLeaderReAllocated(PQUpdateSpecialCheckTeamLeaderViewModel model);
    }
}
