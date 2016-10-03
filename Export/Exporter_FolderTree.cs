using System.Linq;
using System.Windows.Forms;
using MarkoutBackupViewer.Data;

namespace MarkoutBackupViewer.Export
{
    /// <summary>
    /// экспорт в виде дерева папок
    /// </summary>
    [Exporter("FolderTree", "Folder tree")]
    public class Exporter_FolderTree : Exporter
    {
        /// <summary>
        /// экспорт
        /// </summary>
        /// <param name="backup"></param>
        public override string Export(Backup backup)
        {
            var folder = RequestOutputFolder("Folder Tree");
            if (folder == null)
                return null;
            foreach (var document in backup.RootDocument.Children)
                Export(document, folder);
            return folder;
        }

        /// <summary>
        /// экспорт документа в указанную папку
        /// </summary>
        /// <param name="document"></param>
        /// <param name="folder"></param>
        private void Export(Document document, string folder)
        {
            // создадим папку
            if (document.Children.Any())
            {
                var subfolder = folder.CombinePath(document.Name.ToSafeFileName()).CreateDirectory();
                // поддокументы
                foreach (var child in document.Children)
                    Export(child, subfolder);
            }
            // подготовим название файла
            var path = folder.CombinePath(GetDefaultFileName(document));
            while (path.FileExists() || path.DirectoryExists())
                path = path.GetFileNameWithoutExtension() + "_" + path.GetFileExtention();
            // сохраним
            Save(document, path);
            // покажем сообщение
            MessageBox.Show("Done");
        }
    }
}
