using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.ClientViewModel;

namespace BAL.ClientRepository
{
    public interface IClientDashboardRepository
    {
        ClientDashboardViewModel GetClientDashBoardInfo(short ClientRowId, int ClientUserRowID = 0);
        DashboardClientCandidateListViewModel GetClientDashBoardInfo(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string CaseStatus = "", short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", byte tatHours = 0, int ClientUserRowID = 0);
        DashboardClientCandidateListViewModel GetClientTempDashBoardInfo(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string CaseStatus = "", short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", int ClientUserRowID = 0);

        ClientDashboardCaseSummaryViewModel GetClientDashBoardCasewiseSummary(int ClientRowId, int ClientUserRowID = 0);
        List<ClientDashboardCaseSummaryDetailViewModel> GetClientDashBoardCasewiseSummaryDetail(int ClientRowId, int ClientUserRowID = 0, string search = "", string recdDateFrom = "", string recdDateTo = "", string compDateFrom = "", string compDateTo = "", string CaseStatus = "");

        ClientDashboardCheckSummaryViewModel GetClientDashBoardCheckwiseSummary(int ClientRowId, int ClientUserRowID = 0);
        List<ClientDashboardCheckSummaryDetailViewModel> GetClientDashBoardCheckwiseSummaryDetail(int ClientRowId, int ClientUserRowID = 0, string search = "", string recdDateFrom = "", string recdDateTo = "", string compDateFrom = "", string compDateTo = "", string CaseStatus = "");
        ClientDashboardViewModel GetClientDashBoardInfo1(short ClientRowId);     
        ClientDashboardViewModel GetClientDashBoardInfo1(short ClientRowId,int clientUserRowID,int allclient=0);
        DashboardClientCandidateListViewModel GetClientDashBoardInfo1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string CaseStatus = "", short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", byte tatHours = 0, int ClientUserRowID = 0,int allclient=0);
        DashboardClientCandidateListViewModel GetClientTempDashBoardInfo1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string CaseStatus = "", short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", int ClientUserRowID = 0,int allclient=0);
        List<ClientDashboardTrackerMISViewModel> GetClientDashBoardTrackerDetail(int ClientRowId, int clientUserRowID,string datefrom="",string dateto="", int allClients = 0);
        List<DashboardChart> ClientDashboardChart(int ClientRowid,string Year);
        List<ColorCodeGraph> ColorCodeGraph(int ClientRowid,string YEAR);
        List<ColorCodeGraph> CheckFamilyWiseGraph(int ClientRowid);
        List<ColorCodeGraph> TrendCheck(int ClientRowid,string Year);

        ClientDashboardViewModel GetClientDashBoardChartInfo1(short ClientRowId, int clientUserRowID, int allclient = 0);

        List<DashboardChart> Dashboard_Trend_TableData(int ClientRowid, string Year);
        List<ColorCodeGraph> ColorCodeGraph_table(int ClientRowid, string YEAR);
        List<DashboardChart> costingGraph_client(int ClientRowid, string YEAR);
        List<DashboardChart> costingGraph_client_table(int ClientRowid, string YEAR);
        List<ColorCodeGraph> CheckFamilWiseGraphNew_table(int ClientRowid, string Year);
        List<ColorCodeGraph> CheckFamilWiseGraph_table(int ClientRowid);
        List<ColorCodeGraph> CventDepartmentGraph(string colorCode);
        List<ColorCodeGraph> CventDepartmentGraph_table(string colorCode);
        List<ColorCodeGraph> CventBandGraph(string colorCode);
        List<ColorCodeGraph> CventBandGraph_table(string colorCode);
        List<ColorCodeGraph> CventRecuritersGraph(string colorCode);
        List<ColorCodeGraph> CventRecuritersGraph_table(string colorCode);
        string downloadReportFileName(string refno);
        string downloadReportFileName(string refno, string ReportType);
        string downloadReportFileName_Excl(string refno, string ReportUploadName);
        ClientDashboardTrackerMISViewModel getClientCaseWiseAllOpenClosedTracker(int pageNo, int pageSize, string sort, string sortDir, int ClientRowId, int ClientUserRowId, string Casestatus = "", int allClients = 0,string processstatus="");
        ClientDashboardTrackerMISViewModel GetClientDashBoardTrackerDetail(int pageNo, int pageSize, string sort, string sortDir, int ClientRowId, int ClientUserRowId, string datefrom = "", string dateto = "", int allClients = 0, string processstatus = "");

        DashboardClientCandidateListViewModel GetClientDashBoardInfoNonGreenCheck(int pageNo, int pageSize, string sort, string sortDir, string Search = "", string CaseStatus = "", short ClientRowID = 0, string RecievingToDate = "", string RecievingFromDate = "", string CompletedToDate = "", string CompletedFromDate = "", byte tatHours = 0, int ClientUserRowID = 0, int allclints = 0);
        ClientDashboardViewModel GetClientDashBoardChartInfo1DBGraph(short ClientRowId, int clientUserRowID, int allclient = 0, string stYear = null, string stMonth = null);

    }
}
