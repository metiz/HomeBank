using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace HBMiddleTier
{
    class HBContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Bank> Banks { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Transfer> Transfers { get; set; }

        //constructor
        public HBContext() : base("HomeBank")
        {

        }
    }
}
