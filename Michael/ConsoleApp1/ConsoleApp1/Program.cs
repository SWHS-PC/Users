using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            var currentPage = CreatePages();
            while (currentPage.Links.Count != 0)
            {
                Console.WriteLine(currentPage.Description);
                DisplayMenu(currentPage.Links);
                currentPage = GetMenuSelection(currentPage.Links);
            }
            Console.WriteLine(currentPage.Description);
            Console.ReadLine();
        }
        /// <summary>
        /// Creates all the pages and links in the story and returns the reference to the startpage
        /// </summary>
        static Page CreatePages()
        {
            var startPage = new Page("You are outside a scary house...");
            var homePage = new Page("You go home and survive");
            var foyerPage = new Page("You enter the foyer, its dark and can bearly see");
            var kitchenPage = new Page("While opening the refrigerator, it falls on you and you die");
            var mainRoomPage = new Page("The old floor gives out under you and you fall down an endless pit");
            startPage.AddLink("go home", homePage);
            startPage.AddLink("enter the house", foyerPage);
            foyerPage.AddLink("Leave house", startPage);
            foyerPage.AddLink("enter kitchen", kitchenPage);
            foyerPage.AddLink("enter the main room", mainRoomPage);
            return startPage;
        }
        static void DisplayMenu(List<Link> links)
        {
            Console.WriteLine("What do you do?");
            for (int i = 0; i < links.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {links[i].Text}");
            }
        }
        static Page GetMenuSelection(List<Link> links)
        {
            for(; ; )
            {
                char inputChar = Console.ReadKey().KeyChar;
                if (inputChar >= '1' && inputChar <= '9')
                {
                    int index = inputChar - '1';
                    if (index < links.Count)
                        return links[index].Destination;
                }
            }
        }
    }
}
