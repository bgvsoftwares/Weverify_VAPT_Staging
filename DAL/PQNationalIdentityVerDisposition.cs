using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class PQNationalIdentityVerDisposition
    {
        public Int64 IdentityVerDispositionRowID { get; set; }

        public int NationalIdentityVerRowID { get; set; }
        public virtual PQNationalIdentityVer PQNationalIdentityVer { get; set; }

        public int ClientDispositionRowId { get; set; }
        public virtual PQClientDisposition PQClientDisposition { get; set; }
    }
}
