using System;
using System.Security.Cryptography;
using System.Text;

namespace MarkoutBackupViewer.Common
{
    public class Crypto
    {
        public Crypto(string password)
        {
            if (!password.HasValue())
                return;
            MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
            byte[] keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(password));
            TripleDESCryptoServiceProvider cryptoProvider = new TripleDESCryptoServiceProvider
            {
                Key = keyArray,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            Decryptor = cryptoProvider.CreateDecryptor();
        }
        
        private readonly ICryptoTransform Decryptor;

        public byte[] Decrypt(byte[] data)
        {
            if (data == null)
                return null;
            if (Decryptor == null)
                return data;
            try
            {
                return Decryptor.TransformFinalBlock(data, 0, data.Length);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string Decrypt(string text)
        {
            try
            {
                if (text == null)
                    return null;
                if (Decryptor == null)
                    return text;
                var data = Convert.FromBase64String(text);
                return Encoding.UTF8.GetString(Decryptor.TransformFinalBlock(data, 0, data.Length));
            }
            catch (Exception)
            {
                return "Can't decrypt";
            }
        }
    }
}
