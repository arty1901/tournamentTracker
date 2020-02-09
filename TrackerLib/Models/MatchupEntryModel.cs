using System;
using System.Collections.Generic;
using System.Text;

namespace TrackerLib.Models
{
    /// <summary>
    /// Represent one team in a match up
    /// </summary>
    public class MatchupEntryModel
    {
        /// <summary>
        /// Represent one team in the match up
        /// </summary>
        public TeamModel TeamCompeting { get; set; }

        /// <summary>
        /// Represent the score for this particular match up
        /// </summary>
        public double Score { get; set; }
        
        /// <summary>
        /// Represent the match up that this team came
        /// </summary>
        public MatchUpModel ParentMatchUp { get; set; }

    }
}
