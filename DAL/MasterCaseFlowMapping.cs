using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class MasterCaseFlowMapping
    {
        public MasterCaseFlowMapping()
        {
            CaseFlowMappingRowID = 0;
            ReportCheckMappingRowID = 0;
            ColorCode = string.Empty;
            MoveToRWQC = 0;
            ColorCodeReview = 0;
            SendToClient = 0;            
            Status = 1;
        }
        public int CaseFlowMappingRowID { get; set; }
        public int ReportCheckMappingRowID { get; set; }              
        public virtual MasterReportCheckMapping MasterReportCheckMapping { get; set; }
        public string ColorCode { get; set; }
        public byte MoveToRWQC { get; set; }
        public byte ColorCodeReview { get; set; }
        public byte SendToClient { get; set; }        
        public byte Status { get; set; }
    }
}
