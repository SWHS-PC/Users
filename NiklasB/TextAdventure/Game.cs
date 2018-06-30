using System;
using System.Collections.Generic;

namespace TextAdventure
{
    class Game : Helpers
    {
        enum Keyword
        {
            None,
            With,
            Article
        }

        struct Arg
        {
            public string Name;
            public object Value;
            public IList<Item> Collection;  // if value is an Item
            public Link Link;               // value is a Door or null
            public Keyword Keyword;
        }

        Room m_currentRoom;
        bool m_isGameOver;
        List<Item> m_inventory = new List<Item>();

        const string m_helpText =
            "help           List commands\n" +
            "q              Quit\n" +
            "n|s|e|w        Go north, south, east, or west\n" +
            "look           Describe the room or specified item\n" +
            "open           Open specified door or item\n" +
            "unlock         Unlock something with an item\n" +
            "take           Pick up the specified item\n" +
            "i              List items in your inventory\n" +
            "";

        public Game(Room startRoom)
        {
            m_currentRoom = startRoom;
        }

        public void Play()
        {
            m_currentRoom.Describe();

            while (!m_isGameOver)
            {
                Console.WriteLine();
                Console.Write("> ");

                ProcessCommand(Console.ReadLine().ToLower().Split());
            }
        }

        void ProcessCommand(string[] tokens)
        {
            if (tokens.Length == 0)
            {
                Console.WriteLine("Please type a command.");
                Console.WriteLine("To quit, type 'q'.");
                Console.WriteLine("For help, type 'help'.");
                return;
            }

            string verb = tokens[0];

            var args = new List<Arg>();

            int i = 1;
            while (i < tokens.Length)
            {
                var arg = new Arg();
                int c = ParseArg(tokens, i, ref arg);
                if (c == 0)
                    return;

                if (arg.Keyword != Keyword.Article)
                {
                    args.Add(arg);
                }

                i += c;
            }

            switch (verb)
            {
                case "q":
                    m_isGameOver = true;
                    break;
                case "n":
                    TryGo(Direction.North);
                    break;
                case "e":
                    TryGo(Direction.East);
                    break;
                case "s":
                    TryGo(Direction.South);
                    break;
                case "w":
                    TryGo(Direction.West);
                    break;
                case "go":
                    ProcessGo(args);
                    break;
                case "look":
                    ProcessLook(args);
                    break;
                case "open":
                    ProcessOpen(args);
                    break;
                case "unlock":
                    ProcessUnlock(args);
                    break;
                case "take":
                    ProcessTake(args);
                    break;
                case "i":
                    ListInventory();
                    break;
                case "help":
                    Console.WriteLine(m_helpText);
                    break;
                default:
                    Console.WriteLine("I don't know that command.");
                    Console.WriteLine("For help, type 'help'.");
                    break;
            }
        }

        int ParseArg(string[] tokens, int index, ref Arg arg)
        {
            // Get the first token.
            string token = tokens[index];

            // Check for keyword.
            arg.Keyword = ParseKeyword(token);
            if (arg.Keyword != Keyword.None)
            {
                arg.Name = token;
                return 1;
            }

            // Check for other special names.
            switch (token)
            {
                case "door": return ParseDoorArg(ref arg);
                case "north": return ProcessDirectionArg(Direction.North, tokens, index, ref arg);
                case "south": return ProcessDirectionArg(Direction.South, tokens, index, ref arg);
                case "east": return ProcessDirectionArg(Direction.East, tokens, index, ref arg);
                case "west": return ProcessDirectionArg(Direction.West, tokens, index, ref arg);
            }

            // It must be an item name. Concatenate tokens until we reach the end
            // or encounter a keyword.
            string name = token;
            int j = index + 1;
            while (j < tokens.Length && ParseKeyword(tokens[j]) == Keyword.None)
            {
                name = $"{name} {tokens[j]}";
                j++;
            }

            // Find the matching item.
            Item item;
            IList<Item> collection;
            if (FindItem(m_inventory, name, out item, out collection) ||
                FindItem(m_currentRoom.Items, name, out item, out collection))
            {
                // We found the item.
                arg.Name = item.Name;
                arg.Value = item;
                arg.Collection = collection;
                return j - index;
            }

            Console.WriteLine($"I don't know what '{name}' is.");
            return 0;
        }

        int ParseDoorArg(ref Arg arg)
        {
            Link match = null;

            foreach (var link in m_currentRoom.Links)
            {
                if (link.Door != null)
                {
                    if (match == null)
                    {
                        match = link;
                    }
                    else
                    {
                        Console.WriteLine("Which door do you want to open?");
                        Console.WriteLine($"For example, type 'open {Str(match.Direction)} door.");
                        return 0;
                    }
                }
            }

            if (match == null)
            {
                Console.WriteLine("There are no doors in this room.");
                return 0;
            }

            arg.Name = "door";
            arg.Value = match.Door;
            arg.Link = match;
            return 1;
        }

