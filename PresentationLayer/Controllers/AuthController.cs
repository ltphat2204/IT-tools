using System.Diagnostics;
using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models.Auth;

namespace PresentationLayer.Controllers
{
    public class AuthController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public AuthController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var signInResult = await _unitOfWork.Users.PasswordSignInAsync(
                model.Email,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: false);

            Debug.WriteLine(signInResult.Succeeded);
            Debug.WriteLine("ok");
            if (signInResult.Succeeded)
            {
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Thông tin đăng nhập không hợp lệ.");
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            // Sử dụng repository của UnitOfWork để đăng ký người dùng
            var registerResult = await _unitOfWork.Users.RegisterUserAsync(model.Email, model.Password);
            if (registerResult.Succeeded)
            {
                // Sau khi đăng ký thành công, tự động đăng nhập
                await _unitOfWork.Users.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, lockoutOnFailure: false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in registerResult.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _unitOfWork.Users.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
    }
}
