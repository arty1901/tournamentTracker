using System;
using System.Collections.Generic;
using System.Text;

namespace TrackerLib.Models
{
    public class MatchUpModel
    {
        /// <summary>
        /// The unique identifier for the match up
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The set of teams that were involved in this match
        /// </summary>
        public List<MatchupEntryModel> Entries { get; set; } = new List<MatchupEntryModel>();

        /// <summary>
        /// The id from DB that will be used to identify winner
        /// </summary>
        public int WinnerId { get; set; }

        /// <summary>
        /// The winner of this match
        /// </summary>
        public TeamModel Winner { get; set; }

        /// <summary>
        /// Which round this match up is part of
        /// </summary>
        public int MatchUpRound { get; set; }

        /// <summary>
        /// Name of match up
        /// </summary>
        public string DisplayName
        {
            get
            {
                string output = "";
                foreach (MatchupEntryModel me in Entries)
                {
                    if (me.TeamCompeting != null)
                        if (output.Length == 0)
                            output = me.TeamCompeting.TeamName;
                        else
                            output += $" vs. {me.TeamCompeting.TeamName}";
                    else
                    {
                        output = "Match Up Not Yet Determined";
                        break;
                    }
                }

                return output;
            }
        }
    }
}
