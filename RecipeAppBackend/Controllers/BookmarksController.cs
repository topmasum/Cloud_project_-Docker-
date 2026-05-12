using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeAppBackend.Data;
using RecipeAppBackend.Models;
using RecipeAppBackend.Dtos;

namespace RecipeAppBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookmarksController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BookmarksController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Add([FromBody] BookmarkRequest req)
        {
            // Check if the bookmark already exists to prevent duplicates
            var exists = await _context.Bookmarks.AnyAsync(b => 
                b.UserId == req.UserId && b.RecipeId == req.RecipeId);

            if (exists)
            {
                return BadRequest("Already bookmarked");
            }

            _context.Bookmarks.Add(new Bookmark 
            { 
                UserId = req.UserId, 
                RecipeId = req.RecipeId 
            });

            await _context.SaveChangesAsync();
            return Ok(new { message = "Recipe bookmarked successfully!" });
        }

        [HttpGet("library/{userId}")]
        public async Task<IActionResult> GetLibrary(int userId)
        {
            // 1. Get recipes created by this specific user
            var created = await _context.Recipes
                .Where(r => r.CreatedBy == userId)
                .ToListAsync();

            // 2. Find the IDs of recipes this user has bookmarked
            var bookmarkedIds = await _context.Bookmarks
                .Where(b => b.UserId == userId)
                .Select(b => b.RecipeId)
                .ToListAsync();

            // 3. Get the actual recipe details for those bookmarked IDs
            var bookmarked = await _context.Recipes
                .Where(r => bookmarkedIds.Contains(r.RecipeId))
                .ToListAsync();

            // Combine both lists and remove duplicates if a user bookmarked their own recipe
            var fullLibrary = created.Union(bookmarked).ToList();
            
            return Ok(fullLibrary);
        }
    }
}