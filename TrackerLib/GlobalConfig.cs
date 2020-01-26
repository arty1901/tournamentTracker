using System;
using System.Collections.Generic;
using System.Text;
using TrackerLibrary.Connections;
using System.Configuration;
using TrackerLib;

namespace TrackerLibrary
{
    public static class GlobalConfig
    {
        public static IDataConnection Connection { get; private set; }

        public static void InitConnections(DataBaseType type)
        {
            switch (type)
            {
                case DataBaseType.Sql:
                    SQLConnector sql = new SQLConnector();
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

        public static string CnnString(string name)
        {
            return ConfigurationManager.ConnectionStrings[name].ConnectionString;
        }
    }
}
