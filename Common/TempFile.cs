using System;
using System.IO;

namespace MarkoutBackupViewer.Common
{
    /// <summary>
    /// временный файл
    /// </summary>
    public class TempFile : IDisposable
    {
        /// <summary>
        /// конструктор
        /// </summary>
        public TempFile()
        {
            Path = System.IO.Path.GetTempPath().CombinePath(Guid.NewGuid().ToString());
        }

        /// <summary>
        /// путь к файлу
        /// </summary>
        public readonly string Path;

        /// <summary>
        /// освобождение ресурсов
        /// </summary>
        public void Dispose()
        {
            if(Path.FileExists())
                File.Delete(Path);
        }
    }
}
