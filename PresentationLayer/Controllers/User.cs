using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models.User;
using DataAccessLayer;

namespace PresentationLayer.Controllers
{
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Profile(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Id == id.ToString());
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
