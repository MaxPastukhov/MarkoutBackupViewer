using System.IO;

namespace MarkoutBackupViewer.Common
{
    /// <summary>
    /// хранилище содержимого на Amazon S3
    /// </summary>
    public class ContentStorage
    {
        /// <summary>
        /// конструктор
        /// </summary>
        /// <param name="folder">локальная папка</param>
        public ContentStorage(string folder)
        {
            LocalStorage = new LocalContentStorage(folder);
        }

        /// <summary>
        /// хранилище файлов в папке
        /// </summary>
        private readonly LocalContentStorage LocalStorage;

        /// <summary>
        /// клиент амазона
        /// </summary>
        private readonly AmazonS3 Client = new AmazonS3
        {
            Bucket = "markout.ru",
            AccessKey = "AKIAJGNZ7PTS22KDDG6Q",
            SecretAccessKey = "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"
        };

        /// <summary>
        /// вычитать указанное содержимое
        /// </summary>
        /// <param name="key">ключ</param>
        /// <returns>содержимое</returns>
        public byte[] this[string key]
        {
            get
            {
                // для начала попробуем декодировать из ключа
                byte[] content;
                if (key.TryDecodeKey(out content))
                    return content;
                // проверим вместе с попыткой скачать
                if (!Contains(key))
                    return null;
                // есть в локальной папке
                return LocalStorage.Get(key);
            }
        }

        /// <summary>
        /// существует ли содержимое с указанным ключом?
        /// </summary>
        /// <param name="key">ключ</param>
        /// <returns></returns>
        private bool Contains(string key)
        {
            // для начала попробуем декодировать из ключа
            byte[] content;
            if (key.TryDecodeKey(out content))
                return true;
            // проверим в локальном хранилище
            if (!LocalStorage.Contains(key))
            {
                // в локальном нет, попробуем загрузить
                if (!TryDownload(key))
                    return false; // нет и на амазоне
            }
            // либо уже был, либо скачали
            return true;
        }
       
        /// <summary>
        /// попытка скачивания файла
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private bool TryDownload(string id)
        {
            // скачаем во временный файл
            using (var file = new TempFile())
            {
                try
                {
                    if (!Client.Download(id, file.Path))
                        return false; // такого файла нет
                }
                catch
                {
                    return false;
                }
                // распакуем
                File.WriteAllBytes(file.Path, file.Path.ReadAllBytes().DecompressGZip());
                // восстановим элемент в локальном хранилище
                LocalStorage.Restore(file.Path, id);
                // файл скачан
                return true;
            }
        }
    }
}
