using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.ProvidedInfoViewModel;

namespace ViewModels.VerificationViewModel
{   
    public class VerifyInfoForClientViewModel
    {
        public int PQAddressVarPtrRowId { get; set; }
     
        public int VerificationRowID { get; set; }
     
        public short ClientRowID { get; set; }
        public string ClientName { get; set; }
     
        public int PersonalRowID { get; set; }
        public string PersonalName { get; set; }

        public string CandidateCode { get; set; }
     
        public int ClientPackageRowID { get; set; }
        public string ClientPackageName { get; set; }
     
        public short CheckFamilyRowID { get; set; }
        public string CheckFamilyName { get; set; }
     
        public short SubCheckRowID { get; set; }
        public string SubCheckName { get; set; }
     
        public string UniqueComponentID { get; set; }
     
        public string ClientRefID { get; set; }
        public string CompanyRefNo { get; set; }

        public string Severity { get; set; }
        public string ColorName { get; set; }

        public string StateName { get; set; }
        public string DistrictName { get; set; }

        public short CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public DateTime? OutDate { get; set; }
        public DateTime? InternalOutDate { get; set; }
        public bool IsSynced { get; set; }
        public string CheckStatus { get; set; }
        public string PtrStatus { get; set; }
        public string ReWorkCheckStatus { get; set; }

        public string RevwRequestBy { get; set; }
        public DateTime? RevwRequestDate { get; set; }

        public string Remarks { get; set; }
        public byte Status { get; set; }
        // Add new filed 22-07-2017
        public int Ageing { get; set; }
        public string SecuritasSPOC { get; set; }
        public string AllocatedTLName { get; set; }
        public string AllocatedTMName { get; set; }
        public string AllocatedQCName { get; set; }
        public string ActionHistoryName { get; set; }
        public string Location { get; set; }
        public string Partner { get; set; }
        public string Package { get; set; }
        public string Emailid { get; set; }
        public string Entity { get; set; }
        public string MobileNo { get; set; }

    }

    public class ClientVerifyInfoListPagedModel
    {
        public IEnumerable<VerifyInfoForClientViewModel> PQVerifyInfo { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }
        public PQPersonalSearchPagedModel ItemSearch { get; set; }
    }
}
