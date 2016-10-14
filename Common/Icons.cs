using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MarkoutBackupViewer.Common
{
    /// <summary>
    /// иконки
    /// </summary>
    public class Icons
    {
        /// <summary>
        /// конструктор
        /// </summary>
        /// <param name="spriteFilePath">путь к файлу со спрайтом</param>
        /// <param name="listFilePath">путь к файлу со списком</param>
        public Icons(string spriteFilePath, string listFilePath)
        {
            // подгрузим спрайт
            var sprite = new Bitmap(spriteFilePath);
            // построим словарь индексов иконок в спрайте
            var names = File.ReadAllText(listFilePath).Split("\n").Select(x => x.Trim()).Where(x => x.HasValue()).Select(x => x + ".png").ToArray();
            for (int i = 0; i < names.Length; i++)
            {
                var icon = new Bitmap(16, 16);
                var gr = Graphics.FromImage(icon);
                gr.DrawImage(sprite, 0, 0, new Rectangle(0, i * 16, 16, 16), GraphicsUnit.Pixel);
                ImageList.Images.Add(names[i], icon);
            }
        }

        /// <summary>
        /// список подгруженных картинок
        /// </summary>
        public readonly ImageList ImageList = new ImageList();

        /// <summary>
        /// удобный вариант доступа
        /// </summary>
        /// <param name="name"></param>
        /// <param name="defaultName"></param>
        /// <returns></returns>
        public int this[string name, string defaultName]
        {
            get
            {
                var index = ImageList.Images.IndexOfKey(name);
                if (index < 0)
                    return ImageList.Images.IndexOfKey(defaultName);
                return index;
            }
        }
    }
}
