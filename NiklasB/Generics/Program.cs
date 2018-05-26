using System;
using System.Collections.Generic;

namespace Generics
{
    class Program
    {
        static void Main(string[] args)
        {
            // Generate some random numbers for testing purposes.
            Console.WriteLine("Input data:");
            var rand = new Random();
            var numbers = new int[33];
            for (int i = 0; i < numbers.Length; i++)
            {
                int n = rand.Next(10, 99);
                numbers[i] = n;
                Console.Write($" {n}");
            }
            Console.WriteLine();

            // Test our list implementation.
            Console.WriteLine("\nMyList:");
            var list = new MyList<int>();
            TestList(list, numbers);

            // Test our queue implementation.
            Console.WriteLine("\nQueue:");
            var queue = new Queue<int>();
            TestQueue(numbers, queue);

            // Test our priority queue implementation.
            Console.WriteLine("\nPriorityQueue:");
            var priorityQueue = new PriorityQueue<int>((int a, int b) => a < b);
            TestQueue(numbers, priorityQueue);

            Console.WriteLine("\nPress ENTER to exit.");
            Console.ReadLine();
        }

        static void TestList(MyList<int> list, int[] numbers)
        {
            // Add the numbers to the list.
            foreach (int n in numbers)
            {
                list.Add(n);
            }

            // Iterate over the list explicitly using the indexer.
            for (int i = 0; i < list.Count; i++)
            {
                Console.Write($" {list[i]}");
            }
            Console.WriteLine();

            // Iterate over the list using foreach, which requires an enumerator.
            foreach (var n in list)
            {
                Console.Write($" {n}");
            }
            Console.WriteLine();
        }

        static void TestQueue(int[] numbers, IQueue<int> queue)
        {
            foreach (int n in numbers)
            {
                queue.Push(n);
            }

            while (queue.Count != 0)
            {
                Console.Write($" {queue.Front}");
                queue.Pop();
            }

            Console.WriteLine();
        }
    }
}
