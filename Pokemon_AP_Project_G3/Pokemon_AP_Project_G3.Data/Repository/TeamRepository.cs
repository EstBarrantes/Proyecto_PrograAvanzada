using Pokemon_AP_Project_G3.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon_AP_Project_G3.Data.Repository
{
    public interface ITeamRepository
    {
        Task<BaseResponse<TeamQuery>> GetTeamsByUserId(int pUserId);
        Task<BaseResponse<PokedexQuery>> GetPokedexByUserId(int pUserId);
        Task<BaseResponse<Pokemon>> GetPokemonsNotOwned(int pUserId);
        Task<BaseResponse> AddPokemonsToPokedex(List<int> pokemons, int pUserId);
        Task<BaseResponse> DeletePokemonsToPokedex(int pokemon, int pUserId);
        Task<BaseResponse<Pokemon>> GetPokemon(int pPokemonId);
        Task<BaseResponse> AddEditTeam(List<int> pokemons, int pUserId, int pTeamId);
    }
    public class TeamRepository : ITeamRepository
    {
        private readonly PokemonGameEntities _context;

        public TeamRepository()
        {
            _context = new PokemonGameEntities();
        }

        public async Task<BaseResponse<TeamQuery>> GetTeamsByUserId(int pUserId)
        {
            var res = new BaseResponse<TeamQuery>();
            try
            {
                var teamsData = await (from t in _context.Teams
                                       where t.user_id == pUserId
                                       select new
                                       {
                                           TeamId = t.team_id,
                                           UserId = t.user_id,
                                           TeamPokemons = (from tp in _context.Team_Pokemon
                                                           join p2 in _context.Pokemon on tp.pokemon_id equals p2.pokemon_id
                                                           where tp.team_id == t.team_id
                                                           select new
                                                           {
                                                               p2.pokemon_id,
                                                               p2.name,
                                                               p2.type,
                                                               p2.weight,
                                                               p2.img_url_enemy
                                                           }).ToList()
                                       }).ToListAsync();

                res.List = teamsData.Select(t => new TeamQuery
                {
                    TeamId = t.TeamId,
                    UserId = t.UserId,
                    TeamPokemons = t.TeamPokemons.Select(p => new Pokemon
                    {
                        pokemon_id = p.pokemon_id,
                        name = p.name,
                        type = p.type,
                        weight = p.weight,
                        img_url_enemy = p.img_url_enemy
                    }).ToList()
                }).OrderBy(x => x.TeamId).ToList();

                res.ErrorMessage = "";
                res.Success = true;
            }
            catch (Exception ex)
            {
                res.ErrorMessage = $"{ex.Message} - TeamRepository";
                res.Success = false;
                res.List = new List<TeamQuery>();
            }
            return res;
        }

        public async Task<BaseResponse<PokedexQuery>> GetPokedexByUserId(int pUserId)
        {
            var res = new BaseResponse<PokedexQuery>();
            try
            {
                var pokedexData = await (from p in _context.Pokedex
                                         join pokemon in _context.Pokemon on p.pokemon_id equals pokemon.pokemon_id
                                         where p.user_id == pUserId
                                         select new
                                         {
                                             PokedexId = p.pokedex_id,
                                             UserId = p.user_id,
                                             Status = p.status,
                                             Pokemon = new
                                             {
                                                 pokemon_id = pokemon.pokemon_id,
                                                 name = pokemon.name,
                                                 type = pokemon.type,
                                                 weight = pokemon.weight,
                                                 img_url_enemy = pokemon.img_url_enemy
                                             }
                                         }).ToListAsync();

                res.List = pokedexData.Select(p => new PokedexQuery
                {
                    PokedexId = p.PokedexId,
                    UserId = p.UserId,
                    Status = p.Status,
                    Pokemon = new Pokemon
                    {
                        pokemon_id = p.Pokemon.pokemon_id,
                        name = p.Pokemon.name,
                        type = p.Pokemon.type,
                        weight = p.Pokemon.weight,
                        img_url_enemy = p.Pokemon.img_url_enemy
                    }
                }).OrderBy(x => x.Pokemon.pokemon_id).ToList();

                res.ErrorMessage = "";
                res.Success = true;
            }
            catch (Exception ex)
            {
                res.ErrorMessage = $"{ex.Message} - TeamRepository";
                res.Success = false;
                res.List = new List<PokedexQuery>();
            }
            return res;
        }

        public async Task<BaseResponse<Pokemon>> GetPokemonsNotOwned(int pUserId)
        {
            var res = new BaseResponse<Pokemon>();
            try
            {
                var ownedPokemonIds = await _context.Pokedex
                                                        .Where(p => p.user_id == pUserId)
                                                        .Select(p => p.pokemon_id)
                                                        .ToListAsync();

                var pokemonsNotOwned = await _context.Pokemon
                    .Where(p => !ownedPokemonIds.Contains(p.pokemon_id)) 
                    .Select(p => new 
                    {
                        pokemon_id = p.pokemon_id,
                        name = p.name,
                        type = p.type,
                        weight = p.weight,
                        img_url_enemy = p.img_url_enemy
                    })
                    .ToListAsync();

                res.List = pokemonsNotOwned.Select(p => new Pokemon
                {
                    pokemon_id = p.pokemon_id,
                    name = p.name,
                    type = p.type,
                    weight = p.weight,
                    img_url_enemy = p.img_url_enemy
                }).OrderBy(x => x.pokemon_id).ToList(); 

                res.ErrorMessage = "";
                res.Success = true;
            }
            catch (Exception ex)
            {
                res.ErrorMessage = $"{ex.Message} - TeamRepository";
                res.Success = false;
                res.List = new List<Pokemon>();
            }
            return res;
        }

        public async Task<BaseResponse> AddPokemonsToPokedex(List<int> pokemons, int pUserId)
        {
            var res = new BaseResponse();
            try
            {
                foreach (var pokemon in pokemons)
                {
                    var pokedex = new Pokedex() { 
                        pokedex_id = _context.Pokedex.Max(x => x.pokedex_id)+1,
                        pokemon_id = pokemon,
                        status = "Available",
                        user_id = pUserId
                    };
                    _context.Pokedex.AddOrUpdate(pokedex);
                    await _context.SaveChangesAsync();
                }

                res.ErrorMessage = "";
                res.Success = true;
            }
            catch (Exception ex)
            {
                res.ErrorMessage = $"{ex.Message} - TeamRepository";
                res.Success = false;
            }
            return res;
        }

        public async Task<BaseResponse> DeletePokemonsToPokedex(int pokemon, int pUserId)
        {
            var res = new BaseResponse();
            try
            {
                var pokedex = await _context.Pokedex.FirstOrDefaultAsync(p => p.pokemon_id == pokemon && p.user_id == pUserId);

                if (pokedex != null)
                {
                    _context.Pokedex.Remove(pokedex);
                    await _context.SaveChangesAsync();
                    res.ErrorMessage = "";
                    res.Success = true;
                }
                else
                {
                    res.Success = false;
                    res.ErrorMessage = "Pokémon no encontrado.";
                }
            }
            catch (Exception ex)
            {
                res.ErrorMessage = $"{ex.Message} - TeamRepository";
                res.Success = false;
            }
            return res;
        }

        public async Task<BaseResponse<Pokemon>> GetPokemon(int pPokemonId)
        {
            var res = new BaseResponse<Pokemon>();
            try
            {
                var pokemons = await _context.Pokemon.Where(p => p.pokemon_id == pPokemonId).ToListAsync();

                res.List = pokemons;

                res.ErrorMessage = "";
                res.Success = true;
            }
            catch (Exception ex)
            {
                res.ErrorMessage = $"{ex.Message} - TeamRepository";
                res.Success = false;
                res.List = new List<Pokemon>();
            }
            return res;
        }

        public async Task<BaseResponse> AddEditTeam(List<int> pokemons, int pUserId, int pTeamId)
        {
            var res = new BaseResponse();
            try
            {
                int teamId = pTeamId != 0 ? pTeamId : _context.Teams.Count() != 0 ? _context.Teams.Max(p => p.team_id) + 1 : 1;
                var team = new Teams()
                {
                    team_id = teamId,
                    user_id = pUserId
                };
                _context.Teams.AddOrUpdate(team);
                await _context.SaveChangesAsync();

                var existingTeamPokemons = _context.Team_Pokemon.Where(tp => tp.team_id == teamId).ToList();

                if (existingTeamPokemons.Count == 6)
                {
                    for (int i = 0; i < pokemons.Count; i++)
                    {
                        var teamPokemon = existingTeamPokemons[i];
                        teamPokemon.pokemon_id = pokemons[i];
                        teamPokemon.position = i; 
                        _context.Team_Pokemon.AddOrUpdate(teamPokemon);
                    }
                }
                else
                {
                    int i = 0;
                    foreach (var pokemon in pokemons)
                    {
                        int teamPokemonId = _context.Team_Pokemon.Count() != 0 ? _context.Team_Pokemon.Max(p => p.team_pokemon_id) + 1 : 1;
                        var teamPokemon = new Team_Pokemon()
                        {
                            team_id = team.team_id,
                            position = i,
                            pokemon_id = pokemon,
                            team_pokemon_id = teamPokemonId
                        };
                        _context.Team_Pokemon.AddOrUpdate(teamPokemon);
                        i++;
                    }
                }

                await _context.SaveChangesAsync();

                res.ErrorMessage = "";
                res.Success = true;
            }
            catch (Exception ex)
            {
                res.ErrorMessage = $"{ex.Message} - TeamRepository";
                res.Success = false;
            }
            return res;
        }

    }
}
