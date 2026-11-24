using DAL;
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
    public interface IPQVerifiedUploadDocRepository
    {

        PQVerifiedUploadDoc GetVerifiedUploadDoc1(VerifiedUploadDocViewModel model);
        void VerifiedUploadcaseDoc1(VerifiedUploadDocViewModel model);
        void VerifiedUploadDoc1(VerifiedUploadDocViewModel model);
        void AddPQVerifiedUploadDoc(PQAddVerifiedUploadDocViewModel model);
        int AddOfflineInstantCheckDocs(OfflineInstantCheckDocsViewModel model);
        int AddPQVerifiedUploadDocNew(PQAddVerifiedUploadDocViewModel model);      
        string ExistFileByPersonalIdSubCheckIdDocType(int PersonalRowID = 0, short SubCheckRowID = 0, string DocumentType = "");
        string ExistFileByPersonalIdSubCheckIdDocTypeNew(int PersonalRowID = 0, short SubCheckRowID = 0, string DocumentType = "",string FileName="");
        void AdInsuffUploadDoc(PQAddVerifiedUploadDocViewModel model);
        PQVerifiedUploadDocListPagedModel GetCandidateUploadedDocList(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short SubCheckRowID = 0, int PersonalRowID = 0,int clientRowid=0);
        UploadedDocListForRWQCTMPagedModel GetCandidateUploadedDocListForRWQCTM(int PersonalRowID = 0, short SubCheckRowID = 0);
        void UpdateArtifactDocumentByRWQCTM(IEnumerable<MapUploadedDocRWQCTMModel> model, int personalRowId, short SubcheckId);
        UploadedCaseDocListForRWQCTMPagedModel GetCandidateUploadedCaseDocListForRWQCTM(int PersonalRowID = 0);
        void UpdateArtifactCaseDocumentByRWQCTM(IEnumerable<MapUploadedDocRWQCTMModel> model, int personalRowId);
        bool IsExistsChecksInUploadDocList(int PersonalRowID = 0, short SubCheckRowID = 0);
        void VerifiedUploadDoc(VerifiedUploadDocViewModel model);
        void VerifiedAddressUploadDoc(VerifiedUploadAddressDocViewModel model);
        IEnumerable<PQVerifiedUploadDocViewModel> GetCheckUploadDocList(int PersonalRowID = 0, short SubCheckRowID = 0);
        IEnumerable<PQVerifiedUploadDocViewModel> GetTempCheckUploadDocList(int PersonalRowID = 0, short SubCheckRowID = 0);
        IEnumerable<PQVerifiedUploadDocViewModel> GetTempCheckUploadCaseDocList(int PersonalRowID = 0); 
        string DeleteUploadedDocsByID(int id = 0, short SubCheckRowID = 0);

        // Updated 19-01-2018 Rahul
        string DeleteUploadedTempDocsByID(int id = 0, short SubCheckRowID = 0);


        void UpdateTempPQVerifiedUploadDoc(AddUploadDocByHRViewModel model);
        #region ****  TempPQVerifiedUploadDoc  ****

        void AddTempPQVerifiedUploadDoc(AddTempPQVerifiedUploadDocViewModel model);

        TempPQVerifiedUploadDocListPagedModel GetTempCheckDocumentUploadedList(int pageNo, int pageSize, string sort,
            string sortDir, string Search = "", short SubCheckRowID = 0, int PersonalRowID = 0);

        // Updated 18-01-2017
        IEnumerable<PQVerifiedUploadDocViewModelUpdated> GetTempCheckDocumentUploadedListUpdated(short SubCheckRowID = 0, int PersonalRowID = 0);



        bool IsExistsTempChecksInUploadDocList(int PersonalRowID = 0, short SubCheckRowID = 0);


        // Updated 23-01-2017 Rahul
        bool IsExistsTempLoa(int PersonalRowID = 0);
        bool IsExistsTempAddressUpdated(int PersonalRowID = 0, short SubCheckRowID = 0);
        bool IsExistsTempEducationUpdated(int PersonalRowID = 0, short SubCheckRowID = 0);
        bool IsExistsTempEmploymentUpdated(int PersonalRowID = 0, short SubCheckRowID = 0);
        bool IsExistsTempReferenceUpdated(int PersonalRowID = 0, short SubCheckRowID = 0);
        bool IsExistsTempNationalIdentityUpdated(int PersonalRowID = 0, short SubCheckRowID = 0);
        bool IsExistsTempNationalCriminalUpdated(int PersonalRowID = 0, short SubCheckRowID = 0);
        bool IsExistsTempSpecialUpdated(int PersonalRowID = 0, short SubCheckRowID = 0);

        bool IsExistsTempAddressUpdatedManDoc(int PersonalRowID = 0, short SubCheckRowID = 0);
        bool IsExistsTempEduEmpManDoc(int PersonalRowID = 0, short SubCheckRowID = 0);


        #endregion

        VerificationInfoListPagedModel GetInsuffUploadedDocList(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short ClientRowID = 0, string CheckStatus = "", string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "");

        VerificationInfoListPagedModel GetRejectedListForDEScan(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short ClientRowID = 0, string CheckStatus = "", string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "");

        VerificationInfoListPagedModel GetReviewRequestedListForDETL(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short ClientRowID = 0, string CheckStatus = "", short TeamMemberID = 0);

        VerificationInfoListPagedModel GetFundPendingCheckListForDepartTL(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short ClientRowID = 0, short TeamMemberID = 0);
        VerificationInfoListPagedModel GetFundPendingCheckListForSpoc(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string SpocEmail = "", short ClientRowID = 0);
        ClientVerifyInfoListPagedModel GetFundPendingCheckListForClientById(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short ClientRowID = 0, int ClientUserRowID = 0);

        VerificationInfoListPagedModel GetRejectedChecksByDepTMForDEQC(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short ClientRowID = 0, short TeamMemberRowID = 0);

        VerificationInfoListPagedModel GetReviewRequestedListForDepartmentTL(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short ClientRowID = 0, string CheckStatus = "", short TeamMemberID = 0);
        VerificationInfoListPagedModel GetReviewRequestedListForDepartmentTLNew(int pageNo, int pageSize, string sort, string sortDir, string Search = "", int ClientRowID = 0, int CheckStatusID = 0, short TeamMemberID = 0, int CheckFamilyRowID = 0);

        ClientVerifyInfoListPagedModel GetFundPendingCheckListForClientById1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short ClientRowID = 0, int ClientUserRowID = 0,int allclient=0);


        int SaveChanges();
       
    }
}
