using System;

namespace SignalProcessing.Core
{
    public sealed class TriangleGenerator : ISignalGenerator
    {
        private readonly float _period;
        private readonly float _phase;
        private readonly float _amplitude;

        public TriangleGenerator(float period, float phase, float amplitude)
        {
            if (period <= 0)
                throw new ArgumentOutOfRangeException(nameof(period), "Период должен быть положительным.");

            _period = period;
            _phase = phase;
            _amplitude = amplitude;
        }

        public SingleChannelSignal Generate(SamplingParameters sampling)
        {
            if (sampling == null)
                throw new ArgumentNullException(nameof(sampling));

            float[] data = new float[sampling.SampleCount];
            for (int i = 0; i < data.Length; i++)
            {
                float t = sampling.TimeAt(i);
                float position = (t - _phase) % _period;
                if (position < 0)
                    position += _period;

                float half = _period / 2;
                if (position < half)
                    data[i] = _amplitude * (2 * position / half - 1);
                else
                    data[i] = _amplitude * (1 - 2 * (position - half) / half);
            }
            return new SingleChannelSignal(data, sampling.SamplingRate);
        }
    }
}
