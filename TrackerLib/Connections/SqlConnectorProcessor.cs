using System.Collections.Generic;
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
        public static void SaveTournament(TournamentModel model, IDbConnection connection)
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

        public static void SaveTournamentRounds(TournamentModel model, IDbConnection connection)
        {
            // Loop through the rounds
            // loop through the match ups
            // save a match up
            // loop though the entries in a match up and save them

            foreach (var round in model.Rounds)
            {
                foreach (var match in round)
                {
                    var p = new DynamicParameters();
                    p.Add("@MatchupRound", match.MatchUpRound);
                    p.Add("@TournamentId", model.Id);
                    p.Add("@Id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                    connection.Execute("dbo.spMatchups_Insert", p, commandType: CommandType.StoredProcedure);

                    match.Id = p.Get<int>("@Id");

                    foreach (var entry in match.Entries)
                    {
                        p = new DynamicParameters();
                        p.Add("@MatchupId", match.Id);
                        
                        if (entry.ParentMatchUp != null) 
                            p.Add("@ParentMatchupId", entry.ParentMatchUp.Id);
                        else
                            p.Add("@ParentMatchupId", null);

                        if (entry.TeamCompeting != null)
                            p.Add("@TeamCompetingId", entry.TeamCompeting.Id);
                        else
                            p.Add("@TeamCompetingId", null);

                        p.Add("@Id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                        connection.Execute("dbo.spMatchupEntries_Insert", p, commandType: CommandType.StoredProcedure);

                        entry.Id = p.Get<int>("@Id");
                    }
                }
            }
        }
    }
}