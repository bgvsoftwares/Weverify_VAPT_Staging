using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class MasterArtifactMapping
    {
        public MasterArtifactMapping()
        {
            ArtifactMappingRowID = 0;
            ReportCheckMappingRowID = 0;
            ClientSeverityRowId = 0;
            ColorCode = string.Empty;
            CheckFamilyRowID = 0;
            DocumentTypeId = 0;
            DocumentType = string.Empty;
            DocumentSeqNo = 0;                  
            Status = 1;
        }
        public int ArtifactMappingRowID { get; set; }
        public int ReportCheckMappingRowID { get; set; }              
        public virtual MasterReportCheckMapping MasterReportCheckMapping { get; set; }
        public int ClientSeverityRowId { get; set; }        
        public string ColorCode { get; set; }
        public virtual PQClientSeverity PQClientSeverity { get; set; }
        public short CheckFamilyRowID { get; set; }
        public virtual MasterCheckFamily MasterCheckFamily { get; set; }
        public int DocumentTypeId { get; set; }
        public string DocumentType { get; set; }
        public short DocumentSeqNo { get; set; }             
        public byte Status { get; set; }
    }
}
