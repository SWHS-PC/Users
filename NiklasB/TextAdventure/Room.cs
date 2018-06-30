using System;
using System.Collections.Generic;

namespace TextAdventure
{
    class Room : Helpers, IDescribable
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

        public Description Description { get; set; }

        public void Describe()
        {
            if (Description != null)
            {
                Description.Describe(this);
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine($"This is the {this.Name}.");
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

            if (Items.Count != 0)
            {
                Console.WriteLine();
                Console.WriteLine("The room contains the following items:");
                ListItems(Items);
            }
        }
    }
}
