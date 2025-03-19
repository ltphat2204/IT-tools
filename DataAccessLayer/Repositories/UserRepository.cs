using BusinessLayer.Entities;
using BusinessLayer.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace DataAccessLayer.Repositories
{
    public class UserRepository(ApplicationDbContext context,
                          UserManager<ApplicationUser> userManager,
                          SignInManager<ApplicationUser> signInManager) : GenericRepository<ApplicationUser>(context), IUserRepository
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;

        public async Task<IdentityResult> RegisterUserAsync(string name, string email, string password)
        {
            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                Name = name
            };

            var createAccount = await _userManager.CreateAsync(user, password);
            if (!createAccount.Succeeded)
            {
                throw new Exception("Failed to create user: " +
                    string.Join(", ", createAccount.Errors.Select(e => e.Description)));
            }
            return await _userManager.AddToRoleAsync(user, "User");
        }

        public async Task<ApplicationUser?> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<SignInResult> PasswordSignInAsync(string email, string password, bool isPersistent, bool lockoutOnFailure)
        {
            return await _signInManager.PasswordSignInAsync(email, password, isPersistent, lockoutOnFailure);
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }
    }
}
