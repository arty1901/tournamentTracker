using System;
using System.Collections.Generic;
using TrackerLib.Models;
using TrackerLib.Connections.TextHelpers;
using System.Linq;

namespace TrackerLib.Connections
{
    public class TextConnector : IDataConnection
    {
        public PrizeModel CreatePrize(PrizeModel model)
        {
            List<PrizeModel> prizes = GlobalConfig.PrizesFile.FullFileName().LoadFile().ConvertToPrizeModels();

            int currentId = 1;
            if (prizes.Count > 0)
            {
                currentId = prizes.OrderByDescending(x => x.Id).First().Id + 1;
            }
            model.Id = currentId;

            prizes.Add(model);

            prizes.SaveToPrizeFile();

            return model;
        }

        public PersonModel CreatePerson(PersonModel model)
        {
            List<PersonModel> persons = GlobalConfig.PersonsFile.FullFileName().LoadFile().ConvertToPersonModels();

            int currentId = 1;
            if (persons.Count > 0)
            {
                currentId = persons.OrderByDescending(x => x.Id).First().Id + 1;
            }

            model.Id = currentId;
            persons.Add(model);
            persons.SaveToPersonFile(GlobalConfig.PersonsFile);

            return model;
        }

        public TeamModel CreateTeam(TeamModel model)
        {
            List<TeamModel> teamList = GlobalConfig.TeamFile.FullFileName().LoadFile().ConvertToTeamModels();

            int currentId = 1;
            if (teamList.Count > 0)
            {
                currentId = teamList.OrderByDescending(x => x.Id).First().Id + 1;
            }

            model.Id = currentId;
            teamList.Add(model);
            teamList.SaveToTeamFile(GlobalConfig.TeamFile);

            return model;
        }

        public void CreateTournament(TournamentModel model)
        {
            List<TournamentModel> tournament = GlobalConfig.TournamentFile.FullFileName().LoadFile().ConvertToTournamentModel();

            int currentId = tournament.Count > 0 ? tournament.OrderByDescending(x => x.Id).First().Id + 1 : 1;

            model.Id = currentId;

            model.SaveRoundsToFile();

            tournament.Add(model);

            tournament.SaveToTournamentFile(GlobalConfig.TournamentFile.FullFileName());
        }

        public List<TeamModel> GetAllTeams()
        {
            return GlobalConfig.TeamFile.FullFileName().LoadFile().ConvertToTeamModels();
        }

        public List<PrizeModel> GetAllPrizes()
        {
            return GlobalConfig.PrizesFile.FullFileName().LoadFile().ConvertToPrizeModels();
        }

        public List<TournamentModel> GetAllTournaments()
        {
            throw new NotImplementedException();
        }

        public List<PersonModel> GetAllPersons()
        {
            return GlobalConfig.PersonsFile.FullFileName().LoadFile().ConvertToPersonModels();
        }
    }
}
