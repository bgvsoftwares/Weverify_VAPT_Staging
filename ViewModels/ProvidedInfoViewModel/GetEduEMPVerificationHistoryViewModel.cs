using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.ProvidedInfoViewModel
{
    public class GetEduEMPVerificationHistoryViewModel
    {
        public int PersonalRowID { get; set; }
        public short SubCheckRowID { get; set; }
        public string CollegeName { get; set; }
        public string EmploymentName { get; set; }
        public string ConcernPersonName { get; set; }
        public string DesigConcernPerson { get; set; }
        public string OfficialEmailId { get; set; }
        public string OfficialLandlineNo { get; set; }
        public DateTime? CreationDate { get; set; }
        public string AuditType { get; set; }
        public string Message { get; set; }
        public string CourseNameDegreeQualificationName { get; set; }
        public string Category { get; set; }

    }
}
