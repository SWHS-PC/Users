using System;
using System.Collections.Generic;

namespace TextAdventure
{
    class Game : Helpers
    {
        public Game(Room startRoom)
        {
            m_currentRoom = startRoom;
        }

        Room m_currentRoom;
        bool m_isGameOver;

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

        void ProcessCommand(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please type a command.");
                Console.WriteLine("To quit, type 'q'.");
                Console.WriteLine("For help, type 'help'.");
                return;
            }

            switch (args[0])
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
                case "open":
                    ProcessOpen(args);
                    break;
                default:
                    Console.WriteLine("I don't know that command.");
                    Console.WriteLine("For help, type 'help'.");
                    break;
            }
        }

        void ProcessGo(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("Please type a direction after 'go'.");
                return;
            }

            var dir = ParseDirection(args[1]);
            if (dir == Direction.None)
            {
                Console.WriteLine($"I don't recognize {args[1]} as a direction.");
                return;
            }


            TryGo(dir);
        }

        void ProcessOpen(string[] args)
        {
            if (args.Length == 2 && args[1] == "door")
            {
                Link selectedLink = null;
                foreach (var link in m_currentRoom.Links)
                {
                    if (link.Door != null)
                    {
                        if (selectedLink == null)
                        {
                            selectedLink = link;
                        }
                        else
                        {
                            Console.WriteLine("Which door do you want to open?");
                            Console.WriteLine($"For example, type 'open {Str(selectedLink.Direction)} door.");
                            return;
                        }
                    }
                }

                if (selectedLink == null)
                {
                    Console.WriteLine("There are no doors in this room.");
                    return;
                }

                TryOpenDoor(selectedLink);
            }
            else if (args.Length == 3 && args[2] == "door")
            {
                Direction dir = ParseDirection(args[1]);
                if (dir == Direction.None)
                {
                    Console.WriteLine($"I don't recognize {args[1]} as a direction.");
                    return;
                }

                foreach (var link in m_currentRoom.Links)
                {
                    if (link.Direction == dir)
                    {
                        TryOpenDoor(link);
                        return;
                    }
                }
            }
            else
            {
                Console.WriteLine("I don't understand what you're trying to open.");
            }
        }

        void TryOpenDoor(Link link)
        {
            if (link.Door == null)
            {
                Console.WriteLine($"There is no door to the {Str(link.Direction)}.");
            }
            else if (link.Door.IsOpen)
            {
                Console.WriteLine($"The {Str(link.Direction)} door is already open.");
            }
            else if (link.Door.IsLocked)
            {
                Console.WriteLine($"The {Str(link.Direction)} door is locked.");
            }
            else
            {
                link.Door.IsOpen = true;
                Console.WriteLine($"The {Str(link.Direction)} door is now open.");
            }
        }

        void TryGo(Direction dir)
        {
            foreach (var link in m_currentRoom.Links)
            {
                if (link.Direction == dir)
                {
                    if (link.Door == null || link.Door.IsOpen)
                    {
                        m_currentRoom = link.To;
                        m_currentRoom.Describe();
                        return;
                    }
                    else
                    {
                        Console.WriteLine("You can't walk through closed doors.");
                    }
                    return;
                }
            }

            Console.WriteLine("You cannot go that way.");
        }
    }
}
