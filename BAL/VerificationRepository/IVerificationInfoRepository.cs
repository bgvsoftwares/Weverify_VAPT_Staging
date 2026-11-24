using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.ProvidedInfoViewModel;
using ViewModels.VerificationViewModel;

namespace BAL.VerificationRepository
{
    public interface IVerificationInfoRepository
    {

        #region ***** Addresss Verification Department *****

        //Address Manager

        VerificationInfoListPagedModel GetEducationAutoAllocatedChecks(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string designation = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", string EmployerName = "", string Allocatedto = "");
        VerificationInfoListPagedModel GetCandidateAddressListByDepartmentManager(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte AllocatedToTL = 0, byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", short State = 0, short City = 0, short Partner = 0);
        VerificationInfoListPagedModel GetCandidateAddressListForTLAllocationByMgr(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "");

        //Address TeamLeader

        VerificationInfoListPagedModel PVCVCheckDetailsForReAllocation(int pageNo, int pageSize, string Search = "", string department = "", string status = "", short teamMemerRowId = 0);
        VerificationInfoListPagedModel GetCandidateAddressListByDepartmentTeamLeader(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte dtm = 0, byte dtmqc = 0, short AllocateToTM = 0, short AllocateToTMQC = 0, byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", short State = 0, short City = 0, short Partner = 0);

        VerificationInfoListPagedModel GetCandidateAddressListForTMQCAllocationByTL(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short StateRowId = 0, string CheckStatus = "", string ReAllocation = "", short AllocateTM = 0, short AllocateTMQC = 0);
        VerificationInfoListPagedModel GetCandidateAddressListForPTRMgrAllocationByTM(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "");

        //Address TeamMember

        VerificationInfoListPagedModel GetCandidateAddressListForTMM(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", string EmployerName = "", string Allocatedto = "", short State = 0, short City = 0, short Partner = 0);

        VerificationInfoListPagedModel GetCandidateAddressListForTMMQC(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", string EmployerName = "", string Allocatedto = "", short State = 0, short City = 0, short Partner = 0);

        VerificationInfoListPagedModel GetCandidateAddressListByDepartmentTeamLeader1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, string department = "", string  statusid = "", short ClientRowID = 0, string CheckStatus = "", byte dtm = 0, byte dtmqc = 0, short AllocateToTM = 0, short AllocateToTMQC = 0, byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", short State = 0, short City = 0, short Partner = 0);

        VerificationInfoListPagedModel GetCandidateAddressListForTMQCAllocationByTL1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string department = "", string  statusid = "", short teamMemerRowId = 0, short stateRowId = 0, string CheckStatus = "", string ReAllocation = "", short AllocateTM = 0, short AllocateTMQC = 0);

        VerificationInfoListPagedModel GetCandidateAddressListForTMM1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string department = "", string  statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", string EmployerName = "", string Allocatedto = "", short State = 0, short City = 0, short Partner = 0);

        VerificationInfoListPagedModel GetCandidateAddressListForPTRMgrAllocationByTM1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string department = "", string statusid = "", short teamMemerRowId = 0, short StateId = 0, string CheckStatus = "", short LocationRowID = 0, string StateName = "");

        VerificationInfoListPagedModel GetCandidateAddressListForTMMQC1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string department = "", string statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", string EmployerName = "", string Allocatedto = "", short State = 0, short City = 0, short Partner = 0);

        VerificationInfoListPagedModel GetCandidateListForTMMQCNew(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string department = "", string statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", string EmployerName = "", string Allocatedto = "", short State = 0, short City = 0, short Partner = 0 , bool fetchDataCheckbox=false, string designation = "");

        VerificationInfoListPagedModel GetCandidateAddressListByDepartmentManager1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string department = "", string statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte AllocatedToTL = 0, byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", short State = 0, short City = 0, short Partner = 0);
        VerificationInfoListPagedModel GetCandidateAddressListForTLAllocationByMgr1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string department = "", string statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "");
        VerificationInfoListPagedModel GetAgingDetails(int pageNo, int pageSize, string Search = "", string department = "", string status = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", string SearchType = "", string sort = "", string sortDir = "");
        VerificationInfoListPagedModel GetAgingDetails_Action(int pageNo, int pageSize, string Search = "", string department = "", string status = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", string CheckStatusId = "");
        #endregion

        #region ***** Employment Verification Department *****

        //Employment Manager
        VerificationInfoListPagedModel GetCandidateEmploymentListByDepartmentManager(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte AllocatedToTL = 0, byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", string EmployerName = "", string Allocatedto = "");
        VerificationInfoListPagedModel GetEmploymentAutoAllocatedChecks(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string designation = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", string EmployerName = "", string Allocatedto = "");
        VerificationInfoListPagedModel GetCandidateEmploymentListForTLAllocationByMgr(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "");

        //Employment TeamLeader
        VerificationInfoListPagedModel GetCandidateEmploymentListByDepartmentTeamLeader(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte dtm = 0, byte dtmqc = 0, short AllocateToTM = 0, short AllocateToTMQC = 0, byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", string EmployerName = "", string Allocatedto = "");

        VerificationInfoListPagedModel GetCandidateEmploymentListForTMQCAllocationByTL(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", string ReAllocation = "", short AllocateTM = 0, short AllocateTMQC = 0);

        //Employment TeamMember
        VerificationInfoListPagedModel GetCandidateEmploymentListForTMM(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", string EmployerName = "", string Allocatedto = "");

        VerificationInfoListPagedModel GetCandidateEmploymentListForTMMQC(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", string EmployerName = "", string Allocatedto = "");
        VerificationInfoListPagedModel GetCandidateEmploymentListByDepartmentManager1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string department = "", string statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte AllocatedToTL = 0, byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", string EmployerName = "", string Allocatedto = "", string SearchType = "");
        VerificationInfoListPagedModel GetCandidateEmploymentListForTLAllocationByMgr1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string department = "", string statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "",string SearchType = "");
        VerificationInfoListPagedModel GetCandidateEmploymentListByDepartmentTeamLeader1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string department = "", string statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte dtm = 0, byte dtmqc = 0, short AllocateToTM = 0, short AllocateToTMQC = 0, byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", string EmployerName = "", string Allocatedto = "", string designation = "");
        #endregion

        #region ***** Education Verification Department *****

        //Education Manager
        VerificationInfoListPagedModel GetCandidateEducationListByDepartmentManager(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte AllocatedToTL = 0, byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", string InstituteName = "", string Allocatedto = "");

        VerificationInfoListPagedModel GetCandidateEducationListForTLAllocationByMgr(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "");

        //Education TeamLeader
        VerificationInfoListPagedModel GetCandidateEducationListByDepartmentTeamLeader(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte dtm = 0, byte dtmqc = 0, short AllocateToTM = 0, short AllocateToTMQC = 0, byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", string InstituteName = "", string Allocatedto = "");

        VerificationInfoListPagedModel GetCandidateEducationListForTMQCAllocationByTL(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", string ReAllocation = "", short AllocateTM = 0, short AllocateTMQC = 0);

        //Education TeamMember
        VerificationInfoListPagedModel GetCandidateEducationListForTMM(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", string InstituteName = "", string Allocatedto = "");

        VerificationInfoListPagedModel GetCandidateEducationListForTMMQC(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", string EmployerName = "", string Allocatedto = "");
        VerificationInfoListPagedModel GetCandidateEducationListByDepartmentManager1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string department = "", string statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte AllocatedToTL = 0, byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", string InstituteName = "", string Allocatedto = "", string CheckType = "", string SearchType = "");
        VerificationInfoListPagedModel GetCandidateEducationListForTLAllocationByMgr1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string department = "", string statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", string SearchType = "");
        VerificationInfoListPagedModel GetCandidateEducationListForTMQCAllocationByTL1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string department = "", string statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", string ReAllocation = "", short AllocateTM = 0, short AllocateTMQC = 0);
        VerificationInfoListPagedModel GetCandidateEducationListByDepartmentTeamLeader1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string department = "", string statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte dtm = 0, byte dtmqc = 0, short AllocateToTM = 0, short AllocateToTMQC = 0, byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", string InstituteName = "", string Allocatedto = "");
        VerificationInfoListPagedModel GetCandidateEmploymentListForTMQCAllocationByTL1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string department = "", string statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", string ReAllocation = "", short AllocateTM = 0, short AllocateTMQC = 0, string designation = "");
        VerificationInfoListPagedModel GetCandidateEducationListForTMMQC1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string department = "", string statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", string InstituteName = "", string Allocatedto = "");
        VerificationInfoListPagedModel GetCandidateEducationListForTMM1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string department = "", string statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", string InstituteName = "", string Allocatedto = "", string SearchType = "" , bool fetchDataCheckbox=false);
        VerificationInfoListPagedModel GetCandidateEmploymentListForTMM1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string department = "", string statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", string EmployerName = "", string Allocatedto = "", string SearchType = "" , bool fetchDataCheckbox = false, string designation = "");
        VerificationInfoListPagedModel CheckDetailsForReAllocation(int pageNo, int pageSize, string Search = "", string department = "", string status = "", short teamMemerRowId = 0);
        VerificationInfoListPagedModel GetCandidateEducationReverificationDetails(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string department = "", string statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckType = "", string designation = "");
        #endregion

        #region ***** Criminal Verification Department *****

        //Criminal Manager
        VerificationInfoListPagedModel GetCandidateCriminalListByDepartmentManager(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte AllocatedToTL = 0, byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", short State = 0, short City = 0, short Partner = 0);

        VerificationInfoListPagedModel GetCandidateCriminalListForTLAllocationByMgr(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "");

        //Criminal TeamLeader
        VerificationInfoListPagedModel GetCandidateCriminalListByDepartmentTeamLeader(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte dtm = 0, byte dtmqc = 0, short AllocateToTM = 0, short AllocateToTMQC = 0, byte tatdays = 0, string RecievingDate = "", string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string CaseStatus = "", short State = 0, short City = 0, short Partner = 0);

        VerificationInfoListPagedModel GetCandidateCriminalListForTMQCAllocationByTL(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", string ReAllocation = "", short AllocateTM = 0, short AllocateTMQC = 0);

        //Criminal TeamMember
        VerificationInfoListPagedModel GetCandidateCriminalListForTMM(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "");

        VerificationInfoListPagedModel GetCandidateCriminalListForTMMQC(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "");
        VerificationInfoListPagedModel GetCandidateCriminalListByDepartmentManager1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string department = "", string statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte AllocatedToTL = 0, byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", short State = 0, short City = 0, short Partner = 0);
        VerificationInfoListPagedModel GetCandidateCriminalListForTLAllocationByMgr1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string department = "", string statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "");
        VerificationInfoListPagedModel GetCandidateCriminalListForTMQCAllocationByTL1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string department = "", string statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", string ReAllocation = "", short AllocateTM = 0, short AllocateTMQC = 0);
        VerificationInfoListPagedModel GetCandidateCriminalListByDepartmentTeamLeader1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string department = "", string statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte dtm = 0, byte dtmqc = 0, short AllocateToTM = 0, short AllocateToTMQC = 0, byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", short State = 0, short City = 0, short Partner = 0);
        VerificationInfoListPagedModel GetCandidateCriminalListForTMM1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string department = "", string statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "");
        #endregion

        #region ***** Reference Verification Department *****

        //Reference Manager
        VerificationInfoListPagedModel GetCandidateReferenceListByDepartmentManager(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte AllocatedToTL = 0, byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "");

        VerificationInfoListPagedModel GetCandidateReferenceListForTLAllocationByMgr(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "");

        //Reference TeamLeader
        VerificationInfoListPagedModel GetCandidateReferenceListByDepartmentTeamLeader(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte dtm = 0, byte dtmqc = 0, short AllocateToTM = 0, short AllocateToTMQC = 0, byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "");

        VerificationInfoListPagedModel GetCandidateReferenceListForTMQCAllocationByTL(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", string ReAllocation = "", short AllocateTM = 0, short AllocateTMQC = 0);

        //Reference TeamMember
        VerificationInfoListPagedModel GetCandidateReferenceListForTMM(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "");

        VerificationInfoListPagedModel GetCandidateReferenceListForTMMQC(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "");
        VerificationInfoListPagedModel GetCandidateReferenceListByDepartmentManager1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string department = "", string statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte AllocatedToTL = 0, byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "");
        VerificationInfoListPagedModel GetCandidateReferenceListForTLAllocationByMgr1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string department = "", string statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "");
        VerificationInfoListPagedModel GetCandidateReferenceListForTMQCAllocationByTL1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string department = "", string statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", string ReAllocation = "", short AllocateTM = 0, short AllocateTMQC = 0);
        VerificationInfoListPagedModel GetCandidateReferenceListByDepartmentTeamLeader1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string department = "", string statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte dtm = 0, byte dtmqc = 0, short AllocateToTM = 0, short AllocateToTMQC = 0, byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "");
        VerificationInfoListPagedModel GetCandidateReferenceListForTMM1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string department = "", string statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "" , bool fetchDataCheckbox=false);
        #endregion

        #region ***** National Identity Verification Department *****

        //NationalIdentity Manager
        VerificationInfoListPagedModel GetCandidateIdentityListByDepartmentManager(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte AllocatedToTL = 0, byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "");

        VerificationInfoListPagedModel GetCandidateIdentityListForTLAllocationByMgr(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "");

        //NationalIdentity TeamLeader
        VerificationInfoListPagedModel GetCandidateIdentityListByDepartmentTeamLeader(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte dtm = 0, byte dtmqc = 0, short AllocateToTM = 0, short AllocateToTMQC = 0, byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "");

        VerificationInfoListPagedModel GetCandidateIdentityListForTMQCAllocationByTL(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", string ReAllocation = "", short AllocateTM = 0, short AllocateTMQC = 0);
        VerificationInfoListPagedModel GetCandidateIdentityListByDepartmentTeamLeader1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string department = "", string statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte dtm = 0, byte dtmqc = 0, short AllocateToTM = 0, short AllocateToTMQC = 0, byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "");
        //VerificationInfoListPagedModel GetCandidateIdentityListForTMQCAllocationByTL(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", short AllocateTM = 0, short AllocateTMQC = 0);

        //NationalIdentity TeamMember
        VerificationInfoListPagedModel GetCandidateIdentityListForTMM(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "");

        VerificationInfoListPagedModel GetCandidateIdentityListForTMMQC(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "");
        VerificationInfoListPagedModel GetCandidateIdentityListByDepartmentManager1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string department = "", string statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte AllocatedToTL = 0, byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "");
        VerificationInfoListPagedModel GetCandidateIdentityListForTLAllocationByMgr1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string department = "", string statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "");
        VerificationInfoListPagedModel GetCandidateIdentityListForTMQCAllocationByTL1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string department = "", string statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", string ReAllocation = "", short AllocateTM = 0, short AllocateTMQC = 0);
        VerificationInfoListPagedModel GetCandidateIdentityListForTMM1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string department = "", string statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "" , bool fetchDataCheckbox = false);
        #endregion

        #region ***** Special Check Verification Department *****

        //Special Check Manager
        VerificationInfoListPagedModel GetCandidateSpecialCheckListByManager(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte AllocatedToTL = 0, byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "");

        VerificationInfoListPagedModel GetCandidateSpecialCheckListForTLAllocationByMgr(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "");

        //Special Check TeamLeader
        VerificationInfoListPagedModel GetCandidateSpecialCheckListByTeamLeader(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte dtm = 0, byte dtmqc = 0, short AllocateToTM = 0, short AllocateToTMQC = 0, byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "");

        VerificationInfoListPagedModel GetCandidateSpecialCheckListForTMQCAllocationByTL(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", string ReAllocation = "", short AllocateTM = 0, short AllocateTMQC = 0);

        //Special Check TeamMember
        VerificationInfoListPagedModel GetCandidateSpecialCheckListForTMM(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "");

        VerificationInfoListPagedModel GetCandidateSpecialCheckListForTMMQC(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "");
        VerificationInfoListPagedModel GetCandidateSpecialCheckListByManager1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string department = "", string statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte AllocatedToTL = 0, byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "");
        VerificationInfoListPagedModel GetCandidateSpecialCheckListForTLAllocationByMgr1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string department="", string statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "");
        //VerificationInfoListPagedModel GetCandidateSpecialCheckListForTMQCAllocationByTL(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", string ReAllocation = "", short AllocateTM = 0, short AllocateTMQC = 0)
        VerificationInfoListPagedModel GetCandidateSpecialCheckListForTMQCAllocationByTL1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string department = "", string statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", string ReAllocation = "", short AllocateTM = 0, short AllocateTMQC = 0);
        VerificationInfoListPagedModel GetCandidateSpecialCheckListByTeamLeader1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string department = "", string statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte dtm = 0, byte dtmqc = 0, short AllocateToTM = 0, short AllocateToTMQC = 0, byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "");
        VerificationInfoListPagedModel GetCandidateSpecialCheckListForTMM1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string department = "", string statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "");
        void UpdateSpecialCheckRejectionByMgrOrTL(ViewSpecialCheckViewModel model, string designationName, short RWMgrID = 0);
        #endregion

        #region ***** Fund Pending Checks Update By Department TL *****

        void FundPendingChecksUpdateByDepartmentTL(FundPendingApprovalViewModel model);
        VerificationInfoListPagedModel GetCandidateAllFundsResolved(int pageNo, int pageSize, int UserId, string Search = "", string processname = "", string sort="");
        #endregion

        #region ***** Update RW Approval Checks By Manager And TeamLeader *****

        VerificationInfoListPagedModel GetRWApprovalCheckListForDepartmentTL(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short ClientRowID = 0, short TeamMemberID = 0);

        VerificationInfoListPagedModel GetRWApprovalCheckListForDepartmentManager(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short ClientRowID = 0, short TeamMemberID = 0);

        void UpdateAddressCheckRejectionByMgrOrTL(ViewAddressViewModel model, string designationName, short RWMgrID = 0);

        void UpdateEmploymentCheckRejectionByMgrOrTL(ViewEmploymentViewModel model, string designationName, short RWMgrID = 0);

        void UpdateEducationCheckRejectionByMgrOrTL(ViewEducationViewModel model, string designationName, short RWMgrID = 0);

        void UpdateCriminalCheckRejectionByMgrOrTL(ViewCriminalViewModel model, string designationName, short RWMgrID = 0);

        void UpdateReferenceCheckRejectionByMgrOrTL(ViewReferenceViewModel model, string designationName, short RWMgrID = 0);

        void UpdateNationalIdentityCheckRejectionByMgrOrTL(ViewNationalIdentityViewModel model, string designationName, short RWMgrID = 0);

        #endregion


        #region Excel Emport

        VerificationInfoListPagedModel GetExportCandidateAddressListByDepartmentManager(string Designation = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte AllocatedToTL = 0, byte tatdays = 0, string DCheckStatus = "", string CaseStatus = "");
        VerificationInfoListPagedModel GetExportCandidateCriminalListByDepartmentManager(string Designation = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte AllocatedToTL = 0, byte tatdays = 0, string DCheckStatus = "", string CaseStatus = "", string ReAllocation = "");
        VerificationInfoListPagedModel GetExportCandidateEducationListByDepartmentManager(string Designation = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte AllocatedToTL = 0, byte tatdays = 0, string DCheckStatus = "", string CaseStatus = "");
        VerificationInfoListPagedModel GetExportCandidateEmployeeListByDepartmentManager(string Designation = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte AllocatedToTL = 0, byte tatdays = 0, string DCheckStatus = "", string CaseStatus = "");
        VerificationInfoListPagedModel GetExportCandidateReferenceListByDepartmentManager(string Designation = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte AllocatedToTL = 0, byte tatdays = 0, string DCheckStatus = "", string CaseStatus = "");
        VerificationInfoListPagedModel GetExportCandidateNationalIdentitieListByDepartmentManager(string Designation = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte AllocatedToTL = 0, byte tatdays = 0, string DCheckStatus = "", string CaseStatus = "");
        IEnumerable<ExportVerificationAddressViewModel> GetExportAddressManager(int CheckFamilyRowId, string CheckStatusId, string department, int userid);
        IEnumerable<ExportVerificationSpecialViewModel> GetExportSpecial(int CheckFamilyRowId, string CheckStatusId, string department, int userid);
        IEnumerable<ExportVerificationNationalViewModel> GetExportNational(int CheckFamilyRowId, string CheckStatusId, string department, int userid);
        IEnumerable<ExportVerificationCriminalViewModel> GetExportCriminal(int CheckFamilyRowId, string CheckStatusId, string department, int userid);
        IEnumerable<ExportVerificationEduMgrViewModel> GetExportEducationManager(int CheckFamilyRowId, string CheckStatusId, string department, int userid);
        IEnumerable<AllocationonAddMgrViewModel> GetExportEmployeemant(int CheckFamilyRowId, string CheckStatusId, string department, int userid);
        IEnumerable<AllocationonAddMgrViewModel> GetExportReference(int CheckFamilyRowId, string CheckStatusId, string department, int userid);

        #endregion
        VerificationInfoListPagedModel getAutoRevertDetail(int pageNo, int pageSize, string Search = "", string department = "", string status = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", string CheckStatusId = "", int mailStatus = 0);
        VerificationInfoListPagedModel GetAutoCandidateEmploymentListByDepartmentTeamLeader1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string department = "", string statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte dtm = 0, byte dtmqc = 0, short AllocateToTM = 0, short AllocateToTMQC = 0, byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", string EmployerName = "", string Allocatedto = "");
        List<PQVerifiedUploadDocViewModel> GetAutoUploadMails(int PersonalRowID = 0, int subCheckId = 0);
        AutoEmploymentDetail GetAutoEmployementDetail(int EmployementRowId = 0);
        VerificationInfoListPagedModel GetAutoGroupMailDetail(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string department = "", string statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", string EmployerName = "", string Allocatedto = "");
        VerificationInfoListPagedModel GetAutoManualGroupDetails(string GroupId);
        VerificationInfoListPagedModel GetFakeCompaniesList(int page, int pageSize);

        VerificationInfoListPagedModel EmailAutomationCheckStatus(int pageNo, int pageSize, int status = 0, string Search = "", string fromdate = null, string todate = null, int? teamMemberRowID = null);
        VerificationInfoListPagedModel GetReverification(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string department = "", string statusid = "", short teamMemerRowId = 0, string Statename = "", string CheckStatus = "", short LocationRowID = 0);
        VerificationInfoListPagedModel GetCandidateEducationListForTLAllocation(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string department = "", string statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", string ReAllocation = "", string universityName = "", string collegeName = "", string CourseName = "", string YrOfPassing = "", short AllocateTM = 0, short AllocateTMQC = 0);

        VerificationInfoListPagedModel GetReallocationChecks_Operation(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", string ReAllocation = "", short AllocateTM = 0, short AllocateTMQC = 0, string department = "");
        VerificationInfoListPagedModel GetReallocationChecks_OperationForEmploymment(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", string ReAllocation = "", short AllocateTM = 0, short AllocateTMQC = 0, string department = "");
        VerificationInfoListPagedModel GetReallocationChecks_OperationForEmploymmentQC(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", string ReAllocation = "", short AllocateTM = 0, short AllocateTMQC = 0, string department = "",string AllocationStatus="");
        VerificationInfoListPagedModel getEducationAutoRevertDetail(int pageNo, int pageSize, string Search = "", string department = "", string status = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", string CheckStatusId = "", int mailstatus = 0);
        VerificationInfoListPagedModel GetdataAddressPartnerClose(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string department = "", string statusid = "", short teamMemerRowId = 0, short stateRowId = 0, string CheckStatus = "", string ReAllocation = "", short AllocateTM = 0, short AllocateTMQC = 0);
        WiproUANCheckViewModel GetWipro_UAN_Detail(WiproUANCheckViewModel model, int pageNo, int pageSize, string Search = "", string personalrowid = "");
        CheckVerificationViewModel GetCheckVerificationDeatils(string CompanyRefNo);
        StopCheckVerificationView GetCheckVerificationByCandCheckRowID(int CandCheckRowID);
        VerificationInfoListPagedModel GetOperationCriminalManagerUnallocated(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string department = "", string statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte AllocatedToTL = 0, byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", short State = 0, short City = 0, short Partner = 0);
        VerificationInfoListPagedModel GetOperationCriminalManagerAllocated(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string department = "", string statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte AllocatedToTL = 0, byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", short State = 0, short City = 0, short Partner = 0);
        VerificationInfoListPagedModel GetOperationCriminalTeamMember(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string department = "", string statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", short State = 0, short City = 0, short Partner = 0 , bool fetchDataCheckbox=false);

        VerificationInfoListPagedModel GetCandidateAddressListForAllocationByTL(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string department = "", string statusid = "", short teamMemerRowId = 0, int stateRowId = 0, string CheckStatus = "", string ReAllocation = "", short AllocateTM = 0, short AllocateTMQC = 0);
        VerificationInfoListPagedModel GetCandidateAddressListForPTRMgrAllocationByOPTM(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string department = "", string statusid = "", short teamMemerRowId = 0, short StateId = 0, string CheckStatus = "", short LocationRowID = 0, string StateName = "");
        VerificationInfoListPagedModel GetCandidateAddressListForAddressTMM(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string department = "", string statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", string EmployerName = "", string Allocatedto = "", short State = 0, short City = 0, short Partner = 0, bool fetchDataCheckbox = false , short StateRowId = 0, string Statename = "");
        VerificationInfoListPagedModel GetOperationCriminalTeamMemberQc(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string department = "", string statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "");
        VerificationInfoListPagedModel GetOperationSpecialTeamMemberQc(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string department = "", string statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "");
        VerificationInfoListPagedModel GetOperationSpecialTeamMember(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string department = "", string statusid = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "" , bool fetchDataCheckbox = false);
        List<SpocActionRemark> GetSpocActionRemark(int PersonalRowID, int SubCheckRowID);
        VerificationInfoListPagedModel GetWiproBulkEmailAuto(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", string CompletedToDate = "", string CompletedFromDate = "");
        VerificationInfoListPagedModel GetInfosysBulkEmailAuto(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", string CompletedToDate = "", string CompletedFromDate = "");
        VerificationInfoListPagedModel GetBulkEmailAuto(int tmid , int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", string CompletedToDate = "", string CompletedFromDate = "", string EmployerType = "");
        OnlineLinkSearchingPageModel GetOnlineLInkSearch(int pageNo, int pageSize, string sort, string sortDir);
        VerificationInfoListPagedModel GetUANEmpchecks(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string designation = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", string EmployerName = "", string Allocatedto = "");
        VerificationInfoListPagedModel getReferenceAutoRevertDetail(int pageNo, int pageSize, string Search = "", string department = "", string status = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", string CheckStatusId = "", int mailstatus = 0);

    }
}
