using BusinessLayer.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccessLayer.Seeds
{
    public class SeedRolesAndAdmin
    {
        public static async Task SeedRolesAndAdminAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roles = ["User", "Premium", "Admin"];

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    var roleResult = await roleManager.CreateAsync(new IdentityRole(role));
                    if (!roleResult.Succeeded)
                    {
                        throw new Exception("Failed to create role " + role + ": " +
                            string.Join(", ", roleResult.Errors.Select(e => e.Description)));
                    }
                }
            }

            var adminEmail = "admin@it-tools.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail
                };

                var createAdmin = await userManager.CreateAsync(adminUser, "Admin123!");
                if (createAdmin.Succeeded)
                {
                    var addToRoleResult = await userManager.AddToRoleAsync(adminUser, "Admin");
                    if (!addToRoleResult.Succeeded)
                    {
                        throw new Exception("Failed to add admin user to Admin role: " +
                            string.Join(", ", addToRoleResult.Errors.Select(e => e.Description)));
                    }
                }
                else
                {
                    throw new Exception("Failed to create admin user: " +
                        string.Join(", ", createAdmin.Errors.Select(e => e.Description)));
                }
            }
        }
    }
}
