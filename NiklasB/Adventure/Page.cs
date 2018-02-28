using System;
using System.Collections.Generic;

namespace Adventure
{
    /// <summary>
    /// Represents a point in the story.
    /// </summary>
    class Page
    {
        public Page(string name)
        {
            Name = name;
            // links is a list
            Links = new List<Link>();
        }

        //room name of type string
        public string Name { get; }
        //room description of type string
        public string Description { get; set; }
        public List<Link> Links { get; }
    }
}
