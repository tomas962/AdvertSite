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
        public async Task<IActionResult> Index()
        {
            var advert_siteContext = 
                _context.UsersHasMessages
                .Where(m => m.RecipientId == _userManager.GetUserId(User) && m.Messages.IsDeleted == 0)
                .Include(m => m.Messages)
                .ThenInclude(m => m.Sender);
            return View(await advert_siteContext.ToListAsync());
        }

        // GET: Messages/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var messages = await _context.Messages
                .Include(m => m.Sender)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (messages == null)
            {
                return NotFound();
            }

            return View(messages);
        }

        // GET: Messages/Create
        public async Task<IActionResult> Create()
        {
            var recipient = await _context.Users.FirstOrDefaultAsync(user => user.Id == Request.Query["recipientId"]);
            var model = new CreateMessageModel { RecipientId = Request.Query["recipientId"], Recipient = recipient};
            return View(model);
        }

        // POST: Messages/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Message,RecipientId")] CreateMessageModel model)
        {

            var sender = _context.Users.FirstOrDefaultAsync(user => user.Id == _userManager.GetUserId(User));
            model.Message.Sender = await sender;

            model.Message.SenderId = model.Message.Sender.Id;
            model.Message.IsDeleted = 0;
            model.Message.AlreadyRead = 0;
            model.Message.DateSent = DateTime.Now;
            if (ModelState.IsValid)
            {
                _context.Add(model.Message);
                await _context.SaveChangesAsync();
                _context.Add(new UsersHasMessages { MessagesId = model.Message.Id.Value, RecipientId = model.RecipientId, MessagesSenderId = model.Message.SenderId});
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // POST: Messages/MarkAsRead
        [HttpPost, ActionName("MarkAsRead")]
        [ValidateAntiForgeryToken]
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
        public async Task<IActionResult> Delete(int id)
        {
            var messages = await _context.Messages.FindAsync(id);
            messages.IsDeleted = 1;
            _context.Messages.Update(messages);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        /*
        public void UpdateUnreadMessageCount()
        {
            int count = _context.UsersHasMessages
                 .Select(m => m.RecipientId == _userManager.GetUserId(User))
                 .Count();
            ViewBag.UnreadCount = count;
            
        }
        */

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
