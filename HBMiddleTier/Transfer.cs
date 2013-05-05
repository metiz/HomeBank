using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBMiddleTier
{
    public class Transfer
    {
        //properties

        public int ID { get; set; }
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public DateTime Date { get; set; }

        //navigation
        public Transaction IncomeTransaction { get; set; }
        public Transaction ExpenceTransaction { get; set; }
        
    }
}
