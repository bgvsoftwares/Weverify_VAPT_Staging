using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;

namespace WebAppBGV.Services
{
    public class CustomHttpPostedFileBase : HttpPostedFileBase
    {
        private readonly string _fileName;
        private readonly string _filePath;
        private readonly long _contentLength;
        private readonly string _contentType;

        public CustomHttpPostedFileBase(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("File not found at path: " + filePath);

            _filePath = filePath;
            _fileName = Path.GetFileName(filePath);
            _contentLength = new FileInfo(filePath).Length;
            _contentType = MimeMapping.GetMimeMapping(_fileName);
        }

        public override string FileName => _fileName;
        public override int ContentLength => (int)Math.Min(_contentLength, int.MaxValue);                
        public override Stream InputStream
        {
            get
            {
                return new FileStream(_filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            }
        }
        public override string ContentType => _contentType;

        public override void SaveAs(string filename)
        {
            using (var sourceStream = InputStream)
            using (var fileStream = System.IO.File.Create(filename))
            {
                sourceStream.CopyTo(fileStream);
            }
        }


    }
}