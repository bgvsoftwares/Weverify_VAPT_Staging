using BAL;
using BAL.CandidateRepository;
using BAL.ClientRepository;
using BAL.ProvidedInfoRepository;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using ViewModels;
using ViewModels.ClientViewModel;
using ViewModels.ProvidedInfoViewModel;
using ViewModels.TempViewModel;
using WebAppBGV.CommonMethods;
using WebAppBGV.Security;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Dynamic;
using DAL;
using System.Drawing;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Net;
using Image = System.Drawing.Image;
using System.IO.Compression;
using static BAL.WhatsAppAPI;
using WebAppBGV.Areas.ClientArea.Models;
using System.Text;
using System.Net.Mime;
using Renci.SshNet;
using Rectangle = iTextSharp.text.Rectangle;
using iTextSharp.text.pdf.security;
using com.itextpdf.text.pdf.security;
using System.Security.Cryptography;
using eSignProject.Helpers;
using Org.BouncyCastle.X509;
using System.Drawing.Imaging;


namespace WebAppBGV.Areas.Candidate.Controllers
{
    //[CustomAuthorizeCandidate]
    public class DownloadController : Controller
    {
        private readonly EsignXmlSigner _esignXmlSigner;
        private const string SignatureFieldName = "Signature1";
        TempPQCandidateCheck tpc = new TempPQCandidateCheck();

        [SessionExpireFilterAttributeCandidate]
        [HttpGet]
        public ActionResult DownloadDocument(string FileName)
        {
            try
            {
                string FldPathDoc = System.Configuration.ConfigurationManager.AppSettings["provideddocumentsAPI"].ToString();
                string base64 = FileSaveDownloadAPI.DocAndReportDownloadDynamic(FileName, FldPathDoc);
                base64 = base64.Trim().Replace("\r", "").Replace("\n", "").Replace(" ", "");
                byte[] fileBytes;
                fileBytes = Convert.FromBase64String(base64);
                return File(fileBytes, MediaTypeNames.Application.Octet, FileName);
                //FldPathDoc = Server.MapPath(FldPathDoc) + FileName;
                //byte[] fileBytes = null;
                //fileBytes = System.IO.File.ReadAllBytes(FldPathDoc);
                //if (fileBytes != null)
                //{
                //    return File(fileBytes, MediaTypeNames.Application.Octet, FileName);
                //}
            }
            catch (Exception ex)
            {
                string str = ex.Message;
            }
            return View();
        }

       


