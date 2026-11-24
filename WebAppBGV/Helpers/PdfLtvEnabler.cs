using iTextSharp.text.pdf;
using System.Collections.Generic;
using System.IO;

namespace eSignProject.Helpers
{
    public class PdfLtvEnabler
    {
        public static void AddDss(
            string signedPdf,
            string outputPdf,
            List<byte[]> ocsp,
            List<byte[]> crl)
        {
            PdfReader reader = new PdfReader(signedPdf);
            using (FileStream fs = new FileStream(outputPdf, FileMode.Create))
            {
                PdfStamper stamper = new PdfStamper(reader, fs, '\0', true);
                PdfWriter writer = stamper.Writer;

                PdfDictionary dss = new PdfDictionary();
                PdfArray ocspArray = new PdfArray();
                PdfArray crlArray = new PdfArray();

                foreach (var o in ocsp)
                {
                    PdfStream ps = new PdfStream(o);
                    ps.FlateCompress();
                    ocspArray.Add(writer.AddToBody(ps).IndirectReference);
                }

                foreach (var c in crl)
                {
                    PdfStream ps = new PdfStream(c);
                    ps.FlateCompress();
                    crlArray.Add(writer.AddToBody(ps).IndirectReference);
                }

                if (ocspArray.Size > 0)
                    dss.Put(new PdfName("OCSPs"), ocspArray);

                if (crlArray.Size > 0)
                    dss.Put(new PdfName("CRLs"), crlArray);

                stamper.Writer.ExtraCatalog.Put(new PdfName("DSS"), dss);

                stamper.Close();
            }
            reader.Close();
        }
    }
}