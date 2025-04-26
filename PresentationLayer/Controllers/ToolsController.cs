using BusinessLayer.Entities;
using BusinessLayer.Interfaces;
using DataAccessLayer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models.Tool;

namespace PresentationLayer.Controllers
{
    public class ToolsController(ILogger<HomeController> logger, IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager) : Controller
    {
        public async Task<IActionResult> Index(int id)
        {
            var tool = unitOfWork.Tools.GetById(id);
            if (tool == null || tool.IsDisabled)
            {
                return NotFound();
            }

            bool isFavorite = false;
            var user = await userManager.GetUserAsync(User);

            bool isBlocked = tool.IsPremium;
            if (user != null)
            {
                isFavorite = await unitOfWork.Users.IsToolFavoriteAsync(user.Id, id);
                var roles = await userManager.GetRolesAsync(user);
                isBlocked = tool.IsPremium && !(roles.Contains("Premium") || roles.Contains("Admin"));
            }

            var viewModel = new ToolDetailViewModel
            {
                Tool = tool,
                IsFavorite = isFavorite,
                IsBlock = isBlocked
            };

            return View(viewModel);
        }

        public IActionResult Render(int id)
        {
            var tool = unitOfWork.Tools.GetById(id);
            return View(tool);
        }
    }
}
