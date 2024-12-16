using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pokemon_AP_Project_G3.Data.Repository;
using Pokemon_AP_Project_G3.Data.Models;
using Pokemon_AP_Project_G3.Data;
using System.Xml.Linq;

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
        Task<BaseResponse> SendPokemonsToPharmacy(int pokemon, int pUserId);
        Task<BaseResponse<Pokemon>> GetPokemon(int pPokemonId);
        Task<BaseResponse> AddEditTeam(List<int> pokemons, int pUserId, int pTeamId);

        //Pokedex
        Task<BaseResponse<Pokemon>> GetAllPokemons();
        Task<Pokemon> GetOnePokemon(int pokemonID);
        Task<BaseResponse> DeletePokemon(int pokemonID);
        Task<BaseResponse> AddOrUpdatePokemon(Pokemon pokemon);
        Task<BaseResponse<Pokemon>> GetAllPokemonsFiltered(string pName = null, string pType = null, string pWeight = null);

        //Challenges
        Task<BaseResponse> CreateChallenge(int challengerId, int challengedId, string status);
        Task<BaseResponse> UpdateChallengeStatus(int challengeId, string newStatus);
        //Pharmacy
        Task<BaseResponse<PharmacyQuery>> GetAllPokemonsInPharmacy();
        Task<BaseResponse<PharmacyQuery>> GetAllPokemonsInPharmacyFiltered(string pStatus = null);
        Task<BaseResponse> HealPokemon(int pokemon, int pUserId);

    }

    public class PokemonService: IPokemonService
    {
        private readonly ITeamRepository  _repositoryTeam;
        private readonly IPokedexRepository _repositoryPokedex;
        private readonly IChallengesRepository _repositoryChallenges;
        private readonly IPharmacyRepository _pharmacyPokedex;
        public PokemonService()
        {
            _repositoryTeam = new TeamRepository();
            _repositoryPokedex = new PokedexRepository();
            _repositoryChallenges = new ChallengesRepository();
            _pharmacyPokedex = new PharmacyRepository();
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

        public async Task<BaseResponse> SendPokemonsToPharmacy(int pokemon, int pUserId)
        {
            var res = new BaseResponse();
            try
            {
                res = await _repositoryTeam.SendPokemonsToPharmacy(pokemon, pUserId);
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

        public async Task<BaseResponse> AddEditTeam(List<int> pokemons, int pUserId, int pTeamId)
        {
            var res = new BaseResponse();
            try
            {
                res = await _repositoryTeam.AddEditTeam(pokemons, pUserId, pTeamId);
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
        public async Task<Pokemon> GetOnePokemon(int pokemonID)
        {
            var res = new Pokemon();
            try
            {
                res = await _repositoryPokedex.GetOnePokemon(pokemonID);
            }
            catch (Exception)
            {
                res = new Pokemon();
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

       

        public async Task<BaseResponse> DeletePokemon(int pokemonID)
        {
            var res = new BaseResponse();
            try
            {
                res = await _repositoryPokedex.DeletePokemon(pokemonID);
            }
            catch (Exception ex)
            {
                res.ErrorMessage = $"{ex.Message} - Service";
                res.Success = false;
            }
            return res;
        }

        public async Task<BaseResponse> AddOrUpdatePokemon(Pokemon pokemon)
        {
            var res = new BaseResponse();
            try
            {
                if (pokemon.pokemon_id == 0)
                {
                   res = await _repositoryPokedex.AddPokemon(pokemon);
                }
                else
                {
                    res = await _repositoryPokedex.UpdatePokemon(pokemon);
                };
            }
            catch (Exception ex)
            {
                res.ErrorMessage = $"{ex.Message} - Service";
                res.Success = false;
            }
            return res;
        }


        #endregion

        #region Challenges
        public async Task<BaseResponse> CreateChallenge(int challengerId, int challengedId, string status)
        #region Pharmacy
        public async Task<BaseResponse<PharmacyQuery>> GetAllPokemonsInPharmacy()
        {
            var res = new BaseResponse<PharmacyQuery>();
            try
            {
                res = await _pharmacyPokedex.GetAllPokemonsInPharmacy();
            }
            catch (Exception ex)
            {
                res.ErrorMessage = $"{ex.Message} - Service";
                res.Success = false;
                res.List = new List<PharmacyQuery>();
            }
            return res;
        }

        public async Task<BaseResponse<PharmacyQuery>> GetAllPokemonsInPharmacyFiltered(string pStatus = null)
        {
            var res = new BaseResponse<PharmacyQuery>();
            try
            {
                res = await _pharmacyPokedex.GetAllPokemonsInPharmacyFiltered(pStatus);
            }
            catch (Exception ex)
            {
                res.ErrorMessage = $"{ex.Message} - Service";
                res.Success = false;
                res.List = new List<PharmacyQuery>();
            }
            return res;
        }



        public async Task<BaseResponse> HealPokemon(int pokemon, int pUserId)
        {
            var res = new BaseResponse();
            try
            {
                res = await _repositoryChallenges.CreateChallenge(challengerId, challengedId, status);
            }
            catch (Exception ex)
            {
                res.ErrorMessage = $"{ex.Message} - Service";
                res.Success = false;
            }
            return res;
        }
        public async Task<BaseResponse> UpdateChallengeStatus(int challengeId, string newStatus)
        {
            var res = new BaseResponse();
            try
            {
                res = await _repositoryChallenges.UpdateChallengeStatus(challengeId,newStatus);
                res = await _pharmacyPokedex.HealPokemon(pokemon,pUserId);
            }
            catch (Exception ex)
            {
                res.ErrorMessage = $"{ex.Message} - Service";
                res.Success = false;
            }
            return res;
        }
               


        #endregion
    }
}
