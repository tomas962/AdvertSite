using AdvertSite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Threading.Tasks;

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
            if (pictureInfo == null)
                return NotFound();

            string path = Path.GetFullPath("UserPictures\\" + pictureInfo.FileName);

            if (!System.IO.File.Exists(path))
                return NotFound();

            return PhysicalFile(path, pictureInfo.ContentType);
        }
    }
}
