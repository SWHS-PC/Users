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
            var story = StoryParser.Parse(storyFileName);
            if (story != null)
            {
                StoryPlayer.Play(story);
            }
        }

        static void CompileStory(string fileName, string outputDir)
        {
            Story story = StoryParser.Parse(fileName);
            if (story != null)
            {
                StoryWriter.Write(story, outputDir);
            }
        }
    }
}
