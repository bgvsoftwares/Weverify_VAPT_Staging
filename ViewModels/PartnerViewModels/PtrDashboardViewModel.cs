using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.PartnerViewModels
{
    public class PtrDashboardViewModel
    {
        public int OutTat { get; set; }
        public int totWIP { get; set; }
        public int totAllocated { get; set; }
        public int totNonAllocated { get; set; }
        public int totInitiated { get; set; }
        public int totNonInitiated { get; set; }
        public int totRejectionWIP { get; set; }
        public int totReVisit { get; set; }
        public int totPriority { get; set; }
        public int totReallocation { get; set; }

        public int totTodaysDue { get; set; }
        public int totDueinNext2days { get; set; }
        public int totInTAT4to7days { get; set; }
        public int totInTATGrater7days { get; set; }
        public int totBT1To2days { get; set; }
        public int totBT3To5days { get; set; }
        public int totBT5To10days { get; set; }
        public int totBT10daysPlus { get; set; }

        public int totInsuffWIP { get; set; }
        public int totQCPending { get; set; }
        public int totFundApproval { get; set; }
        public int totApprovalpending { get; set; }
        public int totEscalated { get; set; }
        public int totClosed { get; set; }
        public int totOutTAT { get; set; }
        public int totOrange { get; set; }

        public int totAllocated1 { get; set; }
        public int totNonAllocated1 { get; set; }
        public int totReVisit1 { get; set; }
        public int totRejectionWIP1 { get; set; }
        public int totClosed1 { get; set; }
        public int totFundApproval1 { get; set; }
        public int totApprovalpending1 { get; set; }
        public int FieldPartnerClosed { get; set; }


    }
    public class PartnerCandidateViewModel
    {
        public int PersonalRowID { get; set; }
        public string CandidateName { get; set; }

        public short ClientRowID { get; set; }
        public string ClientName { get; set; }

        public string CompanyRefNo { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? OutDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        
        public string CheckStatus { get; set; }

        [ScaffoldColumn(false)]
        public short CheckFamilyRowID { get; set; }
        public string CheckFamilyName { get; set; }

        [ScaffoldColumn(false)]
        public short SubCheckRowID { get; set; }
        public string SubCheckName { get; set; }

        [ScaffoldColumn(false)]
        public string UniqueComponentID { get; set; }

        public byte IsSynced { get; set; }

        public string  PartnerUserName { get; set; }
        public int PQAddressVarPtrRowId { get; set; }
    }

    public class DashboardPartnerCandidateListViewModel
    {
        public IEnumerable<PartnerCandidateViewModel> DashboardPartnerCandidates { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
    }
}
