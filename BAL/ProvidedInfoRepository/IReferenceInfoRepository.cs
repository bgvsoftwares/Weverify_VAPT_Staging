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
    public interface IReferenceInfoRepository
    {
        void AddReferenceInformation(PQReferenceViewModel model);
        int GetAllReferenceByClientId(short ClientRowID = 0);

        PQReferenceViewModel GetReferenceInfoForDataEntry(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0);
        void UpdateReferenceInfoByDataEntry(PQReferenceViewModel model);

        PQUpdateReferenceViewModel GetReferenceInfoForQualityCheck(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0);
        PQUpdateReferenceViewModel GetReferenceInfoForQualityCheck(int ReferenceRowID = 0);

        void UpdateReferenceInfoByQC(PQUpdateReferenceViewModel model);
        void UpdateReferenceInfoByManager(PQUpdateReferenceManagerViewModel model);
        void UpdateReferenceInfoByTeamLeader(PQUpdateReferenceTeamLeaderViewModel model);
        void UpdateReferenceInfoByTeamMember(PQUpdateReferenceTeamMemberViewModel model);
        void RejectionUpdateRefrenceInfoByQC(UpdateRefrnceRejectionViewModel model);
        void UpdateReferenceInfoByTeamLeaderReAllocated(PQUpdateReferenceTeamLeaderViewModel model);

        bool IfExistReference(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0, string CheckStatus = "");
        bool IfExistReferenceForDELeftMenu(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0);
        bool IfExistReferenceForQCLeftMenu(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0, int CheckStatusid = 0);

        int SaveChanges();

        #region ****Temp Reference************

        TempPQReferenceViewModel GetTempReferenceInfoForBySelf(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0);
        int GetAllTempReferenceByClientId(short ClientRowID = 0);
        void UpdateTempReferenceInfoBySelf(TempPQReferenceViewModel model);
        void AddTempReferenceInformation(TempPQReferenceViewModel model);

        #endregion

        PQReferenceInsuffListPagedModel GetAllReferenceInsuffListForDETL(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", int PersonalId = 0, string spocEmail = "", short TeamMemberRowid = 0, int ClientUserRowID = 0);
        PQReferenceInsuffListPagedModel GetAllReferenceInsuffListForDETL1(int pageNo, int pageSize, string sort, string sortDir, string subcheckType, string Search = "", short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", int PersonalId = 0, string spocEmail = "", short TeamMemberRowid = 0, int ClientUserRowID = 0,int allclints=0);
        PQReferenceInsuffListPagedModel GetAllReferenceInsuffListForCSSpoc(int pageNo, int pageSize, string sort, string sortDir, string subcheckType, string Search = "", short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", int PersonalId = 0, string spocEmail = "", short TeamMemberRowid = 0, int ClientUserRowID = 0);

        UpdatePQReferenceInsuffClearViewModel GetPQReferenceDetailsById(int ReferenceRowID = 0);
        void UpdatePQReferenceInsuffClear(UpdatePQReferenceInsuffClearViewModel model);
        void ViewReferenceInfoDETL(PQUpdateReferenceViewModel model);
        void UpdateReferenceRejectionByDEQC(PQUpdateReferenceViewModel model,bool flag=false);
        void ViewReferenceInfoByVrifierTL(ViewReferenceViewModel model);
    }
}
