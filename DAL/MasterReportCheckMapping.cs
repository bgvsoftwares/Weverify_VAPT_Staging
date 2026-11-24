using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class MasterReportCheckMapping
    {
        public MasterReportCheckMapping()
        {
            ReportCheckMappingRowID = 0;
            ClientRowID = 0;            
            ReportType = string.Empty;
            AddressChk = 0;
            CriminalChk = 0;
            EducationChk = 0;
            EmploymentChk = 0;
            IdentityChk = 0;
            ReferenceChk = 0;
            SpecialChk = 0;
            Status = 1;
        }
        public int ReportCheckMappingRowID { get; set; }
        public short ClientRowID { get; set; }        
        public virtual PQClientMaster PQClientMaster { get; set; }
        public string ReportType { get; set; }
        public byte AddressChk { get; set; }
        public byte CriminalChk { get; set; }
        public byte EducationChk { get; set; }
        public byte EmploymentChk { get; set; }
        public byte IdentityChk { get; set; }
        public byte ReferenceChk { get; set; }
        public byte SpecialChk { get; set; }
        public byte Status { get; set; }
    }
}
