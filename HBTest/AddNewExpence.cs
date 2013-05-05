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

namespace HBTest
{
    public partial class AddNewExpForm : HomeBankModel.frmDialogBase
    {
        List<Transaction> expenceList;
        private Account activeAcccount;
        int userID;

        //constructor
        public AddNewExpForm(Account account)
        {
            InitializeComponent();
            this.activeAcccount = account;

        }
        //form load event
        private void AddNewExpForm_Load(object sender, EventArgs e)
        {
            expenceList = new List<Transaction>();
            List<Category> categoriesList;

            using (HBContext context = new HBContext())
            {

                //load userID
                userID = (int)context.Accounts.Where(x => x.ID == activeAcccount.ID).First().Bank.UserID;


                categoriesList = context.Categories.Where(x => x.Type == false && x.ID != -200 && x.UserID == userID && x.BelongsTo == null).ToList();

                var categories = from category in categoriesList
                                 where category.Type == false
                                 select new { ID = category.ID, Name = category.Name };

                cboCategory.DataSource = categories.ToList();

                //autocompleate categories

                cboCategory.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                cboSubCategory.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
                cboCategory.AutoCompleteSource = AutoCompleteSource.ListItems;
                cboCategory.AutoCompleteSource = AutoCompleteSource.ListItems;

            }


            //add event to deny nnon numeric input
            txtAmount.KeyPress += Helpers.ControlHelper.DecimalTextboxKeyPressEvent;
        }

        //button add event
        private void brnAdd_Click(object sender, EventArgs e)
        {
            try
            {
                int catID; // categoryID
                // new main catogory entered
                if (cboCategory.SelectedValue == null && !String.IsNullOrEmpty(cboCategory.Text.Trim()))
                {
                    // sub category was entered
                    if (!String.IsNullOrEmpty(cboSubCategory.Text.Trim()))
                    {
                        catID = Helpers.CategoryHelper.AddNewExpenceSubcategory(cboSubCategory.Text.Trim(), userID, categoryID);
                    }
                    else
                    {
                        catID = Helpers.CategoryHelper.AddNewExpenceCategory(cboCategory.Text.Trim(), userID);
                    }
                }
                // existing category select
                else if (cboCategory.SelectedValue != null)
                {
                    // new subcatory entered
                    if (cboSubCategory.SelectedValue == null && !String.IsNullOrEmpty(cboSubCategory.Text.Trim()))
                    {
                        catID = Helpers.CategoryHelper.AddNewExpenceSubcategory(cboSubCategory.Text.Trim(), userID, categoryID);
                    }
                    // existing subCategory selected
                    else if (cboSubCategory.SelectedValue != null)
                    {
                        catID = (int)cboSubCategory.SelectedValue;
                    }
                    // subCategory is empty
                    else
                    {
                        catID = categoryID;
                    }
                }
                else
                {
                    catID = categoryID;
                }





                string name = txtName.Text.Trim();

                string description = txtDescription.Text.Trim();
                decimal amount = decimal.Parse(txtAmount.Text);
                DateTime date = dtpDate.Value;

                Transaction trans = new Transaction()
                {
                    AccountID = activeAcccount.ID,
                    Amount = amount,
                    CategoryID = catID,
                    Date = date,
                    Description = description,
                    Name = name
                };
                expenceList.Add(trans);

                lstExpencePool.Items.Add(string.Format("{0} \t:\t {1:c}", trans.Name, trans.Amount));
                txtName.Text = "";
                txtAmount.Text = "";
                txtDescription.Text = "";
                dtpDate.Value = DateTime.Now;
                cboCategory.SelectedIndex = 0;
                // if SubCategory dataSource is empty don't reset selectedIndex
                if (cboSubCategory.DataSource != null)
                    cboSubCategory.SelectedIndex = 0;

            }
            catch
            {
                MessageBox.Show("Oops.. something wrong, sorry.");
            }

        }

        // main category changed event
        int categoryID;
        private void cboCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            categoryID = (int)cboCategory.SelectedValue;

            // if nothing is selected in main category, turn off subccategory
            if (String.IsNullOrEmpty(cboCategory.Text.Trim()))
            {
                cboSubCategory.Enabled = false;
            }
            else
            {
                cboSubCategory.Enabled = true;
            }


