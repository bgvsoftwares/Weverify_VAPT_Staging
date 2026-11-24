using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.ProvidedInfoViewModel;
using ViewModels.TempViewModel;

namespace BAL.ProvidedInfoRepository
{
    public interface IEducationInfoRepository
    {
        void AddEducationInformation(AddPQEducationViewModel model);
        CollegeAddressAndAffiliated GetAddressAndAffiliated(int CollegeRowID);
        int GetEducationIDByUniqueComponentID(int PersonalRowID = 0, string UniqueComponentID = "");
        AddPQEducationViewModel GetEducationInfoForDataEntry(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0);
        void UpdateEducationInfoByDataEntry(AddPQEducationViewModel model);
        int GetAllEducationsByClientId(short ClientRowID = 0);
        int SaveChanges();
        PQUpdateEducationViewModel GetEducationInfoForQualityCheck(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0);
        PQUpdateEducationViewModel GetEducationInfoForQualityCheck(int EducationRowID = 0);
        void UpdateEducationInfoByQualityCheck(PQUpdateEducationViewModel model);
        void UpdateEducationInfoByManager(PQUpdateEducationManagerViewModel model);
        void UpdateEducationInfoByTeamLeader(PQUpdateEducationTeamLeaderViewModel model);
        void UpdateEducationInfoByTeamMember(PQUpdateEducationTeamMemberViewModel model);
        void RejectionUpdateEducationInfoByQC(UpdateEducationRejectionViewModel model);
        void UpdateEducationInfoByTeamLeaderReAllocated(PQUpdateEducationTeamLeaderViewModel model);
        bool IfExistEducation(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0, string CheckStatus = "");
        bool IfExistEducationForDELeftMenu(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0);
        bool IfExistEducationForQCLeftMenu(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0, int CheckStatusid = 0);
        CollegeAddressAndAffiliated GetAddressAndAffiliated_New(int CollegeRowID);
        //PQUpdateEducationResearcherViewModel UpdateEducationInfoByResearcher(PQUpdateEducationResearcherViewModel model);

        #region ********* Temp Education*********

        AddTempPQEducationViewModel GetTempEducationInfoByself(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0);

        int GetAllTempEducationsByClientId(short ClientRowID = 0);

        void UpdateTempEducationInfoByself(AddTempPQEducationViewModel model);

        void AddTempEducationInformation(AddTempPQEducationViewModel model);

        #endregion

        PQEducationInsuffListPagedModel GetAllEducationInsuffListForDETL(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", int PersonalId = 0, string spocEmail = "", short TeamMemberRowid = 0, int ClientUserRowID = 0);
        PQEducationInsuffListPagedModel GetAllEducationInsuffListForDETL1(int pageNo, int pageSize, string sort, string sortDir,string subCheckType, string Search = "", short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", int PersonalId = 0, string spocEmail = "", short TeamMemberRowid = 0, int ClientUserRowID = 0,int allclinte=0);
        PQEducationInsuffListPagedModel GetAllEducationInsuffListForCSSpoc(int pageNo, int pageSize, string sort, string sortDir, string subcheckType, string Search = "", short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", int PersonalId = 0, string spocEmail = "", short TeamMemberRowid = 0, int ClientUserRowID = 0);
        UpdatePQEducationInsuffClearViewModel GetPQEducationDetailsById(int EducationRowID = 0);
        void UpdatePQEducationInsuffClear(UpdatePQEducationInsuffClearViewModel model);
        void ViewEducationInfoDETL(PQUpdateEducationViewModel model);
        void UpdateEducationRejectionByDEQC(PQUpdateEducationViewModel model,bool flag=false);
        void ViewEducationInfoByVrifierTL(ViewEducationViewModel model);
        CollegeAddressAndAffiliated GetUniversityName(short CollegeRowID);
    }
}
