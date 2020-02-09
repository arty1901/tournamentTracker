using System.Data;
using Dapper;
using TrackerLib.Models;

namespace TrackerLib.Connections
{
    public static class SqlConnectorProcessor
    {
        /// <summary>
        /// Save tournament info in DB
        /// </summary>
        /// <param name="model"></param>
        /// <param name="connection"></param>
        public static void SaveTournament ( TournamentModel model, IDbConnection connection )
        {
            DynamicParameters p = new DynamicParameters();
            p.Add("@TournamentName", model.TournamentName);
            p.Add("@EntryFee", model.EntryFee);
            p.Add("@Id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

            connection.Execute("dbo.spTournaments_Insert", p, commandType: CommandType.StoredProcedure);

            model.Id = p.Get<int>("@Id");
        }

        /// <summary>
        /// Save tournament entries (teams) witch take part in
        /// tournament
        /// </summary>
        /// <param name="model"></param>
        /// <param name="connection"></param>
        public static void SaveTournamentEntries(TournamentModel model, IDbConnection connection)
        {
            foreach (TeamModel team in model.EnteredTeams)
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("@TournamentID", model.Id);
                p.Add("@TeamID", team.Id);

                connection.Execute("dbo.spTournamentEntries_Insert", p, commandType: CommandType.StoredProcedure);
            }
        }

        /// <summary>
        /// Save prizes of a tournament
        /// </summary>
        /// <param name="model"></param>
        /// <param name="connection"></param>
        public static void SaveTournamentPrizes(TournamentModel model, IDbConnection connection)
        {
            foreach (PrizeModel prize in model.Prizes)
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("@TournamentID", model.Id);
                p.Add("@PrizeID", prize.Id);

                connection.Execute("dbo.spTournamentPrizes_Insert", p, commandType: CommandType.StoredProcedure);
            }
        }
    }
}