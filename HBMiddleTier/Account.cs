using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBMiddleTier
{
    public class Account
    {
        //properties
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastModDate { get; set; }

        //navigation
        public Bank Bank { get; set; }
        public HashSet<Transaction> Transactions;

        //constructor
        public Account()
        {
            Transactions = new HashSet<Transaction>();
        }

    }
}
