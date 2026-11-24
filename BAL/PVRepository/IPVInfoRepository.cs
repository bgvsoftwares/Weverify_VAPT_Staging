using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.PVViewModels;
using ViewModels.VerificationViewModel;

namespace BAL.PVRepository
{
    public interface IPVInfoRepository
    {
        UpdatePVInsuffClearViewModel GetPVEmploymentDetailsById(int EmploymentRowID = 0);

        void UpdatePVCriminalInfoByTeamLeader(PQUpdatePVTeamLeaderViewModel model);
        void UpdatePVEducationInfoByTeamLeader(PQUpdatePVTeamLeaderViewModel model);
        void UpdatePVEmploymentInfoByTeamLeader(PQUpdatePVTeamLeaderViewModel model);
        void UpdatePTRPVEmploymentInfoByTeamLeader(UpdatePVPartnerTeamMemberViewModel model);
        void UpdatePTRPVEducationInfoByTeamLeader(UpdatePVPartnerTeamMemberViewModel model);
        void UpdatePTRPVCriminalInfoByTeamLeader(UpdatePVPartnerTeamMemberViewModel model);

        void AddEmploymentByPVTeamMember(PQEmploymentVerViewModel model);
        void UpdateEmploymentStatusByPVTM(int EmploymentRowId, string CheckStatus, DateTime UpdatedDate);
        void AddEducationByPVTeamMember(PQEducationVerViewModel model);
        void UpdateEducationStatusByPVTM(int EducationRowId, string CheckStatus, DateTime UpdatedDate);
        void AddCriminalByPVTeamMember(PQCriminalVerViewModel model);
        void UpdateCriminalStatusByPVTM(int CriminalRowId, string CheckStatus, DateTime UpdatedDate);

        bool IsClientDepartPVTeamMembersExist(short CId = 0, short tmid = 0, string DepartmentName = "", string DesignationName = "");
        bool IsClientDepartPVTeamMembersExist(short CId = 0, short tmid = 0, short tmQCid = 0, string DepartmentName = "", string TMDesiName = "", string QCDesiName = "");

