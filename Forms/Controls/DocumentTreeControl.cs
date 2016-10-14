using System;
using System.Drawing;
using System.Windows.Forms;
using MarkoutBackupViewer.Common;
using MarkoutBackupViewer.Data;
using MarkoutBackupViewer.Export;

namespace MarkoutBackupViewer.Forms.Controls
{
    /// <summary>
    /// контрол дерева документов
    /// </summary>
    public partial class DocumentTreeControl : UserControl
    {
        /// <summary>
        /// конструктор
        /// </summary>
        public DocumentTreeControl()
        {
            InitializeComponent();
            if (Program.IsRunning)
            {
                TreeView = new BufferedTreeView
                {
                    Dock = DockStyle.Fill,
                    ShowLines = false,
                    ShowPlusMinus = true,
                    ShowRootLines = true,
                    FullRowSelect = true,
                    ImageList = Program.Icons.ImageList,
                    Font = new Font(Font.FontFamily, 10)
                };
                TreeView.AfterSelect += treeView_AfterSelect;
                TreeView.MouseDown += TreeView_MouseDown;
                Controls.Add(TreeView);
            }
        }

        /// <summary>
        /// дерево документов
        /// </summary>
        private readonly TreeView TreeView;

        /// <summary>
        /// бекап
        /// </summary>
        public Backup Backup 
        {
            get { return _backup; }
            set
            {
                _backup = value;
                TreeView.Nodes.Clear();
                if (value != null)
                    foreach (var document in value.RootDocument.Children.Sort())
                        TreeView.Nodes.Add(new DocumentTreeNode(document));
            }
        }

        private Backup _backup;

        /// <summary>
        /// выбран другой документ
        /// </summary>
        public event EventHandler<EventArgs<Document>> DocumentSelected;

        /// <summary>
        /// выбран другой документ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (DocumentSelected != null)
                DocumentSelected(this, new EventArgs<Document>(((DocumentTreeNode) e.Node).Document));
        }

        /// <summary>
        /// контекстное меню по правой кнопке
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TreeView_MouseDown(object sender, MouseEventArgs e)
        {
            // сохраним последние кнопки
            var hitInfo = TreeView.HitTest(e.Location);
            if (hitInfo.Node == null)
                return;
            // по правой кнопке - контекстное меню
            if (e.Button == MouseButtons.Right)
            {
                // выберем узел под мышью
                TreeView.SelectedNode = hitInfo.Node;
                contextMenu.Show(this, e.Location);
            }
        }

        /// <summary>
        /// сохранить выбранный документ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var node = TreeView.SelectedNode as DocumentTreeNode;
            if (node != null)
                Exporter.Export(node.Document);
        }
    }
}
