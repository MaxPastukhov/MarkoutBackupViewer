using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using MarkoutBackupViewer.Data;

namespace MarkoutBackupViewer
{
    /// <summary>
    /// расширения
    /// </summary>
    public static class Extentions
    {
        #region TValue GetValue<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue = default(TValue))

        /// <summary>
        /// получение значения из словаря
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="key"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static TValue GetValue<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue = default(TValue))
        {
            TValue value;
            if (dictionary.TryGetValue(key, out value))
                return value;
            return defaultValue;
        }

        #endregion

        #region int ToInt32(this string source, int defaultValue)

        /// <summary>
        /// string to int
        /// </summary>
        /// <param name="source">исходная строка</param>
        /// <param name="defaultValue">дефолтное значение</param>
        /// <returns>число</returns>
        public static int ToInt32(this string source, int defaultValue = 0)
        {
            if (source == null)
                return defaultValue;
            int result;
            if (int.TryParse(source, out result))
                return result;
            return defaultValue;
        }

        #endregion

        #region long ToInt64(this string source, long defaultValue)

        /// <summary>
        /// string to long
        /// </summary>
        /// <param name="source">исходная строка</param>
        /// <param name="defaultValue">дефолтное значение</param>
        /// <returns>число</returns>
        public static long ToInt64(this string source, long defaultValue = 0)
        {
            if (source == null)
                return defaultValue;
            long result;
            if (long.TryParse(source, out result))
                return result;
            return defaultValue;
        }

        #endregion

        #region bool HasValue(this string source)

        /// <summary>
        /// есть значение?
        /// </summary>
        /// <returns>флаг</returns>
        public static bool HasValue(this string source)
        {
            return !string.IsNullOrEmpty(source);
        }

        #endregion

        #region string R(this string source, params object[] values)

        /// <summary>
        /// подстановка параметров вида {0}
        /// </summary>
        /// <param name="source">исходная строка</param>
        /// <param name="values">набор строковых параметров</param>
        /// <returns>получившаяся строка</returns>
        public static string R(this string source, params object[] values)
        {
            if (source == null)
                return null;
            string result = source;
            for (int i = 0; i < values.Length; i++)
            {
                string value = (values[i] != null) ? values[i].ToString() : "";
                result = result.Replace("{" + i + "}", value);
            }
            return result;
        }

        #endregion

        #region string GetFileNameWithoutExtension(this string source)

        /// <summary>
        /// получить имя файла без расширения
        /// </summary>
        /// <returns>флаг</returns>
        public static string GetFileNameWithoutExtension(this string source)
        {
            return Path.GetFileNameWithoutExtension(source);
        }

        #endregion

        #region string CreateDirectory(this string folder)

        /// <summary>
        /// создание папки, если ее еще нет
        /// </summary>
        /// <param name="folder">папка</param>
        /// <returns>число</returns>
        public static string CreateDirectory(this string folder)
        {
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            return folder;
        }

        #endregion

        #region string Join(this IEnumerable<string> list, string separator)

        /// <summary>
        /// объединить список в строку с указанным разделителем
        /// </summary>
        /// <param name="list">список</param>
        /// <param name="separator">разделитель</param>
        public static string Join(this IEnumerable<string> list, string separator)
        {
            return string.Join(separator, list.ToArray());
        }

        #endregion

        #region bool TryDecodeKey(this string key, out byte[] content)

        /// <summary>
        /// попытка декодирования бинарного контента из ключа
        /// </summary>
        /// <param name="key">ключ</param>
        /// <param name="content">получившееся содержимое</param>
        /// <returns>получилось декодировать?</returns>
        public static bool TryDecodeKey(this string key, out byte[] content)
        {
            // нулевой ключ
            if (!key.HasValue())
            {
                content = null;
                return true;
            }
            // пустой контент
            if (key == "#")
            {
                content = new byte[0];
                return true;
            }
            // начинается с признака кодированных данных?
            if (!key.StartsWith("#"))
            {
                content = null;
                return false;
            }
            // декодируем
            content = Convert.FromBase64String(key.Substring(1));
            return true;
        }

