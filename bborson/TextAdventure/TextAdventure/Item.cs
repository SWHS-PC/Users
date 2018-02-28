using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventure
{
    abstract class Item
    {
        public string Name { get; set; }
        public abstract void Use(ref Room currentRoom);
    }
    class KeyItem : Item
    {
        public override void Use(ref Room currentRoom)
        {
            if (currentRoom.North?.Door == Door ||
                currentRoom.South?.Door == Door || 
                currentRoom.East?.Door == Door || 
                currentRoom.West?.Door == Door)
            {
                if (Door.IsLocked)
                {
                    Door.IsLocked = false;
                    Console.WriteLine($"The {Door.Name} is now unlocked.");
                }
                else
                {
                    Console.WriteLine($"The {Door.Name} is already open.");
                }
            }
            else
            {
                Console.WriteLine("You can't use that here.");
            }
        }
        public Door Door { get; set; }
    }
    class MessageItem : Item
    {
        public override void Use(ref Room currentRoom)
        {
            if (currentRoom == ActiveRoom)
            {
                Program.WriteParagraph(Message);
            }
            else
            {
                Console.WriteLine("You can't use that here.");
            }
        }
        public string Message { get; set; }
        public Room ActiveRoom { get; set; }
    }
    class TeleportItem : Item
    {
        public override void Use(ref Room currentRoom)
        {
            if (currentRoom == Room1)
            {
                currentRoom = Room2;
                currentRoom.Describe();
            }
            else if (currentRoom == Room2)
            {
                currentRoom = Room1;
                currentRoom.Describe();
            }
            else
            {
                Console.WriteLine("You can't use that here.");
            }
        }
        public Room Room1 { get; set; }
        public Room Room2 { get; set; }
    }
}
