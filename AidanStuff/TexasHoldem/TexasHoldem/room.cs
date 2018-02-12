using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tad
{
    class room
    {
        public room(string name)
        {
            rname = name;
        }

        string rname;

        public string Name
        {
            get { return rname; }
        }

        List<item> ritems = new List<item>();
        public List<item> items => ritems;

        public room North { get; set; }
        public room East  { get; set; }
        public room South { get; set; }
        public room West  { get; set; }

        public void Describe()
        {
            Console.WriteLine($"This is the{Name}.");
            if (items.Count != 0)
            {
                Console.WriteLine("In this room are the following items:");
                foreach (var item in items)
                {
                    Console.WriteLine(item.Name);
                }
            }

            if(North != null)
            {
                Console.WriteLine($"To the North is the {North.Name}.");
            }
            if (East != null)
            {
                Console.WriteLine($"To the East is the {East.Name}.");
            }
            if (South != null)
            {
                Console.WriteLine($"To the South is the {South.Name}.");
            }
            if (West != null)
            {
                Console.WriteLine($"To the West is the {West.Name}.");
            }
        }
    }
}
