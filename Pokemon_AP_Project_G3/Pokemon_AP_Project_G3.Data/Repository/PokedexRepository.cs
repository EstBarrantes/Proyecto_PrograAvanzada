using Pokemon_AP_Project_G3.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon_AP_Project_G3.Data.Repository
{
    public interface IPokedexRepository
    {
        Task<BaseResponse<Pokemon>> GetAllPokemons();
        Task<BaseResponse<Pokemon>> GetAllPokemonsFiltered(string pName = null, string pType = null, string pWeight = null);
    }
    public class PokedexRepository : IPokedexRepository
    {
        private readonly PokemonGameEntities _context;

        public PokedexRepository()
        {
            _context = new PokemonGameEntities();
        }
        public async Task<BaseResponse<Pokemon>> GetAllPokemons()
        {
            var res = new BaseResponse<Pokemon>();
            try
            {
                res.List = await _context.Pokemon.OrderBy(x => x.pokemon_id).ToListAsync();

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

        public async Task<BaseResponse<Pokemon>> GetAllPokemonsFiltered(string pName = null, string pType = null, string pWeight = null)
        {
            var res = new BaseResponse<Pokemon>();
            try
            {
                var query = _context.Pokemon.AsQueryable();
                if (!string.IsNullOrEmpty(pName))
                {
                    query = query.Where(x => x.name.Contains(pName));
                }
                if (!string.IsNullOrEmpty(pType))
                {
                    query = query.Where(x => x.type.Contains(pType));
                }
                if (!string.IsNullOrEmpty(pWeight))
                {
                    decimal? valor = Convert.ToDecimal(pWeight, CultureInfo.GetCultureInfo("en-US"));
                    query = query.Where(x => x.weight == valor);
                }

                res.List = await query.OrderBy(x => x.pokemon_id).ToListAsync();
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
    }
}
