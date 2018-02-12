using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdven
{
    class page
    {
        public page(string description)
        {
            Description = description;
            Links = new List<Link>();
        }
        public void AddLink(string text, page destination)
        {
            Links.Add(new Link(text, destination));
        }

        public string Description { get; }

        public List<Link> Links { get; } 
    }
}
