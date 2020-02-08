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
        TournamentModel CreateTournament(TournamentModel tournament);
        List<PersonModel> GetAllPersons();
        List<TeamModel> GetAllTeams();
        List<PrizeModel> GetAllPrizes();
    }
}
