using System;
using System.Collections;
using System.Collections.Generic;


namespace Generics
{
    class Program
    {
        static void Main(string[] args)
        {
            var numbers = new int[33];
            var rand = new Random();
            Console.WriteLine("Array");
            for (int i = 0; i < 33; i++)
            {
                int n = rand.Next(10, 99);
                numbers[i] = n;
                Console.Write($" {n}");
            }

            var list = new Generics.List<int>();
            Testlist(list, numbers);

            var queue = new Queue<int>();
            TestQueue(numbers, queue);
            Console.Read();
        }
        static void Testlist(Generics.List<int> list, int[] numbers)
        {
            foreach(int n in numbers)
            {
                list.Add(n);
            }
            Console.WriteLine();
            Console.WriteLine("For from list");
            for (int i = 0; i< list.Count; i++)
            {
                Console.Write($" {list[i]}");
            }

            Console.WriteLine();
            Console.WriteLine("Foreach from list");
            foreach (var n in list)
            {
                Console.Write($" {n}");
            }
            
        }
        static void TestQueue(int[] numbers, Queue<int> queue)
        {
            foreach(int n in numbers)
            {
                queue.Push(n);
            }

            Console.WriteLine();
            Console.WriteLine("Queue");
            while (queue.Count != 0)
            {
                Console.Write($" {queue.Front}");
                queue.Pop();
            }
            Console.WriteLine();
        }
    }
}
