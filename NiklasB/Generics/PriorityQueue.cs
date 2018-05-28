using System;
using System.Collections.Generic;

namespace Generics
{
    // A priority queue is a collection of items in which the first item is
    // always less than or equal to all the other items in the collection.
    // Priority queues have several useful applications (the Wikipedia page
    // on priority queues lists several), but I've included it here mostly
    // because it's built on a rather clever algorithm.
    class PriorityQueue<T> : IQueue<T>
    {
        // Delegate that returns true if a < b. The priority queue ensures
        // that no other item is less than the front item. Using a delegate
        // enables the PriorityQueue to be used with any time and also makes
        // it more flexible because the user of the class can decide with
        // "less" means in a given context.
        public delegate bool LessFunc(T a, T b);

        // The constructor saves the delegate for future use. Following is
        // an example of how you could use this constructor to create a
        // PriorityQueue of int, using a lambda expression as the delegate:
        //
        //      var q = new PriorityQueue<int>((int a, int b) => a < b);
        //
        public PriorityQueue(LessFunc less)
        {
            Less = less;
        }

        // Private, read-only property initialized by the constructor.
        LessFunc Less { get; }

        // Internally use a list to hold the items.
        List<T> _items = new List<T>();

        // The number of items in the priority queue is simply the number in the list.
        public int Count => _items.Count;

        // The first item in the priority queue is the first item in the list.
        public T Front => _items[0];

        // The requirement is that the first item in the priority queue is less than
        // or equal to all the other items. There are various naive ways one might do
        // this. For example, one could try to keep the list sorted at all times by
        // inserting each item in sorted order. However, that would make insertion an
        // O(N) operation. The usual implementation of a priority queue uses a clever
        // data structure called a heap to make both insertion and removal take
        // O(log N) time.
        //
        // A heap is a kind of binary tree. A binary tree is a collection of objects
        // that are linked together in a tree-like arrangement such that each parent
        // node has a most two child nodes, as shown below:
        //
        //             0
        //           /   \
        //         /       \
        //        1         2
        //       / \       / \
        //      3   4     5   6
        //     /\   /\   /
        //    7  8 9 10 11
        //
        // Binary trees are widely used in computer science, but most binary trees
        // are not heaps. To be a heap, a binary tree must meet two additional
        // constraints:
        //
        //      1.  The order property: each parent node is less than or equal
        //          to its child nodes. It follows that the root node is less
        //          than or equal to all the other nodes.
        //
        //      2.  The shape property: all of the nodes have two children
        //          except for those in the last row and the end of the previous
        //          row, as shown above.
        //
        // Because of the shape property, each row of the tree has twice as may nodes
        // as the row above. Therefore, if we number the nodes as shown above, it
        // the number of a parent node is half the number of either of its children 
        // (rounding up).
        //
        // This observation leads us to a very simple and compact representation for
        // a heap. Instead of using a linked data structure as we would for most trees,
        // we can simply store the items in an array and compute the parent/child
        // relationships from the array indices using the following helper methods:
        static int Parent(int childIndex) => (childIndex - 1) / 2;
        static int LeftChild(int parentIndex) => (parentIndex * 2) + 1;
        static int RightFromLeft(int leftIndex) => leftIndex + 1;

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
    }
}
