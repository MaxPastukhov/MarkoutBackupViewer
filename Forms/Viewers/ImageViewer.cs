using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MarkoutBackupViewer.Data;

namespace MarkoutBackupViewer.Forms.Viewers
{
    public partial class ImageViewer : UserControl, IViewer
    {
        public ImageViewer()
        {
            InitializeComponent();
        }
        
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
