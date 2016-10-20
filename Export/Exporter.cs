using System.Windows.Forms;
using MarkoutBackupViewer.Data;

namespace MarkoutBackupViewer.Export
{
    public abstract class Exporter
    {
        public string ID;

        public string Name;

        public abstract string Export(Backup backup);

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

        protected static string GetDefaultFileName(Document document)
        {
            return document.Name.GetFileNameWithoutExtension() + GetFileExtention(document);
        }

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
