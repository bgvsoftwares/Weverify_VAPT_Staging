using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.ProvidedInfoViewModel;

namespace BAL.ProvidedInfoRepository
{
    public interface ICaseActionHistoryRepository
    {
        void AddCaseActionHistory(AddCaseActionHistoryViewModel model);
        CaseActionHistoryListPagedModel GetCaseActionHistories(int PersonalRowID = 0);
        int SaveChanges();
    }
}
