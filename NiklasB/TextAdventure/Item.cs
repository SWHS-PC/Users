using System;
using System.Collections.Generic;

namespace TextAdventure
{
    interface IDescribable
    {
        void Describe();
        Description Description { get; set; }
    }

    abstract class Item : IDescribable
    {
        public string Name { get; set; }

        public bool IsFixed { get; set; }

        public Description Description { get; set; }

        public abstract void Describe();
    }

    class SimpleItem : Item
    {
        public override void Describe()
        {
            if (Description != null)
            {
                Description.Describe(this);
            }
            else
            {
                Console.WriteLine($"The {Name} is nothing special.");
            }
        }
    }

    interface IContainer : IOpenable
    {
        IList<Item> Items { get; }
    }

    class ContainerItem : Item, IContainer
    {
        List<Item> m_items = new List<Item>();

        public override void Describe()
        {
            if (Description != null)
            {
                Description.Describe(this);
            }

            if (IsOpen)
            {
                if (Items.Count != 0)
                {
                    Console.WriteLine($"The {Name} is open and contains the following items:");
                    Helpers.ListItems(Items);
                }
                else
                {
                    Console.WriteLine($"The {Name} is empty.");
                }
            }
            else
            {
                Console.WriteLine($"The {Name} is closed.");
            }
        }

        public bool IsOpen { get; set; }
        public bool IsLocked { get; set; }
        public Item Key { get; set; }
        public IList<Item> Items => m_items;
    }
}
