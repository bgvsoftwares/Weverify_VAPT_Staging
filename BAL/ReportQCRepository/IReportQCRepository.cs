using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels;
using ViewModels.ProvidedInfoViewModel;
using ViewModels.ReportQCViewModel;
using ViewModels.VerificationViewModel;

namespace BAL.ReportQCRepository
{
    public interface IReportQCRepository
    {
        #region **** For Report QC ****
        PQPersonalReportQCListPagedModel GetCandidateListForReportQCManager(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, byte tatHours = 0);
        PQPersonalReportQCListPagedModel GetCandidateListForReportQCTL(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, byte tatHours = 0);
        PQPersonalReportQCListPagedModel GetCandidateListForReportQCTeamMember(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0);
       VerificationInfoListPagedModel GetCandidateCheckListForReportQC(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short ClientRowID = 0, int PersonalRowID = 0);
        void UpdateAllocationForRWQCTLByRWQCManager(ReportQCAllocationViewModel model);
        void UpdateAllocationForRWQCTMByRWQCTL(ReportQCAllocationViewModel model);

        void UpdateAddressCheckRejectionByRWQCTM(ViewAddressViewModel model);

        void UpdateEmploymentCheckRejectionByRWQCTM(ViewEmploymentViewModel model);

        void UpdateEducationCheckRejectionByRWQCTM(ViewEducationViewModel model);

        void UpdateCriminalCheckRejectionByRWQCTM(ViewCriminalViewModel model);

        void UpdateReferenceCheckRejectionByRWQCTM(ViewReferenceViewModel model);

         void UpdateNationalIdentityCheckRejectionByRWQCTM(ViewNationalIdentityViewModel model);

        void UpdateSpecialCheckRejectionByRWQCTM(ViewSpecialCheckViewModel model);
        PQPersonalReportQCListPagedModel GetCandidateListForReportQCManager1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string Statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, byte tatHours = 0, string ClientRowIDs = "");
        #endregion

        #region **** Report Generate Part ****
        PQPersonalDetailsVerViewModel GetPersonalDetailsForTM(int personalRowid = 0, int subcheckRowid = 0);
        IEnumerable<CandidateCaseDocumentListViewModel> GetCandidateCaseDocListForReport(int PersonalRowID = 0);
        IEnumerable<CandidateCaseDocumentListViewModel> GetCandidatePersistentCaseDocListForReport(int PersonalRowID = 0, int PersonalRowID1 = 0);
        IEnumerable<CandidateCaseDocumentListViewModel> GetCandidateCasePdfListForReport(int PersonalRowID = 0);
        IEnumerable<CandidateCaseDocumentListViewModel> GetCandidateCasePersistentPdfListForReport(int PersonalRowID = 0, int PersonalRowID1 = 0);
        bool UpdateImageNameNew(Int64 VerifiedUploadRowID, string ImageName);
        IEnumerable<CandidateAllDocumentListViewModel> GetCandidatePdfListForConversionNew(int PersonalRowID = 0);
        IEnumerable<CandidateAllDocumentListViewModel> GetCandidatePdfListForConversionNew1(int PersonalRowID = 0);
        IEnumerable<PersonalCheckBriefDetailsViewModel> GetCandidateChecksBriefDetailsByPersonalId(int PersonalRowID = 0);
        IEnumerable<PersonalCheckBriefDetailsViewModel> GetCandidateChecksBriefDetailsByPersonalId1(int PersonalRowID = 0);
        IEnumerable<PersonalCheckBriefDetailsViewModel> GetCandidateChecksBriefDetailsByPersistentPersonalId1(int PersonalRowID = 0 , int PersonalRowID1 = 0);
        IEnumerable<PersonalCheckBriefDetailsViewModel> GetCandidateChecksBriefDetailsByPersonalIdTCS(int PersonalRowID = 0);
        IEnumerable<PersonalCheckBriefDetailsViewModel> GetCandidateChecksTcsvendorBriefDetailsByPersonalId1(int PersonalRowID = 0);
        IEnumerable<PersonalCheckBriefDetailsViewModel> GetCandidateChecksBriefCapcoDetailsByPersonalId1(int PersonalRowID = 0);
        IEnumerable<CandidateAllDocumentListViewModel> GetCandidateDocListForReport(int PersonalRowID = 0);
        IEnumerable<CandidateAllDocumentListViewModel> GetCandidatePersistentDocListForReport(int PersonalRowID = 0, int PersonalRowID1 = 0);
        //IEnumerable<CandidateAllDocumentListViewModel> GetCandidateLivenessDocListForReport(int PersonalRowID = 0);
        IEnumerable<CandidateAllDocumentListViewModel> GetCandidatePdfListForConversion(int PersonalRowID = 0);
        IEnumerable<CandidateAllDocumentListViewModel> GetCandidatePersistentPdfListForConversion(int PersonalRowID = 0, int PersonalRowID1 = 0);
        IEnumerable<CandidateAllDocumentListViewModel> GetCandidateLIvenessPdfListForConversion(int PersonalRowID = 0);
        IEnumerable<CandidateAllDocumentListViewModel> GetCandidatePdfListFor_Conversion(int PersonalRowID = 0);
        InterimReportGenerateViewModel GenerateCandidateInterimReportByReportQCTM(int PersonalRowID = 0);
        IEnumerable<AntecedentListViewModel> GetAntecedentVerList(short ClientRowId = 0);

