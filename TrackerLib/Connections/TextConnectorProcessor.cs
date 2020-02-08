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
        public static List<PrizeModel> ConverToPrizeModels( this List<string> lines)
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
                    t.TeamMembers.Add(persons.Where(x => x.Id == int.Parse(id)).First());
                }

                output.Add(t);
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
                lines.Add($"{p.Id},{p.TeamName},{ConvertListToString(p.TeamMembers)}");
            }

            File.WriteAllLines(filename.FullFileName(), lines);
        }

        private static string ConvertListToString(List<PersonModel> list)
        {
            string output = "";

            if (list.Count == 0)
            {
                return "";
            }

            foreach (PersonModel person in list)
            {
                output += $"{person.Id}|";
            }

            return output.Substring(0, output.Length - 1);
        }
    }
}