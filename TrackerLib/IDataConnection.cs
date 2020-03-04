using System;
using System.Collections.Generic;
using System.Text;
using TrackerLib.Models;

namespace TrackerLib
{
    public interface IDataConnection
    {
        PrizeModel CreatePrize(PrizeModel model);
        PersonModel CreatePerson(PersonModel model);
        TeamModel CreateTeam(TeamModel team);
        void CreateTournament(TournamentModel tournament);
        void UpdateMatchUp(MatchUpModel model);
        List<PersonModel> GetAllPersons();
        List<TeamModel> GetAllTeams();
        List<PrizeModel> GetAllPrizes();
        List<TournamentModel> GetAllTournaments();
    }
}
