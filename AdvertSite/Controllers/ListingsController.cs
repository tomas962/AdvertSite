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
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Text.RegularExpressions;
using System.Drawing;
using Microsoft.EntityFrameworkCore.Query;

namespace AdvertSite.Controllers
{
    public class ListingsController : Controller
    {
        private readonly advert_siteContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ListingsController(advert_siteContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }


        // GET: Listings
        [AllowAnonymous]
        public ViewResult Index(int? id)
        {

            var list = GetListingsByCategoriesAndSubCategories(Request.Query["type"], Request.Query["key"], id);

            return View(list);
        }

        // GET: Uncomfirmed
        [Authorize(Roles = "Admin")]
        public IActionResult UncomfirmedListings()
        {
            return View(GetUncomfirmedListings());
        }

        // GET: Listings/Details/5
        [AllowAnonymous]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var listing = await GetListingWithAdditionalInformation((int)id);

            if (listing == null)
            {
                return NotFound();
            }

            var listingAndComment = new ListingAndComment { Listing = listing, Comment = new Comments() };

            return View(listingAndComment);
        }

        // GET: Listings/Create
        [Authorize(Roles ="Admin,User")]
        public IActionResult Create(ListingNewModel listingModel)
        {
            ViewData["Subcategoryid"] = new SelectList(_context.Subcategory, "Id", "Name");
            ViewData["Userid"] = new SelectList(_context.Users, "Id", "UserName");
            if (TempData.ContainsKey("PictureError")) ViewData["PictureError"] = TempData["PictureError"];
            ModelState.Clear();
            return View(listingModel);
        }

