using Pokemon_AP_Project_G3.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon_AP_Project_G3.Data.Repository
{
    public interface IPharmacyRepository
    {
        Task<BaseResponse<PharmacyQuery>> GetAllPokemonsInPharmacy();
        Task<BaseResponse<PharmacyQuery>> GetAllPokemonsInPharmacyFiltered(string pStatus = null);
        Task<BaseResponse> HealPokemon(int pokemon, int pUserId);
    }
    public class PharmacyRepository : IPharmacyRepository
    {
        private readonly PokemonGameEntities _context;

        public PharmacyRepository()
        {
            _context = new PokemonGameEntities();
        }
        public async Task<BaseResponse<PharmacyQuery>> GetAllPokemonsInPharmacy()
        {
            var res = new BaseResponse<PharmacyQuery>();
            try
            {
                res.List = await (from pm in _context.Medical_Attention
                                  join us in _context.Users on pm.user_id equals us.user_id
                                  join pk in _context.Pokemon on pm.pokemon_id equals pk.pokemon_id
                                  select new PharmacyQuery()
                                  {
                                      attention_date = pm.attention_date,
                                      attention_id = pm.attention_id,
                                      pokemon_id = pm.pokemon_id,
                                      request_date = pm.request_date,
                                      status = pm.status,
                                      user_id = pm.user_id,
                                      pokemon_img_url = pk.img_url_enemy,
                                      pokemon_name = pk.name,
                                      username = us.username
                                  }).ToListAsync();

                res.ErrorMessage = "";
                res.Success = true;
            }
            catch (Exception ex)
            {
                res.ErrorMessage = $"{ex.Message} - TeamRepository";
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
                res.List = await (from pm in _context.Medical_Attention
                                  where pm.status == (!String.IsNullOrEmpty(pStatus) ? pStatus : pm.status)
                                  join us in _context.Users on pm.user_id equals us.user_id
                                  join pk in _context.Pokemon on pm.pokemon_id equals pk.pokemon_id
                                  select new PharmacyQuery() { 
                                      attention_date = pm.attention_date,
                                      attention_id = pm.attention_id,
                                      pokemon_id = pm.pokemon_id,
                                      request_date = pm.request_date,
                                      status = pm.status,
                                      user_id = pm.user_id,
                                      pokemon_img_url = pk.img_url_enemy,
                                      pokemon_name = pk.name,
                                      username = us.username
                                  }).ToListAsync();
                res.ErrorMessage = "";
                res.Success = true;
            }
            catch (Exception ex)
            {
                res.ErrorMessage = $"{ex.Message} - TeamRepository";
                res.Success = false;
                res.List = new List<PharmacyQuery>();
            }
            return res;
        }

        public async Task<BaseResponse> HealPokemon(int pokemonId, int pUserId)
        {
            var res = new BaseResponse();
            try
            {
                var medicalRecord = await _context.Medical_Attention
                    .AsNoTracking()
                    .FirstOrDefaultAsync(m => m.pokemon_id == pokemonId && m.user_id == pUserId);

                if (medicalRecord == null)
                {
                    res.ErrorMessage = "Medical record not found";
                    res.Success = false;
                    return res;
                }

                medicalRecord.status = "In Progress";
                _context.Medical_Attention.AddOrUpdate(medicalRecord);
                await _context.SaveChangesAsync();

                _ = Task.Run(async () =>
                {
                    await Task.Delay(5000); 

                    medicalRecord.status = "Completed";
                    medicalRecord.attention_date = DateTime.Now;

                    _context.Medical_Attention.AddOrUpdate(medicalRecord);
                    await _context.SaveChangesAsync();
                });

                res.ErrorMessage = "";
                res.Success = true;
            }
            catch (Exception ex)
            {
                res.ErrorMessage = $"{ex.Message} - HealPokemon";
                res.Success = false;
            }

            return res;
        }

    }
}
