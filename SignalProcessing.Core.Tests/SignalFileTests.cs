using System;
using System.Globalization;
using System.IO;
using System.Threading;
using Xunit;

namespace SignalProcessing.Core.Tests
{
    public class SignalFileTests : IDisposable
    {
        private readonly string _filePath;

        public SignalFileTests()
        {
            _filePath = Path.GetTempFileName();
        }

        public void Dispose()
        {
            if (File.Exists(_filePath))
                File.Delete(_filePath);
        }

        [Fact]
        public void WriteThenRead_Signal_RestoresFrequencyAndSamples()
        {
            SingleChannelSignal signal = new SingleChannelSignal(new float[] { 0.5f, -1.25f, 3f, 0.1f }, 250f);

            SingleChannelSignalWriter.Write(signal, _filePath);
            SingleChannelSignal restored = SingleChannelSignalReader.Read(_filePath);

            Assert.Equal(250f, restored.Frequency, 1e-5f);
            AssertSignal.Equal(signal.Data, restored.Data);
        }

        [Fact]
        public void Write_RussianCulture_UsesInvariantDecimalSeparator()
        {
            SingleChannelSignal signal = new SingleChannelSignal(new float[] { 0.5f, -1.25f, 3f }, 250f);
            CultureInfo previous = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("ru-RU");
            try
            {
                SignalProcessing.Core.SingleChannelSignalWriter.Write(signal, _filePath);
                string[] lines = File.ReadAllLines(_filePath);
                Assert.Equal(new[] { "250", "0.5", "-1.25", "3" }, lines);
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = previous;
            }
        }

        [Fact]
        public void Read_RussianCulture_ParsesInvariantDecimalSeparator()
        {
            File.WriteAllLines(_filePath, new[] { "100", "0.5", "-1.5" });
            CultureInfo previous = Thread.CurrentThread.CurrentCulture;
            Thread.CurrentThread.CurrentCulture = new CultureInfo("ru-RU");
            try
            {
                SingleChannelSignal signal = SingleChannelSignalReader.Read(_filePath);

                Assert.Equal(100f, signal.Frequency, 1e-5f);
                AssertSignal.Equal(new float[] { 0.5f, -1.5f }, signal.Data);
            }
            finally
            {
                Thread.CurrentThread.CurrentCulture = previous;
            }
        }

        [Fact]
        public void Read_FileWithFrequencyOnly_ReturnsSignalWithoutSamples()
        {
            File.WriteAllLines(_filePath, new[] { "100" });

            SingleChannelSignal signal = SingleChannelSignalReader.Read(_filePath);

            Assert.Equal(100f, signal.Frequency, 1e-5f);
            Assert.Empty(signal.Data);
        }

        [Fact]
        public void Read_EmptyFile_ThrowsInvalidDataException()
        {
            File.WriteAllText(_filePath, string.Empty);

            Assert.Throws<InvalidDataException>(() => SingleChannelSignalReader.Read(_filePath));
        }

        [Fact]
        public void Read_NonNumericValue_ThrowsInvalidDataException()
        {
            File.WriteAllLines(_filePath, new[] { "100", "0.5", "abc" });

            InvalidDataException exception = Assert.Throws<InvalidDataException>(() => SingleChannelSignalReader.Read(_filePath));
            Assert.Contains("3", exception.Message);
        }

        [Fact]
        public void Read_NonPositiveFrequency_ThrowsInvalidDataException()
        {
            File.WriteAllLines(_filePath, new[] { "0", "0.5" });

            Assert.Throws<InvalidDataException>(() => SingleChannelSignalReader.Read(_filePath));
        }
    }
}