        [HttpPost]
        public ActionResult GetDocs()
        {
            try
            {
                //string sessionId = Request.QueryString["AuthenticationToken"].ToString();
                string responseXml = Request.Unvalidated.Form["esignresp"];

                if (string.IsNullOrEmpty(responseXml))
                {
                    ViewBag.EsignResponse = "Error: 'esignresp' not found in response.";
                    return View();
                }

                ViewBag.EsignResponse = responseXml;
                System.Xml.Linq.XDocument xDoc = System.Xml.Linq.XDocument.Parse(responseXml);

                string txn = xDoc.Root.Attribute("txn")?.Value;

                // Extract your ID
                string[] txnId = txn.Split('_');
                string a = txnId[1].ToString();
                ClientCustomize cc = new ClientCustomize();

                DataTable objDataTable = cc.SP_GetSessionTokenbyCandidateUserRowIDeSign(Convert.ToInt32(a));
                string DBStoredSessionId = string.Empty;
                if (objDataTable.Rows.Count > 0)
                    DBStoredSessionId = objDataTable.Rows[0]["SessionToken"].ToString();
                Session["SessionToken"] = DBStoredSessionId;
                Session["SessionTokenCandidate"] = DBStoredSessionId;
                Session["AuthenticationToken"] = DBStoredSessionId;
                Session["CandidateUserRowID"] = objDataTable.Rows[0]["CandidateUserRowID"].ToString();
                Session["ClientRowID"] = objDataTable.Rows[0]["ClientRowID"].ToString();
                Session["CandidateNameForHeader"] = objDataTable.Rows[0]["CandidateName"].ToString();
                Session["HRApproval"] = objDataTable.Rows[0]["HRApprovalRequired"].ToString();
                Session["TempPersonalRowID"] = objDataTable.Rows[0]["TempPersonalRowID"].ToString();
                Session["LoginCandidateName"] = objDataTable.Rows[0]["CandidateName"].ToString();
                Session["CandidateEmail"] = objDataTable.Rows[0]["CandidateEmail"].ToString();
                Session["ClientUserRowID"] = objDataTable.Rows[0]["ClientUserRowID"].ToString();
                Session["ClientUType"] = "Candidate";
                Session["Valid"] = 1;
                Session["CadidateUserName"] = objDataTable.Rows[0]["UserID"].ToString(); ;
                Session["UserLogin"] = "Candidate User";
                Session["LoginType"] = "";
                int tempPersonalRowID = Convert.ToInt32(objDataTable.Rows[0]["TempPersonalRowID"].ToString());

                if (xDoc.Root.Attribute("status")?.Value != "1")
                {
                    TempData["SigningResult"] = "eSign Error: " + xDoc.Root.Attribute("errMsg")?.Value;
                    return RedirectToAction("Dashboard");
                }

                string pkcs7Base64 = xDoc.Descendants("DocSignature").First().Value.Trim();
                byte[] pkcs7Bytes = Convert.FromBase64String(pkcs7Base64);







                string pdfDir = Server.MapPath("~/eSign/eDocuments");
                string providedpath = System.Configuration.ConfigurationManager.AppSettings["provideddocuments"].ToString();

                string FldPathDoc = System.Configuration.ConfigurationManager.AppSettings["provideddocuments"].ToString();

                FldPathDoc = Server.MapPath(FldPathDoc) + "signed_" + tempPersonalRowID + ".pdf";
                //string tempPdfPath = Path.Combine(pdfDir, "temp.pdf");
                string tempPdfPath = Path.Combine(pdfDir, "temp_" + tempPersonalRowID + ".pdf");
                //string signedPdfPath = Path.Combine(pdfDir, "signed.pdf");
                string signedPdfPath = Path.Combine(pdfDir, "signed_" + tempPersonalRowID + ".pdf");

                //string ltvPdfPath = Path.Combine(pdfDir, "signed_ltv.pdf");
                string ltvPdfPath = Path.Combine(pdfDir, "signed_ltv_" + tempPersonalRowID + ".pdf");




                string backupDir = Path.Combine(pdfDir, "backup");
                Directory.CreateDirectory(backupDir);

                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");

                string tempBackup = Path.Combine(backupDir, $"temp_{timestamp}.pdf");
                System.IO.File.Copy(tempPdfPath, tempBackup, true);


                string embedResult = EmbedPkcs7(tempPdfPath, signedPdfPath, pkcs7Bytes);

                if (!embedResult.Contains("SUCCESS"))
                {
                    TempData["SigningResult"] = embedResult;
                    return RedirectToAction("Dashboard");
                }


                string signedBackup = Path.Combine(backupDir, $"signed_{timestamp}.pdf");
                System.IO.File.Copy(signedPdfPath, signedBackup, true);

                
                string ltvResult = AddLtvValidation(signedPdfPath, ltvPdfPath);

                if (ltvResult.Contains("SUCCESS"))
                {
                    if (System.IO.File.Exists(signedPdfPath))
                    {
                        System.IO.File.Delete(signedPdfPath);
                    }

                    System.IO.File.Move(ltvPdfPath, signedPdfPath);
                    if (System.IO.File.Exists(FldPathDoc))
                    {
                        System.IO.File.Delete(FldPathDoc);
                    }
                    System.IO.File.Move(signedPdfPath, FldPathDoc);







                    cc.proc_InsertUpdateDigitalLOAInTempPQVerifiedUploadCaseDocs(tempPersonalRowID, "signed_" + tempPersonalRowID + ".pdf", FldPathDoc, "Provided", "e Sign document");




                    TempData["SigningResult"] = embedResult + " | " + ltvResult;
                }
                else
                {
                    TempData["SigningResult"] = embedResult + " | " + ltvResult;
                }

                CreateMenuitemforDataEntryOperator(tempPersonalRowID);
                CreateMenuitemforDataEntrySubmit(tempPersonalRowID);
                return RedirectToAction("Digital_ESignLOA", "Home", new { CanName = Session["LoginCandidateName"].ToString() });
                // return View();


            }
            catch (Exception ex)
            {
                ViewBag.EsignResponse = "Error: " + ex.Message;
                return View();
            }
        }






        private void CreateMenuitemforDataEntrySubmit(Int64 id = 0)
        {
            try
            {
                string inEncript = clsCommonMethods.PEncrypt(id.ToString());
                if (id > 0)
                {

                    DataSet d = new DataSet();
                    if (Session["ClientRowID"].ToString() == "679")
                    {
                        string refno = "";
                        DataTable dt = tpc.GetNewCandidateDetails(id);
                        if (dt.Rows.Count > 0)
                        {
                            refno = dt.Rows[0]["CompanyRefNo"].ToString();
                        }
                        if (!string.IsNullOrEmpty(refno))
                        {
                            d = tpc.getTempPQCandidateChecksDT_Fractal(id);
                        }
                        else
                        {
                            d = tpc.GetTempCandidateChecksDT(id);
                        }

                    }
                    else
                    {
                        d = tpc.GetTempCandidateChecksDT(id);
                    }



                    string NavInputDataMenuListsubmit = string.Empty;

                    NavInputDataMenuListsubmit += "<li><a class='final_submit' id='btnFinalSub' href='" + Url.Action("FinalSubmit", "Home", new { id = inEncript }) + "'><span>Final Submission</span><i class='fa fa-check-circle tx-16' aria-hidden='true'></i></a></li>";

                    Session["NavCandidateInputDataMenuListsubmit"] = NavInputDataMenuListsubmit;
                }
            }
            catch (Exception ee)
            {

            }
        }






