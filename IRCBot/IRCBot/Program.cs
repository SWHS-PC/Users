using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace IRCBot
{
    class Program
    {
        static void Main(string[] args)
        {
            var ircBot = new IRC();

            ircBot.Run();
        }
    }
}
