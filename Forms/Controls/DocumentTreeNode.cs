using System.Windows.Forms;
using MarkoutBackupViewer.Data;

namespace MarkoutBackupViewer.Forms.Controls
{
    /// <summary>
    /// узел дерева документов
    /// </summary>
    public class DocumentTreeNode : TreeNode
    {
        /// <summary>
        /// конструктор
        /// </summary>
        /// <param name="document"></param>
        public DocumentTreeNode(Document document)
        {
            Document = document;
            Text = document.Name;
            SelectedImageIndex = ImageIndex = Program.Icons[document.Icon(), "document.png"];
            // добавим поддокументы
            foreach (var child in document.Children)
                Nodes.Add(new DocumentTreeNode(child));
        }

        /// <summary>
        /// документ
        /// </summary>
        public readonly Document Document;

        /// <summary>
        /// обновить картинки
        /// </summary>
        public void UpdateImages()
        {
            SelectedImageIndex = ImageIndex = Program.Icons[Document.Icon(), "document.png"];
            foreach (var node in Nodes)
                ((DocumentTreeNode)node).UpdateImages();
        }
    }
}
