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
    public partial class frmUserInfo : Form
    {
        public frmUserInfo(int UserID)
        {
            InitializeComponent();

            ctrlUserInformation1.LoadUserInfo(UserID);
        }

        private void ctrlUserInformation1_Load(object sender, EventArgs e)
        {

        }
    }
}
