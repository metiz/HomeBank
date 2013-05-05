using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HBTest.Helpers
{
    public static class ControlHelper
    {
        // event to deny enter any non numeric symbols in textbox
        public static void DecimalTextboxKeyPressEvent(object sender, KeyPressEventArgs e)
        {

            TextBox textbox = (TextBox)sender;

            if (e.KeyChar == '.')
            {
                if (textbox.Text.Contains('.') || textbox.Text == string.Empty)
                {
                    e.Handled = true;
                    return;
                }
                else
                {
                    e.Handled = false;
                    return;
                }
            }
            // check for negative sign
            if (e.KeyChar == '-')
            {
                if (textbox.Text.Length > 0)
                {
                    e.Handled = true;
                    return;
                }
                else
                {
                    e.Handled = false;
                    return;
                }
            }
            //check for backspace
            byte n;
            if (!byte.TryParse(e.KeyChar.ToString(), out n))
            {
                if (e.KeyChar != 8)
                    e.Handled = true;
            }
        }
    }
}
