using System;
using System.Collections.Generic;

namespace TextAdventure
{
    class GameController
    {
        Game m_game;
        CommandParser m_commandParser;
        List<string> m_commandArgs = new List<string>();

        public GameController(Game game)
        {
            m_game = game;
            m_commandParser = new CommandParser(m_formatStrings);
        }

        public void Run()
        {
            m_game.CurrentRoom.Describe();

            while (!m_game.IsGameOver)
            {
                Console.WriteLine();
                Console.Write("> ");

                ProcessCommand(Console.ReadLine().ToLower());
            }
        }

        public void ProcessCommand(string input)
        {
            int commandIndex;
            if (m_commandParser.ParseCommand(input, m_commandArgs, out commandIndex))
            {
                m_commands[commandIndex](m_game, m_commandArgs);
            }
            else if (commandIndex >= 0)
            {
                Console.WriteLine($"That command has the form:\n{m_formatStrings[commandIndex]}");
            }
            else
            {
                Console.WriteLine("I don't know that command. Type 'h' or 'help' for help.");
            }
        }

        static bool TryParseDoorName(string input, out Direction dir, out DoorFlags flags)
        {
            dir = Direction.None;
            flags = DoorFlags.None;

            var token = new StringToken(input);

            // Process any modifying tokens before the word "door".
            while (token.HaveNext)
            {
                switch (token[0])
                {
                    case 'n':
                        if (token == "north")
                        {
                            dir = Direction.North;
                            break;
                        }
                        return false;

                    case 's':
                        if (token == "south")
                        {
                            dir = Direction.South;
                            break;
                        }
                        return false;

                    case 'e':
                        if (token == "east")
                        {
                            dir = Direction.East;
                            break;
                        }
                        return false;

                    case 'w':
                        if (token == "west")
                        {
                            dir = Direction.West;
                            break;
                        }
                        return false;

                    case 'l':
                        if (token == "locked")
                        {
                            flags = flags | DoorFlags.Locked;
                            break;
                        }
                        return false;

                    case 'u':
                        if (token == "unlocked")
                        {
                            flags = flags | DoorFlags.Locked;
                            break;
                        }
                        return false;

                    case 'o':
                        if (token == "open")
                        {
                            flags = flags | DoorFlags.Open;
                            break;
                        }
                        return false;

                    case 'c':
                        if (token == "closed")
                        {
                            flags = flags | DoorFlags.Closed;
                            break;
                        }
                        return false;

                    default:
                        return false;
                }

                token.Next();
            }

            // The last token must be the door keyword.
            return token == "door";
        }

        delegate void CommandDelegate(Game game, IList<string> args);

        static readonly string[] m_formatStrings = new string[]
        {
            "h[elp]",
            "q[uit]",
            "n[orth]",
            "s[outh]",
            "e[ast]",
            "w[est]",
            "take <item>",
            "drop <item>",
            "i[nventory]",
            "look",
            "examine <item>",
            "open <item>",
            "close <item>",
            "lock <item> with <item>",
            "unlock <item> with <item>",
        };

        static readonly CommandDelegate[] m_commands = new CommandDelegate[]
        {
            ShowHelp,
            (Game game, IList<string> args) => game.IsGameOver = true,
            (Game game, IList<string> args) => game.TryGo(Direction.North),
            (Game game, IList<string> args) => game.TryGo(Direction.South),
            (Game game, IList<string> args) => game.TryGo(Direction.East),
            (Game game, IList<string> args) => game.TryGo(Direction.West),
            (Game game, IList<string> args) => game.TakeItem(args[0]),
            (Game game, IList<string> args) => game.DropItem(args[0]),
            (Game game, IList<string> args) => game.ListInventory(),
            (Game game, IList<string> args) => game.CurrentRoom.Describe(),
            (Game game, IList<string> args) => game.ExamineItem(args[0]),
            Open,
            Close,
            (Game game, IList<string> args) => TryKey(game, args[0], args[1], /*isLocking*/ true),
            (Game game, IList<string> args) => TryKey(game, args[0], args[1], /*isLocking*/ false),
        };

        static void ShowHelp(Game game, IList<string> args)
        {
            foreach (var formatString in m_formatStrings)
            {
                Console.WriteLine(formatString);
            }
        }

        static void Open(Game game, IList<string> args)
        {
            string name = args[0];
            var openable = GetOpenable(game, ref name, "open");

            if (openable != null)
            {
                game.TryOpen(openable, name);
            }
        }

        static void Close(Game game, IList<string> args)
        {
            string name = args[0];
            var openable = GetOpenable(game, ref name, "close");

            if (openable != null)
            {
                game.TryClose(openable, name);
            }
        }

        static void TryKey(Game game, string openableName, string keyName, bool isLocking)
        {
            var openable = GetOpenable(game, ref openableName, "lock");

            if (openable != null)
            {
                var key = game.FindItem(ItemSource.Inventory, keyName);
                if (key != null)
                {
                    if (isLocking)
                    {
                        game.TryLock(openable, openableName, key);
                    }
                    else
                    {
                        game.TryUnlock(openable, openableName, key);
                    }
                }
            }
        }

        static IOpenable GetOpenable(Game game, ref string name, string verb)
        {
            Direction dir;
            DoorFlags flags;
            if (TryParseDoorName(name, out dir, out flags))
            {
                return game.FindDoor(dir, flags, name);
            }
            else
            {
                Item item = game.FindItem(ItemSource.Inventory | ItemSource.Room, name);
                if (item != null)
                {
                    name = item.Name;

                    var openable = item as IOpenable;
                    if (openable == null)
                    {
                        Console.WriteLine($"You can't {verb} the {name}, silly!");
                    }

                    return openable;
                }
            }

            return null;
        }
    }
}
