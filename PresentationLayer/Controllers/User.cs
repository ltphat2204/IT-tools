using Microsoft.AspNetCore.Mvc;

namespace PresentationLayer.Controllers
{
    public class User : Controller
    {
        public IActionResult Profile()
        {
            return View();
        }
    }
}
