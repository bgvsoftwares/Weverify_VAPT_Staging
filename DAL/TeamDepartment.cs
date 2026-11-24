using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class TeamDepartment
    {
        public TeamDepartment()
        {
            TeamDepartmentRowID = 0;
            TeamMemberRowID = 0;
            DepartmentRowID = 0;
            DesignationRowID = 0;
            Status = 0;
        }

        public short TeamDepartmentRowID { get; set; }
        
        public short TeamMemberRowID { get; set; }
        public virtual TeamMember TeamMember { get; set; }

        public byte DepartmentRowID { get; set; }
        public virtual MasterDepartment MasterDepartment { get; set; }

        public byte DesignationRowID { get; set; }
        public virtual MasterDesignation MasterDesignation { get; set; }

        public byte Status { get; set; }
    }
}
