using System;
using System.Collections.Generic;
using System.Linq;

namespace SignalProcessing.Core
{
    public static class Tools
    {
        public static SingleChannelSignal Line(float len, float freq, float k, float b)
        {
            int count = GetCount(len, freq);
            float[] y = Enumerable.Range(0, count).Select(i => k * i / freq + b).ToArray();
            return new SingleChannelSignal(y, freq);
        }

        public static SingleChannelSignal Sin(float len, float freq, float period, float phase, float scale = 1)
        {
            if (period <= 0)
                throw new ArgumentException("ѕериод должен быть положительным.", nameof(period));
            int count = GetCount(len, freq);
            float[] y = Enumerable.Range(0, count)
                .Select(i => scale * (float)Math.Sin((i / freq - phase) * 2 * Math.PI / period)).ToArray();
            return new SingleChannelSignal(y, freq);
        }

        public static SingleChannelSignal Noise(float len, float freq, float amplitude)
        {
            if (amplitude < 0)
                throw new ArgumentException("–азмах шума должен быть неотрицательным.", nameof(amplitude));
            Random random = new Random();
            int count = GetCount(len, freq);
            float[] y = Enumerable.Range(0, count).Select(i => (float)(random.NextDouble() * 2 - 1) * amplitude).ToArray();
            return new SingleChannelSignal(y, freq);
        }

        private static int GetCount(float len, float freq)
        {
            if (len <= 0 || freq <= 0)
                throw new ArgumentException("ƒлина и частота должны быть положительными.");
            return Math.Max(1, (int)Math.Round(len * freq, MidpointRounding.AwayFromZero));
        }
    }
}
