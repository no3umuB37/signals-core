using System;
using System.Linq;

namespace SignalProcessing.Core
{
    public static class OfflineFilter
    {
        public static float[] Filter(float[] data, int windowSize)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            if (windowSize <= 0 || windowSize % 2 == 0)
                throw new ArgumentOutOfRangeException(nameof(windowSize), "Размер окна должен быть положительным нечётным числом.");
            if (data.Length == 0)
                return new float[0];
            int radius = (windowSize - 1) / 2;
            float[] extended = Enumerable.Repeat(data[0], radius).Concat(data).Concat(Enumerable.Repeat(data[data.Length - 1], radius)).ToArray();
            float[] result = new float[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                double sum = 0;
                for (int j = 0; j < windowSize; j++)
                    sum += extended[i + j];
                result[i] = (float)(sum / windowSize);
            }
            return result;
        }
    }
}
