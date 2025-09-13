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
    public partial class ctrlUserInformation : UserControl
    {
        public ctrlUserInformation()
        {
            InitializeComponent();
        }

        private void ctrlUserInformation_Load(object sender, EventArgs e)
        {

        }

        public void LoadUserInfo(int UserID)
        {
            clsUser User = clsUser.Find(UserID);

            if (User != null)
            {
                ctrlPersonDetails1.LoadPersonInfo(User.PersonID);
                lblUserID.Text = "UserID: " + UserID.ToString();
                lblUserName.Text = "UserName: " + User.UserName;
                lblIsActive.Text = "IsActive: " + ((User.IsActive)? "Yes" : "No");
            }
        }
    }
}
