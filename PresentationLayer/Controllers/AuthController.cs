using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Mvc;
using PresentationLayer.Models.Auth;

namespace PresentationLayer.Controllers
{
    public class AuthController(IUnitOfWork unitOfWork, ILogger<AuthController> logger) : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Deny()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            try
            {
                logger.LogInformation("Login attempt for user: {Email}", model.Email);
                var signInResult = await unitOfWork.Users.PasswordSignInAsync(
                    model.Email,
                    model.Password,
                    model.RememberMe,
                    lockoutOnFailure: false);

                if (signInResult.Succeeded)
                {
                    logger.LogInformation("User {Email} logged in successfully.", model.Email);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    logger.LogWarning("Failed login attempt for user: {Email}", model.Email);
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during login for user: {Email}", model.Email);
                ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again later.");
                return View(model);
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

            try
            {
                logger.LogInformation("Registration attempt for user: {Email}", model.Email);
                var registerResult = await unitOfWork.Users.RegisterUserAsync(model.Name, model.Email, model.Password);
                if (registerResult.Succeeded)
                {
                    logger.LogInformation("New user registered: {Email}", model.Email);
                    await unitOfWork.Users.PasswordSignInAsync(model.Email, model.Password, isPersistent: false, lockoutOnFailure: false);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in registerResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                        logger.LogWarning("Registration error for user {Email}: {Error}", model.Email, error.Description);
                    }
                }
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during registration for user: {Email}", model.Email);
                ModelState.AddModelError(string.Empty, "An unexpected error occurred. Please try again later.");
                return View(model);
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            try
            {
                logger.LogInformation("User logging out.");
                await unitOfWork.Users.SignOutAsync();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "An error occurred during logout.");
            }
            return RedirectToAction("Index", "Home");
        }
    }
}
