using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.ProvidedInfoViewModel;
using ViewModels.TempViewModel;

namespace BAL.ProvidedInfoRepository
{
    public interface IInsuranceInfoRepository
    {
        IEnumerable<PQInsuranceList> GetInsuranceList(short ClientRowId = 0, short ChFamilRowId = 0, short subCheckId = 0);

        void AddInsuranceInformation(PQInsuranceViewModel model);

        int GetAllInsuranceByClientId(short ClientRowID = 0);

        int SaveChanges();

        #region ****** Temp Insurance Information *******
        void AddTempInsuranceInformation(TempPQInsuranceViewModel model);
        IEnumerable<TempPQInsuranceList> GetTempInsuranceList(short ClientRowId = 0, short ChFamilRowId = 0, short subCheckId = 0);
        int GetTempAllInsuranceByClientId(short ClientRowID = 0);

        #endregion
    }
}
