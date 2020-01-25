using System;
using System.Collections.Generic;
using System.Text;
using TrackerLibrary.Models;

namespace TrackerLibrary.Connections
{
    public class SQLConnector : IDataConnection
    {
        // TODO - Make the createprize method actually save data to DB
        /// <summary>
        /// Save a new prize to a database
        /// </summary>
        /// <param name="model"></param>
        /// <returns>The prize info, including the id</returns>
        public PrizeModel CreatePrize(PrizeModel model)
        {
            model.Id = 1;

            return model;
        }
    }
}
