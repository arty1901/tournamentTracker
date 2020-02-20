using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TrackerLib;
using TrackerLib.Models;
using TrackerUI.FormInterfaces;

namespace TrackerUI
{
    public partial class TournamentDashboardForm : Form, ITournamentRequester
    {
        private readonly List<TournamentModel> _availableTournamentModels = GlobalConfig.Connection.GetAllTournaments();

        public TournamentDashboardForm()
        {
            InitializeComponent();
            WiredUpLists();
        }

        public void WiredUpLists()
        {
            selectTournamentDropDown.DataSource = null;

            selectTournamentDropDown.DataSource = _availableTournamentModels;
            selectTournamentDropDown.DisplayMember = "tournamentName";
        }

        private void LoadTournamentButton_Click(object sender, EventArgs e)
        {

        }

        private void createTournamentButton_Click(object sender, EventArgs e)
        {
            CreateTournamentForm form  = new CreateTournamentForm(this);
            form.Show();
        }

        public void TournamentComplete(TournamentModel model)
        {
            _availableTournamentModels.Add(model);
            WiredUpLists();
        }
    }
}
