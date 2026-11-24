using DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.ClientViewModel;
using ViewModels.ProvidedInfoViewModel;
using ViewModels.TempViewModel;
using ViewModels.VerificationViewModel;

namespace BAL.ProvidedInfoRepository
{
    public interface IPQPersonalRepository       
    {
        int AddCandidateNew(AddPQPersonalBulkImportViewModel model);
        int AddCandidate(AddPQPersonalViewModel model);
        bool IsCandidateExistWiproBulk(short clientRowId, string VoterIDCardNo, string DrivingLicenseNo, string AadhaarCardNo, string PassportNo, string PanCardNo, string ClientrefId);
        void AddCandidateAsBulkImport(AddPQPersonalBulkImportViewModel model);
        IEnumerable<PQPersonalViewModel> GetCandidates();
        IEnumerable<ClientLatestCasesInitiatedToCandidate> LastFiveClientCasesInitiated(int clientRowID, int clientUserRowID);
        PQPersonalInfoListPagedModel GetCandidateListForAllocation(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string RecievingfromDate = "", string RecievingToDate = "");
        //following method added on 20-Feb-2017 by Arvind
        PQPersonalInfoListPagedModel GetAllocatedCandidateListForDETL(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, short AllocateDEID = 0, short AllocateQCID = 0, short AllocateDEScan = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string SearchType = "", string CaseStatus = "");
        //following method added on 20-Feb-2017 by Arvind
        PQPersonalInfoListPagedModel GetAllocatedCandidateListForDEScan(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string CaseStatus = "", byte tatHours = 0);

        PQPersonalInfoListPagedModel GetAllocatedCandidateListForDataEntry(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string CaseStatus = "", byte tatHours = 0);

        PQPersonalInfoListPagedModel GetAllocatedCandidateListForQC(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string SearchType = "", string CaseStatus = "", byte tatHours = 0);

        bool IsCandidateExist(short clientRowId, string VoterIDCardNo, string DrivingLicenseNo, string AadhaarCardNo, string PassportNo, string PanCardNo);
        UpdatePQPersonalViewModel GetPQPersonalByCandidateCode(string CompanyRefNo = "");
        int GetAllCandidatesById(short ClientRowID = 0);
        int GetAllCandidateCodeGenerationById(short ClientRowID = 0);
        UpdatePQPersonalViewModel GetPQPersonalForUpdateById(int PersonalRowID = 0);
        PQPersonalDetailViewModel GetCandidateNameOrCmpIDById(int PersonalRowID = 0);
        void UpdateCandidate(UpdatePQPersonalViewModel model);
        void UpdateCadidateAllocationByID(int PersonalRowID = 0);
        void UpdatePQPersonalAllocateDEOrQCByID(UpdatePQPersonalViewModel model);
        bool UpdatePQPersonalReAllocateDEOrQCByID(UpdatePQPersonalViewModel model);
        byte GetClientTATByCandidateId(short cid = 0);
        void UpdatePQPersonalForQC(int PersonalRowID = 0, string CaseStatus = "");
        void UpdatePQPersonalForDE(int PersonalRowID = 0, string CaseStatus = "");
        void UpdatePQPersonalForDEQualityCheck(int PersonalRowID = 0, string CaseStatus = "");
        short GetClientDetailsByPersonalId(int id = 0);
        short GetClientDetailsByPersonalIdForB2C(int id = 0);
         PQAllChecksCountViewModel GetAllInsuffChecksCountForDETL1();
        PQEmploymentInsuffListPagedModel GetEmploymentInsuffChecksDetailForDETL1(int pageNo, int pageSize, string Search = "", string CheckType = "");
        PQEducationInsuffListPagedModel GetEducationInsuffChecksDetailForDETL1(int pageNo, int pageSize, string Search = "", string CheckType = "");
        PQCriminalInsuffListPagedModel GetCriminalInsuffChecksDetailForDETL1(int pageNo, int pageSize, string Search = "", string CheckType = "");
        PQReferenceInsuffListPagedModel GetReferenceInsuffChecksDetailForDETL1(int pageNo, int pageSize, string Search = "", string CheckType = "");
        PQAddressInsuffListPagedModel GetAddressInsuffChecksDetailForDETL1(int pageNo, int pageSize, string Search = "", string CheckType = "");
        PQSpecialCheckInsuffListPagedModel GetSpecialInsuffChecksDetailForDETL1(int pageNo, int pageSize, string Search = "", string CheckType = "");
        PQNationalIdentityInsuffListPagedModel GetNationalInsuffChecksDetailForDETL1(int pageNo, int pageSize, string Search = "", string CheckType = "");
        int SaveChanges();

