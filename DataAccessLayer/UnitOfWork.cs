using BusinessLayer.Entities;
using BusinessLayer.Interfaces;
using DataAccessLayer.Repositories;
using Microsoft.AspNetCore.Identity;

namespace DataAccessLayer
{
    public class UnitOfWork(ApplicationDbContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager) : IUnitOfWork
    {
        private readonly ApplicationDbContext _context = context;
        public IUserRepository Users { get; private set; } = new UserRepository(context, userManager, signInManager);

        public Task<int> SaveChangesAsync()
        {
            return _context.SaveChangesAsync();
        }
        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
