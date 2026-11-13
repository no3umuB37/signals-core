using System;
using Xunit;

namespace SignalProcessing.Core.Tests
{
    public class CircularBufferTests
    {
        [Fact]
        public void Add_BeyondCapacity_OldestElementsAreDropped()
        {
            var buffer = new CircularBuffer<int>(3);

            for (int i = 1; i <= 5; i++)
                buffer.Add(i);

            Assert.Equal(3, buffer.Count);
            Assert.Equal(new[] { 3, 4, 5 }, buffer.ToArray());
            Assert.Equal(3, buffer[0]);
            Assert.Equal(5, buffer[2]);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(2)]
        public void Indexer_OutOfRange_Throws(int index)
        {
            var buffer = new CircularBuffer<int>(5);
            buffer.Add(10);
            buffer.Add(20);

            Assert.Throws<ArgumentOutOfRangeException>(() => buffer[index]);
        }

        [Fact]
        public void Constructor_NewBuffer_IsEmpty()
        {
            var buffer = new CircularBuffer<int>(4);

            Assert.Equal(4, buffer.Capacity);
            Assert.Equal(0, buffer.Count);
            Assert.False(buffer.IsFull);
            Assert.Empty(buffer.ToArray());
        }

        [Fact]
        public void Add_FewerThanCapacity_KeepsOrderAndIsNotFull()
        {
            var buffer = new CircularBuffer<int>(4);

            buffer.Add(7);
            buffer.Add(8);

            Assert.Equal(2, buffer.Count);
            Assert.False(buffer.IsFull);
            Assert.Equal(new[] { 7, 8 }, buffer.ToArray());
        }

        [Fact]
        public void Add_ExactlyCapacity_BufferIsFull()
        {
            var buffer = new CircularBuffer<int>(3);

            buffer.Add(1);
            buffer.Add(2);
            buffer.Add(3);

            Assert.True(buffer.IsFull);
        }

        [Fact]
        public void Add_CapacityOne_KeepsOnlyLastElement()
        {
            var buffer = new CircularBuffer<int>(1);

            buffer.Add(1);
            buffer.Add(2);
            buffer.Add(3);

            Assert.Equal(1, buffer.Count);
            Assert.Equal(new[] { 3 }, buffer.ToArray());
        }

        [Fact]
        public void Add_ManyElements_KeepsLastCapacityElements()
        {
            var buffer = new CircularBuffer<int>(7);

            for (int i = 1; i <= 1000; i++)
                buffer.Add(i);

            Assert.Equal(new[] { 994, 995, 996, 997, 998, 999, 1000 }, buffer.ToArray());
        }

        [Fact]
        public void Clear_AfterOverflow_RemovesAllElements()
        {
            var buffer = new CircularBuffer<int>(3);
            for (int i = 1; i <= 5; i++)
                buffer.Add(i);

            buffer.Clear();
            buffer.Add(42);

            Assert.Equal(1, buffer.Count);
            Assert.Equal(42, buffer[0]);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void Constructor_InvalidCapacity_Throws(int capacity)
        {
            Assert.Throws<ArgumentOutOfRangeException>("capacity", () => new CircularBuffer<int>(capacity));
        }

        [Fact]
        public void ToArray_ModifyingResult_DoesNotAffectBuffer()
        {
            var buffer = new CircularBuffer<int>(3);
            buffer.Add(1);
            buffer.Add(2);

            int[] copy = buffer.ToArray();
            copy[0] = 100;

            Assert.Equal(new[] { 1, 2 }, buffer.ToArray());
        }
    }
}
