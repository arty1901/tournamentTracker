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
        /// <summary>
        /// Save a new prize to a database
        /// </summary>
        /// <param name="model"></param>
        /// <returns>The prize info, including the id</returns>
        public PrizeModel CreatePrize(PrizeModel model)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString("Tournaments")))
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

        public PersonModel CreatePerson(PersonModel model)
        {
            using (IDbConnection connection = new SqlConnection(GlobalConfig.CnnString("Tournaments")))
            {
                DynamicParameters p = new DynamicParameters();
                p.Add("@FirstName", model.Firstname);
                p.Add("@LastName", model.LastName);
                p.Add("@Email", model.Email);
                p.Add("@Phone", model.Phone);
                p.Add("@Id", 0, dbType: DbType.Int32, direction: ParameterDirection.Output);

                // Надо передать имя_прецедуры, данные, тип команды, в данном случае это storedprocedure
                connection.Execute("dbo.spPerson_Insert", p, commandType: CommandType.StoredProcedure);

                //получить созданный id из базы и сохранить в модель
                model.Id = p.Get<int>("@Id");

                return model;
            }
        }
    }
}
