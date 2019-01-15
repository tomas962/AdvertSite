using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AdvertSite.Models;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace AdvertSite.Controllers
{
    public class ListingPicturesController : Controller
    {
        private readonly advert_siteContext _context;

        public ListingPicturesController(advert_siteContext context)
        {
            _context = context;
        }


        [AllowAnonymous]
        public async Task<IActionResult> GetPicture(int id)
        {
            var pictureInfo = await _context.ListingPictures.FirstOrDefaultAsync((pic) => pic.PictureId == id);
            if (pictureInfo == null) return NotFound();

            string path = Path.GetFullPath("UserPictures\\" + pictureInfo.FileName);

            if (!System.IO.File.Exists(path))
                return NotFound();

            return PhysicalFile(path, pictureInfo.ContentType);
        }


        // GET: ListingPictures
        public async Task<IActionResult> Index()
        {
            var advert_siteContext = _context.ListingPictures.Include(l => l.Listing);
            return View(await advert_siteContext.ToListAsync());
        }

        // GET: ListingPictures/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listingPictures = await _context.ListingPictures
                .Include(l => l.Listing)
                .FirstOrDefaultAsync(m => m.PictureId == id);
            if (listingPictures == null)
            {
                return NotFound();
            }

            return View(listingPictures);
        }

        // GET: ListingPictures/Create
        public IActionResult Create()
        {
            ViewData["ListingId"] = new SelectList(_context.Listings, "Id", "Description");
            return View();
        }

        // POST: ListingPictures/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PictureId,ListingId,FileName")] ListingPictures listingPictures)
        {
            if (ModelState.IsValid)
            {
                _context.Add(listingPictures);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ListingId"] = new SelectList(_context.Listings, "Id", "Description", listingPictures.ListingId);
            return View(listingPictures);
        }

        // GET: ListingPictures/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listingPictures = await _context.ListingPictures.FindAsync(id);
            if (listingPictures == null)
            {
                return NotFound();
            }
            ViewData["ListingId"] = new SelectList(_context.Listings, "Id", "Description", listingPictures.ListingId);
            return View(listingPictures);
        }

        // POST: ListingPictures/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PictureId,ListingId,FileName")] ListingPictures listingPictures)
        {
            if (id != listingPictures.PictureId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(listingPictures);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ListingPicturesExists(listingPictures.PictureId))
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
            ViewData["ListingId"] = new SelectList(_context.Listings, "Id", "Description", listingPictures.ListingId);
            return View(listingPictures);
        }

        // GET: ListingPictures/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listingPictures = await _context.ListingPictures
                .Include(l => l.Listing)
                .FirstOrDefaultAsync(m => m.PictureId == id);
            if (listingPictures == null)
            {
                return NotFound();
            }

            return View(listingPictures);
        }

        // POST: ListingPictures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var listingPictures = await _context.ListingPictures.FindAsync(id);
            _context.ListingPictures.Remove(listingPictures);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ListingPicturesExists(int id)
        {
            return _context.ListingPictures.Any(e => e.PictureId == id);
        }
    }
}
