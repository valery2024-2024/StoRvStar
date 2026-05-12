using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StoRvStar.Data;
using StoRvStar.Models.Entities;

namespace StoRvStar.Controllers
{
    [Authorize(Roles = "Admin,Manager")]
    public class UserController : Controller
    {
        private readonly AppDbContext _context;

        public UserController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var users = _context.Users.ToList();
            return View(users);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(User user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            user.Username = user.Name;
            user.Role = "User";

            _context.Users.Add(user);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}
