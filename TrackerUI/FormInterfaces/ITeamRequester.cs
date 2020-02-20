using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackerLib.Models;

namespace TrackerUI.FormInterfaces
{
    public interface ITeamRequester
    {
        void TeamComplete(TeamModel team);
    }
}
