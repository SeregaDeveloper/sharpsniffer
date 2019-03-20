// класс, в котором будут находиться все основные операции для работы над БД

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using System.Data.Common;
using System.Data;

namespace SharpSniffer
{
    class Loader
    {
        static void Main(string[] args)
        {

            // Получаю соединение к базе данных.
            MySqlConnection connection = DbUtils.GetDBConnection();
            connection.Open();
            try
            {
                // Команда Insert.
                string sql = "Insert into  "  + " values  ";

                MySqlCommand cmd = connection.CreateCommand();
                cmd.CommandText = sql;

               // тут будут объявлены все основные операции производимые над БД
             

               
               
               // Выполнить Command (использованная для  delete, insert, update).
                int rowCount = cmd.ExecuteNonQuery();

                Console.WriteLine("Row Count affected = " + rowCount);
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e);
                Console.WriteLine(e.StackTrace);
            }
            finally
            {
                connection.Close();
                connection.Dispose();
                connection = null;
            }


            Console.Read();

        }
    }

}
// CREATED BY SERGEY BESEDIN
