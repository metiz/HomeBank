using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace HBMiddleTier
{
    public class Category
    {

        //propperties
        
        public int ID { get; set; }
        public int UserID { get; set; }
        public string Name { get; set; }
        public byte Type { get; set; }
        public int BelongsTo { get; set; }


        //navigation
        public HashSet<Transaction> Transactions;

        //constructor
        public Category()
        {
            this.Transactions = new HashSet<Transaction>();
        }
    }
}
