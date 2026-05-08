using Microsoft.EntityFrameworkCore;
using RecipeAppBackend.Models; // Ensure this matches your namespace

namespace RecipeAppBackend.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}