using DVLD_BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD
{
    public partial class frmManageApplicationTypes : Form
    {
        public frmManageApplicationTypes()
        {
            InitializeComponent();
        }

        private void frmManageApplicationTypes_Load(object sender, EventArgs e)
        {
            _GetAppTypesList();
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        private void _GetAppTypesList()
        {
            dataGridView1.DataSource = clsApplicationType.GetAllApplicationTypes();
            lblRecords.Text = "# Records: " + dataGridView1.RowCount.ToString();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void editApplicationTypeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmUpdateApplicationType((int)dataGridView1.CurrentRow.Cells[0].Value);
            frm.ShowDialog();

            _GetAppTypesList();
        }
    }
}
