using System;
using Xunit;

namespace SignalProcessing.Core.Tests
{
    public class NoiseGeneratorTests
    {
        [Fact]
        public void Generate_FixedSeed_ValuesAreInsideRange()
        {
            float range = 0.5f;
            NoiseGenerator generator = new NoiseGenerator(range, () => new Random(42));
            SamplingParameters sampling = new SamplingParameters(100f, 1f);

            SingleChannelSignal signal = generator.Generate(sampling);

            foreach (float value in signal.Data)
            {
                Assert.True(value >= -range && value <= range,
                    "Значение " + value + " вышло за пределы [-" + range + ", " + range + "].");
            }
        }

        [Fact]
        public void Generate_SameSeedTwice_ReturnsSameResult()
        {
            SamplingParameters sampling = new SamplingParameters(100f, 1f);
            NoiseGenerator first = new NoiseGenerator(1f, () => new Random(42));
            NoiseGenerator second = new NoiseGenerator(1f, () => new Random(42));

            SingleChannelSignal firstSignal = first.Generate(sampling);
            SingleChannelSignal secondSignal = second.Generate(sampling);

            AssertSignal.Equal(firstSignal.Data, secondSignal.Data);
        }

        [Fact]
        public void Constructor_NegativeRange_Throws()
        {
            Assert.Throws<ArgumentOutOfRangeException>("range", () => new NoiseGenerator(-1f, null));
        }

        [Fact]
        public void Generate_NullRandomFactory_UsesDefaultSource()
        {
            NoiseGenerator generator = new NoiseGenerator(1f, null);
            SamplingParameters sampling = new SamplingParameters(10f, 1f);

            SingleChannelSignal signal = generator.Generate(sampling);

            Assert.Equal(10, signal.Data.Length);
        }

        [Fact]
        public void Generate_ZeroRange_ReturnsZeroSignal()
        {
            NoiseGenerator generator = new NoiseGenerator(0f, () => new Random(42));
            SamplingParameters sampling = new SamplingParameters(10f, 1f);

            SingleChannelSignal signal = generator.Generate(sampling);

            foreach (float value in signal.Data)
                Assert.Equal(0f, value, 1e-6f);
        }
    }
}
