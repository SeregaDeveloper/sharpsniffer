// Class witch contains the data to connect to the DB server

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MySql.Data.MySqlClient;

namespace SharpSniffer
{
    class DbUtils
    {
        public static MySqlConnection GetDBConnection()
        {
            // Data
            
            string host = "127.0.0.1";
            int port = 3306;
            string database = "Your_DB_name";
            string username = "Your_DB_User";
            string password = "Your_User_Pass";
            return DBMySQLUtils.GetDBConnection(host, port, database, username, password);
        }

    }
}
// CREATED BY SERGEY BESEDIN
