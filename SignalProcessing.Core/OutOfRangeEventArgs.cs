using System;

namespace SignalProcessing.Core
{
    public sealed class OutOfRangeEventArgs : EventArgs
    {
        public OutOfRangeEventArgs(int startIndex, int endIndex)
        {
            StartIndex = startIndex;
            EndIndex = endIndex;
        }

        public int StartIndex { get; }
        public int EndIndex { get; }
    }
}

