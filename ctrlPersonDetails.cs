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
    public partial class ctrlPersonDetails : UserControl
    {
        public ctrlPersonDetails()
        {
            InitializeComponent();
        }

        private void ctrlPersonDetails_Load(object sender, EventArgs e)
        {
            //SetDefaultData();
        }

        public void LoadPersonInfo(int PersonID)
        {
            clsPeople Person = clsPeople.Find(PersonID);

            lblPersonID.Text = Person.PersonID.ToString();
            lblFullName.Text = Person.FirstName + " " + Person.SecondName + 
                " " + Person.ThirdName + " " + Person.LastName;
            lblEmail.Text = Person.Email;
            lblPhone.Text = Person.Phone;
            lblAddress.Text = Person.Address;
            lblDateOfBirth.Text = Person.DateOfBirth.ToString("dd/MM/yyyy");
            lblCountry.Text = clsCountry.Find(Person.NationalityCountryID).CountryName;
            lblGender.Text = (Person.Gender == 0) ? "Male" : "Female";
            if (!string.IsNullOrEmpty(Person.ImagePath))
                pbImage.Image = new Bitmap(Person.ImagePath);
            else
                pbImage.Image = (Person.Gender == 0)? Properties.Resources.Male_512:
                    Properties.Resources.Female_512;
            lblNationalNo.Text = Person.NationalNo;
        }

        public void LoadPersonInfo(clsPeople Person)
        {

            lblPersonID.Text = Person.PersonID.ToString();
            lblFullName.Text = Person.FirstName + " " + Person.SecondName +
                " " + Person.ThirdName + " " + Person.LastName;
            lblEmail.Text = Person.Email;
            lblPhone.Text = Person.Phone;
            lblAddress.Text = Person.Address;
            lblDateOfBirth.Text = Person.DateOfBirth.ToString("dd/MM/yyyy");
            lblCountry.Text = clsCountry.Find(Person.NationalityCountryID).CountryName;
            lblGender.Text = (Person.Gender == 0) ? "Male" : "Female";
            if (!string.IsNullOrEmpty(Person.ImagePath))
                pbImage.Image = new Bitmap(Person.ImagePath);
            else
                pbImage.Image = (Person.Gender == 0) ? Properties.Resources.Male_512 :
                    Properties.Resources.Female_512;
            lblNationalNo.Text = Person.NationalNo;
        }

        public void SetDefaultData()
        {
            lblPersonID.Text = "[????]";
            lblFullName.Text = "[????]";
            lblEmail.Text = "[????]";
            lblPhone.Text = "[????]";
            lblAddress.Text = "[????]";
            lblDateOfBirth.Text = "[????]";
            lblCountry.Text = "[????]";
            lblGender.Text = "[????]";
            pbImage.Image = Properties.Resources.Male_512;
            lblNationalNo.Text = "[????]";
        }
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form frm = new frmAddEditPersonInfo(Convert.ToInt32(lblPersonID.Text));
            frm.ShowDialog();

            LoadPersonInfo(Convert.ToInt32(lblPersonID.Text));

        }
    }
}
