using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.ProvidedInfoViewModel;

namespace BAL.ProvidedInfoRepository
{
    public interface ICheckActionHistoryRepository
    {
        void AddCheckActionHistory(AddCheckActionHistoryViewModel model);
        IEnumerable<CheckActionHistoryViewModel> GetCheckActionHistories(int PersonalRowID = 0, short SubCheckRowID = 0);
        int SaveChanges();
    }
}
