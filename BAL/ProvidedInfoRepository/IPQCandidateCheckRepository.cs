using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.ProvidedInfoViewModel;
using ViewModels.TempViewModel;

namespace BAL.ProvidedInfoRepository
{
    public interface IPQCandidateCheckRepository
    {
        void AddCandidateCheck(AddPQCandidateCheckViewModel model);
        bool UpdateYTR_RaisedOrCleared_Date(int PersonalRowId, int SubcheckFamilyId, string Action);
        int SaveChanges();

        bool UpdateModeOfverificationPqcandidateCheck(int personalrowid , int subcheckfamilyId , string ModeOfVerification);
        bool UpdateFundDateandRemark(int PersonalRowId, int SubcheckFamilyId, string Action, string Remarks);
        ClientCheckPackageTAT GetTATForOutDateById(short ClientRowId = 0, int PersonalRowID = 0, short SubCheckRowID = 0);

        IEnumerable<PQCandidateCheckViewModel> GetCandidateChecks(int id = 0);
        IEnumerable<PQCandidateCheckViewModel> GetCandidateChecks1(int PersonalRowID = 0, int CheckStatusId = 0);

        #region ****temp  ******
        void AddTempCandidateCheck(AddTempPQCandidateCheckViewModel model);
        ClientCheckPackageTAT GetTATForOutDateTempCandidateById(short ClientRowId = 0, int PersonalRowID = 0, short SubCheckRowID = 0);
        IEnumerable<PQCandidateCheckViewModel> GetTempCandidateChecks(int id = 0);
        IEnumerable<PQCandidateCheckViewModel> GetInfuffCheck(int PersonalRowID = 0);
        #endregion

    }
}
