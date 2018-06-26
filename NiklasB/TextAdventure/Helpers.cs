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
    }
}
