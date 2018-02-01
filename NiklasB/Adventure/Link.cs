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
            //setting text and target to the Link argument
            Text = text;
            Target = target;
        }

        //page text of type string, read only
        public string Text { get; }
        //target of the page linkage of type page, read only
        public Page Target { get; }
    }
}
