using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace SignalProcessing.Core
{
    public static class SingleChannelSignalReader
    {
        public static SingleChannelSignal Read(string filePath)
        {
            if (filePath == null)
                throw new ArgumentNullException(nameof(filePath));

            using (StreamReader reader = new StreamReader(filePath))
            {
                string frequencyLine = reader.ReadLine();
                if (frequencyLine == null)
                    throw new InvalidDataException("Файл сигнала пуст.");

                float frequency;
                if (!float.TryParse(frequencyLine, NumberStyles.Float, CultureInfo.InvariantCulture, out frequency))
                    throw new InvalidDataException("Строка 1: частота квантования не является числом.");
                if (frequency <= 0)
                    throw new InvalidDataException("Строка 1: частота квантования должна быть положительной.");

                List<float> values = new List<float>();
                int lineNumber = 1;
                while (true)
                {
                    string line = reader.ReadLine();
                    if (line == null)
                        break;
                    lineNumber++;
                    if (line.Length == 0)
                        continue;
                    float value;
                    if (!float.TryParse(line, NumberStyles.Float, CultureInfo.InvariantCulture, out value))
                        throw new InvalidDataException("Строка " + lineNumber + ": значение не является числом.");
                    values.Add(value);
                }

                return new SingleChannelSignal(values.ToArray(), frequency);
            }
        }
    }
}

