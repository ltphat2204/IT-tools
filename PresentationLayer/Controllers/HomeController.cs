using System.Diagnostics;
using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models;

namespace PresentationLayer.Controllers;

public class HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork) : Controller
{
    public IActionResult Index()
    {
        var tools = unitOfWork.Tools.GetAllWithGroup();
        return View(tools);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
