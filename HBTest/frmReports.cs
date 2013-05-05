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
using System.Windows.Forms.DataVisualization.Charting;

namespace HBTest
{
    public partial class frmReports : HomeBankModel.frmDialogBase
    {
        private int userID;
        public frmReports(int userID)
        {
            this.userID = userID;
            InitializeComponent();
        }

        private void frmReports_Load(object sender, EventArgs e)
        {
            //data
            
            HBContext context = new HBContext();
            var accounts = context.GetAllAccounts(userID);

            chart1.DataSource = accounts;
            chart1.Series[0].XValueMember = "Name";
            chart1.Series[0].YValueMembers = "Balance";
           


            //set title
            this.chart1.Titles.Add("Accounts");
            //set palette
            this.chart1.Palette = ChartColorPalette.Berry;

            
            /*
            // add series
            for (int i = 0; i < accounts.Count; i++)
            {
                this.chart1.Series.Add(new Series(accounts[i].Name.ToString()));
                //add point
               
                this.chart1.Series[i].Points.AddY(accounts[i].Balance);
                this.chart1.Series[i].Legend = accounts[i].Balance.ToString("c");
               
            }
            
            
           */
        }
    }
}
