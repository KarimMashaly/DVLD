using DVLD_BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD
{
    public partial class frmListTestTypes : Form
    {
        public frmListTestTypes()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmListTestTypes_Load(object sender, EventArgs e)
        {
            _LoadListTestTypes();
        }

        private void _LoadListTestTypes()
        {
            DataTable dt = clsTestTypes.GetAllTestTypes();

            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.Columns.Clear();

            dataGridView1.Columns.Add("colTestTypeID", "ID");
            dataGridView1.Columns["colTestTypeID"].DataPropertyName = "TestTypeID";

            dataGridView1.Columns.Add("colTestTypeTitle", "Title");
            dataGridView1.Columns["colTestTypeTitle"].DataPropertyName = "TestTypeTitle";

            dataGridView1.Columns.Add("colTestTypeDescription", "Description");
            dataGridView1.Columns["colTestTypeDescription"].DataPropertyName = "TestTypeDescription";

            dataGridView1.Columns.Add("colTestTypeFees", "Fees");
            dataGridView1.Columns["colTestTypeFees"].DataPropertyName = "TestTypeFees";

            dataGridView1.DataSource = dt;

            dataGridView1.Columns["colTestTypeID"].Width = 50;
            dataGridView1.Columns["colTestTypeTitle"].Width = 200;
            dataGridView1.Columns["colTestTypeDescription"].Width = 350;
            dataGridView1.Columns["colTestTypeFees"].Width = 80;


            lblRecords.Text = "# Records: " +  dataGridView1.Rows.Count.ToString();
        }

        private void editApplicationTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmUpdateTestType((int)dataGridView1.CurrentRow.Cells[0].Value);
            frm.ShowDialog();

            _LoadListTestTypes();
        }
    }
}
