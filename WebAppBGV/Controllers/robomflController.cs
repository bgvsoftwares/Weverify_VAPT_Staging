using BAL;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace WebAppBGV.Controllers
{
    public class robomflController : Controller
    {
        
        public FileResult wrbmfl(string id="")
        {
            string encString = Encryption_Heptagon("https://bgv.securitas-india.com/robomfl/wrbmfl?id=8F22C7F2-C1C0-4161-9260-AEECF8AF2A71");
            //string decString = Decrypt_Heptagon(encString);            
            try
           {
                var config = System.Configuration.ConfigurationManager.AppSettings;
                string username = "";
                string pwd = "";
                if (config.Count > 0)
                {
                     username = config["username"];
                    pwd = config["pwd"];

                }
                string filename=Request.QueryString["id"].ToString();
                WebClient request = new WebClient();
                string url = "ftp://10.83.0.75/Report_File_API/" + filename +".pdf";
                request.Credentials = new NetworkCredential(username, pwd);

                byte[] fileBytes = request.DownloadData(url);
                //byte[] fileBytes = System.IO.File.ReadAllBytes(@"F:\Report\10FA-01-02-2021-94.pdf");
                string fileName = filename + ".pdf";
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            catch
            {
                return null;
            };

        }

        private string Encryption_Heptagon(string s1)
        {
            var key = "aFT5J4xU7P6vmvJUUygqd6KUeGKBuv28";
            var iv = "Xwizyqm36ihpjdfr";
            var plainText = s1;

            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException("key");

            byte[] encrypted;
            using (AesManaged aes = new AesManaged())
            {
                ICryptoTransform encryptor = aes.CreateEncryptor(Encoding.UTF8.GetBytes(key), Encoding.UTF8.GetBytes(iv));
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter sw = new StreamWriter(cs))
                            sw.Write(plainText);
                        encrypted = ms.ToArray();
                    }
                }
            }
            return Convert.ToBase64String(encrypted);
        }

        private string Decrypt_Heptagon(string s2)
        {
            var key = "aFT5J4xU7P6vmvJUUygqd6KUeGKBuv28";
            var iv = "Xwizyqm36ihpjdfr";
            var cipherText = s2;

            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException("key");

            string plaintext = null;
            using (AesManaged aes = new AesManaged())
            {
                ICryptoTransform decryptor = aes.CreateDecryptor(Encoding.UTF8.GetBytes(key), Encoding.UTF8.GetBytes(iv));
                using (MemoryStream ms = new MemoryStream(Convert.FromBase64String(cipherText)))
                {
                    using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader reader = new StreamReader(cs))
                            plaintext = reader.ReadToEnd();
                    }
                }
            }
            return plaintext;


        }
    }
}