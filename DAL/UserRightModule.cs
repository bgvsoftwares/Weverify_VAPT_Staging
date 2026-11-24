using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class UserRightModule
    {
        public UserRightModule()
        {
            UserRightModuleRowID = 0;
            ModuleName = string.Empty;
            SubModuleName = "All";
            None = 3;
            ReadOnly = 2;
            ReadWrite = 1;
            Status = 1;
        }

        public short UserRightModuleRowID { get; set; }
        public string ModuleName { get; set; }
        public string SubModuleName { get; set; }
        public byte None { get; set; }
        public byte ReadOnly { get; set; }
        public byte ReadWrite { get; set; }
        public byte Status { get; set; }

    }
}
