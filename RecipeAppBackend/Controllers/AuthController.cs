using Microsoft.AspNetCore.Mvc;
using RecipeAppBackend.Data;
using RecipeAppBackend.Models;
using Microsoft.EntityFrameworkCore;

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
    public async Task<IActionResult> Signup(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return Ok(new { message = "User created successfully!" });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] User loginData)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == loginData.Email && u.Password == loginData.Password);

        if (user == null) return Unauthorized("Invalid email or password");
        return Ok(new { message = $"Welcome back, {user.Name}!" });
    }
}