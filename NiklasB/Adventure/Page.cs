using System;
using System.Collections.Generic;

namespace Adventure
{
    /// <summary>
    /// Represents a point in the story.
    /// </summary>
    class Page
    {
        public string Description { get; set; }

        List<Link> m_links = new List<Link>();
        public List<Link> Links => m_links;
    }
}
