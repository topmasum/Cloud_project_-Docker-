using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeAppBackend.Data;
using RecipeAppBackend.Models;
using RecipeAppBackend.Dtos; // Added this to use LoginDto

namespace RecipeAppBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;

        public AuthController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> Signup([FromBody] User user)
        {
            // Check if email exists
            if (await _context.Users.AnyAsync(u => u.Email == user.Email))
            {
                return BadRequest("Email is already registered.");
            }

            // In your frontend, make sure you send 'fullName' and 'passwordHash'
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            
            return Ok(new { message = "User created successfully!", userId = user.UserId });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginData) // Updated to use LoginDto
        {
            // We search the database using the clean DTO data
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == loginData.Email && u.PasswordHash == loginData.PasswordHash);

            if (user == null) 
            {
                return Unauthorized("Invalid email or password");
            }

            // Returns the UserID so your Next.js app can save it in localStorage
            return Ok(new { 
                message = $"Welcome back, {user.FullName}!", 
                userId = user.UserId 
            });
        }
    }
}