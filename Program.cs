using System;
using System.Windows.Forms;
using MarkoutBackupViewer.Common;
using MarkoutBackupViewer.Forms;

namespace MarkoutBackupViewer
{
    static class Program
    {
        /// <summary>
        /// точка входа
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            IsRunning = true;
            // менеджер документов
            SDI = new SDI(args);
            Icons = new Icons(Application.StartupPath.CombinePath("icons.png"), Application.StartupPath.CombinePath("icons.txt"));
            // покажем основное окно приложения
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        /// <summary>
        /// программа запущена?
        /// </summary>
        public static bool IsRunning { get; private set; }

        /// <summary>
        /// корневая папка
        /// </summary>
        public static string RootFolder
        {
            get { return Application.StartupPath; }
        }

        /// <summary>
        /// папка с настройками приложения
        /// </summary> 
        public static string SettingsFolder
        {
            get { return Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData).CombinePath(Application.ProductName.Replace(" ","")).CreateDirectory(); }
        }

        /// <summary>
        /// Single Document Interface
        /// </summary>
        public static SDI SDI { get; private set; }

        /// <summary>
        /// хранилище иконок
        /// </summary>
        public static Icons Icons { get; private set; }
    }
}
