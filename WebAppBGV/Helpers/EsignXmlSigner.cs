using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;

namespace eSignProject.Helpers
{
    public class EsignXmlSigner
    {

        
        public string GenerateEsignXml(string pdfHashHex ,string CandUserId, string authToken, string authType="1" )
        {
            string version = "2.1";
            string sc = "Y";
            string ts = DateTime.Now.ToString("yyyy-MM-ddTHH:mm:ss");
            string txn = "UKC:public:" + Guid.NewGuid().ToString("N").Substring(0, 8);
            string aspId = "walsonsaspiduat";

            string xml =
    @"<Esign ver=""" + version + @""" sc=""" + sc + @""" ts=""" + ts + @""" txn=""" + authToken + "_" + CandUserId +"_" + txn + @""" ekycId="""" ekycIdType=""A"" aspId=""" + aspId + @""" AuthMode="""+authType+ @""" responseSigType=""PKCS7pdf"" responseUrl=""https://bgv.securitas-india.com/Candidate/Download/GetDocs"">
  <Docs>
    <InputHash id=""1"" hashAlgorithm=""SHA256"" docInfo=""eSign Document"">" + pdfHashHex + @"</InputHash>
  </Docs>
</Esign>";

            //            string xml =
            //@"<Esign ver=""" + version + @""" sc=""" + sc + @""" ts=""" + ts + @""" txn=""" + CandUserId + "_" + txn + @""" ekycId="""" ekycIdType=""A"" aspId=""" + aspId + @""" AuthMode=""" + authType + @""" responseSigType=""PKCS7pdf"" responseUrl=""https://bgv.securitas-india.com/Candidate/Home/GetDocs"">
            //  <Docs>
            //    <InputHash id=""1"" hashAlgorithm=""SHA256"" docInfo=""eSign Document"">" + pdfHashHex + @"</InputHash>
            //  </Docs>
            //</Esign>";

            // ✅ MVC 5 Session
            System.Web.HttpContext.Current.Session["XMLRequest"] = xml;
            System.Web.HttpContext.Current.Session["AuthenticationToken"] = authToken;


            return xml;
        }

        public static string SignEsignXml(string xmlInput, string pfxPath, string password)
        {
            X509Certificate2 cert = new X509Certificate2(
                pfxPath,
                password,
                X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.Exportable
            );

            XmlDocument doc = new XmlDocument();
            doc.PreserveWhitespace = true;
            doc.LoadXml(xmlInput);

            SignedXml sx = new SignedXml(doc)
            {
                SigningKey = cert.PrivateKey   // ✅ MVC 5 compatible
            };

            // ✅ Reference (whole document)
            Reference r = new Reference();
            r.Uri = "";

            r.AddTransform(new XmlDsigEnvelopedSignatureTransform());
            r.AddTransform(new XmlDsigC14NTransform());

            r.DigestMethod = "http://www.w3.org/2001/04/xmlenc#sha256";

            sx.AddReference(r);

            // ✅ Signature Method SHA256
            sx.SignedInfo.SignatureMethod = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256";


            // ✅ KeyInfo
            KeyInfo ki = new KeyInfo();

            KeyInfoX509Data x509Data = new KeyInfoX509Data(cert);
            x509Data.AddSubjectName(cert.Subject);
            x509Data.AddIssuerSerial(cert.Issuer, cert.SerialNumber);

            ki.AddClause(x509Data);
            sx.KeyInfo = ki;

            // ✅ Generate Signature
            sx.ComputeSignature();

            XmlElement xmlSig = sx.GetXml();
            doc.DocumentElement.AppendChild(doc.ImportNode(xmlSig, true));

            return doc.OuterXml;
        }
    }

}
