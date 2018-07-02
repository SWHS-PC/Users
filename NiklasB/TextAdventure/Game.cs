using System;
using System.Collections.Generic;

namespace TextAdventure
{
    [Flags]
    enum ItemSource
    {
        Inventory   = 0x1,
        Room        = 0x2
    }

    [Flags]
    enum DoorFlags
    {
        None        = 0,
        Locked      = 0x1,
        Unlocked    = 0x2,
        Open        = 0x4,
        Closed      = 0x8
    }

    class Game : Helpers
    {
        Room m_currentRoom;
        List<Item> m_inventory = new List<Item>();

        public Game(Room startRoom)
        {
            m_currentRoom = startRoom;
        }

        public bool IsGameOver { get; set; }
        public Room CurrentRoom => m_currentRoom;
        public IList<Item> Inventory => m_inventory;

        static bool IsFuzzyMatch(string name, string itemName)
        {
            var nameToken = new StringToken(name);
            var itemToken = new StringToken(itemName);

            // Process all the name tokens except the last one.
            while (nameToken.HaveNext)
            {
                // Scan ahead for the matching item token.
                // Fail if we reach the last item token.
                while (itemToken != nameToken)
                {
                    itemToken.Next();
                    if (!itemToken.HaveNext)
                        return false;
                }

                // The name and item tokens match; advance both.
                nameToken.Next();
                itemToken.Next();
            }

            // Advance to the last item token.
            while (itemToken.HaveNext)
            {
                itemToken.Next();
            }

            // Make sure the last tokens match.
            return nameToken == itemToken;
        }

        public Item FindItem(ItemSource source, string name)
        {
            IList<Item> matchingList;
            return FindItem(source, name, out matchingList);
        }

        public Item FindItem(ItemSource source, string name, out IList<Item> matchingList)
        {
            matchingList = null;

            Item firstMatch = null;
            IList<Item> firstMatchList = null;

            var listsToSearch = new List<IList<Item>>();
            if (source.HasFlag(ItemSource.Inventory))
            {
                listsToSearch.Add(m_inventory);
            }
            if (source.HasFlag(ItemSource.Room))
            {
                listsToSearch.Add(m_currentRoom.Items);
            }

            for (int listIndex = 0; listIndex < listsToSearch.Count; listIndex++)
            {
                var itemList = listsToSearch[listIndex];

                foreach (var item in itemList)
                {
                    if (item.Name == name)
                    {
                        matchingList = itemList;
                        return item;
                    }

                    if (IsFuzzyMatch(name, item.Name))
                    {
                        if (firstMatch != null)
                        {
                            if (source == ItemSource.Inventory)
                            {
                                Console.WriteLine($"You have more than one {name} in your inventory.");
                            }
                            else
                            {
                                Console.WriteLine($"There is more than one {name}.");
                            }
                            return null;
                        }

                        firstMatch = item;
                        firstMatchList = itemList;
                    }

                    var container = item as IContainer;
                    if (container != null && container.IsOpen)
                    {
                        listsToSearch.Add(container.Items);
                    }
                }
            }

            if (firstMatch == null)
            {
                if (source == ItemSource.Inventory)
                {
                    Console.WriteLine($"There is no {name} in your inventory.");
                }
                else
                {
                    Console.WriteLine($"There is no {name}.");
                }
            }

            matchingList = firstMatchList;
            return firstMatch;
        }

        public void ExamineItem(string name)
        {
            var item = FindItem(ItemSource.Inventory | ItemSource.Room, name);
            if (item != null)
            {
                var describable = item as IDescribable;
                if (describable != null)
                {
                    describable.Describe();
                }
                else
                {
                    Console.WriteLine($"The {item.Name} is nothing special.");
                }
            }
        }

        public void ListInventory()
        {
            if (m_inventory.Count == 0)
            {
                Console.WriteLine("There is nothing in your inventory.");
            }
            else
            {
                Console.WriteLine("Items in your inventory:");
                ListItemsRecursively(m_inventory);
            }
        }

