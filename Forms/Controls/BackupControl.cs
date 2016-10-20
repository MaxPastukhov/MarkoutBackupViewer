using System.Windows.Forms;
using MarkoutBackupViewer.Common;
using MarkoutBackupViewer.Data;

namespace MarkoutBackupViewer.Forms.Controls
{
    public partial class BackupControl : UserControl
    {
        public BackupControl()
        {
            InitializeComponent();
            documentTreeControl.DocumentSelected += documentTreeControl_DocumentSelected;
        }

        public Backup Backup
        {
            set
            {
                documentTreeControl.Backup = value;
                documentViewerControl.Document = null;
                Visible = value != null;
            }
        }

        private void documentTreeControl_DocumentSelected(object sender, EventArgs<Document> e)
        {
            documentViewerControl.Document = e.Value;
        }
    }
}
