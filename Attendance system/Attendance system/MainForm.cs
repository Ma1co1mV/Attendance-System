using Attendance_system.DataSet1TableAdapters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Attendance_system
{
    public partial class MainForm : MetroFramework.Forms.MetroForm 
    {
        public int loggedIn { get; set; }
        public int UserID { get; set; }
        public MainForm()
        {
            InitializeComponent();
            loggedIn = 0;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dataSet11.AttendanceRecords' table. You can move, or remove it, as needed.
            
            // TODO: This line of code loads data into the 'dataSet1.Classes' table. You can move, or remove it, as needed.

        }

        private void MainForm_Activated(object sender, EventArgs e)
        {
            if (loggedIn == 0)
            {
                //Open login Form
                LoginForm newLogin = new LoginForm();
                newLogin.ShowDialog();

                if (newLogin.loginFlag == false)
                {
                    Close();
                }
                else
                {
                    UserID = newLogin.UserID;
                    statelabelUser.Text = UserID.ToString(); 
                    loggedIn = 1;

                    this.classesTableAdapter.Fill(this.dataSet1.Classes);

                    classesBindingSource.Filter = "UserId = '" + UserID.ToString() + "'";

                }
            }
        }

        private void metroButton3_Click(object sender, EventArgs e)
        {
            FormAddClass addClass = new FormAddClass(); 
            addClass.UserID = this.UserID;   
            addClass.ShowDialog();
        }

        private void metroButton4_Click(object sender, EventArgs e)
        {
            StudentsForm students = new StudentsForm();
            students.ClassName = metroComboBox1.Text;
            students.ClassID = (int)metroComboBox1.SelectedValue;
            students.ShowDialog();
        }

        private void metroButtonGet_Click_1(object sender, EventArgs e)
        {

            //Check if records exist
            AttendanceRecordsTableAdapter ada = new AttendanceRecordsTableAdapter();
            DataTable dt = ada.GetDataBy((int)metroComboBox1.SelectedValue, dateTimePicker1.Text);

            if (dt.Rows.Count > 0)
            {
                //we have records
                DataTable dt_new = ada.GetDataBy((int)metroComboBox1.SelectedValue, dateTimePicker1.Text);
                dataGridView2.DataSource = dt_new;

            }
            else
            {
                StudentsTableAdapter students_adapet = new StudentsTableAdapter();
                DataTable dt_Students = students_adapet.GetDataByClassID((int)metroComboBox1.SelectedValue);

                foreach (DataRow row in dt_Students.Rows)
                {
                    //Insert a new row for this student
                    ada.InsertQuery((int)row[0], (int)metroComboBox1.SelectedValue, dateTimePicker1.Text, "", row[1].ToString(), metroComboBox1.Text);


                }

                DataTable dt_new = ada.GetDataBy((int)metroComboBox1.SelectedValue, dateTimePicker1.Text);
                dataGridView2.DataSource = dt_new;
            }


        }

        private void metroButton2_Click(object sender, EventArgs e)
        {
            AttendanceRecordsTableAdapter ada = new AttendanceRecordsTableAdapter();

            foreach (DataGridViewRow row in dataGridView2.Rows) 
            {
                if((row.Cells[1].Value != null))
                {
                    
                    ada.UpdateQuery(row.Cells[1].Value.ToString(), row.Cells[0].Value.ToString(), (int)metroComboBox1.SelectedValue, dateTimePicker1.Text);
                    
                    
                }
                

            }

            DataTable dt_new = ada.GetDataBy((int)metroComboBox1.SelectedValue, dateTimePicker1.Text);
            dataGridView2.DataSource = dt_new;
        }

        private void metroButton1_Click(object sender, EventArgs e)
        {

            AttendanceRecordsTableAdapter ada = new AttendanceRecordsTableAdapter();

            foreach (DataGridViewRow row in dataGridView2.Rows)
            {
                if ((row.Cells[1].Value != null))
                {

                    ada.UpdateQuery("", row.Cells[0].Value.ToString(), (int)metroComboBox1.SelectedValue, dateTimePicker1.Text);


                }


            }

            DataTable dt_new = ada.GetDataBy((int)metroComboBox1.SelectedValue, dateTimePicker1.Text);
            dataGridView2.DataSource = dt_new;
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void metroButton5_Click(object sender, EventArgs e)
        {
            // Get students
            StudentsTableAdapter students_adapet = new StudentsTableAdapter();
            DataTable dt_Students = students_adapet.GetDataByClassID((int)metroComboBox2.SelectedValue);

            AttendanceRecordsTableAdapter ada = new AttendanceRecordsTableAdapter();

            int P, A, L, E = 0;

            //loop through students
            foreach (DataRow row in dt_Students.Rows) 
            {
                //Presence Count
                P = (int)ada.GetDataByReport(dateTimePicker2.Value.Month, row[1].ToString(), "present").Rows[0][6];

                //Absence Count
                A = (int)ada.GetDataByReport(dateTimePicker2.Value.Month, row[1].ToString(), "absent").Rows[0][6];

                //Late
                L = (int)ada.GetDataByReport(dateTimePicker2.Value.Month, row[1].ToString(), "late").Rows[0][6];


                //Excused
                E = (int)ada.GetDataByReport(dateTimePicker2.Value.Month, row[1].ToString(), "excused").Rows[0][6];

                ListViewItem litem = new ListViewItem();
                litem.Text = row[1].ToString();
                litem.SubItems.Add(P.ToString());
                litem.SubItems.Add(A.ToString());
                litem.SubItems.Add(L.ToString());
                litem.SubItems.Add(E.ToString());
                listView1.Items.Add(litem);
            }


 


        }
    }
}
