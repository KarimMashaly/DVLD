using DVLD_BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD
{
    public partial class frmLocalDrivingLicenseApps : Form
    {
        public frmLocalDrivingLicenseApps()
        {
            InitializeComponent();
        }
         enum enAppStatus
        {
            eNew = 1,
            eCancelled = 2, 
            eCompleted = 3
        }

        enAppStatus AppStatus;
        private void frmLocalDrivingLicenseApps_Load(object sender, EventArgs e)
        {
            txtFilter.Visible = false;
            cbFilterApps.SelectedIndex = 0;
            _RefreshAppsList(_FilterApps());
        }

        private void _RefreshAppsList(DataTable dt)
        {
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.Columns.Clear();

            dataGridView1.Columns.Add("colLocalDrivingLicenseApplicationID", "L.D.L.AppID");
            dataGridView1.Columns["colLocalDrivingLicenseApplicationID"].DataPropertyName = "LocalDrivingLicenseApplicationID";

            dataGridView1.Columns.Add("colClassName", "Class Name");
            dataGridView1.Columns["colClassName"].DataPropertyName = "ClassName";

            dataGridView1.Columns.Add("colNationalNo", "National No.");
            dataGridView1.Columns["colNationalNo"].DataPropertyName = "NationalNo";

            dataGridView1.Columns.Add("colFullName", "Full Name");
            dataGridView1.Columns["colFullName"].DataPropertyName = "FullName";

            dataGridView1.Columns.Add("colApplicationDate", "Application Date");
            dataGridView1.Columns["colApplicationDate"].DataPropertyName = "ApplicationDate";

            dataGridView1.Columns.Add("colPassedTest", "Passed Test");
            dataGridView1.Columns["colPassedTest"].DataPropertyName = "PassedTestCount";

            dataGridView1.Columns.Add("colStatus", "Status");
            dataGridView1.Columns["colStatus"].DataPropertyName = "Status";

            dataGridView1.DataSource = dt;
            lblRecords.Text = "# Records: " + dataGridView1.RowCount.ToString();
           
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.Columns["colLocalDrivingLicenseApplicationID"].FillWeight = 80; // L.D.L.AppID
            dataGridView1.Columns["colClassName"].FillWeight = 190;                        // Class Name
            dataGridView1.Columns["colNationalNo"].FillWeight = 90;                       // National No.
            dataGridView1.Columns["colFullName"].FillWeight = 200;                         // Full Name
            dataGridView1.Columns["colApplicationDate"].FillWeight = 120;                  // Application Date
            dataGridView1.Columns["colPassedTest"].FillWeight = 100;                       // Passed Test
            dataGridView1.Columns["colStatus"].FillWeight = 80;
        }

        private DataTable _FilterApps(string ColumnName = "", string FilterText = "")
        {
            DataTable dt = clsLoaclDrivingLicenseApplication.GetAllAppsFromView();

            if (string.IsNullOrWhiteSpace(FilterText))
                return dt;

            string FilterExpression = "";

            if (ColumnName == "L.D.L.AppID")
                ColumnName = "LocalDrivingLicenseApplicationID";
            else if (ColumnName == "Class Name")
                ColumnName = "ClassName";
            else if (ColumnName == "Full Name")
                ColumnName = "FullName";
            else if (ColumnName == "Application Date")
                ColumnName = "ApplicationDate";
            else if (ColumnName == "National No.")
                ColumnName = "NationalNo";

            if (ColumnName == "LocalDrivingLicenseApplicationID")
                if (int.TryParse(FilterText, out int number))
                    FilterExpression = $"{ColumnName} = {number}";
                else
                    return dt.Clone();
            else
                FilterExpression = $"{ColumnName} like '%{FilterText}%'";

            DataRow [] FilterRows = dt.Select(FilterExpression);

            return (FilterRows.Length > 0) ? FilterRows.CopyToDataTable() : dt.Clone();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            _RefreshAppsList(_FilterApps(cbFilterApps.Text, txtFilter.Text));
        }

        private void cbFilterApps_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cbFilterApps.SelectedIndex == 0)
            {
                _RefreshAppsList(_FilterApps());
                txtFilter.Visible = false;
            }
            else
                txtFilter.Visible = true;

        }

        private void cancelApplicationToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            clsLoaclDrivingLicenseApplication L_D_L_App = clsLoaclDrivingLicenseApplication.FindByL_D_L_AppID((int)dataGridView1.CurrentRow.Cells[0].Value);
            clsApplication App = clsApplication.FindByAppID(L_D_L_App.AppID);

            DialogResult result = MessageBox.Show("Are you sure do want to Cancel this Application?", "Confirm", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            if(result == DialogResult.OK)
            {
                App.ApplicationStatus = (int)enAppStatus.eCancelled;
                App.Save();
                _RefreshAppsList(_FilterApps());
            }
            
            
        }

        private void btnAddNewLocalDrivingLicenseApp_Click(object sender, EventArgs e)
        {
            Form frm = new frmNewLocalDrivingLicenseApplication(-1);
            frm.ShowDialog();
            _RefreshAppsList(_FilterApps());
        }
    }
}