        PQPersonalSessionViewModel GetPQPersonalInfoForSession(int PersonalRowID = 0);
        TempPQPersonalSessionViewModel GetTempPQPersonalInfoForSession(int PersonalRowID = 0);
        List<BGVInstructionslist> BGVInstructionslist(int PersonalRowID = 0, int ClientRowID=0);

        #region ****** Temp Client Candidate *****
        PQPersonalInfoListPagedModel GetCandidateListWhoHaveDoneDataEntry(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", int ClientUserRowID = 0);
        void AddTempPQPersonalCaseOfBulkUpload(AddTempPQPersonalForBulkUpViewModel model);
        TempPQPersonalInfoListPagedModel GetTempCandidateListByClientBulkUpload(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short ClientRowID = 0, int ClientUserRowID = 0);
        TempPQPersonalForSendMailViewModel GetTempPQPersonalDetailsForSendMailById(int TempPersonalRowID = 0);
        void UpdateTempPQPersonalCandidateUserID(int PersonalRowID = 0, int CandidateUserID = 0);
        #endregion

        #region ***** Temp PQPersonal Repository ***** 
        TempPQPersonalInfoListPagedModel GetTempCandidateListBySelf(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short ClientRowID = 0);
        void AddTempPQPersonal(AddTempPQPersonalViewModel model);
        bool IsTempPQPersonalExist(string ModileNo, string Email);
        UpdateTempPQPersonalViewModel GetTempPQPersonalByCandidateCode(string CandidateCode = "");
        UpdateTempPQPersonalViewModel GetTempPQPersonalForUpdateById(int TempPersonalRowID = 0);
        void UpdateCandidateBySelf(UpdateTempPQPersonalViewModel model);
        void UpdatePQPersonalForBySelfTL(int PersonalRowID = 0, string CaseStatus = "");
        TempPQPersonalDetailViewModel GetTempCandidateNameOrCmpIDById(int PersonalRowID = 0);
        PQPersonalInfoViewModel GetTempPersonalInfoByPersonalRowID(int PersonalRowID = 0);
        void UpdateTempPQPersonalInfo(PQPersonalInfoViewModel model);
        string UpdatetempPQCandidateChecksData(int crid, long prid);
        #endregion

        PQPersonalOrCheckForSendMailViewModel GetCheckOrPersonalDetailsForSendMailById(short ChFamilRowId = 0, short SubCheckRowID = 0, int CheckRowID = 0);

        PQPersonalInfoListPagedModel GetCandidateListByClientRowID(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short ClientRowID = 0, int ClientUserRowID = 0);
        bool IsCandidateEmailExist(string CandidateEmail);
        bool IsCandidateEmailExist(string CandidateEmail,int clientrowid);


        TempClientHrApprovalCasesListPagedModel GetClientHrApprovalAllCases(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short ClientRowID = 0, int ClientUserRowID = 0);
        PQAllChecksCountViewModel GetAllInsuffChecksCountForDETL(short teamMemberRowID = 0);
        PQAllChecksCountViewModel GetAllInsuffChecksCountForClient(short clientRowId = 0, int ClientUserRowID = 0);
        PQAllChecksCountViewModel GetAllInsuffChecksCountForCandidate(int PersonalId = 0);
        PQAllChecksCountViewModel GetAllInsuffChecksCountForCSSpoc(string SpocEmail = "");
        PQAllChecksCountViewModel GetAllInsuffChecksCountForCSSpoc1(int clientRowId, string SpocEmail = "");
        PQAllChecksCountViewModel GetAllInsuffChecksCountForCSSpoc1_MultipleClient(DataTable clientRowId, string SpocEmail = "");
        short GetDETLByClientID(short ClientID = 0, string Designation = "");

        void UpdateInsuffCheckByPeraonalIdAndSubCheckID(short CheckFamilyRowID, int CheckRowId, string Remarks);
        //bool UpdateCaseCompletedByPersonalId(int PersonalId);
        IEnumerable<PQPersonalCheckListForClientViewModel> GetPersonalCheckListByPersonalId(int personalId = 0);
        IEnumerable<PQPersonalCheckListForClientViewModel> GetPersonalCheckListByPersonalId1(int personalId = 0);
        IEnumerable<ExportPQPersonalDetailViewModel> GetClientAreaForExport(short ClientRowID);
        IEnumerable<ExportPQPersonalCandidateViewModel> GetCandidateAreaForExport(short ClientRowID = 0);
        string GetPQPersonalCaseDocUploadedByCandidate(long PersonalRowId);
        PQPersonalInfoListPagedModel GetCandidateListWhoHaveDoneDataEntry1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", int ClientUserRowID = 0, string CaseStatus = "",int allcliens=0);
        TempClientHrApprovalCasesListPagedModel GetClientHrApprovalAllCases1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short ClientRowID = 0, int ClientUserRowID = 0,string CaseStatus="", int allclient=0);
        IEnumerable<ExportPQPersonalCandidateViewModel> GetCandidateAreaForExport1(short ClientRowID = 0, string CaseStatus = "", int allclients = 0, int clientuserRowid = 0, string recievingfromDate = null, string recievingToDate = null);
        IEnumerable<ExportPQPersonalDetailViewModel> GetClientAreaForExport1(short ClientRowID, string CaseStatus = "", int ClientUserRowID = 0, int allclient = 0);
        PQAllChecksCountViewModel GetAllInsuffChecksCountForClient1(short clientRowId = 0, int ClientUserRowID = 0,int allclints=0);
        PQPersonalInfoListPagedModel GetCandidateListForAllocation1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string CaseStatus = "", string rolename = "", short teamMemerRowId = 0, short ClientRowID = 0, string RecievingfromDate = "", string RecievingToDate = "", string ModuleType = "");
        PQPersonalInfoListPagedModel GetAllocatedCandidateListForDETL1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string rolename = "", short teamMemerRowId = 0, short ClientRowID = 0, short AllocateDEID = 0, short AllocateQCID = 0, short AllocateDEScan = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string SearchType = "", string CaseStatus = "", string ModuleType = "");
        PQPersonalInfoListPagedModel GetAllocatedCandidateListForDEScan1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string CaseStatus = "", string rolename = "", short teamMemerRowId = 0, short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", byte tatHours = 0);
        PQPersonalInfoListPagedModel GetAllocatedCandidateListForDataEntry1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string rolename="" ,short teamMemerRowId = 0, short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string CaseStatus = "", byte tatHours = 0,string version="", bool fetchDataCheckbox = false);
        PQPersonalInfoListPagedModel GetAllocatedCandidateListForQC1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string rolename = "", short teamMemerRowId = 0, short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string SearchType = "", string CaseStatus = "", byte tatHours = 0 , bool fetchDataCheckbox=false);
        PQAllChecksCountViewModel GetAllInsuffChecksCountForQC();
        IEnumerable<ExportPQPersonalCandidateViewModel> GetYTRExport1(short ClientRowID = 0, string CaseStatus = "", int clientUserRowId = 0, int allclients = 0);
        IEnumerable<ExportPQPersonalCandidateViewModel> GetInsufficencyExport1(short ClientRowID = 0, string CaseStatus = "", int ClientUserRowID = 0,int allclients=0);
        IEnumerable<SubcheckViewModel> getSubcheckdata(int ClientRowId, int PersonalRowID, string UsedataBase = "");
        IEnumerable<SubcheckViewModel> getSubcheckdataforDelete(int ClientRowId, int PersonalRowID);
        IEnumerable<SubcheckViewModel> GetSubchecksForSpocRemark(int PersonalRowID);
        
        IEnumerable<SubcheckViewModel> GetMastercheckId();
        IEnumerable<SubcheckViewModel> GetMasterCaseId();
        IEnumerable<ExportPQPersonalCandidateViewModel> GetCandidateAreaForExportGoAir(short ClientRowID = 0, string CaseStatus = "");
        //IEnumerable<SubcheckViewModel> GetMastercheckIdForSpoc();
        IEnumerable<SubcheckViewModel> GetMastercheckIdForSpoc(string Department);
        IEnumerable<SubcheckViewModel> getSubcheckdataforaddress(int ClientRowId, int PersonalRowID, string Department);
        IEnumerable<SubcheckViewModel> getAddresscheckName(int PersonalRowID);
        AddressCheckDetailsViewModel GetCandidateAddress(int PersonalRowId, int SubCheckRowId);
        bool GetCVValidation(string ClientCheckRowIDs);
        ClientCandidateSearchListPagedModel ClientCandidateSearchList(int pageNo, int pageSize, string sort, string sortDir, string SearchType = "", short ClientRowID = 0, int ClientUserRowID = 0, string SearchValue = "", int allclints=0);
        IEnumerable<SubcheckViewModel> GetSubSetchecks(string RefNo);
        IEnumerable<AddPQPersonalViewModel> CheckCandidateduplicateNameFromClient( int clientRowId, string Name, string MiddleName, string LastName);
        IEnumerable<AddTempPQPersonalForBulkUpViewModel> CheckCandidateduplicateNameFromCandidate(int clientRowId, string Name, string MiddleName, string LastName,string Email="",string MobNo="", string EmpId = "");
        ClientCandidateSearchListPagedModel GetDuplicateReferenceDetails(int pageNo, int pageSize, string sort, string sortDir, short ClientRowID = 0, int ClientUserRowID = 0, string TicketNo = "", string fromDate = "", string todate = "");
        IEnumerable<AddPQPersonalBulkImportViewModel> CheckCandidateduplicateNameFromDETL(int clientRowId, string Name, string MiddleName, string LastName, string Email = "",string MobNo="", string EmpId = "");
        APICandidateSummaryListPagedModel GetAPI_IntegrationCaseSummary(int pageNo, int pageSize, string sort, string sortDir, short ClientRowID = 0, int ClientUserRowID = 0, int status = 0, string referenceId = "");
        APICandidateSummaryDetailListPagedModel GetAPI_IntegrationCaseSummaryDetail( string UniqueReferenceID = "");
        List<APICandidateSummaryDetail> GetAPI_ReverificationCaseSummaryDetail(string UniqueReferenceID = "");
        List<APICandidateSummaryDetail> GetAPI_InsuffCaseSummaryDetail(string UniqueReferenceID = "");
        PQPersonalDetailsVerViewModel GetPersonalDetailsForTM(int personalRowid = 0, int subcheckRowid = 0);
        PQPersonalOrCheckForSendMailViewModel GetCheckOrPersonalDetailsForSendMailByIdNew(short ChFamilRowId = 0, short SubCheckRowID = 0, int personalRowid = 0,int CheckStatusId=0);
        int AddCandidate(AddPQPersonalBulkImportViewModel model);
        IEnumerable<SubcheckViewModel> GetMasterStopcheckStatusId();
        QuickCaseSearch QuickCaseSearch(string id, int clientRowID, string candidateName);
        GetLocationByPin GetLocationByPincode(int Pincode);
        IEnumerable<ExportPQPersonalCandidateViewModel> GetCandidateAreaForExportKRAZYBEE(short ClientRowID = 0, string CaseStatus = "", int allclients = 0, int clientuserRowid = 0);
        PQPersonalDetailsVerViewModel GetEduHisContactEmail(int personalRowid = 0, int subcheckRowid = 0);
    }
}
