using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Models;

namespace Client
{
    public partial class MainForm : Form
    {
        static int changedRow, changedColumn, changedDiscipline;
        readonly DataGridView[] dgvWeekArray;
        private int myIndex, ringIndex, discIndex;
      //  private List<Lesson> lessons;
        string[] discArray, roomArray, lecturerArray;

        private readonly Applic app = new Applic();
        
        public MainForm()
        {
            
            InitializeComponent();
            changedRow = -1;
            myIndex = -1;
            changedColumn = -1;
            ringIndex = -1;
            dGV_help2.Visible = false;
            dGV_help2.Visible = false;
            
            showDataBase();

            dgvWeekArray = new[] { dGV_tabMonday, dGV_tabTuesday, dGV_tabWednesday, dGV_tabThursday, dGV_tabFriday, dGV_tabSaturday };
            //commonDataAdapter.Update(commonDataSet);
            fillDGVnull();
            fillTimetableDataGrid(dgvWeekArray);

            refresh("typeofroom");
            refresh("typeofdiscipline");
            refresh("discipline");
            refresh("room");
            refresh("lecturer");
           
            showTimeTable();
           
        }

        private void refresh(string refrashable)
        {
            switch (refrashable)
            {
                case "typeofroom":
                    RefreshComboBox(comboBox_roomType, refrashable);
                    break;
                case "typeofdiscipline":
                    RefreshComboBox(comboBox_typeOfDisc, refrashable);
                    break;
                case "discipline":
                    discArray = app.GetDisciplinesWithTypes();
                    refreshArrayComboBox(discArray, comboBox_testDiscipline);
                    refreshArrayComboBox(discArray, comboBox_testDiscipline1);
                    refreshArrayComboBox(discArray, comboBox_discipline);
                    break;
                case "room":
                    roomArray = app.GetRoomsWithTypes();
                    refreshArrayComboBox(roomArray, comboBox_room);
                    break;
                case "lecturer":
                    lecturerArray = app.GetLecturers();
                    refreshArrayComboBox(lecturerArray, comboBox_Lecturer);
                    break;
            }
        }

