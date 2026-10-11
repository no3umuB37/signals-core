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
                throw new ArgumentException("Размер окна должен быть положительным нечётным числом.", nameof(windowSize));
            if (data.Length == 0)
                return new float[0];
            int radius = (windowSize - 1) / 2;
            float[] extended = Enumerable.Repeat(data[0], radius).Concat(data).Concat(Enumerable.Repeat(data[data.Length - 1], radius)).ToArray();
            return Enumerable.Range(0, data.Length)
                .Select(i => (float)extended.Skip(i).Take(windowSize).Average()).ToArray();
        }
    }
}
