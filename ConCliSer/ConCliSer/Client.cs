using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

namespace ConCliSer
{
    class Client
    {
        public void Run()
        {
            string server = Console.ReadLine();
            int port = 6877;

            using (var conn = new TcpClient(server, port))
            {
                conn.Connect(server, port);

                if (conn.Connected == true)
                {
                    Console.WriteLine("Connected");
                }
                Console.Read();
                conn.Close();
            }
        }
    }
}
