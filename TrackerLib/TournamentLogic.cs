using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TrackerLib.Models;

namespace TrackerLib
{
    public static class TournamentLogic
    {
        // order out list randomly of teams
        // Check if it is big enough  - if not, add in byes
        // create a first round of match ups
        // create every round after that

        public static void CreateRounds(TournamentModel model)
        {
            List<TeamModel> randomizedTeams = RandomizeTeamModel ( model.EnteredTeams );
            int rounds = FindNumberOfRounds(randomizedTeams.Count);
            int byes = NumberOfByes ( rounds, randomizedTeams.Count );

            model.Rounds.Add(CreateFirstRound(byes, randomizedTeams));

            CreateOtherRounds(model, rounds);
        }

        /// <summary>
        /// Randomize an order of a team list
        /// </summary>
        /// <param name="model"></param>
        /// <returns>A new list of Team Model</returns>
        private static List<TeamModel> RandomizeTeamModel(List<TeamModel> model)
        {
            return model.OrderBy(a => Guid.NewGuid()).ToList();
        }

        private static int FindNumberOfRounds(int teamCount)
        {
            int output = 1;
            int val = 2;

            while (val < teamCount)
            {
                output += 1;
                val *= 2;
            }

            return output;
        }

        private static int NumberOfByes(int rounds, int numberOfTeams)
        {
            int output = 0;
            int totalTeams = 1;

            for (int i = 1; i <= rounds; i++)
            {
                totalTeams *= 2;
            }

            output = totalTeams - numberOfTeams;

            return output;
        }

        private static List<MatchUpModel> CreateFirstRound(int numberOfByes, List<TeamModel> teams)
        {
            List<MatchUpModel> output = new List<MatchUpModel>();
            MatchUpModel current = new MatchUpModel();

            foreach (var team in teams)
            {
                current.Entries.Add( new MatchupEntryModel{ TeamCompeting = team } );

                if (numberOfByes > 0 || current.Entries.Count > 1)
                {
                    current.MatchUpRound = 1;
                    output.Add(current);
                    current = new MatchUpModel();

                    if (numberOfByes > 0) numberOfByes--;
                }
            }

            return output;
        }

        /// <summary>
        /// Creates next rounds after the first one
        /// </summary>
        /// <param name="tournament"></param>
        /// <param name="rounds">Total number of rounds</param>
        private static void CreateOtherRounds (TournamentModel tournament, int rounds)
        {
            // Set next number of round, is 2
            int round = 2;
            
            // Previous round is the first element in a list
            // because it is the first round of a tournament
            List<MatchUpModel> previousRound = tournament.Rounds[0];

            // current match up will be added to the current list of matches of current round
            MatchUpModel currentMatchUp = new MatchUpModel();

            // List of matches in the current round
            List<MatchUpModel> currRound = new List<MatchUpModel>();

            while (round <= rounds) 
            {
                // Loop through each game in a prev round
                foreach (MatchUpModel match in previousRound)
                {
                    currentMatchUp.Entries.Add(new MatchupEntryModel{ ParentMatchUp = match });

                    if (currentMatchUp.Entries.Count > 1)
                    {
                        currentMatchUp.MatchUpRound = round;
                        currRound.Add(currentMatchUp);
                        currentMatchUp = new MatchUpModel();
                    }
                }

                round++;
                tournament.Rounds.Add(currRound);
                previousRound = currRound;

                currRound = new List<MatchUpModel>();
            }
        }
    }
}