using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrackerUI.Helper_Methods
{
    static class Helper
    {
        static public bool ValidateEmail(string email)
        {
            try
            {
                MailAddress validate = new MailAddress(email);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        static public void ShowMessage(string message, bool isError)
        {
            if (isError)
                MessageBox.Show(message, "Error", MessageBoxButtons.OKCancel);
            else
                MessageBox.Show(message, "Info", MessageBoxButtons.OK);
        }
    }
}
