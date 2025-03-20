using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    public class AdminController(IUnitOfWork unitOfWork, ILogger<AuthController> logger) : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Groups()
        {
            var groups = unitOfWork.Groups.GetAll();
            return View(groups);
        }
    }
}
