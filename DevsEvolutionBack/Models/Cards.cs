﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevsEvolutionBack.Models
{
    public class Cards
    {
        internal string text;

        internal string position;

        internal string direction;

        public int id { get; set; }

        public int userId { get; set; }

        public string name { get; set; }

        public string description { get; set; }

        public string pillar { get; set; }

        public List<Tasks> Task { get; set; }

        public Users Users { get; set; }
        
    }
}