        [NonAction]
        private string GetPageNameByCheckId(short Id = 0, int Sid = 0)
        {
            string pageName = string.Empty;
            if (Id == 2)
            {
                pageName = "AddAddressInfo";
            }
            if (Id == 5)
            {
                pageName = "AddCriminalInfo";
            }
            if (Id == 11)
            {
                pageName = "AddEducationInfo";
            }
            if (Id == 12)
            {
                pageName = "AddEmploymentInfo";
            }
            if (Id == 26)
            {
                pageName = "AddNationalIdentityInfo";
            }
            if (Id == 30)
            {
                pageName = "AddReferenceInfo";
            }
            if (Id == 40)
            {
                if (Sid == 890)
                {
                    pageName = "AddlivenessInfo";
                }
                else
                {
                    pageName = "AddSpecialCheckInfo";
                }
            }
            return pageName;
        }

        private class MyExternalSignatureContainer : IExternalSignatureContainer
        {
            private readonly byte[] _pkcs7;

            public MyExternalSignatureContainer(byte[] signature)
            {
                _pkcs7 = signature;
            }

            public byte[] Sign(System.IO.Stream data)
            {
                return _pkcs7;
            }

            public void ModifySigningDictionary(PdfDictionary signDic)
            {
                signDic.Put(PdfName.FILTER, PdfName.ADOBE_PPKLITE);
                signDic.Put(PdfName.SUBFILTER, PdfName.ADBE_PKCS7_DETACHED);
            }
        }