        //***************** For Employment ***********************
        VerificationInfoListPagedModel GetCandidatePVEmploymentListByDepartmentManager(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte AllocatedToTL = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", string EmployerName = "", string Allocatedto = "");

        VerificationInfoListPagedModel GetCandidateEmploymentListForPVTMQCAllocationByTL(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0);

        VerificationInfoListPagedModel GetCandidatePVEmploymentListByDepartmentTeamLeader(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte dtm = 0, byte dtmqc = 0, short AllocateToTM = 0, short AllocateToTMQC = 0, byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", string EmployerName = "", string Allocatedto = "");

        VerificationInfoListPagedModel GetCandidatePVEmploymentListForPTRMgrAllocationByTM(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short StateId = 0, string CheckStatus = "", byte tatdays = 0);

        VerificationInfoListPagedModel GetCandidatePVEmploymentListForTMM(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", string EmployerName = "", string Allocatedto = "");

        VerificationInfoListPagedModel GetCandidatePVEmploymentListForTMMQC(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", string EmployerName = "", string Allocatedto = "");

        //***************** For Education ***********************
        VerificationInfoListPagedModel GetCandidatePVEducationListByDepartmentManager(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte AllocatedToTL = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", string InstituteName = "", string Allocatedto = "");

        VerificationInfoListPagedModel GetCandidatePVEducationListByDepartmentTeamLeader(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte dtm = 0, byte dtmqc = 0, short AllocateToTM = 0, short AllocateToTMQC = 0, byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", string InstituteName = "", string Allocatedto = "");

        VerificationInfoListPagedModel GetCandidatePVEducationListForPTRMgrAllocationByTM(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short StateId = 0, string CheckStatus = "", byte tatdays = 0);

        VerificationInfoListPagedModel GetCandidatePVEducationListForTMM(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", string InstituteName = "", string Allocatedto = "");

        VerificationInfoListPagedModel GetCandidatePVEducationListForTMMQC(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", string InstituteName = "", string Allocatedto = "");

        //***************** For Criminal ***********************
        VerificationInfoListPagedModel GetCandidatePVCriminalListByDepartmentManager(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte AllocatedToTL = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", short State = 0, short City = 0, short Partner = 0);

        VerificationInfoListPagedModel GetCandidatePVCriminalListByDepartmentTeamLeader(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte dtm = 0, byte dtmqc = 0, short AllocateToTM = 0, short AllocateToTMQC = 0, byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", short State = 0, short City = 0, short Partner = 0);

        VerificationInfoListPagedModel GetCandidateEmploymentListForPVTLAllocationByMgr(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0);
        void UpdatePVEmploymentInfoByManager(UpdatePVManagerViewModel model);

        UpdatePVInsuffClearViewModel GetPVEducationDetailsById(int CriminalRowID = 0);

        VerificationInfoListPagedModel GetCandidateEducationListForPVTLAllocationByMgr(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0);
        void UpdatePVEducationInfoByManager(UpdatePVManagerViewModel model);

        UpdatePVInsuffClearViewModel GetPVCriminalDetailsById(int CriminalRowID = 0);

        VerificationInfoListPagedModel GetCandidateCriminalListForPVTLAllocationByMgr(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0);

        void UpdatePVCriminalInfoByManager(UpdatePVManagerViewModel model);

        VerificationInfoListPagedModel GetCandidateEducationListForPVTMQCAllocationByTL(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "",byte tatdays = 0);

        VerificationInfoListPagedModel GetCandidateCriminalListForPVTMQCAllocationByTL(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0);

        VerificationInfoListPagedModel GetCandidatePVCriminalListForPTRMgrAllocationByTM(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short StateId = 0, string CheckStatus = "", byte tatdays = 0);

        VerificationInfoListPagedModel GetCandidatePVCriminalListForTMM(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "");

        VerificationInfoListPagedModel GetCandidatePVCriminalListForTMMQC(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "");

        // Excel export

        VerificationInfoListPagedModel GetExportCandidateCriminalListByDepartmentManager(string Designation = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte AllocatedToTL = 0, byte tatdays = 0, string DCheckStatus = "", string CaseStatus = "", string ReAllocation = "");

        VerificationInfoListPagedModel GetExportCandidateEducationListByDepartmentManager(string Designation = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte AllocatedToTL = 0, byte tatdays = 0, string DCheckStatus = "", string CaseStatus = "");

        VerificationInfoListPagedModel GetExportCandidateEmployeeListByDepartmentManager(string Designation = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte AllocatedToTL = 0, byte tatdays = 0, string DCheckStatus = "", string CaseStatus = "");

        VerificationInfoListPagedModel GetCandidateEmploymentListForPVTLAllocationByMgr1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0, string statusid = "");
        VerificationInfoListPagedModel GetCandidatePVEmploymentListByDepartmentManager1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte AllocatedToTL = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", string EmployerName = "", string Allocatedto = "", string statusid = "");
        VerificationInfoListPagedModel GetCandidateEmploymentListForPVTMQCAllocationByTL1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0, string statusid = "");
        VerificationInfoListPagedModel GetCandidatePVEmploymentListByDepartmentTeamLeader1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte dtm = 0, byte dtmqc = 0, short AllocateToTM = 0, short AllocateToTMQC = 0, byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", string EmployerName = "", string Allocatedto = "", string Statusid = "");
        VerificationInfoListPagedModel GetCandidatePVEmploymentListForTMM1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", string EmployerName = "", string Allocatedto = "", string Statusid = "");
        VerificationInfoListPagedModel GetCandidatePVEmploymentListForTMMQC1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", string EmployerName = "", string Allocatedto = "", string Statusid = "");
        VerificationInfoListPagedModel GetCandidateEducationListForPVTLAllocationByMgr1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0, string statusid = "");
        VerificationInfoListPagedModel GetCandidateEducationListForPVTMQCAllocationByTL1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0, string statusid = "");
        VerificationInfoListPagedModel GetCandidatePVEducationListByDepartmentTeamLeader1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte dtm = 0, byte dtmqc = 0, short AllocateToTM = 0, short AllocateToTMQC = 0, byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", string InstituteName = "", string Allocatedto = "", string Statusid = "");
        VerificationInfoListPagedModel GetCandidateCriminalListForPVTLAllocationByMgr1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0, string statusid = "");
        VerificationInfoListPagedModel GetCandidatePVEducationListByDepartmentManager1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte AllocatedToTL = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", string InstituteName = "", string Allocatedto = "", string statusid = "");
        VerificationInfoListPagedModel GetCandidatePVCriminalListByDepartmentManager1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte AllocatedToTL = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", short State = 0, short City = 0, short Partner = 0, string statusid = "");
        VerificationInfoListPagedModel GetCandidateCriminalListForPVTMQCAllocationByTL1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0, string Statusid = "");
        VerificationInfoListPagedModel GetCandidatePVCriminalListByDepartmentTeamLeader1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte dtm = 0, byte dtmqc = 0, short AllocateToTM = 0, short AllocateToTMQC = 0, byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", short State = 0, short City = 0, short Partner = 0, string Statusid = "");
        VerificationInfoListPagedModel GetCandidatePVEducationListForTMM1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", string InstituteName = "", string Allocatedto = "", string Statusid = "");
        VerificationInfoListPagedModel GetCandidatePVCriminalListForTMM1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", string Statusid = "");
        VerificationInfoListPagedModel GetCandidatePVEducationListForTMMQC1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", string InstituteName = "", string Allocatedto = "", string Statusid = "");
        VerificationInfoListPagedModel GetCandidatePVCriminalListForTMMQC1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short teamMemerRowId = 0, short ClientRowID = 0, string CheckStatus = "", byte tatdays = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", string DCheckStatus = "", string CaseStatus = "", string Statusid = "");
    }
}
