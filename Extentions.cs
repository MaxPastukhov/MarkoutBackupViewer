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

        public static TValue GetValue<TKey, TValue>(this Dictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue = default(TValue))
        {
            TValue value;
            if (dictionary.TryGetValue(key, out value))
                return value;
            return defaultValue;
        }

        #endregion

        #region int ToInt32(this string source, int defaultValue)

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

        public static bool HasValue(this string source)
        {
            return !string.IsNullOrEmpty(source);
        }

        #endregion

        #region string R(this string source, params object[] values)

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

        public static string GetFileNameWithoutExtension(this string source)
        {
            return Path.GetFileNameWithoutExtension(source);
        }

        #endregion

        #region string CreateDirectory(this string folder)

        public static string CreateDirectory(this string folder)
        {
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            return folder;
        }

        #endregion

        #region string Join(this IEnumerable<string> list, string separator)

        public static string Join(this IEnumerable<string> list, string separator)
        {
            return string.Join(separator, list.ToArray());
        }

        #endregion

        #region bool TryDecodeKey(this string key, out byte[] content)

        public static bool TryDecodeKey(this string key, out byte[] content)
        {
            if (!key.HasValue())
            {
                content = null;
                return true;
            }
            if (key == "#")
            {
                content = new byte[0];
                return true;
            }
            if (!key.StartsWith("#"))
            {
                content = null;
                return false;
            }
            content = Convert.FromBase64String(key.Substring(1));
            return true;
        }

        #endregion

        #region byte[] DecompressGZip(this byte[] source)

        public static byte[] DecompressGZip(this byte[] source)
        {
            if (source == null || source.Length == 0)
                return null;
            using (var resultStream = new MemoryStream())
            using (var memoryStream = new MemoryStream(source))
            using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Decompress))
            {
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

        public static string[] Split(this string source, string s, StringSplitOptions options = StringSplitOptions.RemoveEmptyEntries)
        {
            if (string.IsNullOrEmpty(source))
                return new string[0];
            return source.Split(new[] { s }, options);
        }

        #endregion

        #region bool FileExists(this string filePath)

        public static bool FileExists(this string path)
        {
            return File.Exists(path);
        }

        #endregion

        #region byte[] ReadAllBytes(this string filePath)

        public static byte[] ReadAllBytes(this string filePath, byte[] defaultValue = null)
        {
            if (!File.Exists(filePath))
                return defaultValue;
            return File.ReadAllBytes(filePath);
        }

        #endregion

        #region long FileSize(this string filePath)

        public static long FileSize(this string filePath)
        {
            if (!filePath.FileExists())
                return 0;
            return new FileInfo(filePath).Length;
        }

        #endregion

        #region string CombinePath(this string folder, string fileName)

        public static string CombinePath(this string folder, string fileName)
        {
            return Path.Combine(folder, fileName);
        }

        #endregion

        #region string ReadString(this Stream stream, Encoding encoding = null)

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

        public static bool ReadBool(this Stream stream)
        {
            return stream.ReadByte() == 1;
        }

        #endregion

        #region byte[] (this Stream stream)

        public static byte[] ReadBlob(this Stream stream)
        {
            int length = stream.ReadPackedInt32();
            if (length < 0)
                return null;
            if (length == 0)
                return new byte[0];
            byte[] buffer = new byte[length];
            int read = stream.Read(buffer, 0, length);
            if (read != length)
                throw new EndOfStreamException("ReadBlob expected {0} but read {1}".R(length, read));
            return buffer;
        }

        #endregion

        #region int ReadPackedInt32(this Stream stream)

        public static int ReadPackedInt32(this Stream stream)
        {
            int byteLength = stream.ReadByte();
            if (byteLength < 0)
                return byteLength;
            if (byteLength == 0xFF)
                return stream.ReadInt32();
            return byteLength;
        }

        #endregion

        #region int ReadInt32(this Stream stream)

        public static int ReadInt32(this Stream stream)
        {
            byte[] buffer = new byte[sizeof(int)];
            stream.Read(buffer, 0, sizeof(int));
            return BitConverter.ToInt32(buffer, 0);
        }

        #endregion

        #region long ReadInt64(this Stream stream)

        public static long ReadInt64(this Stream stream)
        {
            byte[] buffer = new byte[sizeof(long)];
            stream.Read(buffer, 0, sizeof(long));
            return BitConverter.ToInt64(buffer, 0);
        }

        #endregion

        #region void WriteAllBytes(this string filePath, byte[] bytes)

        public static void WriteAllBytes(this string filePath, byte[] bytes)
        {
            File.WriteAllBytes(filePath, bytes);
        }

        #endregion

        #region string Icon(this Note note)

        public static string Icon(this Document document)
        {
            return document.Icon ?? DefaultDocumentIcon(document);
        }

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

        public static string HashEncryptionPassword(this string password)
        {
            return ("{0A47ABEC-38CC-43B3-BC73-52690F107F17}" + password).Md5();
        }

        #endregion

        #region bool GetFileExtention(this string filePath)

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

        public static void WriteAllText(this string filePath, string text)
        {
            File.WriteAllText(filePath, text);
        }

        #endregion

        #region T GetAttribute<T>(this Type type)

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

        public static bool DirectoryExists(this string path)
        {
            return Directory.Exists(path);
        }

        #endregion

        #region string ToSafeFileName(this string source)

        public static string ToSafeFileName(this string source)
        {
            return source.Replace(":", "").Replace("/", "").Replace(@"\", "").Replace("..", "").Replace("?", "");
        }

        #endregion

        #region IEnumerable<Note> Sort(this IEnumerable<Note> notes)

        public static IEnumerable<Document> Sort(this IEnumerable<Document> notes)
        {
            var sorted = new List<Document>(notes);
            sorted.Sort((x, y) => x.CompareTo(y));
            return sorted;
        }

        #endregion

        #region int NaturalCompareTo(this string x, string y)

        public static int NaturalCompareTo(this string x, string y)
        {
            x = x ?? "";
            y = y ?? "";
            Dictionary<string, string[]> table = new Dictionary<string, string[]>();
            if (x == y)
                return 0;
            double dx, dy;
            if (double.TryParse(x, out dx) && double.TryParse(y, out dy))
                return dx.CompareTo(dy);
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

        #region string[] SplitWithWhitespaces(this string source, string s)

        public static string[] SplitWithWhitespaces(this string source, string s)
        {
            if (source == null)
                return new string[0];
            if (source == "")
                return new[] { "" };
            return source.Split(new[] { s }, StringSplitOptions.None);
        }

        #endregion

        #region string DefaultValue(this string source, string defaultValue)

        public static string DefaultValue(this string source, string defaultValue)
        {
            return source.HasValue() ? source : defaultValue;
        }

        #endregion
    }
}
