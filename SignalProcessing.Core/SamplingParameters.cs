using System;

namespace SignalProcessing.Core
{
    public sealed class SamplingParameters
    {
        public float SamplingRate { get; }

        public float Duration { get; }

        public SamplingParameters(float samplingRate, float duration)
        {
            if (samplingRate <= 0)
                throw new ArgumentOutOfRangeException(nameof(samplingRate), "„астота квантовани€ должна быть положительной.");
            if (duration <= 0)
                throw new ArgumentOutOfRangeException(nameof(duration), "ƒлительность должна быть положительной.");

            SamplingRate = samplingRate;
            Duration = duration;
        }

        public int SampleCount
        {
            get
            {
                int count = (int)Math.Round(Duration * SamplingRate, MidpointRounding.AwayFromZero);
                if (count < 1)
                    count = 1;
                return count;
            }
        }

        public float TimeAt(int index)
        {
            return index / SamplingRate;
        }
    }
}
