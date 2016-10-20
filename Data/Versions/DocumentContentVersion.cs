namespace MarkoutBackupViewer.Data.Versions
{
    public class DocumentContentVersion : DocumentPropertyVersions<DocumentContentVersion>.Version
    {
        public string Key
        {
            get { return this["Key"] ?? Document["ContentKey"]; }
        }
        
        public int Size
        {
            get { return this["Size"].ToInt32(Document["ContentLength"].ToInt32()); }
        }

        public byte[] Content
        {
            get
            {
                if (!Key.HasValue())
                    return null;
                return Document.Backup.GetContent(Key);
            }
        }
    }
}
