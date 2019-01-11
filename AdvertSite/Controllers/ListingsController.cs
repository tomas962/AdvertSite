using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AdvertSite.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace AdvertSite.Controllers
{
    public class ListingsController : Controller
    {
        private readonly advert_siteContext _context;

        public ListingsController(advert_siteContext context)
        {
            _context = context;
            
        }

        
        // GET: Listings
        [AllowAnonymous]
        public async Task<IActionResult> Index(int? id)
        {
            var masterContext = _context.Listings.Where(l => l.Verified == 1 && l.Display == 1);
            if (id  != null )
            {
                if (Request.Query["type"].Equals("Category"))
                    masterContext = masterContext.Where(l => l.Subcategory.Categoryid == id);

                else if (Request.Query["type"].Equals("Subcategory"))
                    masterContext = masterContext.Where(l => l.Subcategoryid == id);
            }

            return View(await masterContext.ToListAsync());
        }
        // GET: Uncomfirmed
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UncomfirmedListings()
        {
            var masterContext = _context.Listings.Where(l => l.Verified == 0).ToListAsync();
            return View(await masterContext);
        }

        // GET: Listings/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listings = await _context.Listings
                .Include(l => l.Subcategory)
                .Include(l => l.User)
                .Include(l => l.Subcategory.Category)
                .Include(l => l.Comments)
                .ThenInclude((Comments c) => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (listings == null)
            {
                return NotFound();
            }

            var listingAndComment = new ListingAndComment { Listing = listings, Comment = new Comments() };

            return View(listingAndComment);
        }

        // GET: Listings/Create
        [Authorize(Roles ="Admin,User")]
        public IActionResult Create()
        {
            ViewData["Subcategoryid"] = new SelectList(_context.Subcategory, "Id", "Name");
            ViewData["Userid"] = new SelectList(_context.Users, "Id", "UserName");
            return View();
        }

        // POST: Listings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles ="Admin,User")]
        public async Task<IActionResult> Create([Bind("Subcategoryid,Name,Description,Price,GoogleLatitude,GoogleLongitude,GoogleRadius")] ListingNewModel newListing)
        {
            if (ModelState.IsValid)
            {
                Listings listings = new Listings()
                {
                    Subcategoryid = newListing.Subcategoryid,
                    Name = newListing.Name,
                    Description = newListing.Description,
                    Price = newListing.Price,
                    GoogleLatitude = newListing.GoogleLatitude,
                    GoogleLongitude = newListing.GoogleLongitude,
                    GoogleRadius = newListing.GoogleRadius
                };

                listings.Userid = this.User.FindFirstValue(ClaimTypes.NameIdentifier);

                listings.Date = DateTime.Now;
                listings.Display = 1;
                listings.Verified = 0;
                /*
                listings.GoogleLongitude = 0;// newListing.GoogleLongitude;
                listings.GoogleLatitude = 0;// newListing.GoogleLatitude;
                listings.GoogleRadius = 10000;// newListing.GoogleRadius;
                */
                _context.Add(listings);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["Subcategoryid"] = new SelectList(_context.Subcategory, "Id", "Id");
            ViewData["Userid"] = new SelectList(_context.Users, "Id", "UserName");
            return View(newListing);
        }

        // GET: Listings/Edit/5
        [Authorize(Roles ="Admin,User")]
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

            if (!listings.Userid.Equals(this.User.FindFirstValue(ClaimTypes.NameIdentifier)) && !this.User.IsInRole("Admin"))
            {
                return Forbid();
            }

            ViewData["Subcategoryid"] = new SelectList(_context.Subcategory, "Id", "Id", listings.Subcategoryid);
            ViewData["Userid"] = new SelectList(_context.Users, "Id", "UserName", listings.Userid);
            return View(listings);
        }

        // POST: Listings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles ="Admin,User")]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Userid,Subcategoryid,Name,Description,Price,Quantity,Date,Verified,Display")] Listings listings)
        {
            if (!listings.Userid.Equals(this.User.FindFirstValue(ClaimTypes.NameIdentifier)) && !this.User.IsInRole("Admin"))
            {
                return Forbid();
            }

            if (id != listings.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    listings.Verified = 0;
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
            ViewData["Userid"] = new SelectList(_context.Users, "Id", "UserName", listings.Userid);
            return View(listings);
        }



        // GET: Listings/Delete/5
        [Authorize(Roles ="Admin,User")]
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
        [Authorize(Roles ="Admin,User")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var listings = await _context.Listings.FindAsync(id);
            _context.Listings.Remove(listings);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("DenyListing")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> DenyListing(int id)
        {
            var listings = await _context.Listings.FindAsync(id);
            _context.Listings.Remove(listings);
            await _context.SaveChangesAsync();
            /*
             *  Vartotojui turetu issiusti zinute, kad jo skelbimas buvo atmestas. 
             */

            // Registracijoje veikia, cia error "Microsoft.AspNetCore.Mvc.ViewFeatures.Internal.TempDataSerializer.EnsureObjectCanBeSerialized(object item)"
            // TempData["Message"] = new MessageViewModel() { CssClassName = "alert-danger", Title = "Operacija sėkminga", Message = "Skelbimas buvo atmestas" };
            return RedirectToAction(nameof(UncomfirmedListings));

        }

        [HttpPost, ActionName("ApproveListing")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> ApproveListing(int id)
        {
            var listings = await _context.Listings.FindAsync(id);
            listings.Verified = 1;
            _context.Listings.Update(listings);
            await _context.SaveChangesAsync();
            /*
             *  Vartotojui turetu issiusti zinute, kad jo skelbimas buvo priimtas.
             */

            // Registracijoje veikia, cia error "Microsoft.AspNetCore.Mvc.ViewFeatures.Internal.TempDataSerializer.EnsureObjectCanBeSerialized(object item)"
            //TempData["Message"] = new MessageViewModel() { CssClassName = "alert-success", Title = "Operacija sėkminga", Message = "Skelbimas dabar matomas kitiems vartotojams" };
            return RedirectToAction(nameof(UncomfirmedListings));
        }

        [HttpPost]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Hide(int id)
        {
            var listings = await _context.Listings.FindAsync(id);
            listings.Display = 0;
            _context.Listings.Update(listings);
            await _context.SaveChangesAsync();
            /*
             *  Vartotojui turetu issiusti zinute, kad jo skelbimas buvo priimtas.
             */

            // Registracijoje veikia, cia error "Microsoft.AspNetCore.Mvc.ViewFeatures.Internal.TempDataSerializer.EnsureObjectCanBeSerialized(object item)"
            //TempData["Message"] = new MessageViewModel() { CssClassName = "alert-success", Title = "Operacija sėkminga", Message = "Skelbimas dabar matomas kitiems vartotojams" };
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> ListingsJSON()
        {
            var listings = await _context.Listings.ToListAsync();
            return Json(listings);
        }

        private bool ListingsExists(int id)
        {
            return _context.Listings.Any(e => e.Id == id);
        }

        
    }
}
