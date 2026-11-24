using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.PVViewModels
{
    class PVEmploymentViewModel
    {
        [ScaffoldColumn(false)]
        [Display(Name = "ID")]
        public int EmploymentRowID { get; set; }

        public short ClientRowID { get; set; }
        public string ClientName { get; set; }

        public int PersonalRowID { get; set; }
        public string CandidateName { get; set; }

        public short SubCheckRowID { get; set; }
        public string SubCheckName { get; set; }

        public string CompanyRefNo { get; set; }

        public short CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string CheckStatus { get; set; }
        public string CaseStatus { get; set; }
    }

    public class UpdatePVManagerViewModel
    {
        [ScaffoldColumn(false)]
        public int PVCheckRowID { get; set; }

        public short ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public short PVTLAllocatedBy { get; set; }
        public short PVTLAllocatedTo { get; set; }
        public DateTime? PVTLAllocatedDate { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "Check Status : ")]
        public string CheckStatus { get; set; }

    }

    public class PQUpdatePVTeamLeaderViewModel
    {
        [ScaffoldColumn(false)]
        public int PVCheckRowID { get; set; }

        public short ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        //public short TMAllocatedBy { get; set; }
        public short PVTMAllocatedTo { get; set; }
        public DateTime? PVTMAllocatedDate { get; set; }
        //public short TMQCAllocatedBy { get; set; }
        public short PVTMQCAllocatedTo { get; set; }
        public DateTime? PVTMQCAllocatedDate { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "Check Status : ")]
        public string CheckStatus { get; set; }

    }

    public class UpdatePVPartnerTeamMemberViewModel
    {
        [ScaffoldColumn(false)]
        public int PVCheckRowID { get; set; }

        public short ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public string PTRMgrCheckStatus { get; set; }
        public short PTRMgrAllocatedTo { get; set; }
        public DateTime? PTRMgrAllocatedDate { get; set; }

        [ScaffoldColumn(false)]
        [Display(Name = "Check Status : ")]
        public string CheckStatus { get; set; }

    }

    public class UpdatePVInsuffClearViewModel
    {
        public int CheckRowID { get; set; }
        public short ClientRowID { get; set; }
        public int PersonalRowID { get; set; }
        public short ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }

        [Display(Name = "Check Status : ")]
        public string CheckStatus { get; set; }
        public string ReWorkCheckStatus { get; set; }
        public short SubCheckRowID { get; set; }
        public string UniqueComponentID { get; set; }
        public short InfSuffRaiseBy { get; set; }
        public DateTime? InfSuffRaisedDate { get; set; }
        public short InfSuffClearBy { get; set; }
        public DateTime? InfSuffClearDate { get; set; }

        [Display(Name = "Insuff Remarks : ")]
        public string INFRemarks { get; set; }

        //public IEnumerable<CheckActionHistoryViewModel> CheckActionHistories { get; set; }
    }
}
