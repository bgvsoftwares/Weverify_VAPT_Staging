using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.ProvidedInfoViewModel;
using ViewModels.VerificationViewModel;

namespace BAL.VerificationRepository
{
   public interface IPQEmploymentVerRepository
    {
        PQEmploymentVerViewModel GetEmploymentInfoForDataEntry(int EmploymentRowId = 0);

        UpdatePQEmploymentVerViewModel GetEmploymentInfoForTMQC(int EmploymentRowId = 0, int CheckstatusId = 0);

        PQEmploymentVerViewModel GetEmploymentForTM(int EmploymentRowId = 0);

        PQPersonalDetailsVerViewModel GetPersonalDetailsForTM(int EmploymentRowId = 0);

        void AddEmploymentByTeamMember(PQEmploymentVerViewModel model);
        
        void UpdateEmploymentByTeamMember(PQEmploymentVerViewModel model);
        
        void UpdateEmploymentByTMQC(UpdatePQEmploymentVerViewModel model,short RptQCManagerId = 0);

        void AddEmploymentVerCloserDetails(EmploymentVerCloserViewModel model);

        EmploymentVerCloserViewModel GetEmploymentVerCloserDetails(int EmpVerId = 0);
        
        int SaveChanges();

        string GetCompanyNameForResearch(int EmploymentRowId = 0);
    }
}
