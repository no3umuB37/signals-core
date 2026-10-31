using System;

namespace SignalProcessing.Core
{
    public class OnlineMedianFilter
    {
        private readonly int _windowSize;
        private readonly CircularBuffer<float> _window;
        private bool _started;

        public OnlineMedianFilter(int windowSize)
        {
            if (windowSize <= 0 || windowSize % 2 == 0)
                throw new ArgumentOutOfRangeException(nameof(windowSize), "Размер окна должен быть положительным нечётным числом.");
            _windowSize = windowSize;
            _window = new CircularBuffer<float>(windowSize);
        }

        public float[] Filter(float[] data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            float[] result = new float[data.Length];
            int middle = (_windowSize - 1) / 2;
            for (int i = 0; i < data.Length; i++)
            {
                float value = data[i];
                if (!_started)
                {
                    for (int j = 0; j < _windowSize - 1; j++)
                        _window.Add(value);
                    _started = true;
                }
                _window.Add(value);

                float[] sorted = _window.ToArray();
                Array.Sort(sorted);
                result[i] = sorted[middle];
            }
            return result;
        }

        public void Reset()
        {
            _window.Clear();
            _started = false;
        }
    }
}
