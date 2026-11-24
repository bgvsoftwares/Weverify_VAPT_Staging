using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.ClientViewModel;

namespace BAL.CandidateRepository
{
    public interface IImportCandidateCases
    {
        bool ImportCandidateCasesAndAllocatedToTL(short ClientRowID, int TempPersonalRowID);
        string GetCompanyRefNo(short ClientRowID = 0);
        string GetCandidateCode(short ClientRowID = 0);
        byte GetClientMaxTAT(short ClientRowID = 0);
        DateTime CalculateDeliveryDate(DateTime oDate, byte TAT, PQClientCodeHolidaysViewModel client);
    }
}
