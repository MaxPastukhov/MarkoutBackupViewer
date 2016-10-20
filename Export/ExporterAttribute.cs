using System;

namespace MarkoutBackupViewer.Export
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class ExporterAttribute : Attribute
    {
        public ExporterAttribute(string id, string name)
        {
            ID = id;
            Name = name;
        }

        public readonly string ID;

        public readonly string Name;
    }
}
