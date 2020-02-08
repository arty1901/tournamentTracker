using System;
using System.Collections.Generic;
using System.Text;

namespace TrackerLib.Models
{
    public class MatchUpModel
    {
        public int Id { get; set; }
        public List<MatchupEntryModel> Entries { get; set; }
        public TeamModel Winner { get; set; }
        public int MatchUpRound { get; set; }
    }
}
