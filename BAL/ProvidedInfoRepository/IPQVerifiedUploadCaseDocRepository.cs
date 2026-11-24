using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.ProvidedInfoViewModel;

namespace BAL.ProvidedInfoRepository
{
    public interface IPQVerifiedUploadCaseDocRepository
    {
        string ExistcaseFileByPersonalIdAndDocType(int PersonalRowID = 0, string DocumentType = "", string fileName = "");
        void AddPQVerifiedUploadCaseDoc(AddPQVerifiedUploadCaseDocViewModel model);
        string ExistFileByPersonalIdAndDocType(int PersonalRowID = 0, string DocumentType = "");
        IEnumerable<PQVerifiedUploadCaseDocViewModel> GetCaseDocumentListByPersonalId(int PersonalRowID = 0, int SubCheckId = 0);
        IEnumerable<PQVerifiedUploadCaseDocViewModel> GetCaseDocumentList1ByPersonalId(int PersonalRowID = 0, int SubCheckId = 0, int ClientRowid = 0);
        IEnumerable<PQVerifiedUploadCaseDocViewModel> GetCaseDocumentList(int PersonalRowID = 0);
        int SaveChanges();
    }
}
