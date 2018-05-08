using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using MySql.Data.MySqlClient;

namespace FirstSeed
{
    public class dbconnect
    {
        public static MySqlConnection myConnection = new MySqlConnection("User ID=Admin;" +
                                                   "password=SalenHale21;Data Source=www.getrect.xyz;" +
                                                   "Initial Catalog=VS; " +
                                                   "connection timeout=5;" + "SslMode=none ");
        public static void Run()
        {
            StartConnection();
            Command();
            Console.Read();
            EndConnection();
        }

        public static void StartConnection()
        {
            try
            {
                myConnection.Open();
            }
            catch (MySqlException e)
            {
                Debug.WriteLine(e.ToString());
            }
        }  
        public static void Command()
        {
            MySqlCommand myCommand = new MySqlCommand("INSERT INTO `VS`.`Queries` (`a`) VALUES('test')", myConnection);
            myCommand.ExecuteNonQuery();
        }
        public static void EndConnection()
        {
            try
            {
                myConnection.Close();
            }
            catch (MySqlException e)
            {
                Console.WriteLine(e.ToString());
            }
        }
    }
}
