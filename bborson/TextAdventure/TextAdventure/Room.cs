using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventure
{
    class Room
    {
        List<Item> m_items = new List<Item>();
        public List<Item> Items => m_items;

        public string Name { get; }
        public Room(string name)
        {
            Name = name;
        }
        public MapLink North { get; set; }
        public MapLink South { get; set; }
        public MapLink East { get; set; }
        public MapLink West { get; set; }

        public string Description { get; set; }

        public void Describe()
        {
            Console.WriteLine("You are at {0}.", Name);
            Console.WriteLine();

            if (Description != null)
            {
                Program.WriteParagraph(Description);
                Console.WriteLine();
            }

            if (Items.Count != 0)
            {
                foreach (var item in Items)
                {
                    Console.WriteLine($"You can see a {item.Name} here.");
                }
                Console.WriteLine();
            }

            DescribeLink(North, "North");
            DescribeLink(South, "South");
            DescribeLink(East, "East");
            DescribeLink(West, "West");

        }
        public void DescribeLink(MapLink link, string direction)
        {
            if (link == null)
                return;
            if (link.Description != null)
            {
                Program.WriteParagraph(link.Description);
            }
            else if (link.Door != null)
            {
                if (!link.Door.IsLocked)
                {
                    Console.WriteLine($"To the {direction}, an open {link.Door.Name} leads to {link.To.Name}.");
                }
                else
                {
                    Console.WriteLine($"To the {direction}, a firmly shut {link.Door.Name} blocks your path.");
                }

            }
            else
            {
                Console.WriteLine($"To the {direction} is {link.To.Name}.");
            }
        }
    }
}
