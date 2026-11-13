namespace SignalProcessing.Core
{
    public interface ISignalGenerator
    {
        SingleChannelSignal Generate(SamplingParameters sampling);
    }
}
