using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventure
{
    class Room
    {
        public Room(string name)
        {
            m_name = name;
        }

        // Private field that holds the value of the public Name property.
        string m_name;

        // Verbose syntax for a read-only property.
        public string Name
        {
            get { return m_name; }
        }

        List<Item> m_items = new List<Item>();
        public List<Item> Items => m_items;

        public Room North { get; set; }
        public Room South { get; set; }
        public Room East  { get; set; }
        public Room West  { get; set; }

        public void Describe()
        {
            Console.WriteLine($"This is the {this.Name}.");

            if (Items.Count != 0)
            {
                Console.WriteLine("In this room are the following items:");
                foreach (var item in this.Items)
                {
                    Console.WriteLine(item.Name);
                }
            }

            if (North != null)
            {
                Console.WriteLine($"To the North is the {North.Name}.");
            }
            if (South != null)
            {
                Console.WriteLine($"To the South is the {South.Name}.");
            }
            if (East != null)
            {
                Console.WriteLine($"To the East is the {East.Name}.");
            }
            if (West != null)
            {
                Console.WriteLine($"To the West is the {West.Name}.");
            }
        }
    }
}
