using Amazon;
using Amazon.S3;
using Amazon.S3.Model;

namespace MarkoutBackupViewer.Common
{
    /// <summary>
    /// клиент Amazon S3
    /// </summary>
    public class AmazonS3
    {
        /// <summary>
        /// бакет
        /// </summary>
        public string Bucket;

        /// <summary>
        /// папка
        /// </summary>
        public string Folder;

        /// <summary>
        /// ключ доступа
        /// </summary>
        public string AccessKey;

        /// <summary>
        /// секретный ключ доступа
        /// </summary>
        public string SecretAccessKey;

        /// <summary>
        /// только по чтению?
        /// </summary>
        public bool ReadOnly;

        /// <summary>
        /// клиент
        /// </summary>
        private AmazonS3Client Client
        {
            get
            {
                return _client ?? (_client = new AmazonS3Client(AccessKey, SecretAccessKey, new AmazonS3Config
                {
                    UseHttp = true,
                    BufferSize = 1024*1024,
                    RegionEndpoint = RegionEndpoint.EUWest1
                }));
            }
        }

        private AmazonS3Client _client;

        /// <summary>
        /// существует ли такой файл?
        /// </summary>
        /// <param name="key">ключ файла</param>
        public bool Exists(string key)
        {
            try
            {
                Client.GetObjectMetadata(new GetObjectMetadataRequest
                {
                    BucketName = Bucket,
                    Key = Combine(Folder, key)
                });
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// скачивание указанного файла
        /// </summary>
        /// <param name="key"></param>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public bool Download(string key, string filePath)
        {
            using (GetObjectResponse response = Client.GetObject(new GetObjectRequest
            {
                BucketName = Bucket,
                Key = Combine(Folder, key)

            }))
            {
                response.WriteResponseStreamToFile(filePath);
                return true;
            }
        }

        /// <summary>
        /// комбинация путей
        /// </summary>
        /// <param name="path"></param>
        /// <param name="folder"></param>
        /// <returns></returns>
        private string Combine(string path, string folder)
        {
            if (!path.HasValue())
                return folder;
            return path + "/" + folder;
        }

        /// <summary>
        /// создание клиента над подпапкой
        /// </summary>
        /// <param name="folder"></param>
        /// <returns></returns>
        public AmazonS3 SubFolder(string folder)
        {
            return new AmazonS3
            {
                Bucket = Bucket,
                Folder = Combine(Folder, folder),
                AccessKey = AccessKey,
                SecretAccessKey = SecretAccessKey,
                ReadOnly = ReadOnly
            };
        }
    }
}
