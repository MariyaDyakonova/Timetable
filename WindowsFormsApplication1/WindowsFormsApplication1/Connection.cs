using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace TimeTableProject
{
    class Connection
    {
        //создаем соединение с БД
        static string connectionString = "server=localhost;user=root;database=facultytimetable;password=mariya1111;SslMode=none";
        public static MySqlConnection connection = new MySqlConnection(connectionString);

    }
}
