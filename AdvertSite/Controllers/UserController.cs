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
using Microsoft.AspNetCore.Http;
using AdvertSite.Data;

namespace AdvertSite.Controllers
{
    public class UserController : Controller
    {
        private readonly advert_siteContext _context;

        public UserController(advert_siteContext context)
        {
            _context = context;
        }

        //[HttpGet]
        //public ActionResult Login()
        //{
        //    // Jei vartotojas jau prisijunges
        //    if (HttpContext.Session.GetInt32("id") != null)
        //    {
        //        //TempData["Message"] = new MessageViewModel() { CssClassName = "alert-info", Title = "!!!", Message = "Vartotojas jau prisijunges" };
        //        return RedirectToAction("Index", "Home");
        //    }

        //    Users userModel = new Users();
        //    return View(userModel);
        //}

        //[HttpGet]
        //public ActionResult Register() {
        //    // Jei vartotojas jau prisijunges
        //    if (HttpContext.Session.GetInt32("id") != null)
        //    {
        //        //TempData["Message"] = new MessageViewModel() { CssClassName = "alert-info", Title = "!!!", Message = "Vartotojas jau prisijunges" };
        //        return RedirectToAction("Index", "Home");
        //    }

        //    UserRegisterModel userModel = new UserRegisterModel();
        //    return View(userModel);
        //}


        [HttpGet]
        public ActionResult Logout()
        {
            //  await _signManager.SignOutAsync();
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Home");
        }

        //[HttpPost]
        //public async Task<IActionResult> Register(UserRegisterModel model)
        //{

        //    if (ModelState.IsValid)
        //    {
        //        var user = new Users
        //        {
        //            UserName = model.Username,
        //            Email = model.Email,
        //            Password = model.Password,
        //            RegistrationDate = DateTime.Now,
        //            Userlevel = true
        //        };
               
        //        if (_context.Users.Any(i => i.UserName == user.UserName))
        //        {
        //            TempData["Message"] = new MessageViewModel() { CssClassName = "alert-danger", Title = "Operacija nesėkminga", Message = "Vartotojo vardas užimtas" };
        //            return View(model);
        //        }
        //        if (_context.Users.Any(i => i.Email == user.Email))
        //        {
        //            TempData["Message"] = new MessageViewModel() { CssClassName = "alert-danger", Title = "Operacija nesėkminga", Message = "El. paštas jau užimtas" };
        //            return View(model);
        //        }

        //        _context.Add(user);

        //        await _context.SaveChangesAsync();

        //        // await _signManager.SignInAsync(userModel, false);

        //        TempData["Message"] = new MessageViewModel(){ CssClassName = "alert-success", Title = "Operacija sėkminga", Message = "Vartotojas užregistruotas" };
        //        return RedirectToAction(nameof(Login));
        //    }
        //    // should not get this far
        //    return View(model);
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Login(UserLogin login, string ReturnUrl = "")
        //{

        //    var checkUser = _context.Users.Where(i => i.UserName == login.Username).FirstOrDefault();
        //    if (checkUser != null)
        //    {
        //        if (string.Compare(login.Password, checkUser.Password) == 0) // reik encryptiono
        //        {
        //            HttpContext.Session.SetInt32("id", checkUser.Id);
        //            //HttpContext.Session.SetInt32("ulevel", checkUser.Userlevel);
        //            /*
        //             * Pakeisti userlevel i int vietoj boolean.
        //             */

        //            //TempData["Message"] = new MessageViewModel(){ CssClassName = "alert-success", Title = "", Message = $"Prisijungta sėkmingai. Sveikas {v.Username}" };
        //        }
        //        else
        //        {
        //            //TempData["Message"] = new MessageViewModel() { CssClassName = "alert-danger", Title = "Klaida!", Message = "Neteisingas vartotojo vardas arba slaptažodis" };
        //        }
        //    }
        //    else
        //    {
        //        //TempData["Message"] = new MessageViewModel() { CssClassName = "alert-danger", Title = "Klaida!", Message = "Neteisingas vartotojo vardas arba slaptažodis" };
        //    }
        //    return View();
        //}


        // GET: Users/Details/RandomString
        public async Task<IActionResult> Details(string id)
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