using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DevsEvolutionBack.Models
{
    public class Tasks
    {
        public int id { get; set; }

        public int cardId { get; set; }

        public string text { get; set; }

        public bool done { get; set; }

        public Cards Cards { get; set; }
    }
}
