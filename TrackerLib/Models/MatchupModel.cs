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
        public List<MatchupEntryModel> Entries { get; set; }
        
        /// <summary>
        /// The winner of this match
        /// </summary>
        public TeamModel Winner { get; set; }

        /// <summary>
        /// Which round this match up is part of
        /// </summary>
        public int MatchUpRound { get; set; }
    }
}
