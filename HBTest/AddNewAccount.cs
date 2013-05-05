using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;


namespace HBTest
{
    public partial class frmAddNewAccount : Form
    {
        private int userID;
        public frmAddNewAccount(int userID)
        {
            InitializeComponent();
            this.userID = userID;
        }

        private void frmAddNewAccount_Load(object sender, EventArgs e)
        {
            //set datetimepickers max date as today
            dtpDate.MaxDate = DateTime.Now;
            //load bank combobox
            using (HomeBankModel.HBContext context = new HomeBankModel.HBContext())
            {
                cboCred.DataSource = context.GetAllBanks(userID);
            }
            // disable the Add button
            btnAdd.Enabled = false;


            // combobox autoComleate
            
            cboCred.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboCred.AutoCompleteSource = AutoCompleteSource.ListItems;

            //decimal filter
            txtBalance.KeyPress += Helpers.ControlHelper.DecimalTextboxKeyPressEvent;
        }

        private int bankID;
        private void btnAdd_Click(object sender, EventArgs e)
        {
            // bank combobox cannot be empty
            if (cboCred.SelectedItem == null && cboCred.Text.Trim() != String.Empty)
            {
                //addnewcreditinst method add new bank and returns its ID
                bankID = Helpers.BankHelper.AddNewBank(cboCred.Text.Trim(),userID);
            }
            else if (cboCred.SelectedItem == null)
            {
                MessageBox.Show("Please enter a valid Bank");
                return;
            }
            else
            {
                bankID = (int)cboCred.SelectedValue;
            }

            // set new accounts properties
            string name = txtName.Text.Trim();
            decimal balance = decimal.Parse(txtBalance.Text);
            DateTime date = dtpDate.Value;

            if (!String.IsNullOrEmpty(name) || date < DateTime.Now)
            {
                // call method to add an account
                AddNewAccount(name, balance, date);
            }
            //close form and send OK result to main form
            this.DialogResult = System.Windows.Forms.DialogResult.OK;

        }

        private void AddNewAccount(string name, decimal balance, DateTime date)
        {
            using (HomeBankModel.HBContext context = new HomeBankModel.HBContext())
            {
                context.AddNewAccount(name, balance, bankID, date);
            }
        }

        // cancel button event
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }
        

        private void txtBalance_TextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtBalance.Text.Trim()) && !String.IsNullOrEmpty(txtName.Text.Trim()))
            {
                btnAdd.Enabled = true;
            }
            else
                btnAdd.Enabled = false;
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtBalance.Text.Trim()) && !String.IsNullOrEmpty(txtName.Text.Trim()))
            {
                btnAdd.Enabled = true;
            }
            else
                btnAdd.Enabled = false;
        }
    }
}
