using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Texashold
{
    class Program
    {
        static void Main(string[] args)
        {
            
        }
        public Deck()
        {
            cards = new Card[52];
            int check = 0;

            for (int suitVal = 1; suitVal < 4; suitVal++)
            {
                for (int rankVal = 1; rankVal < 14; rankVal++)
                {
                    if (suitVal == 1)
                    {
                        cards[check] = new Card(rankVal, "Spades");
                    }
                    else if (suitVal == 2)
                    {
                        cards[check] = new Card(rankVal, "Hearts");
                    }
                    else if (suitVal == 3)
                    {
                        cards[check] = new Card(rankVal, "Clubs");
                    }
                    else if (suitVal == 4)
                    {
                        cards[check] = new Card(rankVal, "Diamonds");
                    }
                }
            }
        }
}
