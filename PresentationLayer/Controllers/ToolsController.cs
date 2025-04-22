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
            bool isFavorite = false;
            var user = await userManager.GetUserAsync(User);
            if (user != null)
            {
                isFavorite = await unitOfWork.Users.IsToolFavoriteAsync(user.Id, id);
            }

            var viewModel = new ToolDetailViewModel
            {
                Tool = tool,
                IsFavorite = isFavorite
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