        #endregion

        #region byte[] DecompressGZip(this byte[] source)

        /// <summary>
        /// распаковка указанного блока
        /// </summary>
        /// <param name="source">исходный блок</param>
        /// <returns>распакованный блок</returns>
        public static byte[] DecompressGZip(this byte[] source)
        {
            if (source == null || source.Length == 0)
                return null;
            using (var resultStream = new MemoryStream())
            using (var memoryStream = new MemoryStream(source))
            using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
            {
                // содержимое
                byte[] buffer = new byte[65535];
                int read = gZipStream.Read(buffer, 0, buffer.Length);
                while (read > 0)
                {
                    resultStream.Write(buffer, 0, read);
                    read = gZipStream.Read(buffer, 0, buffer.Length);
                }
                return resultStream.ToArray();
            }
        }

        #endregion

        #region string[] Split(this string source, string c, StringSplitOptions options)

        /// <summary>
        /// разбиение строки
        /// </summary>
        /// <param name="source">исходный текст</param>
        /// <param name="s">строка</param>
        /// <param name="options">параметры разбиения</param>
        /// <returns></returns>
        public static string[] Split(this string source, string s, StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries)
        {
            if (string.IsNullOrEmpty(source))
                return new string[0];
            return source.Split(new[] { s }, options);
        }

        #endregion

        #region bool FileExists(this string filePath)

        /// <summary>
        /// существует ли файл?
        /// </summary>
        /// <param name="path">путь к файлу</param>
        /// <returns>существует?</returns>
        public static bool FileExists(this string path)
        {
            return File.Exists(path);
        }

        #endregion

        #region byte[] ReadAllBytes(this string filePath)

        /// <summary>
        /// считать байтовый массив
        /// </summary>
        /// <param name="filePath">путь к файлу</param>
        /// <param name="defaultValue">дефолтное значение</param>
        /// <returns>поток</returns>
        public static byte[] ReadAllBytes(this string filePath, byte[] defaultValue = null)
        {
            if (!File.Exists(filePath))
                return defaultValue;
            return File.ReadAllBytes(filePath);
        }

        #endregion

        #region long FileSize(this string filePath)

        /// <summary>
        /// размер файла
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static long FileSize(this string filePath)
        {
            if (!filePath.FileExists())
                return 0;
            return new FileInfo(filePath).Length;
        }

        #endregion

        #region string CombinePath(this string folder, string fileName)

        /// <summary>
        /// комбинация путей
        /// </summary>
        /// <param name="folder">папка</param>
        /// <param name="fileName">подпапка или файл</param>
        /// <returns>число</returns>
        public static string CombinePath(this string folder, string fileName)
        {
            return Path.Combine(folder, fileName);
        }

        #endregion

        #region string ReadString(this Stream stream, Encoding encoding = null)

        /// <summary>
        /// чтение строки
        /// </summary>
        /// <param name="stream">поток</param>
        /// <param name="encoding">кодировка, по умолчанию UTF8</param>
        /// <returns>значение</returns>
        public static string ReadString(this Stream stream, Encoding encoding = null)
        {
            if (encoding == null)
                encoding = Encoding.UTF8;
            byte[] data = stream.ReadBlob();
            if (data != null)
                return encoding.GetString(data);
            return null;
        }

        #endregion

        #region bool ReadBool(this Stream stream)

        /// <summary>
        /// чтение bool
        /// </summary>
        /// <param name="stream">поток</param>
        /// <returns>значение</returns>
        public static bool ReadBool(this Stream stream)
        {
            return stream.ReadByte() == 1;
        }

        #endregion

        #region byte[] (this Stream stream)

