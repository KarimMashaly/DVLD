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
    public partial class frmAddEditPersonInfo : Form
    {
        enum enGender { eMale, eFemale}
        //enGender _Gender;
        bool IsCustomImage = false;
        public frmAddEditPersonInfo()
        {
            InitializeComponent();
        }

        private void frmAddEditPersonInfo_Load(object sender, EventArgs e)
        {
            _FillCountriesInComboBox();
            _SetDefaultImage();
            _Validateing18Years();
            linkLblRemoveImage.Visible = false;
        }

        private void _SetDefaultImage()
        {
            rbMale.Checked = true;
            pbImage.Image = Properties.Resources.Male_512;

        }

        private void _Validateing18Years()
        {
            dtpDateOfBirth.MaxDate = DateTime.Now.AddYears(-18);
        }
        private void _FillCountriesInComboBox()
        {
            DataTable dtCountries = clsCountry.GetAllCountries();

            foreach (DataRow row in dtCountries.Rows)
            {

                cbCountry.Items.Add(row["CountryName"]);

            }

            cbCountry.SelectedIndex = 0;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            int CountryID = clsCountry.Find(cbCountry.Text).CountryID;
            clsPeople NewPerson = new clsPeople();

            NewPerson.FirstName = txtFirstName.Text;
            NewPerson.SecondName = txtSecondName.Text;
            NewPerson.ThirdName = txtThirdName.Text;
            NewPerson.LastName = txtLastName.Text;
            NewPerson.NationalNo = txtNationalNo.Text;
            NewPerson.DateOfBirth = dtpDateOfBirth.Value;
            NewPerson.Gender = (rbMale.Checked) ? (short)enGender.eMale : (short)enGender.eFemale;
            NewPerson.Phone = txtPhone.Text;
            NewPerson.Email = txtEmail.Text;
            NewPerson.Address = txtAddress.Text;
            NewPerson.NationalityCountryID = CountryID;
            NewPerson.ImagePath = openFileDialog1.FileName;
            if (NewPerson.Save())
                MessageBox.Show("New Person is added successfully!!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("Faild To added the new person", "Fail", MessageBoxButtons.OK, MessageBoxIcon.Error);

            lblPersonID.Text = NewPerson.PersonID.ToString();
        }

        private void LinkLblSetImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openFileDialog1.InitialDirectory = @"Downloads";
            openFileDialog1.Title = "Set Image";
            openFileDialog1.DefaultExt = "png";
            openFileDialog1.Filter = @"All Files (*.*)|*.*| PNG File (*.png)|*.png| JPG File (*.jpg)|*.jpg";

            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pbImage.Image = Image.FromFile(openFileDialog1.FileName);
                IsCustomImage = true;
                linkLblRemoveImage.Visible = true;
            }
        }

        private void rbFemale_CheckedChanged(object sender, EventArgs e)
        {
            
            if(rbFemale.Checked && !IsCustomImage)
                pbImage.Image = Properties.Resources.Female_512;
        }

        private void rbMale_CheckedChanged(object sender, EventArgs e)
        {
            if (rbMale.Checked && !IsCustomImage)
                pbImage.Image = Properties.Resources.Male_512;
        }

        private void txtFirstName_Validating(object sender, CancelEventArgs e)
        {
            if(string.IsNullOrWhiteSpace(txtFirstName.Text))
            {
                txtFirstName.Focus();
                errorProvider1.SetError(txtFirstName, "First Name should have a value");
            }
            else
                errorProvider1.SetError(txtFirstName, "");

        }

        private void txtSecondName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSecondName.Text))
            {
                txtSecondName.Focus();
                errorProvider1.SetError(txtSecondName, "Second Name should have a value");
            }
            else
                errorProvider1.SetError(txtSecondName, "");
        }

        private void txtThirdName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtThirdName.Text))
            {
                txtThirdName.Focus();
                errorProvider1.SetError(txtThirdName, "Third Name should have a value");
            }
            else
                errorProvider1.SetError(txtThirdName, "");
        }

        private void txtLastName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLastName.Text))
            {
                txtLastName.Focus();
                errorProvider1.SetError(txtLastName, "Last Name should have a value");
            }
            else
                errorProvider1.SetError(txtLastName, "");
        }

        private void txtNationalNo_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtNationalNo.Text))
            {
                txtNationalNo.Focus();
                errorProvider1.SetError(txtNationalNo, "National Number should have a value");
                return;
            }
            else
               errorProvider1.SetError(txtNationalNo, "");



            if (clsPeople.IsPersonExist(txtNationalNo.Text))
            {
                txtNationalNo.Focus();
                errorProvider1.SetError(txtNationalNo, "National Number is used for another person!");
            }
            else
                errorProvider1.SetError(txtNationalNo, "");
        }

        private void txtPhone_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtPhone.Text))
            {
                txtPhone.Focus();
                errorProvider1.SetError(txtPhone, "Phone should have a value");
            }
            else
                errorProvider1.SetError(txtPhone, "");
        }

        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            string Email = txtEmail.Text.Trim();

            if (!Email.EndsWith("@gmail.com"))
            {
                txtEmail.Focus();
                errorProvider1.SetError(txtEmail, "Invalid Email Address Format");
            }
            else
                errorProvider1.SetError(txtEmail, "");

        }

        private void linkLblRemoveImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            _SetDefaultImage();
        }
    }
}
