using System;
using System.Collections.Generic;

namespace Generics
{
    class LinkedQueue<T> : IQueue<T>
    {
        class Node
        {
            public Node _next;
            public T _value;
        }

        Node _first;
        Node _last;
        int _count;

        public LinkedQueue()
        {
        }

        public int Count => _count;

        public T Front => _first._value;

        public void Push(T item)
        {
            Node node = new Node { _value = item };

            if (_first == null)
            {
                _first = node;
                _last = node;
            }
            else
            {
                _last._next = node;
                _last = node;
            }

            _count++;
        }

        public void Pop()
        {
            if (_count == 0)
                throw new InvalidOperationException();

            _first = _first._next;

            if (_first == null)
            {
                _last = null;
            }

            _count--;
        }
    }
}
