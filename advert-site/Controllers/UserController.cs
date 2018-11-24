using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using advert_site.Models;

namespace advert_site.Controllers
{
    public class UserController : Controller
    {
        [HttpGet] 
        public IActionResult Register() { 
            return View(); 
        }

        [HttpPost] 
        public IActionResult Register (RegisterViewModel model) {  
            ViewData["Message"] = model.username;
             return View(); 
        }
    }
}
