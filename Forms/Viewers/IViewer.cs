using MarkoutBackupViewer.Data;

namespace MarkoutBackupViewer.Forms.Viewers
{
    /// <summary>
    /// интерфейс вьювера документа
    /// </summary>
    public interface IViewer
    {
        /// <summary>
        /// документ
        /// </summary>
        Document Document { set; }
    }
}
