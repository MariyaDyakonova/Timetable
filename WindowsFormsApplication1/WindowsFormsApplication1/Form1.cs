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
        static int changedRow, changedColumn, changedDiscipline;
        DataGridView[] dgvWeekArray;
        private int myIndex, ringIndex, discIndex;

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
            showDataBase();
            dgvWeekArray = new DataGridView[] { dGV_tabMonday, dGV_tabTuesday, dGV_tabWednesday, dGV_tabThursday, dGV_tabFriday, dGV_tabSaturday };
            fillDGVnull();
            fillTimetableDataGrid(dgvWeekArray);
        }
        private void fillDGVnull()
        {
            for (int m = 0; m < dgvWeekArray.Length; m++)
            {
                for (int i = 0; i < dgvWeekArray[m].RowCount; i++)
                {
                    for (int j = 0; j < dgvWeekArray[m].ColumnCount; j++)
                    {
                        dgvWeekArray[m].Rows[i].Cells[j].Value = "";
                    }
                }
            }
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
            try
            {
                if (tB_groupName.Text != "")
                {
                    try
                    {
                        tB_groupName.Clear();
                        // app.getSqlWithAliasFor2Object(tB_groupName.Text, "@groupName", myIndex, "@groupID", Queries.updateGroup, connect);

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                    tB_groupName.Clear();

                }
                else
                {
                    MessageBox.Show("Выберите группу для редактирования");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dGV_Lecturer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        #endregion

        #region Дисциплины для конкретного преподавателя
        private void button_addDiscForLect_Click(object sender, EventArgs e)
        {
            tb_lectName.Clear();
            tB_lectSurname.Clear();
            tB_lectPatro.Clear();
        }

        private void dGV_DiscForLecturer_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                changedDiscipline = dGV_DiscForLecturer.SelectedCells[0].RowIndex;
                discIndex = Convert.ToInt32(dGV_DiscForLecturer.Rows[changedDiscipline].Cells[0].Value);
                label_helpDiscID.Text = discIndex.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button_updDiscForLect_Click(object sender, EventArgs e)
        {
            tb_lectName.Clear();
            tB_lectSurname.Clear();
            tB_lectPatro.Clear();
        }

        private void button_delDiscForLect_Click(object sender, EventArgs e)
        {

            tb_lectName.Clear();
            tB_lectSurname.Clear();
            tB_lectPatro.Clear();
        }
        #endregion


        #region Аудитория (Room)
        private void button_addRoom_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox_roomType.Text != "" && tB_roomNumber.Text != "")
                {
                    tB_roomNumber.Clear();
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

        private void button_updateRoom_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox_roomType.Text != "" && tB_roomNumber.Text != "")
                {
                    tB_roomNumber.Clear();
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

        private void button_deleteRoom_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox_roomType.Text != "" && tB_roomNumber.Text != "")
                {
                    tB_roomNumber.Clear();
                    showDataBase();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        
        private void dGV_Rooms_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                changedRow = dGV_Rooms.SelectedCells[0].RowIndex;
                myIndex = Convert.ToInt32(dGV_Rooms.Rows[changedRow].Cells[0].Value);
                tB_roomNumber.Text = dGV_Rooms.Rows[changedRow].Cells[1].Value.ToString();
                comboBox_roomType.Text = dGV_Rooms.Rows[changedRow].Cells[2].Value.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion


        #region Общие методы (common methods)
        private void showDataBase()
        {
            dGV_Rings.DataSource = app.fillDataTable(Queries.selectRings, connect, ref commonDataAdapter, ref commonDataSet);
            dGV_Lecturer.DataSource = app.fillDataTable(Queries.selectLecturer, connect, ref commonDataAdapter, ref commonDataSet);
            dGV_Lecturer.Columns[0].Width = 30;
            dGV_Rooms.DataSource = app.fillDataTable(Queries.selectRoomWithType, connect, ref commonDataAdapter, ref commonDataSet);
            dGV_Rooms.Columns[0].Visible = false;
            dGV_Discipline.DataSource = app.fillDataTable(Queries.selectDisciplineWithType, connect, ref commonDataAdapter, ref commonDataSet);
            dGV_Discipline.Columns[0].Visible = false;
            dGV_groups.DataSource = app.fillDataTable(Queries.selectGroupNameWithAlias, connect, ref commonDataAdapter, ref commonDataSet);
            dGV_groups.Columns[0].Visible = false;
        }

        private void fillTimetableDataGrid(DataGridView[] dgvArray)
        {
            for (int i = 0; i < dgvArray.Length; i++)
            {
                drawDataGrid(dgvArray[i]);
            }
        }
        private void drawDataGrid(DataGridView dgv)
        {
            dgv.RowCount = app.getScalarCount(Queries.selectCountRings, connect);
            dgv.ColumnCount = app.getScalarCount(Queries.selectCountGroups, connect);
            string[] helpArray = new string[dgv.ColumnCount];
            List<string> groupNameForHeaders = app.getGroupList(Queries.selectGroupName, connect);
            for (int i = 0; i < dgv.RowCount; i++)
            {
                dgv.Rows[i].HeaderCell.Value = (i + 1).ToString();
            }
            int j = 0;
            foreach (string gName in groupNameForHeaders)
            {
                helpArray[j] = gName;
                j++;
            }
            for (int i = 0; i < dgv.ColumnCount; i++)
            {
                dgv.Columns[i].HeaderCell.Value = helpArray[i];
            }
        }
        #endregion
    }
}

