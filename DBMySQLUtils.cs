// Reqire for connect database 

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using MySql.Data.MySqlClient;

namespace SharpSniffer
{
    class DBMySQLUtils
    {
        public static MySqlConnection
                GetDBConnection(string host, int port, string database, string username, string password)
        {
            // Connection string
            String connString = "Server=" + host + ";Database=" + database
                + ";port=" + port + ";User Id=" + username + ";password=" + password;
            MySqlConnection conn = new MySqlConnection(connString);
            return conn;
        }

    }
}
