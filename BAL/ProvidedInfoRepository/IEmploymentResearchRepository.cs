using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.ProvidedInfoViewModel;

namespace BAL.ProvidedInfoRepository
{
    public interface IEmploymentResearchRepository
    {
        void AddEmploymentResearchInfo(AddPQEmploymentResearchViewModel model);
        int SaveDetailForResearch(AddPQEmploymentResearchViewModel model);
        PQEmploymentResearchListPagedModel GetEmploymentCheckListForResearch(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short ClientRowID = 0, string RecievingDate = "", string CompletedDate = "", string DCheckStatus = "", string CaseStatus = "", string EmployerName = "", string Allocatedto = "");
        PQEmploymentResearchListPagedModel GetEmploymentCheckListForResearch1(int pageNo, int pageSize, string sort, string sortDir, string Search = "", short ClientRowID = 0, string RecievingDate = "", string CompletedDate = "", string DCheckStatus = "", string CaseStatus = "", string EmployerName = "", string Allocatedto = "");

        PQUpdateEmploymentResearcherViewModel UpdateEmploymentResearchInfo(UpdatePQEmploymentResearchViewModel model, int EmploymentRowID = 0);
        IEnumerable<UploadResearchDoc> UploadResearchDoc(int personalRowID, int subCheckRowID);
        int SaveChanges();
    }
}
