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
    public partial class frmManageCategories : HomeBankModel.frmDialogBase
    {

        //fields
        private int userID;
        HBContext context;


        //constructor
        public frmManageCategories(int userID)
        {
            this.userID = userID;
            InitializeComponent();
        }
        // form load event
        private void frmManageCategories_Load(object sender, EventArgs e)
        {
            context = new HBContext();
            // set combo box selected index
            cboType.SelectedIndex = 0;


        }
        // main categories listBox selected index changed event
        private void lstMainCat_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                // get selected category's ID and load sub categories
                int id = (int)lstMainCat.SelectedValue;
                List<Category> cats = context.Categories.Where(x => x.BelongsTo == id).ToList();
                lstSubCat.DataSource = cats;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().Name + ". " + ex.Message);
            }



        }
        // category type comboBox selected index changed event
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                RefreshListBoxes();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetType().Name + ". " + ex.Message);
            }
        }

        private void RefreshListBoxes()
        {
            List<Category> mainCategories;
            mainCategories = context.GetAllMainCategories(userID).Where(x => x.ID > 0).ToList();
            if (cboType.SelectedIndex == 0)  // Expence
            {
                mainCategories = mainCategories.Where(x => x.Type == false).ToList();
                //turn on subs
                SubsEnabled(true);
            }
            else // incom
            {
                mainCategories = mainCategories.Where(x => x.Type == true).ToList();
                // turn off sub categories
                // because the is no subs in income
                SubsEnabled(false);
            }
            lstMainCat.DataSource = mainCategories;
        }
        // method to turn on/off subcategories
        private void SubsEnabled(bool state)
        {
            lstSubCat.Enabled = state;
            btnRemoveSub.Enabled = state;
            txtNewSub.Enabled = state;
        }
        // button remove main category click event
        private void btnRemoveMain_Click(object sender, EventArgs e)
        {
            // TO DO:
            // get selected category's id
            int id = (int)lstMainCat.SelectedValue;
            // check if there are related subcategories
            var subs = context.GetSubCategoriesByID(id).ToList();
            // promt the user
            DialogResult promt = MessageBox.Show("Selected category and all of it's subcategories will be deleted.", "Delete Category",
                   MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation);
            if (promt == System.Windows.Forms.DialogResult.OK)
            {
                // remove it from database
                context.RemoveCategoryByID(id);
                foreach (var sub in subs)
                {
                    context.RemoveCategoryByID(sub.ID);
                }
                // refresh list box
                RefreshListBoxes();
            }

        }
        // textbox new main category key press event

        private void txtNewMain_KeyDown(object sender, KeyEventArgs e)
        {
            //TO DO:
            // if Enter Key pressed
            if (e.KeyCode == Keys.Enter)
            {
                // get user input
                string newCat = txtNewMain.Text.Trim();
                // check if already exists
                if (context.Categories.Where(x => x.Name.ToLower() == newCat.ToLower()).Count() < 1)
                {
                    bool type = (cboType.SelectedIndex == 0) ? false : true;
                    // if no add new category to database
                    context.AddNewCategory(newCat, userID, type, null);
                    // refresh listbox
                    RefreshListBoxes();
                    // clear textBox
                    txtNewMain.Text = String.Empty;
                }
            }
        }
    }
}
