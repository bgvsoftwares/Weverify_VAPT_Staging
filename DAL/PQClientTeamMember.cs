using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class PQClientTeamMember
    {
        public PQClientTeamMember()
        {
            ClientTMemberRowID = 0;
            ClientRowID = 0;
            TeamDepartmentRowID = 0;
            Status = 1;
        }

        public int ClientTMemberRowID { get; set; }

        public short ClientRowID { get; set; }
        public virtual PQClientMaster PQClientMaster { get; set; }

        public short TeamDepartmentRowID { get; set; }
        public virtual TeamDepartment TeamDepartment { get; set; }

        public byte Status { get; set; }
    }
}
