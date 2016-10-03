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
            // хранилище контента
            ContentStorage = new ContentStorage(SettingsFolder.CombinePath("Content"));
            // менеджер документов
            SDI = new SDI(args);
            Icons = new IconStorage(SettingsFolder.CombinePath("Icons"), "https://markout.org/static/icons/16x16/");
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
        /// хранилище контента документов
        /// </summary>
        public static ContentStorage ContentStorage { get; private set; }

        /// <summary>
        /// Single Document Interface
        /// </summary>
        public static SDI SDI { get; private set; }

        /// <summary>
        /// хранилище иконок
        /// </summary>
        public static IconStorage Icons { get; private set; }
    }
}
