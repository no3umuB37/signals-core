using System;

namespace SignalProcessing.Core
{
    public sealed class LineGenerator : ISignalGenerator
    {
        private readonly float _k;
        private readonly float _b;

        public LineGenerator(float k, float b)
        {
            _k = k;
            _b = b;
        }

        public SingleChannelSignal Generate(SamplingParameters sampling)
        {
            if (sampling == null)
                throw new ArgumentNullException(nameof(sampling));

            float[] data = new float[sampling.SampleCount];
            for (int i = 0; i < data.Length; i++)
            {
                float t = sampling.TimeAt(i);
                data[i] = _k * t + _b;
            }
            return new SingleChannelSignal(data, sampling.SamplingRate);
        }
    }
}
