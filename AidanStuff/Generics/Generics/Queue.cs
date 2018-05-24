using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generics
{
    class Queue<T>
    {
        T[] items;
        int count;
        int firstIndex;

        int ArrayIndex(int i) => (firstIndex + i) & (items.Length - 1);

        public int Count => count;
        public T Front => items[firstIndex];
        public T this[int i] => items[ArrayIndex(i)];

        public void Push(T item)
        {
            if(items == null)
            {
                items = new T[8];
            }
            else if(items.Length == count)
            {
                var newItems = new T[items.Length * 2];

                for(int i = 0; i < count; i++)
                {
                    newItems[i] = this[i];
                }

                items = newItems;
                firstIndex = 0;
            }

            items[ArrayIndex(count++)] = item;
        }
        public void Pop()
        {
            if(count == 0)
            {
                throw new IndexOutOfRangeException();
            }

            items[firstIndex] = default(T);

            firstIndex = ArrayIndex(1);
            count--;
        }
    }
}
