using System;
using System.Collections;
using System.Collections.Generic;

namespace MarkoutBackupViewer.Data.Versions
{
    public class DocumentPropertyVersions<T> : IEnumerable<T>
        where T : DocumentPropertyVersions<T>.Version
    {
        public DocumentPropertyVersions(Document document, string property)
        {
            Document = document;
            Property = property;
        }

        private readonly Document Document;

        private readonly string Property;

        private readonly Dictionary<string, T> Versions = new Dictionary<string, T>();

        public T this[string id]
        {
            get
            {
                var version = Versions.GetValue(id);
                if (version == null)
                {
                    version = Versions[id] = Activator.CreateInstance<T>();
                    version.Versions = this;
                    version.Document = Document;
                    version.ID = id;
                }
                return version;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            var ids = new HashSet<string>();
            foreach (var cell in Document)
                if (cell.Key.StartsWith(Property + "."))
                    ids.Add(cell.Key.Split(".")[1]);
            var versions = new List<T>();
            foreach (var id in ids)
                versions.Add(this[id]);
            versions.Sort();
            return versions.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public T Current
        {
            get
            {
                var id = Document[Property + "Current"];
                if (!id.HasValue())
                    return null;
                return this[id];
            }
        }

        public class Version : IComparable<Version>
        {
            public DocumentPropertyVersions<T> Versions;

            public string ID;

            public Document Document;

            protected string this[string key]
            {
                get { return Document[Versions.Property + "." + ID + "." + key]; }
            }

            public DateTime Created
            {
                get { return DateTime.FromBinary(this["Created"].ToInt64()); }
            }

            public int UserID
            {
                get { return this["User"].ToInt32(); }
            }

            public int CompareTo(Version other)
            {
                return Created.CompareTo(other.Created);
            }

            public T Previous
            {
                get
                {
                    var id = this["Previous"];
                    if (!id.HasValue())
                        return null;
                    return Versions[id];
                }
            }

            public T Next
            {
                get
                {
                    var id = this["Next"];
                    if (!id.HasValue())
                        return null;
                    return Versions[id];
                }
            }
        }
    }
}
