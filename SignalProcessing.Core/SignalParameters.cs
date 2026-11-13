namespace SignalProcessing.Core
{
    public sealed class SineParameters : GeneratorParameters
    {
        public float Period { get; set; }

        public float Phase { get; set; }

        public float Amplitude { get; set; }
    }

    public sealed class LineParameters : GeneratorParameters
    {
        public float K { get; set; }

        public float B { get; set; }
    }

    public sealed class NoiseParameters : GeneratorParameters
    {
        public float Range { get; set; }
    }

    public sealed class SquareParameters : GeneratorParameters
    {
        public float Period { get; set; }

        public float Phase { get; set; }

        public float Amplitude { get; set; }

        public float DutyCycle { get; set; }
    }

    public sealed class SawParameters : GeneratorParameters
    {
        public float Period { get; set; }

        public float Phase { get; set; }

        public float Amplitude { get; set; }
    }

    public sealed class TriangleParameters : GeneratorParameters
    {
        public float Period { get; set; }

        public float Phase { get; set; }

        public float Amplitude { get; set; }
    }

    public sealed class ConstantParameters : GeneratorParameters
    {
        public float Value { get; set; }
    }
}
