using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MarkoutBackupViewer.Data
{
    /// <summary>
    /// таблица с данными
    /// </summary>
    public class Table : IEnumerable<Row>
    {
        /// <summary>
        /// конструктор
        /// </summary>
        /// <param name="stream"></param>
        public Table(MemoryStream stream)
        {
            // загрузим
            while (stream.Position < stream.Length)
            {
                var id = stream.ReadString();
                // получим или создадим строку
                var row = Rows.GetValue(id) ?? (Rows[id] = new Row(id));
                // обновленные ячейки
                var count = stream.ReadInt32();
                for (int i = 0; i < count; i++)
                {
                    var column = stream.ReadString();
                    var value = stream.ReadString();
                    row[column] = value;
                    stream.ReadInt32();
                }
            }
            // удалим удаленые строки
            foreach (var row in Rows.Values.ToArray())
                if (row["Deleted"].HasValue())
                    Rows.Remove(row.ID);
            // построим индекс по колонке Parent
            foreach (var row in Rows.Values)
            {
                var value = row["Parent"];
                if (!value.HasValue())
                    continue;
                var index = ParentIndex.GetValue(value) ?? (ParentIndex[value] = new List<string>());
                index.Add(row.ID);
            }
        }

        /// <summary>
        /// строки таблицы
        /// </summary>
        private readonly Dictionary<string,Row> Rows = new Dictionary<string, Row>();

        /// <summary>
        /// получить строку с указанным идентификатором
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// индекс по колонке Parent
        /// </summary>
        private readonly Dictionary<string,List<string>> ParentIndex = new Dictionary<string, List<string>>();

        /// <summary>
        /// выбрать все строки, в которых указанная колонка равна указанному значению
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public IEnumerable<string> SelectParent(string value)
        {
            if (!value.HasValue())
                return new string[0];
            var index = ParentIndex.GetValue(value);
            if (index == null)
                return new string[0];
            return index;
        }

        /// <summary>
        /// обход всех строк
        /// </summary>
        /// <returns></returns>
        public IEnumerator<Row> GetEnumerator()
        {
            return Rows.Values.GetEnumerator();
        }

        /// <summary>
        /// обход всех строк
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
