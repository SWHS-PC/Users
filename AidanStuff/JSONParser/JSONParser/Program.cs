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
        static void Main(string[] args)
        {
            if(args.Length != 2)
            {
                Console.WriteLine("fixyoshit");
                return;
            }

            JSONNode rootNode;
            using (TextReader textReader = new StreamReader(args[0]))
            {
                rootNode = new JSONReader(textReader).Parse();
            }
            using (TextWriter textWriter = new StreamWriter(args[1]))
            {
                new JSONWriter(textWriter).Write(rootNode);

            }
        }
    }
}
