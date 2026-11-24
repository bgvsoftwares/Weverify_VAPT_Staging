using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class PQReferenceVerDisposition
    {
        public Int64 ReferenceVerDispositionRowID { get; set; }

        public int ReferenceCheckVerRowID { get; set; }
        public virtual PQReferenceVer PQReferenceVer { get; set; }

        public int ClientDispositionRowId { get; set; }
        public virtual PQClientDisposition PQClientDisposition { get; set; }
    }
}
