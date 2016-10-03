using System.Windows.Forms;
using MarkoutBackupViewer.Data;

namespace MarkoutBackupViewer.Export
{
    /// <summary>
    /// базовый класс экспортеров бекапов в разные форматы
    /// </summary>
    public abstract class Exporter
    {
        /// <summary>
        /// идентификатор
        /// </summary>
        public string ID;

        /// <summary>
        /// название
        /// </summary>
        public string Name;

        /// <summary>
        /// экспорт бекапа
        /// </summary>
        /// <param name="backup"></param>
        public abstract string Export(Backup backup);

        /// <summary>
        /// запросить путь к папке для экспорта
        /// </summary>
        /// <returns></returns>
        protected static string RequestOutputFolder(string typeName)
        {
            using (var dialog = new FolderBrowserDialog
            {
                Description = "Export to " + typeName,
                ShowNewFolderButton = true
            })
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                    return null;
                return dialog.SelectedPath;
            }
        }

        /// <summary>
        /// запросить путь к файлу для экспорта
        /// </summary>
        /// <param name="defaultName"></param>
        /// <param name="typeName"></param>
        /// <param name="typeExtention"></param>
        /// <returns></returns>
        protected static string RequestOutputFile(string defaultName, string typeName, string typeExtention)
        {
            using (var dialog = new SaveFileDialog
            {
                Title = "Export " + typeName,
                DefaultExt = typeExtention,
                Filter = "{0} (*{1})|*{1}|All Files (*.*)|*.*".R(typeName, typeExtention),
                FileName = defaultName.GetFileNameWithoutExtension() + typeExtention,
                OverwritePrompt = true,
                RestoreDirectory = true
            })
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                    return null;
                return dialog.FileName;
            }
        }

        /// <summary>
        /// получить расширение файла для экспорта указанного документа
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        protected static string GetFileExtention(Document document)
        {
            switch (document.Type)
            {
                case Document.Types.Note:
                case Document.Types.Text:
                    return ".txt";
                case Document.Types.Html:
                    return ".html";
                case Document.Types.Image:
                    return ".png";
                case Document.Types.File:
                    return document.Name.GetFileExtention();
                default:
                    return ".txt";
            }
        }

        /// <summary>
        /// получить дефолтное название файла для сохранения документа
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        protected static string GetDefaultFileName(Document document)
        {
            return document.Name.GetFileNameWithoutExtension() + GetFileExtention(document);
        }

        /// <summary>
        /// экспорт одиночного документа
        /// </summary>
        /// <param name="document"></param>
        public static void Export(Document document)
        {
            var filePath = RequestOutputFile(document.Name, "*" + GetFileExtention(document), GetFileExtention(document));
            if (filePath == null)
                return;
            if (document.IsBinary)
                filePath.WriteAllBytes(document.Content ?? new byte[0]);
            else
                filePath.WriteAllText(document.Text ?? "");
        }

        /// <summary>
        /// сохранить документ по указанному пути
        /// </summary>
        /// <param name="document"></param>
        /// <param name="filePath"></param>
        protected static void Save(Document document, string filePath)
        {
            if (document.IsBinary)
            {
                if(document.Content != null)
                    filePath.WriteAllBytes(document.Content);
            }
            else if (document.Text != null)
                filePath.WriteAllText(document.Text);
        }
    }
}
