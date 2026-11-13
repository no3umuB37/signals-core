using System.Linq;

namespace SignalProcessing.Core
{
    public class ThresholdCutter
    {
        private SingleChannelSignal _signal;

        public void SetSignal(SingleChannelSignal signal)
        {
            _signal = signal;
        }

        public SingleChannelSignal GetSignal()
        {
            return _signal;
        }

        public void CutUp(float upThr)
        {
            for (int i = 0; i < _signal.Data.Count(); i++)
            {
                if (_signal.Data[i] > upThr)
                {
                    _signal.Data[i] = upThr;
                }
            }
        }
        public void CutDown(float downThr)
        {
            for (int i = 0; i < _signal.Data.Count(); i++)
            {
                if (_signal.Data[i] < downThr)
                {
                    _signal.Data[i] = downThr;
                }
            }
        }
        public void CutUpAndDown(float upThr, float downThr)
        {
            for (int i = 0; i < _signal.Data.Count(); i++)
            {
                if (_signal.Data[i] > upThr)
                {
                    _signal.Data[i] = upThr;
                }
                else if (_signal.Data[i] < downThr)
                {
                    _signal.Data[i] = downThr;
                }
            }
        }
    }
}

