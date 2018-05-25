using System;
using System.Collections.Generic;

namespace Generics
{
    // A queue is first-in-first-out (FIFO), so items are added at the end
    // (by Push) and removed from the beginning (by Pop).
    class Queue<T>
    {
        // We want Pop to be a fast, so when removing the first item from the
        // queue we don't want to have to copy all the other items to fill the
        // gap. That would make Pop a linear-time operation whereas we want it
        // to be constant time -- i.e., O(N) vs. O(1). Therefore, we store the
        // items in a circular array:
        //
        //      +---+---+---+---+---+---+---+---+
        //      | 2 | 3 | 4 | . | . | . | 0 | 1 |
        //      +---+---+---+---+---+---+---+---+
        //                                 \
        //                                  _firstIndex
        //
        // The _firstIndex field specifies the array index of the first item
        // in the logical sequence. Items are stored at array positions starting
        // with this index and wrapping around to the beginning as shown above.
        // To remove an item from the beginning, we simply increment _firstIndex
        // and decrement _count.
        T[] _items;
        int _count;
        int _firstIndex;

        // Helper to compute the array index from a logical zero-based index
        // in the queue. If the sum (_firstIndex + i) is past the end of the
        // array then the index needs to "wrap around" to the beginning. In
        // effect, we want to take the sum MODULO the array length. However,
        // since we ensure that the array length is always a power of two, this
        // is much more efficiently achieved using a bitwise AND as shown below.
        int ArrayIndex(int i) => (_firstIndex + i) & (_items.Length - 1);

        // Number of items in the queue.
        public int Count => _count;

        // First item in the queue.
        public T Front => _items[_firstIndex];

        // Indexer to get any item in the sequence.
        public T this[int i] => _items[ArrayIndex(i)];

        // Push adds an item to the end of the queue.
        // This executes in amortized constant time.
        public void Push(T item)
        {
            if (_items == null)
            {
                // First push: allocate the array with a default initial capacity.
                // The initial capacity must be a power of two, but is otherwise
                // somewhat arbitrary.
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

        // Pop removes an item from the beginning of the queue.
        // This executes in constant time.
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
