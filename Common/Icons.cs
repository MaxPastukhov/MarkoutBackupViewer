using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace MarkoutBackupViewer.Common
{
    public class Icons
    {
        public Icons(string spriteFilePath, string listFilePath)
        {
            var sprite = new Bitmap(spriteFilePath);
            var names = File.ReadAllText(listFilePath).Split("\n").Select(x => x.Trim()).Where(x => x.HasValue()).Select(x => x + ".png").ToArray();
            for (int i = 0; i < names.Length; i++)
            {
                var icon = new Bitmap(16, 16);
                var gr = Graphics.FromImage(icon);
                gr.DrawImage(sprite, 0, 0, new Rectangle(0, i * 16, 16, 16), GraphicsUnit.Pixel);
                ImageList.Images.Add(names[i], icon);
            }
        }

        public readonly ImageList ImageList = new ImageList();
        
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
