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
        /// Represent unique identifier for team in the match up
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The id from DB that will be used to identify the competing team
        /// </summary>
        public int TeamCompetingId { get; set; }

        /// <summary>
        /// Represent one team in the match up
        /// </summary>
        public TeamModel TeamCompeting { get; set; }

        /// <summary>
        /// Represent the score for this particular match up
        /// </summary>
        public double Score { get; set; }

        /// <summary>
        /// the id from DB that will be used to identify the parent match up
        /// </summary>
        public int ParentMatchUpId { get; set; }

        /// <summary>
        /// Represent the match up that this team came
        /// </summary>
        public MatchUpModel ParentMatchUp { get; set; }

    }
}
