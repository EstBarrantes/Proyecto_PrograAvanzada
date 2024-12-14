using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon_AP_Project_G3.Data.Models
{
    public class TeamQuery
    {
        public int TeamId { get; set; }
        public int? UserId { get; set; }
        public List<Pokemon> TeamPokemons { get; set; }
    }
}
