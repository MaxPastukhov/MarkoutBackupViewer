using System.Windows.Forms;
using MarkoutBackupViewer.Data;
using MarkoutBackupViewer.Forms.Viewers;

namespace MarkoutBackupViewer.Forms.Controls
{
    /// <summary>
    /// вьювер документов
    /// </summary>
    public partial class DocumentViewerControl : UserControl
    {
        /// <summary>
        /// конструктор
        /// </summary>
        public DocumentViewerControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// документ
        /// </summary>
        public Document Document
        {
            get { return _document; }
            set
            {
                _document = value;
                Visible = value != null;
                // уберем старый редактор
                viewerPanel.Controls.Clear();
                // добавим новый редактор
                var viewer = GetViewer(value);
                if (viewer != null)
                {
                    viewer.Document = value;
                    var control = (Control) viewer;
                    viewerPanel.Controls.Add(control);
                    control.Dock = DockStyle.Fill;
                }
            }
        }

        private Document _document;

        /// <summary>
        /// активный вьювер
        /// </summary>
        private IViewer GetViewer(Document document)
        {
            if (document == null)
                return null;
            switch (document.Type)
            {
                case Document.Types.Note:
                case Document.Types.Text:
                case Document.Types.Html:
                    return new NoteViewer();
                case Document.Types.Image:
                    return new ImageViewer();
                default:
                    return null;
            }
        }
    }
}
