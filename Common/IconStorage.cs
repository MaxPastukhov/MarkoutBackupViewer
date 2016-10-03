using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using MarkoutBackupViewer.Properties;

namespace MarkoutBackupViewer.Common
{
    /// <summary>
    /// хранилище иконок
    /// </summary>
    public class IconStorage
    {
        /// <summary>
        /// конструктор
        /// </summary>
        /// <param name="cacheFolder">папка с кешем</param>
        /// <param name="downloadUrl">адрес скачивания</param>
        public IconStorage(string cacheFolder, string downloadUrl)
        {
            CacheFolder = cacheFolder.CreateDirectory();
            DownloadUrl = downloadUrl;
        }

        /// <summary>
        /// папка с кешем скачанных картинок
        /// </summary>
        private readonly string CacheFolder;

        /// <summary>
        /// адрес скачивания недостающих иконок
        /// </summary>
        private readonly string DownloadUrl;

        /// <summary>
        /// список подгруженных картинок
        /// </summary>
        public readonly ImageList ImageList = new ImageList();

        /// <summary>
        /// очередь загрузки
        /// </summary>
        private readonly HashSet<string> DownloadQueue = new HashSet<string>();

        /// <summary>
        /// попытка подгрузки изображения
        /// </summary>
        /// <param name="name">название</param>
        /// <returns></returns>
        public bool TryLoad(string name)
        {
            // попробуем получить из кеша
            lock (ImageList)
                if (ImageList.Images.ContainsKey(name))
                    return true;
            // в кеше - нет, попробуем найти в статичных
            var image = GetStaticImage(name);
            if (image != null)
            {
                lock (ImageList)
                    ImageList.Images.Add(name, image);
                return true;
            }
            // в локальных - нет, попробуем найти в скачанных
            if (CacheFolder.CombinePath(name).FileExists())
            {
                image = Image.FromFile(CacheFolder.CombinePath(name));
                lock (ImageList)
                    ImageList.Images.Add(name, image);
                return true;
            }
            // попробуем скачать в фоне, чтобы в следующий раз они уже были в наличии
            try
            {
                // добавим в очередь загрузки
                lock (DownloadQueue)
                    DownloadQueue.Add(name);
                // запустим закачку
                Download();
                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// получить из статичных картинок, встренных в ресурсы
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private Image GetStaticImage(string name)
        {
            switch (name)
            {
                case "document.png":
                    return Resources.document;
                case "folder.png":
                    return Resources.folder;
                case "paperclip.png":
                    return Resources.paperclip;
                case "photo_scenery.png":
                    return Resources.photo_scenery;
                case "book_blue.png":
                    return Resources.book_blue;
            }
            return null;
        }

        /// <summary>
        /// подгрузка изображения
        /// </summary>
        /// <param name="name">название желаемого изображения</param>
        /// <param name="defaultName">ее дефолтный вариант, если желаемого нет</param>
        /// <returns>то, что получилось на выходе</returns>
        public string Load(string name, string defaultName)
        {
            // попробуем подгрузить желаемое
            if (TryLoad(name))
                return name;
            // попробуем подгрузить дефолтное
            if (TryLoad(defaultName))
                return defaultName;
            // ничего не получилось подгрузить
            return "document.png";
        }

        /// <summary>
        /// удобный вариант доступа
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defaultName"></param>
        /// <returns></returns>
        public int this[string name, string defaultName]
        {
            get { return ImageList.Images.IndexOfKey(Load(name, defaultName)); }
        }

        /// <summary>
        /// уже загружается что-то?
        /// </summary>
        private volatile bool IsDownloading;

        /// <summary>
        /// список обновлен из-за подгрузки очередного изображения
        /// </summary>
        public event EventHandler Updated;

        /// <summary>
        /// картинки, которые не получилось загрузить
        /// </summary>
        private readonly HashSet<string> FailedToDownload = new HashSet<string>();

        /// <summary>
        /// скачивание картинок
        /// </summary>
        private void Download()
        {
            if(IsDownloading)
                return;
            // получим очередной элемент на скачивание
            string name;
            lock (DownloadQueue)
            {
                if (DownloadQueue.Count == 0)
                    return;
                name = DownloadQueue.First();
            }
            // загрузим его в фоне
            Background.Run(() =>
            {
                IsDownloading = true;
                if (!FailedToDownload.Contains(name))
                {
                    var client = new WebClient();
                    var data = client.DownloadData(DownloadUrl + name);
                    CacheFolder.CombinePath(name).WriteAllBytes(data);
                }
                lock (DownloadQueue)
                    DownloadQueue.Remove(name);
            }, exception =>
            {
                IsDownloading = false;
                if (exception == null)
                {
                    lock (ImageList)
                        ImageList.Images.Add(name, Image.FromFile(CacheFolder.CombinePath(name)));
                    if (Updated != null)
                        Updated(this, new EventArgs());
                    Download();
                }
                else
                    FailedToDownload.Add(name);
            });
        }
    }
}
