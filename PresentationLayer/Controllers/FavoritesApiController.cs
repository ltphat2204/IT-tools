using BusinessLayer.Entities;
using DataAccessLayer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace PresentationLayer.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class FavoritesApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public FavoritesApiController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpPost("toggle")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleFavorite([FromForm] int toolId)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized(new { success = false, message = "User not logged in." });
            }

            var tool = await _context.Tools.FindAsync(toolId);
            if (tool == null)
            {
                return NotFound(new { success = false, message = "Tool not found." });
            }

            var userWithFavorites = await _context.Users
                                                 .Include(u => u.FavoriteTools)
                                                 .FirstOrDefaultAsync(u => u.Id == user.Id);

            if (userWithFavorites == null)
            {
                return NotFound(new { success = false, message = "User data not found." });
            }


            bool isCurrentlyFavorite = userWithFavorites.FavoriteTools.Any(t => t.ToolId == toolId);
            bool newIsFavorite;

            if (isCurrentlyFavorite)
            {
                var toolToRemove = userWithFavorites.FavoriteTools.First(t => t.ToolId == toolId);
                userWithFavorites.FavoriteTools.Remove(toolToRemove);
                newIsFavorite = false;
            }
            else
            {
                userWithFavorites.FavoriteTools.Add(tool);
                newIsFavorite = true;
            }

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { success = true, isFavorite = newIsFavorite });
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new { success = false, message = "Could not update favorite status. Please try again." });
            }
        }
    }
}
