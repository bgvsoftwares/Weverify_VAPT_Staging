using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class UserType
    {
        public UserType()
        {
            UserTypeRowID = 0;
            UserTypeName = string.Empty;
            Status = 1;
        }
        public short UserTypeRowID { get; set; }
        public string UserTypeName { get; set; }
        public byte Status { get; set; }
    }
}
