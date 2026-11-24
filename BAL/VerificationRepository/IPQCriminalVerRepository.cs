using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.VerificationViewModel;

namespace BAL.VerificationRepository
{
    public interface IPQCriminalVerRepository
    {
        PQCriminalVerViewModel GetCriminalInfoForDataEntry(int CriminalRowId = 0);

        PQCriminalVerViewModel GetCriminalForTM(int CriminalRowId = 0);

        PQPersonalDetailsVerViewModel GetPersonalDetailsForTM(int CriminalRowId = 0);

        void AddCriminalByTeamMember(PQCriminalVerViewModel model);

        void UpdateCriminalByTeamMember(PQCriminalVerViewModel model);

        void AddCriminalVerCloserDetails(CriminalVerCloserViewModel model);

        CriminalVerCloserViewModel GetCriminalVerCloserDetails(int CriVerId = 0);

        void UpdateCriminalByTMQC(UpdatePQCriminalVerViewModel model, short RptQCManagerId = 0);

        UpdatePQCriminalVerViewModel GetCriminalForTMQC(int CriminalRowId = 0,int CheckStatusId=0);
        
        int SaveChanges();
    }
}