        /// <summary>
        /// чтение byte[] с размером в префиксе
        /// </summary>
        public static byte[] ReadBlob(this Stream stream)
        {
            // длина
            int length = stream.ReadPackedInt32();
            // конец потока?
            if (length < 0)
                return null;
            // если длина нулевая нечего читать
            if (length == 0)
                return new byte[0];
            // подготовим буфер
            byte[] buffer = new byte[length];
            // прочитаем
            int read = stream.Read(buffer, 0, length);
            // если прочитали меньше, чем требовалось, обрежем буфер
            if (read != length)
                throw new EndOfStreamException("ReadBlob expected {0} but read {1}".R(length, read));
            // вернем
            return buffer;
        }

        #endregion

        #region int ReadPackedInt32(this Stream stream)

        /// <summary>
        /// прочитать пакованный int
        /// </summary>
        public static int ReadPackedInt32(this Stream stream)
        {
            // если 0 >= length < 0xFF, длина записывается в виде byte
            // если длина больше или равна 0xFF, записывается как 0xFF, после нее - 4 байта длины
            int byteLength = stream.ReadByte();
            // конец потока?
            if (byteLength < 0)
                return byteLength;
            // признак полной длины
            if (byteLength == 0xFF)
                return stream.ReadInt32();
            return byteLength;
        }

        #endregion

        #region int ReadInt32(this Stream stream)

        /// <summary>
        /// чтение long
        /// </summary>
        /// <param name="stream">поток</param>
        /// <returns>значение</returns>
        public static int ReadInt32(this Stream stream)
        {
            byte[] buffer = new byte[sizeof(int)];
            stream.Read(buffer, 0, sizeof(int));
            return BitConverter.ToInt32(buffer, 0);
        }

        #endregion

        #region long ReadInt64(this Stream stream)

        /// <summary>
        /// чтение long
        /// </summary>
        /// <param name="stream">поток</param>
        /// <returns>значение</returns>
        public static long ReadInt64(this Stream stream)
        {
            byte[] buffer = new byte[sizeof(long)];
            stream.Read(buffer, 0, sizeof(long));
            return BitConverter.ToInt64(buffer, 0);
        }

        #endregion

        #region void WriteAllBytes(this string filePath, byte[] bytes)

        /// <summary>
        /// записать байтовый массив
        /// </summary>
        /// <param name="filePath">путь к файлу</param>
        /// <param name="bytes">массив</param>
        public static void WriteAllBytes(this string filePath, byte[] bytes)
        {
            File.WriteAllBytes(filePath, bytes);
        }

        #endregion

        #region string Icon(this Note note)

        /// <summary>
        /// иконка заметки
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        public static string Icon(this Document document)
        {
            return document.Icon ?? DefaultDocumentIcon(document);
        }

        /// <summary>
        /// дефолтная иконка документа
        /// </summary>
        /// <param name="document"></param>
        /// <returns></returns>
        private static string DefaultDocumentIcon(Document document)
        {
            switch (document.Type)
            {
                case Document.Types.Note:
                    return "document.png";
                case Document.Types.Root:
                    return "book_blue.png";
                case Document.Types.Text:
                    return "document_plain.png";
                case Document.Types.Folder:
                    return "folder.png";
                case Document.Types.Image:
                    return "photo_scenery.png";
                case Document.Types.Html:
                    return "text_rich_colored.png";
                case Document.Types.File:
                    return "paperclip.png";
            }
            return "document.png";
        }

        #endregion

        #region string Md5(this string source)

        /// <summary>
        /// md5-хеш
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string Md5(this string source)
        {
            var md5 = MD5.Create();
            var inputBytes = Encoding.ASCII.GetBytes(source);
            var hash = md5.ComputeHash(inputBytes);
            var sb = new StringBuilder();
            foreach (byte c in hash)
                sb.Append(c.ToString("X2"));
            return sb.ToString();
        }

        #endregion

        #region string HashEncryptionPassword(this string password)

        /// <summary>
        /// хеширование пароля шифрации блокнотов
        /// </summary>
        /// <param name="password">пароль</param>
        /// <returns>хеш-пароля, уникальный для этого приложени</returns>
        public static string HashEncryptionPassword(this string password)
        {
            return ("{0A47ABEC-38CC-43B3-BC73-52690F107F17}" + password).Md5();
        }

