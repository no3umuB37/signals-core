using System;

namespace SignalProcessing.Core
{
    public class SingleChannelSignal
    {
        public SingleChannelSignal(float[] data, float frequency)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (frequency <= 0)
                throw new ArgumentOutOfRangeException(nameof(frequency), "„астота квантовани€ должна быть положительной.");
            Data = data;
            Frequency = frequency;
        }

        public float[] Data { get; }

        public float Frequency { get; }

        public float[] TimeTick
        {
            get
            {
                float[] timeTick = new float[Data.Length];
                for (int i = 0; i < Data.Length; i++)
                {
                    timeTick[i] = i / Frequency;
                }
                return timeTick;
            }
        }
    }
}

