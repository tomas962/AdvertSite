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
            var messages =
                await _context.UsersHasMessages
                      .Where(m => m.RecipientId == _userManager.GetUserId(User) && m.IsDeleted == 0)
                      .Include(m => m.Messages)
                      .ThenInclude(m => m.UsersHasMessages)
                      .ThenInclude(userMessages => userMessages.Sender)
                      .OrderByDescending(m => m.Messages.DateSent).ToListAsync();

            return View(messages);
        }

        // GET: MEssages/OutBox
        [Authorize(Roles = "Admin,User")]
        [HttpGet]
        public async Task< IActionResult> Outbox()
        {
            var advert_siteContext = await _context.UsersHasMessages
                .Where(m => m.SenderId == _userManager.GetUserId(User) && m.IsDeleted == 0 && m.IsAdminMessage == 0)
                .Include(m => m.Recipient)
                .Include(m => m.Messages)
                .ThenInclude(m => m.UsersHasMessages)
                .ThenInclude(userMessages => userMessages.Recipient)
                .OrderByDescending(m => m.Messages.DateSent).ToListAsync();

            return View(advert_siteContext);
        }

        // GET: Messages/Details/5
        [Authorize(Roles = "Admin,User")]
        public async Task <IActionResult> Details(int? id)
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
        // GET: Messages/CreateAdmin
        [Authorize(Roles = "Admin")]
        public IActionResult CreateAdmin()
        {

            var recipient = GetRecipientUser(Request.Query["recipientId"]);
            var model = new CreateMessageModel { RecipientId = Request.Query["recipientId"], Recipient = recipient };
            return View(model);
        }



        // GET: Messages/Create
        [Authorize(Roles = "Admin,User")]
        public IActionResult Create()
        {
            // Jeigu vartotojas bando rasyti zinute sau
            if (Request.Query["recipientId"].Equals(_userManager.GetUserId(User)))
            {
                return RedirectToAction("Index", "Home");
            }


            var recipient = GetRecipientUser(Request.Query["recipientId"]);
            var model = new CreateMessageModel { RecipientId = Request.Query["recipientId"], Recipient = recipient, Message = new Messages() };
            model.Message.Subject = Request.Query["subject"];
            return View(model);
        }

        // POST: Messages/CreateAdmin
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateAdmin([Bind("Message,RecipientId")] CreateMessageModel model)
        {
            var sender = _context.Users.FirstOrDefaultAsync(user => user.Id == _userManager.GetUserId(User));
            model.UsersHasMessages = new UsersHasMessages { Sender = await sender };

            if (ModelState.IsValid)
            {
                model.Message.DateSent = DateTime.Now;
                _context.Add(model.Message);
                await _context.SaveChangesAsync();

                model.UsersHasMessages.IsAdminMessage = 1;
                model.UsersHasMessages.IsDeleted = 0;
                model.UsersHasMessages.AlreadyRead = 0;
                model.UsersHasMessages.Messages = model.Message;
                model.UsersHasMessages.MessagesId = model.Message.Id;

                IList<ApplicationUser> users = _context.Users.ToList();
                foreach (var user in users)
                {

                    //Data for UsersHasMessages table
                    model.UsersHasMessages.RecipientId = user.Id;

                    _context.Add(model.UsersHasMessages);
                    await _context.SaveChangesAsync();
                }
                return RedirectToAction(nameof(Index));
            }

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

            model.Message.DateSent = DateTime.Now;
            if (ModelState.IsValid)
            {
                _context.Add(model.Message);
                await _context.SaveChangesAsync();

                //Data for UsersHasMessages table
                model.UsersHasMessages.Messages = model.Message;
                model.UsersHasMessages.MessagesId = model.Message.Id;
                model.UsersHasMessages.RecipientId = model.RecipientId;
                model.UsersHasMessages.IsAdminMessage = 0;
                model.UsersHasMessages.IsDeleted = 0;
                model.UsersHasMessages.AlreadyRead = 0;

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
        public async Task<IActionResult> MarkAsRead(int id, string sender_id, string recipient_id)
        {
            var messages = await _context.UsersHasMessages.FindAsync(recipient_id, id, sender_id);
            messages.AlreadyRead = 1;
            _context.UsersHasMessages.Update(messages);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // POST: Messages/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,User")]
        public async Task<IActionResult> Delete(int id, string sender_id, string recipient_id)
        {
            var messages = await _context.UsersHasMessages.FindAsync(recipient_id, id, sender_id);
            messages.IsDeleted = 1;
            _context.UsersHasMessages.Update(messages);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        #region Helper Methods


        public int UpdateUnreadMessageCount()
        {
            int count = _context.UsersHasMessages
                 .Where(m => m.RecipientId == _userManager.GetUserId(User) && m.AlreadyRead == 0 && m.IsDeleted == 0)
                 .Count();

            return count;
        }

        public ApplicationUser GetRecipientUser(String id)
        {
            return _context.Users.FirstOrDefault(user => user.Id.Equals(id));
        }
        #endregion

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