        private string ComputeSHA256(byte[] data)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] hashBytes = sha.ComputeHash(data);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLowerInvariant();
            }
        }

        private byte[] GetOcspResponse(X509Certificate cert, X509Certificate issuer)
        {
            try
            {
                string ocspUrl = GetOcspUrlFromCert(cert);

                if (string.IsNullOrEmpty(ocspUrl))
                {
                    System.Diagnostics.Debug.WriteLine("No OCSP URL found for: " + cert.SubjectDN);
                    return null;
                }

                System.Diagnostics.Debug.WriteLine("OCSP URL: " + ocspUrl);
                System.Diagnostics.Debug.WriteLine("Fetching OCSP for: " + cert.SubjectDN);

                OcspClientBouncyCastle ocspClient = new OcspClientBouncyCastle();
                byte[] ocspResponse = ocspClient.GetEncoded(cert, issuer, ocspUrl);

                if (ocspResponse != null && ocspResponse.Length > 0)
                {
                    System.Diagnostics.Debug.WriteLine("OCSP response received: " + ocspResponse.Length + " bytes");
                    return ocspResponse;
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("OCSP response is empty");
                    return null;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("OCSP Error: " + ex.Message);
                return null;
            }
        }

        private string GetOcspUrlFromCert(X509Certificate cert)
        {
            try
            {
                if (cert == null)
                {
                    System.Diagnostics.Debug.WriteLine("Certificate is null.");
                    return null;
                }

                var derOctet = cert.GetExtensionValue(Org.BouncyCastle.Asn1.X509.X509Extensions.AuthorityInfoAccess);
                if (derOctet == null)
                    return null;

                using (var asn1InputStream = new Org.BouncyCastle.Asn1.Asn1InputStream(derOctet.GetOctetStream()))
                {
                    var asn1Object = asn1InputStream.ReadObject();

                    var authorityInfoAccess = Org.BouncyCastle.Asn1.X509.AuthorityInformationAccess.GetInstance(asn1Object);
                    if (authorityInfoAccess == null)
                        return null;

                    var accessDescriptions = authorityInfoAccess.GetAccessDescriptions();

                    foreach (var accessDescription in accessDescriptions)
                    {
                        if (accessDescription.AccessMethod.Equals(Org.BouncyCastle.Asn1.X509.X509ObjectIdentifiers.IdADOcsp))
                        {
                            var generalName = accessDescription.AccessLocation;

                            if (generalName.TagNo == Org.BouncyCastle.Asn1.X509.GeneralName.UniformResourceIdentifier)
                            {
                                var uri = (Org.BouncyCastle.Asn1.DerIA5String)generalName.Name;
                                return uri.GetString();
                            }
                        }
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error extracting OCSP URL: " + ex.Message);
                return null;
            }
        }

        private List<byte[]> GetCrls(X509Certificate cert)
        {
            try
            {
                if (cert == null)
                {
                    System.Diagnostics.Debug.WriteLine("Certificate is null.");
                    return null;
                }

                var crlUrlsList = GetCrlUrlsFromCert(cert);

                if (crlUrlsList == null || crlUrlsList.Count == 0)
                {
                    System.Diagnostics.Debug.WriteLine("No CRL URLs found for: " + cert.SubjectDN);
                    return null;
                }

                System.Diagnostics.Debug.WriteLine("Found " + crlUrlsList.Count + " CRL URL(s) for: " + cert.SubjectDN);

                CrlClientOnline crlClient = new CrlClientOnline();
                List<byte[]> allCrls = new List<byte[]>();

                foreach (string crlUrl in crlUrlsList)
                {
                    try
                    {
                        System.Diagnostics.Debug.WriteLine("Fetching CRL from: " + crlUrl);


                        ICollection<byte[]> crls = crlClient.GetEncoded(cert, crlUrl);

                        if (crls != null && crls.Count > 0)
                        {
                            foreach (var crl in crls)
                            {
                                allCrls.Add(crl);
                            }

                            System.Diagnostics.Debug.WriteLine("CRL received: " + crls.Count + " item(s)");
                        }
                        else
                        {
                            System.Diagnostics.Debug.WriteLine("No CRL data received");
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine("Failed to fetch CRL from " + crlUrl + ": " + ex.Message);
                    }
                }

                return allCrls.Count > 0 ? allCrls : null;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("CRL Error: " + ex.Message);
                return null;
            }
        }

        private List<string> GetCrlUrlsFromCert(X509Certificate cert)
        {
            List<string> crlUrls = new List<string>();

            try
            {
                if (cert == null)
                {
                    System.Diagnostics.Debug.WriteLine("Certificate is null.");
                    return crlUrls;
                }

                var derOctet = cert.GetExtensionValue(Org.BouncyCastle.Asn1.X509.X509Extensions.CrlDistributionPoints);
                if (derOctet == null)
                    return crlUrls;

                using (var asn1InputStream = new Org.BouncyCastle.Asn1.Asn1InputStream(derOctet.GetOctetStream()))
                {
                    var asn1Object = asn1InputStream.ReadObject();

                    var crlDistPoints = Org.BouncyCastle.Asn1.X509.CrlDistPoint.GetInstance(asn1Object);
                    if (crlDistPoints == null)
                        return crlUrls;

                    var distPoints = crlDistPoints.GetDistributionPoints();

                    foreach (var distPoint in distPoints)
                    {
                        var distPointName = distPoint.DistributionPointName;

                        if (distPointName != null &&
                            distPointName.PointType == Org.BouncyCastle.Asn1.X509.DistributionPointName.FullName)
                        {
                            var generalNames = Org.BouncyCastle.Asn1.X509.GeneralNames.GetInstance(distPointName.Name);

                            foreach (var generalName in generalNames.GetNames())
                            {
                                if (generalName.TagNo == Org.BouncyCastle.Asn1.X509.GeneralName.UniformResourceIdentifier)
                                {
                                    string url = ((Org.BouncyCastle.Asn1.DerIA5String)generalName.Name).GetString();

                                    if (!string.IsNullOrEmpty(url) && !crlUrls.Contains(url))
                                    {
                                        crlUrls.Add(url);
                                    }
                                }
                            }
                        }
                    }
                }

                return crlUrls;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Error extracting CRL URLs: " + ex.Message);
                return new List<string>();
            }
        }


        private string AddLtvValidation(string signedPdfPath, string outputPdfPath)
        {
            StringBuilder debugInfo = new StringBuilder();

            try
            {
                using (PdfReader reader = new PdfReader(signedPdfPath))
                {
                    AcroFields fields = reader.AcroFields;
                    List<string> names = fields.GetSignatureNames();

                    if (names.Count == 0)
                        return "No signatures found in PDF.";

                    string signatureName = names[0];
                    debugInfo.AppendLine("Processing signature: " + signatureName);

                    PdfPKCS7 pkcs7 = fields.VerifySignature(signatureName);

                    if (pkcs7 == null)
                        return "Could not extract PKCS7 data from signature.";

                    debugInfo.AppendLine("Signature verified: " + pkcs7.Verify());

                    X509Certificate[] chain = pkcs7.SignCertificateChain;

                    if (chain == null || chain.Length == 0)
                        return "No certificate chain found in signature.";

                    debugInfo.AppendLine("Certificate chain length: " + chain.Length);

                    for (int i = 0; i < chain.Length; i++)
                    {
                        debugInfo.AppendLine("Cert " + i + ": " + chain[i].SubjectDN);
                    }

                    List<byte[]> ocspList = new List<byte[]>();
                    List<byte[]> crlList = new List<byte[]>();

                    
                    for (int i = 0; i < chain.Length - 1; i++)
                    {
                        try
                        {
                            debugInfo.AppendLine("Fetching OCSP for cert " + i);

                            byte[] ocspResponse = GetOcspResponse(chain[i], chain[i + 1]);

                            if (ocspResponse != null && ocspResponse.Length > 0)
                            {
                                ocspList.Add(ocspResponse);
                                debugInfo.AppendLine("OCSP obtained for cert " + i);
                            }
                            else
                            {
                                debugInfo.AppendLine("No OCSP response for cert " + i);
                            }
                        }
                        catch (Exception ex)
                        {
                            debugInfo.AppendLine("OCSP failed for cert " + i + ": " + ex.Message);
                        }
                    }


                    for (int i = 0; i < chain.Length; i++)
                    {
                        try
                        {
                            debugInfo.AppendLine("Fetching CRL for cert " + i);

                            List<byte[]> certCrls = GetCrls(chain[i]);

                            if (certCrls != null && certCrls.Count > 0)
                            {
                                crlList.AddRange(certCrls);
                                debugInfo.AppendLine(certCrls.Count + " CRL(s) obtained for cert " + i);
                            }
                            else
                            {
                                debugInfo.AppendLine("No CRL for cert " + i);
                            }
                        }
                        catch (Exception ex)
                        {
                            debugInfo.AppendLine("CRL failed for cert " + i + ": " + ex.Message);
                        }
                    }

                    using (FileStream os = new FileStream(outputPdfPath, FileMode.Create))
                    {
                        PdfStamper stamper = new PdfStamper(reader, os, '\0', true);
                        LtvVerification ltvVerification = stamper.LtvVerification;

                        if (ocspList.Count > 0 || crlList.Count > 0)
                        {
                            try
                            {
                                List<byte[]> certBytes = new List<byte[]>();
                                foreach (var cert in chain)
                                {
                                    certBytes.Add(cert.GetEncoded());
                                }

                                debugInfo.AppendLine("Adding LTV data...");

                                ltvVerification.AddVerification(
                                    signatureName,
                                    ocspList.Count > 0 ? ocspList : null,
                                    crlList.Count > 0 ? crlList : null,
                                    certBytes
                                );

                                stamper.Close();

                                return "SUCCESS: LTV validation added. OCSP: "
                                    + ocspList.Count + ", CRL: "
                                    + crlList.Count + ", Certs: "
                                    + chain.Length + "\n\nDebug:\n" + debugInfo.ToString();
                            }
                            catch (Exception ex)
                            {
                                stamper.Close();
                                return "Error adding LTV data: " + ex.Message + "\n\nDebug:\n" + debugInfo.ToString();
                            }
                        }
                        else
                        {
                            stamper.Close();
                            return "No OCSP/CRL data fetched.\n\nDebug:\n" + debugInfo.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return "LTV Error: " + ex.Message + "\n\nDebug:\n" + debugInfo.ToString();
            }
        }

        private string EmbedPkcs7(string tempPdf, string outputPdf, byte[] pkcs7Data)
        {
            try
            {
                if (pkcs7Data == null || pkcs7Data.Length == 0)
                    return "ERROR: PKCS7 signature data is missing.";

                if (pkcs7Data.Length < 100)
                    return "ERROR: PKCS7 signature is too small (" + pkcs7Data.Length + " bytes). Likely invalid.";

                using (PdfReader reader = new PdfReader(tempPdf))
                {
                    AcroFields af = reader.AcroFields;
                    var sigNames = af.GetSignatureNames();

                    if (sigNames.Count == 0 || !sigNames.Contains(SignatureFieldName))
                        return "ERROR: Signature field '" + SignatureFieldName + "' not found. Fields: " + string.Join(", ", sigNames);

                    using (FileStream os = new FileStream(outputPdf, FileMode.Create, FileAccess.Write))
                    {
                        IExternalSignatureContainer external = new MyExternalSignatureContainer(pkcs7Data);
                        MakeSignature.SignDeferred(reader, SignatureFieldName, os, external);
                    }




                    if (!System.IO.File.Exists(outputPdf))
                        return "ERROR: Signed PDF file was not created.";

                    FileInfo tempInfo = new FileInfo(tempPdf);
                    FileInfo signedInfo = new FileInfo(outputPdf);

                    byte[] signedPdfBytes = System.IO.File.ReadAllBytes(outputPdf);
                    string signedPdfHash = ComputeSHA256(signedPdfBytes);


                    System.Web.HttpContext.Current.Session["SignedPdfHash"] = signedPdfHash;

                    return "SUCCESS: Signature embedded successfully. Temp: "
                        + tempInfo.Length + " bytes, Signed: "
                        + signedInfo.Length + " bytes, PKCS7: "
                        + pkcs7Data.Length + " bytes";
                }
            }
            catch (Exception ex)
            {
                return "EXCEPTION in EmbedPkcs7: " + ex.Message + " | StackTrace: " + ex.StackTrace;
            }
        }

        private void CreateMenuitemforDataEntryOperator(Int64 id = 0)
        {
            try
            {
                string inEncript = clsCommonMethods.PEncrypt(id.ToString());
                if (id > 0)
                {
                    string refno = "";
                    DataSet d = new DataSet();
                    if (Session["ClientRowID"].ToString() == "679")
                    {
                        DataTable dt = tpc.GetNewCandidateDetails(id);
                        if (dt.Rows.Count > 0)
                        {
                            refno = dt.Rows[0]["CompanyRefNo"].ToString();
                        }
                        if (!string.IsNullOrEmpty(refno))
                        {
                            d = tpc.getTempPQCandidateChecksDT_Fractal(id);
                        }
                        else
                        {
                            d = tpc.GetTempCandidateChecksDT(id);
                        }

                    }
                    else if (Session["ClientRowID"].ToString() == "559")
                    {
                        d = tpc.AutoCheckInitiation_proc(id);
                    }
                    else
                    {
                        d = tpc.GetTempCandidateChecksDT(id);
                    }



                    string NavInputDataMenuList = string.Empty;

                    if (Convert.ToString(Session["ClientRowID"]) != "1606")
                    {
                        if (Convert.ToString(d.Tables[0].Rows[0]["CaseinfoStatus"]) == "Completed")
                            NavInputDataMenuList += "<li class='slide'><a class='side-menu__item side-menu__item_completed' href='" + Url.Action("UpdateCandidateDetails", "Home", new { id = inEncript }) + "'><img class='menu_icon' src='/assets/DashboaredImages/arrow-circle-red.png'/><span class='side-menu__label'>Personal Information</span><i class='fa fa-check-circle text-white' aria-hidden='true'></i></a></li>";
                        else
                            NavInputDataMenuList += "<li class='slide'><a class='side-menu__item' href='" + Url.Action("UpdateCandidateDetails", "Home", new { id = inEncript }) + "'><img class='menu_icon' src='/assets/DashboaredImages/arrow-circle-white.png'/><span class='side-menu__label'>Personal Information</span></a></li>";
                    }

                    int AarryCount = d.Tables[1].Rows.Count;
                    string[] nxtprv = new string[AarryCount];
                    if (d.Tables[1].Rows.Count > 0)
                    {
                        for (int J = 0; J < d.Tables[1].Rows.Count; J++)
                        {
                            string NextSubCheckRowID = "End";
                            string PreviousSubCheckRowID = "Start";
                            string NextCheckFamilyRowID = "";
                            string PreviousFamilyRowID = "";
                            string enPersonalRowID = clsCommonMethods.PEncrypt(d.Tables[1].Rows[J]["PersonalRowID"].ToString());
                            string enClientRowID = clsCommonMethods.PEncrypt(d.Tables[1].Rows[J]["ClientRowID"].ToString());
                            string enCheckFamilyRowID = clsCommonMethods.PEncrypt(d.Tables[1].Rows[J]["CheckFamilyRowID"].ToString());
                            string enSubCheckRowID = clsCommonMethods.PEncrypt(d.Tables[1].Rows[J]["SubCheckFamilyID"].ToString());
                            string urlValuePrevious = "Start";
                            string urlValueNext = "End";

                            if (J < d.Tables[1].Rows.Count - 1)
                            {
                                NextSubCheckRowID = clsCommonMethods.PEncrypt(d.Tables[1].Rows[J + 1]["SubCheckFamilyID"].ToString());
                                NextCheckFamilyRowID = clsCommonMethods.PEncrypt(d.Tables[1].Rows[J + 1]["CheckFamilyRowID"].ToString());
                                if (Convert.ToInt16(d.Tables[1].Rows[J + 1]["SubCheckFamilyID"].ToString()) == 890)
                                {
                                    urlValuePrevious = Url.Content(string.Format("~/Candidate/Home/" + GetPageNameByCheckId(Convert.ToInt16(d.Tables[1].Rows[J + 1]["CheckFamilyRowID"].ToString()), Convert.ToInt16(d.Tables[1].Rows[J + 1]["SubCheckFamilyID"].ToString())) + "?Pid={0}&Sid={1}", enPersonalRowID, NextSubCheckRowID));

                                }
                                else
                                {
                                    urlValueNext = Url.Content(string.Format("~/Candidate/Home/" + GetPageNameByCheckId(Convert.ToInt16(d.Tables[1].Rows[J + 1]["CheckFamilyRowID"].ToString()), Convert.ToInt16(d.Tables[1].Rows[J + 1]["SubCheckFamilyID"].ToString())) + "/client/{0}/candidate/{1}/check/{2}/{3}", enClientRowID, enPersonalRowID, NextCheckFamilyRowID, NextSubCheckRowID));
                                }
                            }
                            if (J > 0)
                            {
                                PreviousSubCheckRowID = clsCommonMethods.PEncrypt(d.Tables[1].Rows[J - 1]["SubCheckFamilyID"].ToString());
                                PreviousFamilyRowID = clsCommonMethods.PEncrypt(d.Tables[1].Rows[J - 1]["CheckFamilyRowID"].ToString());
                                if (Convert.ToInt16(d.Tables[1].Rows[J - 1]["SubCheckFamilyID"].ToString()) == 890)
                                {
                                    urlValuePrevious = Url.Content(string.Format("~/Candidate/Home/" + GetPageNameByCheckId(Convert.ToInt16(d.Tables[1].Rows[J - 1]["CheckFamilyRowID"].ToString()), Convert.ToInt16(d.Tables[1].Rows[J - 1]["SubCheckFamilyID"].ToString())) + "?Pid={0}&Sid={1}", enPersonalRowID, PreviousSubCheckRowID));

                                }
                                else
                                {
                                    urlValuePrevious = Url.Content(string.Format("~/Candidate/Home/" + GetPageNameByCheckId(Convert.ToInt16(d.Tables[1].Rows[J - 1]["CheckFamilyRowID"].ToString()), Convert.ToInt16(d.Tables[1].Rows[J - 1]["SubCheckFamilyID"].ToString())) + "/client/{0}/candidate/{1}/check/{2}/{3}", enClientRowID, enPersonalRowID, PreviousFamilyRowID, PreviousSubCheckRowID));
                                }
                            }

                            if (d.Tables[1].Rows.Count == 1)
                            {
                                NextSubCheckRowID = clsCommonMethods.PEncrypt(d.Tables[1].Rows[J]["SubCheckFamilyID"].ToString());
                                NextCheckFamilyRowID = clsCommonMethods.PEncrypt(d.Tables[1].Rows[J]["CheckFamilyRowID"].ToString());
                                if (Convert.ToInt16(d.Tables[1].Rows[J]["SubCheckFamilyID"].ToString()) == 890)
                                {
                                    urlValueNext = Url.Content(string.Format("~/Candidate/Home/" + GetPageNameByCheckId(Convert.ToInt16(d.Tables[1].Rows[J]["CheckFamilyRowID"].ToString()), Convert.ToInt16(d.Tables[1].Rows[J]["SubCheckFamilyID"].ToString())) + "?Pid={0}&Sid={1}", enPersonalRowID, NextSubCheckRowID));

                                }
                                else
                                {
                                    urlValueNext = Url.Content(string.Format("~/Candidate/Home/" + GetPageNameByCheckId(Convert.ToInt16(d.Tables[1].Rows[J]["CheckFamilyRowID"].ToString()), Convert.ToInt16(d.Tables[1].Rows[J]["SubCheckFamilyID"].ToString())) + "/client/{0}/candidate/{1}/check/{2}/{3}", enClientRowID, enPersonalRowID, NextCheckFamilyRowID, NextSubCheckRowID));

                                }

                            }
                            string urlValue = "";
                            if (Convert.ToInt16(d.Tables[1].Rows[J]["SubCheckFamilyID"].ToString()) == 890)
                            {
                                urlValue = Url.Content(string.Format("~/Candidate/Home/" + GetPageNameByCheckId(Convert.ToInt16(d.Tables[1].Rows[J]["CheckFamilyRowID"].ToString()), Convert.ToInt16(d.Tables[1].Rows[J]["SubCheckFamilyID"].ToString())) + "?Pid={0}&Sid={1}", enPersonalRowID, enSubCheckRowID));

                            }
                            else
                            {
                                urlValue = Url.Content(string.Format("~/Candidate/Home/" + GetPageNameByCheckId(Convert.ToInt16(d.Tables[1].Rows[J]["CheckFamilyRowID"].ToString()), Convert.ToInt16(d.Tables[1].Rows[J]["SubCheckFamilyID"].ToString())) + "/client/{0}/candidate/{1}/check/{2}/{3}", enClientRowID, enPersonalRowID, enCheckFamilyRowID, enSubCheckRowID));

                            }
                            nxtprv[J] = urlValueNext + "|" + urlValuePrevious + "|" + PreviousSubCheckRowID + "|" + NextSubCheckRowID + "|" + enSubCheckRowID;

                            if (Session["ClientRowID"].ToString() == "559")
                            {
                                if (d.Tables[1].Rows[J]["CheckInfoStatus"].ToString() == "Completed" && d.Tables[1].Rows[J]["CheckDocStatus"].ToString() == "Completed")
                                    NavInputDataMenuList += "<li class='slide'><a class='side-menu__item side-menu__item_completed' href='" + urlValue + "'><img class='menu_icon' src='/assets/DashboaredImages/arrow-circle-red.png'/><span class='side-menu__label'>" + d.Tables[1].Rows[J]["CheckName"] + " </span><i class='fa fa-check-circle text-white' aria-hidden='true'></i></a></li>";
                                else
                                    NavInputDataMenuList += "<li class='slide'><p class='side-menu__item' href=''><img class='menu_icon' src='/assets/DashboaredImages/arrow-circle-white.png'/><span class='side-menu__label'>" + d.Tables[1].Rows[J]["CheckName"].ToString() + " </span></p></li>";
                            }
                            else
                            {
                                if (d.Tables[1].Rows[J]["CheckInfoStatus"].ToString() == "Completed" && d.Tables[1].Rows[J]["CheckDocStatus"].ToString() == "Completed")
                                    NavInputDataMenuList += "<li class='slide'><a class='side-menu__item side-menu__item_completed' href='" + urlValue + "'><img class='menu_icon' src='/assets/DashboaredImages/arrow-circle-red.png'/><span class='side-menu__label'>" + d.Tables[1].Rows[J]["CheckNameInCandidatePortal"] + " </span><i class='fa fa-check-circle text-white' aria-hidden='true'></i></a></li>";
                                else
                                    NavInputDataMenuList += "<li class='slide'><a class='side-menu__item' href='" + urlValue + "'><img class='menu_icon' src='/assets/DashboaredImages/arrow-circle-white.png'/><span class='side-menu__label'>" + d.Tables[1].Rows[J]["CheckNameInCandidatePortal"].ToString() + " </span></a></li>";
                            }
                        }
                    }

                    if (Session["ClientRowID"].ToString() == "559")
                    {
                        if (Convert.ToString(d.Tables[0].Rows[0]["LOA_Name"]) != "")
                            NavInputDataMenuList += "<li class='slide'><a class='side-menu__item side-menu__item_completed' href='" + Url.Action("Digital_LOA", "Home", new { id = inEncript }) + "'><img class='menu_icon' src='/assets/DashboaredImages/arrow-circle-red.png'/><span class='side-menu__label'>Digital (LOA)</span><i class='fa fa-check-circle text-white' aria-hidden='true'></i></a></li>";
                        else
                            NavInputDataMenuList += "<li class='slide'><a class='side-menu__item' href='" + Url.Action("Digital_LOA", "Home", new { id = inEncript }) + "'><img class='menu_icon' src='/assets/DashboaredImages/arrow-circle-white.png'/><span class='side-menu__label'>Digital (LOA)</span></a></li>";
                    }
                    else
                    {
                        if (refno == "")
                        {
                            if (Session["ClientRowID"].ToString() != "267")
                            {
                                if (Convert.ToString(d.Tables[0].Rows[0]["LOA_Name"]) != "")
                                    NavInputDataMenuList += "<li class='slide'><a class='side-menu__item side-menu__item_completed' href='" + Url.Action("Digital_LOA", "Home", new { id = inEncript }) + "'><img class='menu_icon' src='/assets/DashboaredImages/arrow-circle-red.png'/><span class='side-menu__label'>Digital (LOA)</span><i class='fa fa-check-circle text-white' aria-hidden='true'></i></p></li>";
                                else
                                    NavInputDataMenuList += "<li class='slide'><a class='side-menu__item' href='" + Url.Action("Digital_LOA", "Home", new { id = inEncript }) + "'><img class='menu_icon' src='/assets/DashboaredImages/arrow-circle-white.png'/><span class='side-menu__label'>Digital (LOA)</span></p></li>";
                            }
                        }
                    }
                    //NavInputDataMenuList += "<li class='slide'><a class='side-menu__item final_submit' href='" + Url.Action("FinalSubmit", "Home", new { id = inEncript }) + "'><b id='btnFinalSub'; class='submit-btn'>Final Submission</b></a></li>";

                    if (Session["ClientRowID"].ToString() == "267")
                    {

                        //NavInputDataMenuList += "<li><a href='" + Url.Action("eSigns", "Home", new { AuthType = "1" }) + "'><span class='glyphicon glyphicon-user'></span>  eSign By OTP </a></li>";
                        NavInputDataMenuList += "<li class='slide'><a class='side-menu__item' href='" + Url.Action("eSigns", "Home", new { AuthType = "1" }) + "'><img class='menu_icon' src='/assets/DashboaredImages/arrow-circle-white.png'/><span class='side-menu__label'>eSign via AAdhar</span></p></li>";
                        NavInputDataMenuList += "<li class='slide'><a class='side-menu__item' href='" + Url.Action("eSigns", "Home", new { AuthType = "2" }) + "'><img class='menu_icon' src='/assets/DashboaredImages/arrow-circle-white.png'/><span class='side-menu__label'>eSign By Bio Matric</span></p></li>";
                        if (refno == "")
                        {
                            if (Convert.ToString(d.Tables[0].Rows[0]["LOA_Name"]) != "")
                                NavInputDataMenuList += "<li class='slide'><a class='side-menu__item side-menu__item_completed' href='" + Url.Action("Digital_ESignLOA", "Home", new { id = inEncript }) + "'><img class='menu_icon' src='/assets/DashboaredImages/arrow-circle-red.png'/><span class='side-menu__label'>Digital eSign (LOA)</span><i class='fa fa-check-circle text-white' aria-hidden='true'></i></p></li>";
                            else
                                NavInputDataMenuList += "<li class='slide'><a class='side-menu__item' href='" + Url.Action("Digital_ESignLOA", "Home", new { id = inEncript }) + "'><img class='menu_icon' src='/assets/DashboaredImages/arrow-circle-white.png'/><span class='side-menu__label'>Digital eSign (LOA)</span></p></li>";
                        }
                    }

                    Session["NavCandidateInputDataMenuList"] = NavInputDataMenuList;
                    Session["NextPreviousButtonsURL"] = nxtprv;
                }
            }
            catch (Exception ee)
            {

            }
        }
    }
}