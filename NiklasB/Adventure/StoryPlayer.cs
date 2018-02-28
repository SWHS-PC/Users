using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Adventure
{
    class StoryPlayer
    {
        static public void Play(Story story)
        {
            // PlayOnce returns true if the user reaches the end of the story.
            // If so, ask the user whether to play again.
            while (PlayOnce(story))
            {
                Console.WriteLine("THE END\n\nDo you want to play again? [yn] ");
                if (!ReadYesNo())
                    break;

                Console.WriteLine("\n------\n");
            }
        }

        static bool PlayOnce(Story story)
        {
            var currentPage = story.StartPage;

            // Loop until we reach an ending page (one with no links).
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

        static bool ReadYesNo()
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
    }
}
