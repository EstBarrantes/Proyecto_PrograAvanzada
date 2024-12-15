﻿using System;
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

        //Team Builder
        Task<BaseResponse<TeamQuery>> GetTeamsByUserId(int pUserId);
        Task<BaseResponse<PokedexQuery>> GetPokedexByUserId(int pUserId);
        Task<BaseResponse<Pokemon>> GetPokemonsNotOwned(int pUserId);
        Task<BaseResponse> AddPokemonsToPokedex(List<int> pokemons, int pUserId);
        Task<BaseResponse> DeletePokemonsToPokedex(int pokemon, int pUserId);
        Task<BaseResponse<Pokemon>> GetPokemon(int pPokemonId);
        Task<BaseResponse> AddNewTeam(List<int> pokemons, int pUserId);

        //Pokedex
        Task<BaseResponse<Pokemon>> GetAllPokemons();
        Task<BaseResponse<Pokemon>> GetAllPokemonsFiltered(string pName = null, string pType = null, string pWeight = null);

    }

    public class PokemonService: IPokemonService
    {
        private readonly ITeamRepository  _repositoryTeam;
        private readonly IPokedexRepository _repositoryPokedex;
        public PokemonService()
        {
            _repositoryTeam = new TeamRepository();
            _repositoryPokedex = new PokedexRepository();
        }

        #region Team Builder
        public async Task<BaseResponse<TeamQuery>> GetTeamsByUserId(int pUserId)
        {
            var res = new BaseResponse<TeamQuery>();
            try
            {
                res = await _repositoryTeam.GetTeamsByUserId(pUserId);
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
                res = await _repositoryTeam.GetPokedexByUserId(pUserId);
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
                res = await _repositoryTeam.GetPokemonsNotOwned(pUserId);
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
                res = await _repositoryTeam.AddPokemonsToPokedex(pokemons,pUserId);
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
                res = await _repositoryTeam.DeletePokemonsToPokedex(pokemon, pUserId);
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
                res = await _repositoryTeam.GetPokemon(pPokemonId);
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
                res = await _repositoryTeam.AddNewTeam(pokemons, pUserId);
            }
            catch (Exception ex)
            {
                res.ErrorMessage = $"{ex.Message} - Service";
                res.Success = false;
            }
            return res;
        }

        #endregion

        #region Pokedex
        public async Task<BaseResponse<Pokemon>> GetAllPokemons()
        {
            var res = new BaseResponse<Pokemon>();
            try
            {
                res = await _repositoryPokedex.GetAllPokemons();
            }
            catch (Exception ex)
            {
                res.ErrorMessage = $"{ex.Message} - Service";
                res.Success = false;
                res.List = new List<Pokemon>();
            }
            return res;
        }

        public async Task<BaseResponse<Pokemon>> GetAllPokemonsFiltered(string pName = null, string pType = null, string pWeight = null)
        {
            var res = new BaseResponse<Pokemon>();
            try
            {
                res = await _repositoryPokedex.GetAllPokemonsFiltered(pName,pType,pWeight);
            }
            catch (Exception ex)
            {
                res.ErrorMessage = $"{ex.Message} - Service";
                res.Success = false;
                res.List = new List<Pokemon>();
            }
            return res;
        }
        #endregion
    }
}
