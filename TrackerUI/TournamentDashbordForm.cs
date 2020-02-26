using System;
using System.Collections.Generic;
using System.Windows.Forms;
using TrackerLib;
using TrackerLib.Models;
using TrackerUI.FormInterfaces;
using TrackerUI.Helper_Methods;

namespace TrackerUI
{
    //todo - create GetAllTournaments for text connection
    //todo - crete loadTournament event handler

    public partial class TournamentDashboardForm : Form, ITournamentRequester
    {
        private readonly List<TournamentModel> _availableTournamentModels = GlobalConfig.Connection.GetAllTournaments();

        public TournamentDashboardForm()
        {
            InitializeComponent();
            CenterToScreen();
            WireUpLists();
        }

        /// <summary>
        /// ComboBox update data method
        /// </summary>
        public void WireUpLists()
        {
            selectTournamentDropDown.DataSource = null;

            selectTournamentDropDown.DataSource = _availableTournamentModels;
            selectTournamentDropDown.DisplayMember = "tournamentName";
        }

        /// <summary>
        /// Retrieve selected Tournament and open TournamentViewer Form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LoadTournamentButton_Click(object sender, EventArgs e)
        {
            TournamentModel selectedModel = (TournamentModel) selectTournamentDropDown.SelectedItem;
            if (selectedModel == null)
            {
                Helper.ShowMessage("There is no selected tournament", true);
                return;
            }

            TournamentViewerForm viewerForm = new TournamentViewerForm(selectedModel);
            viewerForm.Show();
        }

        /// <summary>
        /// Opens CreateTournament Form and get created tournament
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void createTournamentButton_Click(object sender, EventArgs e)
        {
            CreateTournamentForm form  = new CreateTournamentForm(this);
            form.Show();
        }

        /// <summary>
        /// Retrieve created model and add it to combobox
        /// </summary>
        /// <param name="model"></param>
        public void TournamentComplete(TournamentModel model)
        {
            _availableTournamentModels.Add(model);
            WireUpLists();
        }
    }
}
