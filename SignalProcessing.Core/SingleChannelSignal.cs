using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SignalProcessing.Core
{
    public class SingleChannelSignal
    {
        public SingleChannelSignal(float freq, float[] value)
        {
            Freq = freq;
            Value = value;
        }
        public float Freq { get; }
        public float[] Value { get; }
        public float[] TimeTick
        {
            get
            {
                float[] timeTick = new float[Value.Length];
                for (int i = 0; i < Value.Length; i++)
    {
                    timeTick[i] = i / Freq;
                }
                return timeTick;
            }
        }
    }
}

