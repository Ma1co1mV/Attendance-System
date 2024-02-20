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
    public partial class StudentsForm : Form
    {
        public int ClassID { get; set; }
        public string ClassName { get; set; }
        public StudentsForm()
        {
            InitializeComponent();
        }

        private void StudentsForm_Load_1(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'dataSet1.Students' table. You can move, or remove it, as needed.
            this.studentsTableAdapter.Fill(this.dataSet1.Students);
            labelClassName.Text = ClassName.ToString();
            labelClassID.Text = ClassID.ToString();
        }

        private void metroButton1_Click_1(object sender, EventArgs e)
        {
            this.studentsBindingSource.EndEdit();
            this.studentsTableAdapter.Update(dataSet1.Students);
        }
    }
}
