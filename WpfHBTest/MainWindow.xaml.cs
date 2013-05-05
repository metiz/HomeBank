using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using HomeBankModel;
using System.Data.Entity;

namespace WpfHBTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        HBEntities HBContext = new HBEntities();
        IEnumerable<Account> accounts;
        public MainWindow()
        {
            InitializeComponent();
            accounts = HBContext.GetAllAccounts();
            dataGridView1.DataContext = accounts;

           
            /*
            dataGridView1.Columns[0].Visible = false;
            dataGridView1.Columns[2].DefaultCellStyle.Format = "C";
            dataGridView1.Columns[3].Visible = false;
            dataGridView1.Columns[4].Visible = false;
            dataGridView1.Columns[5].Visible = false;
             * */
        }
    }
}
