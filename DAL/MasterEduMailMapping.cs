using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class MasterEduMailMapping
    {
        public MasterEduMailMapping()
        {
            EduMailMappingRowID = 0;
            UniversityRowID = 0;            
            CollegeRowID = 0;
            EMailType = string.Empty;
            EMailID = string.Empty;
            Status = 1;
        }
        public int EduMailMappingRowID { get; set; }
        public short UniversityRowID { get; set; }
        public int CollegeRowID { get; set; }
        public string EMailType { get; set; }
        public string EMailID { get; set; }
        public virtual MasterUniversity MasterUniversity { get; set; }
        public virtual MasterCollege MasterCollege { get; set; }
        public byte Status { get; set; }
    }
}
