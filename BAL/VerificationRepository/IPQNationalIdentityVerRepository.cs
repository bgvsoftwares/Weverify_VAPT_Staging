using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.VerificationViewModel;

namespace BAL.VerificationRepository
{
    public interface IPQNationalIdentityVerRepository
    {
        PQNationalIdentityVerViewModel GetNationalIdentityInfoForDataEntry(int NationalIdentityRowId = 0);

        PQNationalIdentityVerViewModel GetNationalIdentityForTM(int NationalIdentityRowId = 0);

        PQPersonalDetailsVerViewModel GetPersonalDetailsForTM(int NationalIdentityRowId = 0);

        void AddNationalIdentityByTeamMember(PQNationalIdentityVerViewModel model);

        void UpdateNationalIdentityByTeamMember(PQNationalIdentityVerViewModel model);

        void AddNationalIdentityVerCloserDetails(NationalIdentityVerCloserViewModel model);

        NationalIdentityVerCloserViewModel GetNationalIdentityVerCloserDetails(int EduVerId = 0);

        void UpdateNationalIdentityByTMQC(UpdatePQNationalIdentityVerViewModel model, short RptQCManagerId = 0);

        UpdatePQNationalIdentityVerViewModel GetNationalIdentityForTMQC(int NationalIdentityRowId = 0,int CheckStatusid=0);

        int SaveChanges();
    }
}
