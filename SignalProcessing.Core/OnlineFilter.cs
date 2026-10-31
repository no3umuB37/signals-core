using System;

namespace SignalProcessing.Core
{
    public class OnlineFilter
    {
        private readonly int _windowSize;
        private readonly CircularBuffer<float> _window;
        private double _sum;
        private bool _started;

        public OnlineFilter(int windowSize)
        {
            if (windowSize <= 0)
                throw new ArgumentOutOfRangeException(nameof(windowSize), "–азмер окна должен быть положительным числом.");
            _windowSize = windowSize;
            _window = new CircularBuffer<float>(windowSize);
        }

        public float[] Filter(float[] data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            float[] result = new float[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                float value = data[i];
                if (!_started)
                {
                    for (int j = 0; j < _windowSize - 1; j++)
                        _window.Add(value);
                    _sum = (double)(_windowSize - 1) * value;
                    _started = true;
                }

                if (_window.IsFull)
                    _sum -= _window[0];
                _window.Add(value);
                _sum += value;
                result[i] = (float)(_sum / _windowSize);
            }
            return result;
        }

        public void Reset()
        {
            _window.Clear();
            _sum = 0;
            _started = false;
        }
    }
}
