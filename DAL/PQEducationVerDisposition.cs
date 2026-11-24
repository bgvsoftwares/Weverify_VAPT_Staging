using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class PQEducationVerDisposition
    {
        public Int64 EducationVerDispositionRowID { get; set; }

        public int EducationVerRowID { get; set; }
        public virtual PQEducationVer PQEducationVer { get; set; }

        public int ClientDispositionRowId { get; set; }
        public virtual PQClientDisposition PQClientDisposition { get; set; }
    }
}
