using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models.User;
using BusinessLayer.Interfaces;
using DataAccessLayer;

namespace PresentationLayer.Controllers
{
    //[Authorize]
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IPhotoService _photoService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserController(ApplicationDbContext context, IPhotoService photoService, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _photoService = photoService;
            _httpContextAccessor = httpContextAccessor;
        }

        public IActionResult Profile()
        {
            var user = _context.Users.FirstOrDefault();

            if (user == null)
                return NotFound();

            var profile = new UpdateProfileViewModel
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                ImageUrl = user.Photo
            };

            return View(profile);
        }

        public async Task<IActionResult> UpdateProfile(UpdateProfileViewModel model, IFormFile? ProfileImage)
        {
            if (!ModelState.IsValid)
                return View("Profile", model);

            var user = _context.Users.FirstOrDefault(u => u.Id == model.Id);
            if (user == null)
                return NotFound();

            user.Name = model.Name;

            if (ProfileImage != null && ProfileImage.Length > 0)
            {
                if (!string.IsNullOrEmpty(user.Photo) 
                    && !user.Photo.Contains("default_profile")
                    && !user.Photo.Contains("admin_profile"))
                {
                    object value = await _photoService.DeletePhotoAsync(user.Photo);
                }

                var uploadResult = await _photoService.AddPhotoAsync(ProfileImage);
                if (uploadResult.Error != null)
                {
                    ModelState.AddModelError("ProfileImage", uploadResult.Error.Message);
                    return View("Profile", model);
                }

                user.Photo = uploadResult.SecureUrl.ToString();
            }

            await _context.SaveChangesAsync();

            return RedirectToAction("Profile");
        }
    }
}
