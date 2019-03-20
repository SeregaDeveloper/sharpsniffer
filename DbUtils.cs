// класс, содержащий данные для соединения с сервером
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
            string host = "127.0.0.1";
            int port = 3306;
            string database = "ваша ДБ";
            string username = "ваш MySQL юзер";
            string password = "ваш пароль";
            return DBMySQLUtils.GetDBConnection(host, port, database, username, password);
        }

    }
}
