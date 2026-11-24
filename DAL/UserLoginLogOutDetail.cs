using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
   public class UserLoginLogOutDetail
    {
        public int LoginDetailRowID { get; set; }
        public string LoginName { get; set; }
        public string LoginType { get; set; }
        public string LoginId { get; set; }
        public string LoginBy { get; set; }
        public string IPDetails { get; set; }
        public DateTime? LoginDate { get; set; }
        public DateTime? LoginTime { get; set; }
        public DateTime? LogoutTime { get; set; }
        public byte LoginStatus { get; set; }
        public string Deviceidentifiers { get; set; }
        public string BrowserName { get; set; }
    }
}
