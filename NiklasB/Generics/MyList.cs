using System;
using System.Collections;
using System.Collections.Generic;

namespace Generics
{
    class MyList<T> : IEnumerable<T>
    {
        T[] _items;
        int _count;

        public int Count => _count;

        public T this[int i]
        {
            get { return _items[i]; }
            set { _items[i] = value; }
        }

        public void Add(T item)
        {
            EnsureCapacity(_count + 1);
            _items[_count++] = item;
        }

        void EnsureCapacity(int minCapacity)
        {
            if (_items == null)
            {
                // We don't have an array so allocate it with an initial size
                // of eight or the specified minimum, whichever is greater.
                int c = Math.Max(8, minCapacity);
                _items = new T[c];
            }
            else if (_items.Length < minCapacity)
            {
                // We have an array, but it's not big enough. Allocate a new 
                // array that's double the size of the old one or the specified
                // minimum, whichever is greater. We double the size of the array
                // each time instead of increasing the size by a constant amount
                // in order to ensure that the amortized cost of Add is O(1). See
                // https://en.wikipedia.org/wiki/Amortized_analysis.
                int c = Math.Max(_items.Length * 2, minCapacity);
                var newItems = new T[c];
                Array.Copy(_items, newItems, _count);
                _items = newItems;
            }
        }

        class Enumerator : IEnumerator<T>
        {
            T[] _items;
            int _count;
            int _index = -1;

            public Enumerator(T[] items, int count)
            {
                _items = items;
                _count = count;
            }

            public T Current => _items[_index];

            object IEnumerator.Current => _items[_index];

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                // If we're not at the end, increment the index.
                if (_index < _count)
                {
                    ++_index;
                }

                // Return true if and only if we're still not at the end.
                return _index < _count;
            }

            public void Reset()
            {
                _index = -1;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(_items, _count);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(_items, _count);
        }
    }
}
