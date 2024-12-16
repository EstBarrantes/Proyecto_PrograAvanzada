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
    public interface IChallengesRepository
    {
        Task<BaseResponse> CreateChallenge(int challengerId, int challengedId, string status);
        Task<BaseResponse> UpdateChallengeStatus(int challengeId, string newStatus);
        
    }
    public class ChallengesRepository : IChallengesRepository
    {
        private readonly PokemonGameEntities _context;

        public ChallengesRepository()
        {
            _context = new PokemonGameEntities();
        }
        public async Task<BaseResponse> CreateChallenge(int challengerId, int challengedId, string status)
        {
            var res = new BaseResponse();
            try
            {
                var challenge = new Challenges
                {
                    challenger_id = challengerId,
                    challenged_id = challengedId,
                    status = status,
                    challenge_date = DateTime.Now
                };

                _context.Challenges.Add(challenge);
                await _context.SaveChangesAsync();

                res.ErrorMessage = challenge.challenge_id.ToString();
                res.Success = true;
            }
            catch (Exception ex)
            {
                res.ErrorMessage = $"{ex.Message} - ChallengeRepository";
                res.Success = false;
            }
            return res;



        }

        public async Task<BaseResponse> UpdateChallengeStatus(int challengeId, string newStatus)
        {
           
            var res = new BaseResponse();
            try
            {
                var challenge = await _context.Challenges.FindAsync(challengeId);
                if (challenge != null)
                {
                    challenge.status = newStatus;
                    await _context.SaveChangesAsync();
                }
                else
                {
                    res.ErrorMessage = "No se encontro el Challenge";
                    res.Success = false;
                }

                res.ErrorMessage = "";
                res.Success = true;
            }
            catch (Exception ex)
            {
                res.ErrorMessage = $"{ex.Message} - ChallengeRepository";
                res.Success = false;
            }
            return res;


        }




    }
}
