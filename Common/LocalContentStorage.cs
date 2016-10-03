using System.Collections.Generic;
using System.IO;

namespace MarkoutBackupViewer.Common
{
    /// <summary>
    /// хранилище содержимого в обычной папке
    /// </summary>
    public class LocalContentStorage
    {
        /// <summary>
        /// конструктор
        /// </summary>
        /// <param name="folder">папка с содержимым</param>
        public LocalContentStorage(string folder)
        {
            Folder = folder;
        }

        /// <summary>
        /// папка с содержимым
        /// </summary>
        public readonly string Folder;
        
        /// <summary>
        /// получить путь к файлу по идентификатору
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private string GetFilePath(string id)
        {
            List<string> parts = new List<string>(id.Split("/"));
            string fileName = parts[parts.Count-1];
            parts.RemoveAt(parts.Count - 1);
            string folder = (Folder + @"\" + parts.Join(@"\")).CreateDirectory();
            return folder + @"\" + fileName;
        }

        /// <summary>
        /// существует ли содержимое с указанным ключом?
        /// </summary>
        /// <param name="key">ключ</param>
        /// <returns></returns>
        public bool Contains(string key)
        {
            // для начала попробуем декодировать из ключа
            byte[] content;
            if (key.TryDecodeKey(out content))
                return true;
            // есть на диске?
            return GetFilePath(key).FileExists();
        }

        /// <summary>
        /// вычитать указанное содержимое
        /// </summary>
        /// <param name="key">ключ</param>
        /// <returns>содержимое</returns>
        public byte[] Get(string key)
        {
            // для начала попробуем декодировать из ключа
            byte[] content;
            if (key.TryDecodeKey(out content))
                return content;
            // возьмем с диска
            return GetFilePath(key).ReadAllBytes();
        }

        /// <summary>
        /// получить размер содержимого
        /// </summary>
        /// <param name="key">ключ</param>
        /// <returns>размер содержимого</returns>
        public long GetSize(string key)
        {
            // для начала попробуем декодировать из ключа
            byte[] content;
            if (key.TryDecodeKey(out content))
                return content.Length;
            // файл вообще существует?
            if (!Contains(key))
                return 0;
            // вернем размер соответствующего файла
            return GetFilePath(key).FileSize();
        }

        /// <summary>
        /// восстановление элемента с уже существующим ключом из указанного файла
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="key"></param>
        public void Restore(string filePath, string key)
        {
            File.Move(filePath, GetFilePath(key));
        }
    }
}
