using System.Collections.Generic;

namespace MarkoutBackupViewer.Data
{
    /// <summary>
    /// строка таблицы данных
    /// </summary>
    public class Row
    {
        /// <summary>
        /// конструктор
        /// </summary>
        /// <param name="id"></param>
        public Row(string id)
        {
            ID = id;
        }

        /// <summary>
        /// идентификатор
        /// </summary>
        public readonly string ID;

        /// <summary>
        /// ячейки
        /// </summary>
        private readonly Dictionary<string,string> Cells = new Dictionary<string, string>();

        /// <summary>
        /// доступ к ячейкам
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
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
    }
}
