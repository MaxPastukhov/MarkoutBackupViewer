using System;
using System.Collections.Generic;

namespace MarkoutBackupViewer.Data.Versions
{
    public class DocumentTextVersion : DocumentPropertyVersions<DocumentTextVersion>.Version
    {
        public int Prefix
        {
            get { return this["Prefix"].ToInt32(); }
        }

        public int Suffix
        {
            get { return this["Suffix"].ToInt32(); }
        }

        public IEnumerable<string> Deleted
        {
            get
            {
                var prevLines = new List<string>(Previous != null ? Previous.Text.SplitWithWhitespaces("\n") : new string[0]);
                for (int i = 0; i < prevLines.Count - Prefix - Suffix; i++)
                    yield return prevLines[Prefix + i];
            }
        }

        public string[] Added
        {
            get { return Document.Backup.Decrypt(this["Added"]).SplitWithWhitespaces("\n"); }
        }

        public virtual IEnumerable<string> Changelog
        {
            get
            {
                foreach (var deleted in Deleted)
                    yield return "-" + deleted;
                foreach (var added in Added)
                    yield return "+" + added;
            }
            set { throw new NotSupportedException(); }
        }
        
        public virtual string Text
        {
            get
            {
                if (Prefix > 0 || Suffix > 0)
                {
                    var prevLines = new List<string>(Previous != null ? Previous.Text.SplitWithWhitespaces("\n") : new string[0]);
                    prevLines.RemoveRange(Prefix, prevLines.Count - Prefix - Suffix);
                    prevLines.InsertRange(Prefix, Added);
                    return prevLines.Join("\n");
                }
                return Added.Join("\n").DefaultValue(null);
            }
        }
    }
}
