using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.SqlServer.Management.Smo;
using Microsoft.SqlServer.Management.Common;
using System.Data.Common;
using System.Configuration;

namespace HBTest
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["HBContextTest"].ToString()))
            {
                try
                {
                    con.Open();

                }
                catch (Exception)
                {
                    CreateDatabase(); 
                }
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        private static void CreateDatabase()
        {
            try
            {
                string connectionString = "Data Source=(local);Integrated Security=True";

                string script = Script.script;
                SqlConnection con = new SqlConnection(connectionString);
                Server server = new Server(new ServerConnection(con));
                server.ConnectionContext.ExecuteNonQuery(script);
                Console.WriteLine("Database has been created.");
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.GetType().Name + "\n" + e.Message);
                Console.ReadLine();
            }
        }
    }
}
