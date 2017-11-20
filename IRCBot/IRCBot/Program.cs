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
            var ircBot = new IRC(server: "4bit.pw", port: 6667, nick: "abot", user: "USER abot 0 * :abot", chan: "#GRP");

            ircBot.Run();
        }
    }
}
