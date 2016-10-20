using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MarkoutBackupViewer.Data
{
    public class Table : IEnumerable<Row>
    {
        public Table(MemoryStream stream)
        {
            while (stream.Position < stream.Length)
            {
                var id = stream.ReadString();
                var row = Rows.GetValue(id) ?? (Rows[id] = new Row(id));
                var count = stream.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    var column = stream.ReadString();
                    var value = stream.ReadString();
                    row[column] = value;
                    stream.ReadInt32();
                }
            }
            foreach (var row in Rows.Values.ToArray())
                if (row["Deleted"].HasValue())
                    Rows.Remove(row.ID);
            foreach (var row in Rows.Values)
            {
                var value = row["Parent"];
                if (!value.HasValue())
                    continue;
                var index = ParentIndex.GetValue(value) ?? (ParentIndex[value] = new List<string>());
                index.Add(row.ID);
            }
        }

        private readonly Dictionary<string,Row> Rows = new Dictionary<string, Row>();

        public Row this[string id]
        {
            get { return Rows.GetValue(id); }
            set
            {
                if (value != null)
                    Rows[id] = value;
                else
                    Rows.Remove(id);
            }
        }

        private readonly Dictionary<string,List<string>> ParentIndex = new Dictionary<string, List<string>>();

        public IEnumerable<string> SelectParent(string value)
        {
            if (!value.HasValue())
                return new string[0];
            var index = ParentIndex.GetValue(value);
            if (index == null)
                return new string[0];
            return index;
        }

        public IEnumerator<Row> GetEnumerator()
        {
            return Rows.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
