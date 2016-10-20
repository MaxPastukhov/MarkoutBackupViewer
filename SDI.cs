using System;
using System.Windows.Forms;
using MarkoutBackupViewer.Data;
using MarkoutBackupViewer.Forms;

namespace MarkoutBackupViewer
{
    public class SDI
    {
        public SDI(string[] args)
        {
            Args = args;
        }
        
        private readonly string[] Args;

        public event EventHandler Closed;

        public event EventHandler Opened;

        public Backup Backup { get; private set; }

        public void Load()
        {
            if (Args.Length > 0)
                Open(Args[0]);
        }

        public void Open()
        {
            Close();
            using (var dialog = new OpenFileDialog
            {
                Title = "Open Markup Backup File",
                RestoreDirectory = true,
                DefaultExt = "markout",
                Filter = "Markout Backups (*.markout)|*.markout|All Files (*.*)|*.*",
                CheckFileExists = true
            })
            {
                if (dialog.ShowDialog() != DialogResult.OK)
                    return;
                Open(dialog.FileName);
            }
        }

        private void Open(string filePath)
        {
            try
            {
                Backup = new Backup(filePath);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message, "Can't load " + filePath, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
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
            if (Backup.RootDocument == null)
            {
                MessageBox.Show("There are no documents in this backup", "Empty backup", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }
            if (Opened != null)
                Opened(this, new EventArgs());
        }
        
        public void Close()
        {
            Backup = null;
            if (Closed != null)
                Closed(this, new EventArgs());
        }
    }
}
