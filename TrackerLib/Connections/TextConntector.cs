using System;
using System.Collections.Generic;
using System.Text;
using TrackerLib.Models;
using TrackerLib.Connections.TextHelpers;
using System.Linq;

namespace TrackerLib.Connections
{
    public class TextConntector : IDataConnection
    {
        private static string PrizesFile = "PrizeModels.csv";
        private static string PersonsFile = "PersonModels.csv";
        private static string TeamFile = "TeamModel.csv";


        // TODO - wire up the create prize for text file
        public PrizeModel CreatePrize(PrizeModel model)
        {
            // создан параметр в app.config filepath
            // он служит для хранеия пути, куда будут сохраняться все файлы

            // План работы с сохранением файлов
            // 1. Загрузить текстовый файл
            // 2. Конвертировать текст в List<PrizeModel>
            List<PrizeModel> prizes = PrizesFile.FullFileName().LoadFile().ConverToPrizeModels();

            // 3. Найти id (max id)
            // сортируем в убывающем порядке и получаем первый элемент листа с прибавлением к нему единицы
            // это даст новый id для новой записи в файл
            int currentId = 1;
            if (prizes.Count > 0)
            {
                currentId = prizes.OrderByDescending(x => x.Id).First().Id + 1;
            }
            model.Id = currentId;

            // 4. Добавить новую запись с новым id (max + 1)
            prizes.Add(model);

            // 5. Конвертировать призы в List<string>
            // 6. Сохранить List<string> в текстовый файл
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

        public List<PersonModel> GetAllPersons()
        {
            return PersonsFile.FullFileName().LoadFile().ConvertToPersonModels();
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
    }
}
