using System;
using System.Collections.Generic;

namespace Generics
{
    class PriorityQueue<T> : IQueue<T>
    {
        // Delegate that returns true if a < b. The priority queue ensures
        // that no other item is less than the front item.
        public delegate bool LessFunc(T a, T b);

        public PriorityQueue(LessFunc less)
        {
            Less = less;
        }

        LessFunc Less { get; }

        public int Count => _items.Count;

        public T Front => _items[0];

        public void Push(T item)
        {
            // Add the new item at the end.
            _items.Add(item);

            // The last item is now out of order, so sift up.
            SiftUp();
        }

        public void Pop()
        {
            if (_items.Count > 1)
            {
                int lastIndex = _items.Count - 1;

                // Replace the first item with the last item and then remove the last item.
                _items[0] = _items[lastIndex];
                _items.RemoveAt(lastIndex);

                // The first item is now out of order, so sift down.
                SiftDown();
            }
            else
            {
                _items.Clear();
            }
        }

        // PriorityQueue uses a heap data structure. Logically, we think of the
        // heap as a binary tree with two constraints.
        //
        // The first constraint is the order property. Each parent node is less
        // than either of its child nodes (though there is no constraint on which
        // child is greater).
        //
        // The second constraint is the shape property. Every node has two children
        // with no gaps, except at the end of the last row as shown below. This
        // makes it possible to represent the (logical) tree efficiently as an array.
        // Instead of using a linked data structure, we can compute the indices of
        // related nodes using the helper methods below.
        //
        //               0
        //        1             2
        //     3     4       5     6
        //   7  8   9 10   11 *   *  *
        //
        static int Parent(int childIndex) => (childIndex - 1) / 2;
        static int LeftChild(int parentIndex) => (parentIndex * 2) + 1;
        static int RightFromLeft(int leftIndex) => leftIndex + 1;

        // SiftUp restores the order constraint in the case where the
        // last node is out of order.
        void SiftUp()
        {
            // Get the index and value of the last (out-of-order) node.
            int index = _items.Count - 1;
            var item = _items[index];

            // Iterate until we reach the root or the order property
            // is restored.
            while (index > 0)
            {
                int parentIndex = Parent(index);
                var parentItem = _items[parentIndex];

                if (Less(item, parentItem))
                {
                    // Swap the node with its parent and continue.
                    _items[parentIndex] = item;
                    _items[index] = parentItem;

                    index = parentIndex;
                }
                else
                {
                    // The item is not less than its parent, so the
                    // order property has been restored.
                    break;
                }
            }
        }

        // SiftDown restores the order constraint in the case where the
        // first node is out of order.
        void SiftDown()
        {
            // Get the index and value of the first (out-of-order) node.
            int index = 0;
            var item = _items[0];

            // Iterate until we reach a node with no children or the order
            // property is restored.
            int count = _items.Count;
            int childIndex = LeftChild(index);

            while (childIndex < count)
            {
                // Let childIndex and childItem be the index and value of the
                // lesser child; assume initially that it's the left child.
                var childItem = _items[childIndex];

                // If there is a right child and it is less than the left child
                // then let childIndex and childItem be the right child.
                int rightIndex = RightFromLeft(childIndex);
                if (rightIndex < count)
                {
                    var right = _items[rightIndex];
                    if (Less(right, childItem))
                    {
                        childIndex = rightIndex;
                        childItem = right;
                    }
                }

                if (Less(childItem, item))
                {
                    // The lesser child is less than the parent, so swap the
                    // parent and child.
                    _items[index] = childItem;
                    _items[childIndex] = item;

                    // Set up for the next iteration.
                    index = childIndex;
                    childIndex = LeftChild(index);
                }
                else
                {
                    // The child is not less than the parent, so order has
                    // been restored.
                    break;
                }
            }
        }

        List<T> _items = new List<T>();
    }
}
