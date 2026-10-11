namespace SignalProcessing.Core
{
    public sealed class RangeAnnotation
    {
        public RangeAnnotation(int startIndex, int endIndex)
        {
            StartIndex = startIndex;
            EndIndex = endIndex;
        }

        public int StartIndex { get; }
        public int EndIndex { get; }
    }
}

