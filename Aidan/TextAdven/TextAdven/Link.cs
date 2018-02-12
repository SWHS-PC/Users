using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdven
{
    class Link
    {
        public Link(string text, page destination)
        {
            Text = text;
            Destination = destination;
        }

        public string Text { get; }
        public page Destination { get; }

    }
}
