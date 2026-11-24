using System.Collections.Generic;
using ViewModels;
using ViewModels.ClientViewModel;
using ViewModels.HRViewModels;
using ViewModels.ProvidedInfoViewModel;

namespace BAL
{
    public interface IDropDownMasterRepository
    {
        bool GetcheckUsingSubcheckRowId(string VendorRowID, string SubCheckRowID, string PersonalRowid);

        IEnumerable<StateDDLViewModel> GetStatesByCountryIdForDDLnew(short CountryRowID, short teamMemerRowId, string CheckStatus);

        IEnumerable<GetMasterVendor> GetMasterVendor(short VendorRowID);
        IEnumerable<CountryDDLViewModel> GetCountriesForDDL();
        IEnumerable<StateDDLViewModel> GetStatesByCountryIdForDDL(short CountryRowID);
        IEnumerable<DistrictDDLViewModel> GetDistrictsByStateIdForDDL(short StateRowID);
        IEnumerable<LocationDDLViewModel> GetLocationsByDistrictIdForDDL(short DistrictRowID);
        IEnumerable<CheckFamilyDDLViewModel> GetCheckFamilyForDDL();
        IEnumerable<CheckFamilyDDLViewModel> GetMasterCheckFamilies(int Id);
        EditPQClientChecksViewModel GetPQClientChecksDetails(int ClientCheckRowID);
        EditPackageClientChecksViewModel GetPackageChecksDetails(int ClientPackageRowID);
        IEnumerable<UniversityDDLViewModel> GetUniversityForDDL();
        IEnumerable<ClientSubGroupDDLViewModel> GetClientSubGroupDDL();
        IEnumerable<ClientSubGroupDDLViewModel> GetClientSubGroupDDLbyId(int tmid = 0);
        IEnumerable<CategoryDDLViewModel> GetCategoriesForDDL();
        IEnumerable<DepartmentDDLViewModel> GetDepartmentsForDDL();

        IEnumerable<DesignationDDLViewModel> GetDesignationsForDDL();
        IEnumerable<DesignationDDLViewModel> GetDesignationsForDDL(short id);

        IEnumerable<RepoToTeamMemberDDLViewModel> GetRepoToTeamMemberForDDL(short tdid = 0);
        IEnumerable<TeamMemberDDLViewModel> GetTeamMemberForDDL();
        IEnumerable<SpocForClientChkBLViewModel> GetSpocForClientForChkBoxList();

        IEnumerable<PrimarySpocForClientChk> GetPrimarySpocForClientForChkBoxList();
        IEnumerable<SecondarySpocForClientChk> GetSecondarySpocForClientForChkBoxList();

        IEnumerable<ClientDDLViewModel> GetClientListForDDLByClientSpocEmailID(string SpocEmailID = "");
        IEnumerable<SeverityGridDDLViewModel> GetClientSeveritiesGridDDL(int ClientRowID);

        IEnumerable<BillingCycleDDLViewModel> GetBillingCycleForDDL();
        IEnumerable<BillingCycleDDLViewModel> GetBillingCycleForDDLNew();
        IEnumerable<BranchOfficeDDLViewModel> GetBranchOfficeForDDL();
        IEnumerable<BranchOfficeDDLViewModel> GetBranchOfficeForDDLNew();
        IEnumerable<ClientAbbSubGroupDDLViewModel> GetClientAbbSubGroupDDL(short ClientAbbRowId);
        IEnumerable<SeverityGridDDLViewModel> GetSeverityGridDDL();
        IEnumerable<AntecedentDDLViewModel> GetAntecedentDDL();
        IEnumerable<ClientDDLViewModel> GetClientListWithAbbAndLocationForDDL();
        IEnumerable<AlertTypeMaster> GetAlerttypeMaster();
        IEnumerable<MailTypeMaster> GetMailTypeMaster();
        IEnumerable<PackageNameMater> GetPackageNameList(int clientrowid);
        IEnumerable<ClientDesignationsDDLViewModel> GetClientDesignationsDDL();
        IEnumerable<PackagesDDLViewModel> GetPackagesForDDL();
        IEnumerable<ClientDDLViewModel> GetClientBaseOnloginMemberForDDL(short TeamMemberRowID = 0);
       
        IEnumerable<EmploymentDDLViewModel> GetEmploymentDDL();
        IEnumerable<CollegeDDLListViewModel> GetCollegeDDL();
        IEnumerable<DegreeDDLListViewModel> GetDegreeDDL();
        IEnumerable<SeverityGridDDLViewModel> GetColorDDL();
        IEnumerable<DispositionDDLViewModel> DispositionForDDL(short ChFamilRowId=0, short ClientRowId=0);
        IEnumerable<VendorDDLViewModel> GetVendorForDDL(short StateRowId = 0);
        IEnumerable<StateDDLViewModel> GetStatesByVendorDDL(short VendorId = 0);
        IEnumerable<VendorUserDDLViewModel> GetVendorByUserDDL(short VendorId = 0);

        //IEnumerable<StateDDLViewModel> GetStatesByCountryIdForDDL(short CountryRowID, short teamMemerRowId);
        IEnumerable<StateDDLViewModel> GetStatesByArressStatesIdForDDL();
        IEnumerable<LocationDDLViewModel> GetCityByArressStatesIdForDDL();
        IEnumerable<PartnerDDLViewModel> GetPartnerByArressPartnerIdForDDL();
        IEnumerable<CollegeDDLListViewModel> GetCollegeDDLForVerification(int CollegeID);
        IEnumerable<DegreeDDLListViewModel> GetDegreeDDLForVerifier(int DegreeId);
        IEnumerable<LocationDDLViewModel> GetCityForDDL(short StateRowId = 0, short teamMemerRowId = 0);

