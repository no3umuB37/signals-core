using System;
using Xunit;

namespace SignalProcessing.Core.Tests
{
    public class SquareGeneratorTests
    {
        [Fact]
        public void Generate_DutyCycleHalf_SwitchesLevelInMiddleOfPeriod()
        {
            SquareGenerator generator = new SquareGenerator(1f, 0f, 2f, 0.5f);
            SamplingParameters sampling = new SamplingParameters(100f, 1f);

            SingleChannelSignal signal = generator.Generate(sampling);

            // Первая половина периода - верхний уровень, вторая - нижний.
            Assert.Equal(2f, signal.Data[0], 1e-6f);
            Assert.Equal(2f, signal.Data[25], 1e-6f);
            Assert.Equal(-2f, signal.Data[50], 1e-6f);
            Assert.Equal(-2f, signal.Data[75], 1e-6f);
        }

        [Theory]
        [InlineData(0f)]
        [InlineData(-1f)]
        public void Constructor_InvalidPeriod_Throws(float period)
        {
            Assert.Throws<ArgumentOutOfRangeException>("period", () => new SquareGenerator(period, 0f, 1f, 0.5f));
        }

        [Theory]
        [InlineData(0f)]
        [InlineData(1f)]
        [InlineData(-0.5f)]
        public void Constructor_InvalidDutyCycle_Throws(float dutyCycle)
        {
            Assert.Throws<ArgumentOutOfRangeException>("dutyCycle", () => new SquareGenerator(1f, 0f, 1f, dutyCycle));
        }
    }

    public class SawGeneratorTests
    {
        [Fact]
        public void Generate_Period1_RisesFromMinusAmplitudeToAmplitude()
        {
            SawGenerator generator = new SawGenerator(1f, 0f, 1f);
            SamplingParameters sampling = new SamplingParameters(100f, 1f);

            SingleChannelSignal signal = generator.Generate(sampling);

            Assert.Equal(-1f, signal.Data[0], 1e-6f);
            Assert.Equal(0f, signal.Data[50], 1e-6f);
            Assert.Equal(0.98f, signal.Data[99], 1e-6f);
        }

        [Theory]
        [InlineData(0f)]
        [InlineData(-1f)]
        public void Constructor_InvalidPeriod_Throws(float period)
        {
            Assert.Throws<ArgumentOutOfRangeException>("period", () => new SawGenerator(period, 0f, 1f));
        }
    }

    public class TriangleGeneratorTests
    {
        [Fact]
        public void Generate_Period1_RisesThenFalls()
        {
            TriangleGenerator generator = new TriangleGenerator(1f, 0f, 1f);
            SamplingParameters sampling = new SamplingParameters(100f, 1f);

            SingleChannelSignal signal = generator.Generate(sampling);

            Assert.Equal(-1f, signal.Data[0], 1e-6f);
            Assert.Equal(0f, signal.Data[25], 1e-6f);
            Assert.Equal(1f, signal.Data[50], 1e-6f);
            Assert.Equal(0f, signal.Data[75], 1e-6f);
        }

        [Theory]
        [InlineData(0f)]
        [InlineData(-1f)]
        public void Constructor_InvalidPeriod_Throws(float period)
        {
            Assert.Throws<ArgumentOutOfRangeException>("period", () => new TriangleGenerator(period, 0f, 1f));
        }
    }

    public class ConstantGeneratorTests
    {
        [Fact]
        public void Generate_Value5_AllSamplesEqual5()
        {
            ConstantGenerator generator = new ConstantGenerator(5f);
            SamplingParameters sampling = new SamplingParameters(100f, 1f);

            SingleChannelSignal signal = generator.Generate(sampling);

            Assert.Equal(100, signal.Data.Length);
            foreach (float value in signal.Data)
                Assert.Equal(5f, value, 1e-6f);
        }

        [Fact]
        public void Generate_NullSampling_ThrowsArgumentNullException()
        {
            ConstantGenerator generator = new ConstantGenerator(1f);

            Assert.Throws<ArgumentNullException>("sampling", () => generator.Generate(null));
        }
    }
}
