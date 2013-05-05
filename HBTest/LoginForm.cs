using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using HomeBankModel;

namespace HBTest
{
    public partial class LoginForm : Form
    {
        public User loginUser;
        public LoginForm(User user)
        {
            InitializeComponent();
            loginUser = user;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string userName = txtLogin.Text.ToLower().Trim();
                string pass = txtPassword.Text.Trim();
                string salt = userName.Substring(0, 3);

                byte[] passHash = PasswordHasher.GenerateSaltedHash(pass, salt);
                string hashedPassword = Convert.ToBase64String(passHash);

                using (var context = new HBContext())
                {
                    
                    if (context.GetUser(userName, hashedPassword).Count() > 0)
                        loginUser = context.GetUser(userName, hashedPassword).ToList()[0];
                    if (loginUser != null)
                        this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    else
                        MessageBox.Show("Wrong password or user name.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                MessageBox.Show("Please check your entry.");
            }
        }
     

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.No;
        }

        private void btnNewAccount_Click(object sender, EventArgs e)
        {
            CreateNewUser cnu = new CreateNewUser();
            if (cnu.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                MessageBox.Show("New user has been added.");
        }

        private void txtLogin_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        
    }
}
