using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HBTest
{
    class DateFilter
    {
        public DateTimePicker StartDate { get; set; }
        public DateTimePicker EndDate { get; set; }

        public DateFilter(DateTimePicker start, DateTimePicker end)
        {
            StartDate = start;
            EndDate = end;
            
            //very end of today's day
            EndDate.MaxDate = DateTime.Now.Date.AddDays(1).AddMilliseconds(-1);

            //set Start date time picker 30 days before today
            //and max value as yesterday
            StartDate.Value = DateTime.Now.AddDays(-30);
            StartDate.MaxDate = DateTime.Now.Date.AddDays(1).AddMilliseconds(-1);          
        }

        public void SetOption(int option)
        {
            /*
             * 0. 90 Days
             * 1. 60 Days
             * 2. 30 Days
             * 3. This month
             * 4.
             */
            switch (option)
            {
                case 0:
                    StartDate.Value = DateTime.Now.Date.AddDays(-90);
                    EndDate.Value = DateTime.Now.Date.AddDays(1).AddMilliseconds(-1);
                    break;
                case 1:
                    StartDate.Value = DateTime.Now.Date.AddDays(-60);
                    EndDate.Value = DateTime.Now.Date.AddDays(1).AddMilliseconds(-1);
                    break;
                case 2:
                    StartDate.Value = DateTime.Now.Date.AddDays(-30);
                    EndDate.Value = DateTime.Now.Date.AddDays(1).AddMilliseconds(-1);
                    break;
                case 3:
                    StartDate.Value = DateTime.Now.Date.AddDays(-(DateTime.Now.Day - 1));
                    EndDate.Value = DateTime.Now.Date.AddDays(1).AddMilliseconds(-1);
                    break;
                default:
                    StartDate.Value = DateTime.Now.Date.AddDays(-30);
                    EndDate.Value = DateTime.Now.Date.AddDays(1).AddMilliseconds(-1);
                    break;
            }
        }

    }
}
