using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Link
    {
        public Link(string text, Page destination)
        {
            Text = text;
            Destination = destination;
        }
        public string Text { get; }
        public Page Destination { get; }
    }
}
