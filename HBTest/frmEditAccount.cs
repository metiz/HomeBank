using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HomeBankModel;
using System.Collections;

namespace HBTest
{
    public partial class frmEditAccount : Form
    {
        //initialize account object, wich is going to be modified
        private Account activeAccount;
        HBContext context = new HBContext();
        private int userID;
        private int bankID;


        //constructor
        public frmEditAccount(Account account)
        {
            InitializeComponent();
            activeAccount = context.Accounts.Find(new object[] { account.ID });
            
        }
        // form load event
        private void frmEditAccount_Load(object sender, EventArgs e)
        {
            //load account info
            txtName.Text = activeAccount.Name;
            //load bank combobox

            userID = (int)context.GetUserIDByAccountID(activeAccount.ID).First();
            cboBank.DataSource = context.GetAllBanks(userID);
            
            //autoccomlete combobox
            cboBank.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            cboBank.AutoCompleteSource = AutoCompleteSource.ListItems;

            //set the current bank
            cboBank.SelectedValue = activeAccount.CreditInst;
          






        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // Set dialog result to Cancel
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            //TO DO: remove all transaction from database witch are in transactionsToDelete list,
            // save changes to database and set dialogResult as OK

            // validate account name
            if (String.IsNullOrEmpty(txtName.Text.Trim()))
            {
                MessageBox.Show("Account name cannot be empty.");
                return;
            }

            if (txtName.Text.Length > 25)
            {
                MessageBox.Show("Account name is too long");
                return;
            }
            // validate bank
            if (String.IsNullOrEmpty(cboBank.Text))
            {
                MessageBox.Show("Bank name cannot be empty");
                return;
            }

            if (cboBank.Text.Length > 12)
            {
                MessageBox.Show("Bank name is too long");
                return;
            }

            // bank combobox cannot be empty
            if (cboBank.SelectedItem == null && cboBank.Text.Trim() != String.Empty)
            {
                //addnewcreditinst method add new bank and returns its ID
                bankID = Helpers.BankHelper.AddNewBank(cboBank.Text.Trim(), userID);
            }
            else if (cboBank.SelectedItem == null)
            {
                MessageBox.Show("Please enter a valid Bank");
                return;
            }
            else
            {
                bankID = (int)cboBank.SelectedValue;
            }



            activeAccount.Name = txtName.Text.Trim();
            context.Entry(activeAccount).Property(x => x.Name).IsModified = true;
            activeAccount.CreditInst = (cboBank.SelectedValue != null) ? (int)cboBank.SelectedValue : bankID;
            context.Entry(activeAccount).Property(x => x.CreditInst).IsModified = true;
            context.SaveChanges();
            // close form and send OK to main form
            this.DialogResult = System.Windows.Forms.DialogResult.OK;

        }


    }


}
