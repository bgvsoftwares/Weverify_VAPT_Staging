using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class PQEmploymentVerDisposition
    {
        public Int64 EmploymentVerDispositionRowID { get; set; }

        public int EmploymentVerRowID { get; set; }
        public virtual PQEmploymentVer PQEmploymentVer { get; set; }

        public int ClientDispositionRowId { get; set; }
        public virtual PQClientDisposition PQClientDisposition { get; set; }
    }
}
