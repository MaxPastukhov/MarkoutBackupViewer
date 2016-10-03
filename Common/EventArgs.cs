using System;

namespace MarkoutBackupViewer.Common
{
    /// <summary>
    /// типизированные аргументы
    /// </summary>
    public class EventArgs<T> : EventArgs
    {
        public EventArgs(T value)
        {
            Value = value;
        }

        public readonly T Value;
    }
}
