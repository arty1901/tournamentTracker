using System;
using System.Collections.Generic;
using System.Text;

namespace TrackerLibrary
{
    public class TextConntector : IDataConnection
    {
        // TODO - wire up the create prize for text file
        public PrizeModel CreatePrize(PrizeModel model)
        {
            model.Id = 1;

            return model;
        }
    }
}
