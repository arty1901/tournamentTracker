﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TrackerLib.Models
{
    public class MatchupEntryModel
    {
        public TeamModel TeamCompeting { get; set; }
        public double Score { get; set; }
        public MatchUpModel ParentMatchUp { get; set; }

    }
}
