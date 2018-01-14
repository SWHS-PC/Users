using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdventure
{
    class Program
    {
        static void Main(string[] args)
        {
            var game = new GameState();
            game.Run();
        }

        public static void WriteParagraph(string text)
        {
            const int maxLength = 76;

            while (text.Length > maxLength)
            {
                int wordBreak = text.IndexOf(' ', 1);

                if (wordBreak < 0)
                    break;

                while (wordBreak < maxLength)
                {
                    int i = text.IndexOf(' ', wordBreak + 1);
                    if (i >= 0 && i <= maxLength)
                    {
                        wordBreak = i;
                    }
                    else
                    {
                        break;
                    }
                }

                Console.WriteLine(text.Substring(0, wordBreak));
                text = text.Substring(wordBreak + 1);
            }

            Console.WriteLine(text);
        }
    }
}
