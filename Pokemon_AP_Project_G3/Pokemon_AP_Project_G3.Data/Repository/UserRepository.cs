using Pokemon_AP_Project_G3.Data.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace Pokemon_AP_Project_G3.Data.Repository
{
    public interface IUserRepository
    {
        Users GetUserByusernameAndPassword(string username, string password);
        bool RegisterUser(Users user);
    }


    public class UserRepository : IUserRepository
    {

        private readonly PokemonGameEntities _context;
        public UserRepository()
        {
            _context = new PokemonGameEntities();
        }
        public Users GetUserByusernameAndPassword(string username, string password)
        {
            return _context.Users.FirstOrDefault(u => u.username == username && u.password_hash == password);
        }

        public bool RegisterUser(Users user)
        {
            try
            {
                _context.Users.Add(user);
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
