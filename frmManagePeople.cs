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
    public partial class frmManagePeople : Form
    {
        public frmManagePeople()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmManagePeople_Load(object sender, EventArgs e)
        {
            _RefreshPeopleList();
        }

        private void _RefreshPeopleList()
        {
            dataGridView1.DataSource = clsPeople.GetAllPeople();
            lblNumberOfRecords.Text = dataGridView1.RowCount.ToString();
        }

        private void btnAddNewPerson_Click(object sender, EventArgs e)
        {
            Form frm = new frmAddEditPersonInfo();
            frm.ShowDialog();
            _RefreshPeopleList();
        }
    }
}
