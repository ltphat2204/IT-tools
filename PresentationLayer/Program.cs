using System.Diagnostics;
using Azure.Identity;
using BusinessLayer.Entities;
using BusinessLayer.Helpers;
using BusinessLayer.Interfaces;
using DataAccessLayer;
using DataAccessLayer.Repositories;
using DataAccessLayer.Seeds;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var connectionString = string.Empty;
if (builder.Environment.IsDevelopment())
{
    builder.Configuration.AddEnvironmentVariables().AddJsonFile("appsettings.Development.json");
}
else
{
    builder.Configuration.AddEnvironmentVariables().AddJsonFile("appsettings.json").AddUserSecrets<Program>();
}    

connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
Debug.WriteLine("oke");
Debug.WriteLine(connectionString);
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

builder.Services.Configure<IdentityOptions>(options =>
{
    // Password settings.
    options.Password.RequiredLength = 6;
    options.Password.RequiredUniqueChars = 1;

    // Lockout settings.
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;

    // User settings.
    options.User.AllowedUserNameCharacters =
    "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@";

    // SignIn settings.
    options.SignIn.RequireConfirmedEmail = false;
    options.SignIn.RequireConfirmedPhoneNumber = false;
});

builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);

    // Login settings
    options.LoginPath = "/auth/login";
    options.AccessDeniedPath = "/auth/deny";
    options.LogoutPath = "/auth/logout";
    options.SlidingExpiration = true;
});

builder.Services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddTransient<IUnitOfWork, UnitOfWork>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

if (args.Length > 0 && args[0].Equals("seed", StringComparison.OrdinalIgnoreCase))
{
    using var scope = app.Services.CreateScope();
    var serviceProvider = scope.ServiceProvider;
    await SeedRolesAndAdmin.SeedRolesAndAdminAsync(serviceProvider);
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
