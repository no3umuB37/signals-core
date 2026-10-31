using System;

namespace SignalProcessing.Core
{
    public sealed class CircularBuffer<T>
    {
        private readonly T[] _items;
        private int _start;
        private int _count;

        public CircularBuffer(int capacity)
        {
            if (capacity <= 0)
                throw new ArgumentOutOfRangeException(nameof(capacity), "Ёмкость буфера должна быть положительной.");
            _items = new T[capacity];
            _start = 0;
            _count = 0;
        }

        public int Capacity
        {
            get { return _items.Length; }
        }

        public int Count
        {
            get { return _count; }
        }

        public bool IsFull
        {
            get { return _count == _items.Length; }
        }

        public void Add(T item)
        {
            if (_count == _items.Length)
            {
                _items[_start] = item;
                _start = _start + 1 == _items.Length ? 0 : _start + 1;
            }
            else
            {
                int index = _start + _count;
                if (index >= _items.Length)
                    index -= _items.Length;
                _items[index] = item;
                _count++;
            }
        }

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= _count)
                    throw new ArgumentOutOfRangeException(nameof(index), "Индекс вне диапазона хранимых элементов.");
                int position = _start + index;
                if (position >= _items.Length)
                    position -= _items.Length;
                return _items[position];
            }
        }

        public T[] ToArray()
        {
            T[] result = new T[_count];
            for (int i = 0; i < _count; i++)
            {
                int position = _start + i;
                if (position >= _items.Length)
                    position -= _items.Length;
                result[i] = _items[position];
            }
            return result;
        }

        public void Clear()
        {
            Array.Clear(_items, 0, _items.Length);
            _start = 0;
            _count = 0;
        }
    }
}
