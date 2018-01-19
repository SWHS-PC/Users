using System;
using System.Collections.Generic;

namespace Adventure
{
    /// <summary>
    /// A link from one page in the interactive story to another.
    /// </summary>
    class Link
    {
        public Link(string text, Page target)
        {
            Text = text;
            Target = target;
        }

        public string Text { get; }
        public Page Target { get; }
    }
}
