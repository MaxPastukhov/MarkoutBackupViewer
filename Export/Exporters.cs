using System;
using System.Collections.Generic;

namespace MarkoutBackupViewer.Export
{
    /// <summary>
    /// список вариантов экспорта бекапов
    /// </summary>
    public static class Exporters
    {
        /// <summary>
        /// конструктор
        /// </summary>
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

        /// <summary>
        /// варианты экспорта
        /// </summary>
        private static readonly List<Exporter> Items = new List<Exporter>();

        /// <summary>
        /// все доступные варианты экспорта бекапов
        /// </summary>
        public static IEnumerable<Exporter> All
        {
            get { return Items; }
        }
    }
}
