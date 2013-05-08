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
    public partial class MainForm : Form
    {
        // Initialize variables
        // user as current user

        HBContext contextGlob = new HBContext();
        private User user = null;
        IEnumerable<GetAllAccounts_Result> accounts;
        // datefilter object to keep track manage date filter
        DateFilter filter;
        //constructor
        public MainForm()
        {
            // call the login form before main form starts
            using (LoginForm loginF = new LoginForm(user))
            {

                if (loginF.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    user = loginF.loginUser;
                else
                {   // if login form result not OK exit the application
                    Environment.Exit(0);
                }
                if (user == null) // just in case if user still not defined exit the application
                    Environment.Exit(0);
            }

            InitializeComponent();
            //greetings
            label1.Text = "Hi there, " + user.FirstName + "! Here is your accounts summary: ";

            using (HBContext context = new HBContext())
            {
                accounts = context.GetAllAccounts(user.ID);
                dgvAccounts.DataSource = accounts;
            }
            //call method to set only two columns visible
            AccountsDGViewSettings();
            dgvAccounts.RowEnter += dataGridView1RowEnter;


            // create date filter to manage dateTimePickers and combobox
            filter = new DateFilter(dtpStartDate, dtpEndDate);
            // add an events to dateTimePickers
            filter.EndDate.ValueChanged += dtpValueChanged;
            filter.StartDate.ValueChanged += dtpValueChanged;

        }

        // form load event
        private void MainForm_Load(object sender, EventArgs e)
        {
            comboBox1.SelectedIndex = 0;

            //create event to select row when rigth click performed, before context menu
            dgvAccounts.MouseDown += dgvAccounts_MouseDown;
            this.deleteToolStripMenuItem.Click += deleteToolStripMenuItem_Click;
            // display total balance
            RefreshTotal();


            SetStyles();
        }

        private void SetStyles()
        {
            SetTransGridStyle(dgvTransactions);
            SetTransGridStyle(dgvExpences);
            SetTransGridStyle(dgvIncome);
            // copy rowTemplate to all gridViews
            if (dgvTransactions.RowTemplate != dgvAccounts.RowTemplate)
            {
                dgvTransactions.RowTemplate = dgvAccounts.RowTemplate;
                dgvExpences.RowTemplate = dgvAccounts.RowTemplate;
                dgvIncome.RowTemplate = dgvAccounts.RowTemplate;
            }
        }

        private void RefreshTotal()
        {
            // display total balance
            using (HBContext context = new HBContext())
            {
                //check if user have accounts already
                if (context.GetAllAccounts(user.ID).Count() > 0)
                {
                    var total = context.Accounts.Where(x => x.Bank.UserID == user.ID).Sum(x => x.Balance);
                    lblTotal.Text = String.Format("Total balance is : {0:c}", total);
                }
                else
                    lblTotal.Text = "";
            }
        }
        // right mouse click event on accounts DGV
        void dgvAccounts_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var hti = dgvAccounts.HitTest(e.X, e.Y);
                dgvAccounts.ClearSelection();
                dgvAccounts.Rows[hti.RowIndex].Selected = true;
                selectedAccoutId = (int)dgvAccounts["ID", hti.RowIndex].Value;
            }
        }
        // method to set only two columns visible
        private void AccountsDGViewSettings()
        {
            foreach (DataGridViewColumn column in dgvAccounts.Columns)
            {
                column.Visible = (column.Name == "Name" || column.Name == "Balance") ? true : false;
            }
            dgvAccounts.Columns["Name"].DisplayIndex = 0;
            dgvAccounts.Columns["Balance"].DefaultCellStyle.Format = "C";


        }


        // dgvAccounts Row Enter Event
        int selectedAccoutId = -100;
        private void dataGridView1RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            selectedAccoutId = int.Parse(dgvAccounts[0, e.RowIndex].Value.ToString());
            FillTransactionsDGV(selectedAccoutId);

            SetStyles();
        }

        private void SetTransGridStyle(DataGridView dgv)
        {
            foreach (DataGridViewRow row in dgv.Rows)
            {

                DataGridViewCell cell = row.Cells["Amount"];
                double amount = double.Parse(cell.Value.ToString());
                cell.Style.Format = "c";

                cell.Style.ForeColor = (amount < 0) ? Color.FromArgb(200, 0, 0) : Color.FromArgb(0, 255, 0);
                DataGridViewCell DateCell = row.Cells["Date"];
                DateCell.Style.Format = "d";
            }



        }

        private void FillTransactionsDGV(int selectedAccoutId)
        {
            if (selectedAccoutId != -100)
                using (HBContext context = new HBContext())
                {
                    // all transactions list

                    List<GetAllTransForAccountAndDate_Result> transactions;
                    transactions = context.GetAllTransForAccountAndDate(selectedAccoutId, dtpStartDate.Value, dtpEndDate.Value).ToList();


                    // income transactions list
                    List<GetAllTransForAccountAndDate_Result> incomeTrans;
                    incomeTrans = transactions.Where(x => x.Amount > 0).ToList();

                    //expences list
                    List<GetAllTransForAccountAndDate_Result> expenceList;
                    expenceList = transactions.Where(x => x.Amount < 0).ToList();

                    if (dgvAccounts.SelectedRows.Count <= 0)
                    {
                        dgvTransactions.DataSource = null;
                        dgvIncome.DataSource = null;
                        dgvExpences.DataSource = null;
                    }
                    else
                    {
                        dgvTransactions.DataSource = transactions;
                        dgvIncome.DataSource = incomeTrans;
                        dgvExpences.DataSource = expenceList;
                        // hide ID column                        

                        HideColumns(dgvTransactions, new string[] { "ID" });
                    }
                }

        }

        //method to display only necessary columns
        private void HideColumns(DataGridView dgv, string[] columns)
        {
            foreach (DataGridViewColumn col in dgv.Columns)
            {
                if (!columns.Contains(col.Name))
                {
                    col.Visible = true;
                }
                else
                {
                    col.Visible = false;
                }
            }
        }

        private void newExpenceListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AddNewExpForm nef;
            Account acc;
            using (HBContext context = new HBContext())
            {
                int selectedAccountID;
                int.TryParse(dgvAccounts.SelectedRows[0].Cells[0].Value.ToString(), out selectedAccountID);

                acc = context.GetAccountByID(selectedAccountID).ToList()[0];


            }

            nef = new AddNewExpForm(acc);
            if (nef.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                MessageBox.Show("OK");
            RefreshAccounts();

        }

        // method to reload account data
        private void RefreshAccounts()
        {
            using (HBContext context = new HBContext())
            {
                accounts = context.GetAllAccounts(user.ID);

                dgvAccounts.DataSource = accounts;
            }

            RefreshTotal();
            FillTransactionsDGV(selectedAccoutId);
            SetStyles();
        }

        private void dtpValueChanged(object sender, EventArgs e)
        {
            FillTransactionsDGV(selectedAccoutId);
        }
        //add new account event
        private void addNewAccountToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddNewAccount na = new frmAddNewAccount(user.ID);
            if (na.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                MessageBox.Show("OK");
                RefreshAccounts();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            filter.SetOption(comboBox1.SelectedIndex);
        }


        // edit acccount event
        private void editToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //account by ID
            Account activeAccount;
            using (HBContext context = new HBContext())
            {
                activeAccount = context.GetAccountByID(selectedAccoutId).First();
            }
            //initialize new edit account form with passing the active account
            if (activeAccount != null)
            {
                frmEditAccount frmEA = new frmEditAccount(activeAccount);
                //call EditAccount form, if result OK refresh accounts 
                if (frmEA.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    RefreshAccounts();
            }
        }
        // delete account event
        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //get the active account
            using (HBContext context = new HBContext())
            {
                Account acc = context.Accounts.Where(x => x.ID == selectedAccoutId).First();

                // message box
                DialogResult promt = MessageBox.Show(String.Format("Account {0} at {1} and all related transactions will be deleted!",
                    acc.Name, acc.Bank.Name), "Delete Account",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);

                if (promt == System.Windows.Forms.DialogResult.Cancel)
                    return;
                else
                {
                    // remove account and show a message
                    context.RemoveAccountByID(acc.ID);
                    MessageBox.Show("Account has been deleted.");
                    RefreshAccounts();
                }
            }
        }
        // remove selected transactions event
        private void removeSelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //TO DO: get all selected transactions
            // promt user and delete them

            DialogResult promt = MessageBox.Show("Seleted Transactions will be deleted.", "Delete Transactions",
                    MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);

            using (HBContext context = new HBContext())
            {

                switch (tabControl1.SelectedIndex)
                {

                    case 0:
                        // all transactions tab selected                        
                        removeTrans(promt, context);
                        break;
                    case 1:
                        // income tab selected 
                        removeTrans(promt, context);
                        break;
                    case 2:
                        // expenses tab selected
                        removeTrans(promt, context);
                        break;
                    default:
                        break;
                }
                

            }
        }

        private void removeTrans(DialogResult promt, HBContext context)
        {
            int ID =0;
            if (promt == System.Windows.Forms.DialogResult.OK)
            {
                foreach (DataGridViewRow row in dgvTransactions.SelectedRows)
                {

                    ID = (int)row.Cells["ID"].Value;
                    context.RemoveTransByID(ID);

                }
                MessageBox.Show("Selected transaction has been deleted.");
                RefreshAccounts();
            }
        }
        // add new income
        private void addIncomeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmIncome fIncome = new frmIncome(selectedAccoutId);

            if (fIncome.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                MessageBox.Show("OK");
                RefreshAccounts();
            }
        }
        // new transfer event
        private void transferToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
            frmTransfer fTrans = new frmTransfer(user.ID);
            if (fTrans.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                MessageBox.Show("Transfer has been added.");
                RefreshAccounts();
            }
        }
        // about box
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new aboutBox().ShowDialog();
        }

        private void reportWindowToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //TEST
            frmReports rep = new frmReports(user.ID);
            rep.Show();
        }
        // manage categories toolStripMenu event
        private void manageCategoriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //create new form object
            frmManageCategories frmManCat = new frmManageCategories(user.ID);
            //show dialog
            frmManCat.ShowDialog();
        }





    }

}
