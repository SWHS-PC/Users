using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Adventure
{
    class Program
    {
        const string m_usage =
            "To play an interactive adventure:\n" +
            "\n" +
            "    Adventure /play <storyFile.txt>\n" +
            "\n" +
            "To compile an interactive adventure to HTML:\n" +
            "\n" +
            "    Adventure /compile [ /out <outputDir> ] <storyFile.txt>\n";

        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine(m_usage);
                return;
            }

            switch (args[0])
            {
                case "-play":
                case "/play":
                    if (args.Length != 2)
                    {
                        Console.WriteLine(m_usage);
                    }
                    else
                    {
                        PlayStory(args[1]);
                    }
                    break;

                case "-compile":
                case "/compile":
                    if (args.Length < 2)
                    {
                        Console.WriteLine(m_usage);
                    }
                    else
                    {
                        // The last argument must be the story file name.
                        int lastIndex = args.Length - 1;
                        string storyFileName = args[lastIndex];

                        // Process any other arguments.
                        string outputDir = null;

                        for (int i = 1; i < lastIndex; ++i)
                        {
                            switch (args[i])
                            {
                                case "-out":
                                case "/out":
                                    outputDir = args[++i];
                                    break;

                                default:
                                    Console.WriteLine(m_usage);
                                    return;
                            }
                        }

                        CompileStory(storyFileName, outputDir);
                    }
                    break;

                default:
                    Console.WriteLine(m_usage);
                    return;
            }
        }

        static void PlayStory(string storyFileName)
        {
            // Load the story file, returning if it fails.
            var startPage = LoadPages(storyFileName);
            if (startPage == null)
                return;

            // Play the game repeatedly until the user quits or says no.
            while (PlayStoryOnce(startPage))
            {
                Console.WriteLine("THE END\n\nDo you want to play again? [yn] ");

                if (!AskYesNo())
                    break;

                Console.WriteLine("\n------\n");
            }
        }

        static bool PlayStoryOnce(Page startPage)
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
                    var story = StoryParser.Parse(reader);
                    if (story != null)
                    {
                        return story.StartPage;
                    }
                }
            }
            catch (IOException)
            {
                Console.Error.Write($"Error: Could not open {fileName}.");
            }
            return null;
        }

        static void CompileStory(string fileName, string outputDir)
        {
            Story story = null;

            try
            {
                using (var reader = new StreamReader(fileName))
                {
                    story = StoryParser.Parse(reader);
                }
            }
            catch (IOException)
            {
                Console.Error.Write($"Error: Could not open {fileName}.");
            }

            if (story != null)
            {
                StoryWriter.Write(story, outputDir);
            }
        }
    }
}
