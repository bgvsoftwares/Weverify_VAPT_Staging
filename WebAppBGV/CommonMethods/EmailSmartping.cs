using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Security;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Web;
using Newtonsoft.Json;
using HeyRed.Mime;
using System.Net.Mail;
using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Windows.Interop;
using BAL;

namespace WebAppBGV.CommonMethods
{
    public class EmailSmartping
    {
        public string SendMail(int personalRowid, string senderEmail, string recipientEmail, string ccMail, string bccMail, string subject, string htmlBody, Attachment attadata, string fileName = "")
        {
            ClientCustomize ccs = new ClientCustomize();
            try
            {
                if (ccMail == "" || ccMail == null)
                {
                    bccMail = "-";
                }
                SendMailRootobject rootObject = new SendMailRootobject();
                rootObject.messages = new List<Message>();
                rootObject.options = new Options
                {
                    clientPriority = "STANDARD",
                    tracking = new Tracking
                    {
                        track = true,
                        trackOpens = true,
                        trackClicks = true
                    }
                };
                Message message = new Message
                {
                    sender = senderEmail,
                    destinations = new List<Destination>(),
                    content = new Content
                    {
                        subject = subject,
                        html = htmlBody,
                        text = System.Text.RegularExpressions.Regex.Replace(htmlBody, "<.*?>", string.Empty),
                        attachments = new List<MailAttachment>()
                    }
                };
                Destination destination = new Destination
                {
                    to = recipientEmail.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => new To { destination = x.Trim() }).ToList(),
                    cc = ccMail.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => new Cc { destination = x.Trim() }).ToList(),
                    bcc = bccMail.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => new Bcc { destination = x.Trim() }).ToList(),
                    replyTo = senderEmail
                };
                message.destinations.Add(destination);
                if (attadata != null)
                {
                    byte[] fileBytes = ReadStreamFully(attadata.ContentStream);
                    string StrExtension = GetFileExtensionFromBytes(fileBytes);
                    string base64Content = Convert.ToBase64String(fileBytes);
                    string StrfileName = "";
                    if (fileName != null)
                    {
                        if (fileName != "")
                        {
                            StrfileName = fileName;
                        }
                        else
                        {
                            StrfileName = Regex.Replace(subject.Trim(), @"\s+", "_") + "_Doc_attachment" + StrExtension;
                        }
                    }
                    else
                    {
                        StrfileName = Regex.Replace(subject.Trim(), @"\s+", "_") + "_Doc_attachment" + StrExtension;
                    }
                    message.content.attachments.Add(new MailAttachment
                    {
                        type = "binary",
                        fileName = StrfileName,
                        contentType = MimeTypesMap.GetMimeType(StrfileName),
                        content = base64Content
                    });
                }
                rootObject.messages.Add(message);
                string jsonPayload = JsonConvert.SerializeObject(rootObject);
                string url = "https://694rx8.apiemail.smartping.io/email/4/messages";
                //string authorizationToken = "App 5ed1b4c3801fc5dabc8913ae5e176332-bf5a04af-a798-4f7a-a1eb-0280b1b5f2e4";
                string authorizationToken = "App 52aa71d282329ff1109ca9bd11a4895d-aef7df42-43b2-4bc2-8158-c48e27b591c4";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = delegate (
                    object sender,
                    X509Certificate certificate,
                    X509Chain chain,
                    SslPolicyErrors sslPolicyErrors)
                {
                    return true; // Accept all certificates
                };
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add("Authorization", authorizationToken);
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(jsonPayload);
                }
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    return "Response email sending successfully: " + reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    using (var reader = new StreamReader(ex.Response.GetResponseStream()))
                    {
                        ccs.InsertReportEmailSentDetails(personalRowid, subject, "smartping WebException" + reader.ReadToEnd() + htmlBody);
                        return "Error: ";// + reader.ReadToEnd();
                    }
                }
                return "Error: ";// + ex.Message;
            }
            catch (Exception ex)
            {
                ccs.InsertReportEmailSentDetails(personalRowid, subject, "smartping Exception" + ex.Message.ToString() + htmlBody);
                return "Exception: ";// + ex.Message;
            }
        }

        public string SendMailmultiAttach(int personalRowid, string senderEmail, string recipientEmail, string ccMail, string bccMail, string subject, string htmlBody, List<System.Net.Mail.Attachment> attadata, string fileName = "")
        {
            ClientCustomize ccs = new ClientCustomize();
            try
            {
                if (ccMail == "" || ccMail == null)
                {
                    bccMail = "-";
                }
                SendMailRootobject rootObject = new SendMailRootobject();
                rootObject.messages = new List<Message>();
                rootObject.options = new Options
                {
                    clientPriority = "STANDARD",
                    tracking = new Tracking
                    {
                        track = true,
                        trackOpens = true,
                        trackClicks = true
                    }
                };
                Message message = new Message
                {
                    sender = senderEmail,
                    destinations = new List<Destination>(),
                    content = new Content
                    {
                        subject = subject,
                        html = htmlBody,
                        text = System.Text.RegularExpressions.Regex.Replace(htmlBody, "<.*?>", string.Empty),
                        attachments = new List<MailAttachment>()
                    }
                };
                Destination destination = new Destination
                {
                    to = recipientEmail.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => new To { destination = x.Trim() }).ToList(),
                    cc = ccMail.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => new Cc { destination = x.Trim() }).ToList(),
                    bcc = bccMail.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => new Bcc { destination = x.Trim() }).ToList(),
                    replyTo = senderEmail
                };
                message.destinations.Add(destination);
                if (attadata != null && attadata.Count > 0)
                {
                    foreach (var attach in attadata)
                    {
                        byte[] fileBytes = ReadStreamFully(attach.ContentStream);
                        string StrExtension = GetFileExtensionFromBytes(fileBytes);
                        string base64Content = Convert.ToBase64String(fileBytes);
                        string StrfileName = "";
                        string currentFileName = attach.Name;
                        if (currentFileName != null)
                        {
                            if (currentFileName != "")
                            {
                                StrfileName = currentFileName;
                            }
                            else
                            {
                                StrfileName = Regex.Replace(subject.Trim(), @"\s+", "_") + "_Doc_attachment" + StrExtension;
                            }
                        }
                        else
                        {
                            StrfileName = Regex.Replace(subject.Trim(), @"\s+", "_") + "_Doc_attachment" + StrExtension;
                        }
                        message.content.attachments.Add(new MailAttachment
                        {
                            type = "binary",
                            fileName = StrfileName,
                            contentType = MimeTypesMap.GetMimeType(StrfileName),
                            content = base64Content
                        });
                    }
                    //    byte[] fileBytes = ReadStreamFully(attach.ContentStream);
                    //string StrExtension = GetFileExtensionFromBytes(fileBytes);
                    //string base64Content = Convert.ToBase64String(fileBytes);
                    //string StrfileName = "";
                    //if (fileName != null)
                    //{
                    //    if (fileName != "")
                    //    {
                    //        StrfileName = fileName;
                    //    }
                    //    else
                    //    {
                    //        StrfileName = Regex.Replace(subject.Trim(), @"\s+", "_") + "_Doc_attachment" + StrExtension;
                    //    }
                    //}
                    //else
                    //{
                    //    StrfileName = Regex.Replace(subject.Trim(), @"\s+", "_") + "_Doc_attachment" + StrExtension;
                    //}
                    //message.content.attachments.Add(new MailAttachment
                    //{
                    //    type = "binary",
                    //    fileName = StrfileName,
                    //    contentType = MimeTypesMap.GetMimeType(StrfileName),
                    //    content = base64Content
                    //});
                }
                rootObject.messages.Add(message);
                string jsonPayload = JsonConvert.SerializeObject(rootObject);
                string url = "https://694rx8.apiemail.smartping.io/email/4/messages";
                //string authorizationToken = "App 5ed1b4c3801fc5dabc8913ae5e176332-bf5a04af-a798-4f7a-a1eb-0280b1b5f2e4";
                string authorizationToken = "App 52aa71d282329ff1109ca9bd11a4895d-aef7df42-43b2-4bc2-8158-c48e27b591c4";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = delegate (
                    object sender,
                    X509Certificate certificate,
                    X509Chain chain,
                    SslPolicyErrors sslPolicyErrors)
                {
                    return true; // Accept all certificates
                };
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add("Authorization", authorizationToken);
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(jsonPayload);
                }
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    return "Response email sending successfully: " + reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    using (var reader = new StreamReader(ex.Response.GetResponseStream()))
                    {
                        ccs.InsertReportEmailSentDetails(personalRowid, subject, "smartping WebException" + reader.ReadToEnd() + htmlBody);
                        return "Error: ";// + reader.ReadToEnd();
                    }
                }
                return "Error: ";// + ex.Message;
            }
            catch (System.Exception ex)
            {
                ccs.InsertReportEmailSentDetails(personalRowid, subject, "smartping Exception" + ex.Message.ToString() + htmlBody);
                return "Exception: ";// + ex.Message;
            }
        }
        public string SendMail_Att(int personalRowid, string senderEmail, string recipientEmail, string ccMail, string bccMail, string subject, string htmlBody, Attachment attadata, Attachment attadata2)
        {
            ClientCustomize ccs = new ClientCustomize();
            try
            {              

                if (ccMail == "" || ccMail == null)
                {
                    bccMail = "-";
                }
                SendMailRootobject rootObject = new SendMailRootobject();
                rootObject.messages = new List<Message>();
                rootObject.options = new Options
                {
                    clientPriority = "STANDARD",
                    tracking = new Tracking
                    {
                        track = true,
                        trackOpens = true,
                        trackClicks = true
                    }
                };
                Message message = new Message
                {
                    sender = senderEmail,
                    destinations = new List<Destination>(),
                    content = new Content
                    {
                        subject = subject,
                        html = htmlBody,
                        text = System.Text.RegularExpressions.Regex.Replace(htmlBody, "<.*?>", string.Empty),
                        attachments = new List<MailAttachment>()
                    }
                };
                Destination destination = new Destination
                {
                    to = recipientEmail.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => new To { destination = x.Trim() }).ToList(),
                    cc = ccMail.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => new Cc { destination = x.Trim() }).ToList(),
                    bcc = bccMail.Split(',').Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => new Bcc { destination = x.Trim() }).ToList(),
                    replyTo = senderEmail
                };
                message.destinations.Add(destination);
                if (attadata != null)
                {
                    byte[] fileBytes = ReadStreamFully(attadata.ContentStream);
                    string StrExtension = GetFileExtensionFromBytes(fileBytes);
                    string base64Content = Convert.ToBase64String(fileBytes);
                    string StrfileName = Regex.Replace(subject.Trim(), @"\s+", "_") + "_Doc_attachment" + StrExtension;
                    message.content.attachments.Add(new MailAttachment
                    {
                        type = "binary",
                        fileName = StrfileName,
                        contentType = MimeTypesMap.GetMimeType(StrfileName),
                        content = base64Content
                    });
                }
                if (attadata2 != null)
                {
                    byte[] fileBytes = ReadStreamFully(attadata2.ContentStream);
                    string StrExtension = GetFileExtensionFromBytes(fileBytes);
                    string base64Content = Convert.ToBase64String(fileBytes);
                    string StrfileName = Regex.Replace(subject.Trim(), @"\s+", "_") + "_Doc_attach" + StrExtension;
                    message.content.attachments.Add(new MailAttachment
                    {
                        type = "binary",
                        fileName = StrfileName,
                        contentType = MimeTypesMap.GetMimeType(StrfileName),
                        content = base64Content
                    });
                }
                rootObject.messages.Add(message);
                string jsonPayload = JsonConvert.SerializeObject(rootObject);
                string url = "https://694rx8.apiemail.smartping.io/email/4/messages";
                //string authorizationToken = "App 5ed1b4c3801fc5dabc8913ae5e176332-bf5a04af-a798-4f7a-a1eb-0280b1b5f2e4";
                string authorizationToken = "App 52aa71d282329ff1109ca9bd11a4895d-aef7df42-43b2-4bc2-8158-c48e27b591c4";
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls12;
                ServicePointManager.ServerCertificateValidationCallback = delegate (
                    object sender,
                    X509Certificate certificate,
                    X509Chain chain,
                    SslPolicyErrors sslPolicyErrors)
                {
                    return true; // Accept all certificates
                };
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add("Authorization", authorizationToken);
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(jsonPayload);
                }
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    return "Response email sending successfully: " + reader.ReadToEnd();
                }
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    using (var reader = new StreamReader(ex.Response.GetResponseStream()))
                    {
                        ccs.InsertReportEmailSentDetails(personalRowid, subject, "smartping WebException" + reader.ReadToEnd() + htmlBody);
                        return "Error: ";// + reader.ReadToEnd();
                    }
                }
                return "Error: ";// + ex.Message;
            }
            catch (Exception ex)
            {
                ccs.InsertReportEmailSentDetails(personalRowid, subject, "smartping Exception" + ex.Message.ToString() + htmlBody);
                return "Exception: ";// + ex.Message;
            }
        }
        private static byte[] ReadStreamFully(Stream stream)
        {
            if (stream == null)
                throw new ArgumentNullException(nameof(stream));

            if (stream.CanSeek)
                stream.Position = 0; // IMPORTANT: rewind stream

            using (var ms = new MemoryStream())
            {
                stream.CopyTo(ms);
                return ms.ToArray();
            }
        }
        public static string GetFileExtensionFromBytes(byte[] bytes)
        {
            if (bytes == null || bytes.Length < 4)
                return null;

            // PDF: 25 50 44 46 -> "%PDF"
            if (StartsWith(bytes, 0x25, 0x50, 0x44, 0x46))
                return ".pdf";

            // PNG: 89 50 4E 47 0D 0A 1A 0A
            if (StartsWith(bytes, 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A))
                return ".png";

            // JPEG: FF D8 FF
            if (StartsWith(bytes, 0xFF, 0xD8, 0xFF))
                return ".jpg"; // could also be .jpeg

            // GIF: "GIF87a" or "GIF89a"
            if (StartsWith(bytes, "GIF87a") || StartsWith(bytes, "GIF89a"))
                return ".gif";

            // ZIP container: 50 4B 03 04 (many formats like .zip, .docx, .xlsx, .pptx)
            if (StartsWith(bytes, 0x50, 0x4B, 0x03, 0x04))
            {
                // Could be Office Open XML – try to distinguish by reading entries
                var office = TryDetectOfficeOpenXml(bytes);
                if (!string.IsNullOrEmpty(office))
                    return office;

                return ".zip";
            }

            // RAR: 52 61 72 21 1A 07 00 (RAR4) or 52 61 72 21 1A 07 01 00 (RAR5)
            if (StartsWith(bytes, 0x52, 0x61, 0x72, 0x21, 0x1A, 0x07) &&
                (bytes.Length >= 7 && (bytes[6] == 0x00 || bytes[6] == 0x01)))
                return ".rar";

            // 7z: 37 7A BCF 27 1C
            if (StartsWith(bytes, 0x37, 0x7A, 0xBC, 0xAF, 0x27, 0x1C))
                return ".7z";

            // MP4: "ftyp" box at offset 4 after 4-byte size (common brands: isom, mp42, avc1, etc.)
            if (bytes.Length > 12 && bytes[4] == 0x66 && bytes[5] == 0x74 && bytes[6] == 0x79 && bytes[7] == 0x70)
                return ".mp4"; // could be .m4v/.mov depending on brand

            // WAV: "RIFF" .... "WAVE"
            if (StartsWith(bytes, "RIFF") && ContainsAt(bytes, "WAVE", 8))
                return ".wav";

            // AVI: "RIFF" .... "AVI "
            if (StartsWith(bytes, "RIFF") && ContainsAt(bytes, "AVI ", 8))
                return ".avi";

            // MP3: ID3 tag or FF FB frame sync (simplified)
            if (StartsWith(bytes, "ID3") || (bytes.Length > 1 && bytes[0] == 0xFF && (bytes[1] & 0xE0) == 0xE0))
                return ".mp3";

            // XML: "<" "?" "x" "m" "l" (may be text files—not uniquely identifiable)
            if (StartsWith(bytes, 0x3C, 0x3F, 0x78, 0x6D, 0x6C))
                return ".xml";

            // Fallback: unknown
            return null;
        }
        private static bool StartsWith(byte[] data, params byte[] prefix)
        {
            if (data.Length < prefix.Length) return false;
            for (int i = 0; i < prefix.Length; i++)
                if (data[i] != prefix[i]) return false;
            return true;
        }
        private static bool StartsWith(byte[] data, string ascii)
        {
            var bytes = System.Text.Encoding.ASCII.GetBytes(ascii);
            return StartsWith(data, bytes);
        }
        private static bool ContainsAt(byte[] data, string ascii, int offset)
        {
            var bytes = System.Text.Encoding.ASCII.GetBytes(ascii);
            if (data.Length < offset + bytes.Length) return false;
            for (int i = 0; i < bytes.Length; i++)
                if (data[offset + i] != bytes[i]) return false;
            return true;
        }
        private static string TryDetectOfficeOpenXml(byte[] data)
        {
            try
            {
                var ms = new MemoryStream(data, writable: false);
                var archive = new ZipArchive(ms, ZipArchiveMode.Read, leaveOpen: true);
                bool hasDoc = archive.GetEntry("word/document.xml") != null;
                bool hasXls = archive.GetEntry("xl/workbook.xml") != null;
                bool hasPpt = archive.GetEntry("ppt/presentation.xml") != null;
                if (hasDoc) return ".docx";
                if (hasXls) return ".xlsx";
                if (hasPpt) return ".pptx";
            }
            catch
            {
                // not a valid ZIP or not an OOXML doc
            }
            return null;
        }
        class SendMailViewModel
        {
        }
        public class SendMailRootobject
        {
            public List<Message> messages { get; set; }
            public Options options { get; set; }
        }

        public class Options
        {
            public Tracking tracking { get; set; }
            public string clientPriority { get; set; }

        }
        public class Tracking
        {
            public bool track { get; set; }
            public bool trackOpens { get; set; }
            public bool trackClicks { get; set; }

        }
        public class Message
        {
            public string sender { get; set; }
            public List<Destination> destinations { get; set; }
            public Content content { get; set; }

        }
        public class Content
        {
            public string subject { get; set; }
            public string text { get; set; }
            public string html { get; set; }
            public List<MailAttachment> attachments { get; set; }
        }
        public class MailAttachment
        {
            public string type { get; set; }
            public string content { get; set; }
            public string contentType { get; set; }
            public string fileName { get; set; }

        }
        public class Destination
        {
            public List<To> to { get; set; }
            public List<Cc> cc { get; set; }
            public List<Bcc> bcc { get; set; }
            public string replyTo { get; set; }

        }
        public class To
        {
            public string destination { get; set; }
        }
        public class Cc
        {
            public string destination { get; set; }
        }
        public class Bcc
        {
            public string destination { get; set; }
        }
        public class Responceobject
        {
            public string bulkId { get; set; }
            public ResponceMessage[] messages { get; set; }

        }
        public class ResponceMessage
        {
            public string messageId { get; set; }

            public ResponceStatus status { get; set; }

            public string destination { get; set; }

        }
        public class ResponceStatus
        {
            public int groupId { get; set; }
            public string groupName { get; set; }
            public int id { get; set; }
            public string name { get; set; }
            public string description { get; set; }

        }
    }
}