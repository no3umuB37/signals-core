using System;
using System.Collections.Generic;
using Xunit;

namespace SignalProcessing.Core.Tests
{
    public class MedianFilterTests
    {
        private static float[] CreateRandomSignal(int length)
        {
            Random random = new Random(42);
            float[] data = new float[length];
            for (int i = 0; i < length; i++)
                data[i] = (float)(random.NextDouble() * 2 - 1);
            return data;
        }

        [Fact]
        public void OfflineFilter_SingleOutlier_IsRemoved()
        {
            float[] data = { 1, 1, 1, 9, 1, 1, 1 };

            float[] result = OfflineMedianFilter.Filter(data, 3);

            AssertSignal.Equal(new float[] { 1, 1, 1, 1, 1, 1, 1 }, result);
        }

        [Fact]
        public void OfflineFilter_StepSignal_KeepsSharpEdge()
        {
            float[] data = { 0, 0, 0, 1, 1, 1 };

            float[] result = OfflineMedianFilter.Filter(data, 3);

            AssertSignal.Equal(data, result);
        }

        [Fact]
        public void OfflineMeanFilter_StepSignal_SmoothsEdge()
        {
            float[] data = { 0, 0, 0, 1, 1, 1 };

            float[] result = OfflineFilter.Filter(data, 3);

            Assert.False(AreEqual(data, result), "—кольз€щее среднее должно размывать ступеньку.");
        }

        private static bool AreEqual(float[] expected, float[] actual)
        {
            if (expected.Length != actual.Length)
                return false;
            for (int i = 0; i < expected.Length; i++)
            {
                if (Math.Abs(expected[i] - actual[i]) > 1e-5f)
                    return false;
            }
            return true;
        }

        [Fact]
        public void OnlineFilter_ComparedWithOffline_IsDelayedByHalfWindow()
        {
            float[] data = CreateRandomSignal(1000);
            const int windowSize = 7;
            const int delay = (windowSize - 1) / 2;
            float[] online = new OnlineMedianFilter(windowSize).Filter(data);
            float[] offline = OfflineMedianFilter.Filter(data, windowSize);

            for (int i = delay; i < data.Length; i++)
                Assert.Equal(offline[i - delay], online[i], 1e-5f);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(7)]
        [InlineData(100)]
        public void OnlineFilter_ChunkedInput_ResultDoesNotDependOnSplitting(int chunkSize)
        {
            float[] data = CreateRandomSignal(1000);
            float[] expected = new OnlineMedianFilter(7).Filter(data);

            OnlineMedianFilter filter = new OnlineMedianFilter(7);
            List<float> actual = new List<float>();
            for (int offset = 0; offset < data.Length; offset += chunkSize)
            {
                int length = Math.Min(chunkSize, data.Length - offset);
                float[] chunk = new float[length];
                Array.Copy(data, offset, chunk, 0, length);
                actual.AddRange(filter.Filter(chunk));
            }

            AssertSignal.Equal(expected, actual.ToArray());
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(4)]
        public void OfflineFilter_InvalidWindowSize_Throws(int windowSize)
        {
            Assert.Throws<ArgumentOutOfRangeException>("windowSize", () => OfflineMedianFilter.Filter(new float[] { 1, 2, 3 }, windowSize));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(4)]
        public void OnlineFilter_InvalidWindowSize_Throws(int windowSize)
        {
            Assert.Throws<ArgumentOutOfRangeException>("windowSize", () => new OnlineMedianFilter(windowSize));
        }
    }
}
