using System;
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
        public Backup(string filePath)
        {
            FilePath = filePath;
            using (var stream = File.OpenRead(filePath))
            {
                // сигнатура
                if (stream.ReadString() != "Markout Backup")
                    throw new NotSupportedException("Invalid backup file signature");
                // версия
                if (stream.ReadInt32() != 1)
                    throw new NotSupportedException("Unsupported backup file version");
                // таблица документов
                using (var memoryStream = new MemoryStream(stream.ReadBlob()))
                    Table = new Table(memoryStream);
                // содержимое
                while (stream.ReadBool())
                {
                    var key = stream.ReadString();
                    var content = stream.ReadBlob();
                    Content[key] = content;
                }
            }
            // подгрузим документы
            foreach (var row in Table)
                Documents[row.ID] = new Document(this, row);
        }

        /// <summary>
        /// путь к файлу
        /// </summary>
        public readonly string FilePath;

        /// <summary>
        /// бинарное содержимое
        /// </summary>
        private readonly Dictionary<string,byte[]> Content = new Dictionary<string, byte[]>();

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

        /// <summary>
        /// получить бинарное содержимое
        /// </summary>
        /// <param name="key">ключ</param>
        /// <returns>содержимое</returns>
        public byte[] GetContent(string key)
        {
            // для начала попробуем декодировать из ключа
            byte[] content;
            if (key.TryDecodeKey(out content))
                return content;
            return Content.GetValue(key);
        }
    }
}
