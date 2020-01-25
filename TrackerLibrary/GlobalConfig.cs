using System;
using System.Collections.Generic;
using System.Text;

namespace TrackerLibrary
{
    public static class GlobalConfig
    {
        public static List<IDataConnection> Connections { get; private set; } = new List<IDataConnection>();

        public static void InitConnections(bool dataBase, bool textFile)
        {
           if (dataBase)
            {
                // TODO - Set up the sql connector properly
                SQLConnector sql = new SQLConnector();
                Connections.Add(sql);
            }

           if (textFile)
            {
                // TODO - Save to File
                TextConntector text = new TextConntector();
                Connections.Add(text);
            }
        }
    }
}
