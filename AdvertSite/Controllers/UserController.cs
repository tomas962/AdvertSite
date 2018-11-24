using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using AdvertSite.Models;
using Microsoft.EntityFrameworkCore;

namespace AdvertSite.Controllers
{
    public class UserController : Controller
    {
        private readonly masterContext _context;
        public UserController(masterContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Register() {
            Users userModel = new Users();
            return View(userModel);
        }

        [HttpPost]
        public async Task<IActionResult> Register(Users userModel)
        {
            userModel.RegistrationDate = DateTime.Now;
            userModel.Userlevel = 1;
            if (_context.Users.Any(i => i.Username == userModel.Username))
            {
                ViewData["Success"] = false;
                ViewData["Message"] = "Vartotojo vardas jau užimtas";
                return View(userModel);
            }
            if (_context.Users.Any(i => i.Email == userModel.Email))
            {
                ViewData["Success"] = false;
                ViewData["Message"] = "Vartotojas, su tokiu elektroniniu paštu jau egzistuoja";
                return View(userModel);
            }

                if (ModelState.IsValid)
            {
                _context.Add(userModel);

                await _context.SaveChangesAsync();
            }
            ViewData["Success"] = true;
            ViewData["Message"] = "Vartotojas užregistruotas";
            return View(userModel);
        }
    }
}