using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
namespace BAL
{
    public static class AesCbcPkcs7
    {
        // Encrypt plain text -> Base64, AES-256-CBC, PKCS7 (PKCS5-compatible)
        public static string EncryptToBase64(string plainText, string keyPlainText, string ivPlainText)
        {
            if (plainText == null) throw new ArgumentNullException(nameof(plainText));
            if (keyPlainText == null) throw new ArgumentNullException(nameof(keyPlainText));
            if (ivPlainText == null) throw new ArgumentNullException(nameof(ivPlainText));

            byte[] key = Encoding.UTF8.GetBytes(keyPlainText);
            byte[] iv = Encoding.UTF8.GetBytes(ivPlainText);

            if (key.Length != 32) throw new ArgumentException("Key must be 32 bytes (256-bit).", nameof(keyPlainText));
            if (iv.Length != 16) throw new ArgumentException("IV must be 16 bytes.", nameof(ivPlainText));

            byte[] plainBytes = new UTF8Encoding(encoderShouldEmitUTF8Identifier: false).GetBytes(plainText);

            using (var aes = Aes.Create())
            {
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;  // PKCS5-compatible for AES
                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Key = key;
                aes.IV = iv;

                using (ICryptoTransform enc = aes.CreateEncryptor())
                {
                    byte[] cipher = enc.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
                    return Convert.ToBase64String(cipher);
                }
            }
        }

        // Decrypt Base64 -> plain text (UTF-8)
        public static string DecryptFromBase64(string base64CipherText, string keyPlainText, string ivPlainText)
        {
            if (base64CipherText == null) throw new ArgumentNullException(nameof(base64CipherText));
            if (keyPlainText == null) throw new ArgumentNullException(nameof(keyPlainText));
            if (ivPlainText == null) throw new ArgumentNullException(nameof(ivPlainText));

            byte[] cipherBytes = Convert.FromBase64String(base64CipherText);
            byte[] key = Encoding.UTF8.GetBytes(keyPlainText);
            byte[] iv = Encoding.UTF8.GetBytes(ivPlainText);

            if (key.Length != 32) throw new ArgumentException("Key must be 32 bytes (256-bit).", nameof(keyPlainText));
            if (iv.Length != 16) throw new ArgumentException("IV must be 16 bytes.", nameof(ivPlainText));

            using (var aes = Aes.Create())
            {
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;  // PKCS5-compatible for AES
                aes.KeySize = 256;
                aes.BlockSize = 128;
                aes.Key = key;
                aes.IV = iv;

                using (ICryptoTransform dec = aes.CreateDecryptor())
                {
                    byte[] plainBytes = dec.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
                    return new UTF8Encoding(false).GetString(plainBytes);
                }
            }
        }

        // (Optional) JSON minifier to remove spaces/newlines/tabs
        public static string MinifyJson(string json)
        {
            if (json == null) return null;
            // lightweight minification (sufficient if you control content)
            return json.Replace("\r", "").Replace("\n", "").Replace("\t", "").Trim();
        }
    }
}
