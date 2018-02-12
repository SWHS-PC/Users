using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tad
{
    class Program
    {
        static void Main(string[] args)
        {
            var kitchen = new room("Kitchen");
            kitchen.items.Add(new item("Spatula"));
            var hallway = new room("Hallway");

            hallway.North = kitchen;
            hallway.South = hallway;

            kitchen.Describe();

            Console.ReadKey();

        }
    }
}
