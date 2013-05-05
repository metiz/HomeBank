using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using HomeBankModel;
using System.Linq;

namespace HBTest
{
    public partial class frmTransfer : HomeBankModel.frmDialogBase
    {
        //fields
        private int userID;
        HBContext context = new HBContext();


        //constructor
        public frmTransfer(int userID)
        {
            InitializeComponent();

            this.userID = userID;
        }


        // from load event
        private void frmTransfer_Load(object sender, EventArgs e)
        {
            // decimal filter
            txtAmount.KeyPress += Helpers.ControlHelper.DecimalTextboxKeyPressEvent;

            //turn off Confirm button
            btnConfirm.Enabled = false;

            // load accounts comboboxes

            List<GetAllAccounts_Result> allAccounts = context.GetAllAccounts(userID).ToList();

            var accountsFrom = (from a in allAccounts
                               select new
                               {
                                   Name = String.Format("{0}: {1:c}", a.Name, a.Balance),
                                   ID = a.ID
                               }).ToList();

            var accountsTo = (from a in allAccounts
                                select new
                                {
                                    Name = String.Format("{0}: {1:c}", a.Name, a.Balance),
                                    ID = a.ID
                                }).ToList();

            cboFrom.DataSource = accountsFrom;
            cboTo.DataSource = accountsTo;


        }


        // cancel event
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }
        //button confirm event
        private void btnConfirm_Click(object sender, EventArgs e){

            try
            {
                // get input data
                int accoutnFrom = (int)cboFrom.SelectedValue;
                int accountTo = (int)cboTo.SelectedValue;
                decimal amount = decimal.Parse(txtAmount.Text);
                string name = txtName.Text.Trim();
                DateTime date = dtpDate.Value;


                //create new transfer object
                Transfer trans = new Transfer()
                {
                    AccountFrom = accoutnFrom,
                    AccountTo = accountTo,
                    Amount = amount,
                    Name = name,
                    Date= date
                };


                //add transfer to database
                context.Transfers.Add(trans);
                //save changes
                context.SaveChanges();
                //dispose contex
                context.Dispose();
                //close from
                this.DialogResult = System.Windows.Forms.DialogResult.OK;
            }
            catch
            { }
        }
        // txtAmount text change avent
        private void txtAmount_TextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtAmount.Text.Trim()) && !String.IsNullOrEmpty(txtName.Text.Trim()))
            {
                btnConfirm.Enabled = true;
            }
            else
            {
                btnConfirm.Enabled = false;
            }
        }

        private void cboTo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        // txtName text changed event
        private void txtName_TextChanged(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(txtAmount.Text.Trim()) && !String.IsNullOrEmpty(txtName.Text.Trim()))
            {
                btnConfirm.Enabled = true;
            }
            else
            {
                btnConfirm.Enabled = false;
            }
        }

        
    }
}
