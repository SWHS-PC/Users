using System;
using System.Collections.Generic;

namespace Generics
{
    class Queue<T>
    {
        T[] _items;
        int _count;
        int _firstIndex;

        // Helper to compute the array index from a logical zero-based index
        // in the queue. The array length is assumed to be a power of two.
        int ArrayIndex(int i) => (_firstIndex + i) & (_items.Length - 1);

        public int Count => _count;
        public T Front => _items[_firstIndex];
        public T this[int i] => _items[ArrayIndex(i)];

        public void Push(T item)
        {
            if (_items == null)
            {
                // First push: allocate the array with a default initial capacity.
                _items = new T[8];
            }
            else if (_items.Length == _count)
            {
                // The array is full: create a new one twice the size.
                var newItems = new T[_items.Length * 2];

                // Copy existing items to the new array.
                for (int i = 0; i < _count; i++)
                {
                    // We're using our indexer so the first item in the 
                    // queue will be stored at the beginning of the new
                    // array.
                    newItems[i] = this[i];
                }

                // Save the new array.
                _items = newItems;

                // We copied the items in logical order, with the first
                // item at the beginning of the array.
                _firstIndex = 0;
            }

            // There should now be room: store the item and increment the count.
            _items[ArrayIndex(_count++)] = item;
        }

        public void Pop()
        {
            if (_count == 0)
                throw new IndexOutOfRangeException();

            // Replace the front item with the default value for the type.
            _items[_firstIndex] = default(T);

            // Advance the first index and decrement the count.
            _firstIndex = ArrayIndex(1);
            _count--;
        }
    }
}
