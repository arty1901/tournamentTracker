using System;
using System.Collections.Generic;
using System.Text;

namespace TrackerLib.Models
{
    public class TournamentModel
    {
        public string  TournamentName { get; set; }
        public decimal EntryFee { get; set; }
        public List<TeamModel> EnteredTeams { get; set; } = new List<TeamModel>();
        public List<PrizeModel> Prizes { get; set; } = new List<PrizeModel>();
        public List<List<MatchupModel>> MyProperty { get; set; } = new List<List<MatchupModel>>();
    }
}
