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

        public void getSqlWithAliasFor4Object(object titleFirstObj, string FirstObjAlias, object titleSecondObj, string SecondObjAlias, object titleThirdObj, string ThirdObjAlias, object titleFourthObj, string FourthObjAlias, string query, MySqlConnection con)
        {
            var command = new MySqlCommand(query, con);
            command.Parameters.AddWithValue(FirstObjAlias, titleFirstObj);
            command.Parameters.AddWithValue(SecondObjAlias, titleSecondObj);
            command.Parameters.AddWithValue(ThirdObjAlias, titleThirdObj);
            command.Parameters.AddWithValue(FourthObjAlias, titleFourthObj);
            command.ExecuteNonQuery();
        }
        public DataTable fillDataTable(string query, MySqlConnection con, ref MySqlDataAdapter da, ref DataSet ds)
        {
            da = new MySqlDataAdapter(query, con);
            ds = new DataSet();
            da.Fill(ds);
            return ds.Tables[0];
        }
        public List<string> getGroupList(string query, MySqlConnection con)
        {
            MySqlCommand command = new MySqlCommand(query, con);
            MySqlDataReader reader = command.ExecuteReader();
            List<string> ourList = new List<string>();
            if (reader.HasRows) //Получает значение, указывающее, содержит ли объект SqlDataReader одну или несколько строк.
                
            {
                while (reader.Read())
                {
                    ourList.Add(reader.GetString(0));
                }
            }
            reader.Close();
            return ourList;
        }
        public void insertNewLesson(int lectID, string aliasLectID, int discID, string aliasDiscID, int ringID, string aliasRingID, int groupID, string aliasGroupID, int roomID, string aliasRoomID, int dayID, string aliasDayID, string query, MySqlConnection con)
        {
            var command = new MySqlCommand(query, con);
            command.Parameters.AddWithValue(aliasDiscID, discID);
            command.Parameters.AddWithValue(aliasLectID, lectID);
            command.Parameters.AddWithValue(aliasRingID, ringID);
            command.Parameters.AddWithValue(aliasGroupID, groupID);
            command.Parameters.AddWithValue(aliasRoomID, roomID);
            command.Parameters.AddWithValue(aliasDayID, dayID);
           /* try
            {
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            showDataBase();*/
        }
        
        public string[] getArraySomething(string query, MySqlConnection con)
        {
            
            MySqlCommand command = new MySqlCommand(query, con);
            MySqlDataReader reader = command.ExecuteReader();
            List<string> firstList = new List<string>();
            List<string> secondList = new List<string>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    firstList.Add(reader.GetString(0));
                    secondList.Add(reader.GetString(1));
                }
            }
            string[] helpArray1 = new string[firstList.Count];
            string[] helpArray2 = new string[secondList.Count];
            string[] outputArray = new string[helpArray1.Length];
            reader.Close();
            int j = 0;
            foreach (string item in firstList)
            {
                helpArray1[j] = item;
                j++;
            }
            int k = 0;
            foreach (string item in secondList)
            {
                helpArray2[k] = item;
                k++;
            }
            for (int i = 0; i < helpArray1.Length; i++)
            {
                outputArray[i] = helpArray1[i] + " (" + helpArray2[i] + ")";
            }

            return outputArray;
        }

       
    }
}
