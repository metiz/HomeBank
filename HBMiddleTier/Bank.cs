using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace HBMiddleTier
{
    public class Bank
    {
        //properties
        [Key]
        public int ID { get; set; }
       // public int USerID { get; set; }
        [ConcurrencyCheck]
        [MaxLength(20,ErrorMessage="Name must be no longer than 20 symbols.")]
        [MinLength(3,ErrorMessage="Name must be longer than 2 symbols.")]
        [Required]
        public string Name { get; set; }

        //navigation
        public HashSet<Account> Accounts;
        public User user { get; set; }

        //constructor 
        public Bank()
        {
            Accounts = new HashSet<Account>();
        }
    }
}
