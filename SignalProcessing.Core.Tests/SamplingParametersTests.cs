using System;
using Xunit;

namespace SignalProcessing.Core.Tests
{
    public class SamplingParametersTests
    {
        [Fact]
        public void SampleCount_Rate100Duration2_Returns200()
        {
            SamplingParameters sampling = new SamplingParameters(100f, 2f);

            Assert.Equal(200, sampling.SampleCount);
        }

        [Theory]
        [InlineData(0f)]
        [InlineData(-1f)]
        public void Constructor_InvalidSamplingRate_Throws(float samplingRate)
        {
            Assert.Throws<ArgumentOutOfRangeException>("samplingRate", () => new SamplingParameters(samplingRate, 2f));
        }

        [Theory]
        [InlineData(0f)]
        [InlineData(-1f)]
        public void Constructor_InvalidDuration_Throws(float duration)
        {
            Assert.Throws<ArgumentOutOfRangeException>("duration", () => new SamplingParameters(100f, duration));
        }

        [Fact]
        public void TimeAt_Rate100_ReturnsTimeInSeconds()
        {
            SamplingParameters sampling = new SamplingParameters(100f, 1f);

            Assert.Equal(0f, sampling.TimeAt(0), 1e-6f);
            Assert.Equal(0.25f, sampling.TimeAt(25), 1e-6f);
            Assert.Equal(0.5f, sampling.TimeAt(50), 1e-6f);
        }

        [Fact]
        public void Constructor_ValidArguments_StoresValues()
        {
            SamplingParameters sampling = new SamplingParameters(250f, 4f);

            Assert.Equal(250f, sampling.SamplingRate, 1e-6f);
            Assert.Equal(4f, sampling.Duration, 1e-6f);
        }
    }
}
