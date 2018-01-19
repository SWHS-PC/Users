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
            // Make sure a story file is specified on the command line.
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: Adventure <storyFile.txt>");
                return;
            }

            // Load the story file, returning if it fails.
            var startPage = LoadPages(args[0]);
            if (startPage == null)
                return;

            // Play the game repeatedly until the user quits or says no.
            while (PlayGame(startPage))
            {
                Console.WriteLine("THE END\n\nDo you want to play again? [yn] ");

                if (!AskYesNo())
                    break;

                Console.WriteLine("\n------\n");
            }
        }

        static bool PlayGame(Page startPage)
        {
            var currentPage = startPage;

            // Loop until we reach and ending page (one with no links).
            while (currentPage.Links.Count != 0)
            {
                // Display the current page description.
                Console.WriteLine(currentPage.Description);

                // Transition to the next page based on user input.
                currentPage = GetSelectedPage(currentPage.Links);

                // Return false if the user chose to quit.
                if (currentPage == null)
                    return false;
            }

            // Display the ending page description.
            Console.WriteLine(currentPage.Description);
            return true;
        }

        static bool AskYesNo()
        {
            for (; ; )
            {
                switch (Console.ReadKey().KeyChar)
                {
                    case 'y':
                        Console.WriteLine();
                        return true;

                    case 'n':
                        Console.WriteLine();
                        return false;
                }
            }
        }

        static Page GetSelectedPage(List<Link> links)
        {
            // Automatically advance if there's only one link.
            if (links.Count == 1)
            {
                return links[0].Target;
            }

            // Display the menu.
            Console.WriteLine("You can:");
            for (int i = 0; i < links.Count; ++i)
            {
                Console.WriteLine($" {i + 1}. {links[i].Text}");
            }
            Console.Write("> ");

            // Loop indefinitely until the user presses a valid key.
            for (; ; )
            {
                char key = Console.ReadKey().KeyChar;

                if (key >= '1' && key <= '9')
                {
                    // It's a digit, so convert it to a zero-based index.
                    // If the index is in range, return the link target.
                    int index = key - '1';
                    if (index < links.Count)
                    {
                        Console.WriteLine("\n\n------\n");
                        return links[index].Target;
                    }
                }
                else if (key == 'q')
                {
                    // The user pressed the quit key, so return null.
                    Console.WriteLine();
                    return null;
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
