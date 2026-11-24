using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class PQEmploymentResearch
    {
        public PQEmploymentResearch()
        {
            EmpResearchRowID = 0;
            EmploymentRowId = 0;
            OtherEmployer = string.Empty;
            AddedBy = 0;
            AddedNameDesig = string.Empty;
            UpdatedBy = 0;
            UpdatedNameDesig = string.Empty;
            Status = 1;
        }
        public int EmpResearchRowID { get; set; }

        public int EmploymentRowId { get; set; }
        public virtual PQEmployment PQEmployment { get; set; }

        public string OtherEmployer { get; set; }

        public short AddedBy { get; set; }
        public string AddedNameDesig { get; set; }
        public DateTime? AddedOn { get; set; }

        public short UpdatedBy { get; set; }
        public string UpdatedNameDesig { get; set; }
        public DateTime? UpdatedOn { get; set; }

        public byte Status { get; set; }
    }
}
