using System;
using System.Collections.Generic;

namespace SignalProcessing.Core
{
    public static class SignalOperations
    {
        private const float FrequencyTolerance = 1e-5f;

        public static SingleChannelSignal Sum(IReadOnlyList<SingleChannelSignal> signals)
        {
            float frequency = Validate(signals);

            int count = 0;
            for (int i = 0; i < signals.Count; i++)
                count = Math.Max(count, signals[i].Data.Length);

            float[] result = new float[count];
            for (int i = 0; i < signals.Count; i++)
            {
                float[] data = signals[i].Data;
                for (int j = 0; j < data.Length; j++)
                    result[j] += data[j];
            }
            return new SingleChannelSignal(result, frequency);
        }

        public static SingleChannelSignal Concat(IReadOnlyList<SingleChannelSignal> signals)
        {
            float frequency = Validate(signals);

            int count = 0;
            for (int i = 0; i < signals.Count; i++)
                count += signals[i].Data.Length;

            float[] result = new float[count];
            int offset = 0;
            for (int i = 0; i < signals.Count; i++)
            {
                float[] data = signals[i].Data;
                Array.Copy(data, 0, result, offset, data.Length);
                offset += data.Length;
            }
            return new SingleChannelSignal(result, frequency);
        }

        private static float Validate(IReadOnlyList<SingleChannelSignal> signals)
        {
            if (signals == null)
                throw new ArgumentNullException(nameof(signals));
            if (signals.Count == 0)
                throw new ArgumentException("—писок сигналов пуст.", nameof(signals));

            for (int i = 0; i < signals.Count; i++)
            {
                if (signals[i] == null)
                    throw new ArgumentNullException(nameof(signals), "—писок сигналов содержит null.");
            }

            float frequency = signals[0].Frequency;
            for (int i = 1; i < signals.Count; i++)
            {
                float current = signals[i].Frequency;
                if (Math.Abs(current - frequency) > FrequencyTolerance)
                    throw new ArgumentException(
                        "„астоты сигналов различаютс€: " + frequency + " √ц и " + current + " √ц.",
                        nameof(signals));
            }
            return frequency;
        }
    }
}
