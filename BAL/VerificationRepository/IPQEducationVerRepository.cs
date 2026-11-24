using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.VerificationViewModel;

namespace BAL.VerificationRepository
{
     public interface IPQEducationVerRepository     
    {
        PQEducationVerViewModel GetEducationInfoForDataEntry(int EducationRowId = 0);

        UpdatePQEducationVerViewModel GetEducationInfoForTMQC(int EducationRowId = 0);

        PQEducationVerViewModel GetEducationForTM(int EducationRowId = 0);

        PQPersonalDetailsVerViewModel GetPersonalDetailsForTM(int EducationRowId = 0);

        void AddEducationByTeamMember(PQEducationVerViewModel model);

        void UpdateEducationByTeamMember(PQEducationVerViewModel model);

        void AddEducationVerCloserDetails(EducationVerCloserViewModel model);

        EducationVerCloserViewModel GetEducationVerCloserDetails(int EduVerId = 0);

        void UpdateEducationByTMQC(UpdatePQEducationVerViewModel model, short RptQCManagerId = 0);

        int SaveChanges();

        string GetCollegeNameForResearch(int EducationRowId = 0);
    }
}
