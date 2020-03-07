using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using TrackerLib;
using TrackerLib.Models;
using TrackerUI.Helper_Methods;

namespace TrackerUI
{
    public partial class TournamentViewerForm : Form
    {
        private readonly TournamentModel _loadedTournament;
        private List<int> _rounds = new List<int>();
        private List<MatchUpModel> _matchUps = new List<MatchUpModel>();

        public TournamentViewerForm(TournamentModel model)
        {
            InitializeComponent();
            CenterToScreen();
            this._loadedTournament = model;

            WireUpRoundList();
            WireUpMatchUpList();

            this.LoadFormData();
            this.LoadRounds();
        }

        private void LoadFormData()
        {
            tournamentName.Text = this._loadedTournament.TournamentName;
        }

        private void WireUpRoundList()
        {
            roundDropDown.DataSource = null;
            roundDropDown.DataSource = _rounds;
        }
        
        private void WireUpMatchUpList()
        {
            matchupListBox.DataSource = null;
            matchupListBox.DataSource = _matchUps;
            matchupListBox.DisplayMember = "displayName";
        }

        private void LoadRounds()
        {
            this._rounds = new List<int> {1};
            int currentRound = 1;

            foreach (List<MatchUpModel> matchUps in this._loadedTournament.Rounds)
            {
                if (matchUps.First().MatchUpRound > currentRound)
                {
                    currentRound = matchUps.First().MatchUpRound;
                    _rounds.Add(currentRound);
                }
            }

            WireUpRoundList();
            LoadMatchUps(1);
        }

        private void roundDropDown_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.LoadMatchUps((int)roundDropDown.SelectedItem);
        }

        private void LoadMatchUps(int round)
        {
            List<MatchUpModel> temp = new List<MatchUpModel>();

            foreach (List<MatchUpModel> matchUp in this._loadedTournament.Rounds)
            {
                if (matchUp.First().MatchUpRound != round) continue;

                foreach (MatchUpModel m in matchUp)
                {
                    if (m.Winner == null || !unplayedOnlyCheckBox.Checked)
                    {
                        temp.Add(m);
                    }
                }

                this._matchUps = temp;
            }


            WireUpMatchUpList();
            DisplayMatchUpInfo();
        }

        private void DisplayMatchUpInfo()
        {
            bool isVisible = this._matchUps.Count > 0;

            teamOneNameLabel.Visible =
            teamOneScoreLabel.Visible =
            teamOneScoreValueTextBox.Visible =
            teamTwoScoreValueTextBox.Visible =
            teamTwoScoreLabel.Visible =
            teamTwoNameLabel.Visible =
            scoreButton.Visible = isVisible;
        }

        private void matchupListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.LoadMatchUp((MatchUpModel)matchupListBox.SelectedItem);
        }

        private void LoadMatchUp(MatchUpModel m)
        {
            if (m == null) return;

            for (int i = 0; i < m.Entries.Count; i++)
            {
                if (i == 0)
                {
                    if (m.Entries[0].TeamCompeting != null)
                    {
                        teamOneNameLabel.Text = m.Entries[0].TeamCompeting.TeamName;
                        teamOneScoreValueTextBox.Text = m.Entries[0].Score.ToString();

                        teamTwoNameLabel.Text = "<bye>";
                        teamTwoScoreValueTextBox.Text = "";
                    }
                    else
                    {
                        teamOneNameLabel.Text = "Not set Yet";
                        teamOneScoreValueTextBox.Text = "";
                    }
                }

                if (i == 1)
                {
                    if (m.Entries[1].TeamCompeting != null)
                    {
                        teamTwoNameLabel.Text = m.Entries[1].TeamCompeting.TeamName;
                        teamTwoScoreValueTextBox.Text = m.Entries[1].Score.ToString();
                    }
                    else
                    {
                        teamTwoNameLabel.Text = "Not set Yet";
                        teamTwoScoreValueTextBox.Text = "";
                    }
                }

            }
        }

        private void unplayedOnlyCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            this.LoadMatchUps((int)roundDropDown.SelectedItem);
        }

        private void scoreButton_Click(object sender, EventArgs e)
        {
            MatchUpModel m = (MatchUpModel) matchupListBox.SelectedItem;
            double teamOneScore = 0, teamTwoScore = 0;

            if (!IsValidData())
            {
                MessageBox.Show("You need to enter valid data before we can score this match up.");
                return;
            }

            for (int i = 0; i < m.Entries.Count; i++)
            {
                if (i == 0)
                {
                    if (m.Entries[0].TeamCompeting != null)
                    {
                        teamOneNameLabel.Text = m.Entries[0].TeamCompeting.TeamName;
                        bool scoreValid = double.TryParse(teamOneScoreValueTextBox.Text, out teamOneScore);

                        if (scoreValid)
                            m.Entries[0].Score = teamOneScore;
                        else
                        {
                            Helper.ShowMessage("Please enter the valid score value for team one", true);
                            return;
                        }
                    }
                }


                if (i == 1)
                {
                    if (m.Entries[1].TeamCompeting != null)
                    {
                        teamTwoNameLabel.Text = m.Entries[1].TeamCompeting.TeamName;
                        bool scoreValid = double.TryParse(teamTwoScoreValueTextBox.Text, out teamTwoScore);

                        if (scoreValid)
                            m.Entries[1].Score = teamTwoScore;
                        else
                        {
                            Helper.ShowMessage("Please enter the valid score value for team two", true);
                            return;
                        }
                    }
                }
            }

            try
            {
                TournamentLogic.UpdateTournamentResults(this._loadedTournament);
            }
            catch (Exception exception)
            {
                MessageBox.Show($@"The application had the following error: {exception.Message}");
                return;
            }

            this.LoadMatchUps((int)roundDropDown.SelectedItem);
        }

        private bool IsValidData()
        {
            bool output = true;

            double scoreOne = 0;
            double scoreTwo = 0;

            bool scoreOneValid = double.TryParse(teamOneScoreValueTextBox.Text, out scoreOne);
            bool scoreTwoValid = double.TryParse(teamOneScoreValueTextBox.Text, out scoreTwo);

            if (!scoreTwoValid || !scoreOneValid)
            {
                output = false;
            }
            else if (scoreOne == 0 && scoreTwo == 0)
            {
                output = false;
            }
            else if (scoreOne == scoreTwo)
            {
                output = false;
            }

            return output;
        }
    }
}
