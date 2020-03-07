using Dapper;
using System.Collections.Generic;
using System.Data;
using TrackerLib.Models;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;

namespace TrackerLib.Connections
{
    public class SqlConnector : IDataConnection
    {
        private const string TournamentDb = "Tournaments";

        /// <summary>
        /// Save a new prize to a database
        /// </summary>
        /// <param name="model"></param>
        /// <returns>The prize info, including the id</returns>
        public void CreatePrize(PrizeModel model)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(TournamentDb)))
            {
                var p = new DynamicParameters();
                p.Add("@PlaceNumber", model.PlaceNumber);
                p.Add("@PlaceName", model.PlaceName);
                p.Add("@PrizeAmount", model.PrizeAmount);
                p.Add("@PrizePercentage", model.PrizePrecentage);
                p.Add("@Id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("dbo.spPrizes_Insert", p, commandType: CommandType.StoredProcedure);

                model.Id = p.Get<int>("@Id");
            }
        }

        /// <summary>
        /// Get all prizes
        /// </summary>
        /// <returns>List of prizes</returns>
        public List<PrizeModel> GetAllPrizes()
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(TournamentDb)))
            {
                return connection.Query<PrizeModel>("dbo.spPrizes_GetAllPrizes").AsList();
            }
        }

        /// <summary>
        /// Save a new person to a database
        /// </summary>
        /// <param name="model"></param>
        /// <returns>The person info, including the id</returns>
        public void CreatePerson(PersonModel model)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(TournamentDb)))
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("@FirstName", model.FirstName);
                p.Add("@LastName", model.LastName);
                p.Add("@EmailAddress", model.EmailAddress);
                p.Add("@Phone", model.Phone);
                p.Add("@Id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("dbo.spPerson_Insert", p, commandType: CommandType.StoredProcedure);

                model.Id = p.Get<int>("@Id");
            }
        }

        /// <summary>
        /// Save a tournament
        /// </summary>
        /// <param name="tournament"></param>
        public void CreateTournament(TournamentModel tournament)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(TournamentDb)))
            {
                SqlConnectorProcessor.SaveTournament(tournament, connection);

                SqlConnectorProcessor.SaveTournamentEntries(tournament, connection);

                SqlConnectorProcessor.SaveTournamentPrizes(tournament, connection);

                SqlConnectorProcessor.SaveTournamentRounds(tournament, connection);

                TournamentLogic.UpdateTournamentResults(tournament);
            }
        }

        public void UpdateMatchUp(MatchUpModel model)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(TournamentDb)))
            {
                DynamicParameters p = new DynamicParameters();
                if (model.Winner != null)
                {
                    p.Add("@Id", model.Id);
                    p.Add("@WinnerId", model.Winner.Id);

                    connection.Execute("dbo.spMatchUps_Update", p, commandType: CommandType.StoredProcedure);
                }


                // spMatchUpEntries_Update id, TeamCompetingId, Score
                foreach (MatchupEntryModel me in model.Entries)
                {
                    if (me.TeamCompeting != null)
                    {
                        p = new DynamicParameters();

                        p.Add("@Id", me.Id);
                        p.Add("@TeamCompetingId", me.TeamCompeting.Id);
                        p.Add("@Score", me.Score);

                        connection.Execute("dbo.spMatchUpEntries_Update", p, commandType: CommandType.StoredProcedure);
                    }

                }
            }
        }

        public List<TournamentModel> GetAllTournaments()
        {
            List<TournamentModel> output;

            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(TournamentDb)))
            {
                output = connection.Query<TournamentModel>("dbo.spTournaments_GetAll").AsList();

                foreach (TournamentModel t in output)
                {
                    DynamicParameters p = new DynamicParameters();
                    p.Add("@TournamentId", t.Id);

                    // Get prizes by tournament
                    t.Prizes = connection.Query<PrizeModel>("dbo.spPrizes_GetByTournament", p, commandType: CommandType.StoredProcedure).AsList();

                    // Get teams 
                    t.EnteredTeams = connection.Query<TeamModel>("dbo.spTeams_GetByTournament", p, commandType: CommandType.StoredProcedure).AsList();

                    foreach (TeamModel team in t.EnteredTeams)
                    {
                        p = new DynamicParameters();
                        p.Add("@TeamId", team.Id);

                        team.TeamMembers = connection.Query<PersonModel>("dbo.spTeamMembers_GetByTeam", p, commandType: CommandType.StoredProcedure).AsList();
                    }

                    // Get rounds
                    p.Add("@TournamentId", t.Id);

                    // Fetch all match ups by tournament Id
                    List<MatchUpModel> matchUps =
                        connection.Query<MatchUpModel>("dbo.spMatchups_GetByTournament", p, commandType: CommandType.StoredProcedure).AsList();

                    foreach (MatchUpModel m in matchUps)
                    {
                        p = new DynamicParameters();
                        p.Add("@MatchUpId", m.Id);

                        m.Entries = connection.Query<MatchupEntryModel>("dbo.spMatchUpEntries_GetByMatchUp", p,
                            commandType: CommandType.StoredProcedure).AsList();

                        // Populate all entry (2 models)
                        List<TeamModel> allTeams = GetAllTeams();

                        if (m.WinnerId > 0)
                            m.Winner = allTeams.First(x => x.Id == m.WinnerId);

                        foreach (MatchupEntryModel entry in m.Entries)
                        {
                            if (entry.TeamCompetingId > 0)
                                entry.TeamCompeting = allTeams.First(x => x.Id == entry.TeamCompetingId);

                            if (entry.ParentMatchUpId > 0)
                                entry.ParentMatchUp = matchUps.First(x => x.Id == entry.ParentMatchUpId);
                        }
                    }

                    List<MatchUpModel> currentListRound = new List<MatchUpModel>();
                    int currentRound = 1;

                    foreach (var matchup in matchUps)
                    {
                        if (currentRound < matchup.MatchUpRound)
                        {
                            t.Rounds.Add(currentListRound);
                            currentListRound = new List<MatchUpModel>();
                            currentRound++;
                        }

                        currentListRound.Add(matchup);
                    }

                    t.Rounds.Add(currentListRound);

                }
            }

            return output;
        }

        /// <summary>
        /// Get all persons
        /// </summary>
        /// <returns>List of persons</returns>
        public List<PersonModel> GetAllPersons()
        {
            List<PersonModel> output;

            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(TournamentDb)))
            {
                output = connection.Query<PersonModel>("dbo.spPerson_GetAll").AsList();
            }

            return output;
        }

        /// <summary>
        /// Save a team to DB
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public void CreateTeam(TeamModel model)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(TournamentDb)))
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("@TeamName", model.TeamName);
                p.Add("@Id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("dbo.spTeams_Insert", p, commandType: CommandType.StoredProcedure);

                model.Id = p.Get<int>("@Id");

                foreach (PersonModel person in model.TeamMembers)
                {
                    p = new DynamicParameters();
                    p.Add("@TeamID", model.Id);
                    p.Add("@PersonID", person.Id);

                    connection.Execute("dbo.spTeamMembers_Insert", p, commandType: CommandType.StoredProcedure);
                }
            }
        }

        /// <summary>
        /// Get all teams
        /// </summary>
        /// <returns>List of teams</returns>
        public List<TeamModel> GetAllTeams()
        {
            List<TeamModel> output;

            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(TournamentDb)))
            {
                output = connection.Query<TeamModel>("dbo.spTeams_GetAll").AsList();

                foreach (TeamModel team in output)
                {
                    DynamicParameters p = new DynamicParameters();
                    p.Add("@TeamId", team.Id);
                    team.TeamMembers = connection.Query<PersonModel>("dbo.spTeamMembers_GetByTeam", p, commandType: CommandType.StoredProcedure).AsList();
                }
            }

            return output;
        }
    }
}