        static void ListItemsRecursively(IList<Item> items)
        {
            ListItems(items);

            foreach (var item in items)
            {
                var container = item as IContainer;
                if (container != null)
                {
                    Console.WriteLine();

                    if (!container.IsOpen)
                    {
                        Console.WriteLine($"The {item.Name} is closed.");
                    }
                    else if (container.Items.Count == 0)
                    {
                        Console.WriteLine($"The {item.Name} is empty.");
                    }
                    else
                    {
                        Console.WriteLine($"The {item.Name} contains the following items:");
                        ListItemsRecursively(container.Items);
                    }
                }
            }
        }

        public void TryOpen(IOpenable openable, string name)
        {
            if (openable.IsOpen)
            {
                Console.WriteLine($"The {name} is already open.");
            }
            else if (openable.IsLocked)
            {
                Console.WriteLine($"The {name} is locked.");
            }
            else
            {
                openable.IsOpen = true;

                Console.WriteLine($"You open the {name}.");

                var describable = openable as IDescribable;
                if (describable != null)
                {
                    describable.Describe();
                }
            }
        }

        public void TryClose(IOpenable openable, string name)
        {
            if (openable.IsOpen)
            {
                openable.IsOpen = false;
                Console.WriteLine($"You close the {name}.");
            }
            else
            {
                Console.WriteLine($"The {name} is already closed.");
            }
        }

        public Door FindDoor(Direction dir, DoorFlags flags, string arg)
        {
            Door match = null;

            foreach (var link in m_currentRoom.Links)
            {
                if (dir != Direction.None && dir != link.Direction)
                    continue;

                var door = link.Door;
                if (door  == null)
                    continue;

                if (flags.HasFlag(DoorFlags.Locked) && !door.IsLocked)
                    continue;

                if (flags.HasFlag(DoorFlags.Unlocked) && door.IsLocked)
                    continue;

                if (flags.HasFlag(DoorFlags.Open) && !door.IsOpen)
                    continue;

                if (flags.HasFlag(DoorFlags.Closed) && door.IsOpen)
                    continue;

                if (match != null)
                {
                    Console.WriteLine($"There is more than one {arg}.");
                }

                match = door;
            }

            return match;
        }

        public void TryUnlock(IOpenable openable, string openableName, Item key)
        {
            if (!openable.IsLocked)
            {
                Console.WriteLine($"The {openableName} is already unlocked.");
            }
            else if (key != openable.Key)
            {
                Console.WriteLine($"The {key.Name} doesn't unlock the {openableName}.");
            }
            else
            {
                openable.IsLocked = false;
                Console.WriteLine($"You unlock the {openableName} with the {key.Name}.");
            }
        }

        public void TryLock(IOpenable openable, string openableName, Item key)
        {
            if (openable.IsLocked)
            {
                Console.WriteLine($"The {openableName} is already locked.");
            }
            else if (openable.Key == null)
            {
                Console.WriteLine($"The {openableName} can't be locked.");
            }
            else if (key != openable.Key)
            {
                Console.WriteLine($"The {key.Name} doesn't lock the {openableName}.");
            }
            else
            {
                openable.IsLocked = true;
                Console.WriteLine($"You lock the {openableName} with the {key.Name}.");
            }
        }

        public void TakeItem(string name)
        {
            IList<Item> itemList;
            Item item = FindItem(ItemSource.Room, name, out itemList);
            if (item != null)
            {
                if (item.IsFixed)
                {
                    Console.WriteLine($"The {item.Name} cannot be moved.");
                }
                else
                {
                    itemList.Remove(item);
                    Inventory.Add(item);
                    Console.WriteLine($"The {item.Name} is now in your inventory.");
                }
            }
        }

        public void DropItem(string name)
        {
            IList<Item> itemList;
            Item item = FindItem(ItemSource.Inventory, name, out itemList);
            if (item != null)
            {
                Inventory.Remove(item);
                CurrentRoom.Items.Add(item);
                Console.WriteLine($"You drop the {item.Name}.");
            }
        }

        public void TryGo(Direction dir)
        {
            foreach (var link in m_currentRoom.Links)
            {
                if (link.Direction == dir)
                {
                    TryGo(link);
                    return;
                }
            }

            Console.WriteLine("You cannot go that way.");
        }

        void TryGo(Link link)
        {
            if (link.Door == null || link.Door.IsOpen)
            {
                m_currentRoom = link.To;
                m_currentRoom.Describe();
            }
            else
            {
                Console.WriteLine("You can't walk through closed doors.");
            }
        }
    }
}
