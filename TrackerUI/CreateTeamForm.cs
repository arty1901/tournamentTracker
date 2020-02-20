using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using TrackerLib;
using TrackerLib.Models;
using TrackerUI.FormInterfaces;
using TrackerUI.Helper_Methods;

namespace TrackerUI
{
    public partial class CreateTeamForm : AbstractCommon
    {
        private List<PersonModel> availableTeamMembers = GlobalConfig.Connection.GetAllPersons();
        private List<PersonModel> selectedTeamMembers = new List<PersonModel>();
        private ITeamRequester callingForm;

        public CreateTeamForm(ITeamRequester team)
        {
            InitializeComponent();
            callingForm = team;
            WiredUpLists();
        }

        override public void WiredUpLists()
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
            if (ValidateNewMemberForm())
            {
                PersonModel p = new PersonModel();

                p.FirstName = firstNameTextBox.Text;
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
        }

        private bool ValidateNewMemberForm()
        {
            if (firstNameTextBox.Text.Length == 0)
            {
                Helper.ShowMessage("First Name field can not be empty", true);
                return false;
            }

            if (lastNameTextBox.Text.Length == 0)
            {
                Helper.ShowMessage("Last Name field can not be empty", true);
                return false;
            }

            if (emailTextBox.Text.Length == 0)
            {
                Helper.ShowMessage("Email field can not be empty", true);
                return false;
            }

            if (!Helper.ValidateEmail(emailTextBox.Text))
            {
                Helper.ShowMessage("Typed invalid email", true);
                return false;
            }

            if (phoneTextBox.Text.Length == 0)
            {
                Helper.ShowMessage("Phone Number field can not be empty", true);
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

            createTeamButton.Enabled = createTeamValueTextBox.TextLength > 0 && selectedTeamMembers.Count > 0;
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

            callingForm.TeamComplete(team);

            this.Close();
        }

        private void phoneTextBox_TextChanged(object sender, EventArgs e)
        {
            string pattern = @"[^0-9]";
            Regex regex = new Regex(pattern);

            if (regex.IsMatch(phoneTextBox.Text))
            {
                phoneTextBox.Text = phoneTextBox.Text.Remove(phoneTextBox.Text.Length - 1);
                phoneTextBox.SelectionStart = phoneTextBox.Text.Length;
            }
        }

        private void teamMembersListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            deleteSelectedPlayerButton.Enabled = teamMembersListBox.SelectedIndex >= 0;
        }

        private void createTeamValueTextBox_TextChanged(object sender, EventArgs e)
        {
            createTeamButton.Enabled = createTeamValueTextBox.Text.Length > 0 && selectedTeamMembers.Count > 0;
        }
    }
}
