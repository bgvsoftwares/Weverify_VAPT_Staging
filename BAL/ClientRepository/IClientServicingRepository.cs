using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.ClientViewModel;
using ViewModels.ProvidedInfoViewModel;

namespace BAL.ClientRepository
{
    public interface IClientServicingRepository
    {
        DashboardManagerClientServicingViewModel GetManagerClientServicingDashBoardInfo_MultipleClient(short TeamMemberRowId, DataTable ClientRowID, string SpocEmailID);
        DashboardManagerClientServicingViewModel GetManagerClientServicingDashBoardInfo(short TeamMemberRowId, short ClientRowID, string SpocEmailID, string Department);
        
        DashboardClientCandidateListViewModel GetClientServicingDashBoardInfo(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string CaseStatus = "", short ClientRowID = 0, string RecievingDate = "", string CompletedDate = "", byte tatdays = 0, string SpocEmailID = "");

        DashboardClientCandidateListViewModel GetClientServicingDashBoardTempPQPersonalInfo(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string CaseStatus = "", short ClientRowID = 0, string RecievingDate = "", string SpocEmailID = "");
        ClientServiceListViewModel getClientCheckDetails(int pageNo, int pageSize, string companyRefNo);



        DashboardClientCandidateListViewModel GetClientServicingDashBoardInfo1(int pageNo, int pageSize, string Search = "", int teamMemberRowID = 0,  string CaseStatus = "", short ClientRowID = 0 , bool fetchDataCheckbox=false);
        DashboardClientCandidateListViewModel GetSpocReviewDashBoardInfo(int pageNo, int pageSize, string Search = "", int teamMemberRowID = 0, int CheckStatusId = 0, short ClientRowID = 0);
        DashboardClientCandidateListViewModel GetSpocReviewDashBoardInfoAPI(int pageNo, int pageSize, string Search = "", int teamMemberRowID = 0, string CheckStatusId = "0", short ClientRowID = 0);

        DashboardClientCandidateListViewModel GetClientServicingDashBoardInfo_Aging(int pageNo, int pageSize, string Search = "", int teamMemberRowID = 0, string CaseStatus = "", short ClientRowID = 0);
        DashboardClientCandidateListViewModel GetClientServicingDashBoardInfoFundApprovespoc(int pageNo, int pageSize, string Search = "", int teamMemberRowID = 0, string CaseStatus = "", short ClientRowID = 0);
        ClientServiceListViewModel GetFakeUniversityList(int pageNo, int pageSize);
        ClientServiceListViewModel GetFakeCompaniesList(int pageNo, int pageSize);
        DashboardClientCandidateListViewModel GetClientServicingDashBoardInfoInvestigation(int pageNo, int pageSize, string Search = "", int teamMemberRowID = 0, string CaseStatus = "", short ClientRowID = 0);
        DashboardClientCandidateListViewModel GetClientServicingDashBoardInfoColorcodeReview(int pageNo, int pageSize, string Search = "", int teamMemberRowID = 0, string CaseStatus = "", short ClientRowID = 0);       
        ClientServiceListViewModel GetReverificationApproval(int pageNo, int pageSize);
        ClientServiceListViewModel GetClientInfo(int pageNo, int pageSize, string Search);
        IEnumerable<PQCandidateCheckViewModel> GetInsuffCheck(int PersonalRowID = 0);
        DashboardClientCandidateListViewModel GetPendingCaseDashBoardInfo1(int pageNo, int pageSize, string Search = "", int teamMemberRowID = 0, string CaseStatus = "", short ClientRowID = 0, bool fetchDataCheckbox = false);
        DashboardClientCandidateListViewModel GetPendingCaseDashBoardInfoFundApprovespoc(int pageNo, int pageSize, string Search = "", int teamMemberRowID = 0, string CaseStatus = "", short ClientRowID = 0);
        DashboardClientCandidateListViewModel GetMISDeptWiseCount(int pageNo, int pageSize, string Search = "", int teamMemberRowID = 0, string CaseStatus = "", short ClientRowID = 0, string FromDate = "", string ToDate = "");
        DashboardClientCandidateListViewModel GetAnnexureInvoicing(int pageNo, int pageSize, string Search = "", int teamMemberRowID = 0, string CaseStatus = "", short ClientRowID = 0, string FromDate = "", string ToDate = "", string ClientName = "", string BilingType = "");
        ClientCandidateViewModel SaveUpdateAnnexureInvoicing(ClientCandidateViewModel AI);
        ClientCandidateViewModel EditAnnexureInvoiceDetailsBasedonId(ClientCandidateViewModel modelAnnexureInvoicing);
        ClientCandidateViewModel GetAnnexureInvoiceDetailsBasedonId(int AnnexureInvoicingId = 0);
        string deleteAnnexureInvoiceDetailsBasedonId(int AnnexureInvoicingId = 0);
        string SnedMailAnnexureInvoiceDetailsBasedonId(int AnnexureInvoicingId = 0, int ReminderSendOption = 0);

        DashboardClientCandidateListViewModel GetClientServicingCheckTagInfo(int pageNo, int pageSize, string Search = "", short teamMemberRowID = 0, string CaseStatus = "", short ClientRowID = 0, bool fetchDataCheckbox = false);

        DashboardClientCandidateListViewModel GetClientServicingCheckTagHistory(int pageNo, int pageSize, string Search = "", short teamMemberRowID = 0, string CaseStatus = "", short ClientRowID = 0, bool fetchDataCheckbox = false);
        IEnumerable<ClientCandidateCheckTagExportViewModel> GetCheckTagForExport(string strExportType = "", short teamMemberRowID = 0);
        DashboardClientCandidateListViewModel GetDataEntryAging(int pageNo, int pageSize, string Search = "", int teamMemberRowID = 0, string CaseStatus = "", short ClientRowID = 0, string FromDate = "", string ToDate = "", string typeOfOption = "");


    }
}
