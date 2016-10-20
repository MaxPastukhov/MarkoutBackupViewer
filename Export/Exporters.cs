using System;
using System.Collections.Generic;

namespace MarkoutBackupViewer.Export
{
    public static class Exporters
    {
        static Exporters()
        {
            foreach (var type in typeof (Exporters).Assembly.GetTypes())
            {
                var attribute = type.GetAttribute<ExporterAttribute>();
                if (attribute != null)
                {
                    var exporter = Activator.CreateInstance(type) as Exporter;
                    if (exporter != null)
                    {
                        exporter.ID = attribute.ID;
                        exporter.Name = attribute.Name;
                        Items.Add(exporter);
                    }
                }
            }
        }

        private static readonly List<Exporter> Items = new List<Exporter>();

        public static IEnumerable<Exporter> All
        {
            get { return Items; }
        }
    }
}
