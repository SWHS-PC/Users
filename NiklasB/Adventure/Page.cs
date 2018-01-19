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
            Links = new List<Link>();
        }

        public string Name { get; }
        public string Description { get; set; }
        public List<Link> Links { get; }
    }
}
