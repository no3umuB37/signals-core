using System;
using Xunit;

namespace SignalProcessing.Core.Tests
{
    public class SineGeneratorTests
    {
        [Fact]
        public void Generate_Period1_ReturnsExpectedValuesAtKeyPoints()
        {
            SineGenerator generator = new SineGenerator(1f, 0f, 1f);
            SamplingParameters sampling = new SamplingParameters(100f, 1f);

            SingleChannelSignal signal = generator.Generate(sampling);

            // 0 ñ -> 0; 0,25 ñ -> 1; 0,5 ñ -> 0
            Assert.Equal(0f, signal.Data[0], 1e-6f);
            Assert.Equal(1f, signal.Data[25], 1e-6f);
            Assert.Equal(0f, signal.Data[50], 1e-6f);
        }

        [Fact]
        public void Generate_Amplitude2_ScalesValues()
        {
            SineGenerator generator = new SineGenerator(1f, 0f, 2f);
            SamplingParameters sampling = new SamplingParameters(100f, 1f);

            SingleChannelSignal signal = generator.Generate(sampling);

            Assert.Equal(2f, signal.Data[25], 1e-6f);
        }

        [Fact]
        public void Generate_ReturnsSignalWithSamplingRateAndCount()
        {
            SineGenerator generator = new SineGenerator(1f, 0f, 1f);
            SamplingParameters sampling = new SamplingParameters(100f, 2f);

            SingleChannelSignal signal = generator.Generate(sampling);

            Assert.Equal(200, signal.Data.Length);
            Assert.Equal(100f, signal.Frequency, 1e-6f);
        }

        [Theory]
        [InlineData(0f)]
        [InlineData(-1f)]
        public void Constructor_InvalidPeriod_Throws(float period)
        {
            Assert.Throws<ArgumentOutOfRangeException>("period", () => new SineGenerator(period, 0f, 1f));
        }

        [Fact]
        public void Generate_NullSampling_ThrowsArgumentNullException()
        {
            SineGenerator generator = new SineGenerator(1f, 0f, 1f);

            Assert.Throws<ArgumentNullException>("sampling", () => generator.Generate(null));
        }
    }
}
