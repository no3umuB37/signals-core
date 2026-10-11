using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalProcessing.Core
{
    public sealed class SingleChannelRangeAnalyzer
    {
        private readonly float _minValue;
        private readonly float _maxValue;
        private readonly int _minDuration;

        public SingleChannelRangeAnalyzer(float minValue, float maxValue, int minDuration)
        {
            _minValue = minValue;
            _maxValue = maxValue;
            _minDuration = minDuration;
        }

        public event EventHandler<OutOfRangeEventArgs> OutOfRange;

        public void Analyze(float[] data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            int start = -1;
            for (int i = 0; i <= data.Length; i++)
            {
                bool outside = i < data.Length && (data[i] < _minValue || data[i] > _maxValue);
                if (outside && start < 0)
                    start = i;
                if (!outside && start >= 0)
                {
                    int end = i - 1;
                    if (end - start + 1 >= _minDuration)
                        OutOfRange?.Invoke(this, new OutOfRangeEventArgs(start, end));
                    start = -1;
                }
            }
        }
    }
}
