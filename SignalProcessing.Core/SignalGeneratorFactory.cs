using System;

namespace SignalProcessing.Core
{
    public static class SignalGeneratorFactory
    {
        public static ISignalGenerator Create(GeneratorParameters parameters)
        {
            if (parameters == null)
                throw new ArgumentNullException(nameof(parameters));

            switch (parameters)
            {
                case SineParameters sine:
                    return new SineGenerator(sine.Period, sine.Phase, sine.Amplitude);

                case LineParameters line:
                    return new LineGenerator(line.K, line.B);

                case NoiseParameters noise:
                    return new NoiseGenerator(noise.Range, () => new Random());

                case SquareParameters square:
                    return new SquareGenerator(square.Period, square.Phase, square.Amplitude, square.DutyCycle);

                case SawParameters saw:
                    return new SawGenerator(saw.Period, saw.Phase, saw.Amplitude);

                case TriangleParameters triangle:
                    return new TriangleGenerator(triangle.Period, triangle.Phase, triangle.Amplitude);

                case ConstantParameters constant:
                    return new ConstantGenerator(constant.Value);

                default:
                    throw new NotSupportedException(
                        $"Неподдерживаемый тип: {parameters.GetType().Name}");
            }
        }
    }
}
