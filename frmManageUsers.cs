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
    public partial class frmManageUsers : Form
    {
        public frmManageUsers()
        {
            InitializeComponent();
        }

        private void frmManageUsers_Load(object sender, EventArgs e)
        {
            cbFilterUsers.SelectedIndex = 0;
           
            _RefreshUsers(_FilterUsers());
        }
        
        private DataTable _FilterUsers(string ColumnName = "", string FilterText = "")
        {
            DataTable dt = clsUser.GetAllUsers();

            dt.Columns.Add("FullName", typeof(string));
            foreach (DataRow row in dt.Rows)
            {
                clsPeople Person = clsPeople.Find((int)row["PersonID"]);
                row["FullName"] = Person.FirstName + " " + Person.SecondName + 
                    " " + Person.ThirdName + " " +  Person.LastName;
            }


            string FilterExpression = null;
            if(ColumnName == "IsActive")
            {
                if (cbForActivation.Text == "All")
                    return dt;
                else if (cbForActivation.Text == "Yes")
                    FilterExpression = $"{ColumnName} = true";
                else
                    FilterExpression = $"{ColumnName} = false";

                return (dt.Select(FilterExpression).Length > 0) ? dt.Select(FilterExpression).CopyToDataTable() : dt.Clone();
            }
            if (string.IsNullOrWhiteSpace(FilterText))
                return dt;

            if (ColumnName == "UserID" || ColumnName == "PersonID")
                if (int.TryParse(FilterText, out int number))
                    FilterExpression = $"{ColumnName} = {number}";
                else
                    return dt.Clone();
            else
                FilterExpression = $"{ColumnName} like '%{FilterText}%'";

            DataRow[] FilterRows = dt.Select(FilterExpression);

            return (FilterRows.Length > 0) ? FilterRows.CopyToDataTable() : dt.Clone();
        }
        private void _RefreshUsers(DataTable dt)
        {
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.Columns.Clear();

            dataGridView1.Columns.Add("colUserID", "UserID");
            dataGridView1.Columns["colUserID"].DataPropertyName = "UserID";

            dataGridView1.Columns.Add("colPersonID", "PersonID");
            dataGridView1.Columns["colPersonID"].DataPropertyName = "PersonID";

            dataGridView1.Columns.Add("colFullName", "Full Name");
            dataGridView1.Columns["colFullName"].DataPropertyName = "FullName";

            dataGridView1.Columns.Add("colUserName", "UserName");
            dataGridView1.Columns["colUserName"].DataPropertyName = "UserName";

            DataGridViewCheckBoxColumn chkIsActive = new DataGridViewCheckBoxColumn();
            chkIsActive.Name = "colIsActive";
            chkIsActive.HeaderText = "IsActive";
            chkIsActive.DataPropertyName = "IsActive";
            chkIsActive.TrueValue = true;
            chkIsActive.FalseValue = false;
            chkIsActive.ThreeState = false;
            dataGridView1.Columns.Add(chkIsActive);

            dataGridView1.DataSource = dt;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            lblNumberOfRecords.Text = dataGridView1.Rows.Count.ToString();
        }
        private void cbFilterUsers_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilterUsers.Text == "None")
            {
                txtFilter.Visible = false;
                cbForActivation.Visible = false;
            }
            else if (cbFilterUsers.Text == "IsActive")
            { 
               txtFilter.Visible = false;
                cbForActivation.SelectedIndex = 0;
                cbForActivation.Visible = true;

            }
            else
                txtFilter.Visible = true;

        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            _RefreshUsers(_FilterUsers(cbFilterUsers.Text , txtFilter.Text));
        }

        private void btnAddNewUser_Click(object sender, EventArgs e)
        {
            Form frm = new frmAddNewUser(-1);
            frm.ShowDialog();
            _RefreshUsers(_FilterUsers());
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(cbFilterUsers.Text == "UserID" ||  cbFilterUsers.Text =="PersonID")
            {
                if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)Keys.Back)
                    e.Handled = true;
            }
        }

        private void cbForActivation_SelectedIndexChanged(object sender, EventArgs e)
        {
            _RefreshUsers(_FilterUsers(cbFilterUsers.Text));
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmAddNewUser((int)dataGridView1.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            _RefreshUsers(_FilterUsers());
        }

        private void _DeleteUser(int UserID)
        {
            if (clsUser.DeleteUser(UserID))
                MessageBox.Show("User Deleted Successfully.", "Successful", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            else
                MessageBox.Show("User was not deleted, There is something wrong", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int UserID = (int)dataGridView1.CurrentRow.Cells[0].Value;
            string Message = "Are you sure you want to delete User with [" + UserID.ToString() + "]?";
            DialogResult Result = MessageBox.Show(Message, "Configuration Message",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            if (Result == DialogResult.OK)
            {
                if (clsApplication.IsApplicationExistByApplicantPersonID(UserID))
                {
                    MessageBox.Show("User was not deleted because it has data linked to it", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (UserID == clsSessionManager.UserID)
                {
                    DialogResult result = MessageBox.Show("Are you sure you want to delete this User because he is the current user",
                       "Configuration Message", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                    if (result == DialogResult.OK)
                    {
                        _DeleteUser(UserID);
                        Application.Exit();
                    }
                    else
                        return;
                }
                else
                    _DeleteUser(UserID);

                
            }

            _RefreshUsers(_FilterUsers());
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUserInfo frmUserInfo = new frmUserInfo((int)dataGridView1.CurrentRow.Cells[0].Value);
            frmUserInfo.ShowDialog();
        }

        private void changePaswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = new frmChangePassword((int)dataGridView1.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
        }
    }
}
