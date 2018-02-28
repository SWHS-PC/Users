using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace SL
{
    class matrix
    {
        //set counter
        //set random
        //set FallTime
        //set 
        static int Counter;
        static Random rand = new Random();
        
        static int FallTime = 100;
        static int MaxFall = 130;
        static int Clearing = 180;

        static ConsoleColor NormalColor = ConsoleColor.DarkGreen;
        static ConsoleColor GlowColor = ConsoleColor.Green;
        static ConsoleColor FancyColor = ConsoleColor.White;

        static char AsciiCharacter
        {
            get
            {
                int t = rand.Next(10);
                if (t <= 2)
                    return (char)(rand.Next(1));
                else
                    return (char)(rand.Next(3,30));
            }
        }

        public static void Run()
        {
            Console.ForegroundColor = NormalColor;
            
            int height = Console.WindowHeight;
            int width = Console.WindowWidth - 1;
            int[] y = new int[width];
            for (int x = 0; x < width; ++x)
            {
                y[x] = rand.Next(height);
            }

            //Console.Write(width + " " + y[0]);
            //Console.Read();

            //option a, normal
            while (true)
            {
                UpdateAllColumns(width, height, y);
                Thread.Sleep(75);
            }
            //option b, pulse
            while (true)
            {
                Counter++;
                UpdateAllColumns(width, height, y);
                if (Counter > (3 * FallTime))
                    Counter = 0;
            }
        }

        private static void UpdateAllColumns(int width, int height, int[] y)
        {
            
            if (Counter < FallTime)
            {
                for (int x = 0; x < width; ++x)
                {
                    if (x % 10 == 1)//Randomly setting up the White Position
                    {
                        Console.ForegroundColor = FancyColor;
                    }
                    else
                    {
                        Console.ForegroundColor = GlowColor;
                    }

                    Console.SetCursorPosition(x, y[x]);
                    Console.Write(AsciiCharacter);
                    

                    if (x % 10 == 9)
                    {
                        Console.ForegroundColor = FancyColor;
                    }
                    else
                    {
                        Console.ForegroundColor = NormalColor;
                    }

                    Console.SetCursorPosition(x, inScreenYPosition(y[x] - 2, height));
                    Console.Write(AsciiCharacter);
                    

                    Console.SetCursorPosition(x, inScreenYPosition(y[x] - 20, height));
                    Console.Write(' ');

                    y[x] = inScreenYPosition(y[x] + 1, height);
                }
            }


            else if (Counter > FallTime && Counter < MaxFall)
            {
                for (int x = 0; x < width; ++x)
                {

                    Console.SetCursorPosition(x, y[x]);
                    if (x % 10 == 9)
                        Console.ForegroundColor = FancyColor;
                    else
                        Console.ForegroundColor = NormalColor;

                    Console.Write(AsciiCharacter);//Printing the Character Always at Fixed position

                    y[x] = inScreenYPosition(y[x] + 1, height);
                }
            }


            else if (Counter > MaxFall)
            {
                for (int x = 0; x < width; ++x)
                {
                    Console.SetCursorPosition(x, y[x]);
                    Console.Write(' ');//Slowly Clearing out the Screen
                    Console.SetCursorPosition(x, inScreenYPosition(y[x] - 20, height));
                    Console.Write(' ');
                    if (Counter > MaxFall && Counter < Clearing)// Clearing the Entire screen to get the Darkness
                    {
                        if (x % 10 == 9)
                            Console.ForegroundColor = FancyColor;
                        else
                            Console.ForegroundColor = NormalColor;
                        Console.SetCursorPosition(x, inScreenYPosition(y[x] - 2, height));
                        Console.Write(AsciiCharacter);//The Text is printed Always

                    }
                    y[x] = inScreenYPosition(y[x] + 1, height);
                }
            }
        }

        public static int inScreenYPosition(int yPosition, int height)
        {
            if (yPosition < 0)//When there is negative value
                return yPosition + height;
            else if (yPosition < height)//Normal 
                return yPosition;
            else// When y goes out of screen when autoincremented by 1
                return 0;
        }
    }
}
