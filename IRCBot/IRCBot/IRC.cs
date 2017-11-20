using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace IRCBot
{
    public class IRC
    {

        private readonly string aserver;
        private readonly int aport;
        private readonly string anick;
        private readonly string achan;

        private readonly string auser;
        private readonly int amaxRetries;

        public IRC(string server, int port, string user, string nick, string chan, int maxRetries = 3)
        {
            aserver = server;
            aport = port;
            anick = nick;
            achan = chan;
            auser = user;
            amaxRetries = maxRetries;
        }
        public void Run()
        {
            var recon = false;
            var reconAmount = 0;

            Console.Write("Server: ");
            string aserver = Console.ReadLine();
            Console.Write("Port: ");
            int aport = Convert.ToInt32(Console.ReadLine());
            Console.Write("Channel<#chan>: ");
            string achan = Console.ReadLine();
            Console.Write("Nick: ");
            string anick = Console.ReadLine();

            try
            {
                using (var irc = new TcpClient(aserver, aport))
                using (var stream = irc.GetStream())
                using (var recieve = new StreamReader(stream))
                using (var send = new StreamWriter(stream))
                {
                    send.WriteLine("NICK " + anick);
                    send.WriteLine(auser);
                    send.Flush();

                    while (true)
                    {
                        string input;
                        while ((input = recieve.ReadLine()) != null)
                        {
                            Console.WriteLine("<- " + input);

                            string[] splitInput = input.Split(' ');

                            if (splitInput[0] == "PING")
                            {
                                string reply = splitInput[1];
                                send.WriteLine("PONG" + reply);
                                send.Flush();
                            }
                            switch (splitInput[1])
                            {
                                case "1":
                                    send.WriteLine("JOIN " + achan);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }

                }
                catch (ArgumentNullException e)
                {
                    throw e;

                    Console.WriteLine(e.ToString());
                    Thread.Sleep(5000);
                    recon = ++reconAmount <= amaxRetries;
                }
        }
    }
}