        IEnumerable<AddresssReportViewModel> GetProvidedAddressForReportQC(int PersonalRowID = 0);
        IEnumerable<AddresssReportViewModel> GetVerifiedAddressForReportQC(int PersonalRowID = 0);

        IEnumerable<EmploymentReportViewModel> GetProvidedEmploymentForReportQC(int PersonalRowID = 0);
        IEnumerable<EmploymentReportViewModel> GetVerifiedEmploymentForReportQC(int PersonalRowID = 0);

        IEnumerable<EducationReportViewModel> GetProvidedEducationForReportQC(int PersonalRowID = 0);
        IEnumerable<EducationReportViewModel> GetVerifiedEducationForReportQC(int PersonalRowID = 0);

        IEnumerable<CriminalReportViewModel> GetProvidedCriminalForReportQC(int PersonalRowID = 0);
        IEnumerable<CriminalReportViewModel> GetVerifiedCriminalForReportQC(int PersonalRowID = 0);

        IEnumerable<ReferenceReportViewModel> GetProvidedReferenceForReportQC(int PersonalRowID = 0);
        IEnumerable<ReferenceReportViewModel> GetVerifiedReferenceForReportQC(int PersonalRowID = 0);

        IEnumerable<NationalIdentityReportViewModel> GetProvidedNationalIdentityForReportQC(int PersonalRowID = 0);
        IEnumerable<NationalIdentityReportViewModel> GetVerifiedNationalIdentityForReportQC(int PersonalRowID = 0);

        IEnumerable<SpecialCheckReportViewModel> GetProvidedSpecialCheckForReportQC(int PersonalRowID = 0);
        IEnumerable<SpecialCheckReportViewModel> GetVerifiedSpecialCheckForReportQC(int PersonalRowID = 0);
        IEnumerable<colorCodeViewModel> getcolorcodeByClient(int ClientRowId);
        IEnumerable<colorCodeViewModel> getcolorcodeByCompanyRefNo(string ClientRowId);
        IEnumerable<PersonalCheckBriefDetailsViewModel> GetCandidateChecksBriefDetailsByPersonalRowId(int PersonalRowID = 0);
        IEnumerable<PersonalCheckBriefDetailsViewModel> PersonalChecksBriefDetailsReportForResourcePro(int PersonalRowID = 0);

        IEnumerable<colorCodeViewModel> GetColorIdWithNameByClient(int ClientRowId);
        IEnumerable<ArtifactDocumentViewModel> GetArtifactDocumentByCheckFamilyId(int CheckFamilyRowID);
        #endregion

