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
    public partial class CreateNewUser : Form
    {
        HBContext hbContext = new HBContext();
        public CreateNewUser()
        {
            InitializeComponent();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.No;
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            


            // input validation:

            if (txtLogin.Text.Length < 4 || txtLogin.Text.Length > 12)
            {
                MessageBox.Show("Login name must be between 4 and 12 characters.");
                return;
            }

            if (txtPassword.Text.Length < 4 || txtPassword.Text.Length > 12)
            {
                MessageBox.Show("Password must be between 4 and 23 characters");
                return;
            }

            if (txtPassword.Text.Trim() != txtPassConf.Text.Trim())
            {
                MessageBox.Show("Passwords must match!");
                return;
            }

            string login = txtLogin.Text.Trim();
            string firstName = txtFirstName.Text.Trim();
            string lastName = txtLastName.Text.Trim();
            string pass = txtPassword.Text.Trim();


            string salt = txtLogin.Text.Substring(0, 3);
            byte[] saltHash = ASCIIEncoding.UTF32.GetBytes(salt);
            byte[] phash = ASCIIEncoding.UTF32.GetBytes(pass);
            byte[] hash = PasswordHasher.GenerateSaltedHash(phash, saltHash);

          


            User newUser = new User()
            {
                UserName = login,
                FirstName = firstName,
                LastName = lastName,
                Password = Convert.ToBase64String(hash)

            };
            // if user with this login already exists show message about that
            if (hbContext.Users.Where(x => x.UserName == login).Count() > 1)
            {
                hbContext.Users.Add(newUser);
                hbContext.SaveChanges();
                hbContext.Dispose();
                this.DialogResult = System.Windows.Forms.DialogResult.OK;

            }
            else
                MessageBox.Show("User with this login name already exist");

          
        }
    }
}
