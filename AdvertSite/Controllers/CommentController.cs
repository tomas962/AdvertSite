using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AdvertSite.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace AdvertSite.Controllers
{
    public class CommentController : Controller
    {
        private readonly advert_siteContext _context;

        public CommentController(advert_siteContext context)
        {
            _context = context;

        }

        // GET: Comment
        public ActionResult Index()
        {
            return View();
        }

        // GET: Comment/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Comment/Create/4
        [Authorize(Roles = "Admin,User")]
        public IActionResult Create(int id)
        {
                // Jeigu skelbimas su duotu id yra
            if (_context.Listings.Select(c => c.Id == id && c.Verified == 1 && c.Display == 1).Count() > 0)
                return View();

            else
            {
                // Erorr message : toks skelbimas neegzistuoja
            }

            // Jeigu cia atejo -  skelbimo nera/vartotojas neprisijunges
            return RedirectToAction("Index", "Home");
        }

        // POST: Comment/Create/4
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> CreateAsync(int id, Comments comment)
        {
            // Jeigu prisijunges
            if (ModelState.IsValid)
            {
                var comments = new Comments
                {
                    Listingid = id,
                    Userid = this.User.FindFirstValue(ClaimTypes.NameIdentifier),
                    Text = comment.Text
                };
                _context.Add(comments);
                await _context.SaveChangesAsync();

                return RedirectToAction("Details", "Listings", new { id = id });
            }

            // turetu sito nepasiekti
            return RedirectToAction("Index", "Home");
        }

        // GET: Comment/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Comment/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Comment/Delete/5
        [Authorize(Roles = "Admin,User")]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            if (_context.Comments.Select(c => c.Id == id).Count() > 0)
            {
                return View();
            }
            else
            {
                // Tokio komentaro nera
            }

            return RedirectToAction("Index", "Home");
        }



        // POST: Comment/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var comments = await _context.Comments.FindAsync(id);
            var listing_id = comments.Listingid;

            if (comments.Userid.Equals(this.User.FindFirstValue(ClaimTypes.NameIdentifier)) || this.User.IsInRole("Admin"))
            {
                _context.Comments.Remove(comments);
                await _context.SaveChangesAsync();
            }
            else
            {
                // Error message goes here - user cannot delete this comment
            }
            return RedirectToAction("Details", "Listings", new { id = listing_id });
        }
    }
}