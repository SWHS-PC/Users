using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventure
{
    interface IOpenable
    {
        bool IsOpen { get; set; }
        bool IsLocked { get; set; }
        Item Key { get; set; }
    }

    class Door : IOpenable
    {
        public bool IsOpen { get; set; }
        public bool IsLocked { get; set; }
        public Item Key { get; set; }
    }
}
