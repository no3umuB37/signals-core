using System;
using System.Globalization;
using System.IO;

namespace SignalProcessing.Core
{
    public static class SingleChannelSignalWriter
    {
        public static void Write(SingleChannelSignal signal, string filePath)
        {
            if (signal == null)
                throw new ArgumentNullException(nameof(signal));
            if (filePath == null)
                throw new ArgumentNullException(nameof(filePath));

            using (StreamWriter writer = new StreamWriter(filePath))
            {
                writer.WriteLine(signal.Frequency.ToString("R", CultureInfo.InvariantCulture));
                foreach (float element in signal.Data)
                {
                    writer.WriteLine(element.ToString("R", CultureInfo.InvariantCulture));
                }
            }
        }
    }
}

