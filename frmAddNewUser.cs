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
    public partial class frmAddNewUser : Form
    {
        enum enMode { eAddNew, eUpdate}
        enMode _Mode;
        clsUser _User;
        int _UserID = 0;
        public frmAddNewUser(int UserID)
        {
            InitializeComponent();

            if (UserID == -1)
                _Mode = enMode.eAddNew;
            else
            {
                _Mode = enMode.eUpdate;
                _UserID = UserID;
            }
        }

        private void _IsChoosedUpdateUser()
        {
            lblAddEditUser.Text = "Update User";
            _User = clsUser.Find(_UserID);

            if (_User != null)
            {
                lblUserID.Text = _User.UserID.ToString();
                ctrlFilterPersons1.LoadDataInctrlPersonDetails(_User);
                txtUserName.Text = _User.UserName;
                txtPassword.Text = _User.Password;
                txtConfirmPassword.Text = txtPassword.Text;
                cbIsActive.Checked = _User.IsActive;
            }
        }

        private void _IsChoosedAddNewUser()
        {
            lblAddEditUser.Text = "Add New User";
            _User = new clsUser();
            
        }
        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (clsUser.IsUserExistByPersonID(ctrlFilterPersons1.PersonID))
                MessageBox.Show("Selected Perosn already has a user, Choose another one.", "Select another Person",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if(ctrlFilterPersons1.PersonID == 0)
                MessageBox.Show("There is no Person you selected, You should select person", "Select another Person",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            else
            {
                tabControl1.SelectedIndex = 1;
                btnSave.Enabled = true;
            }
        }

        private void frmAddNewUser_Load(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = true;
            txtConfirmPassword.UseSystemPasswordChar= true;

            if (_Mode == enMode.eUpdate)
            {
                _IsChoosedUpdateUser();
            }
            else
            {
                _IsChoosedAddNewUser();
                btnSave.Enabled = false;
                cbIsActive.Checked = true;
            }
           
        }

        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if(string.IsNullOrWhiteSpace(txtConfirmPassword.Text) ||
                txtConfirmPassword.Text != txtPassword.Text)
            {
                e.Cancel = true;
                txtConfirmPassword.Focus();
                errorProvider1.SetError(txtConfirmPassword, "Password Confirmation does not match Password!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtConfirmPassword, "");

            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            
            _User.PersonID = ctrlFilterPersons1.PersonID;
            _User.UserName = txtUserName.Text;
            _User.Password = txtPassword.Text;
            _User.IsActive = cbIsActive.Checked;

            if (_User.Save())
            {
                MessageBox.Show("Data Saved Successfully!", "Successful",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                lblAddEditUser.Text = "Update User";
                lblUserID.Text = _User.UserID.ToString();
            }
           
            else
                MessageBox.Show("Error: Data is not saved successfully!!", "Fail",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void tpLoginInfo_Click(object sender, EventArgs e)
        {

        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (e.TabPage == tpLoginInfo && _Mode == enMode.eAddNew && ctrlFilterPersons1.PersonID == 0)
            {
                e.Cancel = true;
                MessageBox.Show("This tap is disabled right now", "Sorry", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            else
                btnSave.Enabled = true;
        }
    }
}
