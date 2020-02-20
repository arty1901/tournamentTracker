using TrackerLib.Models;

namespace TrackerUI.FormInterfaces
{
    public interface ITournamentRequester
    {
        void TournamentComplete(TournamentModel model);
    }
}