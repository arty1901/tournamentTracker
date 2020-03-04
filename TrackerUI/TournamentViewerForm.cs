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

            if (teamOneScore > teamTwoScore)
                m.Winner = m.Entries[0].TeamCompeting;
            else if (teamTwoScore > teamOneScore)
                m.Winner = m.Entries[1].TeamCompeting;
            else
                Helper.ShowMessage("I do not handle tie game", false);

            foreach (List<MatchUpModel> round in this._loadedTournament.Rounds)
            {
                foreach (MatchUpModel rm in round)
                {
                    foreach (MatchupEntryModel entry in rm.Entries)
                    {
                        if (entry.ParentMatchUp != null)
                        {
                            if (entry.ParentMatchUp.Id == m.Id)
                            {
                                entry.TeamCompeting = m.Winner;
                                GlobalConfig.Connection.UpdateMatchUp(rm);
                            }
                        }
                        
                    }
                }
            }

            this.LoadMatchUps((int)roundDropDown.SelectedItem);

            GlobalConfig.Connection.UpdateMatchUp(m);
        }
    }
}
