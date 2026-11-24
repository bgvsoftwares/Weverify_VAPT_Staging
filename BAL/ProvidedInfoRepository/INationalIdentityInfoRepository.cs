using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.ProvidedInfoViewModel;
using ViewModels.TempViewModel;
using ViewModels.VerificationViewModel;

namespace BAL.ProvidedInfoRepository
{
    public interface INationalIdentityInfoRepository
    {
        void AddNationalIdentityInformation(AddPQNationalIdentityViewModel model);
        int GetAllNationalIdentityByClientId(short ClientRowID = 0);
        AddPQNationalIdentityViewModel GetNationalIdentityInfoForDataEntry(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0);
        void UpdateNationalIdentityInfoByDataEntry(AddPQNationalIdentityViewModel model);
        UpdatePQNationalIdentityViewModel GetNationalIdentityInfoForQC(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0);
        UpdatePQNationalIdentityViewModel GetNationalIdentityInfoForQC(int NationalIdentityRowID = 0);
        void UpdateNationalIdentityInfoByQC(UpdatePQNationalIdentityViewModel model);
        void UpdateNationalIdentityInfoByManager(PQUpdateNationalIdentityManagerViewModel model);
        void UpdateNationalIdentityInfoByTeamLeader(PQUpdateNationalIdentityTeamLeaderViewModel model);
        void UpdateNationalIdentityInfoByTeamMember(PQUpdateNationalIdentityTeamMemberViewModel model);
        void RejectionUpdateNationalIInfoByQC(UpdateNationalIRejectionViewModel model);
        void UpdateNationalIdentityInfoByTeamLeaderReAllocated(PQUpdateNationalIdentityTeamLeaderViewModel model);
        bool IfExistNationalIdentityInfo(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0, string CheckStatus = "");
        bool IfExistNationalIdentityForDELeftMenu(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0);
        bool IfExistNationalIdentityForQCLeftMenu(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0, int CheckStatusid = 0);
        int SaveChanges();

        #region ******Temp NationalIdentity ********

        void AddTempNationalIdentityInformation(AddTempPQNationalIdentityViewModel model);
        int GetAllTempNationalIdentityByClientId(short ClientRowID = 0);
        AddTempPQNationalIdentityViewModel GetTempNationalIdentityInfoForBySelf(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0);
        void UpdateTempNationalIdentityInfoBySelf(AddTempPQNationalIdentityViewModel model);

        #endregion

        PQNationalIdentityInsuffListPagedModel GetAllNationalIdentityInsuffListForDETL(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", int PersonalId = 0, string spocEmail = "", short TeamMemberRowid = 0, int ClientUserRowID = 0);
        PQNationalIdentityInsuffListPagedModel GetAllNationalIdentityInsuffListForDETL1(int pageNo, int pageSize, string sort, string sortDir, string subcheckType, string Search = "", short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", int PersonalId = 0, string spocEmail = "", short TeamMemberRowid = 0, int ClientUserRowID = 0,int allclints=0);
        PQNationalIdentityInsuffListPagedModel GetAllNationalIdentityInsuffListForCSSpoc(int pageNo, int pageSize, string sort, string sortDir, string subcheckType, string Search = "", short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", int PersonalId = 0, string spocEmail = "", short TeamMemberRowid = 0, int ClientUserRowID = 0);
        UpdatePQNationalIdentityInsuffClearViewModel GetPQNationalIdentityDetailsById(int NationalIdentityRowID = 0);
        void UpdatePQNationalIdentityInsuffClear(UpdatePQNationalIdentityInsuffClearViewModel model);

        void ViewNationalIdentityInfoDETL(ViewNationalIdentityViewModel model);
        void UpdateNationalIdentityRejectionByDEQC(ViewNationalIdentityViewModel model, bool flag = false);
        void ViewNationalIdentityInfoByVrifierTL(ViewNationalIdentityViewModel model);
    }
}
