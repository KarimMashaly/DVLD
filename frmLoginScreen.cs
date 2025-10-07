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
    public partial class frmLoginScreen : Form
    {
        string _UserName;
        string _Password;
        public frmLoginScreen()
        {
            InitializeComponent();
        }

        private void frmLoginScreen_Load(object sender, EventArgs e)
        {
            txtPassword.UseSystemPasswordChar = true;

            if (Properties.Settings.Default.UserName == null)
                return;

            if (clsUser.IsUserExistByUserName(Properties.Settings.Default.UserName))
            {
                txtUserName.Text = Properties.Settings.Default.UserName;
                txtPassword.Text = Properties.Settings.Default.Password;
            }
            if (!string.IsNullOrEmpty(Properties.Settings.Default.UserName))
                cbRememberMe.Checked = true;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            clsUser User = clsUser.Find(txtUserName.Text.Trim());


            if (User != null && txtPassword.Text == User.Password && User.IsActive)
            {
                clsSessionManager.UserName = User.UserName;
                clsSessionManager.UserID = User.UserID;
                clsSessionManager.IsActive = User.IsActive;

                Properties.Settings.Default.Save();

                Form frm = new frmMainForm();
                frm.Show();

                if (!cbRememberMe.Checked)
                {
                    Properties.Settings.Default.UserName = null;
                    Properties.Settings.Default.Password = null;
                    txtPassword.Text = "";
                    txtUserName.Text = "";
                   
                }
                else
                {
                    Properties.Settings.Default.UserName = txtUserName.Text;
                    Properties.Settings.Default.Password = txtPassword.Text;
                    
                }
                Properties.Settings.Default.Save();
            }
            else if (User == null)
                MessageBox.Show("The User doesn't exist, Please write anoter one", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            else if (!User.IsActive)
            {
                MessageBox.Show("Your account is deactivated, Please contact your admin.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (User.Password != txtPassword.Text)
            {
                MessageBox.Show("The Username or password is incorrect, please try again!", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            

        }

        
    }
}
