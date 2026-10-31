using System;
using System.Collections.Generic;
using Xunit;

namespace SignalProcessing.Core.Tests
{
    public class SignalOperationsTests
    {
        [Fact]
        public void Sum_SignalsOfDifferentLength_PadsShorterWithZeros()
        {
            SingleChannelSignal first = new SingleChannelSignal(new float[] { 1, 2, 3 }, 100f);
            SingleChannelSignal second = new SingleChannelSignal(new float[] { 10, 20 }, 100f);

            SingleChannelSignal result = SignalOperations.Sum(new[] { first, second });

            AssertSignal.Equal(new float[] { 11, 22, 3 }, result.Data);
            Assert.Equal(100f, result.Frequency, 1e-5f);
        }

        [Fact]
        public void Concat_TwoSignals_JoinsSamplesInOrder()
        {
            SingleChannelSignal first = new SingleChannelSignal(new float[] { 1, 2 }, 100f);
            SingleChannelSignal second = new SingleChannelSignal(new float[] { 3 }, 100f);

            SingleChannelSignal result = SignalOperations.Concat(new[] { first, second });

            AssertSignal.Equal(new float[] { 1, 2, 3 }, result.Data);
            Assert.Equal(100f, result.Frequency, 1e-5f);
        }

        [Fact]
        public void Sum_DifferentFrequencies_ThrowsArgumentException()
        {
            SingleChannelSignal first = new SingleChannelSignal(new float[] { 1 }, 100f);
            SingleChannelSignal second = new SingleChannelSignal(new float[] { 2 }, 200f);

            Assert.Throws<ArgumentException>("signals", () => SignalOperations.Sum(new[] { first, second }));
        }

        [Fact]
        public void Concat_DifferentFrequencies_ThrowsArgumentException()
        {
            SingleChannelSignal first = new SingleChannelSignal(new float[] { 1 }, 100f);
            SingleChannelSignal second = new SingleChannelSignal(new float[] { 2 }, 200f);

            Assert.Throws<ArgumentException>("signals", () => SignalOperations.Concat(new[] { first, second }));
        }

        [Fact]
        public void Sum_EmptyList_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>("signals", () => SignalOperations.Sum(new List<SingleChannelSignal>()));
        }

        [Fact]
        public void Concat_EmptyList_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>("signals", () => SignalOperations.Concat(new List<SingleChannelSignal>()));
        }

        [Fact]
        public void Sum_NullList_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>("signals", () => SignalOperations.Sum(null));
        }

        [Fact]
        public void Concat_NullList_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>("signals", () => SignalOperations.Concat(null));
        }

        [Fact]
        public void Sum_ListContainingNull_ThrowsArgumentNullException()
        {
            SingleChannelSignal signal = new SingleChannelSignal(new float[] { 1 }, 100f);

            Assert.Throws<ArgumentNullException>("signals", () => SignalOperations.Sum(new[] { signal, null }));
        }

        [Fact]
        public void Concat_ListContainingNull_ThrowsArgumentNullException()
        {
            SingleChannelSignal signal = new SingleChannelSignal(new float[] { 1 }, 100f);

            Assert.Throws<ArgumentNullException>("signals", () => SignalOperations.Concat(new[] { signal, null }));
        }

        [Fact]
        public void Sum_AnySignals_DoesNotModifySources()
        {
            SingleChannelSignal first = new SingleChannelSignal(new float[] { 1, 2, 3 }, 100f);
            SingleChannelSignal second = new SingleChannelSignal(new float[] { 10, 20 }, 100f);

            SignalOperations.Sum(new[] { first, second });

            AssertSignal.Equal(new float[] { 1, 2, 3 }, first.Data);
            AssertSignal.Equal(new float[] { 10, 20 }, second.Data);
        }
    }
}
