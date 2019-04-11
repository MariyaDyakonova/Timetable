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


namespace TimeTableProject
{
    public partial class Form1 : Form
    {
        private MySqlDataAdapter commonDataAdapter;
        static int changedRow, changedColumn, changedDiscipline;
        private int myIndex, ringIndex, discIndex;
        string[] discArray, roomArray, lecturerArray;
        DataGridView[] dgvWeekArray;
        List<int> ringIDList, groupIDList, dayIDList;
        public Form1()
        {
            InitializeComponent();
            changedRow = -1;
            myIndex = -1;
            changedColumn = -1;
            ringIndex = -1;
        }

        private void button_addLesson_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void button_excel_Click(object sender, EventArgs e)
        {

        }

    }
}
