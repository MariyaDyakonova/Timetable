using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using MySql.Data.MySqlClient;


namespace WindowsFormsApplication1
{
    class Applic
    {
        public int getScalarCount(string query, MySqlConnection con)
        {
            MySqlCommand command = new MySqlCommand(query, con);
            object count = command.ExecuteScalar();
            return Convert.ToInt32(count);
        }

        public void getSqlWithAliasFor1Object(object titleObj, string ObjAlias, string query, MySqlConnection con)
        {


            var command = new MySqlCommand(query, con);
            command.Parameters.AddWithValue(ObjAlias, titleObj);
            command.ExecuteNonQuery(); //просто выполняет sql-выражение и возвращает количество измененных записей.


        }

        public void getSqlWithAliasFor2Object(object titleFirstObj, string FirstObjAlias, object titleSecondObj, string SecondObjAlias, string query, MySqlConnection con)
        {
            var command = new MySqlCommand(query, con);
            command.Parameters.AddWithValue(FirstObjAlias, titleFirstObj);
            command.Parameters.AddWithValue(SecondObjAlias, titleSecondObj);
            command.ExecuteNonQuery();
        }

        public void getSqlWithAliasFor3Object(object titleFirstObj, string FirstObjAlias, object titleSecondObj, string SecondObjAlias, object titleThirdObj, string ThirdObjAlias, string query, MySqlConnection con)
        {
            var command = new MySqlCommand(query, con);
            command.Parameters.AddWithValue(FirstObjAlias, titleFirstObj);
            command.Parameters.AddWithValue(SecondObjAlias, titleSecondObj);
            command.Parameters.AddWithValue(ThirdObjAlias, titleThirdObj);
            command.ExecuteNonQuery();
        }

        public DataTable fillDataTable(string query, MySqlConnection con, ref MySqlDataAdapter da, ref DataSet ds)
        {
            da = new MySqlDataAdapter(query, con);
            ds = new DataSet();
            da.Fill(ds);
            return ds.Tables[0];
        }

    }
}
