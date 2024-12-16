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
        const int machineId = 404; // ID de la máquina

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
            var userID = Request.Cookies["UserID"].Value;

            var selectedTeamJson = Session["SelectedTeam"] as string;
            if (string.IsNullOrEmpty(selectedTeamJson))
            {
                ViewBag.Error = "No hay team seleccionado";
                return RedirectToAction("Index");
            }

            var selectedTeam = JsonConvert.DeserializeObject<TeamQuery>(selectedTeamJson);


            var enemyTeam = await GenerateRandomEnemyTeam();

            // Guardar listas completas de equipos en la sesión
            Session["playerTeam"] = selectedTeam.TeamPokemons;
            Session["enemyTeam"] = enemyTeam;

            // Seleccionar el primer Pokémon de cada equipo
            var playerPokemon = selectedTeam.TeamPokemons.First();
            var enemyPokemon = enemyTeam.First();

            // Guardar los Pokémon iniciales en sesión
            Session["playerPokemon"] = playerPokemon;
            Session["enemyPokemon"] = enemyPokemon;

            var challengeId = await _service.CreateChallenge(Convert.ToInt32(userID), machineId, "Accepted");
            if (challengeId.Success)
                Session["ChallengeId"] = challengeId.ErrorMessage;
            else
                return Json(new { success = false });

            return Json(new
            {
                success = true,
                playerPokemon = new
                {
                    name = playerPokemon.name,
                    img_url = playerPokemon.img_url_enemy,
                    hp = playerPokemon.HP
                },
                enemyPokemon = new
                {
                    name = enemyPokemon.name,
                    img_url = enemyPokemon.img_url_enemy,
                    hp = enemyPokemon.HP
                }
            });
        }

        [HttpPost]
        public async Task<JsonResult> Attack()
        {
            var playerTeam = (List<Pokemon>)Session["playerTeam"];
            var enemyTeam = (List<Pokemon>)Session["enemyTeam"];
            var playerPokemon = (Pokemon)Session["playerPokemon"];
            var enemyPokemon = (Pokemon)Session["enemyPokemon"];

            var challengeId = Session["ChallengeId"];

            var random = new Random();
            var playerAttack = random.Next(1, 51); // daño aleatorio entre 1 a 51
            var enemyAttack = random.Next(1, 51);

            enemyPokemon.HP -= playerAttack;
            playerPokemon.HP -= enemyAttack;

            var battleMessage = $"{playerPokemon.name} hizo {playerAttack} daño a {enemyPokemon.name}. " +
                                $"{enemyPokemon.name} hizo {enemyAttack} daño a {playerPokemon.name}.";

            // Rotar pokemon si alguno llega a 0 de vida
            if (playerPokemon.HP <= 0)
            {
                playerTeam.Remove(playerPokemon);
                playerPokemon = playerTeam.FirstOrDefault();
                Session["playerPokemon"] = playerPokemon;
                battleMessage += $" {playerPokemon?.name ?? "No tiene mas Pokemones"} entro a la batalla.";
            }

            if (enemyPokemon.HP <= 0)
            {
                enemyTeam.Remove(enemyPokemon);
                enemyPokemon = enemyTeam.FirstOrDefault();
                Session["enemyPokemon"] = enemyPokemon;
                battleMessage += $" {enemyPokemon?.name ?? "No tiene mas Pokemones"} entro a la batalla.";
            }

            // Verifica si alguno gano
            bool gameOver = playerTeam.Count == 0 || enemyTeam.Count == 0;
            string winner = gameOver ? (playerTeam.Count == 0 ? "Enemigo" : "Jugador") : string.Empty;
            if (!string.IsNullOrEmpty(winner))
            {
                var res = await _service.UpdateChallengeStatus(Convert.ToInt32(challengeId), "Completed");
                if (!res.Success)
                    return Json(new { success = false });
            }

            return Json(new
            {
                success = true,
                message = battleMessage,
                playerPokemon = playerPokemon != null ? new
                {
                    name = playerPokemon.name,
                    img_url = playerPokemon.img_url_enemy,
                    hp = playerPokemon.HP
                } : null,
                enemyPokemon = enemyPokemon != null ? new
                {
                    name = enemyPokemon.name,
                    img_url = enemyPokemon.img_url_enemy,
                    hp = enemyPokemon.HP
                } : null,
                gameOver = gameOver,
                winner = winner
            });
        }

        [HttpPost]
        public async Task<JsonResult> Flee()
        {
            // si el jugador huye, pierde la batalla

            var challengeId = Session["ChallengeId"];
             var res = await _service.UpdateChallengeStatus(Convert.ToInt32(challengeId), "Rejected");
            if(!res.Success) return Json(new { success = false });
            return Json(new { success = true});
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

    }



}


