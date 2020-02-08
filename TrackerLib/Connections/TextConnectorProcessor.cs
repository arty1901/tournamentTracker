using System.Collections.Generic;
using System.Linq;
using System.Configuration;
using System.IO;
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
        /// Возвращаем полный путь до файла
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static string FullFileName(this string filename)
        {
            return $@"{ConfigurationManager.AppSettings.Get("filepath")}\{filename}";
        }

        /// <summary>
        /// Считываем файл
        /// </summary>
        /// <param name="file"></param>
        /// <returns>Возвращает список строк, прочитанного файла</returns>
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
        public static List<PrizeModel> ConverToPrizeModels(this List<string> lines)
        {
            List<PrizeModel> output = new List<PrizeModel>();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');

                PrizeModel p = new PrizeModel();

                p.Id = int.Parse(cols[0]);
                p.PlaceNumber = int.Parse(cols[1]);
                p.PlaceName = cols[2];
                p.PrizeAmount = decimal.Parse(cols[3]);
                p.PrizePrecentage = double.Parse(cols[4]);

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

                PersonModel p = new PersonModel();

                p.Id = int.Parse(cols[0]);
                p.FirstName = cols[1];
                p.LastName = cols[2];
                p.EmailAddress = cols[3];
                p.Phone = cols[4];

                output.Add(p);
            }

            return output;
        }

        /// <summary>
        /// Convert a list of lines to a list of Team Model
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="peopleFile"></param>
        /// <returns>List of teams model</returns>
        public static List<TeamModel> ConvertToTeamModels(this List<string> lines, string peopleFile)
        {
            // id, team name, list of ids separated by pipe
            List<TeamModel> output = new List<TeamModel>();
            List<PersonModel> persons = peopleFile.FullFileName().LoadFile().ConvertToPersonModels();

            foreach (string line in lines)
            {
                string[] cols = line.Split(',');

                TeamModel t = new TeamModel();

                t.Id = int.Parse(cols[0]);
                t.TeamName = cols[1];

                string[] personIds = cols[2].Split('|');

                foreach (string id in personIds)
                {
                    t.TeamMembers.Add(persons.First(x => x.Id == int.Parse(id)));
                }

                output.Add(t);
            }

            return output;
        }

        /// <summary>
        /// Convert a list of lines to a list of tournament model
        /// </summary>
        /// <param name="lines"></param>
        /// <param name="teamFile"></param>
        /// <param name="prizeFile"></param>
        /// <param name="personFile"></param>
        /// <returns>List of tournament model</returns>
        public static List<TournamentModel> ConvertTournamentModel(this List<string> lines, string teamFile, string prizeFile, string personFile)
        {
            // Id,tournamentName,EntryFee,Active,(id|id|id - team list),(id|id|id - prize list),(id:id:id|id:id:id|id:id:id - match list)
            List<TournamentModel> output = new List<TournamentModel>();
            List<TeamModel> teamList = teamFile.FullFileName().LoadFile().ConvertToTeamModels(personFile);
            List<PrizeModel> prizeList = prizeFile.FullFileName().LoadFile().ConverToPrizeModels();

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
                string[] prizeIds = cols[5].Split('|');
                string[] matchUpsArray = cols[6].Split('|');

                foreach (var id in teamIds)
                {
                    tm.EnteredTeams.Add(teamList.First(x => x.Id == int.Parse(id)));
                }

                foreach (var id in prizeIds)
                {
                    tm.Prizes.Add(prizeList.First(x => x.Id == int.Parse(id)));
                }

                // TODO - capture rounds info

                output.Add(tm);
            }

            return output;
        }

        public static void SaveToPrizeFile(this List<PrizeModel> models, string filename)
        {
            List<string> lines = new List<string>();

            foreach (PrizeModel p in models)
            {
                lines.Add($"{ p.Id },{p.PlaceNumber},{p.PlaceName},{p.PrizeAmount},{p.PrizePrecentage}");
            }

            File.WriteAllLines(filename.FullFileName(), lines);
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
            List<string> lines = new List<string>();

            foreach (var m in model)
            {
                lines.Add($@"{m.Id},
                                {m.TournamentName},
                                {m.EntryFee},
                                {ConvertTeamListToString(m.EnteredTeams)},
                                {ConvertPrizeListToString(m.Prizes)},
                                {ConvertRoundListToString(m.Rounds)}");
            }

            File.WriteAllLines(fileName, lines);
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
    }
}