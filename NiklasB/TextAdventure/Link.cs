using System;
using System.Collections.Generic;

namespace TextAdventure
{
    enum Direction
    {
        None,
        North,
        East,
        South,
        West
    }

    /// <summary>
    /// One-way connection between rooms.
    /// </summary>
    class Link
    {
        public Direction Direction { get; set; }
        public Room From { get; set; }
        public Room To { get; set; }
        public Door Door { get; set; }
    }
}
