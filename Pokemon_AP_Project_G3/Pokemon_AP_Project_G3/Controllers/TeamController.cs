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
            var resTeams = await _service.GetTeamsByUserId(1);
            var teams = new List<TeamQuery>();
            var pokedex = new List<PokedexQuery>();
            if (resTeams.Success)
                teams = resTeams.List ?? new List<TeamQuery>(); 
            else
                ViewBag.Teams = new List<TeamQuery>(); 

            var resPokedex = await _service.GetPokedexByUserId(1);
            if (resPokedex.Success)
                pokedex = resPokedex.List ?? new List<PokedexQuery>();
            else
                pokedex = new List<PokedexQuery>();


            var model = new TeamViewModel
            {
                Teams = teams,
                Pokedex = pokedex
            };
            return View(model);
        }

        
    }
    public class TeamViewModel
    {
        public List<TeamQuery> Teams { get; set; }
        public List<PokedexQuery> Pokedex { get; set; }
    }
}