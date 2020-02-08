using System;
using System.Collections.Generic;
using System.Text;
using TrackerLib.Connections;
using System.Configuration;
using TrackerLib;

namespace TrackerLib
{
    public static class GlobalConfig
    {
        public static IDataConnection Connection { get; private set; }

        /// <summary>
        /// Initiation of connection
        /// </summary>
        /// <param name="type"></param>
        public static void InitConnections(DataBaseType type)
        {
            switch (type)
            {
                case DataBaseType.Sql:
                    SqlConnector sql = new SqlConnector();
                    Connection = sql;
                    break;
                case DataBaseType.TextFile:
                    TextConntector text = new TextConntector();
                    Connection = text;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Get ConnectionString for connecting to DB
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static string CnnString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
    }
}
