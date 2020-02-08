using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Windows.Forms;
using TrackerLib.Models;
using System.Data.SqlClient;

namespace TrackerLib.Connections
{
    public class SQLConnector : IDataConnection
    {
        private const string tournamentDB = "Tournaments";

        /// <summary>
        /// Save a new prize to a database
        /// </summary>
        /// <param name="model"></param>
        /// <returns>The prize info, including the id</returns>
        public PrizeModel CreatePrize(PrizeModel model)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(tournamentDB)))
            {
                var p = new DynamicParameters();
                p.Add("@PlaceNumber", model.PlaceNumber);
                p.Add("@PlaceName", model.PlaceName);
                p.Add("@PrizeAmount", model.PrizeAmount);
                p.Add("@PrizePercentage", model.PrizePrecentage);
                p.Add("@Id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("dbo.spPrizes_Insert", p, commandType: CommandType.StoredProcedure);

                // Эта команда просматривает динамические параметры (в данном случае это "р")
                // и ищет переданное имя параметра  
                model.Id = p.Get<int>("@Id");

                return model;
            }
        }

        public List<PrizeModel> GetAllPrizes()
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(tournamentDB)))
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
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(tournamentDB)))
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

        public TournamentModel CreateTournament(TournamentModel tournament)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(tournamentDB)))
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("@TournamentName", tournament.TournamentName);
                p.Add("@EntryFee", tournament.EntryFee);
                p.Add("@Active", tournament.Active);
                p.Add("@Id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                connection.Execute("dbo.spTournaments_Insert", p, commandType: CommandType.StoredProcedure);

                tournament.Id = p.Get<int>("@Id");

                foreach (TeamModel team in tournament.EnteredTeams)
                {
                    p = new DynamicParameters();
                    p.Add("@TournamentID", tournament.Id);
                    p.Add("@TeamID", team.Id);

                    connection.Execute("@dbo.spTournamentEntries_Insert", p, commandType: CommandType.StoredProcedure);
                }

                foreach (PrizeModel prize in tournament.Prizes)
                {
                    p = new DynamicParameters();
                    p.Add("@TournamentID", tournament.Id);
                    p.Add("@PrizeID", prize.Id);

                    connection.Execute("dbo.spTournamentPrizes_Insert", p, commandType: CommandType.StoredProcedure);
                }

                return tournament;
            }
        }

        /// <summary>
        /// Get all Persons from a DB
        /// </summary>
        /// <returns>List of persons</returns>
        public List<PersonModel> GetAllPersons()
        {
            List<PersonModel> output;

            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(tournamentDB)))
            {
                output = connection.Query<PersonModel>("dbo.spPerson_GetAll").AsList();
            }

            return output;
        }

        public TeamModel CreateTeam(TeamModel model)
        {
            using ( IDbConnection connection = new SqlConnection( GlobalConfig.CnnString(tournamentDB) ) )
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

        public List<TeamModel> GetAllTeams()
        {
            List<TeamModel> output;

            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString(tournamentDB)))
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
