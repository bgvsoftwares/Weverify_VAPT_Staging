using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
   public class PQVerifiedUploadCaseDoc
    {
        public PQVerifiedUploadCaseDoc()
        {
            CaseDocRowID = 0;
            PersonalRowID = 0;
            DocumentUploadFrom = string.Empty;
            DocumentType = string.Empty;
            FileName = string.Empty;
            FilePath = string.Empty;
            Remarks = string.Empty;
            UploadDate = DateTime.Now;
            Status = 1;
            PdfConvertedToImage = false;
            ImageFileName = string.Empty;
            RptArtiFacts = 0;
            RptDisplayOrder = 0;
        }

        public Int64 CaseDocRowID { get; set; }

        public int PersonalRowID { get; set; }
        public virtual PQPersonal PQPersonal { get; set; }

        public string DocumentUploadFrom { get; set; }      //  Provided / Verified
        public string DocumentType { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string Remarks { get; set; }
        public DateTime? UploadDate { get; set; }
        public byte Status { get; set; }                //Default - 1

        public bool PdfConvertedToImage { get; set; }
        public string ImageFileName { get; set; }
        public byte RptArtiFacts { get; set; }
        public DateTime? RptArtiFactsDate { get; set; }
        public byte RptDisplayOrder { get; set; }
    }
}
