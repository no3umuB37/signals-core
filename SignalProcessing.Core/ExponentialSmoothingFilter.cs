using System;

namespace SignalProcessing.Core
{
    public class ExponentialSmoothingFilter
    {
        private readonly float _alpha;
        private float _previous;
        private bool _started;

        public ExponentialSmoothingFilter(float alpha)
        {
            if (alpha <= 0 || alpha > 1)
                throw new ArgumentOutOfRangeException(nameof(alpha), "Коэффициент сглаживания должен лежать в диапазоне (0, 1].");
            _alpha = alpha;
        }

        public float[] Filter(float[] data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            float[] result = new float[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                if (!_started)
                {
                    _previous = data[i];
                    _started = true;
                }
                else
                {
                    _previous = _previous + _alpha * (data[i] - _previous);
                }
                result[i] = _previous;
            }
            return result;
        }

        public void Reset()
        {
            _previous = 0;
            _started = false;
        }
    }
}
