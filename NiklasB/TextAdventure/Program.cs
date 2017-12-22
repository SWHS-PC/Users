using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace TextAdventure
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create the Kitchen and add an item to it.
            var kitchen = new Room("Kitchen");
            kitchen.Items.Add(new Item("Spatula"));

            // Create the hallway.
            var hallway = new Room("Hallway");

            // Link up hallway and kitchen
            hallway.North = kitchen;
            kitchen.South = hallway;

            // Describe the kitchen.
            kitchen.Describe();

            Console.ReadKey();
        }
    }
}
