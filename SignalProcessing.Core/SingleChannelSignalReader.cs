using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace SignalProcessing.Core
{
    public static class SingleChannelSignalReader
    {
        public static SingleChannelSignal Load(string filePath)
        {
            using (StreamReader reader = new StreamReader(filePath))
            {
                float freq = float.Parse(reader.ReadLine(), CultureInfo.InvariantCulture);
                List<float> value = new List<float>();
                while (true)
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

