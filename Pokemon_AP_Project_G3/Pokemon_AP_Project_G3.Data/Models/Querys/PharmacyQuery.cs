using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon_AP_Project_G3.Data.Models
{
    public class PharmacyQuery
    {
        public int attention_id { get; set; }
        public int? user_id { get; set; }
        public string username { get; set; }
        public int? pokemon_id { get; set; }
        public string pokemon_name { get; set; }
        public string pokemon_img_url { get; set; }
        public DateTime? request_date { get; set; }
        public DateTime? attention_date { get; set; }
        public string status { get; set; }
    }
}
