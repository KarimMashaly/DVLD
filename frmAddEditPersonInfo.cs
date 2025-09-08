using DVLD_BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
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
        bool _IsImageChanged = false;
        string _NewImagePath = null;
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
            lblAddEdit.Text = "Update Person";
            _FillCountriesInComboBox();
            cbCountry.SelectedItem = clsCountry.Find(_Person.NationalityCountryID).CountryName;
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

            if (_IsImageChanged)
            {
                // امسح القديمة لو موجودة
                if (!string.IsNullOrEmpty(_Person.ImagePath) && File.Exists(_Person.ImagePath))
                    File.Delete(_Person.ImagePath);


                if (!string.IsNullOrEmpty(_NewImagePath))
                {
                    // انسخ الصورة الجديدة
                    _Person.ImagePath = _CopyImage(_NewImagePath);
                }
                else
                {
                    // لو عملت Remove
                    _Person.ImagePath = null;
                }
            }

            if (_Person.Save())
                MessageBox.Show("Data saved successfully!!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            else
                MessageBox.Show("Error: Data is not saved successfully!!", "Fail", MessageBoxButtons.OK, MessageBoxIcon.Error);

            _Mode = enMode.eUpdate;
            lblAddEdit.Text = "Update Person";
            lblPersonID.Text = _Person.PersonID.ToString();


        }

        private Control FindControlRecursive(Control parent, string controlName)
        {
            foreach (Control ctrl in parent.Controls)
            {
                if (ctrl.Name == controlName)
                    return ctrl;

                Control found = FindControlRecursive(ctrl, controlName);
                if (found != null)
                    return found;
            }
            return null;
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
                //pbImage.Image = Image.FromFile(openFileDialog1.FileName);
               pbImage.Image =new Bitmap(openFileDialog1.FileName);
               // _IsCustomImage = true;
                _IsImageChanged = true;
                _NewImagePath = openFileDialog1.FileName;
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
            if (string.IsNullOrWhiteSpace(txtFirstName.Text))
            {
                txtFirstName.Focus();
                e.Cancel = true;
                errorProvider1.SetError(txtFirstName, "First Name should have a value");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtFirstName, "");
            }

        }

        private void txtSecondName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtSecondName.Text))
            {
                e.Cancel = true;
                txtSecondName.Focus();
                errorProvider1.SetError(txtSecondName, "Second Name should have a value");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtSecondName, "");
            }
        }

        private void txtThirdName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtThirdName.Text))
            {
                e.Cancel = true;
                txtThirdName.Focus();
                errorProvider1.SetError(txtThirdName, "Third Name should have a value");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtThirdName, "");
            }
        }

        private void txtLastName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLastName.Text))
            {
                e.Cancel = true;
                txtLastName.Focus();
                errorProvider1.SetError(txtLastName, "Last Name should have a value");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtLastName, "");
            }
        }

        private void txtNationalNo_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtNationalNo.Text))
            {
                e.Cancel = true;
                txtNationalNo.Focus();
                errorProvider1.SetError(txtNationalNo, "National Number should have a value");
                return;
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtNationalNo, "");
            }


            if (clsPeople.IsPersonExist(txtNationalNo.Text))
            {
                e.Cancel = true;
                txtNationalNo.Focus();
                errorProvider1.SetError(txtNationalNo, "National Number is used for another person!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtNationalNo, "");
            }
        }

        private void txtPhone_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtPhone.Text))
            {
                e.Cancel = true;
                txtPhone.Focus();
                errorProvider1.SetError(txtPhone, "Phone should have a value");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtPhone, "");
            }
        }

        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            string Email = txtEmail.Text.Trim();

            if (!Email.EndsWith("@gmail.com"))
            {
                e.Cancel = true;
                txtEmail.Focus();
                errorProvider1.SetError(txtEmail, "Invalid Email Address Format");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtEmail, "");
            }
        }

        
        private void linkLblRemoveImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            
            linkLblRemoveImage.Visible = false;
            pbImage.Image.Dispose();//علشان يحرر الصورة من الذاكرة 
            pbImage.Image = null;
            _IsImageChanged = true;
            _NewImagePath = null;
            _SetDefaultImage();
        }
    }
}
