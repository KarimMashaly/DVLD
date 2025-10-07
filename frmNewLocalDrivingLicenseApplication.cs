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
using static System.Net.Mime.MediaTypeNames;

namespace DVLD
{
    public partial class frmNewLocalDrivingLicenseApplication : Form
    {
        enum enMode { eAddNew, eUpdate}
        enMode _Mode;
        clsApplication _Application;
        clsLoaclDrivingLicenseApplication _LoaclDrivingLicenseApp;
        int _ApplicationID = 0;
        public frmNewLocalDrivingLicenseApplication(int AppID)
        {
            InitializeComponent();
            _ApplicationID = AppID;
        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (e.TabPage == tpApplicationInfo && _Mode == enMode.eAddNew && ctrlFilterPersons1.PersonID == 0)
            {
                e.Cancel = true;
                MessageBox.Show("This tap is disabled right now, You Should Select a person", "Sorry", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            else
                btnSave.Enabled = true;
        }

        private void frmNewLocalDrivingLicenseApplication_Load(object sender, EventArgs e)
        {
            if (_ApplicationID == -1)
                _IsChoosedAddNew();
            else
                _IsChoosedUpdate();
        }

        private void _LoadLicenseClassesInComboBox()
        {
            DataTable dt = clsLicenseClass.GetAllLicenseClasses();

            cbLicenseClasses.DataSource = dt;
            cbLicenseClasses.DisplayMember = "ClassName";
            cbLicenseClasses.ValueMember = "LicenseClassID";

        }
        private void _IsChoosedUpdate()
        {
            _Mode = enMode.eUpdate;
            lblAddUpdateApplication.Text = "Update Local Driving License Application";
            _Application= clsApplication.FindByAppID(_ApplicationID);
            _LoaclDrivingLicenseApp= clsLoaclDrivingLicenseApplication.FindByAppID(_ApplicationID);
            cbLicenseClasses.SelectedValue = _LoaclDrivingLicenseApp.LicenseClassID;
            ctrlFilterPersons1.LoadDataInctrlPersonDetails(_Application.ApplicantPersonID);
            lblAppID.Text = _Application.ApplicationID.ToString();
            lblAppDate.Text = _Application.ApplicationDate.ToString("dd/MM/yyyy");
            lblUserName.Text = clsSessionManager.UserName;
        }

        private void _IsChoosedAddNew()
        {
            _Mode = enMode.eAddNew;
            lblAddUpdateApplication.Text = "New Local Driving License Application";
            _Application = new clsApplication();
            _LoaclDrivingLicenseApp = new clsLoaclDrivingLicenseApplication();
            _LoadLicenseClassesInComboBox();
            cbLicenseClasses.SelectedIndex = 2;
            lblAppDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            lblUserName.Text = clsSessionManager.UserName;
            btnSave.Enabled  = false;
        }

        private int _GetExistingAppID()
        {
            int PersonID = ctrlFilterPersons1.PersonID;
            DataTable dt = clsApplication.GetAllApplications();
            DataRow[] Rows = dt.Select($"ApplicantPersonID = {PersonID} and ApplicationTypeID = 1 and ApplicationStatus =1");

            foreach (DataRow Row in Rows)
            {
                clsLoaclDrivingLicenseApplication LocalDrivingLicenseClassApp = clsLoaclDrivingLicenseApplication.FindByAppID((int)Row["ApplicationID"]);
                if (LocalDrivingLicenseClassApp.LicenseClassID == (int)cbLicenseClasses.SelectedValue)
                    return LocalDrivingLicenseClassApp.AppID;
            }

            return 0;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            int AppID = _GetExistingAppID();
            if (AppID != 0)
            {
                MessageBox.Show($@"Choose another License Class, the selected person already have an active application for the selected calss with id = {AppID}", "Error",
                 MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _Application.ApplicantPersonID = ctrlFilterPersons1.PersonID;
            if (_Mode == enMode.eAddNew)
            {
                _Application.ApplicationDate = DateTime.Now;
                _Application.LastStatusDate = DateTime.Now;
            }
            _Application.ApplicationTypeID = 1;
            _Application.ApplicationStatus = 1;
            _Application.PaidFees = 15;
            _Application.CreatedByUserID = clsSessionManager.UserID;
            _Application.Save();
            _LoaclDrivingLicenseApp.AppID = _Application.ApplicationID;
            _LoaclDrivingLicenseApp.LicenseClassID = (int)cbLicenseClasses.SelectedValue;
            if (_LoaclDrivingLicenseApp.Save())
            {
                MessageBox.Show("Data Saved Successfully", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                lblAppID.Text = _LoaclDrivingLicenseApp.LocalDrivingLicenseAppID.ToString();
                lblAddUpdateApplication.Text = "Update Local Driving License Class Application";
                _Mode = enMode.eUpdate;
            }
            else
                MessageBox.Show("Data is not Saved", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 1;
        }
    }
}
