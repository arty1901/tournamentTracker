using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TrackerLib;

namespace TrackerUI
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Init DB connections
            TrackerLibrary.GlobalConfig.InitConnections(DataBaseType.Sql);
            Application.Run(new CreatePrizeForm());

            //Application.Run(new TournamentDashboardForm());
        }
    }
}
