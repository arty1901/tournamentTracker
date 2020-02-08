using System;
using System.Collections.Generic;
using TrackerLib.Models;
using TrackerLib.Connections.TextHelpers;
using System.Linq;

namespace TrackerLib.Connections
{
    public class TextConntector : IDataConnection
    {
        private const string PrizesFile = "PrizeModels.csv";
        private const string PersonsFile = "PersonModels.csv";
        private const string TeamFile = "TeamModel.csv";
        private const string TournamentFile = "TournamentModel.csv";


        public PrizeModel CreatePrize(PrizeModel model)
        {
            List<PrizeModel> prizes = PrizesFile.FullFileName().LoadFile().ConverToPrizeModels();

            int currentId = 1;
            if (prizes.Count > 0)
            {
                currentId = prizes.OrderByDescending(x => x.Id).First().Id + 1;
            }
            model.Id = currentId;

            prizes.Add(model);

            prizes.SaveToPrizeFile(PrizesFile);

            return model;
        }

        public PersonModel CreatePerson(PersonModel model)
        {
            List<PersonModel> persons = PersonsFile.FullFileName().LoadFile().ConvertToPersonModels();

            int currentId = 1;
            if (persons.Count > 0)
            {
                currentId = persons.OrderByDescending(x => x.Id).First().Id + 1;
            }

            model.Id = currentId;
            persons.Add(model);
            persons.SaveToPersonFile(PersonsFile);

            return model;
        }

        public TeamModel CreateTeam(TeamModel model)
        {
            List<TeamModel> teamList = TeamFile.FullFileName().LoadFile().ConvertToTeamModels(TeamFile);

            int currentId = 1;
            if (teamList.Count > 0)
            {
                currentId = teamList.OrderByDescending(x => x.Id).First().Id + 1;
            }

            model.Id = currentId;
            teamList.Add(model);
            teamList.SaveToTeamFile(TeamFile);

            return model;
        }

        public void CreateTournament(TournamentModel model)
        {
            List<TournamentModel> tournament = TournamentFile.FullFileName().LoadFile().ConvertTournamentModel(TeamFile, PrizesFile, PersonsFile);

            int currentId = tournament.Count > 0 ? tournament.OrderByDescending(x => x.Id).First().Id + 1 : 1;

            model.Id = currentId;

            tournament.Add(model);

            tournament.SaveToTournamentFile(TournamentFile);

        }

        public List<TeamModel> GetAllTeams()
        {
            return TeamFile.FullFileName().LoadFile().ConvertToTeamModels(PersonsFile);
        }

        public List<PrizeModel> GetAllPrizes()
        {
            return PrizesFile.FullFileName().LoadFile().ConverToPrizeModels();
        }

        public List<PersonModel> GetAllPersons()
        {
            return PersonsFile.FullFileName().LoadFile().ConvertToPersonModels();
        }
    }
}