        private void refreshArrayComboBox(string[] inputArray, ComboBox cBox)
        {
            cBox.DropDownStyle = ComboBoxStyle.DropDownList;
            
            try
            {
                cBox.Items.Clear();
                foreach (var item in inputArray)
                {
                    cBox.Items.Add(item);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void RefreshComboBox(ComboBox cBox, string type)
        {
            cBox.DropDownStyle = ComboBoxStyle.DropDownList;
            try
            {
                cBox.Items.Clear();
                var table = app.GetDataTable(type);
                foreach (DataRow row in table.Rows)
                {
                    cBox.Items.Add(row[1]);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void fillDGVnull()
        {
            foreach (DataGridView dgv in dgvWeekArray)
            {
                foreach (DataGridViewRow row in dgv.Rows)
                {
                    foreach (DataGridViewCell cell in row.Cells)
                    {
                        cell.Value = "";
                    }
                }
            }
        }

       

        #region Lecturer
        
        private void dGV_lecturer_MouseClick(object sender, MouseEventArgs e)
        {
            try
            {
                changedRow = dGV_Lecturer.SelectedCells[0].RowIndex;
                myIndex = Convert.ToInt32(dGV_Lecturer.Rows[changedRow].Cells[0].Value);
                label_helpLectID.Text = myIndex.ToString();
                tB_lectSurname.Text = dGV_Lecturer.Rows[changedRow].Cells[1].Value.ToString();
                tb_lectName.Text = dGV_Lecturer.Rows[changedRow].Cells[2].Value.ToString();
                tB_lectPatro.Text = dGV_Lecturer.Rows[changedRow].Cells[3].Value.ToString();
               // dGV_DiscForLecturer.DataSource = app.GetDisciplinesWithTypesTable(myIndex);
                dGV_DiscForLecturer.Columns[0].Visible = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private async void button_addLect_Click(object sender, EventArgs e)
        {
            try
            {
                if (tb_lectName.Text != "" && tB_lectSurname.Text != "" && tB_lectPatro.Text != "")
                {
                    try
                    {
                        await app.Add(new Lecturer{Lecturer_name = tb_lectName.Text, Lecturer_surname = tB_lectSurname.Text, Lecturer_patronymic = tB_lectPatro.Text}, "lecturer");
                        lecturerArray = app.GetLecturers();
                        refreshArrayComboBox(lecturerArray, comboBox_Lecturer);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }

                    dGV_Lecturer.DataSource = app.GetLecturerDataTable();
                    dGV_Lecturer.Columns[0].Width = 30;
                   // commonDataAdapter.Update(commonDataSet);

                    tb_lectName.Clear();
                    tB_lectSurname.Clear();
                    tB_lectPatro.Clear();
                }
                else
                {
                    MessageBox.Show(@"Заполните поля!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void button_updateLect_Click(object sender, EventArgs e)
        {
            try
            {
                if (tb_lectName.Text != "" && tB_lectSurname.Text != "" && tB_lectPatro.Text != "")
                {
                    try
                    {
                        await app.Update(
                            new Lecturer
                            {
                                Lecturer_name = tb_lectName.Text, Lecturer_surname = tB_lectSurname.Text,
                                Lecturer_patronymic = tB_lectPatro.Text
                            }, "lecturer", myIndex);
                        lecturerArray = app.GetLecturers();
                        refreshArrayComboBox(lecturerArray, comboBox_Lecturer);
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
                    MessageBox.Show(@"Выберите преподавателя!");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void button_deleteLect_Click(object sender, EventArgs e)
        {
            try
            {
                if (tb_lectName.Text != "" && tB_lectSurname.Text != "" && tB_lectPatro.Text != "")
                {
                    await app.Delete("lecturer", myIndex);
                    tb_lectName.Clear();
                    tB_lectSurname.Clear();
                    tB_lectPatro.Clear();
                    showDataBase();
                    myIndex = -1;
                    lecturerArray = app.GetLecturers();
                    refreshArrayComboBox(lecturerArray, comboBox_Lecturer);
                }
                else
                {
                    MessageBox.Show(@"Выберите преподавателя!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void KeyPressSurname(object sender, KeyPressEventArgs e)
        {
            e.Handled = app.CorrectInput(e.KeyChar);
        }

        private void KeyPressName(object sender, KeyPressEventArgs e)
        {
            e.Handled = app.CorrectInput(e.KeyChar);
        }

        private void KeyPressPatronymic(object sender, KeyPressEventArgs e)
        {
            e.Handled = app.CorrectInput(e.KeyChar);
        }
        #endregion

        #region Group 


        

        private void dGV_Lecturer_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        #endregion

        #region Discipline for one lecturer

        private async void button_addDiscForLect_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox_testDiscipline.Text != "")
                {
                    string discNameWithType = comboBox_testDiscipline.Text;
                    string[] split = discNameWithType.Split('(');
                    string disciplineName = split[0].Trim();
                    int disciplineID = app.GetDisciplineByName(disciplineName).Discipline_ID;
                    changedRow = dGV_Lecturer.SelectedCells[0].RowIndex;
                    int lecturerID = Convert.ToInt32(dGV_Lecturer.Rows[changedRow].Cells[0].Value);
                    await app.Add(new DisciplineLecturer{Discipline_ID = disciplineID, Lecturer_ID = lecturerID}, "disciplinelecturer");
                    dGV_DiscForLecturer.DataSource = app.GetLecturerDisciplines(lecturerID);
                }
                else
                {
                    MessageBox.Show(@"Выберите предмет!");
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

        private async void button_delDiscForLect_Click(object sender, EventArgs e)
        {
            try
            {
                if (label_helpDiscID.Text != "" && label_helpLectID.Text != "")
                {
                    changedRow = dGV_Lecturer.SelectedCells[0].RowIndex;
                    var lecturerID = Convert.ToInt32(dGV_Lecturer.Rows[changedRow].Cells[0].Value);
                    await app.DeleteDisciplineLecturer(lecturerID, discIndex);
                    dGV_DiscForLecturer.DataSource = app.GetLecturerDisciplines(lecturerID);

                }
                else
                {
                    MessageBox.Show(@"Выберите предмет!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        #region Room
        private async void button_addRoom_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox_roomType.Text != "" && tB_roomNumber.Text != "")
                {
                    string roomNumb = tB_roomNumber.Text;
                    string roomType = comboBox_roomType.Text;
                    dGV_help1.DataSource = app.GetTypeOfRoomTableByName(roomType);
                    int roomTypeIndex = Convert.ToInt32(dGV_help1.Rows[0].Cells[0].Value);
                    await app.Add(new Room { Room_number = roomNumb, Type_ID = roomTypeIndex}, "room");
                    tB_roomNumber.Clear();
                    showDataBase();
                    roomArray = app.GetRoomsWithTypes();               
                    refreshArrayComboBox(roomArray, comboBox_room);
                }
                else
                {
                    MessageBox.Show(@"Заполните поля!");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void button_updateRoom_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox_roomType.Text != "" && tB_roomNumber.Text != "")
                {
                    string roomNumb = tB_roomNumber.Text;
                    string roomType = comboBox_roomType.Text;
                    dGV_help1.DataSource = app.GetTypeOfRoomTableByName(roomType);
                    int roomTypeIndex = Convert.ToInt32(dGV_help1.Rows[0].Cells[0].Value);
                    await app.Update(new Room{ Room_number = roomNumb, Type_ID = roomTypeIndex}, "room", myIndex);
                    myIndex = -1;
                    tB_roomNumber.Clear();
                    showDataBase();
                    roomArray = app.GetRoomsWithTypes();
                    refreshArrayComboBox(roomArray, comboBox_room);
                }
                else
                {
                    MessageBox.Show(@"Выберите аудиторию!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void button_deleteRoom_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox_roomType.Text != "" && tB_roomNumber.Text != "")
                {
                    await app.Delete("room", myIndex);
                    tB_roomNumber.Clear();
                    showDataBase();
                    myIndex = -1;
                    roomArray = app.GetRoomsWithTypes();
                    refreshArrayComboBox(roomArray, comboBox_room);
                }
                else
                {
                    MessageBox.Show(@"Выберите аудиторию для удаления!");
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

        #region Discipline

        private async void button_addDisc_Click(object sender, EventArgs e)
        {
            showDataBase();
            try
            {

                if (comboBox_typeOfDisc.Text != "" && tB_discName.Text != "")
                {
                    string discName = tB_discName.Text;
                    string discType = comboBox_typeOfDisc.Text;
                    dGV_help2.DataSource = app.GetTypeOfDisciplineTableByName(discType);
                    int discTypeIndex = Convert.ToInt32(dGV_help2.Rows[0].Cells[0].Value);
                    await app.Add(new Discipline { Discipline_name = discName, Type_ID = discTypeIndex }, "discipline");
                    tB_discName.Clear();
                    discArray = app.GetDisciplinesWithTypes();
                    refreshArrayComboBox(discArray, comboBox_discipline);
                    showDataBase();
                }
                else
                {
                    MessageBox.Show(@"Заполните поля!");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private async void button_deleteDisc_Click(object sender, EventArgs e)
        {

            try
            {
                if (comboBox_typeOfDisc.Text != "" && tB_discName.Text != "")
                {
                    await app.Delete("discipline", myIndex);
                    tB_discName.Clear();
                    showDataBase();
                    myIndex = -1;
                    discArray = app.GetDisciplinesWithTypes();
                    refreshArrayComboBox(discArray, comboBox_discipline);

                }
                else
                {
                    MessageBox.Show(@"Выберите дисциплину для удаления!");
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

        private async void button_updateDisc_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox_typeOfDisc.Text != "" && tB_discName.Text != "")
                {
                    string discName = tB_discName.Text;
                    string discType = comboBox_typeOfDisc.Text;
                    dGV_help2.DataSource = app.GetTypeOfDisciplineTableByName(discType);
                    int discTypeIndex = Convert.ToInt32(dGV_help2.Rows[0].Cells[0].Value);
                    await app.Update(new Discipline { Discipline_name = discName, Type_ID = discTypeIndex }, "discipline", myIndex);
                    tB_discName.Clear();
                    myIndex = -1;
                    showDataBase();
                    discArray = app.GetDisciplinesWithTypes();
                    refreshArrayComboBox(discArray, comboBox_discipline);
                }
                else
                {
                    MessageBox.Show(@"Выберите дисциплину для изменения!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button_excel_Click(object sender, EventArgs e)
        {
            var ExcelApp = new Microsoft.Office.Interop.Excel.Application();
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

        private void KeyPressDisciplineName(object sender, KeyPressEventArgs e)
        {
            e.Handled = app.CorrectInput(e.KeyChar);
        }
        #endregion

        private async void button_addLesson_Click(object sender, EventArgs e)
        {
            try
            {
                if (comboBox_discipline.Text != "" && comboBox_room.Text != "" && comboBox_Lecturer.Text != "" && label_selectedGroup.Text != "" && label_selectedRing.Text != "")
                {

                    int changedDay = tabControl_Days.SelectedIndex + 1;
                    changedColumn = dgvWeekArray[changedDay - 1].SelectedCells[0].ColumnIndex;
                    string groupName = (dgvWeekArray[changedDay - 1].Columns[changedColumn].HeaderCell.Value).ToString();
                    int groupID = app.GetGroupByName(groupName).Group_ID;

                    string roomNameWithType = comboBox_room.Text;
                    var roomNumber = roomNameWithType.Split('(')[0].Trim();
                    int roomID = app.GetRoomByNumber(roomNumber).Room_ID;

                    string discNameWithType = comboBox_discipline.Text;
                    var disciplineName = discNameWithType.Split('(')[0].Trim();
                    int discID = app.GetDisciplineByName(disciplineName).Discipline_ID;

                    string lectFullName = comboBox_Lecturer.Text;
                    var lectName = lectFullName.Split(' ');
                    int lectID = app.GetLecturerByName(lectName).Lecturer_ID;

                    changedRow = dgvWeekArray[changedDay - 1].SelectedCells[0].RowIndex;
                    ringIndex = Convert.ToInt32(dgvWeekArray[changedDay - 1].Rows[changedRow].HeaderCell.Value);

                    await app.AddLesson(new Lesson {Day_ID = changedDay, Discipline_ID = discID, Group_ID = groupID, Lecturer_ID = lectID, Room_ID = roomID, Ring_ID = ringIndex});
                    showTimeTable();
                }
                else
                {
                    MessageBox.Show(@"Заполните поля!");
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
        private async void button_deleteLesson_Click(object sender, EventArgs e)
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
                    int groupID = app.GetGroupByName(groupName).Group_ID;
                    await app.Delete("lesson", app.GetLessonByDesc(ringIndex, groupID, changedDay).Lesson_ID);

                    dgvWeekArray[changedDay - 1].Rows[ringIndex - 1].Cells[groupID - 1].Value = "";
                    showTimeTable();
                }
                else
                {
                    MessageBox.Show(@"Заполните группу и номер урока!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void showTimeTable()
        {
            getLessons();
            var lessonsArray = app.GetLessonsStrings(lessons);
            var ringIDList = (from lesson in lessons select lesson.Ring_ID).ToList();
            var groupIDList = (from lesson in lessons select lesson.Group_ID).ToList();
            var dayIDList = (from lesson in lessons select lesson.Day_ID).ToList();
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
            var ExcelApp = new Microsoft.Office.Interop.Excel.Application();
            ExcelApp.Application.Workbooks.Add(Type.Missing);
            ExcelApp.Columns.ColumnWidth = 10;
            
            var columnNames = new List<string>();
            var ringNames = new List<string>();

            foreach (var item in dgvWeekArray)
            {
                foreach (DataGridViewColumn column in item.Columns)
                {
                    columnNames.Add(column.HeaderCell.Value.ToString());
                }
            }
            foreach (DataGridViewRow row in dgvWeekArray[0].Rows)
            {
                ringNames.Add(row.HeaderCell.Value.ToString());
            }

            int k = 0;
            foreach (var item in columnNames)
            {
                ExcelApp.Cells[1, k + 2] = "" + item + "";
                k++;
            }
            k = 0;
            foreach (var item in ringNames)
            {
                ExcelApp.Cells[k + 2, 1] = "" + item + "";
                k++;
            }
            for (var m = 0; m < dgvWeekArray.Length-1; m++)
            {
                for (var i = 0; i < dgvWeekArray[m].RowCount; i++)
                {
                    for (var j = 0; j < dgvWeekArray[m].ColumnCount; j++)
                    {
                        if (dgvWeekArray[m].Rows[i].Cells[j].Value !=null)
                        {
                            ExcelApp.Cells[i + 2, j + 2] = (dgvWeekArray[m].Rows[i].Cells[j].Value).ToString();
                        }
                    }
                }
            }
            ExcelApp.Visible = true;

        }

        private async void button_addDiscForGroup_Click(object sender, EventArgs e)
        {
            
        }



        private async void button_delDiscForGroup_Click(object sender, EventArgs e)
        {
        }

        private void dGV_tabSaturday_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            clickOnTimeiableDGV(dGV_tabSaturday);
        }

        private void dGV_groups_MouseDoubleClick(object sender, MouseEventArgs e)
        {
        
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

        private void fillTable(string[] lessonsArr, List<int> r, List<int> g, List<int> d) 
        {
            for (var i = 0; i < lessonsArr.Length; i++)
            {
                dgvWeekArray[d[i] - 1].Rows[r[i] - 1].Cells[g[i] - 1].Value = lessonsArr[i];
            }
        }
        
        #region Общие методы (common methods)

        private void showDataBase()
        {
            dGV_Lecturer.DataSource = app.GetLecturerDataTable();
            dGV_Lecturer.Columns[0].Width = 30;
            dGV_Rooms.DataSource = app.GetRoomsWithTypesTable();
            dGV_Rooms.Columns[0].Visible = false;
            //dGV_Discipline.DataSource = app.GetDisciplinesWithTypesTable();
            dGV_Discipline.Columns[0].Visible = false;
            //dGV_groups.DataSource = app.GetGroupDataTable();
            dGV_groups.Columns[0].Visible = false;
        }

        private void fillTimetableDataGrid(DataGridView[] dgvArray)
        {
            foreach (var item in dgvArray)
            {
                drawDataGrid(item);
            }
        }
       

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
        #endregion
    }
}
