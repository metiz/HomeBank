using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBMiddleTier
{
    public class Transaction
    {

        //properties
        public int ID { get; set; }
        //public int AccountID { get; set; }
        //public int CategoryID { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }

        //navigation
        public HashSet<Category> Categories;
        public Account Account { get; set; }
        public Category Category { get; set; }

        //constructor
        public Transaction()
        {
            this.Categories = new HashSet<Category>();
        }


    }
}
