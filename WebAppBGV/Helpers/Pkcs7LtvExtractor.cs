using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Cms;
using Org.BouncyCastle.X509;
using Org.BouncyCastle.X509.Store;

namespace WebAppBGV.Helpers
{

    public class Pkcs7LtvExtractor
    {
        public static LtvExtractionResult Extract(byte[] pkcs7Bytes)
        {
            List<byte[]> ocspResponses = new List<byte[]>();
            List<byte[]> crlResponses = new List<byte[]>();

            CmsSignedData cms = new CmsSignedData(pkcs7Bytes);

            // ✅ CRL extraction (SUPPORTED)
            IX509Store crlStore = cms.GetCrls("Collection");
            if (crlStore != null)
            {
                System.Collections.ICollection matches = crlStore.GetMatches(null);

                foreach (object obj in matches)
                {
                    X509Crl crl = (X509Crl)obj;
                    crlResponses.Add(crl.GetEncoded());
                }
            }

            // ❌ OCSP extraction NOT supported directly
            // Will remain empty unless manually parsed or fetched externally

            LtvExtractionResult result = new LtvExtractionResult();
            result.Ocsp = ocspResponses;
            result.Crl = crlResponses;

            return result;
        }


        public class LtvExtractionResult
        {
            public List<byte[]> Ocsp { get; set; }
            public List<byte[]> Crl { get; set; }

            public LtvExtractionResult()
            {
                Ocsp = new List<byte[]>();
                Crl = new List<byte[]>();
            }
        }

    }
}