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
            "    Adventure /compile <storyFile.txt> <outputFile.html>\n";

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
                    if (args.Length != 3)
                    {
                        Console.WriteLine(m_usage);
                    }
                    else
                    {
                        CompileStory(args[1], args[2]);
                    }
                    break;

                default:
                    Console.WriteLine(m_usage);
                    return;
            }
        }

        static void PlayStory(string storyFileName)
        {
            Story story = StoryParser.Parse(storyFileName);
            if (story != null)
            {
                StoryPlayer.Play(story);
            }
        }

        static void CompileStory(string storyFileName, string outputFileName)
        {
            Story story = StoryParser.Parse(storyFileName);
            if (story != null)
            {
                StoryWriter.Write(story, outputFileName);
            }
        }
    }
}
