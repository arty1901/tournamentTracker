﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TrackerLib.Models
{
    public class PersonModel
    {
        public int Id { get; set; }
        public string  FirstName { get; set; }
        public string  LastName { get; set; }
        public string EmailAddress { get; set; }
        public string Phone { get; set; }
        public string FullName { get
            {
                return $"{FirstName} {LastName}";
            } 
        }
    }
}
