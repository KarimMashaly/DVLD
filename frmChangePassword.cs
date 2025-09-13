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
    public partial class frmChangePassword : Form
    {
        int _UserID = 0;
        clsUser _User;
        public frmChangePassword(int UserID)
        {
            InitializeComponent();
            _UserID = UserID;
            
        }

        private void frmChangePassword_Load(object sender, EventArgs e)
        {
            ctrlUserInformation1.LoadUserInfo(_UserID);

            txtCurrentPassword.UseSystemPasswordChar = true;
            txtNewPassword.UseSystemPasswordChar = true;
            txtConfirmNewPassword.UseSystemPasswordChar = true;
            
            _User = clsUser.Find(_UserID);
        }

        private void txtCurrentPassword_Validating(object sender, CancelEventArgs e)
        {
            if(txtCurrentPassword.Text !=_User.Password)
            {
                e.Cancel = true;
                txtCurrentPassword.Focus();
                errorProvider1.SetError(txtCurrentPassword, "Current Password is wrong!");
            }
            else
            {
                e.Cancel =false;
                errorProvider1.SetError(txtCurrentPassword, "");
            }
        }

        private void txtConfirmNewPassword_Validating(object sender, CancelEventArgs e)
        {
            if(txtConfirmNewPassword.Text != txtNewPassword.Text)
            {
                e.Cancel = true;
                txtConfirmNewPassword.Focus();
                errorProvider1.SetError(txtConfirmNewPassword, "Password Confirmation does not match Password!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtConfirmNewPassword, "");

            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();   
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _User.Password = txtNewPassword.Text;
            if (_User.Save())
            {
                MessageBox.Show("Data Saved Successfully!", "Successful",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            else
                MessageBox.Show("Error: Data is not saved successfully!!", "Fail",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
