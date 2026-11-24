using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class PQEducationResearch
    {
        public PQEducationResearch()
        {
            EduResearchRowID = 0;
            EducationRowId = 0;
            OtherInstitute = string.Empty;
            AddedBy = 0;
            AddedNameDesig = string.Empty;
            UpdatedBy = 0;
            UpdatedNameDesig = string.Empty;
            Status = 1;
        }

        public int EduResearchRowID { get; set; }

        public int EducationRowId { get; set; }
        public virtual PQEducation PQEducation { get; set; }

        public string OtherInstitute { get; set; }

        public short AddedBy { get; set; }
        public string AddedNameDesig { get; set; }
        public DateTime? AddedOn { get; set; }

        public short UpdatedBy { get; set; }
        public string UpdatedNameDesig { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public byte Status { get; set; }
    }
}
