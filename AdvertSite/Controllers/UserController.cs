using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using AdvertSite.Models;
using Microsoft.EntityFrameworkCore;
using System.Web;

namespace AdvertSite.Controllers
{
    public class UserController : Controller
    {
        private readonly advert_siteContext _context;

        public UserController(advert_siteContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult Login()
        {
            Users userModel = new Users();
            return View(userModel);
        }

        [HttpGet]
        public ActionResult Register() {
            UserRegisterModel userModel = new UserRegisterModel();
            return View(userModel);
        }


        [HttpPost]
        public async Task<IActionResult> Logout()
        {
          //  await _signManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserRegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new Users
                {
                    Username = model.Username,
                    Email = model.Email,
                    Password = model.Password,
                    RegistrationDate = DateTime.Now,
                    Userlevel = true
                };
               
                if (_context.Users.Any(i => i.Username == user.Username))
                {
                    TempData["Message"] = new MessageViewModel() { CssClassName = "alert-danger", Title = "Operacija nesėkminga", Message = "Vartotojo vardas užimtas" };
                    return View(model);
                }
                if (_context.Users.Any(i => i.Email == user.Email))
                {
                    TempData["Message"] = new MessageViewModel() { CssClassName = "alert-danger", Title = "Operacija nesėkminga", Message = "El. paštas jau užimtas" };
                    return View(model);
                }

                _context.Add(user);

                await _context.SaveChangesAsync();

                // await _signManager.SignInAsync(userModel, false);

                TempData["Message"] = new MessageViewModel(){ CssClassName = "alert-success", Title = "Operacija sėkminga", Message = "Vartotojas užregistruotas" };
                return RedirectToAction(nameof(Login));
            }
            // should not get this far
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(UserLogin login, string ReturnUrl = "")
        {
            string message = "";
            var v = _context.Users.Where(i => i.Username == login.Username).FirstOrDefault();
            if (v != null)
            {
                if (string.Compare(login.Password, v.Password) == 0) // reik encryptiono
                {
                    int timeout = 5;    // 5min timeout
          //          var ticket = new FormsAuthenticationTicket(login.UserId, false, timeout);

                }
                else
                {
                    message = "Neteisingai vartotojo duomenys";
                }
            }
            else
            {
                message = "Neteisingai vartotojo duomenys";
            }
            ViewBag.Message = message;
            return View();
        }


        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var user = await _context.Users
                .Include(u => u.Listings)
                .Include(u => u.ReviewsSeller)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            return View(user);
        }
    }
}