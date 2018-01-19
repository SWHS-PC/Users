using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Adventure
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: Adventure <fileName.txt>");
                return;
            }

            var currentPage = LoadPages(args[0]);
            if (currentPage == null)
            {
                return;
            }

            while (currentPage.Links.Count != 0)
            {
                Console.WriteLine(currentPage.Description);
                currentPage = GetSelectedPage(currentPage.Links);
            }

            Console.WriteLine(currentPage.Description);

            Console.WriteLine("\nTHE END - Press ENTER to exit.");
            Console.ReadLine();
        }

        static Page GetSelectedPage(List<Link> links)
        {
            if (links.Count == 1)
            {
                return links[0].Target;
            }

            Console.WriteLine("You can:");
            for (int i = 0; i < links.Count; ++i)
            {
                Console.WriteLine($" {i + 1}. {links[i].Text}");
            }

            Console.Write("> ");

            for (; ; )
            {
                char key = Console.ReadKey().KeyChar;
                if (key >= '1' && key <= '9')
                {
                    int index = key - '1';
                    if (index < links.Count)
                    {
                        Console.WriteLine("\n\n------\n");
                        return links[index].Target;
                    }
                }
            }
        }

        static Page LoadPages(string fileName)
        {
            try
            {
                using (var reader = new StreamReader(fileName))
                {
                    var parser = new StoryParser();

                    if (parser.Parse(reader))
                    {
                        return parser.StartPage;
                    }
                }
            }
            catch (IOException)
            {
                Console.Error.Write($"Error: Could not open {fileName}.");
            }
            return null;
        }
    }
}
