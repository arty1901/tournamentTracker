using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TrackerLib;
using TrackerLib.Models;
using TrackerUI.FormInterfaces;

namespace TrackerUI
{
    public partial class CreateTournamentForm : Form, IPrizeRequester, ITeamRequester
    {
        private List<TeamModel> _availableTeams = GlobalConfig.Connection.GetAllTeams();
        private List<TeamModel> _selectedTeams = new List<TeamModel>();
        private List<PrizeModel> _selectedPrizes = new List<PrizeModel>();

        public CreateTournamentForm(ITournamentRequester form)
        {
            InitializeComponent();
        }

        private void CreateTournamentForm_Load(object sender, EventArgs e)
        {
            WiredUpLists();
        }

        public void WiredUpLists()
        {
            selectTeamDropDown.DataSource = null;
            tournamentTeamsListBox.DataSource = null;
            prizeListBox.DataSource = null;

            selectTeamDropDown.DataSource = _availableTeams;
            selectTeamDropDown.DisplayMember = "teamname";

            tournamentTeamsListBox.DataSource = _selectedTeams;
            tournamentTeamsListBox.DisplayMember = "teamname";

            prizeListBox.DataSource = _selectedPrizes;
            prizeListBox.DisplayMember = "placename";
        }

        private void addTeamButton_Click(object sender, EventArgs e)
        {
            TeamModel t = (TeamModel)selectTeamDropDown.SelectedItem;

            if (t != null)
            {
                _availableTeams.Remove(t);
                _selectedTeams.Add(t);
            }

            WiredUpLists();
        }

        private void deleteSelectedPlayerButton_Click(object sender, EventArgs e)
        {
            TeamModel team = (TeamModel)tournamentTeamsListBox.SelectedItem;

            if (team == null) return;

            _availableTeams.Add(team);
            _selectedTeams.Remove(team);

            WiredUpLists();
        }

        private void deleteSelectedPrizeButton_Click(object sender, EventArgs e)
        {
            PrizeModel prize = (PrizeModel)prizeListBox.SelectedItem;

            if (prize == null) return;

            _selectedPrizes.Remove(prize);

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
            _selectedPrizes.Add(prizeModel);
            WiredUpLists();
        }

        public void TeamComplete(TeamModel team)
        {
            _selectedTeams.Add(team);
            WiredUpLists();
        }

        private void createTeamLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            CreateTeamForm frm = new CreateTeamForm(this);
            frm.Show();
        }

        private void createTournamentButton_Click(object sender, EventArgs e)
        {
            if (_selectedPrizes.Count == 0 || _selectedTeams.Count == 0) return;

            decimal fee = decimal.Parse(entryFeeTextBox.Text);

            TournamentModel tournament = new TournamentModel
            {
                TournamentName = tournamentNameTextBox.Text,
                EntryFee = fee,
                EnteredTeams = _selectedTeams,
                Prizes = _selectedPrizes
            };

            // TODO - wire up match up
            TournamentLogic.CreateRounds(tournament);

            GlobalConfig.Connection.CreateTournament(tournament);

            this.Close();
        }
    }
}
