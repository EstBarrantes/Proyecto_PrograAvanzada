using System.Linq;
using System.Web.Mvc;
using Pokemon_AP_Project_G3.Data.Repository;
using Pokemon_AP_Project_G3.Data.Models;
using Pokemon_AP_Project_G3.Data;

namespace Pokemon_AP_Project_G3.Controllers
{
    public class ChatController : Controller
    {
        private readonly MessageRepository _messageRepo;
        private readonly PokemonGameEntities _context;

        public ChatController()
        {
            _context = new PokemonGameEntities();
            _messageRepo = new MessageRepository(_context);
        }

        public ActionResult Index()
        {
            var messages = _context.Messages
                .OrderByDescending(m => m.sent_date)
                .Take(20)
                .ToList()
                .Select(m => new Message
                {
                    message_id = m.message_id,
                    sender_id = m.sender_id ?? 0, 
                    receiver_id = m.receiver_id ?? 0,
                    content = m.content,
                    sent_date = m.sent_date ?? System.DateTime.Now
                });

            return PartialView("_ChatPartial", messages);
        }

        [HttpGet]
        public ActionResult Create()
        {
            var users = _context.Users.Select(u => new SelectListItem
            {
                Text = u.username,
                Value = u.username
            }).ToList();

            ViewBag.UserList = users;
            return View();
        }

        [HttpPost]
        public ActionResult Send(string receiverUsername, string content)
        {
            string senderUsername = User.Identity.Name; 

            if (!string.IsNullOrEmpty(content) && !string.IsNullOrEmpty(receiverUsername))
            {
                _messageRepo.AddMessage(senderUsername, receiverUsername, content);
            }

            return RedirectToAction("Index");
        }
    }
}
