using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MarkoutBackupViewer.Data.Versions;

namespace MarkoutBackupViewer.Data
{
    public class Document : IEnumerable<KeyValuePair<string,string>>
    {
        public Document(Backup backup, Row row)
        {
            Backup = backup;
            Row = row;
            TextVersions = new DocumentPropertyVersions<DocumentTextVersion>(this, "Text");
            ContentVersions = new DocumentPropertyVersions<DocumentContentVersion>(this, "Content");
        }

        public readonly Backup Backup;

        private readonly Row Row;

        public string this[string key]
        {
            get { return Row[key]; }
        }

        public string ID
        {
            get { return Row.ID; }
        }

        public string ShortID
        {
            get { return this["ShortID"]; }
        }

        public string Type
        {
            get { return this["Type"] ?? "Note"; }
        }

        public static class Types
        {
            public const string Root = "Root";
            
            public const string Note = "Note";

            public const string Folder = "Folder";

            public const string Image = "Image";

            public const string File = "File";

            public const string Text = "Text";
            
            public const string Html = "Html";
        }

        public bool IsBinary
        {
            get { return Type == Types.Image || Type == Types.File; }
        }

        public string Name
        {
            get
            {
                if (Type == Types.Root)
                    return Backup.FilePath;
                return Backup.Decrypt(this["Name"]);
            }
        }

        public string Icon
        {
            get { return this["Icon"]; }
        }

        public DateTime Created
        {
            get { return DateTime.FromBinary(this["Created"].ToInt64()); }
        }

        public DateTime Modified
        {
            get
            {
                if (!this["Modified"].HasValue())
                    return Created;
                return DateTime.FromBinary(this["Modified"].ToInt64());
            }
        }

        public readonly DocumentPropertyVersions<DocumentTextVersion> TextVersions;

        public string Text
        {
            get { return TextVersions.Current != null ? TextVersions.Current.Text : Backup.Decrypt(this["Text"]); }
        }

        public readonly DocumentPropertyVersions<DocumentContentVersion> ContentVersions;

        public byte[] Content
        {
            get { return ContentKey.HasValue() ? Backup.GetContent(ContentKey) : null; }
        }

        public string ContentKey
        {
            get { return ContentVersions.Current != null ? ContentVersions.Current.Key : this["ContentKey"]; }
        }

        public int ContentSize
        {
            get { return ContentVersions.Current != null ? ContentVersions.Current.Size : this["ContentLength"].ToInt32(); }
        }

        public string[] Tags
        {
            get { return (this["Tags"] ?? "").Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray(); }
        }

        public Document Parent
        {
            get { return Backup[this["Parent"]]; }
        }

        public IEnumerable<Document> Children
        {
            get
            {
                foreach (var id in Backup.Table.SelectParent(ID))
                    yield return Backup[id];
            }
        }

        public int SortOrder
        {
            get
            {
                // папки всегда чуть выше
                if (Type == Types.Folder)
                    return (this["SortOrder"] ?? "1").ToInt32();
                return this["SortOrder"].ToInt32();
            }
        }

        public int CompareTo(Document document)
        {
            var compare = SortOrder.CompareTo(document.SortOrder);
            if (compare != 0)
                return compare;
            compare = Name.NaturalCompareTo(document.Name);
            if (compare != 0)
                return compare;
            return (ShortID ?? "").NaturalCompareTo(document.ShortID ?? "");
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return Row.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
