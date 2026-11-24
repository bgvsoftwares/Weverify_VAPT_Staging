using System.Text;
using System.Security.Cryptography;
using System.Collections.Generic;
using System;

namespace ViewModels.refinitiv
{
    class AuthHeadersBuilder
    {
        /// <summary>
        /// Scheme to be used in `Authorization' header
        /// </summary>
        public static string AUTHENTICATION_SCHEME = "Signature";

        private static string CONTENT_TYPE_PREFIX = "application/json";

        /// <summary>
        /// Generates required request headers
        /// </summary>
        /// <param name="apiKey">User's api key</param>
        /// <param name="apiSecret">User's api secret</param>
        /// <param name="method">Http method of the request in lowercase</param>
        /// <param name="uri">Uri request is sent to</param>
        /// <returns>Dictionary containing required headers</returns>
        public static Dictionary<string, string> GenerateAuthHeaders(
            string apiKey,
            string apiSecret,
            string method,
            Uri uri)
        {
            return GenerateAuthHeaders(apiKey, apiSecret, method, uri, null, null);
        }

        /// <summary>
        /// Generates required request headers
        /// </summary>
        /// <param name="apiKey">User's api key</param>
        /// <param name="apiSecret">User's api secret</param>
        /// <param name="method">Http method of the request in lowercase</param>
        /// <param name="uri">Uri request is sent to</param>
        /// <param name="contentType">
        /// Indicates media type of the content.
        /// Should be the same value as 'Content-Type' header
        /// </param>
        /// <param name="content">Content, sent in the request. Should be the same as body of the request</param>
        /// <returns>Dictionary containing required headers</returns>
        public static Dictionary<string, string> GenerateAuthHeaders(
            string apiKey,
            string apiSecret,
            string method,
            Uri uri,
            string contentType,
            string content)
        {
            ValidateContentType(contentType, content);

            Dictionary<string, string> headers = new Dictionary<string, string>();
            DateTime dateValue = DateTime.UtcNow;
            string date = dateValue.ToString("R");
            headers.Add("Date", date);

            string authorization = GenerateAuthHeaderValue(apiKey, apiSecret, method, uri, contentType, content, date);
            headers.Add("Authorization", authorization);
            return headers;
        }

        private static string GenerateAuthHeaderValue(
            string apiKey,
            string apiSecret,
            string method,
            Uri uri,
            string contentType,
            string content,
            string date)
        {
            string dataToSign = CreateDataToSign(
                method,
                uri,
                contentType,
                content,
                date);

            string hmac = GenerateHmac(dataToSign, apiSecret);
            string headers = content == null ? "host date" : "host date content-type content-length";

            return CreateAuthHeader(apiKey, headers, hmac);
        }

        private static string CreateDataToSign(
            string method,
            Uri uri,
            string contentType,
            string content,
            string date)
        {
            string dataToSign = "(request-target): " + method + " " + uri.LocalPath + "\n" +
                                "host: " + uri.Host + "\n" +
                                "date: " + date;
            if(content != null)
            {
                int contentLength = Encoding.UTF8.GetByteCount(content);
                dataToSign = dataToSign + "\n" +
                    "content-type: " + contentType + "\n" +
                    "content-length: " + contentLength + "\n" +
                    content;
            }
            return dataToSign;
        }

        private static string GenerateHmac(string dataToSign, string apiSecret)
        {
            byte[] secretKey = Encoding.UTF8.GetBytes(apiSecret);
            HMACSHA256 hmac = new HMACSHA256(secretKey);
            hmac.Initialize();

            byte[] bytes = Encoding.UTF8.GetBytes(dataToSign);
            byte[] rawHmac = hmac.ComputeHash(bytes);

            return Convert.ToBase64String(rawHmac);
        }

        private static string CreateAuthHeader(string apiKey, string headers, string hmac)
        {
            return "Signature keyId=\"" + apiKey + "\",algorithm=\"hmac-sha256\",headers=\"(request-target) " + headers + "\",signature=\"" + hmac + "\"";
        }

        private static void ValidateContentType(string contentType, string content)
        {
            if (contentType != null && !contentType.StartsWith(CONTENT_TYPE_PREFIX))
            {
                throw new ArgumentException("Unsupported content type " + contentType);
            }

            if (contentType != null && content == null)
            {
                throw new ArgumentException("The request payload(body) has not been provided");
            }

            if (contentType == null && content != null)
            {
                throw new ArgumentException("The content type of request payload(body) has not been provided");
            }
        }
    }
}
