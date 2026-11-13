using System;

namespace SignalProcessing.Core
{
    public sealed class ConstantGenerator : ISignalGenerator
    {
        private readonly float _value;

        public ConstantGenerator(float value)
        {
            _value = value;
        }

        public SingleChannelSignal Generate(SamplingParameters sampling)
        {
            if (sampling == null)
                throw new ArgumentNullException(nameof(sampling));

            float[] data = new float[sampling.SampleCount];
            for (int i = 0; i < data.Length; i++)
            {
                data[i] = _value;
            }
            return new SingleChannelSignal(data, sampling.SamplingRate);
        }
    }
}
