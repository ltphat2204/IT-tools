using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models.User;
using DataAccessLayer;
using System.Diagnostics;

namespace PresentationLayer.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Profile(string id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                return NotFound();
            }

            UpdateProfileViewModel profile = new UpdateProfileViewModel
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            };

            return View(profile);
        }
    }
}
