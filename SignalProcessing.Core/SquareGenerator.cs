using System;

namespace SignalProcessing.Core
{
    public sealed class SquareGenerator : ISignalGenerator
    {
        private readonly float _period;
        private readonly float _phase;
        private readonly float _amplitude;
        private readonly float _dutyCycle;

        public SquareGenerator(float period, float phase, float amplitude, float dutyCycle)
        {
            if (period <= 0)
                throw new ArgumentOutOfRangeException(nameof(period), "Период должен быть положительным.");
            if (dutyCycle <= 0 || dutyCycle >= 1)
                throw new ArgumentOutOfRangeException(nameof(dutyCycle), "Скважность должна лежать в диапазоне от 0 до 1.");

            _period = period;
            _phase = phase;
            _amplitude = amplitude;
            _dutyCycle = dutyCycle;
        }

        public SingleChannelSignal Generate(SamplingParameters sampling)
        {
            if (sampling == null)
                throw new ArgumentNullException(nameof(sampling));

            float[] data = new float[sampling.SampleCount];
            for (int i = 0; i < data.Length; i++)
            {
                float t = sampling.TimeAt(i);
                float positionInPeriod = GetPositionInPeriod(t);
                if (positionInPeriod < _dutyCycle * _period)
                    data[i] = _amplitude;
                else
                    data[i] = -_amplitude;
            }
            return new SingleChannelSignal(data, sampling.SamplingRate);
        }

        private float GetPositionInPeriod(float time)
        {
            float position = (time - _phase) % _period;
            if (position < 0)
                position += _period;
            return position;
        }
    }
}
