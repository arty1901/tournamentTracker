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
        private List<PersonModel> availableTeamMembers = GlobalConfig.Connection.GetAllPersons();
        private List<PersonModel> selectedTeamMembers = new List<PersonModel>();

        public CreateTeamForm()
        {
            InitializeComponent();

            WiredUpLists();
        }

        private void WiredUpLists()
        {
            selectTeamMemberDropDown.DataSource = null;

            selectTeamMemberDropDown.DataSource = availableTeamMembers;
            selectTeamMemberDropDown.DisplayMember = "FullName";

            teamMembersListBox.DataSource = null;

            teamMembersListBox.DataSource = selectedTeamMembers;
            teamMembersListBox.DisplayMember = "FullName";
        }

        private void createMemberButton_Click(object sender, EventArgs e)
        {
            if (ValidateForm())
            {
                PersonModel p = new PersonModel();

                p.Firstname = firstNameTextBox.Text;
                p.LastName = lastNameTextBox.Text;
                p.EmailAddress = emailTextBox.Text;
                p.Phone = phoneTextBox.Text;

                p = GlobalConfig.Connection.CreatePerson(p);

                selectedTeamMembers.Add(p);

                WiredUpLists();

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

        private void CreateTeamForm_Load(object sender, EventArgs e)
        {

        }

        private void addTeamMemberButton_Click(object sender, EventArgs e)
        {
            PersonModel p = (PersonModel)selectTeamMemberDropDown.SelectedItem;

            if (p != null)
            {
                availableTeamMembers.Remove(p);
                selectedTeamMembers.Add(p);

                WiredUpLists();
            }
        }

        private void deleteSelectedPlayerButton_Click(object sender, EventArgs e)
        {
            PersonModel p = (PersonModel)teamMembersListBox.SelectedItem;
            if (p != null)
            {
                selectedTeamMembers.Remove(p);
                availableTeamMembers.Add(p);

                WiredUpLists();
            }
        }

        private void createTeamButton_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(createTeamValueTextBox.Text) || teamMembersListBox.Items.Count == 0)
            {
                MessageBox.Show("Team name or team member list cannot be empty");
                return;
            }

            TeamModel team = new TeamModel();

            team.TeamName = createTeamValueTextBox.Text;
            team.TeamMembers = selectedTeamMembers;

            GlobalConfig.Connection.CreateTeam(team);
        }
    }
}
