using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.ProvidedInfoViewModel;

namespace BAL.ProvidedInfoRepository
{
    public interface IEducationResearchRepository
    {
        int SaveandUpdateInsuffRemark(int PersonlRowID, int SubCheckRowID, string RemarkRaised, string InsuffRaisedDate, string check);
        void AddEducationResearchInfo(AddPQEducationResearchViewModel model);
        int SaveDetailForResearch(AddPQEducationResearchViewModel model);
        PQEducationResearchListPagedModel GetEducationCheckListForResearch(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short ClientRowID = 0, string RecievingDate = "", string CompletedDate = "", string DCheckStatus = "", string CaseStatus = "");
        PQEducationResearchListPagedModel GetEducationCheckListForResearch1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short ClientRowID = 0, string RecievingDate = "", string CompletedDate = "", string DCheckStatus = "", string CaseStatus = "",string SearchType = "");
        PQUpdateEducationResearcherViewModel UpdateEducationResearchInfo(UpdatePQEducationResearchViewModel model, int EducationRowID = 0);
        int SaveChanges();
    }
}
