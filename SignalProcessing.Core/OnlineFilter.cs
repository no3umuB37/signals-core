using System;
using System.Collections.Generic;

namespace SignalProcessing.Core
{
    public sealed class OnlineFilter
    {
        private readonly int _windowSize;
        private readonly Queue<float> _history = new Queue<float>();
        private double _sum;

        public OnlineFilter(int windowSize)
        {
            if (windowSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(windowSize));
            _windowSize = windowSize;
        }

        public float[] Filter(float[] data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            float[] result = new float[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                _history.Enqueue(data[i]);
                _sum += data[i];
                if (_history.Count > _windowSize)
                    _sum -= _history.Dequeue();
                result[i] = (float)(_sum / _windowSize);
            }
            return result;
        }
    }
}
