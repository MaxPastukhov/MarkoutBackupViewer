using System.Collections;
using System.Collections.Generic;

namespace MarkoutBackupViewer.Data
{
    public class Row : IEnumerable<KeyValuePair<string,string>>
    {
        public Row(string id)
        {
            ID = id;
        }

        public readonly string ID;

        private readonly Dictionary<string,string> Cells = new Dictionary<string, string>();

        public string this[string key]
        {
            get { return Cells.GetValue(key); }
            set
            {
                if (value != null)
                    Cells[key] = value;
                else
                    Cells.Remove(key);
            }
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return Cells.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
