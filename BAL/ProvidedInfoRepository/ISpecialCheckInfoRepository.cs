using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.ProvidedInfoViewModel;
using ViewModels.TempViewModel;

namespace BAL.ProvidedInfoRepository
{
    public interface ISpecialCheckInfoRepository
    {
        int GetAllSpacialByClientId(short ClientRowID = 0);
        int GetSpecialCheckIDByUniqueComponentID(int PersonalRowID = 0, string UniqueComponentID = "");
        bool IfExistSpecialCheckForDELeftMenu(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0);
        bool IfExistSpecialCheckForDEQCLeftMenu(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0, int CheckStatusid = 0);

        AddSpecialCheckViewModel GetSpecialCheckInfoForDataEntry(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0);
        void AddSpacelCheckInformation(AddSpecialCheckViewModel model);
        void UpdateSpecialCheckInfoByDataEntry(AddSpecialCheckViewModel model);
        void RejectionUpdateSpecialCheckInfoByQC(UpdateSpecialCheckRejectionViewModel model);
        UpdateSpecialCheckViewModel GetSpecialCheckInfoForDEQC(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0);
        void UpdateSpecialCheckInfoByQualityCheck(UpdateSpecialCheckViewModel model);

        PQSpecialCheckInsuffListPagedModel GetAllSpecialCheckInsuffListForDETL(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", int PersonalId = 0, string spocEmail = "", short TeamMemberRowid = 0, int ClientUserRowID = 0);
        PQSpecialCheckInsuffListPagedModel GetAllSpecialCheckInsuffListForDETL1(int pageNo, int pageSize, string sort, string sortDir, string subcheckType, string Search = "", short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", int PersonalId = 0, string spocEmail = "", short TeamMemberRowid = 0, int ClientUserRowID = 0,int allclints=0);
        PQSpecialCheckInsuffListPagedModel GetAllSpecialCheckInsuffListForCSSpoc(int pageNo, int pageSize, string sort, string sortDir, string subcheckType, string Search = "", short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", int PersonalId = 0, string spocEmail = "", short TeamMemberRowid = 0, int ClientUserRowID = 0);
        UpdatePQSpecialCheckInsuffClearViewModel GetPQSpecialCheckDetailsById(int SpecialCheckRowID = 0);
        int SaveChanges();
        void UpdatePQSpecialChackInsuffClear(UpdatePQSpecialCheckInsuffClearViewModel model);
        UpdateSpecialCheckViewModel GetSpecialCheckInfoForQualityCheck(int SpecialCheckRowId = 0);
        void ViewSpecialCheckInfoDETL(UpdateSpecialCheckViewModel model);
        void UpdateSpecialCheckRejectionByDEQC(UpdateSpecialCheckViewModel model, bool flag = false);

        #region TempSpecialChecks

        AddTempSpecialCheckViewModel GetTempSpecialCheckInfoForDataEntry(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0);
        void AddTempSpacelCheckInformation(AddTempSpecialCheckViewModel model);
        void UpdateTempSpecialInfoByCandidate(AddTempSpecialCheckViewModel model);
        int GetAllTempPQSpecialByClientId(short ClientRowID = 0);
        void UpdateTempSpecialCheckInfoByDataEntry(AddTempSpecialCheckViewModel model);

        #endregion
    }
}
