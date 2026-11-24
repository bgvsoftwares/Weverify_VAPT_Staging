using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ViewModels.ProvidedInfoViewModel;

namespace BAL.ProvidedInfoRepository
{
    public interface IEmailVerificationRepository
    {
        EmploymentEmailVerificationViewModel GetEmploymentEmailVerification(int personalrowid = 0, int subcheckRowid = 0);
        //EducationEmailVerificationViewModel GetEducationEmailVerification(int EducationRowId = 0);
        EducationEmailVerificationViewModel GetEducationEmailVerification(int personalrowid = 0, int subcheckRowid = 0);
        EducationEmailVerificationViewModel StopVendor(int PersonalRowId = 0, int SubCheckRowID = 0, int CheckStatusID = 0, string AuditType = "");
    }
}