        void UpdatePQPersonalUploadReportDoc(string CompanyRefNo, string UploadDoc, string Rep_status, string ReportType = "", string ColorCode = "", int UserId = 0);
        RwqcTrackerUserData GetRwQcTMTrackerData(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short ClientRowID = 0);
        PQEmploymentResearchListPagedModel GetEmploymentCheckListForResearch1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short ClientRowID = 0, string RecievingDate = "", string CompletedDate = "", string DCheckStatus = "", string CaseStatus = "", string EmployerName = "", string Allocatedto = "", string SearchType = "");
        bool UpdateImageName(Int64 VerifiedUploadRowID, string ImageName);
        IEnumerable<PersonalCheckBriefDetailsViewModel> GetCandidateChecksBriefDetailsByPersonalId1New(int PersonalRowID = 0);
        bool UpdateCaseImageName(Int64 CaseDocRowID, string ImageName);
        RWOwnAllocationCases GetRWTMUserBucketData(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string Statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, short AllocateTM = 0, byte tatHours = 0);
        PQPersonalReportQCListPagedModel GetCandidateListForReportQCTL2(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string Statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, short AllocateTM = 0, byte tatHours = 0);
        PQPersonalReportQCListPagedModel GetCandidateListForReportQCTL1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string Statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, byte tatHours = 0, string ClientRowIDs = "");
        PQPersonalReportQCListPagedModel GetCandidateListForReportQCTL1TCSQC(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string Statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, byte tatHours = 0);
        PQPersonalReportQCListPagedModel GetCandidateListForReportTL_QCTL(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string Statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, byte tatHours = 0, string CaseStatus = "");
       PQPersonalReportQCListPagedModel GetCandidateListForReportQCTeamMember1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0);
        VerificationInfoListPagedModel GetCandidateCheckListForReportQC1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short ClientRowID = 0, int PersonalRowID = 0);
        PQPersonalReportQCListPagedModel GetCandidateListForReportQCManager_Interim(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, byte tatHours = 0, string Status = "");
        IEnumerable<colorCodeOrderingViewModel> getCheckColorCodeOrdering(int PersonalRowID);
        PQVerifiedUploadPDFImagesViewModel PQVerifiedUploadPDFImages(int pageNo, int pageSize, string CompanyRefNo);
        IEnumerable<TCS_BPO_Cal_Sheet> GetTCS_calSheet(int PersonalRowID = 0);
        IEnumerable<PersonalCheckBriefDetailsViewModel> GetCandidateChecksBriefDetails_TCSByPersonalId(int PersonalRowID = 0);
        IEnumerable<PersonalCheckBriefDetailsViewModel> GetCandidateChecksBriefDetails_wipro_ByPersonalId1(int PersonalRowID = 0);
        IEnumerable<CandidateAllDocumentListViewModel> GetCandidateDocListForReport_spoc(int PersonalRowID = 0);
        IEnumerable<CandidateAllDocumentListViewModel> GetCandidatePdfListForConversion_spoc(int PersonalRowID = 0);
        IEnumerable<PersonalCheckBriefDetailsViewModel> GetCandidateChecksBriefDetails_CventByPersonalId(int PersonalRowID = 0);
         IEnumerable<colorCodeOrderingViewModel> getCheckColorCodeOrderingTCS(int PersonalRowID);
        IEnumerable<CriminalCheck> CriminalCheck(int ClientRowId = 0);
        IEnumerable<CriminalReportViewModel> GetProvidedCriminalForReportQCTCS(int PersonalRowID = 0);
        IEnumerable<CriminalReportViewModel> GetVerifiedCriminalForReportQCTCS(int PersonalRowID = 0);
        IEnumerable<CriminalReportViewModel> GetProvidedCriminalForReportQCTCSCourt(int PersonalRowID = 0);
        IEnumerable<CriminalReportViewModel> GetVerifiedCriminalForReportQCTCSCourt(int PersonalRowID = 0);
        IEnumerable<PersonalCheckBriefDetailsViewModel> GetCandidateChecksBriefDetails_TCSByPersonalIdDBCheck(int PersonalRowID = 0);
        PQPersonalReportQCListPagedModel GetCandidateListForReportQCManager_TCSDB(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, byte tatHours = 0, string Status = "");
        PQPersonalReportQCListPagedModel GetCandidateListForReportTLDBTCS_QCTL(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string Statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, byte tatHours = 0, string CaseStatus = "");
        PQPersonalReportQCListPagedModel GetCandidateListForReportQCManager_Chola(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, byte tatHours = 0, string Status = "");
        PQPersonalReportQCListPagedModel GetCandidateListForReportTLDBTCS_QCTL_chola(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string Statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, byte tatHours = 0, string CaseStatus = "");
        PQPersonalReportQCListPagedModel GetCandidateListForReportQCManager_wipro(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, byte tatHours = 0, string Status = "");
        PQPersonalReportQCListPagedModel GetCandidateListForReportQCManager_Global(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, byte tatHours = 0, string Status = "");
        PQPersonalReportQCListPagedModel GetCandidateListForReportQCManager_daimler(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, byte tatHours = 0, string Status = "");
        PQPersonalReportQCListPagedModel GetCandidateListForReportQCManager_TCSINTERIMYTR(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, byte tatHours = 0, string Status = "");

        PQPersonalReportQCListPagedModel GetCandidateListForReportQCManager_titan(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, byte tatHours = 0, string Status = "");
        PQPersonalReportQCListPagedModel GetCandidateListForReportTLDBTCS_QCTL_Wipro(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string Statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, byte tatHours = 0, string CaseStatus = "");
        PQPersonalReportQCListPagedModel GetCandidateListForReportTLDBTCS_QCTL_Daimer(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string Statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, byte tatHours = 0, string CaseStatus = "");
        PQPersonalReportQCListPagedModel GetCandidateListForReportTLDBTCS_QCTL_Titan(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string Statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, byte tatHours = 0, string CaseStatus = "");
        PQPersonalReportQCListPagedModel GetCandidateListForReportTLDBTCS_QCTL_Global(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string Statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, byte tatHours = 0, string CaseStatus = "");
        IEnumerable<PersonalCheckBriefDetailsViewModel> GetCandidateChecksBriefDetails_wipro_BGV2_ByPersonalId(int PersonalRowID = 0);
        IEnumerable<SpecialCheckReportViewModel> GetProvidedSpecialCheckForReportQCDB(int PersonalRowID = 0);
        IEnumerable<SpecialCheckReportViewModel> GetVerifiedSpecialCheckForReportQCDB(int PersonalRowID = 0);
        DashboardInterimCaseCheckViewModel GetInterimStatusCheck(int pageNo, int pageSize, string Search = "");
        IEnumerable<PersonalCheckBriefDetailsViewModel> GetCandidateChecksBriefDetailsByPersonalIdTechM(int PersonalRowID = 0);
        IEnumerable<colorCodeOrderingViewModel> getCheckColorCodeOrderingTechM(int PersonalRowID);
        IEnumerable<SpecialCheckReportViewModel> GetProvidedSpecialCheckForReportQCDurgTechM(int PersonalRowID = 0);
        IEnumerable<SpecialCheckReportViewModel> GetVerifiedSpecialCheckForReportQCDurgTechM(int PersonalRowID = 0);
        IEnumerable<SpecialCheckReportViewModel> GetProvidedSpecialCheckForReportQCTechM(int PersonalRowID = 0);
        IEnumerable<SpecialCheckReportViewModel> GetVerifiedSpecialCheckForReportQCTechM(int PersonalRowID = 0);
        PQPersonalReportQCListPagedModel GetCandidateListForReportQCManager_TCSBPO(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, byte tatHours = 0, string Status = "");
        IEnumerable<PersonalCheckBriefDetailsViewModel> GetCandidateChecksBriefDetails_TCS_IT_ByPersonalId(int PersonalRowID = 0);
        IEnumerable<colorCodeOrderingViewModel> getCheckColorCodeOrderingTCS_IT(int PersonalRowID);
        IEnumerable<CandidateAllDocumentListViewModel> GetCandidatePdfListForConversion_Address(int PersonalRowID = 0);
        IEnumerable<WiproCheckSOWViewModel> GetCheckSOW(int PersonalRowID = 0);
        IEnumerable<ViewWiproCheckSOWViewModel> GetCheckSOWView(int PersonalRowID = 0);
        PQPersonalReportQCListPagedModel InterimRequest(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, byte tatHours = 0, string Status = "", string ClientRowIDs = "");
        IEnumerable<CaseAntecedentComment> GetRWQCComment(int personalRowId);
        IEnumerable<WiproAnnexure> WiproAnnexure(int PersonalRowID);
         IEnumerable<PersonalCheckBriefDetailsViewModel> GetCandidateChecksBriefDetails_ConcentrixByPersonalId(int PersonalRowID = 0);
        IEnumerable<PersonalCheckBriefDetailsViewModel> PersonalChecksBriefDetails_ReportForICICI(int PersonalRowID = 0);
        IEnumerable<WiproAnnexure> WiproAnnexureDOPStandard(int PersonalRowID);
        IEnumerable<PersonalCheckBriefDetailsViewModel> GetCandidateChecksBriefDetails_wipro_ByPersonalId_withoutStandard(int PersonalRowID = 0);
        PQPersonalReportQCListPagedModel getDashboardReportDetail_wiproManager_unallocated(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, byte tatHours = 0, string Status = "");
        PQPersonalReportQCListPagedModel Dashboard_Report_detail_wiproTL_Unallocated(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string Statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, byte tatHours = 0, string CaseStatus = "");
        IEnumerable<PersonalCheckBriefDetailsViewModel> GetNagarrowCandidateChecksBriefDetailsByPersonalId(int PersonalRowID = 0);
        IEnumerable<PersonalCheckBriefDetailsViewModel> GetCandidateChecksBriefDetailsByGenpact(int PersonalRowID = 0);
        AddFordRemarkInfoListPagedModel GetReportActionRemark(int pageNo, int pageSize, string PersonalRowID);
        AddFordRemarkInfoListPagedModel GetClientReportActionRemark(string ClientRowid);
        IEnumerable<PersonalCheckBriefDetailsViewModel> PersonalChecksBriefDetails_Sutherland_Report(int PersonalRowID = 0);
        PQPersonalReportQCListPagedModel GetCandidateListForReportQCTL1_Interim(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string Statusid = "", short teamMemerRowId = 0, int ClientRowID = 0, byte tatHours = 0, string ClientRowIDs = "");
        PQPersonalReportQCListPagedModel GetCandidateListForReportQCTL1_InterimNEW(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string Statusid = "", short teamMemerRowId = 0, int ClientRowID = 0, byte tatHours = 0);

        IEnumerable<CandidateAllDocumentListViewModel> GetCandidateDocListForReportArtifactWithApprove(int PersonalRowID = 0);
        IEnumerable<CandidateAllDocumentListViewModel> GetCandidatePdfListForConversionWithOutApprove(int PersonalRowID = 0);
        IEnumerable<PersonalCheckBriefDetailsViewModel> PersonalChecksBriefDetails_ReportForCapGemini(int PersonalRowID = 0);
        IEnumerable<PersonalCheckBriefDetailsViewModel> SutherlandCriminalChecksBriefDetails_Report(int PersonalRowID = 0);
        void UpdatePQPersonalUploadReportSutherland(string CompanyRefNo, string UploadDoc, string Rep_status, string ReportType = "", string ColorCode = "", int UserId = 0);
        void UpdatePQPersonalUploadReportTitan(string CompanyRefNo, string UploadDoc, string Rep_status, string ReportType = "", string ColorCode = "", int UserId = 0);
        ReportViewModel GetPersonalInfo(int PersonalRowID = 0);
        ReportViewModel GetReportPersonalInfo(int PersonalRowID = 0);
        ReportViewModel GetReportLivenessPersonalInfo(int PersonalRowID = 0);
        IEnumerable<PersonalCheckBriefDetailsViewModel> PersonalChecksBriefDetails_ReportSpoc(int PersonalRowID = 0);
        InterimReportGenerateViewModel GetInterimReportPersonalInfo(int PersonalRowID = 0);
        IEnumerable<PersonalCheckBriefDetailsViewModel> PersonalChecksBriefDetails_ReportNew_techM(int PersonalRowID = 0);
        ReportViewModel GetCapgeminiReportPersonalInfo(int PersonalRowID = 0);
        IEnumerable<PersonalCheckBriefDetailsViewModel> PersonalChecksBriefDetails_Report_Northern(int PersonalRowID = 0);
        ReportViewModel GetReportPersonalInfo_Genpact(int PersonalRowID = 0);
        ReportViewModel GetReportPersonalInfo_Accenture(int PersonalRowID = 0, int SubCheckRowID=0);
        ReportViewModel GetPersonalInfo_Accenture_CCC(int PersonalRowID = 0);        
        ReportViewModel GetReportPersonalInfo_Accenture_CDC(int PersonalRowID = 0, int SubCheckRowID = 0);
        ReportViewModel GetPersonalInfo_Accenture_IdCheck(int PersonalRowID = 0, int SubCheckRowID = 0);
        ReportViewModel GetPersonalInfo_Accenture_LegalAgeCheck(int PersonalRowID = 0, int SubCheckRowID = 0);
        ReportViewModel GetPersonalInfo_Accenture_CDC(int PersonalRowID = 0, int SubCheckRowID = 0);
        
        ReportViewModel GetReportPersonalInfo_Accenture_CurrentCompany(int PersonalRowID = 0, int SubCheckRowID = 0);
        ReportViewModel GetReportPersonalInfo_Accenture_CurrentEmployementTier3(int PersonalRowID = 0 ,int SubCheckRowID=0);
        UpdateEmployementTierStatusViewModel GetCurrentEmployementTier3(int PersonalRowID = 0,int subCheckId=0);

        IEnumerable<PersonalCheckBriefDetailsViewModel> PersonalChecksBriefDetails_PWC_Report(int PersonalRowID = 0);
        IEnumerable<PersonalCheckBriefDetailsViewModel> PersonalChecksBriefDetails_CognizantReport(int PersonalRowID = 0);
        IEnumerable<PersonalCheckBriefDetailsViewModel> PersonalChecksBriefDetails_ReportNew_fractal(int PersonalRowID = 0);
        IEnumerable<PersonalCheckBriefDetailsViewModel> PersonalChecksBriefDetails_IBM(int PersonalRowID = 0);
        IEnumerable<PersonalCheckBriefDetailsViewModel> PersonalChecksBriefDetails_ReportNew_Infosys(int PersonalRowID = 0);
        IEnumerable<DrugTestResult> DrugTestResult(int PersonalRowID = 0);
        IEnumerable<PersonalCheckBriefDetailsViewModel> PersonalChecksBriefDetails_TechMNew(int PersonalRowID = 0);
        IEnumerable<ViewModels.ReportQCViewModel.WiproUANCheck> GetWipro_UAN_Detail(int PersonalRowID = 0);

        IEnumerable<PersonalCheckBriefDetailsViewModel> PersonalChecksBriefDetails_Tiger_Analytics(int PersonalRowID = 0);
        IEnumerable<PersonalCheckBriefDetailsViewModel> PersonalChecksBriefDetails_Tiger_AddC_Analytics(int PersonalRowID = 0);
        IEnumerable<ViewModels.ReportQCViewModel.WiproGapChecks> GetWipro_Gap_Detail(int PersonalRowID = 0);
        IEnumerable<ViewModels.ReportQCViewModel.Comperative_Tigerlsts> GetTiger_Comperative(int PersonalRowID = 0);
        IEnumerable<CandidateAllDocumentListViewModel> PdfToImage(int PersonalRowID = 0);
        IEnumerable<CandidateAllDocumentListViewModel> GetCandidateDocForReport(int PersonalRowID = 0);
        ReportViewModel GetUANLTIDetail(ReportViewModel model);

        IEnumerable<PersonalCheckBriefDetailsViewModel> GetCandidateChecksBriefFractalOldDetailsByPersonalId1(int PersonalRowID = 0);
        IEnumerable<ViewModels.ReportQCViewModel.MasterArtifactMappingsOrdering> GetMasterArtifactMappingsOrdering(string ColorCode, int ClientRowID, int CheckFamilyRowID);
        List<ReportVerificationScope> GetWiproVerificationScopeReport(int personalRowId);
        IEnumerable<CandidateAllDocumentListViewModel> GetCandidatePdfListFor_Conversion_wipro(int PersonalRowID = 0);
        IEnumerable<CandidateAllDocumentListViewModel> GetCandidateDocForReport_wipro(int PersonalRowID = 0);
        IEnumerable<PersonalCheckBriefDetailsViewModel> GetCandidateChecksBriefDetails_wipro_bulk(int PersonalRowID = 0);




    }
}
