using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Pokemon_AP_Project_G3.Architecture.Services;
using Pokemon_AP_Project_G3.Data.Models;
using Pokemon_AP_Project_G3.Data;
using System.Web.Mvc;
using System.Web.UI;
using System.Threading.Tasks;

namespace Pokemon_AP_Project_G3.Controllers
{
    public class TeamController : Controller
    {
        private readonly IPokemonService _service;

        public TeamController()
        {
            _service = new PokemonService(); 
        }

        public async Task<ActionResult> Index()
        {
            var teams = new List<TeamQuery>();
            var pokedex = new List<PokedexQuery>();
            var pokemonsNO = new List<Pokemon>();

            var userID = Convert.ToInt32(Request.Cookies["UserID"].Value);

            var resTeams = await _service.GetTeamsByUserId(userID);
            if (resTeams.Success)
                teams = resTeams.List ?? new List<TeamQuery>(); 
            else
                ViewBag.Teams = new List<TeamQuery>(); 

            var resPokedex = await _service.GetPokedexByUserId(userID);
            if (resPokedex.Success)
                pokedex = resPokedex.List ?? new List<PokedexQuery>();
            else
                pokedex = new List<PokedexQuery>();

            var resPokemonsNO = await _service.GetPokemonsNotOwned(userID);
            if (resPokemonsNO.Success)
                pokemonsNO = resPokemonsNO.List ?? new List<Pokemon>();
            else
                pokemonsNO = new List<Pokemon>();


            var model = new TeamViewModel
            {
                Teams = teams,
                Pokedex = pokedex,
                PokemonsNotOwned = pokemonsNO
            };
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> AddPokemonsToPokedex(List<int> pokemons)
        {
            var userID = Convert.ToInt32(Request.Cookies["UserID"].Value);


            if (pokemons == null || pokemons.Count == 0)
            {
                return Json(new { success = false, message = "No Pokémon selected" });
            }

            var res = await _service.AddPokemonsToPokedex(pokemons, userID);

            if (res.Success)
            {
                return Json(new { success = true });
            }

            return Json(new { success = false, message = "Error adding Pokémon" });
        }

        [HttpPost]
        public async Task<ActionResult> DeletePokemon(int pokemonId)
        {
            var userID = Convert.ToInt32(Request.Cookies["UserID"].Value);


            if (pokemonId == null || pokemonId == 0)
            {
                return Json(new { success = false, message = "No Pokémon selected" });
            }

            var res = await _service.DeletePokemonsToPokedex(pokemonId, userID);

            if (res.Success)
            {
                return Json(new { success = true });
            }

            return Json(new { success = false, message = "Error deleting Pokémon" });
        }

        [HttpGet]
        public async Task<ActionResult> GetPokemonImgUrl(int pokemonId)
        {
            if (pokemonId == 0)
            {
                return Json(new { success = false, message = "No Pokémon selected" }, JsonRequestBehavior.AllowGet);
            }

            var res = await _service.GetPokemon(pokemonId);

            if (res.Success && res.List != null && res.List.Any())
            {
                return Json(new 
                { 
                    success = true, 
                    img_url_enemy = res.List.First().img_url_enemy ?? "https://img.pokemondb.net/sprites/items/poke-ball.png" 
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new { success = false, message = "Error Get Pokemon Img" }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public async Task<ActionResult> AddEditTeam(List<int> pokemonIds,int teamId)
        {
            var userID = Convert.ToInt32(Request.Cookies["UserID"].Value);

            if (pokemonIds.Count != 6)
            {
                return Json(new { success = false, message = "El equipo debe tener exactamente 6 Pokémon." });
            }

            var res = await _service.AddEditTeam(pokemonIds, userID, teamId);

            if (res.Success)
            {
                return Json(new { success = true });
            }

            return Json(new { success = false, message = "Error adding new team" });

        }

        [HttpPost]
        public async Task<ActionResult> SendToPharmacy(int pokemonId)
        {
            var userID = Convert.ToInt32(Request.Cookies["UserID"].Value);


            if ( pokemonId == 0)
            {
                return Json(new { success = false, message = "No Pokémon selected" });
            }

            var res = await _service.SendPokemonsToPharmacy(pokemonId, userID);

            if (res.Success)
            {
                return Json(new { success = true });
            }

            return Json(new { success = false, message = "Error deleting Pokémon" });
        }
    }
    public class TeamViewModel
    {
        public List<TeamQuery> Teams { get; set; }
        public List<PokedexQuery> Pokedex { get; set; }
        public List<Pokemon> PokemonsNotOwned { get; set; }
    }
}