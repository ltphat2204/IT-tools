using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    public class ToolsController(ILogger<HomeController> logger, IUnitOfWork unitOfWork) : Controller
    {
        public IActionResult Index(int id)
        {
            var tool = unitOfWork.Tools.GetById(id);
            return View(tool);
        }

        public IActionResult Render(int id)
        {
            var tool = unitOfWork.Tools.GetById(id);
            return View(tool);
        }
    }
}
