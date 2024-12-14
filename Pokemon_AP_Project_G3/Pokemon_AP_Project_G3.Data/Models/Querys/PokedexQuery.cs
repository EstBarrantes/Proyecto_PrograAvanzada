using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon_AP_Project_G3.Data.Models
{
    public class PokedexQuery
    {
        public int PokedexId { get; set; }
        public int? UserId { get; set; }
        public Pokemon Pokemon { get; set; }
        public string Status { get; set; }
    }
}
