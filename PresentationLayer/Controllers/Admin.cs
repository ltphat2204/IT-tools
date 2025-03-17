using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    public class Admin : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
