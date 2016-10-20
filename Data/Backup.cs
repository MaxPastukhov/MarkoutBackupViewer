using System;
using System.Collections.Generic;
using System.IO;
using MarkoutBackupViewer.Common;

namespace MarkoutBackupViewer.Data
{
    public class Backup
    {
        public Backup(string filePath)
        {
            FilePath = filePath;
            using (var stream = File.OpenRead(filePath))
            {
                if (stream.ReadString() != "Markout Backup")
                    throw new NotSupportedException("Invalid backup file signature");
                if (stream.ReadInt32() != 1)
                    throw new NotSupportedException("Unsupported backup file version");
                using (var memoryStream = new MemoryStream(stream.ReadBlob()))
                    Table = new Table(memoryStream);
                while (stream.ReadBool())
                {
                    var key = stream.ReadString();
                    var content = stream.ReadBlob();
                    Content[key] = content;
                }
            }
            foreach (var row in Table)
                Documents[row.ID] = new Document(this, row);
        }

        public readonly string FilePath;

        private readonly Dictionary<string,byte[]> Content = new Dictionary<string, byte[]>();

        public readonly Table Table;

        private readonly Dictionary<string,Document> Documents = new Dictionary<string, Document>();

        public Document this[string id]
        {
            get { return Documents.GetValue(id); }
        }

        public Document RootDocument
        {
            get { return this["Root"]; }
        }

        public virtual string GetProperty(string name)
        {
            var row = Table["Properties"];
            if (row == null)
                return null;
            return row[name];
        }

        public virtual string PasswordHash
        {
            get { return GetProperty("PasswordHash"); }
        }

        public string Password
        {
            set { Crypto = new Crypto(value); }
        }

        private Crypto Crypto;

        public string Decrypt(string text)
        {
            if (Crypto == null)
                return text;
            return Crypto.Decrypt(text);
        }

        public byte[] Decrypt(byte[] data)
        {
            if (Crypto == null)
                return data;
            return Crypto.Decrypt(data);
        }

        public byte[] GetContent(string key)
        {
            byte[] content;
            if (key.TryDecodeKey(out content))
                return content;
            return Content.GetValue(key);
        }
    }
}
