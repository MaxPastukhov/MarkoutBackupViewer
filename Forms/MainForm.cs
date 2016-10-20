using System;
using System.Diagnostics;
using System.Reflection;
using System.Windows.Forms;
using MarkoutBackupViewer.Export;

namespace MarkoutBackupViewer.Forms
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
            Program.SDI.Opened += OnBackupOpened;
            Program.SDI.Closed += OnBackupClosed;
        }

        private void OnBackupOpened(object sender, EventArgs e)
        {
            backupControl.Backup = Program.SDI.Backup;
            exportToolStripDropDownButton.Visible = true;
        }

        private void OnBackupClosed(object sender, EventArgs e)
        {
            backupControl.Backup = null;
            exportToolStripDropDownButton.Visible = false;
        }

        private void OnOpenBackup(object sender, EventArgs e)
        {
            Program.SDI.Open();
        }

        private void OnExit(object sender, EventArgs e)
        {
            Close();
        }

        private void OnMarkoutOrg(object sender, EventArgs e)
        {
            Process.Start("https://markout.org/");
        }

        private void OnAbout(object sender, EventArgs e)
        {
            MessageBox.Show(Application.ProductName + " v" + Assembly.GetEntryAssembly().GetName().Version, "About " + Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

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

        private void MainForm_Shown(object sender, EventArgs e)
        {
            Program.SDI.Load();
        }
    }
}
