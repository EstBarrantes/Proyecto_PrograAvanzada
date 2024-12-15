using System;
using Pokemon_AP_Project_G3.Data.Repository;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Pokemon_AP_Project_G3.Data;

namespace Pokemon_AP_Project_G3.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUserRepository _userrepository;

        public LoginController()
        {
            _userrepository = new UserRepository();
        }

        public ActionResult Index()
        {
            return View();
        }

        // POST: Login
        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "El nombre de usuario y la contraseña son requeridos";
                return View("Index");
            }

            var user = _userrepository.GetUserByusernameAndPassword(username, password);

            if (user != null )
            {
                HttpCookie authCookie = new HttpCookie("UserID", user.user_id.ToString())
                {
                    Expires = DateTime.Now.AddHours(1),
                    HttpOnly = true
                };
                HttpCookie secCookie = new HttpCookie("UserRole", user.role)
                {
                    Expires = DateTime.Now.AddHours(1),
                    HttpOnly = true
                };
                Response.Cookies.Add(authCookie);
                Response.Cookies.Add(secCookie);

                return RedirectToAction("Index", "Home");
            }

            ViewBag.Error = "Contraseña o Usuario invalido.";
            return View("Index");
        }
        //GET: Register 
        public ActionResult Register()
        {
            return View();
        }

        // POST: Register
        [HttpPost]
        public ActionResult Register(string username, string nombre, string gender, string role, string password)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(gender) || string.IsNullOrWhiteSpace(role))
            {
                ViewBag.Error = "Todos los campos son requeridos.";
                return View();
            }

            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
            {
                ViewBag.Error = "El usuario y la contraseña son requeridos";
                return View();
            }

            if (_userrepository.GetUserByusernameAndPassword(username, null) != null)
            {
                ViewBag.Error = "El usuario ya fue registrado.";
                return View();
            }

            var user = new Users
            {
                username = username,
                name = nombre,
                gender = gender,
                role = role,
                password_hash = password, 
                registration_date = DateTime.Now
            };

            if (_userrepository.RegisterUser(user))
            {
                ViewBag.Message = "La cuenta se creo exitosamente. Por favor inicie sesión.";
                return RedirectToAction("Index");
            }

            ViewBag.Error = "hubo un error al crear la cuenta. Por favor intentelo nuevamente.";
            return View();
        }

        public ActionResult Logoff()
        {

            if (Request.Cookies["AuthUser"] != null)
            {

                var authCookie = new HttpCookie("AuthUser") { Expires = DateTime.Now.AddDays(-1) };
                Response.Cookies.Add(authCookie);
            }


            return RedirectToAction("Index", "Login");
        }




    }
}