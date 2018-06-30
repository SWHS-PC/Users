using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventure
{
    class Helpers
    {
        public static void LinkRooms(Room from, Room to, Direction dir, Door door)
        {
            from.Links.Add(new Link
            {
                From = from,
                To = to,
                Direction = dir,
                Door = door
            });

            to.Links.Add(new Link
            {
                From = to,
                To = from,
                Direction = Helpers.OppositeDirection(dir),
                Door = door
            });
        }

        public static Direction ParseDirection(string s)
        {
            switch (s.ToLowerInvariant())
            {
                case "n":
                case "north":
                    return Direction.North;
                case "e":
                case "east":
                    return Direction.East;
                case "s":
                case "south":
                    return Direction.South;
                case "w":
                case "west":
                    return Direction.West;
            }

            return Direction.None;
        }

        public static Direction OppositeDirection(Direction dir)
        {
            switch (dir)
            {
                case Direction.North: return Direction.South;
                case Direction.East: return Direction.West;
                case Direction.South: return Direction.North;
                case Direction.West: return Direction.East;
            }
            return Direction.None;
        }

        public static string Str(Direction dir)
        {
            switch (dir)
            {
                case Direction.North: return "north";
                case Direction.East: return "east";
                case Direction.South: return "south";
                case Direction.West: return "west";
            }
            return string.Empty;
        }

        public static void ListItems(IList<Item> items)
        {
            foreach (var item in items)
            {
                Console.WriteLine($" *  {item.Name}");
            }
        }

        public static void WritePara(string text)
        {
            const int colWidth = 76;

            if (text.Length <= colWidth)
            {
                Console.WriteLine(text);
                return;
            }

            int index = 0;
            while (text.Length - index > colWidth)
            {
                int endIndex = index + colWidth;

                for (int i = endIndex; i > index; i--)
                {
                    if (text[i] == ' ')
                    {
                        endIndex = i;
                        break;
                    }
                }

                Console.WriteLine(text.Substring(index, endIndex - index));

                index = endIndex;
                while (index < text.Length && text[index] == ' ')
                {
                    index++;
                }
            }

            Console.WriteLine(text.Substring(index));
        }
    }
}
