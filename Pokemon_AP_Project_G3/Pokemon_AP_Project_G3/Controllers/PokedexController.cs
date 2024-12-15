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
    public class PokedexController : Controller
    {
        private readonly IPokemonService _service;

        public PokedexController()
        {
            _service = new PokemonService();
        }

        public async Task<ActionResult> Index()
        {
            var user = Request.Cookies["UserID"].Value;
            var rol = Request.Cookies["UserRole"].Value;
            if (rol != null)
                ViewBag.RolUsuario = rol;

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

            return View("Index", model);
        }

        public async Task<ActionResult> Create()
        {
            var pokemons = await _service.GetAllPokemons();
            ViewBag.evolves_from = new SelectList(pokemons.List, "pokemon_id", "name");
            return View();
        }

        [HttpPost]
        public async  Task<ActionResult> Create([Bind(Include = "pokemon_id,name,type,weakness,weight,number,evolves_from,img_url_ally,img_url_enemy")] Pokemon pokemon)
        {
            if (ModelState.IsValid)
            {
                var res = await _service.AddOrUpdatePokemon(pokemon);
                if (res.Success)
                    return RedirectToAction("Index");
                else
                    ViewBag.Error = res.ErrorMessage;

                
            }
            else
                ViewBag.Error = "Modelo no Valido";
            var list = await _service.GetAllPokemons();
            ViewBag.evolves_from = new SelectList(list.List, "pokemon_id", "name", pokemon.evolves_from);

            return RedirectToAction("Index");
        }

        public async Task<ActionResult> Edit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var pokemon = await _service.GetOnePokemon(id);
            if (pokemon == null)
            {
                return HttpNotFound();
            }
            var list = await _service.GetAllPokemons();
            ViewBag.evolves_from = new SelectList(list.List, "pokemon_id", "name", pokemon.evolves_from);
            return View(pokemon);


            
           
        }

        [HttpPost]
        public async Task<ActionResult> Edit([Bind(Include = "pokemon_id,name,type,weakness,weight,number,evolves_from,img_url_ally,img_url_enemy")] Pokemon pokemon)
        {
            if (ModelState.IsValid)
            {


                var res = await _service.AddOrUpdatePokemon(pokemon);
                if (res.Success)
                    return RedirectToAction("Index");
                else
                    ViewBag.Error = res.ErrorMessage;


                

            }
            else
                ViewBag.Error = "Modelo no Valido";

            var list = await _service.GetAllPokemons();
            ViewBag.evolves_from = new SelectList(list.List, "pokemon_id", "name", pokemon.evolves_from);
            return View(pokemon);
        }

        public async Task<ActionResult> Delete(int id)
        {

            var res = await _service.DeletePokemon(id);
            if (res.Success)
                return RedirectToAction("Index");
            else
                ViewBag.Error = res.ErrorMessage;
            return RedirectToAction("Index");
        }

    }
    public class PokedexViewModel
    {
        public List<Pokemon> Pokemons { get; set; }

    }
}