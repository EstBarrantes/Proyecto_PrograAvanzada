using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Pokemon_AP_Project_G3.Data
{
    public partial class Pokemon
    {
        public int HP { get; set; }
        public int Attack { get; set; }
        public int Defense { get; set; }

        public Pokemon(string pName, int hp, int attack, int defense)
        {
            name = pName;
            HP = hp;
            Attack = attack;
            Defense = defense;
        }
    }
}
