using System;
using System.Collections.Generic;
using Xunit;

namespace SignalProcessing.Core.Tests
{
    public class OnlineFilterTests
    {
        private static float[] CreateRandomSignal(int length, double min = -1, double max = 1)
        {
            Random random = new Random(42);
            float[] data = new float[length];
            for (int i = 0; i < length; i++)
                data[i] = (float)(min + random.NextDouble() * (max - min));
            return data;
        }

        private static float[] FilterInChunks(OnlineFilter filter, float[] data, int chunkSize)
        {
            List<float> result = new List<float>();
            for (int offset = 0; offset < data.Length; offset += chunkSize)
            {
                int length = Math.Min(chunkSize, data.Length - offset);
                float[] chunk = new float[length];
                Array.Copy(data, offset, chunk, 0, length);
                result.AddRange(filter.Filter(chunk));
            }
            return result.ToArray();
        }

        [Fact]
        public void Filter_RampSignalSingleCall_ReturnsExpectedValues()
        {
            OnlineFilter filter = new OnlineFilter(3);

            float[] result = filter.Filter(new float[] { 1, 2, 3, 4, 5 });

            AssertSignal.Equal(new float[] { 1, 4f / 3f, 2, 3, 4 }, result);
        }

        [Fact]
        public void Filter_RampSignalTwoCalls_ReturnsSameAsSingleCall()
        {
            OnlineFilter filter = new OnlineFilter(3);

            float[] first = filter.Filter(new float[] { 1, 2 });
            float[] second = filter.Filter(new float[] { 3, 4, 5 });

            List<float> combined = new List<float>(first);
            combined.AddRange(second);
            AssertSignal.Equal(new float[] { 1, 4f / 3f, 2, 3, 4 }, combined.ToArray());
        }

        [Theory]
        [InlineData(1)]
        [InlineData(7)]
        [InlineData(100)]
        public void Filter_ChunkedInput_ResultDoesNotDependOnSplitting(int chunkSize)
        {
            float[] data = CreateRandomSignal(1000);
            float[] expected = new OnlineFilter(7).Filter(data);

            float[] actual = FilterInChunks(new OnlineFilter(7), data, chunkSize);

            AssertSignal.Equal(expected, actual);
        }

        [Fact]
        public void Filter_ComparedWithOfflineFilter_IsDelayedByHalfWindow()
        {
            float[] data = CreateRandomSignal(1000);
            const int windowSize = 7;
            const int delay = (windowSize - 1) / 2;
            float[] online = new OnlineFilter(windowSize).Filter(data);
            float[] offline = OfflineFilter.Filter(data, windowSize);

            for (int i = delay; i < data.Length; i++)
                Assert.Equal(offline[i - delay], online[i], 1e-5f);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Constructor_InvalidWindowSize_Throws(int windowSize)
        {
            Assert.Throws<ArgumentOutOfRangeException>("windowSize", () => new OnlineFilter(windowSize));
        }

        [Fact]
        public void Filter_NullData_ThrowsArgumentNullException()
        {
            OnlineFilter filter = new OnlineFilter(3);

            Assert.Throws<ArgumentNullException>("data", () => filter.Filter(null));
        }

        [Fact]
        public void Reset_AfterAnotherSignal_BehavesLikeNewFilter()
        {
            float[] second = { 10, 20, 30, 40 };
            OnlineFilter reused = new OnlineFilter(3);
            reused.Filter(new float[] { 1, 2, 3, 4, 5 });
            reused.Reset();

            float[] actual = reused.Filter(second);

            float[] expected = new OnlineFilter(3).Filter(second);
            AssertSignal.Equal(expected, actual);
        }

        [Fact]
        public void Filter_WindowSizeOne_ReturnsInputSignal()
        {
            float[] data = { 1, -2, 3, 4 };

            float[] result = new OnlineFilter(1).Filter(data);

            AssertSignal.Equal(data, result);
        }

        [Fact]
        public void Filter_LongSignalWithLargeValues_MatchesDoublePrecisionReference()
        {
            const int windowSize = 7;
            float[] data = CreateRandomSignal(200000, 900, 1100);

            float[] actual = new OnlineFilter(windowSize).Filter(data);

            float[] expected = new float[data.Length];
            for (int i = 0; i < data.Length; i++)
            {
                double sum = 0;
                for (int j = 0; j < windowSize; j++)
                {
                    int index = i - windowSize + 1 + j;
                    sum += index < 0 ? data[0] : data[index];
                }
                expected[i] = (float)(sum / windowSize);
            }
            AssertSignal.Equal(expected, actual, 1e-3f);
        }
    }
}
