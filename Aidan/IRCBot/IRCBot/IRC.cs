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

        string server = "4bit.pw";
        int port = 6667;
        string nick = "abot";
        string chan = "#GRP";
        string user = "USER abot 0 * :abot";
        int maxRetries;

        
        public void Run()
        {
            var recon = false;
            var reconAmount = 0;


            try
            {
                using (var irc = new TcpClient(server, port))
                using (var stream = irc.GetStream())
                using (var recieve = new StreamReader(stream))
                using (var send = new StreamWriter(stream))
                {
                    send.WriteLine("NICK " + nick);
                    send.WriteLine(user);
                    send.Flush();

                    while (true)
                    {
                        string input;
                        while ((input = recieve.ReadLine()) != null)
                        {
                            Console.WriteLine("< " + input);

                            string[] splitInput = input.Split(' ');

                            if (splitInput[0] == "PING")
                            {
                                string reply = splitInput[1];
                                send.WriteLine("PONG " + reply);
                                send.Flush();
                            }
                            else if (splitInput[1] == "376" || splitInput[1] == "422")
                            {
                                send.WriteLine("JOIN " + chan);
                            }
                            
                            //if (splitInput[3] == ":a")
                            //{
                            //    send.WriteLine("PRIVMSG " + "a");
                            //}
                        }
                    }
                }

                }
                catch (ArgumentNullException e)
                {
                    throw e;

                    Console.WriteLine(e.ToString());
                    Thread.Sleep(5000);
                    recon = ++reconAmount <= maxRetries;
                }
        }
    }
}
