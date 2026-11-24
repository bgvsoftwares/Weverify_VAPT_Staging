using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
   public class TempPQVerifiedUploadDoc
    {
        public TempPQVerifiedUploadDoc()
        {
            VerifiedUploadRowID = 0;
            PersonalRowID = 0;
            SubCheckRowID = 0;
            DocumentUploadFrom = string.Empty;
            DocumentType = string.Empty;
            FileName = string.Empty;
            FilePath = string.Empty;
            Remarks = string.Empty;
            UploadDate = DateTime.Now;
            StatusDocxConvert = 0;
            StatusPDF2DOCXConvert = 0;
            PDF_FileName = string.Empty;
            Status = 1;
        }

        public Int64 VerifiedUploadRowID { get; set; }

        public int PersonalRowID { get; set; }
        public virtual TempPQPersonal TempPQPersonal { get; set; }

        public short SubCheckRowID { get; set; }
        public virtual MasterSubCheckFamily MasterSubCheckFamily { get; set; }

        public string DocumentUploadFrom { get; set; }      //  Provided / Verified
        public string DocumentType { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string Remarks { get; set; }
        public DateTime? UploadDate { get; set; }
        public byte StatusDocxConvert { get; set; }      //Default - 0 otherwise 1
        public byte StatusPDF2DOCXConvert { get; set; }  //Default - 0 otherwise 1
        public string PDF_FileName { get; set; }
        public byte Status { get; set; }                //Default - 1
    }
}
