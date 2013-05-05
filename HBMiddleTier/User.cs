using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBMiddleTier
{
    public class User
    {
        // properties
        public int ID { get; set; }
        public string UserName { get; set; }
        public string FirtsName { get; set; }
        public string LastName { get; set; }
        public string Password { get; set; }

        // navigation
        public HashSet<Bank> Banks;

        //constructor
        public User()
        {
            Banks = new HashSet<Bank>();
        }
    }
}
