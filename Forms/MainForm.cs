using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using MarkoutBackupViewer.Export;

namespace MarkoutBackupViewer.Forms
{
    /// <summary>
    /// основная форма
    /// </summary>
    public partial class MainForm : Form
    {
        /// <summary>
        /// конструктор
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
            Program.SDI.Opened += OnBackupOpened;
            Program.SDI.Closed += OnBackupClosed;
        }

        /// <summary>
        /// бекап открыт
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBackupOpened(object sender, EventArgs e)
        {
            backupControl.Backup = Program.SDI.Backup;
            exportToolStripDropDownButton.Visible = true;
        }

        /// <summary>
        /// бекап закрыт
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnBackupClosed(object sender, EventArgs e)
        {
            backupControl.Backup = null;
            exportToolStripDropDownButton.Visible = false;
        }

        /// <summary>
        /// открыть бекап
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnOpenBackup(object sender, EventArgs e)
        {
            Program.SDI.Open();
        }

        /// <summary>
        /// выйти
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnExit(object sender, EventArgs e)
        {
            Close();
        }

        /// <summary>
        /// перейти на https://markout.ru
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMarkoutOrg(object sender, EventArgs e)
        {
            Process.Start("https://markout.org/");
        }

        /// <summary>
        /// показать информацию о приложении
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnAbout(object sender, EventArgs e)
        {
            MessageBox.Show(Application.ProductName + " v" + Assembly.GetEntryAssembly().GetName().Version, "About " + Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        /// <summary>
        /// заполнение списка вариантов экспорта
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void exportToolStripDropDownButton_DropDownOpening(object sender, EventArgs e)
        {
            exportToolStripDropDownButton.DropDown.Items.Clear();
            foreach (var exporter in Exporters.All)
            {
                var item = new ToolStripMenuItem(exporter.Name);
                item.Click += (o, args) => exporter.Export(Program.SDI.Backup);
                exportToolStripDropDownButton.DropDown.Items.Add(item);
            }
        }

        /// <summary>
        /// форма показана
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_Shown(object sender, EventArgs e)
        {
            Program.SDI.Load();
        }
    }
}
