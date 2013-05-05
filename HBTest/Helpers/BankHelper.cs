using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HBTest.Helpers
{
    public class BankHelper
    {
        public static int AddNewBank(string Name, int userID)
        {
            using (HomeBankModel.HBContext context = new HomeBankModel.HBContext())
            {
                //if bank with this name already exists
                if (context.Banks.Where(
                    x => x.Name.ToLower() == Name.ToLower() && x.UserID == userID
                    ).Count() > 0)
                {
                    // return ID
                    return context.Banks.Where(x => x.Name.ToLower() == Name.ToLower()).First().ID;
                }
                //if does not create bank and return id
                return (int)context.AddNewBank(userID, Name).First();
            }
        }
    }
}
