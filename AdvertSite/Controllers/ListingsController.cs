using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AdvertSite.Models;

namespace AdvertSite.Controllers
{
    public class ListingsController : Controller
    {
        private readonly masterContext _context;

        public ListingsController(masterContext context)
        {
            _context = context;
        }

        // GET: Listings
        public async Task<IActionResult> Index()
        {
            var masterContext = _context.Listings.Include(l => l.Subcategory).Include(l => l.User);
            return View(await masterContext.ToListAsync());
        }

        // GET: Listings/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listings = await _context.Listings
                .Include(l => l.Subcategory)
                .Include(l => l.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (listings == null)
            {
                return NotFound();
            }

            return View(listings);
        }

        // GET: Listings/Create
        public IActionResult Create()
        {
            ViewData["Subcategoryid"] = new SelectList(_context.Subcategory, "Id", "Id");
            ViewData["Userid"] = new SelectList(_context.Users, "Id", "Username");
            return View();
        }

        // POST: Listings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Userid,Subcategoryid,Name,Description,Price,Quantity,Date,Verified,Display")] Listings listings)
        {
            if (ModelState.IsValid)
            {
                _context.Add(listings);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Subcategoryid"] = new SelectList(_context.Subcategory, "Id", "Id", listings.Subcategoryid);
            ViewData["Userid"] = new SelectList(_context.Users, "Id", "Username", listings.Userid);
            return View(listings);
        }

        // GET: Listings/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listings = await _context.Listings.FindAsync(id);
            if (listings == null)
            {
                return NotFound();
            }
            ViewData["Subcategoryid"] = new SelectList(_context.Subcategory, "Id", "Id", listings.Subcategoryid);
            ViewData["Userid"] = new SelectList(_context.Users, "Id", "Username", listings.Userid);
            return View(listings);
        }

        // POST: Listings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Userid,Subcategoryid,Name,Description,Price,Quantity,Date,Verified,Display")] Listings listings)
        {
            if (id != listings.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(listings);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ListingsExists(listings.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["Subcategoryid"] = new SelectList(_context.Subcategory, "Id", "Id", listings.Subcategoryid);
            ViewData["Userid"] = new SelectList(_context.Users, "Id", "Username", listings.Userid);
            return View(listings);
        }

        // GET: Listings/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listings = await _context.Listings
                .Include(l => l.Subcategory)
                .Include(l => l.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (listings == null)
            {
                return NotFound();
            }

            return View(listings);
        }

        // POST: Listings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var listings = await _context.Listings.FindAsync(id);
            _context.Listings.Remove(listings);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ListingsExists(int id)
        {
            return _context.Listings.Any(e => e.Id == id);
        }
    }
}
