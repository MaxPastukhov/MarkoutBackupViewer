using System.Linq;
using System.Windows.Forms;
using MarkoutBackupViewer.Data;

namespace MarkoutBackupViewer.Export
{
    [Exporter("FolderTree", "Folder tree")]
    public class Exporter_FolderTree : Exporter
    {
        public override string Export(Backup backup)
        {
            var folder = RequestOutputFolder("Folder Tree");
            if (folder == null)
                return null;
            foreach (var document in backup.RootDocument.Children)
                Export(document, folder);
            return folder;
        }
        
        private void Export(Document document, string folder)
        {
            if (document.Children.Any())
            {
                var subfolder = folder.CombinePath(document.Name.ToSafeFileName()).CreateDirectory();
                foreach (var child in document.Children)
                    Export(child, subfolder);
            }
            var path = folder.CombinePath(GetDefaultFileName(document));
            while (path.FileExists() || path.DirectoryExists())
                path = path.GetFileNameWithoutExtension() + "_" + path.GetFileExtention();
            Save(document, path);
            MessageBox.Show("Done");
        }
    }
}
