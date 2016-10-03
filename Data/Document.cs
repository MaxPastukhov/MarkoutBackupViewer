using System;
using System.Collections.Generic;
using System.Linq;

namespace MarkoutBackupViewer.Data
{
    /// <summary>
    /// документ
    /// </summary>
    public class Document
    {
        /// <summary>
        /// конструктор
        /// </summary>
        /// <param name="backup">бекап</param>
        /// <param name="row">строка в таблице документов</param>
        public Document(Backup backup, Row row)
        {
            Backup = backup;
            Row = row;
        }

        /// <summary>
        /// бекап
        /// </summary>
        public readonly Backup Backup;

        /// <summary>
        /// строка
        /// </summary>
        private readonly Row Row;

        /// <summary>
        /// свойства документа
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private string this[string key]
        {
            get { return Row[key]; }
        }

        /// <summary>
        /// идентификатор
        /// </summary>
        public string ID
        {
            get { return Row.ID; }
        }

        /// <summary>
        /// короткий идентификатор
        /// </summary>
        public string ShortID
        {
            get { return this["ShortID"]; }
        }

        /// <summary>
        /// тип документа
        /// </summary>
        public string Type
        {
            get { return this["Type"] ?? "Note"; }
        }

        /// <summary>
        /// типы
        /// </summary>
        public static class Types
        {
            /// <summary>
            /// корневая заметка
            /// </summary>
            public const string Root = "Root";

            /// <summary>
            /// корзина
            /// </summary>
            public const string Trash = "Trash";

            /// <summary>
            /// заметка
            /// </summary>
            public const string Note = "Note";

            /// <summary>
            /// папка
            /// </summary>
            public const string Folder = "Folder";

            /// <summary>
            /// картинка
            /// </summary>
            public const string Image = "Image";

            /// <summary>
            /// приложенный файл
            /// </summary>
            public const string File = "File";

            /// <summary>
            /// текстовая заметка
            /// </summary>
            public const string Text = "Text";

            /// <summary>
            /// HTML-заметка
            /// </summary>
            public const string Html = "Html";
        }

        /// <summary>
        /// бинарный файл?
        /// </summary>
        public bool IsBinary
        {
            get { return Type == Types.Image || Type == Types.File; }
        }

        /// <summary>
        /// название
        /// </summary>
        public string Name
        {
            get
            {
                if (Type == Types.Root)
                    return Backup.FilePath;
                return Backup.Decrypt(this["Name"]);
            }
        }

        /// <summary>
        /// иконка
        /// </summary>
        public string Icon
        {
            get { return this["Icon"]; }
        }

        /// <summary>
        /// порядок сортировки
        /// </summary>
        public int SortOrder
        {
            get { return this["SortOrder"].ToInt32(); }
        }

        /// <summary>
        /// дата/время создания
        /// </summary>
        public DateTime Created
        {
            get { return DateTime.FromBinary(this["Created"].ToInt64()); }
        }

        /// <summary>
        /// дата/время изменения
        /// </summary>
        public DateTime Modified
        {
            get
            {
                if (!this["Modified"].HasValue())
                    return Created;
                return DateTime.FromBinary(this["Modified"].ToInt64());
            }
        }

        /// <summary>
        /// дата/время удаления в корзину
        /// </summary>
        public DateTime? Trashed
        {
            get
            {
                if (!this["Trashed"].HasValue())
                    return null;
                return DateTime.FromBinary(this["Trashed"].ToInt64());
            }
        }

        /// <summary>
        /// текст заметки
        /// </summary>
        public string Text
        {
            get { return Backup.Decrypt(this["Text"]); }
        }

        /// <summary>
        /// ключ бинарного контента
        /// </summary>
        public string ContentKey
        {
            get { return this["ContentKey"]; }
        }

        /// <summary>
        /// размер бинарного контента
        /// </summary>
        public int ContentSize
        {
            get { return this["ContentLength"].ToInt32(); }
        }

        /// <summary>
        /// бинарное содержимое вроде картинок, файлов и так далее
        /// </summary>
        public byte[] Content
        {
            get
            {
                if (!ContentKey.HasValue())
                    return null;
                return Backup.Decrypt(Backup.Content[ContentKey]);
            }
        }

        /// <summary>
        /// теги
        /// </summary>
        public string[] Tags
        {
            get { return (this["Tags"] ?? "").Split(new[] {','}, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToArray(); }
        }

        /// <summary>
        /// родительский документ
        /// </summary>
        public Document Parent
        {
            get { return Backup[this["Parent"]]; }
        }

        /// <summary>
        /// поддокументы
        /// </summary>
        public IEnumerable<Document> Children
        {
            get
            {
                foreach (var id in Backup.Table.SelectParent(ID))
                    yield return Backup[id];
            }
        }
    }
}
