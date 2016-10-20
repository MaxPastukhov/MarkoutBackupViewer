using System;

namespace MarkoutBackupViewer.Common
{
    public class EventArgs<T> : EventArgs
    {
        public EventArgs(T value)
        {
            Value = value;
        }

        public readonly T Value;
    }
}
