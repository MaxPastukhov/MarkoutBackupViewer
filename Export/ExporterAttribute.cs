using System;

namespace MarkoutBackupViewer.Export
{
    /// <summary>
    /// атрибут для описания экспортеров
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class ExporterAttribute : Attribute
    {
        /// <summary>
        /// конструктор
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        public ExporterAttribute(string id, string name)
        {
            ID = id;
            Name = name;
        }

        /// <summary>
        /// идентификатор
        /// </summary>
        public readonly string ID;

        /// <summary>
        /// название
        /// </summary>
        public readonly string Name;
    }
}
