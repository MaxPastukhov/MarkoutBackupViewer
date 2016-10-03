using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MarkoutBackupViewer.Data;

namespace MarkoutBackupViewer.Forms.Viewers
{
    /// <summary>
    /// просмотрщик картинок
    /// </summary>
    public partial class ImageViewer : UserControl, IViewer
    {
        /// <summary>
        /// конструктор
        /// </summary>
        public ImageViewer()
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
                if(value.Content!=null)
                    using (var stream = new MemoryStream(value.Content))
                        pictureBox1.Image = Image.FromStream(stream);
            }
        }

        private Document _document;
    }
}
