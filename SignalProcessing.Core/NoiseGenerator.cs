using System;

namespace SignalProcessing.Core
{
    public sealed class NoiseGenerator : ISignalGenerator
    {
        private readonly float _range;
        private readonly Func<Random> _randomFactory;

        public NoiseGenerator(float range, Func<Random> randomFactory)
        {
            if (range < 0)
                throw new ArgumentOutOfRangeException(nameof(range), "–азмах шума должен быть неотрицательным.");

            _range = range;
            _randomFactory = randomFactory;
        }

        public SingleChannelSignal Generate(SamplingParameters sampling)
        {
            if (sampling == null)
                throw new ArgumentNullException(nameof(sampling));

            Random random;
            if (_randomFactory == null)
                random = new Random();
            else
                random = _randomFactory();

            float[] data = new float[sampling.SampleCount];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = (float)(random.NextDouble() * 2 - 1) * _range;
            }
            return new SingleChannelSignal(data, sampling.SamplingRate);
        }
    }
}
