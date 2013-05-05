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
using System.Data.Common;
using System.Data.Entity;

namespace HBTest
{
    public partial class frmIncome : HomeBankModel.frmDialogBase
    {
        // private fields
        private int accountID;
        HBContext context;
        private int userID;

        //constructor
        public frmIncome(int accountID)
        {
            InitializeComponent();
            this.accountID = accountID;
        }

        // form load event
        private void frmIncome_Load(object sender, EventArgs e)
        {
            context = new HBContext();

            // set user ID
            Account activeAccount = context.GetAccountByID(accountID).First();
            userID = (int)activeAccount.Bank.UserID;
            cboCategory.DataSource = context.Categories.Where(x => x.ID != -201 && x.Type == true && x.UserID == userID).ToList();
            

            // add event to deny non numeric input
            txtAmount.KeyPress += Helpers.ControlHelper.DecimalTextboxKeyPressEvent;

        }



        // cancel button event
        private void btnCancel_Click(object sender, EventArgs e)
        {
            // set diaalog result to Cancel and close form
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }

        // add button event
        private void btnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                int catID;
                // bank combobox cannot be empty
                if (cboCategory.SelectedItem == null && cboCategory.Text.Trim() != String.Empty)
                {
                    //addnewcreditinst method add new bank and returns its ID
                    string newCategoryName = cboCategory.Text.Trim();
                    catID = Helpers.CategoryHelper.AddNewIncomeCategory(newCategoryName, userID);
                }
                else if (cboCategory.SelectedItem == null)
                {
                    MessageBox.Show("Please enter a valid Category");
                    return;
                }
                else
                {
                    catID = (int)cboCategory.SelectedValue;
                }

                string name = txtName.Text.Trim();
                decimal amount = decimal.Parse(txtAmount.Text.Trim());
                DateTime date = dtpDate.Value;

                context.AddNewIncome(accountID, name, amount, catID, date);

                this.DialogResult = System.Windows.Forms.DialogResult.OK;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

    }
}
