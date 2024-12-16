using Newtonsoft.Json;
using Pokemon_AP_Project_G3.Architecture.Services;
using Pokemon_AP_Project_G3.Data;
using Pokemon_AP_Project_G3.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

namespace Pokemon_AP_Project_G3.Controllers
{
    public class PlayController : Controller
    {
        

        private readonly IPokemonService _service;

        public PlayController()
        {
            _service = new PokemonService();
        }

        public async Task<ActionResult> Index()
        {
            var userID = Request.Cookies["UserID"].Value;
            var resTeams = await _service.GetTeamsByUserId(Convert.ToInt32(userID));

            if (resTeams.Success && resTeams.List.Any())
            {
                var teams = resTeams.List;
                var modelList = new PlayViewModel
                {
                    Teams = teams,
                };
                return View(modelList);
            }
            var model = new PlayViewModel
            {
                Teams = new List<TeamQuery>(),
            };
            return View(model); // Vista para cuando no haya equipos
        }

        [HttpPost]
        public async Task<ActionResult> SelectTeam(int teamId)
        {
            // Lógica para seleccionar el equipo del jugador
            var userID = Request.Cookies["UserID"].Value;
            var playerTeams = await _service.GetTeamsByUserId(Convert.ToInt32(userID)); // Recuperar el equipo por ID
            var selectedTeam = playerTeams.List.Where(x => x.TeamId == teamId).FirstOrDefault();
            if (selectedTeam == null)
            {
                TempData["Error"] = "Team not found!";
                return RedirectToAction("Index"); // Volver a la vista de selección
            }

            // Guardar el equipo seleccionado en session
            Session["SelectedTeam"] = JsonConvert.SerializeObject(selectedTeam);
            //HttpContext.Session.Add("SelectedTeam", JsonConvert.SerializeObject(selectedTeam));

            TempData["Message"] = $"Team seleccionado Correctamente!";
            // Redirigir a la pantalla de batalla
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<ActionResult> StartBattle()
        {
            var selectedTeamJson = Session["SelectedTeam"] as string;
            if (string.IsNullOrEmpty(selectedTeamJson))
            {
                TempData["Error"] = "No hay team seleccionado";
                return RedirectToAction("Play");
            }

            var selectedTeam = JsonConvert.DeserializeObject<TeamQuery>(selectedTeamJson);

            
            var enemyPokemon = await GenerateRandomEnemyTeam(); 

            // Initialize battle data
            Session["playerPokemon"] = selectedTeam.TeamPokemons.First(); // Select the first Pokémon from the team (for simplicity)
            Session["enemyPokemon"] = enemyPokemon.First();

            return Json(new
            {
                success = true,
                playerPokemon = new { name = ((Pokemon)Session["playerPokemon"]).name },
                enemyPokemon = new { name = enemyPokemon.First().name }
            });
        }

        [HttpPost]
        public JsonResult Attack()
        {
            var playerPokemon = (Pokemon)Session["playerPokemon"];
            var enemyPokemon = (Pokemon)Session["enemyPokemon"];

            // Random attack damage between 1 and 50
            var playerAttack = new Random().Next(1, 51);
            var enemyAttack = new Random().Next(1, 51);

            // Simulate battle
            enemyPokemon.HP -= playerAttack;
            playerPokemon.HP -= enemyAttack;

            var battleMessage = $"You attacked! {playerPokemon.name} dealt {playerAttack} damage to {enemyPokemon.name}. " +
                                $"{enemyPokemon.name} attacked! {enemyPokemon.name} dealt {enemyAttack} damage to {playerPokemon.name}.";

            // Check if anyone has lost all HP
            bool gameOver = playerPokemon.HP <= 0 || enemyPokemon.HP <= 0;
            string winner = gameOver ? (playerPokemon.HP <= 0 ? "Machine" : "Player") : string.Empty;

            return Json(new
            {
                success = true,
                message = battleMessage,
                gameOver = gameOver,
                winner = winner
            });
        }

        [HttpPost]
        public JsonResult Flee()
        {
            // If player flees, they lose the battle
            return Json(new { success = true, message = "You fled! You lose the battle." });
        }

        private async Task<List<Pokemon>> GenerateRandomEnemyTeam()
        {
            var allPokemons = await _service.GetAllPokemons(); 
            var random = new Random();
            var randomPokemons = allPokemons.List.OrderBy(x => random.Next()).Take(6).ToList();
            return randomPokemons;
        }
    }
    public class PlayViewModel
    {
        // Lista de equipos del jugador
        public List<TeamQuery> Teams { get; set; }

        // Información adicional si es necesaria
        public string Message { get; set; } // Mensaje opcional para mostrar en la vista
    }



}

    
