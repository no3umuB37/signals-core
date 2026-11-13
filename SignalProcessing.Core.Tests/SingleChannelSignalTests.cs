using System;
using Xunit;

namespace SignalProcessing.Core.Tests
{
    public class SingleChannelSignalTests
    {
        [Fact]
        public void Constructor_NullData_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>("data", () => new SingleChannelSignal(null, 100f));
        }

        [Theory]
        [InlineData(0f)]
        [InlineData(-1f)]
        public void Constructor_NonPositiveFrequency_ThrowsArgumentOutOfRangeException(float frequency)
        {
            Assert.Throws<ArgumentOutOfRangeException>("frequency", () => new SingleChannelSignal(new float[] { 1, 2 }, frequency));
        }

        [Fact]
        public void Constructor_EmptyData_CreatesSignalWithoutSamples()
        {
            SingleChannelSignal signal = new SingleChannelSignal(new float[0], 50f);

            Assert.Empty(signal.Data);
            Assert.Equal(50f, signal.Frequency, 1e-5f);
        }

        [Fact]
        public void Constructor_ValidArguments_StoresDataAndFrequency()
        {
            float[] data = { 1, 2, 3 };

            SingleChannelSignal signal = new SingleChannelSignal(data, 250f);

            AssertSignal.Equal(new float[] { 1, 2, 3 }, signal.Data);
            Assert.Equal(250f, signal.Frequency, 1e-5f);
        }
    }
}
