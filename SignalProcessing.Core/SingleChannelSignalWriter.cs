using System.Globalization;
using System.IO;

namespace SignalProcessing.Core
{
    public static class SingleChannelSignalWriter
    {
        public static void Write(SingleChannelSignal obj, string filePath)
        {
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine(obj.Freq.ToString(CultureInfo.InvariantCulture));
                foreach (double element in obj.Value)
                {
                    writer.WriteLine(element.ToString(CultureInfo.InvariantCulture));
                }
            }
        }
    }
}

