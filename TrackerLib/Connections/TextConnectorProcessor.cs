using System;
using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.Data;
using System.IO;
using System.Security.AccessControl;
using System.Windows.Forms;
using TrackerLib.Models;

namespace TrackerLib.Connections.TextHelpers
{
    /// <summary>
    /// Класс TextConnectorProcessor содержит вспомогательные методы/методы-расширения
    /// для сохранения данных в файл
    /// </summary>
    public static class TextConnectorProcessor
    {
        /// <summary>
        /// Build full path to file
        /// </summary>
        /// <param name="filename"></param>
        /// <returns>Full path to file</returns>
        public static string FullFileName(this string filename)
        {
            return $@"{ConfigurationManager.AppSettings.Get("filepath")}\{filename}";
        }

        /// <summary>
        /// Read file
        /// </summary>
        /// <param name="file"></param>
        /// <returns>Lines of file</returns>
        public static List<string> LoadFile(this string file)
        {
            if (!File.Exists(file))
            {
                return new List<string>();
            }

            return File.ReadAllLines(file).ToList();
        }

        // TODO - пересать методы конвертации в один универсальный метод
        /// <summary>
        /// Конвертация переданного списка строк типа string
        /// в список типа PrizeModel
        /// </summary>
        /// <param name="lines"></param>
        /// <returns>Список типа PrizeModel</returns>
        public static List<PrizeModel> ConvertToPrizeModels(this List<string> lines)
        {
            List<PrizeModel> output = new List<PrizeModel>();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');

                PrizeModel p = new PrizeModel
                {
                    Id = int.Parse(cols[0]),
                    PlaceNumber = int.Parse(cols[1]),
                    PlaceName = cols[2],
                    PrizeAmount = decimal.Parse(cols[3]),
                    PrizePrecentage = double.Parse(cols[4])
                };


                output.Add(p);
            }

            return output;
        }

        /// <summary>
        /// Конвертация переданного списка строк типа string
        /// в список типа PrizeModel
        /// </summary>
        /// <param name="lines"></param>
        /// <returns>Список типа PersonModel</returns>
        public static List<PersonModel> ConvertToPersonModels(this List<string> lines)
        {
            List<PersonModel> output = new List<PersonModel>();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');

                PersonModel p = new PersonModel
                {
                    Id = int.Parse(cols[0]),
                    FirstName = cols[1],
                    LastName = cols[2],
                    EmailAddress = cols[3],
                    Phone = cols[4]
                };

                output.Add(p);
            }

