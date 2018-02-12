using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAdven
{
    class Program
    {
        static void Main(string[] args)
        {
            var currentPage = CreatePages();

            while (currentPage.Links.Count != 0)
            {
                Console.WriteLine(currentPage.Description);
                Display(currentPage.Links);

                currentPage = GetMenuSelector(currentPage.Links);
            }

            Console.WriteLine(currentPage.Description);
        }
        /// <summary>
        /// Creates all pages from story and links to each page/room, starting with the start page.
        /// </summary>
        /// <returns>startPage</returns>
        static page CreatePages()
        {
            var startPage = new page("Welcome to death house.");
            var chickenPage = new page("You scaredy cat gtfo!");
            var foyerPage = new page("You've entered the house of death, your in the main foyer.");
            var kitchenPage = new page("You enter the kitchen to then be stabbed with a kitchen knife!");
            var livingRoomPage = new page("You get killed by Death himself in the livingroom");

            startPage.AddLink("Chicken Out", chickenPage);
            startPage.AddLink("Enter the house of Death", foyerPage);
            foyerPage.AddLink("Enter the kitchen", kitchenPage);
            foyerPage.AddLink("Enter the livingroom", livingRoomPage);

            return startPage;
        }
        static void Display(List<Link> links)
        {
            Console.WriteLine("Choose your destination.");
            for(int i = 0; i < links.Count; i++)
            {
                Console.WriteLine($"{i + 1}.) {links[i].Text}. ");
            }
        }
        static page GetMenuSelector(List<Link> links)
        {
            for(; ; )
            {
                char inputChar = Console.ReadKey().KeyChar;
                if(inputChar >= '1' && inputChar <= '9')
                {
                    int index = inputChar - '1';
                    if (index < links.Count)
                    {
                        return links[index].Destination;
                    }
                }
            }
        }
    }
}
