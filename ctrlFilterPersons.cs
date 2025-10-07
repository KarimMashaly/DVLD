using DVLD_BusinessLogicLayer;
using System;
using System.Windows.Forms;

namespace DVLD
{
    public partial class ctrlFilterPersons : UserControl
    {
        public int PersonID = 0;
        public ctrlFilterPersons()
        {
            InitializeComponent();
        }

        public void LoadDataInctrlPersonDetails(int UserID)
        {
            clsUser User = clsUser.Find(UserID);
            gbFilterPersons.Enabled = false;
            PersonID = User.PersonID;
            ctrlPersonDetails1.LoadPersonInfo(User.PersonID);
        }
        private void btnFindPerson_Click(object sender, EventArgs e)
        {
            clsPeople Person = (cbFilterItems.Text == "NationalNo") ? clsPeople.Find(txtFilterText.Text) :
                clsPeople.Find(Convert.ToInt32(txtFilterText.Text));

            if (Person != null)
            {
                PersonID = Person.PersonID;
                ctrlPersonDetails1.LoadPersonInfo(Person);
            }
            else
            {
                ctrlPersonDetails1.SetDefaultData();
            }
        }

        private void ctrlFilterPersons_Load(object sender, EventArgs e)
        {
            cbFilterItems.SelectedIndex = 0;
            //btnAddNewPerson.Enabled = false;
        }

        private void btnAddNewPerson_Click(object sender, EventArgs e)
        {
            frmAddEditPersonInfo frm = new frmAddEditPersonInfo(-1);
            frm.DataBack += frmAddEditPersonInfo_DataBack;
            frm.ShowDialog();
        }

        private void frmAddEditPersonInfo_DataBack(object sender, int PersonID)
        {
            this.PersonID = PersonID;
            ctrlPersonDetails1.LoadPersonInfo(PersonID);
            txtFilterText.Text = PersonID.ToString();
            cbFilterItems.Text = "PersonID";
        }
        private void txtFilterText_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilterItems.Text == "PersonID")
            {
                if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
                    e.Handled = true;
            }
        }
    }
}
