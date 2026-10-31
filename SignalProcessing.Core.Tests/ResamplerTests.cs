using System;
using Xunit;

namespace SignalProcessing.Core.Tests
{
    public class ResamplerTests
    {
        [Fact]
        public void Resample_SameFrequency_ReturnsSameSamples()
        {
            SingleChannelSignal signal = new SingleChannelSignal(new float[] { 1, 2, 3, 4 }, 100f);

            SingleChannelSignal result = Resampler.Resample(signal, 100f);

            AssertSignal.Equal(new float[] { 1, 2, 3, 4 }, result.Data);
            Assert.Equal(100f, result.Frequency, 1e-5f);
        }

        [Fact]
        public void Resample_UpsampleByTwo_InterpolatesMidpoints()
        {
            SingleChannelSignal signal = new SingleChannelSignal(new float[] { 0, 2, 4, 8 }, 1f);

            SingleChannelSignal result = Resampler.Resample(signal, 2f);

            AssertSignal.Equal(new float[] { 0, 1, 2, 3, 4, 6, 8 }, result.Data);
            Assert.Equal(2f, result.Frequency, 1e-5f);
        }

        [Fact]
        public void Resample_DownsampleByTwo_KeepsEverySecondSample()
        {
            SingleChannelSignal signal = new SingleChannelSignal(new float[] { 0, 1, 2, 3, 4 }, 2f);

            SingleChannelSignal result = Resampler.Resample(signal, 1f);

            AssertSignal.Equal(new float[] { 0, 2, 4 }, result.Data);
        }

        [Fact]
        public void Resample_NonIntegerRatio_InterpolatesCorrectly()
        {
            SingleChannelSignal signal = new SingleChannelSignal(new float[] { 0, 3, 6, 9 }, 3f);

            SingleChannelSignal result = Resampler.Resample(signal, 2f);

            AssertSignal.Equal(new float[] { 0, 4.5f, 9 }, result.Data);
        }

        [Fact]
        public void Resample_LinearSignal_RemainsLinear()
        {
            const float sourceFrequency = 100f;
            const float targetFrequency = 37f;
            float[] data = new float[100];
            for (int i = 0; i < data.Length; i++)
                data[i] = 2f * (i / sourceFrequency) + 1f;
            SingleChannelSignal signal = new SingleChannelSignal(data, sourceFrequency);

            SingleChannelSignal result = Resampler.Resample(signal, targetFrequency);

            for (int j = 0; j < result.Data.Length; j++)
            {
                float time = j / targetFrequency;
                Assert.Equal(2f * time + 1f, result.Data[j], 1e-4f);
            }
        }

        [Fact]
        public void Resample_SingleSample_ReturnsSameSample()
        {
            SingleChannelSignal signal = new SingleChannelSignal(new float[] { 7 }, 100f);

            SingleChannelSignal result = Resampler.Resample(signal, 50f);

            AssertSignal.Equal(new float[] { 7 }, result.Data);
        }

        [Fact]
        public void Resample_EmptySignal_ReturnsEmptySignal()
        {
            SingleChannelSignal signal = new SingleChannelSignal(new float[0], 100f);

            SingleChannelSignal result = Resampler.Resample(signal, 50f);

            Assert.Empty(result.Data);
            Assert.Equal(50f, result.Frequency, 1e-5f);
        }

        [Theory]
        [InlineData(0f)]
        [InlineData(-1f)]
        public void Resample_InvalidFrequency_Throws(float newFrequency)
        {
            SingleChannelSignal signal = new SingleChannelSignal(new float[] { 1, 2 }, 100f);

            Assert.Throws<ArgumentOutOfRangeException>("newFrequency", () => Resampler.Resample(signal, newFrequency));
        }

        [Fact]
        public void Resample_NullSignal_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>("signal", () => Resampler.Resample(null, 50f));
        }
    }
}
