using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class PQCandidateCheck
    {
        public PQCandidateCheck()
        {
            CandCheckRowID = 0;
            ClientRowID = 0;
            PersonalRowID = 0;
            ClientPackageRowID = 0;
            ClientCheckRowID = 0;
            Status = 1;
            //IsCheckDeleted = false;
        }

        public short? SubCheckFamilyID { get; set; }
        public string ModeOfVerification { get; set; }
        public string FundRemarks { get; set; }
        public DateTime? FundApprovalDate { get; set; }
        public DateTime? FundRejectedDate { get; set; }
        public DateTime? YTRClearedDate { get; set; }
        public DateTime? YTRRaisedDate { get; set; }

        public Int64 CandCheckRowID { get; set; }

        public short ClientRowID { get; set; }
        public virtual PQClientMaster PQClientMaster { get; set; }

        public int PersonalRowID { get; set; }
        public virtual PQPersonal PQPersonal { get; set; }

        public int ClientPackageRowID { get; set; }
        public int ClientCheckRowID { get; set; }

        public byte Status { get; set; }
       // public bool IsCheckDeleted { get; set; }
    }
}