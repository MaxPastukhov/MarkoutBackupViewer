using System;
using System.Windows.Forms;
using MarkoutBackupViewer.Data;
using MarkoutBackupViewer.Forms;

namespace MarkoutBackupViewer
{
    /// <summary>
    /// Single Document Interface
    /// </summary>
    public class SDI
    {
        /// <summary>
        /// конструктор
        /// </summary>
        /// <param name="args"></param>
        public SDI(string[] args)
        {
            Args = args;
        }

        /// <summary>
        /// аргументы программы
        /// </summary>
        private readonly string[] Args;

        /// <summary>
        /// файло закрыт
        /// </summary>
        public event EventHandler Closed;

        /// <summary>
        /// файл открыт
        /// </summary>
        public event EventHandler Opened;

        /// <summary>
        /// открытый бекап
        /// </summary>
        public Backup Backup { get; private set; }

        /// <summary>
        /// загрузка
        /// </summary>
        public void Load()
        {
            if (Args.Length > 0)
                Open(Args[0]);
        }

        /// <summary>
        /// открыть файл
        /// </summary>
        public void Open()
        {
            // сначала закроем предыдущий файл
            Close();
            // откроем новый
            using (var dialog = new OpenFileDialog
            {
                Title = "Open Markup Backup File",
                RestoreDirectory = true,
                DefaultExt = "markout",
                Filter = "Markout Backups (*.markout)|*.markout|All Files (*.*)|*.*",
                CheckFileExists = true
            })
            {
                // запросим путь к файлу
                if (dialog.ShowDialog() != DialogResult.OK)
                    return;
                // откроем
                Open(dialog.FileName);
            }
        }

        /// <summary>
        /// открыть указанный файл
        /// </summary>
        /// <param name="filePath"></param>
        private void Open(string filePath)
        {
            // попробуем загрузить
            try
            {
                Backup = new Backup(filePath, Program.ContentStorage);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Can't load " + filePath, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            // требуется пароль?
            if (Backup.PasswordHash.HasValue())
            {
                while (true)
                {
                    using (var passwordDialog = new EnterPasswordForm())
                    {
                        if (passwordDialog.ShowDialog() != DialogResult.OK)
                            return;
                        if (passwordDialog.Password.Text.HashEncryptionPassword() == Backup.PasswordHash)
                        {
                            Backup.Password = passwordDialog.Password.Text;
                            break;
                        }
                    }
                }
            }
            // получилось загрузить, уведомим
            if (Opened != null)
                Opened(this, new EventArgs());
        }

        /// <summary>
        /// закрыть файлов
        /// </summary>
        public void Close()
        {
            Backup = null;
            if (Closed != null)
                Closed(this, new EventArgs());
        }
    }
}
