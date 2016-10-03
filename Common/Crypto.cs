using System;
using System.Security.Cryptography;
using System.Text;

namespace MarkoutBackupViewer.Common
{
    /// <summary>
    /// шифратор
    /// </summary>
    public class Crypto
    {
        /// <summary>
        /// конструктор
        /// </summary>
        /// <param name="password">пароль</param>
        public Crypto(string password)
        {
            // Пустой пароль - отключение шифрации
            if (!password.HasValue())
                return;
            // создадим шифровщик и расщифровщик
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
        
        /// <summary>
        /// расшифровщик
        /// </summary>
        private readonly ICryptoTransform Decryptor;

        /// <summary>
        /// дешифрация блока
        /// </summary>
        /// <param name="data">данные</param>
        /// <returns></returns>
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

        /// <summary>
        /// шифрация строки
        /// </summary>
        /// <param name="text">строка</param>
        /// <returns></returns>
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
