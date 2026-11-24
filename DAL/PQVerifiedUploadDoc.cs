using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class PQVerifiedUploadDoc
    {
        public PQVerifiedUploadDoc()
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
            InsuffDoc = 0;
            RptArtiFacts = 0;
            PdfConvertedToImage = false;
            ImageFileName = string.Empty;
            RptDisplayOrder = 0;
        }

        public Int64 VerifiedUploadRowID { get; set; }

        public int PersonalRowID { get; set; }
        public virtual PQPersonal PQPersonal { get; set; }

        public short SubCheckRowID { get; set; }
        public virtual MasterSubCheckFamily MasterSubCheckFamily { get; set; }

        public string DocumentUploadFrom { get; set; }      //  Provided / Verified
        public string DocumentType {  get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string Remarks { get; set; }
        public DateTime? UploadDate { get; set; }
        public byte StatusDocxConvert { get; set; }       //Store Artifacts Order Sequence For Report
        public byte StatusPDF2DOCXConvert { get; set; }  //Default - 0 otherwise 1
        public string PDF_FileName { get; set; }
        public byte Status { get; set; }                //Default - 1

        public byte InsuffDoc { get; set; }
        public byte RptArtiFacts { get; set; }
        public DateTime? RptArtiFactsDate { get; set; }
        public bool PdfConvertedToImage { get; set; }
        public string ImageFileName { get; set; }
        public byte RptDisplayOrder { get; set; }
    }
}
