using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TrackerLib;
using TrackerLib.Models;
using TrackerUI.Helper_Methods;

namespace TrackerUI
{
    public partial class CreateTournamentForm : AbstractCommon, IPrizeRequester, ITeamRequester
    {
        private List<TeamModel> availableTeams = GlobalConfig.Connection.GetAllTeams();
        private List<TeamModel> selectedTeams = new List<TeamModel>();
        private List<PrizeModel> selectedPrizes = new List<PrizeModel>();

        public CreateTournamentForm()
        {
            InitializeComponent();
        }

        private void CreateTournamentForm_Load(object sender, EventArgs e)
        {
            WiredUpLists();
        }

        public override void WiredUpLists()
        {
            selectTeamDropDown.DataSource = null;
            tournamentTeamsListBox.DataSource = null;
            prizeListBox.DataSource = null;

            selectTeamDropDown.DataSource = availableTeams;
            selectTeamDropDown.DisplayMember = "teamname";

            tournamentTeamsListBox.DataSource = selectedTeams;
            tournamentTeamsListBox.DisplayMember = "teamname";

            prizeListBox.DataSource = selectedPrizes;
            prizeListBox.DisplayMember = "placename";
        }

        private void addTeamButton_Click(object sender, EventArgs e)
        {
            TeamModel t = (TeamModel)selectTeamDropDown.SelectedItem;

            if (t != null)
            {
                availableTeams.Remove(t);
                selectedTeams.Add(t);
            }

            WiredUpLists();
        }

        private void deleteSelectedPlayerButton_Click(object sender, EventArgs e)
        {
            TeamModel team = (TeamModel)tournamentTeamsListBox.SelectedItem;

            if (team == null) return;

            availableTeams.Add(team);
            selectedTeams.Remove(team);

            WiredUpLists();
        }

        private void deleteSelectedPrizeButton_Click(object sender, EventArgs e)
        {
            PrizeModel prize = (PrizeModel)prizeListBox.SelectedItem;

            if (prize == null) return;

            selectedPrizes.Remove(prize);

            WiredUpLists();
        }

        private void createPrizeButton_Click(object sender, EventArgs e)
        {
            // call create prize form
            CreatePrizeForm frm = new CreatePrizeForm(this);
            frm.Show();
        }

        public void PrizeComplete(PrizeModel prizeModel)
        {
            // get back from the form
            //take the prizemodel and put it into out list of selected prizes
            selectedPrizes.Add(prizeModel);
            WiredUpLists();
        }

        public void TeamComplete(TeamModel team)
        {
            selectedTeams.Add(team);
            WiredUpLists();
        }

        private void createTeamLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CreateTeamForm frm = new CreateTeamForm(this);
            frm.Show();
        }

        private void createTournamentButton_Click(object sender, EventArgs e)
        {
            // TODO - Wired up click event with save to DB\file action
            // add team list and prize list to Tournament model
            if (selectedPrizes.Count == 0 || selectedTeams.Count == 0) return;

            decimal fee = decimal.Parse(entryFeeTextBox.Text);

            TournamentModel tournament = new TournamentModel
            {
                TournamentName = tournamentNameTextBox.Text,
                EntryFee = fee,
                EnteredTeams = selectedTeams,
                Prizes = selectedPrizes
            };

            // TODO - wire up match up
            TournamentLogic.CreateRounds(tournament);

            GlobalConfig.Connection.CreateTournament(tournament);
        }
    }
}
