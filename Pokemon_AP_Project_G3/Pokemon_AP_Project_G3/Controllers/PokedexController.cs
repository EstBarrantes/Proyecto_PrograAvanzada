using Pokemon_AP_Project_G3.Architecture.Services;
using Pokemon_AP_Project_G3.Data.Models;
using Pokemon_AP_Project_G3.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Pokemon_AP_Project_G3.Controllers
{
    public class PokedexController : Controller
    {
        private readonly IPokemonService _service;

        public PokedexController()
        {
            _service = new PokemonService();
        }

        public async Task<ActionResult> Index()
        {
            var pokemons = new List<Pokemon>();

            var resPokemons = await _service.GetAllPokemons();
            if (resPokemons.Success)
                pokemons = resPokemons.List ?? new List<Pokemon>();
            else
                pokemons = new List<Pokemon>();


            var model = new PokedexViewModel
            {
                Pokemons = pokemons
            };
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> FilterPokedex(string name = null, string type = null, string weight = null)
        {
            var pokemons = new List<Pokemon>();

            var res = await _service.GetAllPokemonsFiltered(name, type, weight);
            if (res.Success)
                pokemons = res.List ?? new List<Pokemon>();
            else
                pokemons = new List<Pokemon>();

            var model = new PokedexViewModel
            {
                Pokemons = pokemons
            };

            return View("Index",model);
        }

    }
    public class PokedexViewModel
    {
        public List<Pokemon> Pokemons { get; set; }

    }
}