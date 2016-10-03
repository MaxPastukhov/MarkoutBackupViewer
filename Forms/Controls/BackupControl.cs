using System.Windows.Forms;
using MarkoutBackupViewer.Common;
using MarkoutBackupViewer.Data;

namespace MarkoutBackupViewer.Forms.Controls
{
    /// <summary>
    /// контрол бекапа
    /// </summary>
    public partial class BackupControl : UserControl
    {
        /// <summary>
        /// конструктор
        /// </summary>
        public BackupControl()
        {
            InitializeComponent();
            documentTreeControl.DocumentSelected += documentTreeControl_DocumentSelected;
        }

        /// <summary>
        /// бекап
        /// </summary>
        public Backup Backup
        {
            set
            {
                documentTreeControl.Backup = value;
                documentViewerControl.Document = null;
                Visible = value != null;
            }
        }

        /// <summary>
        /// выбран другой документ в дереве
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void documentTreeControl_DocumentSelected(object sender, EventArgs<Document> e)
        {
            documentViewerControl.Document = e.Value;
        }
    }
}
