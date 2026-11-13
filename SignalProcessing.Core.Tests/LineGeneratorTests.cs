using System;
using Xunit;

namespace SignalProcessing.Core.Tests
{
    public class LineGeneratorTests
    {
        [Fact]
        public void Generate_K2B1_ReturnsLineValues()
        {
            LineGenerator generator = new LineGenerator(2f, 1f);
            SamplingParameters sampling = new SamplingParameters(100f, 1f);

            SingleChannelSignal signal = generator.Generate(sampling);

            for (int i = 0; i < signal.Data.Length; i++)
            {
                float t = sampling.TimeAt(i);
                Assert.Equal(2f * t + 1f, signal.Data[i], 1e-6f);
            }
        }

        [Fact]
        public void Generate_ZeroSlope_ReturnsConstant()
        {
            LineGenerator generator = new LineGenerator(0f, 5f);
            SamplingParameters sampling = new SamplingParameters(10f, 1f);

            SingleChannelSignal signal = generator.Generate(sampling);

            foreach (float value in signal.Data)
                Assert.Equal(5f, value, 1e-6f);
        }

        [Fact]
        public void Generate_NullSampling_ThrowsArgumentNullException()
        {
            LineGenerator generator = new LineGenerator(1f, 0f);

            Assert.Throws<ArgumentNullException>("sampling", () => generator.Generate(null));
        }
    }
}
