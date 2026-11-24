using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class ServerDetail
    {
        public ServerDetail()
        {
            ServerRowID = 0;
            ServerName = string.Empty;
            ServerUserID = string.Empty;
            ServerUserName = string.Empty;
            ServerPassword = string.Empty;
            PortNumber = string.Empty;
            Status = 1;
            TeamMemberRowID = 0;
        }
        public short ServerRowID { get; set; }
        public string ServerName { get; set; }
        public string ServerUserID { get; set; }
        public string ServerUserName { get; set; }
        public string ServerPassword { get; set; }
        public string PortNumber { get; set; }
        public byte Status { get; set; }
        public short TeamMemberRowID { get; set; }       
    }
}
