using System;
using System.Windows.Forms;
using MarkoutBackupViewer.Data;


namespace MarkoutBackupViewer.Forms.Viewers
{
    /// <summary>
    /// просмотрщик заметки
    /// </summary>
    public partial class NoteViewer : UserControl, IViewer
    {
        /// <summary>
        /// конструктор
        /// </summary>
        public NoteViewer()
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
                richTextBox.Text = value.Text;
                richTextBox.Padding = new Padding(10, 10, 10, 10);
            }
        }

        private Document _document;

        /// <summary>
        /// ресайз
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NoteViewer_Resize(object sender, EventArgs e)
        {
            richTextBox.Width = Width - richTextBox.Left*2;
            richTextBox.Height = Height - richTextBox.Top * 2;
        }
    }
}
