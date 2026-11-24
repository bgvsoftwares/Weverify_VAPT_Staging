using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.TempViewModel;

namespace BAL.ProvidedInfoRepository
{
    public interface IPQClientCandiBulkUploadRepository
    {
        void AddPQClientCandiBulkUpload(AddPQClientCandiBulkUploadViewModel model);
        int GetNoOfExcelFileByClientID(short ClientId = 0);
        int SaveChanges();
    }
}
