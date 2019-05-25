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
        List<int> ringIDList, groupIDList, dayIDList;
        string[] discArray, roomArray, lecturerArray;

        Applic app = new Applic();

        private DataSet commonDataSet, disciplineDataSet;  //хранилище данных

        //DataGridView[] dgvWeekArray;
        public Form1()
        {
            InitializeComponent();
            changedRow = -1;
            myIndex = -1;
            changedColumn = -1;
            ringIndex = -1;
            dGV_help2.Visible = false;
            dGV_help2.Visible = false;

            try
            {
                connect = Connection.connection;
                connect.Open();
               // MySqlCommand com = new MySqlCommand(Queries.insertDiscipline, connect);//это чтобы запускалась прога
            }
            catch (InvalidCastException f)
            {
                MessageBox.Show(f.Message);
            }

           // connect.Clone();
            showDataBase();
            dgvWeekArray = new DataGridView[] { dGV_tabMonday, dGV_tabTuesday, dGV_tabWednesday, dGV_tabThursday, dGV_tabFriday, dGV_tabSaturday };
            fillDGVnull();
            fillTimetableDataGrid(dgvWeekArray);
            RefreshComboBox(Queries.selectTypeOfRoomName, comboBox_roomType, connect);
            RefreshComboBox(Queries.selectTypeOfDisciplineName, comboBox_typeOfDisc, connect);
            lecturerArray = app.getArraySomething(Queries.selectLecturerSurnameAndName,  connect);
            discArray = app.getArraySomething(Queries.selectDisciplineForList, connect);
            roomArray = app.getArraySomething(Queries.selectRoomForList, connect);
            refreshArrayComboBox(lecturerArray, comboBox_Lecturer);
            refreshArrayComboBox(discArray, comboBox_testDiscipline);
            refreshArrayComboBox(discArray, comboBox_discipline);
            refreshArrayComboBox(roomArray, comboBox_room);
            showTimeTable();
        }

         private void refreshArrayComboBox(string[] inputArray, ComboBox cBox)
        {
            //comboBox_testDiscipline.DropDownStyle = ComboBoxStyle.DropDownList;
            cBox.DropDownStyle = ComboBoxStyle.DropDownList;
            try
            {
                
                cBox.Items.Clear();
                for (int i = 0; i < inputArray.Length; i++)
                {
                    cBox.Items.Add(inputArray[i]);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void RefreshComboBox(string query, ComboBox cBox, MySqlConnection con)
        {
            cBox.DropDownStyle = ComboBoxStyle.DropDownList;
            try
            {
                cBox.Items.Clear();
                commonDataAdapter = new MySqlDataAdapter(query, con);
                commonDataSet = new DataSet();
                commonDataAdapter.Fill(commonDataSet);
                DataTable table = commonDataSet.Tables[0];
                foreach (DataRow row in table.Rows)
                {
                    cBox.Items.Add(row[0]);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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
        private void getIndexesForLesson()  /*Queries как в  app*/
        {
            MySqlCommand command = new MySqlCommand(Queries.selectIndexForTimetable, connect);
            MySqlDataReader reader = command.ExecuteReader();
            ringIDList = new List<int>();
            groupIDList = new List<int>();
            dayIDList = new List<int>();
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    ringIDList.Add(Convert.ToInt32(reader.GetString(0)));
                    groupIDList.Add(Convert.ToInt32(reader.GetString(1)));
                    dayIDList.Add(Convert.ToInt32(reader.GetString(2)));
                }
            }
            reader.Close();

        }
        #region Lecturer

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

        private void button_updateLect_Click(object sender, EventArgs e)
        {
            try
            {
                if (tb_lectName.Text != "" && tB_lectSurname.Text != "" && tB_lectPatro.Text != "")
                {
                    try
                    {
                        app.getSqlWithAliasFor4Object(tb_lectName.Text, "@lectName", tB_lectSurname.Text, "@lectSurname", tB_lectPatro.Text, "@lectPatro", myIndex, "@lectID", Queries.updateLecturer, connect);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    showDataBase();
                    tb_lectName.Clear();
                    tB_lectSurname.Clear();
                    tB_lectPatro.Clear();
                }
                else
                {
                    MessageBox.Show("Выберите преподавателя!");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button_deleteLect_Click(object sender, EventArgs e)
        {
            try
            {
                if (tb_lectName.Text != "" && tB_lectSurname.Text != "" && tB_lectPatro.Text != "")
                {
                    app.getSqlWithAliasFor1Object(myIndex, "@lectID", Queries.deleteLecturer, connect);
                    tb_lectName.Clear();
                    tB_lectSurname.Clear();
                    tB_lectPatro.Clear();
                    showDataBase();
                    myIndex = -1;
                }
                else
                {
                    MessageBox.Show("Выберите преподавателя!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region Group 
        private void button_addGroup_Click(object sender, EventArgs e)
        {
            try
            {
                if (tB_groupName.Text != "")
                {
                    try
                    {
                        app.getSqlWithAliasFor1Object(tB_groupName.Text, "@groupName", Queries.insertGroup, connect);
                        showDataBase();
                        
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
                       // tB_groupName.Clear();
                        app.getSqlWithAliasFor2Object(tB_groupName.Text, "@groupName", myIndex, "@groupID", Queries.updateGroup, connect);
                        tB_groupName.Clear();
                         myIndex = -1;
                        fillTimetableDataGrid(dgvWeekArray);
                         showDataBase();
                        // app.getSqlWithAliasFor2Object(tB_groupName.Text, "@groupName", myIndex, "@groupID", Queries.updateGroup, connect);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    //tB_groupName.Clear();           
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
        private void button_deleteGroup_Click(object sender, EventArgs e)
        {
            try
            {
                if (tB_groupName.Text != "")
                {
                    try
                    {
                        app.getSqlWithAliasFor1Object(myIndex, "@groupID", Queries.deleteGroup, connect);
                        tB_groupName.Clear();
                        showDataBase();
                        fillTimetableDataGrid(dgvWeekArray);
                        myIndex = -1;
                       // app.getSqlWithAliasFor1Object(tB_groupName.Text, "@groupName", Queries.insertGroup, connect);
                        //showDataBase();
                        //метод показывает бд
                    }

                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                  //  tB_groupName.Clear();
                }
                else
                {
                    MessageBox.Show("Выберите группу для удаления!");
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
            try
            {
                if (comboBox_testDiscipline.Text != "")
                {
                    string discNameWithType = comboBox_testDiscipline.Text;
                    string[] split = discNameWithType.Split('(');
                    string disciplineName = split[0].Trim();
                    string sqlForSearchDisciplineByName = "select discipline_ID from facultytimetable.discipline where Discipline_name like '" + disciplineName + "'";
                    int disciplineID = app.getScalarCount(sqlForSearchDisciplineByName, connect);
                    changedRow = dGV_Lecturer.SelectedCells[0].RowIndex;
                    int lecturerID = Convert.ToInt32(dGV_Lecturer.Rows[changedRow].Cells[0].Value);
                    app.getSqlWithAliasFor2Object(lecturerID, "@lectID", disciplineID, "@discID", Queries.insertOneDisciplineForLecturer, connect);
                    disciplineDataSet = searchSomeDataForOneLecturer(Queries.selectDisciplineForOneLecturer, connect, lecturerID, "@discLectID");
                    dGV_DiscForLecturer.DataSource = disciplineDataSet.Tables[0];
                }
                else
                {
                    MessageBox.Show("Выберите предмет!");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
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


        private void button_delDiscForLect_Click(object sender, EventArgs e)
        {

            try
            {
                if (label_helpDiscID.Text != "" && label_helpLectID.Text != "")
                {
                    changedRow = dGV_Lecturer.SelectedCells[0].RowIndex;
                    int lecturerID = Convert.ToInt32(dGV_Lecturer.Rows[changedRow].Cells[0].Value);
                    app.getSqlWithAliasFor2Object(lecturerID, "@lectID", discIndex, "@discID", Queries.deleteOneDisciplineForLecturer, connect);
                    disciplineDataSet = searchSomeDataForOneLecturer(Queries.selectDisciplineForOneLecturer, connect, lecturerID, "@discLectID");
                    dGV_DiscForLecturer.DataSource = disciplineDataSet.Tables[0];
                }
                else
                {
                    MessageBox.Show("Выберите предмет!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region Аудитория (Room)
        private void button_addRoom_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox_roomType.Text != "" && tB_roomNumber.Text != "")
                {
                    string roomNumb = tB_roomNumber.Text;
                    string roomType = comboBox_roomType.Text;
                    string sqlForRoomType = "SELECT Type_ID FROM facultytimetable.typeOfRoom WHERE Type_name LIKE '" + roomType + "';";
                    dGV_help1.DataSource = app.fillDataTable(sqlForRoomType, connect, ref commonDataAdapter, ref commonDataSet);
                    int roomTypeIndex = Convert.ToInt32(dGV_help1.Rows[0].Cells[0].Value);
                    app.getSqlWithAliasFor2Object(roomNumb, "@roomNumber", roomTypeIndex, "@typeRoomID", Queries.insertRoom, connect);
                    tB_roomNumber.Clear();
                    showDataBase();
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
                    string roomNumb = tB_roomNumber.Text;
                    string roomType = comboBox_roomType.Text;
                    string sqlForRoomType = "SELECT Type_ID FROM facultytimetable.typeOfRoom WHERE Type_name LIKE '" + roomType + "';";
                    dGV_help1.DataSource = app.fillDataTable(sqlForRoomType, connect, ref commonDataAdapter, ref commonDataSet);
                    int roomTypeIndex = Convert.ToInt32(dGV_help1.Rows[0].Cells[0].Value);
                    app.getSqlWithAliasFor3Object(roomNumb, "@roomNumber", roomTypeIndex, "@typeRoomID", myIndex, "@roomID", Queries.updateRoom, connect);
                    myIndex = -1;
                    tB_roomNumber.Clear();
                    showDataBase();
                }
                else
                {
                    MessageBox.Show("Выберите аудиторию!");
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
                    app.getSqlWithAliasFor1Object(myIndex, "@roomID", Queries.deleteRoom, connect);
                    tB_roomNumber.Clear();
                    showDataBase();
                    myIndex = -1;
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
        #region Дисциплины
        private void button_addDisc_Click(object sender, EventArgs e)
        {
            showDataBase();
            try
            {

                if (comboBox_typeOfDisc.Text != "" && tB_discName.Text != "")
                {
                    string discName = tB_discName.Text;
                    string discType = comboBox_typeOfDisc.Text;
                    string sqlForDiscType = "SELECT Type_ID FROM facultytimetable.typeOfDiscipline WHERE Type_name LIKE '" + discType + "';";
                    dGV_help2.DataSource = app.fillDataTable(sqlForDiscType, connect, ref commonDataAdapter, ref commonDataSet);
                    int discTypeIndex = Convert.ToInt32(dGV_help2.Rows[0].Cells[0].Value);
                    app.getSqlWithAliasFor2Object(discName, "@disciplineName", discTypeIndex, "@typeDiscID", Queries.insertDiscipline, connect);
                    tB_discName.Clear();
                    showDataBase();
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

        private void Ring_Click(object sender, EventArgs e)
        {
            /*DateTime t = new DateTime(2019, 5, 10, 9, 35, 0);
            string[,] a = new string[7, 2];
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 1; j++)
                {

                    a[i, j] = (i + 1).ToString();
                }

            }
            for (int i = 0; i < 1; i++)
            {

                int m = t.Hour + t.Minute;

                a[i, 1] = m.ToString();
            }

            dgv.RowCount = 7;
            dgv.ColumnCount = 2;
            for (int i = 0; i < 6; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    dgv.Rows[i].Cells[j].Value = a[i, 0].ToString();
                }
            }*/
        }

        private void button_deleteDisc_Click(object sender, EventArgs e)
        {

            try
            {
                if (comboBox_typeOfDisc.Text != "" && tB_discName.Text != "")
                {
                    app.getSqlWithAliasFor1Object(myIndex, "@discID", Queries.deleteDiscipline, connect);
                    tB_discName.Clear();
                    showDataBase();
                    myIndex = -1;

                }
                else
                {
                    MessageBox.Show("Выберите дисциплину для удаления!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void dGV_Discipline_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                changedRow = dGV_Discipline.SelectedCells[0].RowIndex;
                myIndex = Convert.ToInt32(dGV_Discipline.Rows[changedRow].Cells[0].Value);
                tB_discName.Text = dGV_Discipline.Rows[changedRow].Cells[1].Value.ToString();
                comboBox_typeOfDisc.Text = dGV_Discipline.Rows[changedRow].Cells[2].Value.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }



        private void button_updateDisc_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox_typeOfDisc.Text != "" && tB_discName.Text != "")
                {
                    string discName = tB_discName.Text;
                    string discType = comboBox_typeOfDisc.Text;
                    string sqlForDiscType = "SELECT Type_ID FROM facultytimetable.typeofdiscipline WHERE Type_name LIKE '" + discType + "';";
                    dGV_help2.DataSource = app.fillDataTable(sqlForDiscType, connect, ref commonDataAdapter, ref commonDataSet);
                    int discTypeIndex = Convert.ToInt32(dGV_help2.Rows[0].Cells[0].Value);
                    app.getSqlWithAliasFor3Object(discName, "@disciplineName", discTypeIndex, "@typeDiscID", myIndex, "@discID", Queries.updateDiscipline, connect);
                    tB_discName.Clear();
                    myIndex = -1;
                    showDataBase();
                }
                else
                {
                    MessageBox.Show("Выберите дисциплину для изменения!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button_excel_Click(object sender, EventArgs e)
        {
            Microsoft.Office.Interop.Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();
            ExcelApp.Application.Workbooks.Add(Type.Missing);
           // ExcelApp.Columns.ColumnWidth = 20;
            ExcelApp.Columns[1].AutoFit();
            ExcelApp.Columns[2].AutoFit();
            ExcelApp.Columns[3].AutoFit();

            ExcelApp.Cells[1, 1] = " ID дисциплины";
            ExcelApp.Cells[1, 2] = "Дисциплина";
            ExcelApp.Cells[1, 3] = "Тип";


            for (int i = 0; i < dGV_Discipline.ColumnCount; i++)
            {
                for (int j = 0; j < dGV_Discipline.RowCount; j++)
                {
                    ExcelApp.Cells[j + 2, i + 1] = (dGV_Discipline[i, j].Value).ToString();
                }
            }
            ExcelApp.Visible = true;
        }
        #endregion
        private void button_addLesson_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox_discipline.Text != "" && comboBox_room.Text != "" && comboBox_Lecturer.Text != "" && label_selectedGroup.Text != "" && label_selectedRing.Text != "")
                {

                    int changedDay = tabControl_Days.SelectedIndex + 1;


                    changedColumn = dgvWeekArray[changedDay - 1].SelectedCells[0].ColumnIndex;
                    string groupName = (dgvWeekArray[changedDay - 1].Columns[changedColumn].HeaderCell.Value).ToString();
                    string sqlForSearchGroupByName = "select group_ID from facultytimetable.groups where group_name like '" + groupName + "'";
                    MySqlCommand command = new MySqlCommand(sqlForSearchGroupByName, connect);
                    object groupIndex = command.ExecuteScalar();//для извлечения данных из базы данных 
                    int groupID = Convert.ToInt32(groupIndex);

                    string roomNameWithType = comboBox_room.Text;
                    string[] split = roomNameWithType.Split('(');
                    string roomsName = split[0].Trim();
                    string sqlForSearchRoomByName = "select room_ID from facultytimetable.room where Room_number like '" + roomsName + "'";
                    command = new MySqlCommand(sqlForSearchRoomByName, connect);
                    object roomIndex = command.ExecuteScalar();
                    int roomID = Convert.ToInt32(roomIndex);

                    string discNameWithType = comboBox_discipline.Text;
                    split = discNameWithType.Split('(');
                    string disciplineName = split[0].Trim();
                    string sqlForSearchDisciplineByName = "select discipline_ID from facultytimetable.discipline where Discipline_name like '" + disciplineName + "'";
                    command = new MySqlCommand(sqlForSearchDisciplineByName, connect);
                    object disciplineIndex = command.ExecuteScalar();
                    int discID = Convert.ToInt32(disciplineIndex);


                    string lectSurnameWithName = comboBox_Lecturer.Text;
                    split = lectSurnameWithName.Split('(');
                    string lecturerSurname = split[0].Trim();
                    string sqlForSearchLecturerBySurname = "select lecturer_ID from facultytimetable.lecturer where lecturer_surname like '" + lecturerSurname + "'";
                    command = new MySqlCommand(sqlForSearchLecturerBySurname, connect);
                    object lectIndex = command.ExecuteScalar();
                    int lectID = Convert.ToInt32(lectIndex);


                    changedRow = dgvWeekArray[changedDay - 1].SelectedCells[0].RowIndex;
                    ringIndex = Convert.ToInt32(dgvWeekArray[changedDay - 1].Rows[changedRow].HeaderCell.Value);

                    insertNewLesson(lectID, "@lectID", discID, "@discID", ringIndex, "@ringID", groupID, "@groupID", roomID, "@roomID", changedDay, "@dayID", Queries.insertLesson, connect);
                    showTimeTable();
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
             /*       try
                    {
                        command.ExecuteNonQuery();  // возвращает количество задействованных в инструкции строк.
                        app.insertNewLesson(lectID, "@lectID", discID, "@discID", ringIndex, "@ringID", groupID, "@groupID", roomID, "@roomID", changedDay, "@dayID", Queries.insertLesson, connect);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    showDataBase();
                    
                    showTimeTable();
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
        }*/
        public void insertNewLesson(int lectID, string aliasLectID, int discID, string aliasDiscID, int ringID, string aliasRingID, int groupID, string aliasGroupID, int roomID, string aliasRoomID, int dayID, string aliasDayID, string query, MySqlConnection con)
        {
            var command = new MySqlCommand(query, con);
            command.Parameters.AddWithValue(aliasDiscID, discID);
            command.Parameters.AddWithValue(aliasLectID, lectID);
            command.Parameters.AddWithValue(aliasRingID, ringID);
            command.Parameters.AddWithValue(aliasGroupID, groupID);
            command.Parameters.AddWithValue(aliasRoomID, roomID);
            command.Parameters.AddWithValue(aliasDayID, dayID);
             try
             {
                 command.ExecuteNonQuery();
             }
             catch (Exception ex)
             {
                 MessageBox.Show(ex.Message);
             }
             showDataBase();
        }
        private void button_deleteLesson_Click(object sender, EventArgs e)
        {
            try
            {
                if (label_selectedGroup.Text != "" && label_selectedRing.Text != "")
                {
                    int changedDay = tabControl_Days.SelectedIndex + 1;

                    changedRow = dgvWeekArray[changedDay - 1].SelectedCells[0].RowIndex;
                    ringIndex = Convert.ToInt32(dgvWeekArray[changedDay - 1].Rows[changedRow].HeaderCell.Value);

                    changedColumn = dgvWeekArray[changedDay - 1].SelectedCells[0].ColumnIndex;
                    string groupName = (dgvWeekArray[changedDay - 1].Columns[changedColumn].HeaderCell.Value).ToString();
                    string sqlForSearchGroupByName = "select group_ID from facultytimetable.groups where group_name like '" + groupName + "'";
                    int groupID = app.getScalarCount(sqlForSearchGroupByName, connect);
                    app.getSqlWithAliasFor3Object(ringIndex, "@ringID", groupID, "@groupID", changedDay, "@dayID", Queries.deleteLesson, connect);

                    dgvWeekArray[changedDay - 1].Rows[ringIndex - 1].Cells[groupID - 1].Value = "";
                    showTimeTable();
                }
                else
                {
                    MessageBox.Show("Заполните группу и номер урока!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void showTimeTable()
        {
            getIndexesForLesson();
            string[] lessonsArray = getArrayLessons(ringIDList, groupIDList, dayIDList);
            fillTable(lessonsArray, ringIDList, groupIDList, dayIDList);
        }

        private void dGV_tabMonday_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            clickOnTimeiableDGV(dGV_tabMonday);
        }

        private void dGV_tabWednesday_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            clickOnTimeiableDGV(dGV_tabWednesday);
        }

        private void dGV_tabTuesday_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            clickOnTimeiableDGV(dGV_tabTuesday);
        }

        private void dGV_tabThursday_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            clickOnTimeiableDGV(dGV_tabThursday);
        }
        private void dGV_tabFriday_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            clickOnTimeiableDGV(dGV_tabFriday);
        }

        private void button_excel2_Click(object sender, EventArgs e)
        {
           
        }

        private void dGV_tabSaturday_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            clickOnTimeiableDGV(dGV_tabSaturday);
        }

        private void dGV_groups_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                changedRow = dGV_groups.SelectedCells[0].RowIndex;
                myIndex = Convert.ToInt32(dGV_groups.Rows[changedRow].Cells[0].Value);
                tB_groupName.Text = dGV_groups.Rows[changedRow].Cells[1].Value.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

       

        private void clickOnTimeiableDGV(DataGridView inputDGV)
        {
            try
            {
                changedRow = inputDGV.SelectedCells[0].RowIndex;
                changedColumn = inputDGV.SelectedCells[0].ColumnIndex;
                string groupName = (inputDGV.Columns[changedColumn].HeaderCell.Value).ToString();
                ringIndex = Convert.ToInt32(inputDGV.Rows[changedRow].HeaderCell.Value);
                label_selectedGroup.Text = groupName;
                label_selectedRing.Text = ringIndex.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

     
        private void fillTable(string[] lessons, List<int> r, List<int> g, List<int> d) //dgv в app здесь получаем исключание!!!!!!!!!
        {
            for (int i = 0; i < lessons.Length; i++)
            {
                dgvWeekArray[d[i] - 1].Rows[r[i] - 1].Cells[g[i] - 1].Value = lessons[i];
            }
        }
        private string[] getArrayLessons(List<int> rings, List<int> groups, List<int> days)
        {
            List<string> lectSurnameList = new List<string>();
            List<string> roomNumbList = new List<string>();
            List<string> discNameList = new List<string>();
            MySqlCommand command;
            MySqlDataReader reader;

            for (int i = 0; i < groups.Count; i++)
            {
                string selectDataForTimetable = "select facultytimetable.Discipline.Discipline_name ,facultytimetable.Room.Room_number, facultytimetable.Lecturer.Lecturer_Surname from facultytimetable.Lesson join facultytimetable.Discipline on facultytimetable.Discipline.Discipline_ID = facultytimetable.Lesson.Discipline_ID join facultytimetable.Lecturer on facultytimetable.Lesson.Lecturer_ID = facultytimetable.Lecturer.Lecturer_ID join facultytimetable.Room on facultytimetable.Lesson.Room_ID= Room.Room_ID where facultytimetable.Lesson.Ring_ID = '" + rings[i] + "' and facultytimetable.Lesson.Group_ID = '" + groups[i] + "' and facultytimetable.Lesson.Day_ID = '" + days[i] + "';";
                command = new MySqlCommand(selectDataForTimetable, connect);
                reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        discNameList.Add(reader.GetString(0));
                        roomNumbList.Add(reader.GetString(1));
                        lectSurnameList.Add(reader.GetString(2));
                    }
                }
                reader.Close();
            }
            string[] helpArray1 = new string[discNameList.Count];
            string[] helpArray2 = new string[roomNumbList.Count];
            string[] helpArray3 = new string[lectSurnameList.Count];
            string[] outputArray = new string[helpArray1.Length];

            int j = 0;
            foreach (string item in discNameList)
            {
                helpArray1[j] = item;
                j++;
            }
            int k = 0;
            foreach (string item in roomNumbList)
            {
                helpArray2[k] = item;
                k++;
            }
            int m = 0;
            foreach (string item in lectSurnameList)
            {
                helpArray3[m] = item;
                m++;
            }
            for (int i = 0; i < helpArray1.Length; i++)
            {
                outputArray[i] = helpArray1[i] + " \n " + helpArray2[i] + " \n " + helpArray3[i];
            }

            return outputArray;
        }

        #region Общие методы (common methods)

        private void showDataBase()
        {
            //dGV_Rings.DataSource = app.fillDataTable(Queries.selectRings, connect, ref commonDataAdapter, ref commonDataSet);
            dGV_Lecturer.DataSource = app.fillDataTable(Queries.selectLecturer, connect, ref commonDataAdapter, ref commonDataSet); //которого объект DataGridView отображает данные.
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

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        #endregion
    }
}
