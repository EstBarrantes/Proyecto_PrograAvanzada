using Pokemon_AP_Project_G3.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Pokemon_AP_Project_G3.Controllers
{
    public class HomeController : Controller
    {
        private readonly PokemonGameEntities _context;

        public HomeController()
        {
            _context = new PokemonGameEntities();
        }
        public ActionResult Index()
        {

            var users = _context.Users.Select(u => new SelectListItem
            {
                Value = u.username, 
                Text = u.username   
            }).ToList();

            ViewBag.UserList = users; 

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}