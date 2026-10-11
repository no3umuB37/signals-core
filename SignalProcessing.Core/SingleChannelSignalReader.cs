using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace SignalProcessing.Core
{
    {
                    string line = reader.ReadLine();
                    if (line == null)
                        break;
                    value.Add(float.Parse(line, CultureInfo.InvariantCulture));
                }
                float[] arrSignal = value.ToArray();
                SingleChannelSignal data = new SingleChannelSignal(freq, arrSignal);
                return data;
            }
        }
    }
}

