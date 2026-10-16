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
            return new SingleChannelSignal(freq, y);
        }

        public static SingleChannelSignal Sin(float len, float freq, float period, float phase, float scale = 1)
        {
            if (period <= 0)
                throw new ArgumentException("ѕериод должен быть положительным.", nameof(period));
            int count = GetCount(len, freq);
            float[] y = Enumerable.Range(0, count)
                .Select(i => scale * (float)Math.Sin((i / freq - phase) * 2 * Math.PI / period)).ToArray();
            return new SingleChannelSignal(freq, y);
        }

        public static SingleChannelSignal Noise(float len, float freq, float amplitude)
        {
            if (amplitude < 0)
                throw new ArgumentException("–азмах шума должен быть неотрицательным.", nameof(amplitude));
            Random random = new Random();
            int count = GetCount(len, freq);
            float[] y = Enumerable.Range(0, count).Select(i => (float)(random.NextDouble() * 2 - 1) * amplitude).ToArray();
            return new SingleChannelSignal(freq, y);
        }

        public static SingleChannelSignal Summ(List<SingleChannelSignal> signalList)
        {
            if (signalList == null || signalList.Count == 0)
                return null;
            float freq = signalList[0].Freq;
            if (signalList.Any(s => Math.Abs(s.Freq - freq) > 0.0000001))
                return null;
            int count = signalList.Select(s => s.Value.Length).Max();
            float[] result = new float[count];
            foreach (SingleChannelSignal signal in signalList)
                for (int i = 0; i < signal.Value.Length; i++)
                    result[i] += signal.Value[i];
            return new SingleChannelSignal(freq, result);
        }

        public static SingleChannelSignal Concat(List<SingleChannelSignal> signalList)
        {
            if (signalList == null || signalList.Count == 0)
                return null;
            float freq = signalList[0].Freq;
            if (signalList.Any(s => Math.Abs(s.Freq - freq) > 0.0000001))
                return null;
            return new SingleChannelSignal(freq, signalList.SelectMany(s => s.Value).ToArray());
        }

        private static int GetCount(float len, float freq)
        {
            if (len <= 0 || freq <= 0)
                throw new ArgumentException("ƒлина и частота должны быть положительными.");
            return Math.Max(1, (int)Math.Round(len * freq, MidpointRounding.AwayFromZero));
        }
    }
}