        #endregion

        #region bool GetFileExtention(this string filePath)

        /// <summary>
        /// расширение файла
        /// </summary>
        /// <param name="path">путь к файлу</param>
        /// <returns>существует?</returns>
        public static string GetFileExtention(this string path)
        {
            var extention = Path.GetExtension(path);
            if (extention == null)
                return null;
            if (extention.Contains(" "))
                return null;
            return extention;
        }

        #endregion

        #region void WriteAllText(this string filePath, string text)

        /// <summary>
        /// записать текст
        /// </summary>
        /// <param name="filePath">путь к файлу</param>
        /// <param name="text">текст</param>
        public static void WriteAllText(this string filePath, string text)
        {
            File.WriteAllText(filePath, text);
        }

        #endregion

        #region T GetAttribute<T>(this Type type)

        /// <summary>
        /// получить атрибут типа
        /// </summary>
        /// <param name="type">тип</param>
        /// <returns></returns>
        public static T GetAttribute<T>(this Type type)
            where T : Attribute
        {
            foreach (var attribute in type.GetCustomAttributes(false))
            {
                T found = attribute as T;
                if (found != null)
                    return found;
            }
            return null;
        }

        #endregion

        #region bool DirectoryExists(this string filePath)

        /// <summary>
        /// существует ли папка?
        /// </summary>
        /// <param name="path">путь к папке</param>
        /// <returns>существует?</returns>
        public static bool DirectoryExists(this string path)
        {
            return Directory.Exists(path);
        }

        #endregion

        #region string ToSafeFileName(this string source)

        /// <summary>
        /// удаление потенциально опасных элементов из названий файлов и папок
        /// </summary>
        /// <returns>флаг</returns>
        public static string ToSafeFileName(this string source)
        {
            return source.Replace(":", "").Replace("/", "").Replace(@"\", "").Replace("..", "").Replace("?", "");
        }

        #endregion

        #region IEnumerable<Note> Sort(this IEnumerable<Note> notes)

        /// <summary>
        /// сортировка заметок по названию
        /// </summary>
        /// <param name="notes"></param>
        /// <returns></returns>
        public static IEnumerable<Document> Sort(this IEnumerable<Document> notes)
        {
            var sorted = new List<Document>(notes);
            sorted.Sort((x, y) => x.CompareTo(y));
            return sorted;
        }

        #endregion

        #region int NaturalCompareTo(this string x, string y)

        /// <summary>
        /// натуральное сравнение строк
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static int NaturalCompareTo(this string x, string y)
        {
            x = x ?? "";
            y = y ?? "";
            Dictionary<string, string[]> table = new Dictionary<string, string[]>();
            if (x == y)
                return 0;
            // проверим - можно ли сравнить просто как double?
            double dx, dy;
            if (double.TryParse(x, out dx) && double.TryParse(y, out dy))
                return dx.CompareTo(dy);
            // сделаем натуральную сортирвку
            string[] x1, y1;
            if (!table.TryGetValue(x, out x1))
            {
                x1 = Regex.Split(x.Replace(" ", ""), "([0-9]+)");
                table.Add(x, x1);
            }
            if (!table.TryGetValue(y, out y1))
            {
                y1 = Regex.Split(y.Replace(" ", ""), "([0-9]+)");
                table.Add(y, y1);
            }
            for (int i = 0; i < x1.Length && i < y1.Length; i++)
                if (x1[i] != y1[i])
                    return PartCompare(x1[i], y1[i]);
            if (y1.Length < x1.Length)
                return 1;
            if (x1.Length < y1.Length)
                return -1;
            return 0;
        }

        /// <summary>
        /// сравнение частей строк
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static int PartCompare(string left, string right)
        {
            int x, y;
            if (!int.TryParse(left, out x))
                return left.CompareTo(right);
            if (!int.TryParse(right, out y))
                return left.CompareTo(right);
            return x.CompareTo(y);
        }

        #endregion
    }
}
