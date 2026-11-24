using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class WebUser
    {
        public WebUser()
        {
            WebUserRowID = 0;
            TeamMemberRoWID = 0;
            UnBlockedBy = 0;
            IsExportedExcel = 0;
            IsExportedWord = 0;
            IsExportedPDF = 0;
            Status = 1;
        }

        public short WebUserRowID { get; set; }

        public short TeamMemberRoWID { get; set; }
        public virtual TeamMember TeamMember { get; set; }

        public string CRPUserName { get; set; }
        public string CRPPassWord { get; set; }

        public DateTime? CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public DateTime? BlockedDate { get; set; }        
        public short UnBlockedBy { get; set; }
        public DateTime? UnBlockedDate { get; set; }
        public byte IsExportedExcel { get; set; }
        public byte IsExportedWord { get; set; }
        public byte IsExportedPDF { get; set; }
        public byte Status { get; set; }
    }
}
