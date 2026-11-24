using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class TeamMemberReporting
    {
        public TeamMemberReporting()
        {
            ReportTeamRowID = 0;
            TeamDepartmentRowID = 0;
            ReportingTo = 0;
        }

        public int ReportTeamRowID { get; set; }

        public short TeamDepartmentRowID { get; set; }
        public virtual TeamDepartment TeamDepartment { get; set; }

        public short ReportingTo { get; set; }
    }
}
