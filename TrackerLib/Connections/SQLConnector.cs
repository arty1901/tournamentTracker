using Dapper;
using System.Collections.Generic;
using System.Data;
using TrackerLib.Models;
using System.Data.SqlClient;

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
        public PrizeModel CreatePrize(PrizeModel model)
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

                return model;
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
        public PersonModel CreatePerson(PersonModel model)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(TournamentDb)))
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("@FirstName", model.FirstName);
                p.Add("@LastName", model.LastName);
                p.Add("@EmailAddress", model.EmailAddress);
                p.Add("@Phone", model.Phone);
                p.Add("@Id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                // Надо передать имя_прецедуры, данные, тип команды, в данном случае это storedprocedure
                connection.Execute("dbo.spPerson_Insert", p, commandType: CommandType.StoredProcedure);

                //получить созданный id из базы и сохранить в модель
                model.Id = p.Get<int>("@Id");

                return model;
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
            }
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
        public TeamModel CreateTeam(TeamModel model)
        {
            using ( IDbConnection connection = new SqlConnection( GlobalConfig.CnnString(TournamentDb) ) )
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("@TeamName", model.TeamName);
                p.Add("@Id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                // Надо передать имя_прецедуры, данные, тип команды, в данном случае это storedprocedure
                connection.Execute("dbo.spTeams_Insert", p, commandType: CommandType.StoredProcedure);

                //получить созданный id из базы и сохранить в модель
                model.Id = p.Get<int>("@Id");

                foreach (PersonModel person in model.TeamMembers)
                {
                    p = new DynamicParameters();
                    p.Add("@TeamID", model.Id);
                    p.Add("@PersonID", person.Id);

                    // Надо передать имя_прецедуры, данные, тип команды, в данном случае это storedprocedure
                    connection.Execute("dbo.spTeamMembers_Insert", p, commandType: CommandType.StoredProcedure);
                }

                return model;
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
