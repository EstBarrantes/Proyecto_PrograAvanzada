using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pokemon_AP_Project_G3.Data.Repository;
using Pokemon_AP_Project_G3.Data.Models;
using Pokemon_AP_Project_G3.Data;

namespace Pokemon_AP_Project_G3.Architecture.Services
{
    public interface IPokemonService
    {
        Task<BaseResponse<TeamQuery>> GetTeamsByUserId(int pUserId);
        Task<BaseResponse<PokedexQuery>> GetPokedexByUserId(int pUserId);
        Task<BaseResponse<Pokemon>> GetPokemonsNotOwned(int pUserId);
        Task<BaseResponse> AddPokemonsToPokedex(List<int> pokemons, int pUserId);
        Task<BaseResponse> DeletePokemonsToPokedex(int pokemon, int pUserId);
        Task<BaseResponse<Pokemon>> GetPokemon(int pPokemonId);
        Task<BaseResponse> AddNewTeam(List<int> pokemons, int pUserId);

    }

    public class PokemonService: IPokemonService
    {
        private readonly ITeamRepository  _repository;
        public PokemonService()
        {
            _repository = new TeamRepository();
        }

        public async Task<BaseResponse<TeamQuery>> GetTeamsByUserId(int pUserId)
        {
            var res = new BaseResponse<TeamQuery>();
            try
            {
                res = await _repository.GetTeamsByUserId(pUserId);
            }
            catch(Exception ex)
            {
                res.ErrorMessage = $"{ex.Message} - Service";
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
                res = await _repository.GetPokedexByUserId(pUserId);
            }
            catch (Exception ex)
            {
                res.ErrorMessage = $"{ex.Message} - Service";
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
                res = await _repository.GetPokemonsNotOwned(pUserId);
            }
            catch (Exception ex)
            {
                res.ErrorMessage = $"{ex.Message} - Service";
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
                res = await _repository.AddPokemonsToPokedex(pokemons,pUserId);
            }
            catch (Exception ex)
            {
                res.ErrorMessage = $"{ex.Message} - Service";
                res.Success = false;
            }
            return res;
        }

        public async Task<BaseResponse> DeletePokemonsToPokedex(int pokemon, int pUserId)
        {
            var res = new BaseResponse();
            try
            {
                res = await _repository.DeletePokemonsToPokedex(pokemon, pUserId);
            }
            catch (Exception ex)
            {
                res.ErrorMessage = $"{ex.Message} - Service";
                res.Success = false;
            }
            return res;
        }

        public async Task<BaseResponse<Pokemon>> GetPokemon(int pPokemonId)
        {
            var res = new BaseResponse<Pokemon>();
            try
            {
                res = await _repository.GetPokemon(pPokemonId);
            }
            catch (Exception ex)
            {
                res.ErrorMessage = $"{ex.Message} - Service";
                res.Success = false;
                res.List = new List<Pokemon>();
            }
            return res;
        }

        public async Task<BaseResponse> AddNewTeam(List<int> pokemons, int pUserId)
        {
            var res = new BaseResponse();
            try
            {
                res = await _repository.AddNewTeam(pokemons, pUserId);
            }
            catch (Exception ex)
            {
                res.ErrorMessage = $"{ex.Message} - Service";
                res.Success = false;
            }
            return res;
        }
    }
}
