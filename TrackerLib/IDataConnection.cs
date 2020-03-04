using System;
using System.Collections.Generic;
using System.Text;
using TrackerLib.Models;

namespace TrackerLib
{
    public interface IDataConnection
    {
        void CreatePrize(PrizeModel model);
        void CreatePerson(PersonModel model);
        void CreateTeam(TeamModel team);
        void CreateTournament(TournamentModel tournament);
        void UpdateMatchUp(MatchUpModel model);
        List<PersonModel> GetAllPersons();
        List<TeamModel> GetAllTeams();
        List<PrizeModel> GetAllPrizes();
        List<TournamentModel> GetAllTournaments();
    }
}
