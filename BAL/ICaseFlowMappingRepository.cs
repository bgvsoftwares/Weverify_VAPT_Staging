using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels;

namespace BAL
{
    public interface ICaseFlowMappingRepository
    {
        IEnumerable<CaseFlowMappingViewModel> GetCaseFlowMapping();

        CaseFlowMappingListPagedModel GetCaseFlowMapping(int pageNo, int pageSize, string sort, string sortDir, string Search = "", int RCMRowID = 0);

       
    }
}
