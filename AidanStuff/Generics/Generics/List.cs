using System;
using System.Collections;
using System.Collections.Generic;


namespace Generics
{
    class List<T> : IEnumerable<T>
    {
        T[] items;
        int count;

        public int Count => count;

        public T this[int i]
        {
            get { return items[i]; }
            set { items[i] = value; }
        }
        

        public void Add(T item)
        {
            EnsureCapacity(count + 1);
            items[count++] = item;
        }

        void EnsureCapacity(int minCapacity)
        {
            if(items == null)
            {
                int c = Math.Max(8, minCapacity);
                items = new T[c];
            }
            else if(items.Length < minCapacity)
            {
                int c = Math.Max(items.Length * 2, minCapacity);
                var newItems = new T[c];
                Array.Copy(items, newItems, count);
                items = newItems;
            }
            else
            {

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
                if(_index < _count)
                {
                    ++_index;
                }
                return _index < _count;
            }

            public void Reset()
            {
                _index = -1;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new Enumerator(items, count);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return new Enumerator(items, count);
        }
    }
}
