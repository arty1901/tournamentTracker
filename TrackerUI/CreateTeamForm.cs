using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrackerLib;
using TrackerLib.Models;

namespace TrackerUI
{
    public partial class CreateTeamForm : Form
    {
        public CreateTeamForm()
        {
            InitializeComponent();
        }

        private void createMemberButton_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                PersonModel p = new PersonModel();

                p.Firstname = firstNameTextBox.Text;
                p.LastName = lastNameTextBox.Text;
                p.Email = emailTextBox.Text;
                p.Phone = phoneTextBox.Text;

                GlobalConfig.Connection.CreatePerson(p);

                firstNameTextBox.Text = "";
                lastNameTextBox.Text = "";
                emailTextBox.Text = "";
                phoneTextBox.Text = "";
            }
            else
            {
                MessageBox.Show("you need fill all fields", "error Input", MessageBoxButtons.OK);
            }
        }

        private bool ValidateForm()
        {
            // TODO - add validation to the form
            if (firstNameTextBox.Text.Length == 0)
            {
                return false;
            }

            if (lastNameTextBox.Text.Length == 0)
            {
                return false;
            }

            if (emailTextBox.Text.Length == 0)
            {
                return false;
            }

            if (phoneTextBox.Text.Length == 0)
            {
                return false;
            }
            return true;
        }
    }
}
