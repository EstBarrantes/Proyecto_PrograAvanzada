//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PokemonGame_Repository
{
    using System;
    using System.Collections.Generic;
    
    public partial class Pokedex
    {
        public int pokedex_id { get; set; }
        public Nullable<int> user_id { get; set; }
        public Nullable<int> pokemon_id { get; set; }
        public string status { get; set; }
    
        public virtual Pokemon Pokemon { get; set; }
        public virtual Users Users { get; set; }
    }
}