        // POST: Listings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles ="Admin,User")]
        [ActionName("Create")]
        public async Task<IActionResult> CreatePost([Bind("Subcategoryid,Name,Description,Price,GoogleLatitude,GoogleLongitude,GoogleRadius,ListingPictures")] ListingNewModel newListing)
        {
            if (ModelState.IsValid)
            {

                Listings listings = CreateListing(newListing);

                if (newListing.ListingPictures != null && newListing.ListingPictures.Count() > 4) //jei nuotrauku daugiau nei 4 atmetam 
                {
                    TempData["PictureError"] = "Nuotraukų negali būti daugiau nei 4!";                    
                    return RedirectToAction(nameof(Create),newListing);
                }

                if (newListing.ListingPictures != null)
                {
                    foreach (var picture in newListing.ListingPictures)
                    {
                        if (picture.Length > ImageMaximumBytes) //jei dydid didesnis uz 10MB atmeta
                        {
                            TempData["PictureError"] = "Nuotraukos dydis negali būti didesnis nei 10Mb!";
                            return RedirectToAction(nameof(Create), newListing);
                        }

                        if (!IsImage(picture))
                        {
                            TempData["PictureError"] = "Failas nėra nuotrauka!";
                            return RedirectToAction(nameof(Create), newListing);
                        }
                    }

                    foreach (var picture in newListing.ListingPictures)
                    {
                        await CreatePicture(listings.Id, picture);
                    }
                }
                await _context.SaveChangesAsync();
                TempData["Success"] = "Jūsų skelbimas bus patalpintas, kai administratorius jį patikrins";
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

            var listings = await GetListingWithoutAdditionalsInfomration((int)id);

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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Userid,Subcategoryid,Name,Description,Price,Quantity,Date,Verified,Display,GoogleLatitude,GoogleLongitude,GoogleRadius")] Listings listings)
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
                    await EditUserListing(listings);
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

            var listings = await GetListingWithAdditionalInformation((int)id);
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
           await  DeleteUserListing(id);

            return RedirectToAction(nameof(Index));
        }

        [HttpPost, ActionName("DenyListing")]
        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> DenyListing(int id)
        {
            await DeleteUserListing(id);
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
            await ApproveUserListing(id);
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
            await HideUserListing(id);
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

        #region HelperMethods
        //--------------------------------------------------------HELPER METHODS---------------------------------------------------

        public async Task ApproveUserListing(int id)
        {
            var listings = await _context.Listings.FindAsync(id);
            listings.Verified = 1;
            _context.Listings.Update(listings);
            await _context.SaveChangesAsync();
        }


        // Also, if denied
        public async Task DeleteUserListing(int id)
        {
            var listings = await _context.Listings.FindAsync(id);
            _context.Listings.Remove(listings);
            await _context.SaveChangesAsync();
        }

        public async Task HideUserListing(int id)
        {
            var listings = await _context.Listings.FindAsync(id);
            listings.Display = 0;
            _context.Listings.Update(listings);
            await _context.SaveChangesAsync();
        }

        public async Task EditUserListing(Listings listings)
        {
            listings.Display = 1;
            listings.Verified = 0;
            _context.Update(listings);
            await _context.SaveChangesAsync();
        }

        public async Task CreatePicture(int listingId, IFormFile picture)
        {
            if (picture.Length > 0)
            {
                var pic = new ListingPictures { ListingId = listingId, ContentType = picture.ContentType };
                _context.Add(pic);
                await _context.SaveChangesAsync();

                string[] filenameAndExtension = picture.FileName.Split('.');
                filenameAndExtension[0] = pic.PictureId.ToString();

                string fileName = filenameAndExtension[0] + "." + filenameAndExtension[1];

                string path = "UserPictures" + "\\" + fileName;
                path = Path.GetFullPath(path);

                pic.FileName = fileName;
                _context.ListingPictures.Update(pic);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await picture.CopyToAsync(stream);
                }
            }
        }
        public async  Task<List<Listings>> GetListingsByCategoriesAndSubCategories(String queryType = null, String queryKey = null, int? id = null)
        {
            queryKey = queryKey.ToLower();
            var result = _context.Listings.Where(l => l.Verified == 1 && l.Display == 1).Include(l => l.ListingPictures);
            if (queryType != null)
            {
                if (queryType.Equals("Category"))
                    result = result
                       .Where(l => l.Subcategory.Categoryid == id)
                       .Include(l => l.ListingPictures);
                else if (queryType.Equals("Subcategory"))
                    result = result
                        .Where(l => l.Subcategoryid == id)
                        .Include(l => l.ListingPictures);
                else if (queryType.Equals("Search"))
                    if (queryKey != null)
                        result = result
                            .Where(l => l.Name.ToLower().Contains(queryKey) || l.Description.ToLower().Contains(queryKey))
                            .Include(l => l.ListingPictures);
                    else if (queryType.Equals("MyListings"))
                        result = _context.Listings
                            .Where(l => l.Userid.Equals(this.User.FindFirstValue(ClaimTypes.NameIdentifier)))
                            .Include(l => l.ListingPictures);
            }


            return await result.ToListAsync();
        }

        public async Task<Listings> GetListingWithAdditionalInformation(int id)
        {
            return await _context.Listings
                .Include(l => l.ListingPictures)
                .Include(l => l.Subcategory)
                .Include(l => l.User)
                .Include(l => l.Subcategory.Category)
                .Include(l => l.Comments)
                .ThenInclude((Comments c) => c.User)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Listings> GetListingWithoutAdditionalsInfomration(int id)
        {
            return await _context.Listings.FindAsync(id);
        }

        public async Task<List<Listings>> GetUncomfirmedListings()
        {
            return await _context.Listings.Where(l => l.Verified == 0).Include(l => l.ListingPictures).ToListAsync();
        }

        public bool ListingsExists(int id)
        {
            return _context.Listings.Any(e => e.Id == id);
        }

        public Listings CreateListing(ListingNewModel newListing)
        {
            Listings listings = new Listings()
            {
                Subcategoryid = newListing.Subcategoryid,
                Name = newListing.Name,
                Description = newListing.Description,
                Price = newListing.Price,
                GoogleLatitude = newListing.GoogleLatitude,
                GoogleLongitude = newListing.GoogleLongitude,
                GoogleRadius = newListing.GoogleRadius * 1000
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
            _context.SaveChanges();

            return listings;
        }
        public const int ImageMaximumBytes = 10000000;

        public static bool IsImage(IFormFile postedFile)
        {
            //-------------------------------------------
            //  Check the image mime types
            //-------------------------------------------
            if (postedFile.ContentType.ToLower() != "image/jpg" &&
                        postedFile.ContentType.ToLower() != "image/jpeg" &&
                        postedFile.ContentType.ToLower() != "image/pjpeg" &&
                        postedFile.ContentType.ToLower() != "image/gif" &&
                        postedFile.ContentType.ToLower() != "image/x-png" &&
                        postedFile.ContentType.ToLower() != "image/png")
            {
                return false;
            }

            //-------------------------------------------
            //  Check the image extension
            //-------------------------------------------
            if (Path.GetExtension(postedFile.FileName).ToLower() != ".jpg"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".png"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".gif"
                && Path.GetExtension(postedFile.FileName).ToLower() != ".jpeg")
            {
                return false;
            }

            //-------------------------------------------
            //  Attempt to read the file and check the first bytes
            //-------------------------------------------
            try
            {
                if (!postedFile.OpenReadStream().CanRead)
                {
                    return false;
                }


                byte[] buffer = new byte[ImageMaximumBytes];
                postedFile.OpenReadStream().Read(buffer, 0, ImageMaximumBytes);
                string content = System.Text.Encoding.UTF8.GetString(buffer);
                if (Regex.IsMatch(content, @"<script|<html|<head|<title|<body|<pre|<table|<a\s+href|<img|<plaintext|<cross\-domain\-policy",
                    RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline))
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

            //-------------------------------------------
            //  Try to instantiate new Bitmap, if .NET will throw exception
            //  we can assume that it's not a valid image
            //-------------------------------------------

            try
            {
                using (var bitmap = new System.Drawing.Bitmap(postedFile.OpenReadStream()))
                {
                }
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                postedFile.OpenReadStream().Position = 0;
            }

            return true;
        } 
        #endregion

    }
}
