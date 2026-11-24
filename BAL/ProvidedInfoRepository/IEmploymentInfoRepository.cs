using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.ProvidedInfoViewModel;
using ViewModels.TempViewModel;

namespace BAL.ProvidedInfoRepository
{
    public interface IEmploymentInfoRepository
    {
        IEnumerable<EmploymentDDLViewModel> OrganizationNames(string id);
        void AddEmploymentInformation(AddEmploymentViewModel model);

        PQEmploymentListPagedModel GetAllResultAwaitedChecksListForDETL1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string YTRDate = "", string CheckStatus = "", int ClientUserRowID = 0, int allClients = 0);
        PQEmploymentListPagedModel GetAllClientReviewChecksList(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string YTRDate = "", string CheckStatus = "", int ClientUserRowID = 0, int allClients = 0);
        int GetEmploymentIDByUniqueComponentID(int PersonalRowID = 0, string UniqueComponentID = "");
        AddEmploymentViewModel GetEmploymentInfoForDataEntry(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0);
        void UpdateEmploymentInfoByDataEntry(AddEmploymentViewModel model);
        EmployeeAddressAndLocation GetAddressAndLocation(short EmploymentRowID);
        int GetAllEmploymentByClientId(short ClientRowID = 0);
        int SaveChanges();
        UpdateEmploymentViewModel GetEmploymentInfoForQC(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0);
        UpdateEmploymentViewModel GetEmploymentInfoForQC(int EmploymentRowID = 0);
        void UpdateEmploymentInfoByQC(UpdateEmploymentViewModel model);
        void UpdateEmploymentInfoByManager(PQUpdateEmploymentManagerViewModel model);
        void UpdateEmploymentInfoByTeamLeader(PQUpdateEmploymentTeamLeaderViewModel model);
        void UpdateEmploymentInfoByTeamMember(PQUpdateEmploymentTeamMemberViewModel model);
        void RejectionUpdateEmploymentInfoByQC(UpdateEmploymentRejectionViewModel model);
        void UpdateEmploymentInfoByTeamLeaderReAllocated(PQUpdateEmploymentTeamLeaderViewModel model);
        bool IfExistEmployment(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0, string CheckStatus = "");
        bool IfExistEmploymentForDELeftMenu(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0);
        bool IfExistEmploymentForQCLeftMenu(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0,int CheckStatusid=0);
        PQUpdateEmploymentResearcherViewModel UpdateEmploymentInfoByResearcher(PQUpdateEmploymentResearcherViewModel model);
        PQEmploymentListPagedModel GetAllYTRChecksListForDETL(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string YTRDate = "", string CheckStatus = "", int ClientUserRowID = 0);

        #region Temp Employment *****************

        void AddTempEmploymentInformation(AddTempEmploymentViewModel model);
        int GetAllTempEmploymentByClientId(short ClientRowID = 0);
        AddTempEmploymentViewModel GetTempEmploymentInfoBySelf(short ClientRowId = 0, int PersonalRowID = 0, short ChFamilRowId = 0, short subCheckId = 0);
        void UpdateTempEmploymentInfoByDataEntry(AddTempEmploymentViewModel model);

        #endregion

        PQEmploymentInsuffListPagedModel GetAllEmploymentInsuffListForDETL(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "",int PersonalId = 0, string spocEmail = "", short TeamMemberRowid = 0, int ClientUserRowID = 0);
        UpdatePQEmploymentInsuffClearViewModel GetPQEmploymentDetailsById(int EmploymentRowID = 0);
        void UpdatePQEmploymentInsuffClear(UpdatePQEmploymentInsuffClearViewModel model);

        void ViewEmploymentInfoByDataEntry(ViewEmploymentViewModel model, bool flag = false);

        void ViewEmploymentInfoByVrifierTL(ViewEmploymentViewModel model);
        void UpdateEmploymentRejectionByDEQC(ViewEmploymentViewModel model);
        void ViewEmploymentInfoDETL(ViewEmploymentViewModel model);

        PQEmploymentYTRClearViewModel UpdateEmploymentYTRChecksByClient(PQEmploymentYTRClearViewModel model);
        PQEmploymentListPagedModel GetAllYTRChecksListForDETL1(int pageNo, int pageSize, string sort, string sortDir,  string Search = "", short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string YTRDate = "", string CheckStatus = "", int ClientUserRowID = 0,int allclient=0);
        PQEmploymentInsuffListPagedModel GetAllEmploymentInsuffListForCSSpoc(int pageNo, int pageSize, string sort, string sortDir, string subcheckType, string Search = "", short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", int PersonalId = 0, string spocEmail = "", short TeamMemberRowid = 0, int ClientUserRowID = 0);
        PQEmploymentInsuffListPagedModel GetAllEmploymentInsuffListForDETL1(int pageNo, int pageSize, string sort, string sortDir, string subcheckType, string Search = "", short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", int PersonalId = 0, string spocEmail = "", short TeamMemberRowid = 0, int ClientUserRowID = 0, int allclints=0);
        PQEmploymentYTRClearViewModel UpdateRefrenceYTRChecksByClient(PQEmploymentYTRClearViewModel model);

    }
}
