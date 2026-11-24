using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using Microsoft.Office.Interop.Outlook;
using HtmlAgilityPack;

namespace WebAppBGV.Services
{

    public static class PdfSplitter
    {

        public static List<byte[]> SplitPdf(byte[] pdfBytes, int maxPagesPerFile = 10)
        {
            var result = new List<byte[]>();

            using (var reader = new PdfReader(pdfBytes))
            {
                int totalPages = reader.NumberOfPages;
                int fileIndex = 1;

                for (int page = 1; page <= totalPages; page += maxPagesPerFile)
                {
                    using (var ms = new MemoryStream())
                    {
                        using (var doc = new Document())
                        {
                            using (var copy = new PdfCopy(doc, ms))
                            {
                                doc.Open();

                                int startPage = page;
                                int endPage = Math.Min(page + maxPagesPerFile - 1, totalPages);

                                for (int p = startPage; p <= endPage; p++)
                                {
                                    copy.AddPage(copy.GetImportedPage(reader, p));
                                }

                                doc.Close();
                            }
                        }
                        result.Add(ms.ToArray());
                    }

                    fileIndex++;
                }
            }

            return result;
        }


    }


}