using System;
using Xunit;

namespace SignalProcessing.Core.Tests
{
    public class SignalGeneratorFactoryTests
    {
        private sealed class FakeParameters : GeneratorParameters
        {
        }

        [Fact]
        public void Create_SineParameters_ReturnsSineGenerator()
        {
            GeneratorParameters parameters = new SineParameters { Period = 1, Phase = 0, Amplitude = 1 };

            ISignalGenerator generator = SignalGeneratorFactory.Create(parameters);

            Assert.IsType<SineGenerator>(generator);
        }

        [Fact]
        public void Create_LineParameters_ReturnsLineGenerator()
        {
            GeneratorParameters parameters = new LineParameters { K = 2, B = 1 };

            ISignalGenerator generator = SignalGeneratorFactory.Create(parameters);

            Assert.IsType<LineGenerator>(generator);
        }

        [Fact]
        public void Create_NoiseParameters_ReturnsNoiseGenerator()
        {
            GeneratorParameters parameters = new NoiseParameters { Range = 1 };

            ISignalGenerator generator = SignalGeneratorFactory.Create(parameters);

            Assert.IsType<NoiseGenerator>(generator);
        }

        [Fact]
        public void Create_SquareParameters_ReturnsSquareGenerator()
        {
            GeneratorParameters parameters = new SquareParameters { Period = 1, Phase = 0, Amplitude = 1, DutyCycle = 0.5f };

            ISignalGenerator generator = SignalGeneratorFactory.Create(parameters);

            Assert.IsType<SquareGenerator>(generator);
        }

        [Fact]
        public void Create_SawParameters_ReturnsSawGenerator()
        {
            GeneratorParameters parameters = new SawParameters { Period = 1, Phase = 0, Amplitude = 1 };

            ISignalGenerator generator = SignalGeneratorFactory.Create(parameters);

            Assert.IsType<SawGenerator>(generator);
        }

        [Fact]
        public void Create_TriangleParameters_ReturnsTriangleGenerator()
        {
            GeneratorParameters parameters = new TriangleParameters { Period = 1, Phase = 0, Amplitude = 1 };

            ISignalGenerator generator = SignalGeneratorFactory.Create(parameters);

            Assert.IsType<TriangleGenerator>(generator);
        }

        [Fact]
        public void Create_ConstantParameters_ReturnsConstantGenerator()
        {
            GeneratorParameters parameters = new ConstantParameters { Value = 5 };

            ISignalGenerator generator = SignalGeneratorFactory.Create(parameters);

            Assert.IsType<ConstantGenerator>(generator);
        }

        [Fact]
        public void Create_Null_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>("parameters", () => SignalGeneratorFactory.Create(null));
        }

        [Fact]
        public void Create_UnsupportedParameters_ThrowsNotSupportedException()
        {
            GeneratorParameters parameters = new FakeParameters();

            NotSupportedException exception = Assert.Throws<NotSupportedException>(
                () => SignalGeneratorFactory.Create(parameters));

            Assert.Contains("FakeParameters", exception.Message);
        }
    }
}
