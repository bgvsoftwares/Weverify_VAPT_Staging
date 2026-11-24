using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class UserRight
    {
        public UserRight()
        {
            UserRightRowID = 0;
            UserTypeRowID = 0;
            UserRightModuleRowID = 0;
            UserAccessRight = 0;
            Status = 1;
        }
        public short UserRightRowID { get; set; }

        public short UserTypeRowID { get; set; }
        public virtual UserType UserType { get; set; }

        public short UserRightModuleRowID { get; set; }
        public virtual UserRightModule UserRightModule { get; set; }

        public byte UserAccessRight { get; set; }
        public byte Status { get; set; }
    }
}
