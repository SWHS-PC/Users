using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventure
{
    class MapLink
    {
        public Room To { get; set; }
        public Door Door { get; set; }
        public string Description { get; set; }
    }
    class Door
    {
        public string Name { get; set; }
        public bool IsLocked { get; set; }
    }
}
