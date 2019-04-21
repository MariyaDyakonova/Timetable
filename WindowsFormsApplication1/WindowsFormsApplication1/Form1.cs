using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using WindowsFormsApplication1;


namespace TimeTableProject
{
    public partial class Form1 : Form
    {
        private MySqlConnection connect;
        private MySqlDataAdapter commonDataAdapter; //заполняет DataSet данными из БД
        static int changedRow;
        private int myIndex;

        Applic app = new Applic();
  
        private DataSet commonDataSet, disciplineDataSet;  //хранилище данных

        //DataGridView[] dgvWeekArray;
        public Form1()
        {
            InitializeComponent();
            changedRow = -1;
            myIndex = -1;

            try
            {
                connect = Connection.connection;
                connect.Open();
                MySqlCommand com = new MySqlCommand(Queries.insertDiscipline, connect);//это чтобы запускалась прога
            }
            catch (InvalidCastException f)
            {
                MessageBox.Show(f.Message);
            }

            connect.Clone();
         
        }

        #region Преподаватель (Lecturer) 

        private DataSet searchSomeDataForOneLecturer(string query, MySqlConnection con, int lectID, string alias) 
        {
            MySqlDataReader reader;
            DataSet ds = new DataSet();
            DataTable table = new DataTable();
            MySqlCommand command = new MySqlCommand(query, con);  // экземпляр класса MySqlCommand с текстом запроса и MySqlConnection
            command.Parameters.AddWithValue(alias, lectID);       //преобразует string в int lectID
            reader = command.ExecuteReader();     //Отправляет CommandText в соединение и создает MySqlDataReader
            table.Load(reader);
            ds.Merge(table);
            return ds;
        }



        private void dGV_lecturer_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                changedRow = dGV_Lecturer.SelectedCells[0].RowIndex;
                myIndex = Convert.ToInt32(dGV_Lecturer.Rows[changedRow].Cells[0].Value);
                label_helpLectID.Text = myIndex.ToString();      //индекс препода выбранной ячейки
                tB_lectSurname.Text = dGV_Lecturer.Rows[changedRow].Cells[1].Value.ToString();
                tb_lectName.Text = dGV_Lecturer.Rows[changedRow].Cells[2].Value.ToString();
                tB_lectPatro.Text = dGV_Lecturer.Rows[changedRow].Cells[3].Value.ToString();
                disciplineDataSet = searchSomeDataForOneLecturer(Queries.selectDisciplineForOneLecturer, connect, myIndex, "@discLectID");
                dGV_DiscForLecturer.DataSource = disciplineDataSet.Tables[0];
                dGV_DiscForLecturer.Columns[0].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void button_addDiscForLect_Click(object sender, EventArgs e)
        {

        }
        private void button_addLect_Click(object sender, EventArgs e)
        {
            try
            {
                if (tb_lectName.Text != "" && tB_lectSurname.Text != "" && tB_lectPatro.Text != "")
                {
                    try
                    { 
                    app.getSqlWithAliasFor3Object(tb_lectName.Text, "@lectName", tB_lectSurname.Text, "@lectSurname", tB_lectPatro.Text, "@lectPatro", Queries.insertLecturer, connect);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    dGV_Lecturer.DataSource = app.fillDataTable(Queries.selectLecturer, connect, ref commonDataAdapter, ref commonDataSet);
                    dGV_Lecturer.Columns[0].Width = 30;

                    tb_lectName.Clear();
                    tB_lectSurname.Clear();
                    tB_lectPatro.Clear();
                }
                else
                {
                    MessageBox.Show("Заполните поля!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button_addGroup_Click(object sender, EventArgs e)
        {
            try
            {

                if (tB_groupName.Text != "")
                {
                    try
                    {
                        app.getSqlWithAliasFor1Object(tB_groupName.Text, "@groupName", Queries.insertGroup, connect);
                        //метод показывает бд
                    }

                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }


                    tB_groupName.Clear();

                }
                else
                {
                    MessageBox.Show("Заполните поле!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void button_updateGroup_Click(object sender, EventArgs e)
        {
            
        }

        private void dGV_Lecturer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
           
        }
    }

    #endregion

    #region Общие методы (common methods)
   
        


    #endregion

}

