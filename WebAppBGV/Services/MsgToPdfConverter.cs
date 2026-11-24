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

    public static class MsgToPdfConverter
    {

        public static byte[] Convert(string msgFilePath)
        {
            Application outlookApp = new Application();
            MailItem mail = (MailItem)outlookApp.Session.OpenSharedItem(msgFilePath);

            string subject = mail.Subject ?? "";
            string from = "";
            if (mail.Sender != null)
            {
                try
                {
                    var exchUser = mail.Sender.GetExchangeUser();
                    if (exchUser != null && !string.IsNullOrEmpty(exchUser.PrimarySmtpAddress))
                    {
                        from = mail.SenderName + " <" + exchUser.PrimarySmtpAddress + ">";
                    }
                    else
                    {
                        from = mail.SenderName + " <" + mail.SenderEmailAddress + ">";
                    }
                }
                catch
                {
                    from = mail.SenderName + " <" + mail.SenderEmailAddress + ">";
                }
            }

            string to = mail.To ?? "";
            string cc = mail.CC ?? "";
            string sentOn = mail.SentOn.ToString();
            string bodyHtml = !string.IsNullOrEmpty(mail.HTMLBody) ? mail.HTMLBody : mail.Body;

            // Clean HTML
            bodyHtml = CleanHtmlBody(bodyHtml);

            string htmlContent = $@"
<!DOCTYPE html>
<html xmlns='http://www.w3.org/1999/xhtml'>
<head>
    <meta http-equiv='Content-Type' content='text/html; charset=UTF-8' />
    <style type='text/css'>
        body {{ font-family: Arial, sans-serif; font-size: 12pt; }}
        h1 {{ font-size: 16pt; }}
        .header {{ margin-bottom: 20px; }}
        .label {{ font-weight: bold; }}
    </style>
</head>
<body>
    <div class='header'>
        <h1>{System.Web.HttpUtility.HtmlEncode(subject)}</h1>
        <p><span class='label'>From:</span> {System.Web.HttpUtility.HtmlEncode(from)}</p>
        <p><span class='label'>To:</span> {System.Web.HttpUtility.HtmlEncode(to)}</p>
        <p><span class='label'>CC:</span> {System.Web.HttpUtility.HtmlEncode(cc)}</p>
        <p><span class='label'>Date:</span> {System.Web.HttpUtility.HtmlEncode(sentOn)}</p>
    </div>
    <div class='body'>{bodyHtml}</div>
</body>
</html>";

            using (MemoryStream output = new MemoryStream())
            {
                using (var sr = new StringReader(htmlContent))
                {
                    using (Document doc = new Document(PageSize.A4, 50, 50, 50, 50))
                    {
                        PdfWriter writer = PdfWriter.GetInstance(doc, output);
                        doc.Open();
                        XMLWorkerHelper.GetInstance().ParseXHtml(writer, doc, sr);
                        doc.Close();
                    }
                }
                return output.ToArray();
            }
        }


        private static string CleanHtmlBody(string html)
        {
            if (string.IsNullOrWhiteSpace(html)) return "";

            var doc = new HtmlAgilityPack.HtmlDocument
            {
                OptionFixNestedTags = true,   // nested tags
                OptionAutoCloseOnEnd = true,  // unclosed tags
                OptionWriteEmptyNodes = true  
            };

            doc.LoadHtml(html);
                        
            var unwanted = doc.DocumentNode.SelectNodes("//script|//style");
            if (unwanted != null)
            {
                foreach (var node in unwanted)
                    node.Remove();
            }
                        
            var bodyNode = doc.DocumentNode.SelectSingleNode("//body");
            string safeHtml = bodyNode != null ? bodyNode.InnerHtml : doc.DocumentNode.InnerHtml;
                        
            safeHtml = safeHtml.Replace("<br>", "<br/>")
                               .Replace("<hr>", "<hr/>");
            
            safeHtml = System.Text.RegularExpressions.Regex.Replace(
                safeHtml, @"<img([^>/]*)>", "<img$1/>", System.Text.RegularExpressions.RegexOptions.IgnoreCase
            );

            return safeHtml;
        }


    }


}