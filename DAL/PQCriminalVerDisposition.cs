using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class PQCriminalVerDisposition
    {
        public Int64 CriminalVerDispositionRowID { get; set; }

        public int CriminalVerRowId { get; set; }
        public virtual PQCriminalVer PQCriminalVer { get; set; }

        public int ClientDispositionRowId { get; set; }
        public virtual PQClientDisposition PQClientDisposition { get; set; }
    }
}
