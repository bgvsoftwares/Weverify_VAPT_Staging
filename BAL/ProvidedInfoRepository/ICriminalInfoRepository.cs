using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.ProvidedInfoViewModel;
using ViewModels.TempViewModel;

namespace BAL.ProvidedInfoRepository
{
    public interface ICriminalInfoRepository
    {
        void AddCriminalInformation(AddPQCriminalViewModel model);
     
        int GetAllCriminalByClientId(short ClientRowID = 0);
        AddPQCriminalViewModel GetCriminalInfoForDataEntry(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0);
        void UpdateCriminalInforByDataEntry(AddPQCriminalViewModel model);
        int SaveChanges();
        PQUpdateCriminalViewModel GetCriminalInfoForQualityCheck(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0);
        PQUpdateCriminalViewModel GetCriminalInfoForQualityCheck(int CriminalRowID = 0);
        void UpdateCriminalInfoByQC(PQUpdateCriminalViewModel model);
        void UpdateCriminalInfoByManager(PQUpdateCriminalManagerViewModel model);
        void UpdateCriminalInfoByTeamLeader(PQUpdateCriminalTeamLeaderViewModel model);
        void UpdateCriminalInfoByTeamMember(PQUpdateCriminalTeamMemberViewModel model);
        void ViewCriminalInfoByVrifierTL(ViewCriminalViewModel model);
        void RejectionUpdateCriminalInfoByQC(UpdateCriminalRejectionViewModel model);
        void UpdateCriminalInfoByTeamLeaderReAllocated(PQUpdateCriminalTeamLeaderViewModel model);

        bool IfExistCriminal(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0, string CheckStatus = "");
        bool IfExistCriminalForDELeftMenu(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0);
        bool IfExistCriminalForQCLeftMenu(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0, int CheckStatusid = 0);

        #region ****** Temp Client *******

        void AddTempCriminalInformation(AddTempPQCriminalViewModel model);
        int GetAllTempCriminalByClientId(short ClientRowID = 0);
        void UpdateTempCriminalInforBySelf(AddTempPQCriminalViewModel model);
        AddTempPQCriminalViewModel GetCriminalInfoBySelf(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0);

        #endregion

        PQCriminalInsuffListPagedModel GetAllCriminalInsuffListForDETL(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", int PersonalId = 0, string spocEmail = "", short TeamMemberRowid = 0, int ClientUserRowID = 0);
        PQCriminalInsuffListPagedModel GetAllCriminalInsuffListForDETL1(int pageNo, int pageSize, string sort, string sortDir, string subcheckType, string Search = "", short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", int PersonalId = 0, string spocEmail = "", short TeamMemberRowid = 0, int ClientUserRowID = 0,int allclints=0);
        PQCriminalInsuffListPagedModel GetAllCriminalInsuffListForCSSpoc(int pageNo, int pageSize, string sort, string sortDir, string subcheckType, string Search = "", short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", int PersonalId = 0, string spocEmail = "", short TeamMemberRowid = 0, int ClientUserRowID = 0);
        UpdatePQCriminalInsuffClearViewModel GetPQCriminalDetailsById(int CriminalRowID = 0);
        void UpdatePQCriminalInsuffClear(UpdatePQCriminalInsuffClearViewModel model);

        void ViewCriminaInfoDETL(ViewCriminalViewModel model);
        void UpdateCriminalRejectionByDEQC(ViewCriminalViewModel model, bool flag = false);
    }
}
