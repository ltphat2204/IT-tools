﻿using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models.User;
using BusinessLayer.Interfaces;
using DataAccessLayer;
using BusinessLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Diagnostics;

namespace PresentationLayer.Controllers
{
    [Authorize]
    public class UserController(ApplicationDbContext context, IPhotoService photoService, IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager) : Controller
    {
        public async Task<IActionResult> MyFavorites()
        {
            var user = await userManager.GetUserAsync(User);

            var userWithFavorites = await context.Users
                                          .Where(u => u.Id == user.Id)
                                          .Include(u => u.FavoriteTools)
                                            .ThenInclude(t => t.Group)
                                          .FirstOrDefaultAsync();

            if (userWithFavorites == null)
            {
                return NotFound("User data could not be loaded.");
            }

            return View(userWithFavorites.FavoriteTools ?? []);
        }

        public async Task<IActionResult> Profile()
        {
            var user = await userManager.GetUserAsync(User);

            if (user == null)
                return NotFound();

            var roles = await userManager.GetRolesAsync(user);

            var profile = new UpdateProfileViewModel
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                ImageUrl = user.Photo,
                PremiumRequest = user.PremiumRequest,
                IsPremium = roles.Contains("Premium")
            };

            return View(profile);
        }

        [HttpPost]
        public async Task<IActionResult> RequestPremium()
        {
            var user = await userManager.GetUserAsync(User);

            if (user == null)
            {
                return NotFound();
            }

            if (user.PremiumRequest != null && user.PremiumRequest == "Pending")
            {
                return BadRequest("You already have a pending premium request.");
            }

            user.PremiumRequest = "Pending";
            await userManager.UpdateAsync(user);

            return Ok("Premium request has been submitted successfully.");
        }

        public async Task<IActionResult> UpdateProfile(UpdateProfileViewModel model, IFormFile? ProfileImage)
        {
            if (!ModelState.IsValid)
                return View("Profile", model);

            var user = context.Users.FirstOrDefault(u => u.Id == model.Id);
            if (user == null)
                return NotFound();

            user.Name = model.Name;

            if (ProfileImage != null && ProfileImage.Length > 0)
            {
                if (!string.IsNullOrEmpty(user.Photo) 
                    && !user.Photo.Contains("default_profile")
                    && !user.Photo.Contains("admin_profile"))
                {
                    object value = await photoService.DeletePhotoAsync(user.Photo);
                }

                var uploadResult = await photoService.AddPhotoAsync(ProfileImage);
                if (uploadResult.Error != null)
                {
                    ModelState.AddModelError("ProfileImage", uploadResult.Error.Message);
                    return View("Profile", model);
                }

                user.Photo = uploadResult.SecureUrl.ToString();
            }

            await context.SaveChangesAsync();

            return RedirectToAction("Profile");
        }
    }
}
