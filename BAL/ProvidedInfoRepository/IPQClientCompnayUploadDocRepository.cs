using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.TempViewModel;

namespace BAL.ProvidedInfoRepository
{
    public interface IPQClientCompnayUploadDocRepository
    {
        void AddPQClientCompnayUploadDoc(ClientByBulkUploadViewModel model);
        ClientByBulkUploadListPagedModel GetCompanyListForUpload(int pageNo, int pageSize, string sort, string sortDir, string Search = "",  short ClientRowID = 0,string UploadfromDate = "" ,string UploadToDate = "", string CaseStatusId = "");
        int SaveChanges();
    }
}
