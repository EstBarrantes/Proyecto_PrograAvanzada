using Pokemon_AP_Project_G3.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon_AP_Project_G3.Data.Repository
{
    public interface ITeamRepository
    {
        Task<BaseResponse<TeamQuery>> GetTeamsByUserId(int pUserId);
        Task<BaseResponse<PokedexQuery>> GetPokedexByUserId(int pUserId);
    }
    public class TeamRepository: ITeamRepository
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

                // Luego, mapeamos estos datos a objetos TeamQuery y Pokemon
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
                }).ToList();

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

                // Luego, mapeamos estos datos a objetos PokedexQuery y Pokemon
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
                }).ToList();

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
    }
}
