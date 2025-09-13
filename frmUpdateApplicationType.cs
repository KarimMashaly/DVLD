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
    public partial class frmUpdateApplicationType : Form
    {
        int _AppTypeID = 0;
        clsApplicationType _ApplicationType;
        public frmUpdateApplicationType(int AppTypeID)
        {
            InitializeComponent();
            _AppTypeID = AppTypeID;
        }

        private void frmUpdateApplicationType_Load(object sender, EventArgs e)
        {
            _LoadAppTypeData();
        }

        private void _LoadAppTypeData()
        {
             _ApplicationType = clsApplicationType.GetApplicationTypeByID(_AppTypeID);

            if (_ApplicationType != null)
            {
                lblAppTypeID.Text = _ApplicationType.ApplicationTypeID.ToString();
                txtAppTypeTitle.Text = _ApplicationType.ApplicationTypeTitle.ToString();
                txtAppTypeFees.Text = ((int)_ApplicationType.ApplicationFees).ToString();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _ApplicationType.ApplicationTypeTitle = txtAppTypeTitle.Text;
            _ApplicationType.ApplicationFees = Convert.ToDecimal(txtAppTypeFees.Text);

            if (_ApplicationType.UpdateApplicationType())
                MessageBox.Show("Data Saved Successfully", "Seccessful", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            else
                MessageBox.Show("Error, Data is not Saved!", "Error", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
