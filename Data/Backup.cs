using System.Collections.Generic;
using System.IO;
using MarkoutBackupViewer.Common;

namespace MarkoutBackupViewer.Data
{
    /// <summary>
    /// бекап
    /// </summary>
    public class Backup
    {
        /// <summary>
        /// конструктор
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="content"></param>
        public Backup(string filePath, ContentStorage content)
        {
            FilePath = filePath;
            using (var stream = new MemoryStream(filePath.ReadAllBytes()))
                Table = new Table(stream);
            Content = content;
            // подгрузим документы
            foreach (var row in Table)
                Documents[row.ID] = new Document(this, row);
        }

        /// <summary>
        /// путь к файлу
        /// </summary>
        public readonly string FilePath;

        /// <summary>
        /// хранилище бинарного содержимого
        /// </summary>
        public readonly ContentStorage Content;

        /// <summary>
        /// таблица с данными
        /// </summary>
        public readonly Table Table;

        /// <summary>
        /// подгруженные документы
        /// </summary>
        private readonly Dictionary<string,Document> Documents = new Dictionary<string, Document>();

        /// <summary>
        /// получить документ с указанным идентификатором
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Document this[string id]
        {
            get { return Documents.GetValue(id); }
        }

        /// <summary>
        /// корневой документ
        /// </summary>
        public Document RootDocument
        {
            get { return this["Root"]; }
        }

        /// <summary>
        /// получить свойство блокнота
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual string GetProperty(string name)
        {
            var row = Table["Properties"];
            if (row == null)
                return null;
            return row[name];
        }

        /// <summary>
        /// хеш-код пароля шифрации для его проверки
        /// </summary>
        public virtual string PasswordHash
        {
            get { return GetProperty("PasswordHash"); }
        }

        /// <summary>
        /// пароль дешифрации
        /// </summary>
        public string Password
        {
            set { Crypto = new Crypto(value); }
        }

        /// <summary>
        /// шифратор
        /// </summary>
        private Crypto Crypto;

        /// <summary>
        /// расшифровка указанного текста
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public string Decrypt(string text)
        {
            if (Crypto == null)
                return text;
            return Crypto.Decrypt(text);
        }

        /// <summary>
        /// расшифровка указанных данных
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public byte[] Decrypt(byte[] data)
        {
            if (Crypto == null)
                return data;
            return Crypto.Decrypt(data);
        }
    }
}
