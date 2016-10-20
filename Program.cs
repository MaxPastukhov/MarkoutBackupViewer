using System;
using System.Windows.Forms;
using MarkoutBackupViewer.Common;
using MarkoutBackupViewer.Forms;

namespace MarkoutBackupViewer
{
    static class Program
    {
        [STAThread]
        static void Main(string[] args)
        {
            IsRunning = true;
            SDI = new SDI(args);
            Icons = new Icons(Application.StartupPath.CombinePath("icons.png"), Application.StartupPath.CombinePath("icons.txt"));
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        public static bool IsRunning { get; private set; }

        public static string RootFolder
        {
            get { return Application.StartupPath; }
        }

        public static string SettingsFolder
        {
            get { return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData).CombinePath(Application.ProductName.Replace(" ","")).CreateDirectory(); }
        }

        public static SDI SDI { get; private set; }

        public static Icons Icons { get; private set; }
    }
}
