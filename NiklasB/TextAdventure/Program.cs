using System;
using System.Collections.Generic;
using System.Xml;

namespace TextAdventure
{
    class Program
    {
        static void Main()
        {
#if DEBUG
            CommandParserTest.Run();
#endif

            Room startRoom = null;

            using (var stream = typeof(Program).Assembly.GetManifestResourceStream("TextAdventure.Map.xml"))
            {
                startRoom = MapReader.Parse(stream);
            }

            if (startRoom != null)
            {
                var game = new Game(startRoom);
                game.Play();
            }
        }
    }
}
