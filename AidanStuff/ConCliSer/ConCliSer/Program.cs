using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace ConCliSer
{
    class Program
    {
        public static void Main(string[] args)
        {
            Client cli = new Client();
            Server ser = new Server();
            Console.WriteLine("Client or Server (c/s)");
            string choice = Console.ReadLine();
            if(choice == "c")
            {
                cli.Run();
            }
            else if (choice == "s")
            {
                ser.Run();
            }
               
        }
    }
}
