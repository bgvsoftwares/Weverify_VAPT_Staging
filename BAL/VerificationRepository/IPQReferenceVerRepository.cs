using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.VerificationViewModel;

namespace BAL.VerificationRepository
{
    public interface IPQReferenceVerRepository
    {
        PQReferenceVerViewModel GetReferenceInfoForDataEntry(int ReferenceRowId = 0);

        PQReferenceVerViewModel GetReferenceForTM(int ReferenceRowId = 0);

        PQPersonalDetailsVerViewModel GetPersonalDetailsForTM(int ReferenceRowId = 0);

        void AddReferenceByTeamMember(PQReferenceVerViewModel model);

        void UpdateReferenceByTeamMember(PQReferenceVerViewModel model);

        void AddReferenceVerCloserDetails(ReferenceVerCloserViewModel model);

        ReferenceVerCloserViewModel GetReferenceVerCloserDetails(int EduVerId = 0);

        void UpdateReferenceByTMQC(UpdatePQReferenceVerViewModel model, short RptQCManagerId = 0);

        UpdatePQReferenceVerViewModel GetReferenceInfoForTMQC(int ReferenceRowId = 0,int CheckStatusId=0);

        int SaveChanges();
    }
}
