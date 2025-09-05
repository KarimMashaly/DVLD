using DVLD_BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD
{
    public partial class frmAddEditPersonInfo : Form
    {
        enum enGender { eMale, eFemale}
         enum enMode { eAddNew, eUpdate}
         enMode _Mode;
        int _PersonID;
        clsPeople _Person;
        bool _IsCustomImage = false;
        public frmAddEditPersonInfo(int PersonID)
        {
            InitializeComponent();
            if (PersonID == -1)
                _Mode = enMode.eAddNew;
            else
            { 
                _Mode = enMode.eUpdate;
                _PersonID = PersonID;
            }
        }

        private void frmAddEditPersonInfo_Load(object sender, EventArgs e)
        {
            //_IsCustomImage = false;
            if (_Mode == enMode.eAddNew)
            {
                _IsChoosedAddNewPerson();
            }
            else
            {
                _IsChoosedUpdatePerson();
            }
        }
        
        private void _IsChoosedAddNewPerson()
        {
            _Person = new clsPeople();
            lblAddEdit.Text = "Add New Person";
            _FillCountriesInComboBox();
            cbCountry.SelectedIndex = 0;
            _Validateing18Years();
            linkLblRemoveImage.Visible = false;
            rbMale.Checked = true;
            _SetDefaultImage();
        }

        private void _IsChoosedUpdatePerson()
        {
            _Person = clsPeople.Find(_PersonID);
            //MessageBox.Show(_Person.FirstName);
            lblAddEdit.Text = "Update Person";
            _FillCountriesInComboBox();
            cbCountry.SelectedItem = clsCountry.Find(_Person.NationalityCountryID).CountryName;
            //cbCountry.SelectedIndex = cbCountry.FindString(clsCountry.Find(_Person.NationalityCountryID).CountryName);
            lblPersonID.Text = _Person.PersonID.ToString();
            txtNationalNo.Text = _Person.NationalNo;
            txtFirstName.Text = _Person.FirstName;
            txtSecondName .Text = _Person.SecondName;
            txtThirdName .Text = _Person.ThirdName;
            txtLastName.Text = _Person.LastName;   
            txtEmail.Text = _Person.Email;
            txtPhone.Text = _Person.Phone;
            dtpDateOfBirth.Value = _Person.DateOfBirth;

            if (_Person.Gender == 0)
                rbMale.Checked = true;
            else
                rbFemale.Checked = true;

            txtAddress.Text = _Person.Address;

            if(_Person.ImagePath != null && _Person.ImagePath != "")
            {
                //_IsCustomImage = true;
                pbImage.Image = new Bitmap(_Person.ImagePath); 
            }
            else
            {
                _SetDefaultImage();
            }
        }
        private void _SetDefaultImage()
        {
            linkLblRemoveImage.Visible = false;
            if (rbMale.Checked)
                pbImage.Image = Properties.Resources.Male_512;
            else
                pbImage.Image = Properties.Resources.Female_512;

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
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            _RemoveImageFromFolder();
            int CountryID = clsCountry.Find(cbCountry.Text).CountryID;

            _Person.FirstName = txtFirstName.Text;
            _Person.SecondName = txtSecondName.Text;
            _Person.ThirdName = txtThirdName.Text;
            _Person.LastName = txtLastName.Text;
            _Person.NationalNo = txtNationalNo.Text;
            _Person.DateOfBirth = dtpDateOfBirth.Value;
            _Person.Gender = (rbMale.Checked) ? (byte)enGender.eMale : (byte)enGender.eFemale;
            _Person.Phone = txtPhone.Text;
            _Person.Email = txtEmail.Text;
            _Person.Address = txtAddress.Text;
            _Person.NationalityCountryID = CountryID;
            if (_IsCustomImage)
                _Person.ImagePath = _CopyImage(openFileDialog1.FileName);
            else
            {
                if (_Mode == enMode.eAddNew)
                    _Person.ImagePath = null;
            }
                //_Person.ImagePath = (_IsCustomImage) ? _CopyImage(openFileDialog1.FileName) : null;
            if (_Person.Save())
                MessageBox.Show("Data saved successfully!!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("Error: Data is not saved successfully!!", "Fail", MessageBoxButtons.OK, MessageBoxIcon.Error);

            _Mode = enMode.eUpdate;
            lblAddEdit.Text = "Update Person";
            lblPersonID.Text = _Person.PersonID.ToString();

        }

        private string _CopyImage(string SourcePath)
        {

            string FolderPath = @"D:\DVLD_Images";

            if(!Directory.Exists(FolderPath))
                Directory.CreateDirectory(FolderPath);
            
            string Extension = Path.GetExtension(SourcePath);
            string NewFileName = Guid.NewGuid().ToString() + Extension;
            string DestPath = Path.Combine(FolderPath, NewFileName);

            File.Copy(SourcePath, DestPath,true);

            //MessageBox.Show(DestPath);
            return DestPath;
        }
        private void LinkLblSetImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            openFileDialog1.InitialDirectory = @"Downloads";
            openFileDialog1.Title = "Choose Image";
            openFileDialog1.DefaultExt = "png";
            openFileDialog1.Filter = @"All Files (*.*)|*.*| PNG File (*.png)|*.png| JPG File (*.jpg)|*.jpg";

            if(openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pbImage.Image = Image.FromFile(openFileDialog1.FileName);
                _IsCustomImage = true;
                linkLblRemoveImage.Visible = true;
            }
        }

        private void rbFemale_CheckedChanged(object sender, EventArgs e)
        {
            
            if(rbFemale.Checked && !_IsCustomImage)
                pbImage.Image = Properties.Resources.Female_512;
        }

        private void rbMale_CheckedChanged(object sender, EventArgs e)
        {
            if (rbMale.Checked && !_IsCustomImage)
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

        private void _RemoveImageFromFolder()
        {
            if (_Mode == enMode.eUpdate && !string.IsNullOrEmpty(_Person.ImagePath) && File.Exists(_Person.ImagePath))
            {
              
                File.Delete(_Person.ImagePath);
                _Person.ImagePath = null;
            }
        }
        private void linkLblRemoveImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            _IsCustomImage = false;
            linkLblRemoveImage.Visible = false;
            pbImage.Image.Dispose();//علشان يحرر الصورة من الذاكرة 
            pbImage.Image = null;
            _SetDefaultImage();
        }
    }
}
