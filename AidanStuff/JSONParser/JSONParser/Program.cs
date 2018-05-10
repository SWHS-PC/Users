using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JSONParser
{
    class Program
    {
        const string usage = "fixyoshit";
        static void Main(string[] args)
        {
            bool isFormatted = false;
            string InputPath = null;
            string OutputPath = null;
            foreach(var arg in args)
            {
                if (arg == "/f" || arg == "-f")
                {
                    isFormatted = true;
                }
                else if(InputPath == null)
                {
                    InputPath = arg;
                }
                else if (OutputPath == null)
                {
                    OutputPath = arg;
                }
                else
                {
                    Console.WriteLine(usage);
                    return;
                }
            }
            if(OutputPath == null)
            {
                Console.WriteLine(usage);
                return;
            }

            JSONNode rootNode;
            using (TextReader textReader = new StreamReader(InputPath))
            {
                rootNode = new JSONReader(textReader).Parse();
            }
            using (TextWriter textWriter = new StreamWriter(OutputPath))
            {
                var writer = new JSONWriter(textWriter);
                writer.IsFormatted = isFormatted;
                writer.Write(rootNode);

            }
        }
    }
}
