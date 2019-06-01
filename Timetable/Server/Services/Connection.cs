using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

namespace Server.Services
{
    public class Connection
    {
        // создаем соединение с БД
        string connectionString = "server=den1.mysql5.gear.host;user=facultytimetabl1;database=facultytimetabl1;password=olya123.";
        public MySqlConnection connection;

        public Connection()
        {
            connection = new MySqlConnection(connectionString);
            connection.Open();
        }

        public List<List<string>> Execute(string sql)
        {
            var reader = new MySqlCommand(sql, connection).ExecuteReader();
            var result = new List<List<string>>();
            while (reader.Read())
            {
                var tmp = new List<string>();
                for (var i = 0; i < reader.FieldCount; i++)
                {
                    tmp.Add(reader.GetString(i));
                }
                result.Add(tmp);
            }
            reader.Close();
            return result;
        }

        public void ExecuteNonQuery(string sql, List<Tuple<string, string>> parameters)
        {
            var command = new MySqlCommand(sql, connection);
            command.Prepare();
            foreach (var parameter in parameters)
            {
                command.Parameters.AddWithValue(parameter.Item1, parameter.Item2);
            }
            command.ExecuteNonQuery();
        }
    }
}