            //List<Category> subCategoriesList;
            //using (HBContext context = new HBContext())
            //{
            //    int selectedMainCategoryID;
            //    int.TryParse(cboCategory.SelectedValue.ToString(), out selectedMainCategoryID);
            //    subCategoriesList = context.GetSubCategoriesByID(selectedMainCategoryID).ToList();
            //}

            //if (subCategoriesList.Count > 0)
            //{
            //    var subCategoriesFilter = from category in subCategoriesList
            //                              select new { ID = category.ID, Name = category.Name };
            //    cboSubCategory.DataSource = subCategoriesFilter.ToList();
            //}
            //else
            //{
            //    // no sub categories for this main category
            //    cboSubCategory.DataSource = null;
            //    //=====================FIX FIX FIX ==========================================
            //    /*
            //    if (cboCategory.SelectedItem == null && cboCategory.Text.Trim() != String.Empty)
            //    {
            //        //addnewcreditinst method add new bank and returns its ID
            //        categoryID = Helpers.CategoryHelper.AddNewCategory(cboCategory.Text.Trim(), userID);
            //    }
            //    else if (cboCategory.SelectedItem == null)
            //    {
            //        MessageBox.Show("Please enter a valid Bank");
            //        return;
            //    }
            //    else
            //    {
            //        categoryID = (int)cboCategory.SelectedValue;
            //    }*/
            //    //==========================================================================
            //}

        }
        //button remove expence click event
        private void btnRemove_Click(object sender, EventArgs e)
        {
            RemoveExpenceFromPool();
        }
        // method to remove expense from expense listbox
        private void RemoveExpenceFromPool()
        {
            if (lstExpencePool.SelectedIndex > -1)
            {
                int index = lstExpencePool.SelectedIndex;
                expenceList.RemoveAt(index);
                lstExpencePool.Items.RemoveAt(index);
            }
        }
        //button cancel click event
        private void btnCancel_Click(object sender, EventArgs e)
        {
            // set dialog result to Cancel. close the form
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }
        // submit button event
        private void btnSubmit_Click(object sender, EventArgs e)
        {
            if (expenceList.Count > 0)
                using (HBContext context = new HBContext())
                {
                    foreach (Transaction trans in expenceList)
                    {
                        context.AddNewExpence(trans.AccountID, trans.Name, trans.Amount, trans.CategoryID, trans.Date);
                    }
                }

            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
        //edit button click event
        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (lstExpencePool.SelectedIndex >= 0)
            {
                Transaction exp = expenceList[lstExpencePool.SelectedIndex];
                txtName.Text = exp.Name;
                txtAmount.Text = exp.Amount.ToString();
                txtDescription.Text = exp.Description;
                dtpDate.Value = exp.Date;

                using (HBContext context = new HBContext())
                {
                    Category subCatgory = context.GetAllCategories().ToList().Where(x => x.ID == exp.CategoryID).First();
                    Category mainCategory = context.GetAllCategories().ToList().Where(x => x.ID == subCatgory.BelongsTo).First();

                    cboCategory.SelectedValue = mainCategory.ID;
                    cboSubCategory.SelectedValue = subCatgory.ID;
                }


                RemoveExpenceFromPool();
            }
        }

        private void cboSubCategory_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        // event when a user enter a key to main category
        private void cboCategory_TextChanged(object sender, EventArgs e)
        {
            // cboSubCategory.DataSource = null;
        }

        // main category combobox no longer active event
        private void cboCategory_Leave(object sender, EventArgs e)
        {
            if (cboCategory.SelectedItem == null && cboCategory.Text.Trim() != String.Empty)
            {
                //addnewcreditinst method add new bank and returns its ID
                categoryID = Helpers.CategoryHelper.AddNewExpenceCategory(cboCategory.Text.Trim(), userID);
            }
            else if (cboCategory.SelectedItem == null)
            {
                MessageBox.Show("Please enter a valid category name");
                return;
            }
            else
            {
                categoryID = (int)cboCategory.SelectedValue;
            }


            //load the subCategory combobox

            using (HBContext context = new HBContext())
            {
                var subCategories = context.GetSubCategoriesByID(categoryID);
                List<SubCat> result = (from sc in subCategories
                                       select new SubCat() { ID = sc.ID, Name = sc.Name }).ToList();
                if (result.Count() > 0)
                {


                    cboSubCategory.DataSource = result;
                }
            }

        }


    }

    class SubCat
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
