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

        public static void StartConnection()
        {
            try
            {
                myConnection.Open();
                Console.Read();
                EndConnection();
                //MySqlCommand myCommand = new MySqlCommand("INSERT INTO `VS`.`test` (`test`) VALUES('22')", myConnection);
                //myCommand.ExecuteNonQuery();
            }
            catch (MySqlException e)
            {
                Debug.WriteLine(e.ToString());
            }
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
