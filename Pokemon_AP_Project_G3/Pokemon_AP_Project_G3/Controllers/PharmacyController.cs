using Pokemon_AP_Project_G3.Architecture.Services;
using Pokemon_AP_Project_G3.Data.Models;
using Pokemon_AP_Project_G3.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Xml.Linq;
using System.Net;

namespace Pokemon_AP_Project_G3.Controllers
{
    public class PharmacyController : Controller
    {
        private readonly IPokemonService _service;

        public PharmacyController()
        {
            _service = new PokemonService();
        }

        public async Task<ActionResult> Index()
        {
            var list = new List<PharmacyQuery>();

            var resPokemons = await _service.GetAllPokemonsInPharmacy();
            if (resPokemons.Success)
                list = resPokemons.List ?? new List<PharmacyQuery>();
            else
                list = new List<PharmacyQuery>();


            var model = new PharmacyViewModel
            {
                Pokemons = list
            };
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> Filter(string status = null)
        {
            var pokemons = new List<PharmacyQuery>();

            var res = await _service.GetAllPokemonsInPharmacyFiltered(status);
            if (res.Success)
                pokemons = res.List ?? new List<PharmacyQuery>();
            else
                pokemons = new List<PharmacyQuery>();

            var model = new PharmacyViewModel
            {
                Pokemons = pokemons
            };

            return View("Index", model);
        }

        [HttpPost]
        public async Task<ActionResult> HealPokemon(int pokemonId, int userID)
        {
            
            if (pokemonId == 0)
            {
                return Json(new { success = false, message = "No Pokémon selected" });
            }

            var res = await _service.HealPokemon(pokemonId, userID);

            if (res.Success)
            {
                return Json(new { success = true });
            }

            return Json(new { success = false, message = "Error healing Pokémon" });
        }

    }
    public class PharmacyViewModel
    {
        public List<PharmacyQuery> Pokemons { get; set; }

    }
}