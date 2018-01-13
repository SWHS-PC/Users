using System;
using System.Collections.Generic;

namespace TextAdventure
{
    class Room : Helpers
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

        List<Link> m_connections = new List<Link>();
        public List<Link> Links => m_connections;

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

            foreach (var link in Links)
            {
                if (link.Door == null)
                {
                    Console.WriteLine($"To the {Str(link.Direction)} is the {link.To.Name}.");
                }
                else if (link.Door.IsOpen)
                {
                    Console.WriteLine($"To the {Str(link.Direction)} is an open door leading to the {link.To.Name}.");
                }
                else
                {
                    Console.WriteLine($"To the {Str(link.Direction)} is a closed door.");
                }
            }
        }
    }
}