            return output;
        }

        /// <summary>
        /// Convert a list of lines to a list of Team Model
        /// </summary>
        /// <param name="lines"></param>
        /// <returns>List of teams model</returns>
        public static List<TeamModel> ConvertToTeamModels(this List<string> lines)
        {
            // id, team name, list of ids separated by pipe
            List<TeamModel> output = new List<TeamModel>();
            List<PersonModel> persons = GlobalConfig.PersonsFile.FullFileName().LoadFile().ConvertToPersonModels();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');

                TeamModel t = new TeamModel { Id = int.Parse(cols[0]), TeamName = cols[1] };

                string[] personIds = cols[2].Split('|');

                foreach (string id in personIds)
                {
                    t.TeamMembers.Add(persons.First(x => x.Id == int.Parse(id)));
                }

                output.Add(t);
            }

            return output;
        }

        public static List<MatchupEntryModel> ConvertToMatchUpEntryModels(this List<string> lines)
        {
            List<MatchupEntryModel> output = new List<MatchupEntryModel>();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');
                MatchupEntryModel model = new MatchupEntryModel
                {
                    Id = int.Parse(cols[0]),
                    Score = double.Parse(cols[2])
                };

                model.TeamCompeting = !string.IsNullOrEmpty(cols[1]) ? LookUpTeamById(int.Parse(cols[1])) : null;
                model.ParentMatchUp = int.TryParse(cols[3], out var parentId) ? LookUpMatchUpById(parentId) : null;

                output.Add(model);
            }

            return output;
        }

        private static TeamModel LookUpTeamById(int id)
        {
            List<string> teams = GlobalConfig.TeamFile.FullFileName().LoadFile();

            foreach (var team in teams)
            {
                string[] cols = team.Split(',');
                if (cols[0] == id.ToString())
                {
                    List<string> matchingTeams = new List<string>();

                    matchingTeams.Add(team);
                    return matchingTeams.ConvertToTeamModels().First();
                }
            }

            return null;
        }

        private static MatchUpModel LookUpMatchUpById(int id)
        {
            List<string> matchUps = GlobalConfig.MatchUpFile.FullFileName().LoadFile();

            foreach (string match in matchUps)
            {
                string[] cols = match.Split(',');
                if (cols[0] == id.ToString())
                {
                    List<string> matchinMatch = new List<string> { match };

                    return matchinMatch.ConvertToMatchUpModels().First();
                }
            }

            return null;
        }

        public static List<MatchUpModel> ConvertToMatchUpModels(this List<string> lines)
        {
            //id=0,entries=1 (pipe delimited),winner=2,matchupround=3
            List<MatchUpModel> output = new List<MatchUpModel>();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');

                MatchUpModel current = new MatchUpModel
                {
                    Id = int.Parse(cols[0]),
                    Entries = ConvertStringToMatchUpEntryModels(cols[1]),
                    MatchUpRound = int.Parse(cols[3])
                };

                current.Winner = string.IsNullOrEmpty(cols[2]) ? null : LookUpTeamById(int.Parse(cols[2]));

                output.Add(current);
            }

            return output;
        }
        private static List<MatchupEntryModel> ConvertStringToMatchUpEntryModels(string input)
        {
            List<string> entries = GlobalConfig.MatchUpEntryFile.FullFileName().LoadFile();
            List<MatchupEntryModel> output = new List<MatchupEntryModel>();
            string[] ids = input.Split('|');
            List<string> matchingEntries = new List<string>();

            foreach (string id in ids)
            {
                foreach (string entry in entries)
                {
                    string[] cols = entry.Split(',');

                    if (cols[0] == id)
                        matchingEntries.Add(entry);
                }
            }

            output = matchingEntries.ConvertToMatchUpEntryModels();

            return output;
        }

        /// <summary>
        /// Convert a list of lines to a list of tournament model
        /// </summary>
        /// <param name="lines"></param>
        /// <returns>List of tournament model</returns>
        public static List<TournamentModel> ConvertToTournamentModel(this List<string> lines)
        {
            // Id,tournamentName,EntryFee,Active,(id|id|id - team list),(id|id|id - prize list),(id:id:id|id:id:id|id:id:id - match list)
            List<TournamentModel> output = new List<TournamentModel>();
            List<TeamModel> teamList = GlobalConfig.TeamFile.FullFileName().LoadFile().ConvertToTeamModels();
            List<PrizeModel> prizeList = GlobalConfig.PrizesFile.FullFileName().LoadFile().ConvertToPrizeModels();
            List<MatchUpModel> matchUps = GlobalConfig.MatchUpFile.FullFileName().LoadFile().ConvertToMatchUpModels();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');

                TournamentModel tm = new TournamentModel
                {
                    Id = int.Parse(cols[0]),
                    TournamentName = cols[1],
                    EntryFee = decimal.Parse(cols[2]),
                    Active = bool.Parse(cols[3])
                };

                string[] teamIds = cols[4].Split('|');
                string[] rounds = cols[6].Split('|');

                foreach (var id in teamIds)
                {
                    tm.EnteredTeams.Add(teamList.First(x => x.Id == int.Parse(id)));
                }

                if (cols[5].Length > 0)
                {
                    string[] prizeIds = cols[5].Split('|');

                    foreach (var id in prizeIds)
                    {
                        tm.Prizes.Add(prizeList.First(x => x.Id == int.Parse(id)));
                    }
                }

                

                foreach (var round in rounds)
                {
                    string[] msText = round.Split(':');
                    List<MatchUpModel> mm = new List<MatchUpModel>();

                    foreach (string matchUpTextId in msText)
                    {
                        mm.Add(matchUps.First(x => x.Id == int.Parse(matchUpTextId)));
                    }

                    tm.Rounds.Add(mm);
                }

                output.Add(tm);
            }

            return output;
        }

        public static void SaveToPrizeFile(this List<PrizeModel> models)
        {
            List<string> lines = new List<string>();

            foreach (PrizeModel p in models)
            {
                lines.Add($"{ p.Id },{p.PlaceNumber},{p.PlaceName},{p.PrizeAmount},{p.PrizePrecentage}");
            }

            File.WriteAllLines(GlobalConfig.PrizesFile.FullFileName(), lines);
        }

        public static void SaveToPersonFile(this List<PersonModel> models, string filename)
        {
            List<string> lines = new List<string>();

            foreach (PersonModel p in models)
            {
                lines.Add($"{p.Id},{p.FirstName},{p.LastName},{p.EmailAddress},{p.Phone}");
            }

            File.WriteAllLines(filename.FullFileName(), lines);
        }

        public static void SaveToTeamFile(this List<TeamModel> models, string filename)
        {
            List<string> lines = new List<string>();

            foreach (TeamModel p in models)
            {
                lines.Add($"{p.Id},{p.TeamName},{ConvertPeopleListToString(p.TeamMembers)}");
            }

            File.WriteAllLines(filename.FullFileName(), lines);
        }

        public static void SaveToTournamentFile(this List<TournamentModel> model, string fileName)
        {
            List<string> lines = model.Select(m => string.Format(@"{0},{1},{2},{3},{4},{5}",
                m.Id, m.TournamentName,
                m.EntryFee,
                ConvertTeamListToString(m.EnteredTeams),
                ConvertPrizeListToString(m.Prizes),
                ConvertRoundListToString(m.Rounds))).ToList();

            File.WriteAllLines(fileName, lines);
        }

        public static void SaveRoundsToFile(this TournamentModel model)
        {
            // 
            // loop through each round
            // loop through each match up
            // get id for a new match up
            // save the record
            // loop though each entry and save it

            foreach (List<MatchUpModel> round in model.Rounds)
            {
                foreach (MatchUpModel match in round)
                {
                    // load all of match ups from a file
                    // get the top id and add one
                    // Store the id
                    // Save the match up record
                    match.SaveMatchUpToFile();
                }
            }
        }

        public static void SaveMatchUpToFile(this MatchUpModel model)
        {
            //0-id,1-entries (id|id|id),2-winner,3-matchupround
            List<MatchUpModel> matchUps = GlobalConfig.MatchUpFile.FullFileName().LoadFile().ConvertToMatchUpModels();

            int currentId = matchUps.Count > 0 ? matchUps.OrderByDescending(x => x.Id).First().Id + 1 : 1;

            model.Id = currentId;
            matchUps.Add(model);

            foreach (MatchupEntryModel entry in model.Entries)
            {
                entry.SaveEntryToFile();
            }

            List<string> lines = new List<string>();
            // save to file
            foreach (MatchUpModel match in matchUps)
            {
                string winner = match.Winner?.Id.ToString();
                lines.Add($@"{match.Id},{CovertEntryModelToString(match.Entries)},{winner},{match.MatchUpRound}");
            }

            File.WriteAllLines(GlobalConfig.MatchUpFile.FullFileName(), lines);
        }

        public static void SaveEntryToFile(this MatchupEntryModel model)
        {
            List<MatchupEntryModel> entries =
                GlobalConfig.MatchUpEntryFile.FullFileName().LoadFile().ConvertToMatchUpEntryModels();

            int currentId = entries.Count > 0 ? entries.OrderByDescending(x => x.Id).First().Id + 1 : 1;

            model.Id = currentId;
            entries.Add(model);

            // save to file
            List<string> lines = new List<string>();

            foreach (var entry in entries)
            {
                string parent = entry.ParentMatchUp != null ? entry.ParentMatchUp.Id.ToString() : "";
                string competing = entry.TeamCompeting != null ? entry.TeamCompeting.Id.ToString() : "";

                lines.Add($"{entry.Id},{competing},{entry.Score},{parent}");
            }

            File.WriteAllLines(GlobalConfig.MatchUpEntryFile.FullFileName(), lines);
        }

        private static string ConvertRoundListToString(List<List<MatchUpModel>> list)
        {
            if (list.Count == 0) return "";

            string output = list.Aggregate("",
                (current, matchUpList) => current + $"{ConvertMatchUpListToString(matchUpList)}|");

            return output.Substring(0, output.Length - 1);
        }

        private static string ConvertMatchUpListToString(List<MatchUpModel> list)
        {
            if (list.Count == 0) return "";

            string output = list.Aggregate("", ((current, match) => current + $"{match.Id}:"));

            return output.Substring(0, output.Length - 1);
        }

        private static string ConvertTeamListToString(List<TeamModel> list)
        {
            if (list.Count == 0) return "";

            string output = list.Aggregate("", (current, team) => current + $"{team.Id}|");

            return output.Substring(0, output.Length - 1);
        }

        private static string ConvertPrizeListToString(List<PrizeModel> list)
        {
            if (list.Count == 0) return "";

            string output = list.Aggregate("", (current, prize) => current + $"{prize.Id}|");

            return output.Substring(0, output.Length - 1);
        }

        /// <summary>
        /// Convert a list of person model to a pipe split string
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private static string ConvertPeopleListToString(List<PersonModel> list)
        {
            if (list.Count == 0) return "";

            string output = list.Aggregate("", (current, person) => current + $"{person.Id}|");

            return output.Substring(0, output.Length - 1);
        }

        private static string CovertEntryModelToString(List<MatchupEntryModel> list)
        {
            if (list.Count == 0) return "";

            string output = list.Aggregate("", (current, entry) => current + $"{entry.Id}|");

            return output.Substring(0, output.Length - 1);
        }
    }
}