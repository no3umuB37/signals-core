using System;

namespace SignalProcessing.Core
{
    public static class Resampler
    {
        public static SingleChannelSignal Resample(SingleChannelSignal signal, float newFrequency)
        {
            if (signal == null)
                throw new ArgumentNullException(nameof(signal));
            if (newFrequency <= 0)
                throw new ArgumentOutOfRangeException(nameof(newFrequency), "„астота квантовани€ должна быть положительной.");

            float[] source = signal.Data;
            if (source.Length == 0)
                return new SingleChannelSignal(new float[0], newFrequency);

            double ratio = (double)signal.Frequency / newFrequency;
            int count = (int)Math.Floor((source.Length - 1) * (double)newFrequency / signal.Frequency + 1e-6) + 1;

            float[] result = new float[count];
            int last = source.Length - 1;
            for (int j = 0; j < count; j++)
            {
                double position = j * ratio;
                int k = (int)Math.Floor(position);
                if (k >= last)
                {
                    result[j] = source[last];
                    continue;
                }
                double alpha = position - k;
                result[j] = (float)(source[k] + alpha * (source[k + 1] - source[k]));
            }
            return new SingleChannelSignal(result, newFrequency);
        }
    }
}
