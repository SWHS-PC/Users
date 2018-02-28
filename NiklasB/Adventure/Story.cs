using System.Collections.Generic;

namespace Adventure
{
    class Story
    {
        public Story()
        {
            Title = "Text Adventure";
            Pages = new List<Page>();
        }

        public string Title { get; set; }
        public List<Page> Pages { get; }
        public Page StartPage { get; set; }
        public bool IsOneFile { get; set; }
    }
}
