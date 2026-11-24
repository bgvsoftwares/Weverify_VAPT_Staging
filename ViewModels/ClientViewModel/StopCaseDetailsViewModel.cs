using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.ClientViewModel
{
   public class StopCaseDetailsViewModel
    {
        public IEnumerable<StopCaseDetailsListPagedModel> StopCaseDetailsList { get; set; }
        public int PageSize { get; set; }
        public int TotalRecords { get; set; }      

    }
    public class StopCaseDetailsListPagedModel
    {
        public string PersonalRowId { get; set; }
        public string CandidateName { get; set; }
        public string EmailId { get; set; }
        public string MobileNo { get; set; }
        public string CompanyRefNo { get; set; }
        public string ClientRefId { get; set; }
        public string CandidateCode { get; set; }
        public string IpAddresss { get; set; }
        public string CaseStopBy { get; set; }
        public string CaseStopDate { get; set; }
        public string CreatedDate { get; set; }
        public string FinalSubmissionDate { get; set; }
    
    }
}
