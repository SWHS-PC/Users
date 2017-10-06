//
// This module demonstrates classes and objects by implementing a simple
// (and very boring) text adventure game.
//
// A class defines a kind of thing, and an object is a particular thing.
// In other words, an object is an instance of a class.
//
// This module defines two classes. The TextAdventure class represents the
// state of the game as a whole (such as the current room), and the Room
// class represents a particular room in the game map.
//
using System;

namespace HelloWorld
{
    class TextAdventure
    {
        public static void Run()
        {
            // Create an instance of the TextAdventure class using a new statement.
            var game = new TextAdventure();

            // The game variable is now a reference to a TextAdvanture object.
            // Call that object's Play method.
            game.Play();
        }

        // Following are member variables (aka. fields) of the TextAdventure class.
        //
        // They are not declared "static", so each TextAdventure object has its own
        // copy of these variables.
        //
        // The are also not declared "public", so these variables are only visible
        // to code within this class. They are part of its internal implementation,
        // not its public interface.
        //
        // When a TextAdventure object is created (using "new"), each of its member
        // variables is automatically initialized. In this case, the initial value
        // of each variable is specified explicitly. For example, m_currentRoom is
        // initialized to the return value of the CreateMap method. If you set a
        // break point on CreateMap, you'll find that it is called during the
        // creation of the TextAdventure object.
        Room m_currentRoom = CreateMap();
        bool m_isGameOVer = false;

        /// <summary>
        /// The Play method is called by TextAdventure.Run and executes the game.
        /// This method is not declared "static", so it must be called on specific
        /// TextAdventure object.
        /// </summary>
        void Play()
        {
            Console.WriteLine(
                "Welcome to Text Adventure!\n" +
                "\n" +
                "At the prompt, you can type a direction ('n', 's', 'e', or 'w')\n" +
                "or type 'q' or 'x' to exit.\n"
                );

            // The Room class (defined later in this module) has a Describe method
            // that outputs a description of the room. Begin the game by describing
            // the current room.
            m_currentRoom.Describe();

            // Repeatedly process user input until the game is over.
            while (!m_isGameOVer)
            {
                // Display a prompt.
                Console.Write("> ");

                // Branch depending on the user's command.
                switch (Console.ReadLine())
                {
                    case "n":
                        // Move to the room North of the current room, if any.
                        TryMove(m_currentRoom.North);
                        break;

                    case "s":
                        // Move to the room South of the current room, if any.
                        TryMove(m_currentRoom.South);
                        break;

                    case "e":
                        // Move to the room East of the current room, if any.
                        TryMove(m_currentRoom.East);
                        break;

                    case "w":
                        // Move to the room West of the current room, if any.
                        TryMove(m_currentRoom.West);
                        break;

                    case "q":
                    case "x":
                        // Set the game-over field to end the game.
                        m_isGameOVer = true;
                        break;
                }
            }
        }

        /// <summary>
        /// Moves to the specified room if it is not null.
        /// </summary>
        bool TryMove(Room newRoom)
        {
            if (newRoom == null)
            {
                Console.WriteLine("You can't move that way.");
                return false;
            }
            else
            {
                newRoom.Describe();
                m_currentRoom = newRoom;
                return true;
            }
        }

        /// <summary>
        /// Creates a number of linked Room objects.
        /// </summary>
        /// <returns>Returns a reference to the starting room.</returns>
        /// <summary>
        /// Each Room object has references to neighboring Room objects in
        /// each of the four cardinal directions. These neighboring rooms
        /// are exposed as properties (North, South, East, and West). If
        /// there is no neighboring room in a given direction then the
        /// corresponding property is null.
        /// </summary>
        static Room CreateMap()
        {
            // Create each of the rooms.
            Room hallway = new Room("hallway");
            Room coatCloset = new Room("coat closet");
            Room livingRoom = new Room("living room");
            Room diningRoom = new Room("dining room");
            Room kitchen = new Room("kitchen");

            // Link the hallway and coat closet together.
            hallway.West = coatCloset;
            coatCloset.East = hallway;

            // Link the hallway and living room together.
            hallway.East = livingRoom;
            livingRoom.West = hallway;

            // Link the living room and dining room together.
            livingRoom.North = diningRoom;
            diningRoom.South = livingRoom;

            // Link the dining room and kitchen together.
            diningRoom.West = kitchen;
            kitchen.East = diningRoom;

            // Link the kitchen and hallway together.
            kitchen.South = hallway;
            hallway.North = kitchen;

            // Return the starting room.
            return hallway;
        }
    }

    /// <summary>
    /// The Room class represents a room in the text adventure game.
    /// </summary>
    class Room
    {
        /// <summary>
        /// Room object constructor.
        /// </summary>
        /// <param name="name">Specifies the value of the Name property.</param>
        /// <remarks>
        /// A constructor is a special method, which has the same name as the class
        /// it belongs to and no return value. A constructor is called automatically
        /// when an instance of the class is created. Its job is to do any additional
        /// initialization beyond what has already been done by field initializers.
        /// In this case, the constructor sets the Name property to the specified
        /// value. If a constructor has parameters, they are specified as part of
        /// the "new" statement.
        /// </remarks>
        public Room(string name)
        {
            Name = name;
        }

        // The Name property is a string representing the name of the room. It is
        // read-only. You can "get" it, but you can't "set" except in the constructor.
        public string Name { get; }

        // Each of the following properties is a reference to a neighboring Room
        // object (if any) in a particular direction. Each property is automatically
        // initialized to null, meaning there is no room in that direction. The
        // TextAdventure.CreateMap method links rooms together by setting these
        // properties.
        public Room North { get; set; }
        public Room East { get; set; }
        public Room South { get; set; }
        public Room West { get; set; }

        /// <summary>
        /// The Describe method outputs a description of the room to the console.
        /// </summary>
        public void Describe()
        {
            Console.WriteLine("You are in the {0}.", Name);

            if (North != null)
                Console.WriteLine("There is a door to the North.");

            if (East != null)
                Console.WriteLine("There is a door to the East.");

            if (South != null)
                Console.WriteLine("There is a door to the South.");

            if (West != null)
                Console.WriteLine("There is a door to the West.");
        }
    }
}
