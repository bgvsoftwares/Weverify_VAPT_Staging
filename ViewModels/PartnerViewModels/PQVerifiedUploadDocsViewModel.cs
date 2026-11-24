using DAL;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.PartnerViewModels
{
    public class PQVerifiedUploadDocsViewModel
    {
        [ScaffoldColumn(false)]
        public Int64 VerifiedUploadRowID { get; set; }

        [ScaffoldColumn(false)]
        public int PQAddressVarPartenerRowId { get; set; }
      
        [ScaffoldColumn(false)]
        public int PersonalRowID { get; set; }

        [ScaffoldColumn(false)]
        public short SubCheckRowID { get; set; }

        [MaxLength(200)]
        public string DocumentUploadFrom { get; set; }      //  Provided / Verified

        [MaxLength(50)]
        public string DocumentType { get; set; }

        [MaxLength(100)]
        public string FileName { get; set; }

        [MaxLength(300)]
        public string FilePath { get; set; }

        [ScaffoldColumn(false)]
        public byte Status { get; set; }//Default - 1

        public string Longitude { get; set; }
        public string Latitude { get; set; }
    }
}
