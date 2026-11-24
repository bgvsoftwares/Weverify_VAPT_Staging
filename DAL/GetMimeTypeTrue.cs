using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class GetMimeTypeTrue
    {
        private static readonly byte[] BMP = { 66, 77 };
        private static readonly byte[] DOC = { 208, 207, 17, 224, 161, 177, 26, 225 };
        private static readonly byte[] GIF = { 71, 73, 70, 56 };
        private static readonly byte[] ICO = { 0, 0, 1, 0 };
        private static readonly byte[] JPG = { 255, 216, 255 };
        private static readonly byte[] pdf = { 37, 80, 68, 70, 45, 49, 46 };
        private static readonly byte[] PDF = { 172, 237, 0, 5 };
        private static readonly byte[] P_DF = { 84, 104, 101, 32 };
        private static readonly byte[] PDF_html = { 10, 10, 37, 80,68,70 };
        private static readonly byte[] PNG = { 137, 80, 78, 71, 13, 10, 26, 10, 0, 0, 0, 13, 73, 72, 68, 82 };
        private static readonly byte[] TIFF = { 73, 73, 42, 0 };
        private static readonly byte[] ZIP_DOCX = { 80, 75, 3, 4 };
        private static readonly byte[] mp3 = { 73, 68, 51, 4 };
        private static readonly byte[] MP3 = { 255, 251, 20, 196 };
        private static readonly byte[] MP3_1 = { 255, 227, 24, 196 };
        private static readonly byte[] WAV = { 82, 73, 70, 70 };
        private static readonly byte[] zip = { 37, 80, 68, 70, 45 };
        private static readonly byte[] XLSX = { 80, 75, 3, 4 };
        private static readonly byte[] XLS = { 239, 187, 191, 60 };
        private static readonly byte[] XLS2 = { 208, 207, 17, 224 };
        private static readonly byte[] M4A = { 0, 0, 0, 24, 102, 116 };
        public  bool GetMimeType(byte[] file, string fileName)
        {
            bool mime = false;
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return mime;
            }
            string extension = Path.GetExtension(fileName) == null ? string.Empty : Path.GetExtension(fileName).ToUpper();
            if (file.Take(2).SequenceEqual(BMP))
            {
                mime = true;
            }
            else if (file.Take(8).SequenceEqual(DOC))
            {
                mime = true;
            }
            else if (file.Take(4).SequenceEqual(GIF))
            {
                mime = true;
            }
            else if (file.Take(4).SequenceEqual(ICO))
            {
                mime = true;
            }
            else if (file.Take(3).SequenceEqual(JPG))
            {
                mime = true;
            }
            else if (file.Take(7).SequenceEqual(pdf))
            {
                mime = true;
            }

            else if (file.Take(4).SequenceEqual(PDF))
            {
                mime = true;
            }
            else if (file.Take(16).SequenceEqual(PNG))
            {
                mime = true;
            }
            else if (file.Take(4).SequenceEqual(TIFF))
            {
                mime = true;
            }
            else if (file.Take(4).SequenceEqual(ZIP_DOCX))
            {
                mime = true;
            }
            else if (file.Take(4).SequenceEqual(MP3))
            {
                mime = true;
            }
            else if (file.Take(4).SequenceEqual(mp3))
            {
                mime = true;
            }
            else if (file.Take(4).SequenceEqual(WAV))
            {
                mime = true;
            }
            else if (file.Take(5).SequenceEqual(zip))
            {
                mime = true;
            }
            else if (file.Take(6).SequenceEqual(M4A))
            {
                mime = true;
            }
            else if (file.Take(4).SequenceEqual(P_DF))
            {
                mime = true;
            }
            else if (file.Take(6).SequenceEqual(PDF_html))
            {
                mime = true;
            }
            else if (file.Take(4).SequenceEqual(MP3_1))
            {
                mime = true;
            }

            return mime;
        }
        public bool GetMimeTypeXLS(byte[] file, string fileName)
        {
            bool mime = false;
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return mime;
            }
            string extension = Path.GetExtension(fileName) == null ? string.Empty : Path.GetExtension(fileName).ToUpper();
            if (file.Take(4).SequenceEqual(XLS))
            {
                mime = true;
            }

            else if (file.Take(4).SequenceEqual(XLSX))
            {
                mime = true;
            }
            else if (file.Take(4).SequenceEqual(XLS2))
            {
                mime = true;
            }
            return mime;
        }
    }
}
