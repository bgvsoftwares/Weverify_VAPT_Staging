using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class PQLogTrasaction
    {
        public PQLogTrasaction()
        {
            TransactionRowID = 0;
            TeamMemberRowID = 0;
            UserType = string.Empty;
            PersonalRowID = 0;
            SubCheckRowID = 0;
            UniqueComponentID = string.Empty;
            PageName = string.Empty;
            CaseStatus = string.Empty;
            TransactionDate = DateTime.Now;
            TransactionAction = string.Empty;
            Status = 1;
        }

        public Int64 TransactionRowID { get; set; }
        public short TeamMemberRowID { get; set; }
        public string UserType { get; set; }
        public int PersonalRowID { get; set; }
        public short SubCheckRowID { get; set; }
        public string UniqueComponentID { get; set; }
        public string PageName { get; set; }
        public string CaseStatus { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string TransactionAction { get; set; }
        public byte Status { get; set; }
    }
}
