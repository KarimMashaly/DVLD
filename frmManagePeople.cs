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
    public partial class frmManagePeople : Form
    {
        enum enAddEdit { eAdd, eEdit}
        public frmManagePeople()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmManagePeople_Load(object sender, EventArgs e)
        {
            _RefreshPeopleList(_FilterPersons());
            cbFilterPersons.SelectedIndex = 0;
        }

        private void _RefreshPeopleList(DataTable dt)
        {
            
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.Columns.Clear();

            dataGridView1.Columns.Add("colPersonID", "PersonID");
            dataGridView1.Columns["colPersonID"].DataPropertyName = "PersonID";

            dataGridView1.Columns.Add("colNationalNo", "NationalNo");
            dataGridView1.Columns["colNationalNo"].DataPropertyName = "NationalNo";

            dataGridView1.Columns.Add("colFirstName", "FirstName");
            dataGridView1.Columns["colFirstName"].DataPropertyName = "FirstName";

            dataGridView1.Columns.Add("colSecondName", "SecondName");
            dataGridView1.Columns["colSecondName"].DataPropertyName = "SecondName";

            dataGridView1.Columns.Add("colThirdName", "ThirdName");
            dataGridView1.Columns["colThirdName"].DataPropertyName = "ThirdName";

            dataGridView1.Columns.Add("colLastName", "LastName");
            dataGridView1.Columns["colLastName"].DataPropertyName = "LastName";

            dataGridView1.Columns.Add("colGender", "Gender");
            dataGridView1.Columns["colGender"].DataPropertyName = "Gender";

            dataGridView1.Columns.Add("colDateOfBirth", "DateOfBirth");
            dataGridView1.Columns["colDateOfBirth"].DataPropertyName = "DateOfBirth";

            dataGridView1.Columns.Add("colNationality", "Nationality");
            dataGridView1.Columns["colNationality"].DataPropertyName = "Nationality";

            dataGridView1.Columns.Add("colPhone", "Phone");
            dataGridView1.Columns["colPhone"].DataPropertyName = "Phone";

            dataGridView1.Columns.Add("colEmail", "Email");
            dataGridView1.Columns["colEmail"].DataPropertyName = "Email";

            //DataRow[]rows = dt.Select("Nationality = 'Egypt'");
            dataGridView1.DataSource = dt;
            lblNumberOfRecords.Text = dataGridView1.RowCount.ToString();
        }

        private void btnAddNewPerson_Click(object sender, EventArgs e)
        {
            Form frm = new frmAddEditPersonInfo(-1);
            frm.ShowDialog();
            _RefreshPeopleList(_FilterPersons());
        }

        private void addNewPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmAddEditPersonInfo(-1);
            frm.ShowDialog();
            _RefreshPeopleList(_FilterPersons());
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmAddEditPersonInfo((int)dataGridView1.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            _RefreshPeopleList(_FilterPersons());
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int PersonID = (int)dataGridView1.CurrentRow.Cells[0].Value;
            string Message = "Are you sure you want to delete person [" + PersonID.ToString() + "]?";
            DialogResult Result = MessageBox.Show(Message, "Configuration Message",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            if(Result == DialogResult.OK)
            {
                if (clsApplication.IsApplicationExistByApplicantPersonID(PersonID))
                {
                    MessageBox.Show("Person was not deleted because it has data linked to it", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                clsPeople.DeletePerson(PersonID);

                MessageBox.Show("Person Deleted Successfully.", "Successful", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }

            _RefreshPeopleList(clsPeople.GetAllPeople());
        }

        private DataTable _FilterPersons(string ColumnName = "", string FilterText = "")
        {
            DataTable dt = clsPeople.GetAllPeople();

            dt.Columns.Add("Gender", typeof(string));

            foreach (DataRow row in dt.Rows)
            {
                row["Gender"] = (Convert.ToByte(row["Gendor"]) == 0 ? "Male" : "Female");

            }

            dt.Columns.Add("Nationality", typeof(string));

            foreach (DataRow row in dt.Rows)
            {
                row["Nationality"] = clsCountry.Find((int)row["NationalityCountryID"]).CountryName;
            }


            if (string.IsNullOrWhiteSpace(FilterText))
                return dt;

            string FilterExpression = "";

            if (ColumnName == "PersonID")
                if (int.TryParse(FilterText, out int number))
                    FilterExpression = $"{ColumnName} = {number}";
                else
                    return dt.Clone();
            else
                FilterExpression = $"{ColumnName} like '%{FilterText}%'";


            DataRow[] filteredRows = dt.Select(FilterExpression);

            if (filteredRows.Length > 0)
                return filteredRows.CopyToDataTable();
            else
                return dt.Clone();
        }

        private void cbFilterPersons_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilterPersons.SelectedIndex == 0)
            {
                _RefreshPeopleList(_FilterPersons());
                txtFilter.Visible = false;
            }
            else
            {
                txtFilter.Visible = true;
            }
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            _RefreshPeopleList( _FilterPersons(cbFilterPersons.Text, txtFilter.Text));
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmPersonDetails((int)dataGridView1.CurrentRow.Cells[0].Value);
            frm.ShowDialog();

            _RefreshPeopleList(_FilterPersons());
        }
    }
}
