using BusinessLayer.Entities;
using Microsoft.AspNetCore.Identity;

namespace BusinessLayer.Interfaces
{
    public interface IUserRepository : IGenericRepository<ApplicationUser>
    {
        Task<IdentityResult> RegisterUserAsync(string name, string email, string password);
        Task<ApplicationUser?> GetUserByEmailAsync(string email);
        Task<SignInResult> PasswordSignInAsync(string email, string password, bool isPersistent, bool lockoutOnFailure);
        Task SignOutAsync();
        Task<bool> IsToolFavoriteAsync(string userId, int toolId);
    }
}
