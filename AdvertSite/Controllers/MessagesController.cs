using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AdvertSite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace AdvertSite.Controllers
{
    [Authorize]
    public class MessagesController : Controller
    {
        private readonly advert_siteContext _context;
        private readonly UserManager<ApplicationUser> _userManager;


        public MessagesController(advert_siteContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Messages
        [HttpGet]
        [Authorize(Roles = "Admin,User")]
        public IActionResult Index()
        {
            return RedirectToAction(nameof(Inbox));
        }

        // GET: Messages/Inbox
        [Authorize(Roles = "Admin,User")]
        [HttpGet]
        public async Task<IActionResult> Inbox()
        {
            var advert_siteContext =
               _context.UsersHasMessages
                   .Where(m =>  m.RecipientId == _userManager.GetUserId(User) && m.Messages.IsDeleted == 0 )
                   .Include(m => m.Messages)
                   .ThenInclude(m => m.UsersHasMessages)
                   .ThenInclude(userMessages => userMessages.Sender);

            return View(await advert_siteContext.ToListAsync());
        }

        // GET: MEssages/OutBox
        [Authorize(Roles = "Admin,User")]
        [HttpGet]
        public async Task<IActionResult> Outbox()
        {
            var advert_siteContext =
               _context.UsersHasMessages
                   .Where(m => m.SenderId == _userManager.GetUserId(User) && m.Messages.IsDeleted == 0)
                   .Include(m => m.Recipient)
                   .Include(m => m.Messages)
                   .ThenInclude(m => m.UsersHasMessages)
                   .ThenInclude(userMessages => userMessages.Recipient);


            return View(await advert_siteContext.ToListAsync());
        }

        // GET: Messages/Details/5
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var messages = await _context.Messages
                .Include(m => m.UsersHasMessages)
                .ThenInclude(m => m.Sender)
                .Include(m => m.UsersHasMessages)
                .ThenInclude(m => m.Recipient)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (messages == null)
            {
                return NotFound();
            }

            return View(messages);
        }

        // GET: Messages/Create
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Create()
        {
            // Jeigu vartotojas bando rasyti zinute sau
            if (Request.Query["recipientId"].Equals(this.User.FindFirstValue(ClaimTypes.NameIdentifier)))
            {
                return RedirectToAction("Index", "Home");
            }


            var recipient = await _context.Users.FirstOrDefaultAsync(user => user.Id == Request.Query["recipientId"]);
            var model = new CreateMessageModel { RecipientId = Request.Query["recipientId"], Recipient = recipient};
            return View(model);
        }

        // POST: Messages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Create([Bind("Message,RecipientId")] CreateMessageModel model)
        {

            var sender = _context.Users.FirstOrDefaultAsync(user => user.Id == _userManager.GetUserId(User));
            model.UsersHasMessages =new UsersHasMessages { Sender = await sender };
            //model.Message = await sender;

            //model.Message.SenderId = model.Message.Sender.Id;

            model.Message.IsDeleted = 0;
            model.Message.AlreadyRead = 0;
            model.Message.DateSent = DateTime.Now;
            if (ModelState.IsValid)
            {
                _context.Add(model.Message);
                await _context.SaveChangesAsync();

                //Data for UsersHasMessages table
                model.UsersHasMessages.Messages = model.Message;
                model.UsersHasMessages.MessagesId = model.Message.Id;
                model.UsersHasMessages.RecipientId = model.RecipientId;

                _context.Add(model.UsersHasMessages);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // POST: Messages/MarkAsRead
        [HttpPost, ActionName("MarkAsRead")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> MarkAsRead(int id)
        {
            var messages = await _context.Messages.FindAsync(id);
            messages.AlreadyRead = 1;
            _context.Messages.Update(messages);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Messages/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Delete(int id)
        {
            var messages = await _context.Messages.FindAsync(id);
            messages.IsDeleted = 1;
            _context.Messages.Update(messages);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public int UpdateUnreadMessageCount()
        {
            int count = _context.UsersHasMessages
                 .Where(m => m.RecipientId == _userManager.GetUserId(User) && m.Messages.AlreadyRead == 0 && m.Messages.IsDeleted == 0)
                 .Count();

            return count;
        }
        public string MyTestMethod()
        {
            return "Test String";
        }

        //// GET: Messages/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var messages = await _context.Messages
        //        .Include(m => m.Sender)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (messages == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(messages);
        //}

        //private bool MessagesExists(int id)
        //{
        //    return _context.Messages.Any(e => e.Id == id);
        //}
    }
}
