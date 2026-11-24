using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.ProvidedInfoViewModel
{
    public class DashboardManagerDEViewModel
    {
        public int totRefereceGenByTL { get; set; }
        public int totAllocated { get; set; }
        public int totUnAllocated { get; set; }
        public int totPriority { get; set; }
        public int totRejectionWIP { get; set; }
        public int totReOpen { get; set; }

        public int totWithin1Hr { get; set; }
        public int totBetween2To4Hrs { get; set; }
        public int totBeyond4Hrs { get; set; }
        public int totDelayBy2Hrs { get; set; }
        public int totDelayBy2To5Hrs { get; set; }
        public int totDelayBeyond5Hrs { get; set; }

        public int totInsufficiency { get; set; }
        public int totResearch { get; set; }
        public int totDEScanPending { get; set; }
        public int totDEPending { get; set; }
        public int totQCPending { get; set; }
        public int totClosed { get; set; }
        public int totInsuffResolved { get; set; }

        public string CaseStatus { get; set; }
    }

    public class DashboardDataEntryTLViewModel
    {
        public int totRefereceGenByTL { get; set; }
        public int totAllocated { get; set; }
        public int totUnAllocated { get; set; }
        public int totPriority { get; set; }
        public int totRejectionWIP { get; set; }
        public int totReOpen { get; set; }

        public int totWithin1Hr { get; set; }
        public int totBetween2To4Hrs { get; set; }
        public int totBeyond4Hrs { get; set; }
        public int totDelayBy2Hrs { get; set; }
        public int totDelayBy2To5Hrs { get; set; }
        public int totDelayBeyond5Hrs { get; set; }

        public int totInsufficiency { get; set; }
        public int totResearch { get; set; }
        public int totDEScanPending { get; set; }
        public int totDEPending { get; set; }
        public int totQCPending { get; set; }
        public int totClosed { get; set; }

        public int totInsuffResolved { get; set; }
        public DashboardInnerNew DashboardInnerNew;


    }

    public class DashboardInnerNew
    {
        public int totRefereceGenByTL1 { get; set; }
        public int totAllocated1 { get; set; }
        public int ResultAwaitedRaised { get; set; }
        public int ResultAwaitedResolved { get; set; }
        public int totUnAllocated1 { get; set; }
        public int totResearch1 { get; set; }
        public int totClosed1 { get; set; }
        public int totInsufficiency1 { get; set; }
        public int totQCPending1 { get; set; }

        public int L1ResearchCleared { get; set; }
        public int totDETMPending1 { get; set; }
        public int totDEPending1 { get; set; }
        public int totDEScanPending1 { get; set; }
        public int totPending1 { get; set; }
        public int totReceived1 { get; set; }
        public int totRejectionWIP1 { get; set; }
        public int totScanningHold { get; set; }
        public int TotAddCheck { get; set; }       
        public int totYTR1 { get; set; }
        public int InsufficencyResolved1 { get; set; }
        public int InsufficencyRaised1 { get; set; }
        public int YTRResolved1 { get; set; }
        public int RejectedDETM { get; set; }
        public int RejectedDEQC { get; set; }
        public int InsuffClear { get; set; }
    }

        public class DashboardDETeamMemberDEScanViewModel
    {
        public int totReceived { get; set; }
        public int totPending { get; set; }
        public int totRejectionWIP { get; set; }
        public int totReOpenWIP { get; set; }
        public int totPriority { get; set; }
        public int totClosed { get; set; }

        public int totWithin1Hr { get; set; }
        public int totBetween2To4Hrs { get; set; }
        public int totBeyond4Hrs { get; set; }
        public int totDelayBy2Hrs { get; set; }
        public int totDelayBy2To5Hrs { get; set; }
        public int totDelayBeyond5Hrs { get; set; }
        public DashboardInnerNew DashboardInnerNew;
    }

    public class DashboardDETeamMemberDEViewModel
    {
        public int totReceived { get; set; }
        public int totPending { get; set; }
        public int totRejectionWIP { get; set; }
        public int totReOpenWIP { get; set; }
        public int totPriority { get; set; }
        public int totClosed { get; set; }

        public int totWithin1Hr { get; set; }
        public int totBetween2To4Hrs { get; set; }
        public int totBeyond4Hrs { get; set; }
        public int totDelayBy2Hrs { get; set; }
        public int totDelayBy2To5Hrs { get; set; }
        public int totDelayBeyond5Hrs { get; set; }

        public int totInsuffRaised { get; set; }
        public int totInsuffResolved { get; set; }
        public int totResearch { get; set; }
        public int totYTR { get; set; }
        public int totDEPending { get; set; }
        public DashboardInnerNew DashboardInnerNew;
    }

    public class DashboardDETeamMemberDEQCViewModel
    {
        public int totReceived { get; set; }
        public int totPending { get; set; }
        public int totRejectionWIP { get; set; }
        public int totReOpenWIP { get; set; }
        public int totPriority { get; set; }
        public int totClosed { get; set; }

        
        public int totWithin1Hr { get; set; }
        public int totBetween2To4Hrs { get; set; }
        public int totBeyond4Hrs { get; set; }
        public int totDelayBy2Hrs { get; set; }
        public int totDelayBy2To5Hrs { get; set; }
        public int totDelayBeyond5Hrs { get; set; }

        public int totInsuffRaised { get; set; }
        public int totInsuffResolved { get; set; }
        public int totResearch { get; set; }
        public int totYTR { get; set; }
        public int totDEPending { get; set; }
        public DashboardInnerNew DashboardInnerNew;
    }

    public class DashboardDataEntryViewModel
    {
        public int PersonalRowID { get; set; }
        public string CandidateName { get; set; }

        public string IsSeniorCase { get; set; }


        public string SubCheckName { get; set; }

        public short ClientRowID { get; set; }
        public string ClientName { get; set; }

        public string CompanyRefNo { get; set; }
        public string CandidateCode { get; set; }

        public short CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? OutDate { get; set; }

        public string CaseStatus { get; set; }
        public string DEName { get; set; }
        public DateTime? AllocatedToDEDate { get; set; }
       public string AllocatedToDEQC { get; set; }
        public DateTime? AllocatedToDEQCDate { get; set; }
        public string AllocatedToDEScan { get; set; }
        public DateTime? AllocatedToDEScanDate { get; set; }
        public DateTime? YTRTagDate { get; set; }
        public DateTime? YTRResolvedDate { get; set; }
        public DateTime? RejectedDETMDate { get; set; }
        public DateTime? RejectedDEQCDate { get; set; }
        public DateTime? InsuffClearDate { get; set; }
        public string ClientRefId { get; set; }
        public string CreatedBySource { get; set; }

    }

    public class DashboardDEListViewModel
    {
        public IEnumerable<DashboardDataEntryViewModel> DashboardDetailsDE { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public PQPersonalSearchPagedModel ItemSearch { get; set; }
    }
    
    public class ExportExcelDEViewModel
    {
        public int PersonalRowID { get; set; }
        public string ClientName  { get; set; }
        public string SecuritasID  { get; set; }
        public string CandidateName  { get; set; }
        public DateTime? CreatedDate  { get; set; }
        public string Status { get; set; }

    }

    public class ExportExcelAlocationDEViewModel
    {
        public int PersonalRowID { get; set; }
        public string ClientName { get; set; }
        public string SecuritasID { get; set; }
        public string CandidateName { get; set; }      
        public string AllocatedScanner { get; set; }
        public string AllocatedDE { get; set; }
        public string AllocatedQC { get; set; }
        public string Status { get; set; }

    }

    public class DashboardDEExportViewModel
    {
        public int PersonalRowID { get; set; }
        public string CandidateName { get; set; }

        public short ClientRowID { get; set; }
        public string ClientName { get; set; }

        public string CompanyRefNo { get; set; }
        public string CandidateCode { get; set; }

        public short CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? OutDate { get; set; }
        public string SubCheckName { get; set; }
        public string CaseStatus { get; set; }
    }

    public class DashboardExportDEListViewModel
    {
        public IEnumerable<DashboardDEExportViewModel> ExportExDetailsDE { get; set; }
       
    }

    public class ExportExcelDEYTRViewModel
    {
        public int PersonalRowID { get; set; }
        public string ClientName { get; set; }
        public string SecuritasID { get; set; }
        public string CandidateName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CheckName { get; set; }
        public string Status { get; set; }

    }

    public class ExportExcelREAllocationByDEViewModel
    {
        public int PersonalRowID { get; set; }
        public string ClientName { get; set; }
        public string SecuritasID { get; set; }
        public string CandidateName { get; set; }
        public string AllocatedScanner { get; set; }
        public string AllocatedDE { get; set; }
        public string AllocatedDEQC { get; set; }
        public DateTime? CreatedDate { get; set; }     
        
    }

    public class ExportExcelDEQCViewModel
    {
        public int PersonalRowID { get; set; }
        public string ClientName { get; set; }
        public string SecuritasID { get; set; }
        public string CandidateName { get; set; }
        public string AllocatedScanner { get; set; }
        public string AllocatedDE { get; set; }
        public string Status { get; set; }

    }

    public class ExportExcelDEScannerViewModel
    {
        public int PersonalRowID { get; set; }
        public string ClientName { get; set; }
        public string SecuritasID { get; set; }
        public string CandidateName { get; set; }
        public DateTime? AllocatedOn { get; set; }
        public string UploadedByClient { get; set; }

    }
}
