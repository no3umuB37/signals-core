using System;

namespace SignalProcessing.Core
{
    public static class OfflineMedianFilter
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
            float[] extended = new float[data.Length + 2 * radius];
            for (int i = 0; i < radius; i++)
            {
                extended[i] = data[0];
                extended[extended.Length - 1 - i] = data[data.Length - 1];
            }
            Array.Copy(data, 0, extended, radius, data.Length);

            float[] result = new float[data.Length];
            float[] window = new float[windowSize];
            for (int i = 0; i < data.Length; i++)
            {
                Array.Copy(extended, i, window, 0, windowSize);
                Array.Sort(window);
                result[i] = window[radius];
            }
            return result;
        }
    }
}
