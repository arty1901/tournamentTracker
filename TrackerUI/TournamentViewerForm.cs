using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using TrackerLib.Models;

namespace TrackerUI
{
    public partial class TournamentViewerForm : Form
    {
        private readonly TournamentModel _loadedTournament;
        private List<int> rounds = new List<int>();

        public TournamentViewerForm(TournamentModel model)
        {
            InitializeComponent();
            CenterToScreen();
            this._loadedTournament = model;

            this.LoadFormData();
        }

        private void roundDropDown_SelectedValueChanged(object sender, EventArgs e)
        {

        }

        private void LoadFormData()
        {
            tournamentName.Text = this._loadedTournament.TournamentName;

            for (int i = 0; i <= this._loadedTournament.Rounds.Count; i++)
                roundDropDown.Items.Add("Round - " + i++);
        }

        private void LoadRounds()
        {
            rounds.Add(1);
            int currRound = 1;

            foreach (List<MatchUpModel> matchUps in this._loadedTournament.Rounds)
            {
                if (matchUps.First().MatchUpRound > currRound)
                {
                    currRound = matchUps.First().MatchUpRound;
                    rounds.Add(currRound);
                }
            }
        }
    }
}
