using System;
using System.Collections.Generic;
using System.IO;

namespace PrettyJson
{
    class Program
    {
        const string Usage = "PrettyJson [/f] <input.json> <output.json>";

        static void Main(string[] args)
        {
            bool isFormatted = false;
            string inputPath = null;
            string outputPath = null;

            foreach (var arg in args)
            {
                if (arg == "/f" || arg == "-f")
                {
                    isFormatted = true;
                }
                else if (inputPath == null)
                {
                    inputPath = arg;
                }
                else if (outputPath == null)
                {
                    outputPath = arg;
                }
                else
                {
                    // Too many arguments or invalid arg.
                    Console.WriteLine(Usage);
                    return;
                }
            }

            if (outputPath == null)
            {
                // Too few arguments.
                Console.WriteLine(Usage);
                return;
            }

            JsonNode rootNode;

            // Read the input JSON.
            using (TextReader textReader = new StreamReader(inputPath))
            {
                var reader = new JsonReader(textReader);
                rootNode = reader.Parse();
            }

            // Write the output JSON.
            using (TextWriter textWriter = new StreamWriter(outputPath))
            {
                var writer = new JsonWriter(textWriter);
                writer.IsFormatted = isFormatted;
                writer.Write(rootNode);
            }
        }
    }
}
