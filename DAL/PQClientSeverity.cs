using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class PQClientSeverity
    {
        public PQClientSeverity()
        {
            ClientSeverityRowId = 0;
            ClientColorCode = string.Empty;
            ClientRowID = 0;
            SeverityGridRowId = 0;
            Status = 1;
        }
        
        public int ClientSeverityRowId { get; set; }
        public string ClientColorName { get; set; }
        public string ClientColorCode { get; set; }

        public short ClientRowID { get; set; }
        public virtual PQClientMaster PQClientMaster { get; set; }

        public byte SeverityGridRowId { get; set; }
        public virtual MasterSeverityGrid MasterSeverityGrid { get; set; }

        public byte Status { get; set; }
    }
}
