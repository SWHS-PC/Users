using System;
using System.Collections.Generic;
using System.IO;

namespace PrettyJson
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.WriteLine("PrettyJson <input.json> <output.json");
                return;
            }

            JsonNode rootNode;

            // Read the input JSON.
            using (TextReader textReader = new StreamReader(args[0]))
            {
                rootNode = new JsonReader(textReader).Parse();
            }

            // Write the output JSON.
            using (TextWriter textWriter = new StreamWriter(args[1]))
            {
                new JsonWriter(textWriter).Write(rootNode);
            }
        }
    }
}
