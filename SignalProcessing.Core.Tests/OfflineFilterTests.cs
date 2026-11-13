using System;
using Xunit;

namespace SignalProcessing.Core.Tests
{
    public class OfflineFilterTests
    {
        [Fact]
        public void Filter_ConstantSignal_ReturnsSameConstant()
        {
            // Arrange
            float[] data = { 5, 5, 5, 5, 5 };

            // Act
            float[] result = OfflineFilter.Filter(data, 3);

            // Assert
            Assert.Equal(data.Length, result.Length);
            foreach (float value in result)
                Assert.Equal(5f, value, 1e-5f);
        }

        [Fact]
        public void Filter_RampSignal_ReturnsExpectedValues()
        {
            float[] data = { 1, 2, 3, 4, 5 };

            float[] result = OfflineFilter.Filter(data, 3);

            AssertSignal.Equal(new float[] { 4f / 3f, 2, 3, 4, 14f / 3f }, result);
        }

        [Fact]
        public void Filter_Impulse_SpreadsOverWindow()
        {
            float[] data = { 0, 0, 0, 1, 0, 0, 0 };

            float[] result = OfflineFilter.Filter(data, 3);

            AssertSignal.Equal(new float[] { 0, 0, 1f / 3f, 1f / 3f, 1f / 3f, 0, 0 }, result);
        }

        [Fact]
        public void Filter_WindowSizeOne_ReturnsCopyOfInput()
        {
            float[] data = { 1, -2, 3 };

            float[] result = OfflineFilter.Filter(data, 1);

            AssertSignal.Equal(data, result);
            Assert.NotSame(data, result);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(4)]
        public void Filter_InvalidWindowSize_Throws(int windowSize)
        {
            float[] data = { 1, 2, 3 };

            Assert.Throws<ArgumentOutOfRangeException>("windowSize", () => OfflineFilter.Filter(data, windowSize));
        }

        [Fact]
        public void Filter_NullData_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>("data", () => OfflineFilter.Filter(null, 3));
        }

        [Fact]
        public void Filter_EmptyData_ReturnsEmptyArray()
        {
            float[] result = OfflineFilter.Filter(new float[0], 3);

            Assert.Empty(result);
        }

        [Fact]
        public void Filter_AnySignal_DoesNotModifyInput()
        {
            float[] data = { 1, 2, 3, 4, 5 };

            OfflineFilter.Filter(data, 3);

            AssertSignal.Equal(new float[] { 1, 2, 3, 4, 5 }, data);
        }
    }
}
