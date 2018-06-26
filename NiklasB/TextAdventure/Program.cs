using System;
using System.Collections.Generic;
using System.Xml;

namespace TextAdventure
{
    class Program
    {
        static void Main()
        {
            Room startRoom = null;

            using (var mapStream = typeof(Program).Assembly.GetManifestResourceStream("TextAdventure.Map.xml"))
            {
                using (var reader = XmlReader.Create(mapStream))
                {
                    startRoom = MapReader.Parse(reader);
                }
            }

            if (startRoom != null)
            {
                var game = new Game(startRoom);
                game.Play();
            }
        }
    }
}
