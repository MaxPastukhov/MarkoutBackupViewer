using System.Windows.Forms;
using MarkoutBackupViewer.Data;

namespace MarkoutBackupViewer.Forms.Controls
{
    public class DocumentTreeNode : TreeNode
    {
        public DocumentTreeNode(Document document)
        {
            Document = document;
            Text = document.Name;
            SelectedImageIndex = ImageIndex = Program.Icons[document.Icon(), "document.png"];
            foreach (var child in document.Children.Sort())
                Nodes.Add(new DocumentTreeNode(child));
        }
        
        public readonly Document Document;
    }
}