        IEnumerable<StateDDLViewModel> GetStatesByCountryIdForDDL(short CountryRowID, short teamMemerRowId, string CheckStatus);
        #region Comman Funtion
        IEnumerable<CheckActionHistoryViewModel> GetCheckActionHistory(int PersonalRowID = 0, short subCheckId = 0);
        IEnumerable<CaseActionHistoryViewModel> GetCaseActionHistories(int PersonalRowID = 0);
        IEnumerable<ReportHistoryViewModel> GetCaseReportHistories(int PersonalRowID = 0);
        IEnumerable<PQVerifiedUploadDocViewModel> PQAddressVerGetDocumentList(int PersonalRowID = 0, short subCheckId = 0);
        IEnumerable<VerifiedResearchDoc> VerifiedEduResearchDocumentList(int PersonalRowID, int SubCheckRowID, string FilePath);
        IEnumerable<VerifiedResearchDoc> VerifiedEMPResearchDocumentList(int PersonalRowID, int SubCheckRowID, string FilePath);
        IEnumerable<PQAntecedentList> GetAntecedentVerList(short ClientRowId = 0, short ChFamilRowId = 0, short subCheckId = 0);
        IEnumerable<PQAntecedentList> GetAntecedentList(short ClientRowId = 0, short ChFamilRowId = 0, short subCheckId = 0);
        #endregion
        IEnumerable<SubCheckFamilyDDLViewModel> GetSubCheckFamilyForDDL(int id);
        IEnumerable<SubCheckFamilyDDLViewModel> GetSubCheckFamily(int id);        
        IEnumerable<SubcheckViewModel> GetMastercheckId();
        IEnumerable<AddTempPQVerifiedUploadDocViewModel> GetMobileUploaddoc(int PersonalRowId, int subcheckId);
        IEnumerable<ClientDDLViewModel> GetClientGroupingInfo(int ClientRowid);
        IEnumerable<DocumentDDLViewModel> GetTCSDocDetail(int CheckFamilyRowId);
        IEnumerable<SubcheckViewModel> BindChecksDetails(string RefNo);
        IEnumerable<ClientDDLViewModel> userwise_ReportingAccess(int ClientRowid);
        IEnumerable<DuplicateCandidateTicket> GetDupliacteTicket(int ClientRowId);
        IEnumerable<DuplicateCandidateTicket> GetDupliacteTicketDETL(int Teamid);
        IEnumerable<ClientDDLViewModel> GetAllClient();
        IEnumerable<ClientDDLViewModel> GetActiveClientList();
        IEnumerable<VendorDDLViewModel> GetBillingVendorList();
        IEnumerable<ClientDDLViewModelEnc> GetClientListForDDLByClientSpocEmailIDEnc(string SpocEmailID = "");
        IEnumerable<ClientDDLViewModelEnc> GetActiveClientListEnc();
        //IEnumerable<PQVerifiedUploadDocViewModel> GetMCADocumentList(int PersonalRowID = 0, short subCheckId = 0);
        // IEnumerable<ClientDDLViewModel> GetClientListWithAbbAndLocationForDDLclient();
        IEnumerable<ClientDDLViewModel> GetClientListForDDLByClientSpoc(string SpocEmailID = "");
        IEnumerable<LocationDDLViewModel> GetCityForDDLStateName(string StateName = "", short teamMemerRowId = 0);
        IEnumerable<LocationDDLViewModel> GetCityNamePartnerAllocation(int StateRowId = 0, short teamMemerRowId = 0);
        AddClientCustomizedSubGroupViewModel GetClientCustomized(string ClientCheckRowID,string ClientAbbRowID, string checkFamilyRowID, string subcheckRowid, string CustomizedName, string Search = "", int pageNo = 1, int pageSize = 1, int page = 1, string sort = "", string sortDir = "asc");
       
        IEnumerable<DesignationDDLViewModel> GetDesignationsForDDLNew(string Department);
        IEnumerable<TeamMemberDDLViewModel> GetOprationTeam(int DesignationRowID);
        IEnumerable<TeamMemberAllocationDDLViewModel> GetOprationAlloactionTeam(int DesignationRowID);
        IEnumerable<ManageSubAdminViewModels> GetAdmin(int ClientRowID);
        IEnumerable<BusinessUnitView> GetBusinessUnitList(int ClientUserRowid, int BURowId = 0);

        IEnumerable<GetEduEMPVerificationHistoryViewModel> GetEducationVerificationHistories(string AuditType = "", int PersonalRowID = 0, int SubCheckRowID = 0);
        IEnumerable<GetEduEMPVerificationHistoryViewModel> GetEmploymentVerificationHistories(string AuditType = "", int PersonalRowID = 0, int SubCheckRowID = 0);
        IEnumerable<GetEduEMPVerificationHistoryViewModel> StopAutomation(int PersonalRowID = 0, short subCheckId = 0, int CheckStatusID = 0, string AuditType = "");

        IEnumerable<GetEduEMPVerificationHistoryViewModel> StopAutomationResume(int PersonalRowID = 0, int subCheckId = 0, string AuditType = "");
        IEnumerable<CollegeDDLListViewModel> GetCollegeByUniversityForDDL(short UniversityRowID);
        IEnumerable<PQVerifiedUploadDocViewModel> BindEducationPartnerList(int PersonalRowID = 0, short subCheckId = 0);

    }
}
