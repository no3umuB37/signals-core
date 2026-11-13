using System;
using Xunit;

namespace SignalProcessing.Core.Tests
{
    public class ExponentialSmoothingFilterTests
    {
        [Fact]
        public void Filter_ConstantSignal_ReturnsSameConstant()
        {
            float[] data = { 5, 5, 5, 5 };

            float[] result = new ExponentialSmoothingFilter(0.3f).Filter(data);

            AssertSignal.Equal(data, result);
        }

        [Fact]
        public void Filter_AlphaOne_ReturnsInputSignal()
        {
            float[] data = { 1, -2, 3, 4 };

            float[] result = new ExponentialSmoothingFilter(1f).Filter(data);

            AssertSignal.Equal(data, result);
        }

        [Fact]
        public void Filter_StepSignal_FollowsExponentialLaw()
        {
            const float alpha = 0.25f;
            float[] data = { 0, 1, 1, 1, 1, 1 };

            float[] result = new ExponentialSmoothingFilter(alpha).Filter(data);

            for (int n = 1; n < data.Length; n++)
            {
                float expected = 1f - (float)Math.Pow(1 - alpha, n);
                Assert.Equal(expected, result[n], 1e-5f);
            }
        }

        [Theory]
        [InlineData(0f)]
        [InlineData(-0.5f)]
        [InlineData(1.5f)]
        public void Constructor_InvalidAlpha_Throws(float alpha)
        {
            Assert.Throws<ArgumentOutOfRangeException>("alpha", () => new ExponentialSmoothingFilter(alpha));
        }

        [Fact]
        public void Reset_AfterAnotherSignal_BehavesLikeNewFilter()
        {
            float[] second = { 10, 20, 30 };
            ExponentialSmoothingFilter reused = new ExponentialSmoothingFilter(0.5f);
            reused.Filter(new float[] { 1, 2, 3 });
            reused.Reset();

            float[] actual = reused.Filter(second);

            AssertSignal.Equal(new ExponentialSmoothingFilter(0.5f).Filter(second), actual);
        }
    }
}
