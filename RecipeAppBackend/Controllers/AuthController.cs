using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecipeAppBackend.Data;
using RecipeAppBackend.Models;

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
            if (await _context.Users.AnyAsync(u => u.Email == user.Email))
            {
                return BadRequest("Email is already registered.");
            }

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return Ok(new { message = "User created successfully!", userId = user.UserId });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] User loginData)
        {
            // Successfully matching Email and PasswordHash
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == loginData.Email && u.PasswordHash == loginData.PasswordHash);

            if (user == null) 
            {
                return Unauthorized("Invalid email or password");
            }

            // Fixed: Use FullName instead of Name
            return Ok(new { 
                message = $"Welcome back, {user.FullName}!", 
                userId = user.UserId 
            });
        }
    }
}