        int ProcessDirectionArg(Direction dir, string[] tokens, int index, ref Arg arg)
        {
            foreach (var link in m_currentRoom.Links)
            {
                if (link.Direction == dir)
                {
                    string dirName = tokens[index];

                    if (index + 1 < tokens.Length && tokens[index + 1] == "door")
                    {

                        if (link.Door == null)
                        {
                            Console.WriteLine($"There is no door to the {dirName}.");
                            return 0;
                        }

                        arg.Name = $"{dirName} door";
                        arg.Value = link.Door;
                        arg.Link = link;

                        return 2;
                    }
                    else
                    {
                        arg.Name = dirName;
                        arg.Link = link;

                        return 1;
                    }
                }
            }

            Console.WriteLine($"There is nothing to the {Str(dir)}.");
            return 0;
        }

        Keyword ParseKeyword(string name)
        {
            switch (name)
            {
                case "with": return Keyword.With;
                case "the": return Keyword.Article;
                case "a": return Keyword.Article;
                case "an": return Keyword.Article;
            }
            return Keyword.None;
        }

        static bool FindItem(
            IList<Item> collection, 
            string name, 
            out Item matchingItem, 
            out IList<Item> matchingCollection
            )
        {
            foreach (var item in collection)
            {
                if (item.Name == name)
                {
                    matchingItem = item;
                    matchingCollection = collection;
                    return true;
                }

                var container = item as IContainer;
                if (container != null && container.IsOpen)
                {
                    if (FindItem(container.Items, name, out matchingItem, out matchingCollection))
                        return true;
                }
            }
            matchingItem = null;
            matchingCollection = null;
            return false;
        }

        void ProcessLook(IList<Arg> args)
        {
            if (args.Count == 0)
            {
                m_currentRoom.Describe();
            }
            else if (args.Count == 1)
            {
                var item = args[0].Value as IDescribable;
                if (item != null)
                {
                    item.Describe();
                }
                else
                {
                    Console.WriteLine($"The {args[0].Name} is nothing special.");
                }
            }
            else
            {
                Console.WriteLine("Sorry, I don't understand that.");
            }
        }

        void ListInventory()
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

        void ProcessGo(IList<Arg> args)
        {
            if (args.Count != 1 || args[0].Link == null)
            {
                Console.WriteLine("Please type a direction after 'go'.");
                return;
            }

            TryGo(args[0].Link);
        }

        void ProcessOpen(IList<Arg> args)
        {
            if (args.Count == 0)
            {
                Console.WriteLine("What do you want to open?");
            }
            else if (args.Count > 1)
            {
                Console.WriteLine("Sorry, I didn't understand that.");
            }
            else
            {
                TryOpen(args[0]);
            }
        }

        void TryOpen(Arg arg)
        {
            var openable = arg.Value as IOpenable;

            if (openable == null)
            {
                Console.WriteLine("You can't open that.");
            }
            else if (openable.IsOpen)
            {
                Console.WriteLine($"The {arg.Name} is already open.");
            }
            else if (openable.IsLocked)
            {
                Console.WriteLine($"The {arg.Name} is locked.");
            }
            else
            {
                openable.IsOpen = true;

                Console.WriteLine($"You open the {arg.Name}.");

                var describable = arg.Value as IDescribable;
                if (describable != null)
                {
                    describable.Describe();
                }
            }
        }

        void ProcessUnlock(IList<Arg> args)
        {
            if (args.Count != 3 || args[1].Keyword != Keyword.With)
            {
                Console.WriteLine("Say 'unlock <item> with <item>'.");
                return;
            }

            var itemName = args[0].Name;
            var item = args[0].Value as IOpenable;

            var keyName = args[2].Name;
            var key = args[2].Value as Item;

            if (item == null)
            {
                Console.WriteLine($"You can't unlock the {itemName}.");
            }
            else if (!item.IsLocked)
            {
                Console.WriteLine($"The {itemName} is already unlocked.");
            }
            else if (key == null || key != item.Key)
            {
                Console.WriteLine($"The {keyName} doesn't unlock the {itemName}.");
            }
            else
            {
                item.IsLocked = false;
                Console.WriteLine($"You unlock the {itemName}.");
            }
        }

        void ProcessTake(IList<Arg> args)
        {
            if (args.Count == 0)
            {
                Console.WriteLine("What do you want to take?");
            }
            else if (args.Count > 1)
            {
                Console.WriteLine("Sorry, I didn't understand that.");
            }
            else
            {
                var item = args[0].Value as Item;
                if (item == null)
                {
                    Console.WriteLine("You can't take that.");
                }
                else if (item.IsFixed)
                {
                    Console.WriteLine($"The {args[0].Name} cannot be moved.");
                }
                else if (args[0].Collection == m_inventory)
                {
                    Console.WriteLine($"The {args[0].Name} is already in your inventory.");
                }
                else
                {
                    args[0].Collection.Remove(item);
                    m_inventory.Add(item);
                    Console.WriteLine($"The {args[0].Name} is now in your inventory.");
                }
            }
        }
    
        void TryGo(Direction dir)
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
