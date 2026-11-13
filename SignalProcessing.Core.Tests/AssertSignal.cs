using System.Globalization;
using Xunit;

namespace SignalProcessing.Core.Tests
{
    internal static class AssertSignal
    {
        public static void Equal(float[] expected, float[] actual, float tolerance = 1e-5f)
        {
            Assert.NotNull(actual);
            Assert.Equal(expected.Length, actual.Length);
            for (int i = 0; i < expected.Length; i++)
            {
                float difference = expected[i] - actual[i];
                if (difference < 0)
                    difference = -difference;
                Assert.True(
                    difference <= tolerance,
                    string.Format(
                        CultureInfo.InvariantCulture,
                        "Расхождение по индексу {0}: ожидалось {1}, получено {2} (допуск {3}).",
                        i, expected[i], actual[i], tolerance));
            }
        }
    }
}
