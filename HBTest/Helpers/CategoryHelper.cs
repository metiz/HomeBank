using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeBankModel;

namespace HBTest.Helpers
{
    class CategoryHelper
    {
        //method to add new category to database and return it ID
        public static int AddNewExpenceCategory(string Name, int userID)
        {
            using (HBContext context = new HBContext())
            {
                // if category already exists return it ID
                if (context.Categories.Where(
                    x => x.Name.ToLower() == Name.ToLower() && x.UserID == userID
                    ).Count() > 0)
                {
                    return context.Categories.Where(x => x.Name.ToLower() == Name.ToLower() && x.UserID == userID).First().ID;
                }
                // add new category and return it ID
                // false means expense 
                // null = this is the main category 
                return (int)context.AddNewCategory(Name,userID, false,null).First();
            }
        }

        public static int AddNewExpenceSubcategory(string Name,int userID, int mainCategoryID)
        {
            
            using (HBContext context = new HBContext())
            {
                // if SubCategory already exist return it ID
                // variable to hold number of entitiees with this name and mani category
                int count = context.Categories.Where(x => x.Name.ToLower() == Name.ToLower() && x.BelongsTo == mainCategoryID).Count();
                if (count > 0)
                {
                    return context.Categories.Where(x => x.Name.ToLower() == Name.ToLower() && x.BelongsTo == mainCategoryID).First().ID;
                }

                return (int)context.AddNewCategory(Name, userID, false, mainCategoryID).First();
            }
        }

        public static int AddNewIncomeCategory(string Name, int userID)
        {
            using (HBContext context = new HBContext())
            {
                // if category already exists return it ID
                if (context.Categories.Where(
                    x => x.Name.ToLower() == Name.ToLower() && x.UserID == userID
                    ).Count() > 0)
                {
                    return context.Categories.Where(x => x.Name.ToLower() == Name.ToLower() && x.UserID == userID).First().ID;
                }
                // add new category and return it ID
                // false means expense 
                // null = this is the main category 
                return (int)context.AddNewCategory(Name, userID, true, null).First();
            }
        }
    }
}
